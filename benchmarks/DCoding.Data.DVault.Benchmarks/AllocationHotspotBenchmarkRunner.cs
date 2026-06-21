using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class AllocationHotspotBenchmarkRunner {
  private const int CanonicalizationInputCount = 1000;
  private const int DigestInputCount = 1000;
  private const int CustomerHubCount = 100;
  private const int OrderProductPairCount = 100;
  private const int ReplayCustomerCount = 100;
  private const int ReplaySeededHistoryStateCount = 20;
  private const int ReplayChunkSize = 25;
  private const string CanonicalizationWorkloadName = "stable-hash-canonicalization";
  private const string DigestWorkloadName = "stable-hash-digest-generation";
  private const string CustomerHubOnlyWorkloadName = "customer-profile-hub-only-save-prep";
  private const string OrderProductLinkWorkloadName = "order-product-link-bearing-save-prep";
  private const string SatelliteUnchangedReplayWorkloadName = "satellite-unchanged-replay-filter";
  private const string SatelliteChangedReplayWorkloadName = "satellite-changed-replay-filter";
  private static readonly DateTimeOffset BaseTimestamp = new(2026, 6, 21, 8, 0, 0, TimeSpan.Zero);

  public static async Task RunAsync(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability,
      IReadOnlyList<BenchmarkProviderAvailability> optionalProviders,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(options);
    ArgumentNullException.ThrowIfNull(postgresAvailability);
    ArgumentNullException.ThrowIfNull(optionalProviders);

    if (options.ProviderFilter is not BenchmarkProviderFilters.All and not BenchmarkProviderFilters.Sqlite) {
      throw new ArgumentException("--allocation-hotspots supports the required SQLite baseline only.");
    }

    if (options.EffectiveHashKeyVariants.Count != 1 ||
        options.EffectiveHashKeyVariants[0] != BenchmarkHashKeyVariant.Default) {
      throw new ArgumentException("--allocation-hotspots must run on the default sha256-v1 HexString hash-key baseline.");
    }

    Console.WriteLine("DVault allocation hotspot profile");
    Console.WriteLine("Required provider: " + BenchmarkArtifacts.RequiredProviderName);
    Console.WriteLine("Hash key variants: " + BenchmarkHashKeyVariant.Default.Label);
    Console.WriteLine("Measured boundary: SQLite setup and caller-owned HashDiff creation run outside profiled save calls.");
    Console.WriteLine();

    var workloads = CreateWorkloads(options);
    var results = new List<AllocationHotspotWorkloadResult>();
    foreach (var workload in workloads) {
      Console.WriteLine("Profiling " + workload.WorkloadName + "...");
      results.Add(await ExecuteWorkloadAsync(workload, options, cancellationToken).ConfigureAwait(false));
    }

    var summaries = results
        .Select(result => BenchmarkSummary.Create(
            result.Workload.WorkloadName,
            BenchmarkArtifacts.RequiredProviderName,
            result.Workload.BaselineName,
            "allocation-hotspot-profile",
            result.Workload.DatasetSize,
            result.Workload.ChangeRatio,
            result.IterationMeasurements.Select(measurement => measurement.Elapsed).ToArray(),
            result.IterationMeasurements.Select(measurement => measurement.AllocatedBytes).ToArray(),
            result.PersistedOutcome,
            result.Workload.ExecutionDetail))
        .ToArray();
    var rankedHotspots = AllocationHotspotArtifacts.CreateRankedHotspots(results);

    Console.Write(BenchmarkArtifacts.CreateMarkdownTable(summaries));
    Console.WriteLine();
    Console.WriteLine("Recorded " + summaries.Length.ToString(CultureInfo.InvariantCulture) + " allocation hotspot benchmark rows.");
    Console.WriteLine("Recorded " + rankedHotspots.Count.ToString(CultureInfo.InvariantCulture) + " ranked DVault-owned hotspot rows.");

    if (options.ArtifactOutputDirectory is not null) {
      var context = BenchmarkRunContext.Create(options, postgresAvailability, optionalProviders);
      var artifactPaths = await BenchmarkArtifacts
          .WriteAsync(options.ArtifactOutputDirectory, context, summaries, cancellationToken)
          .ConfigureAwait(false);
      var hotspotPaths = await AllocationHotspotArtifacts
          .WriteAsync(
              options.ArtifactOutputDirectory,
              context,
              results.Select(result => result.ToSummary()).ToArray(),
              rankedHotspots,
              cancellationToken)
          .ConfigureAwait(false);

      Console.WriteLine("Wrote benchmark artifacts:");
      Console.WriteLine("  " + artifactPaths.MarkdownPath);
      Console.WriteLine("  " + artifactPaths.CsvPath);
      Console.WriteLine("  " + artifactPaths.JsonPath);
      Console.WriteLine("  " + hotspotPaths.MarkdownPath);
      Console.WriteLine("  " + hotspotPaths.CsvPath);
      Console.WriteLine("  " + hotspotPaths.JsonPath);
    }
  }

  private static IReadOnlyList<AllocationHotspotWorkload> CreateWorkloads(BenchmarkOptions options) {
    var canonicalizationFields = CreateCanonicalizationFields();
    var digestInputs = CreateDigestInputs(canonicalizationFields);

    return [
        new AllocationHotspotWorkload(
            CanonicalizationWorkloadName,
            "dvault-allocation-hotspots/stable-hash-canonicalization",
            CanonicalizationInputCount.ToString(CultureInfo.InvariantCulture) + " structured customer profile field sets",
            "sha256-v1 HexString canonical text only",
            "scenario=stable-hash-canonicalization; provider=SQLite local temporary files; baseline=sha256-v1-hex; surface=DefaultStableHashNormalizer",
            (iteration, collectSamples, token) => ExecuteCanonicalizationWorkloadAsync(
                canonicalizationFields,
                iteration,
                collectSamples,
                token)),
        new AllocationHotspotWorkload(
            DigestWorkloadName,
            "dvault-allocation-hotspots/stable-hash-digest-generation",
            DigestInputCount.ToString(CultureInfo.InvariantCulture) + " pre-normalized customer profile payloads",
            "sha256-v1 HexString digest generation only",
            "scenario=stable-hash-digest-generation; provider=SQLite local temporary files; baseline=sha256-v1-hex; surface=BuiltInStableHashService",
            (iteration, collectSamples, token) => ExecuteDigestWorkloadAsync(
                digestInputs,
                iteration,
                collectSamples,
                token)),
        new AllocationHotspotWorkload(
            CustomerHubOnlyWorkloadName,
            "dvault-allocation-hotspots/customer-profile-hub-only",
            CustomerHubCount.ToString(CultureInfo.InvariantCulture) + " customer hub save operations",
            "hub-only customer-profile save shape",
            "scenario=customer-profile-hub-only-save-prep; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString",
            (iteration, collectSamples, token) => ExecuteCustomerHubOnlySaveWorkloadAsync(
                options.LoadTimestampStorage,
                iteration,
                collectSamples,
                token)),
        new AllocationHotspotWorkload(
            OrderProductLinkWorkloadName,
            "dvault-allocation-hotspots/order-product-link-bearing",
            OrderProductPairCount.ToString(CultureInfo.InvariantCulture) + " order/product hub pairs and order-product links",
            "link-bearing order-product save shape",
            "scenario=order-product-link-bearing-save-prep; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString",
            (iteration, collectSamples, token) => ExecuteOrderProductLinkSaveWorkloadAsync(
                options.LoadTimestampStorage,
                iteration,
                collectSamples,
                token)),
        new AllocationHotspotWorkload(
            SatelliteUnchangedReplayWorkloadName,
            "dvault-allocation-hotspots/satellite-unchanged-replay",
            ReplayCustomerCount.ToString(CultureInfo.InvariantCulture) + " customers, " + ReplaySeededHistoryStateCount.ToString(CultureInfo.InvariantCulture) + " existing profile states each",
            "unchanged satellite replay across " + (ReplayCustomerCount / ReplayChunkSize).ToString(CultureInfo.InvariantCulture) + " retained-state chunks",
            "scenario=satellite-unchanged-replay-filter; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString; callerHashDiffGeneration=outside-profile",
            (iteration, collectSamples, token) => ExecuteSatelliteReplayWorkloadAsync(
                options.LoadTimestampStorage,
                changedReplay: false,
                iteration,
                collectSamples,
                token)),
        new AllocationHotspotWorkload(
            SatelliteChangedReplayWorkloadName,
            "dvault-allocation-hotspots/satellite-changed-replay",
            ReplayCustomerCount.ToString(CultureInfo.InvariantCulture) + " customers, " + ReplaySeededHistoryStateCount.ToString(CultureInfo.InvariantCulture) + " existing profile states each",
            "changed satellite replay across " + (ReplayCustomerCount / ReplayChunkSize).ToString(CultureInfo.InvariantCulture) + " retained-state chunks",
            "scenario=satellite-changed-replay-filter; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString; callerHashDiffGeneration=outside-profile",
            (iteration, collectSamples, token) => ExecuteSatelliteReplayWorkloadAsync(
                options.LoadTimestampStorage,
                changedReplay: true,
                iteration,
                collectSamples,
                token)),
    ];
  }

  private static async Task<AllocationHotspotWorkloadResult> ExecuteWorkloadAsync(
      AllocationHotspotWorkload workload,
      BenchmarkOptions options,
      CancellationToken cancellationToken) {
    for (var iteration = 0; iteration < options.WarmupIterations; iteration++) {
      await workload.ExecuteIterationAsync(iteration, false, cancellationToken).ConfigureAwait(false);
    }

    var iterationMeasurements = new List<BenchmarkMeasurement>();
    var samples = new List<DataVaultAllocationProfilerSample>();
    var persistedOutcome = string.Empty;
    for (var iteration = 0; iteration < options.Iterations; iteration++) {
      var result = await workload.ExecuteIterationAsync(iteration, true, cancellationToken).ConfigureAwait(false);
      iterationMeasurements.Add(result.Measurement);
      samples.AddRange(result.Samples);
      persistedOutcome = result.PersistedOutcome;
    }

    return new AllocationHotspotWorkloadResult(
        workload with { IterationCount = options.Iterations },
        iterationMeasurements,
        samples,
        persistedOutcome);
  }

  private static async Task<AllocationHotspotIterationResult> ExecuteProfiledActionAsync(
      string workloadName,
      int iteration,
      bool collectSamples,
      Func<CancellationToken, Task<string>> operation,
      CancellationToken cancellationToken) {
    if (collectSamples) {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      GC.Collect();
    }

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    string persistedOutcome;
    DataVaultAllocationProfilerSession? session = collectSamples
        ? DataVaultAllocationProfiler.StartSession(workloadName, iteration)
        : null;
    using (session) {
      persistedOutcome = await operation(cancellationToken).ConfigureAwait(false);
    }

    stopwatch.Stop();
    var allocatedBytes = Math.Max(0, GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore);

    return new AllocationHotspotIterationResult(
        new BenchmarkMeasurement(stopwatch.Elapsed, allocatedBytes),
        session?.Samples.ToArray() ?? [],
        persistedOutcome);
  }

  private static Task<AllocationHotspotIterationResult> ExecuteCanonicalizationWorkloadAsync(
      IReadOnlyList<IReadOnlyList<KeyValuePair<string, object?>>> fields,
      int iteration,
      bool collectSamples,
      CancellationToken cancellationToken) {
    return ExecuteProfiledActionAsync(
        CanonicalizationWorkloadName,
        iteration,
        collectSamples,
        token => ExecuteCanonicalizationActionAsync(fields, token),
        cancellationToken);
  }

  private static Task<AllocationHotspotIterationResult> ExecuteDigestWorkloadAsync(
      IReadOnlyList<string> normalizedInputs,
      int iteration,
      bool collectSamples,
      CancellationToken cancellationToken) {
    return ExecuteProfiledActionAsync(
        DigestWorkloadName,
        iteration,
        collectSamples,
        token => ExecuteDigestActionAsync(normalizedInputs, token),
        cancellationToken);
  }

  private static async Task<string> ExecuteCanonicalizationActionAsync(
      IReadOnlyList<IReadOnlyList<KeyValuePair<string, object?>>> fields,
      CancellationToken cancellationToken) {
    using var provider = CreateServiceProvider();
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var totalLength = 0;

    foreach (var fieldSet in fields) {
      cancellationToken.ThrowIfCancellationRequested();
      totalLength += normalizer.NormalizeFields(fieldSet).Length;
    }

    await Task.CompletedTask.ConfigureAwait(false);
    return fields.Count.ToString(CultureInfo.InvariantCulture) +
        " stable-hash field sets normalized; total canonical characters=" +
        totalLength.ToString(CultureInfo.InvariantCulture);
  }

  private static async Task<string> ExecuteDigestActionAsync(
      IReadOnlyList<string> normalizedInputs,
      CancellationToken cancellationToken) {
    using var provider = CreateServiceProvider();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var digestLength = 0;

    foreach (var normalizedInput in normalizedInputs) {
      cancellationToken.ThrowIfCancellationRequested();
      digestLength += hashService.ComputeHash(normalizedInput).Value.Length;
    }

    await Task.CompletedTask.ConfigureAwait(false);
    return normalizedInputs.Count.ToString(CultureInfo.InvariantCulture) +
        " normalized payloads hashed with " +
        hashService.AlgorithmId +
        "; total digest characters=" +
        digestLength.ToString(CultureInfo.InvariantCulture);
  }

  private static async Task<AllocationHotspotIterationResult> ExecuteCustomerHubOnlySaveWorkloadAsync(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      int iteration,
      bool collectSamples,
      CancellationToken cancellationToken) {
    using var database = BenchmarkDatabaseProviders.Sqlite.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileAllocationContext>();
    var providerCapabilities = BenchmarkDatabaseProviders.Sqlite.GetProviderCapabilities(
        loadTimestampStorage,
        BenchmarkHashKeyVariant.Default);
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var request = new DataVaultSaveRequest(
        BaseTimestamp,
        "allocation-hotspot-customer-hub",
        Enumerable.Range(0, CustomerHubCount)
            .Select(customerIndex => new DataVaultHubSaveOperation(
                ScenarioContracts.CustomerHub,
                [new("Customer Id", CreateCustomerBusinessKey(customerIndex))]))
            .ToArray(),
        []);

    try {
      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      AllocationHotspotIterationResult result;
      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        result = await ExecuteProfiledActionAsync(
            CustomerHubOnlyWorkloadName,
            iteration,
            collectSamples,
            async token => {
              await saveService.SaveAsync(context, request, token).ConfigureAwait(false);
              return "customer hub save completed";
            },
            cancellationToken).ConfigureAwait(false);
      }

      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        var persistedRows = await context.Set<Dictionary<string, object>>("HubCustomer")
            .AsNoTracking()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
        return result with {
          PersistedOutcome = persistedRows.ToString(CultureInfo.InvariantCulture) + " customer hub rows persisted from hub-only save shape",
        };
      }
    }
    finally {
      await using var cleanupContext = new CustomerProfileAllocationContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task<AllocationHotspotIterationResult> ExecuteOrderProductLinkSaveWorkloadAsync(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      int iteration,
      bool collectSamples,
      CancellationToken cancellationToken) {
    using var database = BenchmarkDatabaseProviders.Sqlite.CreateDatabase();
    var options = database.CreateOptions<OrderProductAllocationContext>();
    var providerCapabilities = BenchmarkDatabaseProviders.Sqlite.GetProviderCapabilities(
        loadTimestampStorage,
        BenchmarkHashKeyVariant.Default);
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var hubOperations = new List<DataVaultHubSaveOperation>();
    var linkOperations = new List<DataVaultLinkSaveOperation>();

    for (var index = 0; index < OrderProductPairCount; index++) {
      var orderBusinessKey = CreateOrderBusinessKey(index);
      var productBusinessKey = CreateProductBusinessKey(index);
      var orderHashKey = ComputeHash(normalizer, hashService, [new("Order Id", orderBusinessKey)]);
      var productHashKey = ComputeHash(normalizer, hashService, [new("Sku", productBusinessKey)]);
      hubOperations.Add(new DataVaultHubSaveOperation(ScenarioContracts.OrderHub, [new("Order Id", orderBusinessKey)]));
      hubOperations.Add(new DataVaultHubSaveOperation(ScenarioContracts.ProductHub, [new("Sku", productBusinessKey)]));
      linkOperations.Add(new DataVaultLinkSaveOperation(
          ScenarioContracts.OrderProductLink,
          [new("Order", orderHashKey), new("Product", productHashKey)]));
    }

    var request = new DataVaultSaveRequest(
        BaseTimestamp.AddMinutes(1),
        "allocation-hotspot-order-product-link",
        hubOperations,
        linkOperations);

    try {
      await using (var context = new OrderProductAllocationContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      AllocationHotspotIterationResult result;
      await using (var context = new OrderProductAllocationContext(options, providerCapabilities)) {
        result = await ExecuteProfiledActionAsync(
            OrderProductLinkWorkloadName,
            iteration,
            collectSamples,
            async token => {
              await saveService.SaveAsync(context, request, token).ConfigureAwait(false);
              return "order-product link-bearing save completed";
            },
            cancellationToken).ConfigureAwait(false);
      }

      await using (var context = new OrderProductAllocationContext(options, providerCapabilities)) {
        var linkRows = await context.Set<Dictionary<string, object>>("LinkOrderProduct")
            .AsNoTracking()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
        return result with {
          PersistedOutcome = linkRows.ToString(CultureInfo.InvariantCulture) + " order-product link rows persisted from link-bearing save shape",
        };
      }
    }
    finally {
      await using var cleanupContext = new OrderProductAllocationContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task<AllocationHotspotIterationResult> ExecuteSatelliteReplayWorkloadAsync(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      bool changedReplay,
      int iteration,
      bool collectSamples,
      CancellationToken cancellationToken) {
    using var database = BenchmarkDatabaseProviders.Sqlite.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileAllocationContext>();
    var providerCapabilities = BenchmarkDatabaseProviders.Sqlite.GetProviderCapabilities(
        loadTimestampStorage,
        BenchmarkHashKeyVariant.Default);
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    Dictionary<int, string> customerHashKeys;

    try {
      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
        customerHashKeys = await SeedReplayHistoryAsync(context, saveService, cancellationToken).ConfigureAwait(false);
      }

      var measuredRequest = CreateReplayChunkedRequest(changedReplay, customerHashKeys);
      var workloadName = changedReplay ? SatelliteChangedReplayWorkloadName : SatelliteUnchangedReplayWorkloadName;
      AllocationHotspotIterationResult result;
      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        result = await ExecuteProfiledActionAsync(
            workloadName,
            iteration,
            collectSamples,
            async token => {
              await saveService.SaveAsync(context, measuredRequest, token).ConfigureAwait(false);
              return "satellite replay save completed";
            },
            cancellationToken).ConfigureAwait(false);
      }

      await using (var context = new CustomerProfileAllocationContext(options, providerCapabilities)) {
        var persistedRows = await context.Set<Dictionary<string, object>>("SatCustomerProfile")
            .AsNoTracking()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
        var replayLabel = changedReplay ? "changed replay" : "unchanged replay";
        return result with {
          PersistedOutcome = persistedRows.ToString(CultureInfo.InvariantCulture) + " profile satellite rows after " + replayLabel + " latest lookup",
        };
      }
    }
    finally {
      await using var cleanupContext = new CustomerProfileAllocationContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task<Dictionary<int, string>> SeedReplayHistoryAsync(
      DbContext context,
      IDataVaultSaveService saveService,
      CancellationToken cancellationToken) {
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            BaseTimestamp,
            "allocation-hotspot-replay-seed",
            Enumerable.Range(0, ReplayCustomerCount)
                .Select(customerIndex => new DataVaultHubSaveOperation(
                    ScenarioContracts.CustomerHub,
                    [new("Customer Id", CreateCustomerBusinessKey(customerIndex))]))
                .ToArray(),
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKeys = hubResult.SavedRecords
        .Select((record, customerIndex) => new {
          CustomerIndex = customerIndex,
          record.HashKey,
        })
        .ToDictionary(value => value.CustomerIndex, value => value.HashKey);

    foreach (var historyChunkStart in Enumerable.Range(0, ReplaySeededHistoryStateCount).Chunk(5).Select(chunk => chunk[0])) {
      var requests = Enumerable.Range(historyChunkStart, Math.Min(5, ReplaySeededHistoryStateCount - historyChunkStart))
          .Select(historyIndex => new DataVaultSaveRequest(
              BaseTimestamp.AddMinutes(historyIndex + 1),
              "allocation-hotspot-replay-seed",
              [],
              [],
              Enumerable.Range(0, ReplayCustomerCount)
                  .Select(customerIndex => CreateReplaySatelliteOperation(
                      customerIndex,
                      historyIndex,
                      customerHashKeys[customerIndex],
                      changedReplay: false))
                  .ToArray()))
          .ToArray();

      await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(requests),
          cancellationToken).ConfigureAwait(false);
    }

    return customerHashKeys;
  }

  private static DataVaultChunkedSaveRequest CreateReplayChunkedRequest(
      bool changedReplay,
      IReadOnlyDictionary<int, string> customerHashKeys) {
    var historyIndex = ReplaySeededHistoryStateCount - 1;
    var operations = Enumerable.Range(0, ReplayCustomerCount)
        .Select(customerIndex => CreateReplaySatelliteOperation(
            customerIndex,
            historyIndex,
            customerHashKeys[customerIndex],
            changedReplay))
        .ToArray();
    var chunks = operations
        .Chunk(ReplayChunkSize)
        .Select((chunk, chunkIndex) => new DataVaultSaveChunk([
            new DataVaultSaveRequest(
                BaseTimestamp.AddMinutes(ReplaySeededHistoryStateCount + 1 + chunkIndex),
                changedReplay ? "allocation-hotspot-replay-changed" : "allocation-hotspot-replay-unchanged",
                [],
                [],
                chunk),
        ]))
        .ToArray();

    return new DataVaultChunkedSaveRequest(chunks);
  }

  private static DataVaultSatelliteSaveOperation CreateReplaySatelliteOperation(
      int customerIndex,
      int historyIndex,
      string customerHashKey,
      bool changedReplay) {
    var status = changedReplay
        ? "changed"
        : "state-" + historyIndex.ToString("00", CultureInfo.InvariantCulture);

    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", "Customer " + customerIndex.ToString("0000", CultureInfo.InvariantCulture)),
            new("customer_status", status),
        ],
        changedReplay
            ? "profile-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture) + "-changed"
            : CreateSeedHashDiff(customerIndex, historyIndex));
  }

  private static IReadOnlyList<IReadOnlyList<KeyValuePair<string, object?>>> CreateCanonicalizationFields() {
    return Enumerable.Range(0, CanonicalizationInputCount)
        .Select(index => (IReadOnlyList<KeyValuePair<string, object?>>)[
            new("customer_id", CreateCustomerBusinessKey(index)),
            new("customer_name", "Customer " + index.ToString("0000", CultureInfo.InvariantCulture)),
            new("customer_status", index % 3 == 0 ? "active" : "prospect"),
            new("changed_at", BaseTimestamp.AddMinutes(index).UtcDateTime),
        ])
        .ToArray();
  }

  private static IReadOnlyList<string> CreateDigestInputs(
      IReadOnlyList<IReadOnlyList<KeyValuePair<string, object?>>> canonicalizationFields) {
    using var provider = CreateServiceProvider();
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    return canonicalizationFields
        .Take(DigestInputCount)
        .Select(fields => normalizer.NormalizeFields(fields))
        .ToArray();
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(
        services,
        DataVaultBenchmarkStrategy.ProviderNeutralFallback,
        BenchmarkHashKeyVariant.Default);
    return services.BuildServiceProvider(validateScopes: true);
  }

  private static string ComputeHash(
      IStableHashNormalizer normalizer,
      IStableHashService hashService,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizedFields = normalizer.NormalizeFields(
        fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return hashService.ComputeHash(normalizedFields).Value;
  }

  private static string CreateCustomerBusinessKey(int customerIndex) {
    return "C-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture);
  }

  private static string CreateOrderBusinessKey(int orderIndex) {
    return "O-" + orderIndex.ToString("0000", CultureInfo.InvariantCulture);
  }

  private static string CreateProductBusinessKey(int productIndex) {
    return "SKU-" + productIndex.ToString("0000", CultureInfo.InvariantCulture);
  }

  private static string CreateSeedHashDiff(int customerIndex, int historyIndex) {
    return "profile-" +
        customerIndex.ToString("0000", CultureInfo.InvariantCulture) +
        "-state-" +
        historyIndex.ToString("00", CultureInfo.InvariantCulture);
  }

  private sealed class CustomerProfileAllocationContext(
      DbContextOptions<CustomerProfileAllocationContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), ProviderCapabilities);
    }
  }

  private sealed class OrderProductAllocationContext(
      DbContextOptions<OrderProductAllocationContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateOrderProductDataVaultModel(), ProviderCapabilities);
    }
  }
}

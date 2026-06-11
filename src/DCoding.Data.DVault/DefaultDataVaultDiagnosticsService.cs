using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultDiagnosticsService : IDataVaultDiagnosticsService, IDataVaultReadDiagnosticsService {
  private static readonly DataVaultSaveStrategyDiagnostics NotEvaluatedStrategy = new(
      DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultSaveStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultSaveStrategyFallbackCause>());
  private static readonly DataVaultReadStrategyDiagnostics NotEvaluatedReadStrategy = new(
      DataVaultReadStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultReadStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>());

  private readonly IDataVaultProviderBehaviorSelector _providerBehaviorSelector;
  private readonly IReadOnlyList<IDataVaultProviderBridgeReadStrategy> _providerBridgeReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderPitReadStrategy> _providerPitReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderReadStrategy> _providerReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderSaveStrategy> _providerSaveStrategies;
  private readonly IStableHashService _stableHashService;

  public DefaultDataVaultDiagnosticsService(
      IEnumerable<IDataVaultProviderSaveStrategy> providerSaveStrategies,
      IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies,
      IEnumerable<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      IEnumerable<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies,
      IDataVaultProviderBehaviorSelector providerBehaviorSelector,
      IStableHashService stableHashService) {
    ArgumentNullException.ThrowIfNull(providerSaveStrategies);
    ArgumentNullException.ThrowIfNull(providerReadStrategies);
    ArgumentNullException.ThrowIfNull(providerPitReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBridgeReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBehaviorSelector);
    ArgumentNullException.ThrowIfNull(stableHashService);

    _providerSaveStrategies = providerSaveStrategies.ToArray();
    _providerReadStrategies = providerReadStrategies.ToArray();
    _providerPitReadStrategies = providerPitReadStrategies.ToArray();
    _providerBridgeReadStrategies = providerBridgeReadStrategies.ToArray();
    _providerBehaviorSelector = providerBehaviorSelector;
    _stableHashService = stableHashService;
  }

  public DataVaultDiagnosticsResult Analyze(DataVaultMetadataModel metadataModel) {
    return Analyze(metadataModel, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  public DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(metadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return AnalyzeMetadataModel(
        metadataModel,
        providerCapabilities,
        DataVaultMetadataSourceKinds.ModelMetadata,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataModel),
        providerName: null,
        providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
        capabilityProfileDefaulted: false,
        providerBehaviorDefaulted: false);
  }

  public DataVaultDiagnosticsResult Analyze(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    var providerCapabilities = metadataRegistry.TryGetProviderCapabilityProfile(
        DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
        out var registryProfile) && registryProfile is not null
        ? registryProfile
        : DataVaultProviderCapabilityProfiles.Sqlite;

    return Analyze(metadataRegistry, providerCapabilities);
  }

  public DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataRegistry metadataRegistry,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return AnalyzeMetadataModel(
        DataVaultMetadataSourceAnnotations.CreateMetadataModel(metadataRegistry),
        providerCapabilities,
        DataVaultMetadataSourceKinds.ModelRegistry,
        DataVaultMetadataSourceAnnotations.CreateFingerprint(metadataRegistry),
        providerName: null,
        providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
        capabilityProfileDefaulted: false,
        providerBehaviorDefaulted: false);
  }

  public DataVaultDiagnosticsResult Analyze(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    return Analyze(configureModel, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  public DataVaultDiagnosticsResult Analyze(
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(configureModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    try {
      var builder = new DataVaultCodeFirstModelBuilder();
      configureModel(builder);
      return AnalyzeMetadataModel(
          builder.BuildMetadataModel(),
          providerCapabilities,
          "code-first",
          sourceFingerprint: null,
          providerName: null,
          providerBehaviorProfile: DataVaultProviderBehaviorProfiles.ProviderNeutral,
          capabilityProfileDefaulted: false,
          providerBehaviorDefaulted: false);
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return CreateFailureResult(
          "code-first",
          providerCapabilities,
          new DataVaultDiagnosticsIssue(
              DataVaultDiagnosticsIssueSeverity.Error,
              "code-first-validation-failed",
              exception.Message,
              "code-first"));
    }
  }

  public DataVaultDiagnosticsResult Analyze(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var satellite = DataVaultRegistryMetadataResolver.GetRequiredSatellite(
        registry,
        request.Parent,
        request.SatelliteName);

    return AnalyzeDbContext(
        dbContext,
        requests: null,
        readRequest: new DataVaultLatestSatelliteReadRequest(satellite, request.ParentHashKeys, request.AsOf));
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, requests: null, readRequest: request);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var bridge = DataVaultRegistryMetadataResolver.GetRequiredBridge(registry, request.BridgeName);

    return AnalyzeDbContext(
        dbContext,
        requests: null,
        readRequest: new DataVaultBridgeReadRequest(bridge, request.Endpoint, request.EndpointHashKeys, request.MaximumDepth));
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultSaveRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, [request], readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBulkSaveRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    return AnalyzeDbContext(dbContext, request.Requests, readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistrySaveRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    return AnalyzeDbContext(
        dbContext,
        [DataVaultSaveServiceRegistryExtensions.ResolveRequest(registry, request)],
        readRequest: null);
  }

  public DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var requests = request.Requests
        .Select(current => DataVaultSaveServiceRegistryExtensions.ResolveRequest(registry, current))
        .ToArray();

    return AnalyzeDbContext(dbContext, requests, readRequest: null);
  }

  private DataVaultDiagnosticsResult AnalyzeMetadataModel(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities,
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var validationIssues = ValidateMetadataModel(metadataModel)
        .Concat(ValidateProviderMappings(metadataModel, providerCapabilities))
        .Concat(DataVaultEfMetadataTranslator.ValidateProviderIdentifiers(metadataModel, providerCapabilities, providerName))
        .ToArray();
    var issues = validationIssues.ToList();

    ModelBuilder? modelBuilder = null;
    if (!validationIssues.Any(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)) {
      try {
        modelBuilder = new ModelBuilder(new ConventionSet());
        modelBuilder.UseDataVault(providerCapabilities);
        DataVaultEfMetadataTranslator.Apply(modelBuilder, metadataModel, providerCapabilities);
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "metadata-translation-failed",
            exception.Message,
            "explain"));
      }
    }

    var explain = modelBuilder is null
        ? CreateEmptyExplain(
            sourceKind,
            sourceFingerprint,
            providerName,
            providerCapabilities,
            providerBehaviorProfile,
            capabilityProfileDefaulted,
            providerBehaviorDefaulted)
        : CreateExplain(
            modelBuilder.Model,
            sourceKind,
            sourceFingerprint,
            providerName,
            providerCapabilities,
            providerBehaviorProfile,
            capabilityProfileDefaulted,
            providerBehaviorDefaulted);

    return CreateResult(explain, NotEvaluatedStrategy, NotEvaluatedReadStrategy, issues);
  }

  private DataVaultDiagnosticsResult AnalyzeDbContext(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest>? requests,
      object? readRequest) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var providerName = dbContext.Database.ProviderName;
    var capabilityProfile = DataVaultProviderCapabilityProfileSelection.Select(providerName);
    var capabilityProfileDefaulted =
        !string.IsNullOrWhiteSpace(providerName) &&
        !DataVaultProviderCapabilityProfileSelection.TrySelectRegistered(providerName, out _);
    var providerBehavior = _providerBehaviorSelector.SelectBehavior(dbContext);
    var providerBehaviorDefaulted =
        !string.IsNullOrWhiteSpace(providerName) &&
        string.Equals(
            providerBehavior.ProfileName,
            DataVaultProviderBehaviorProfiles.ProviderNeutral.ProfileName,
            StringComparison.Ordinal);
    var issues = new List<DataVaultDiagnosticsIssue>();

    if (capabilityProfileDefaulted) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Warning,
          "capability-profile-defaulted",
          "Provider name '" + providerName + "' did not resolve to a registered Data Vault provider capability profile; diagnostics used '" +
          capabilityProfile.ProfileName +
          "'.",
          "capability-profile"));
    }

    if (providerBehaviorDefaulted) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Warning,
          "provider-behavior-defaulted",
          "Provider name '" + providerName + "' did not resolve to a provider-specific Data Vault behavior profile; diagnostics used '" +
          providerBehavior.ProfileName +
          "'.",
          "provider-behavior"));
    }

    var extension = DataVaultDbContextMetadataSource.FindExtension(dbContext);
    if (extension is not null) {
      try {
        var source = DataVaultDbContextMetadataSource.Resolve(dbContext, extension);
        issues.AddRange(ValidateMetadataModel(DataVaultMetadataSourceAnnotations.CreateMetadataModel(source.MetadataRegistry)));
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "metadata-source-resolution-failed",
            exception.Message,
            "metadata-source"));
      }
    }

    var explainModel = dbContext.GetService<IDesignTimeModel>().Model;
    var explain = CreateExplain(
        explainModel,
        GetStringAnnotation(explainModel, DataVaultAnnotationNames.MetadataSourceKind) ?? "<model>",
        GetStringAnnotation(explainModel, DataVaultAnnotationNames.MetadataSourceFingerprint),
        providerName,
        capabilityProfile,
        providerBehavior,
        capabilityProfileDefaulted,
        providerBehaviorDefaulted);
    var strategy = requests is null
        ? NotEvaluatedStrategy with { ProviderName = providerName }
        : EvaluateSaveStrategy(dbContext, requests, capabilityProfileDefaulted);
    var readStrategy = readRequest switch {
      null => NotEvaluatedReadStrategy with { ProviderName = providerName },
      DataVaultLatestSatelliteReadRequest latestRequest => EvaluateReadStrategy(dbContext, latestRequest, capabilityProfileDefaulted),
      DataVaultPitAsOfReadRequest pitRequest => EvaluatePitReadStrategy(dbContext, pitRequest, capabilityProfileDefaulted),
      DataVaultBridgeReadRequest bridgeRequest => EvaluateBridgeReadStrategy(dbContext, bridgeRequest, capabilityProfileDefaulted),
      _ => NotEvaluatedReadStrategy with { ProviderName = providerName },
    };
    var readShape = CreateReadShapeDiagnostics(explain, readStrategy, readRequest);
    var providerTuning = CreateProviderTuningDiagnostics(strategy, readStrategy, readShape);

    return CreateResult(explain, strategy, readStrategy, issues, readShape, providerTuning);
  }

  private DataVaultSaveStrategyDiagnostics EvaluateSaveStrategy(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerSaveStrategies
        .Select((strategy, registrationOrdinal) => new SaveStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultSaveStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      var stagedProviderBulk = DataVaultStagedProviderBulkDiagnosticsSupport.TryEvaluate(strategy, dbContext, requests);
      bool canSave;
      IReadOnlyList<DataVaultSaveStrategyFallbackCause> fallbackCauses;
      try {
        canSave = strategy.CanSave(dbContext, requests);
        if (canSave) {
          fallbackCauses = Array.Empty<DataVaultSaveStrategyFallbackCause>();
        }
        else if (DataVaultProviderSaveStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                requests,
                out var evaluation)) {
          fallbackCauses = evaluation.FallbackCauses;
        }
        else {
          var stagedFallbackCauses = DataVaultStagedProviderBulkDiagnosticsSupport.CreateFallbackCauses(stagedProviderBulk);
          fallbackCauses = stagedFallbackCauses.Count > 0
              ? stagedFallbackCauses
              : new[]
              {
                  new DataVaultSaveStrategyFallbackCause(
                      DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                      "Provider save strategy '" + strategy.GetType().Name + "' declined the request batch."),
              };
        }
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canSave = false;
        var stagedFallbackCauses = DataVaultStagedProviderBulkDiagnosticsSupport.CreateFallbackCauses(stagedProviderBulk);
        fallbackCauses = stagedFallbackCauses.Count > 0
            ? stagedFallbackCauses
            : new[]
            {
                new DataVaultSaveStrategyFallbackCause(
                    DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider save strategy '" + strategy.GetType().Name + "' failed compatibility evaluation."),
            };
      }

      var candidate = new DataVaultSaveStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canSave,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(strategy),
        StagedProviderBulk = stagedProviderBulk,
      };
      candidates.Add(candidate);

      if (canSave) {
        var representativeStagedProviderBulk = candidate.StagedProviderBulk ??
            DataVaultStagedProviderBulkDiagnosticsSupport.SelectRepresentative(candidates);
        return new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultSaveStrategyFallbackCause>()) {
          StagedProviderBulk = representativeStagedProviderBulk,
        };
      }
    }

    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (orderedStrategies.Length == 0) {
      fallbackCauseList.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          "No provider-specific Data Vault save strategy is registered."));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
          "Every registered provider-specific Data Vault save strategy declined the request batch."));
    }

    return new DataVaultSaveStrategyDiagnostics(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList)) {
      StagedProviderBulk = DataVaultStagedProviderBulkDiagnosticsSupport.SelectRepresentative(candidates),
    };
  }

  private DataVaultReadStrategyDiagnostics EvaluateReadStrategy(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerReadStrategies
        .Select((strategy, registrationOrdinal) => new ReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadLatestSatelliteRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the latest/as-of satellite read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownLatestSatelliteGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (orderedStrategies.Length == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          "No provider-specific Data Vault read strategy is registered."));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
          "Every registered provider-specific Data Vault read strategy declined the latest/as-of satellite read request."));
    }

    return new DataVaultReadStrategyDiagnostics(
        DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList));
  }

  private DataVaultReadStrategyDiagnostics EvaluatePitReadStrategy(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerPitReadStrategies
        .Select((strategy, registrationOrdinal) => new PitReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadPitRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the PIT read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownPitGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    return CreateReadFallbackDiagnostics(
        providerName,
        capabilityProfileDefaulted,
        orderedStrategies.Length,
        candidates,
        "No provider-specific Data Vault PIT read strategy is registered.",
        "Every registered provider-specific Data Vault PIT read strategy declined the request.");
  }

  private DataVaultReadStrategyDiagnostics EvaluateBridgeReadStrategy(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      bool capabilityProfileDefaulted) {
    var providerName = dbContext.Database.ProviderName;
    var orderedStrategies = _providerBridgeReadStrategies
        .Select((strategy, registrationOrdinal) => new BridgeReadStrategyRegistration(strategy, registrationOrdinal))
        .OrderByDescending(registration => registration.Strategy.Priority)
        .ThenBy(registration => registration.RegistrationOrdinal)
        .ToArray();
    var candidates = new List<DataVaultReadStrategyCandidateDiagnostics>();

    for (var ordinal = 0; ordinal < orderedStrategies.Length; ordinal++) {
      var strategy = orderedStrategies[ordinal].Strategy;
      bool canRead;
      IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses;
      try {
        canRead = strategy.CanReadBridgeRows(dbContext, request);
        fallbackCauses = canRead
            ? Array.Empty<DataVaultReadStrategyFallbackCause>()
            : DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
                strategy,
                dbContext,
                request,
                out var evaluation)
                ? evaluation.FallbackCauses
                : [new DataVaultReadStrategyFallbackCause(
                    DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                    "Provider read strategy '" + strategy.GetType().Name + "' declined the bridge read request.")];
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        canRead = false;
        fallbackCauses = [new DataVaultReadStrategyFallbackCause(
            DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
            "Provider read strategy '" + strategy.GetType().Name + "' failed compatibility evaluation.")];
      }

      var candidate = new DataVaultReadStrategyCandidateDiagnostics(
          ordinal,
          strategy.GetType().Name,
          strategy.Priority,
          canRead,
          fallbackCauses) {
        SupportedProviderNames = DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(strategy),
        GateRequirements = DataVaultProviderReadStrategyGateEvaluator.GetKnownBridgeGateRequirements(strategy),
      };
      candidates.Add(candidate);

      if (canRead) {
        return new DataVaultReadStrategyDiagnostics(
            DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            candidate.StrategyName,
            candidate.Priority,
            candidates,
            Array.Empty<DataVaultReadStrategyFallbackCause>());
      }
    }

    return CreateReadFallbackDiagnostics(
        providerName,
        capabilityProfileDefaulted,
        orderedStrategies.Length,
        candidates,
        "No provider-specific Data Vault bridge read strategy is registered.",
        "Every registered provider-specific Data Vault bridge read strategy declined the request.");
  }

  private static DataVaultReadStrategyDiagnostics CreateReadFallbackDiagnostics(
      string? providerName,
      bool capabilityProfileDefaulted,
      int strategyCount,
      IReadOnlyList<DataVaultReadStrategyCandidateDiagnostics> candidates,
      string noStrategyMessage,
      string allDeclinedMessage) {
    var fallbackCauseList = candidates
        .SelectMany(candidate => candidate.FallbackCauses)
        .ToList();

    if (strategyCount == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          noStrategyMessage));
    }

    if (capabilityProfileDefaulted &&
        !fallbackCauseList.Any(cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauseList.Insert(0, new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauseList.Count == 0) {
      fallbackCauseList.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
          allDeclinedMessage));
    }

    return new DataVaultReadStrategyDiagnostics(
        DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        candidates,
        DistinctFallbackCauses(fallbackCauseList));
  }

  private static DataVaultReadShapeDiagnostics? CreateReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultReadStrategyDiagnostics readStrategy,
      object? readRequest) {
    return readRequest switch {
      DataVaultLatestSatelliteReadRequest latestRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.LatestSatellite,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.LatestSatellite),
          Satellite: CreateSatelliteReadShapeDiagnostics(explain, latestRequest)),
      DataVaultPitAsOfReadRequest pitRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.PitAsOf,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.PitAsOf),
          Pit: CreatePitReadShapeDiagnostics(explain, pitRequest)),
      DataVaultBridgeReadRequest bridgeRequest => new DataVaultReadShapeDiagnostics(
          DataVaultReadShapeKind.Bridge,
          CreateReadShapeProviderDiagnostics(explain, readStrategy, DataVaultReadShapeKind.Bridge),
          Bridge: CreateBridgeReadShapeDiagnostics(explain, bridgeRequest)),
      _ => null,
    };
  }

  private static DataVaultReadShapeProviderDiagnostics CreateReadShapeProviderDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultReadStrategyDiagnostics readStrategy,
      DataVaultReadShapeKind readShapeKind) {
    return new DataVaultReadShapeProviderDiagnostics(
        readStrategy.ProviderName ?? explain.ProviderName,
        explain.CapabilityProfileName,
        explain.CapabilityProfileDefaulted,
        explain.ProviderBehaviorProfileName,
        explain.ProviderBehaviorDefaulted,
        readStrategy.Status,
        readStrategy.FallbackCauses) {
      SelectedStrategyName = readStrategy.SelectedStrategyName,
      Recommendation = CreateReadProviderTuningRecommendation(readStrategy, readShapeKind),
    };
  }

  private static DataVaultProviderTuningDiagnostics? CreateProviderTuningDiagnostics(
      DataVaultSaveStrategyDiagnostics saveStrategy,
      DataVaultReadStrategyDiagnostics readStrategy,
      DataVaultReadShapeDiagnostics? readShape) {
    var save = CreateSaveProviderTuningDiagnostics(saveStrategy);
    var read = readShape is null ? null : CreateReadProviderTuningDiagnostics(readStrategy, readShape.Kind);

    return save is null && read is null
        ? null
        : new DataVaultProviderTuningDiagnostics(save, read);
  }

  private static DataVaultSaveProviderTuningDiagnostics? CreateSaveProviderTuningDiagnostics(
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated) {
      return null;
    }

    var thresholdFacts = CreateSaveProviderThresholdFacts(strategy);
    return new DataVaultSaveProviderTuningDiagnostics(
        CreateSaveProviderTuningRecommendation(strategy),
        thresholdFacts.Count == 0 ? null : thresholdFacts);
  }

  private static DataVaultReadProviderTuningDiagnostics? CreateReadProviderTuningDiagnostics(
      DataVaultReadStrategyDiagnostics strategy,
      DataVaultReadShapeKind readShapeKind) {
    if (strategy.Status == DataVaultReadStrategyDiagnosticsStatus.NotEvaluated) {
      return null;
    }

    return new DataVaultReadProviderTuningDiagnostics(
        CreateReadProviderTuningRecommendation(strategy, readShapeKind));
  }

  private static DataVaultProviderTuningRecommendation CreateSaveProviderTuningRecommendation(
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected &&
        !string.Equals(strategy.SelectedStrategyName, "SqliteDataVaultSaveStrategy", StringComparison.Ordinal)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.StagedProviderIngestion,
          "Staged provider ingestion",
          "Provider-specific save diagnostics selected an eligible ordered bulk path; keep the context clean and verify provider-local benchmark evidence before claiming provider-native ingestion behavior.");
    }

    if (strategy.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback &&
        strategy.Candidates.Any(candidate => candidate.SupportedProviderNames.Count > 0)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.StagedProviderIngestion,
          "Staged provider ingestion",
          "Provider-specific save diagnostics evaluated registered candidates but fell back; use the reported gates, fallback causes, and threshold facts before claiming provider-native ingestion behavior.");
    }

    return new DataVaultProviderTuningRecommendation(
        DataVaultPerformanceProfileCategory.SmallAppLocalVault,
        "Small app-local vault",
        "Save diagnostics are provider-neutral or SQLite-selected; use the small app-local vault profile until provider-specific eligibility and local evidence justify a wider ingestion profile.");
  }

  private static DataVaultProviderTuningRecommendation CreateReadProviderTuningRecommendation(
      DataVaultReadStrategyDiagnostics strategy,
      DataVaultReadShapeKind readShapeKind) {
    if (strategy.Status == DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected &&
        IsRepositoryProvenOptimizedReadStrategy(strategy.SelectedStrategyName, readShapeKind)) {
      return new DataVaultProviderTuningRecommendation(
          DataVaultPerformanceProfileCategory.ReadModelHeavy,
          "Read-model heavy",
          "Read diagnostics selected the repository-proven " +
          FormatOptimizedReadProviderName(strategy.SelectedStrategyName) +
          " optimized path for " +
          readShapeKind +
          "; keep PIT and bridge rows maintained when those shapes are used.");
    }

    return new DataVaultProviderTuningRecommendation(
        DataVaultPerformanceProfileCategory.ReadModelHeavy,
        "Read-model heavy",
        "Read diagnostics provide provider-neutral " +
        readShapeKind +
        " guidance; SQLite remains the repository-proven optimized latest-satellite provider, while SQLite, PostgreSQL, and SQL Server are repository-proven optimized PIT/bridge providers when diagnostics select their candidates. Unsupported providers, unsupported shapes, or incomplete read-shape evidence remain fallback guidance.");
  }

  private static bool IsRepositoryProvenOptimizedReadStrategy(
      string? selectedStrategyName,
      DataVaultReadShapeKind readShapeKind) {
    return selectedStrategyName switch {
      "SqliteDataVaultReadStrategy" => true,
      "PostgresDataVaultReadStrategy" => readShapeKind is DataVaultReadShapeKind.PitAsOf or DataVaultReadShapeKind.Bridge,
      "SqlServerDataVaultReadStrategy" => readShapeKind is DataVaultReadShapeKind.PitAsOf or DataVaultReadShapeKind.Bridge,
      _ => false,
    };
  }

  private static string FormatOptimizedReadProviderName(string? selectedStrategyName) {
    return selectedStrategyName switch {
      "SqliteDataVaultReadStrategy" => "SQLite",
      "PostgresDataVaultReadStrategy" => "PostgreSQL",
      "SqlServerDataVaultReadStrategy" => "SQL Server",
      _ => "provider-specific",
    };
  }

  private static IReadOnlyList<DataVaultProviderThresholdFact> CreateSaveProviderThresholdFacts(
      DataVaultSaveStrategyDiagnostics strategy) {
    var facts = new List<DataVaultProviderThresholdFact>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var candidate in strategy.Candidates.OrderBy(candidate => candidate.Ordinal)) {
      foreach (var requirement in candidate.GateRequirements) {
        if (!requirement.MinimumTotalOperationCount.HasValue && !requirement.MaximumSatelliteOperationCount.HasValue) {
          continue;
        }

        foreach (var providerName in candidate.SupportedProviderNames) {
          var fact = CreateSaveProviderThresholdFact(candidate.StrategyName, providerName, requirement);
          var key = fact.Kind +
              "\u001f" +
              fact.GateKind +
              "\u001f" +
              fact.ProviderName +
              "\u001f" +
              fact.MinimumTotalOperationCount?.ToString(CultureInfo.InvariantCulture) +
              "\u001f" +
              fact.MaximumSatelliteOperationCount?.ToString(CultureInfo.InvariantCulture);
          if (keys.Add(key)) {
            facts.Add(fact);
          }
        }
      }
    }

    return facts;
  }

  private static DataVaultProviderThresholdFact CreateSaveProviderThresholdFact(
      string strategyName,
      string providerName,
      DataVaultSaveStrategyGateRequirement requirement) {
    if (requirement.MinimumTotalOperationCount.HasValue) {
      var minimum = requirement.MinimumTotalOperationCount.Value;
      return new DataVaultProviderThresholdFact(
          DataVaultProviderThresholdFactKind.MinimumTotalOperationCount,
          requirement.Kind,
          providerName,
          FormatSaveStrategyDisplayName(strategyName) +
          " optimized dispatch requires at least " +
          minimum.ToString(CultureInfo.InvariantCulture) +
          " total operations.") {
        MinimumTotalOperationCount = minimum,
      };
    }

    var maximum = requirement.MaximumSatelliteOperationCount.GetValueOrDefault();
    return new DataVaultProviderThresholdFact(
        DataVaultProviderThresholdFactKind.MaximumSatelliteOperationCount,
        requirement.Kind,
        providerName,
        FormatSaveStrategyDisplayName(strategyName) +
        " optimized dispatch accepts at most " +
        maximum.ToString(CultureInfo.InvariantCulture) +
        " satellite operations.") {
      MaximumSatelliteOperationCount = maximum,
    };
  }

  private static string FormatSaveStrategyDisplayName(string strategyName) {
    return strategyName switch {
      "SqlServerDataVaultSaveStrategy" => "SQL Server",
      "MySqlStagedDataVaultSaveStrategy" => "MySQL staged bulk",
      "MySqlDataVaultSaveStrategy" => "MySQL",
      "OracleDataVaultSaveStrategy" => "Oracle",
      "PostgresDataVaultSaveStrategy" => "PostgreSQL",
      "SqliteDataVaultSaveStrategy" => "SQLite",
      _ => strategyName,
    };
  }

  private static DataVaultSatelliteReadShapeDiagnostics CreateSatelliteReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultLatestSatelliteReadRequest request) {
    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(request.Satellite);
    var entity = FindEntityExplain(
        explain,
        DataVaultTableKind.Satellite,
        request.Satellite.Name,
        projection.TableName);
    var filterColumns = new List<DataVaultReadShapeColumnSet>
    {
        new("parentHashKeyFilter", [projection.ParentHashKeyColumnName]),
    };
    if (request.AsOf.HasValue) {
      filterColumns.Add(new DataVaultReadShapeColumnSet("asOfCutoff", [projection.LoadTimestampColumnName]));
    }

    var orderingColumns = new[]
    {
        projection.ParentHashKeyColumnName,
    }
        .Concat(projection.DrivingKeyColumnNames)
        .ToArray();

    return new DataVaultSatelliteReadShapeDiagnostics(
        request.AsOf.HasValue
            ? DataVaultSatelliteReadSemantics.AsOf
            : DataVaultSatelliteReadSemantics.Current,
        new DataVaultReadShapeEntity(request.Satellite.Name, DataVaultTableKind.Satellite, projection.TableName),
        new DataVaultParentReferenceExplain(request.Satellite.Parent.Kind, request.Satellite.Parent.Name),
        filterColumns,
        "Select the latest load timestamp per parent hash key and driving-key series.",
        request.AsOf.HasValue
            ? "Apply " + projection.LoadTimestampColumnName + " <= supplied as-of cutoff; the cutoff value is not included in diagnostics."
            : "No as-of cutoff is applied; current reads consider all persisted satellite rows.",
        [new DataVaultReadShapeColumnSet("resultOrdering", orderingColumns)],
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreateSatelliteProjectedColumns(projection),
    };
  }

  private static DataVaultPitReadShapeDiagnostics CreatePitReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultPitAsOfReadRequest request) {
    var pit = request.Pit;
    var tableName = GetPitTableName(pit.Name);
    var parentHashKeyColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, pit.Parent.Name, tableName));
    var loadTimestampColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, pit.Name, tableName));
    var entity = FindEntityExplain(explain, DataVaultTableKind.Pit, pit.Name, tableName);
    var pitDrivingKeyColumnNames = FindPropertyColumnNames(entity, DataVaultPropertyRole.DrivingKey);
    var snapshotColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        pit.Satellites.Select(satellite => satellite.SatelliteName + " Load Timestamp"),
        [parentHashKeyColumnName, .. pitDrivingKeyColumnNames, loadTimestampColumnName]);
    var referencedSatellites = pit.Satellites
        .Select((satellite, index) => {
          var satelliteTableName = DefaultDataVaultNamingPolicy.Instance.GetSatelliteTableName(
              new DataVaultSatelliteNameContext(pit.Parent.Name, satellite.SatelliteName));
          var satelliteParentHashKeyColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
              new DataVaultTechnicalColumnNameContext(
                  DataVaultTechnicalColumnKind.HashKey,
                  pit.Parent.Name,
                  satelliteTableName));
          var satelliteLoadTimestampColumnName = DefaultDataVaultNamingPolicy.Instance.GetTechnicalColumnName(
              new DataVaultTechnicalColumnNameContext(
                  DataVaultTechnicalColumnKind.LoadTimestamp,
                  satellite.SatelliteName,
                  satelliteTableName));
          var satelliteEntity = FindEntityExplain(
              explain,
              DataVaultTableKind.Satellite,
              satellite.SatelliteName,
              satelliteTableName);

          return new DataVaultPitReferencedSatelliteReadShapeDiagnostics(
              satellite.SatelliteName,
              satelliteTableName,
              snapshotColumnNames[index],
              satelliteParentHashKeyColumnName,
              satelliteLoadTimestampColumnName,
              FindPropertyColumnNames(satelliteEntity, DataVaultPropertyRole.DrivingKey));
        })
        .ToArray();
    var rowIdentityColumns = new[]
    {
        parentHashKeyColumnName,
    }
        .Concat(pitDrivingKeyColumnNames)
        .Append(loadTimestampColumnName)
        .ToArray();

    return new DataVaultPitReadShapeDiagnostics(
        new DataVaultReadShapeEntity(pit.Name, DataVaultTableKind.Pit, tableName),
        new DataVaultParentReferenceExplain(pit.Parent.Kind, pit.Parent.Name),
        referencedSatellites,
        [
            new DataVaultReadShapeColumnSet("parentHashKeyFilter", [parentHashKeyColumnName]),
            new DataVaultReadShapeColumnSet("asOfCutoff", [loadTimestampColumnName]),
        ],
        pitDrivingKeyColumnNames.Count == 0
            ? "Select the latest PIT row per parent hash key with " + loadTimestampColumnName + " <= supplied as-of cutoff."
            : "Select the latest PIT row per parent hash key and driving-key tuple with " + loadTimestampColumnName + " <= supplied as-of cutoff.",
        pitDrivingKeyColumnNames.Count == 0
            ? "Resolve each satellite snapshot by parent hash key and the snapshot load-timestamp reference stored on the selected PIT row."
            : "Resolve ordinary satellite snapshots by parent hash key and multi-active satellite snapshots by parent hash key, driving-key tuple, and the snapshot load-timestamp reference stored on the selected PIT row.",
        "Missing PIT rows or null satellite snapshot references yield no latest-satellite fallback.",
        "PIT rows must already be maintained; diagnostics and reads do not rebuild or refresh PIT tables.",
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreatePitProjectedColumns(
          explain,
          pit,
          parentHashKeyColumnName,
          pitDrivingKeyColumnNames,
          loadTimestampColumnName,
          referencedSatellites),
      RowIdentityColumns = [new DataVaultReadShapeColumnSet("pitRowIdentity", rowIdentityColumns)],
      ReferencedSatelliteLookupCount = referencedSatellites.Length,
    };
  }

  private static DataVaultBridgeReadShapeDiagnostics CreateBridgeReadShapeDiagnostics(
      DataVaultExplainDiagnostics explain,
      DataVaultBridgeReadRequest request) {
    var bridge = request.Bridge;
    var tableName = GetBridgeTableName(bridge);
    var endpoints = bridge.Endpoints
        .Select(endpoint => new DataVaultBridgeEndpointReadShapeDiagnostics(
            ToPublicEndpoint(endpoint.Role),
            endpoint.SourceEndpointName,
            GetBridgeEndpointHashKeyColumnName(endpoint)))
        .ToArray();
    var filterEndpoint = endpoints.Single(endpoint => endpoint.Endpoint == request.Endpoint);
    var entity = FindEntityExplain(explain, DataVaultTableKind.Bridge, bridge.Name, tableName);
    var orderingColumns = request.MaximumDepth.HasValue
        ? endpoints.Select(endpoint => endpoint.ColumnName).Append(DataVaultBridgeProjectionRow.TraversalDepthName).ToArray()
        : endpoints.Select(endpoint => endpoint.ColumnName).ToArray();

    return new DataVaultBridgeReadShapeDiagnostics(
        bridge.Kind,
        new DataVaultReadShapeEntity(bridge.Name, DataVaultTableKind.Bridge, tableName),
        endpoints,
        request.Endpoint,
        new DataVaultReadShapeColumnSet("endpointHashKeyFilter", [filterEndpoint.ColumnName]),
        request.MaximumDepth.HasValue
            ? new DataVaultReadShapeColumnSet("maximumDepthPredicate", [DataVaultBridgeProjectionRow.TraversalDepthName])
            : null,
        [new DataVaultReadShapeColumnSet("resultOrdering", orderingColumns)],
        GetSupportedBridgeEndpointRules(bridge.Kind),
        CreateIndexBaseline(entity)) {
      ProjectedColumns = CreateBridgeProjectedColumns(endpoints, request.MaximumDepth.HasValue),
    };
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreateSatelliteProjectedColumns(
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new(
            "technicalProjection",
            [
                projection.ParentHashKeyColumnName,
                projection.HashDiffColumnName,
                projection.LoadTimestampColumnName,
                projection.RecordSourceColumnName,
            ]),
        new("payloadProjection", projection.PayloadColumnNames),
    };

    if (projection.DrivingKeyColumnNames.Count > 0) {
      columnSets.Add(new DataVaultReadShapeColumnSet("drivingKeyProjection", projection.DrivingKeyColumnNames));
    }

    return columnSets;
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreatePitProjectedColumns(
      DataVaultExplainDiagnostics explain,
      DataVaultPitMetadata pit,
      string parentHashKeyColumnName,
      IReadOnlyList<string> pitDrivingKeyColumnNames,
      string loadTimestampColumnName,
      IReadOnlyList<DataVaultPitReferencedSatelliteReadShapeDiagnostics> referencedSatellites) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new DataVaultReadShapeColumnSet(
            "pitTechnicalProjection",
            [
                parentHashKeyColumnName,
                loadTimestampColumnName,
            ]),
    };

    if (pitDrivingKeyColumnNames.Count > 0) {
      columnSets.Add(new DataVaultReadShapeColumnSet("pitDrivingKeyProjection", pitDrivingKeyColumnNames));
    }

    columnSets.AddRange(
    [
        new DataVaultReadShapeColumnSet(
            "snapshotReferenceProjection",
            referencedSatellites.Select(satellite => satellite.SnapshotReferenceColumnName).ToArray()),
        new DataVaultReadShapeColumnSet(
            "satellitePayloadProjection",
            CreatePitSatellitePayloadProjectionColumns(explain, pit)),
    ]);

    return columnSets;
  }

  private static IReadOnlyList<string> FindPropertyColumnNames(
      DataVaultEntityExplain? entity,
      DataVaultPropertyRole role) {
    return entity?.Properties
        .Where(property => property.Role == role)
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .Select(property => property.Name)
        .ToArray() ?? Array.Empty<string>();
  }

  private static IReadOnlyList<string> CreatePitSatellitePayloadProjectionColumns(
      DataVaultExplainDiagnostics explain,
      DataVaultPitMetadata pit) {
    return pit.Satellites
        .SelectMany(satellite => FindSatellitePayloadColumnNames(explain, pit.Parent, satellite.SatelliteName))
        .ToArray();
  }

  private static IReadOnlyList<string> FindSatellitePayloadColumnNames(
      DataVaultExplainDiagnostics explain,
      DataVaultMetadataReference parent,
      string satelliteName) {
    return explain.Entities
        .Where(entity =>
            entity.TableKind == DataVaultTableKind.Satellite &&
            string.Equals(entity.MetadataName, satelliteName, StringComparison.Ordinal) &&
            entity.ParentReference is not null &&
            entity.ParentReference.Kind == parent.Kind &&
            string.Equals(entity.ParentReference.Name, parent.Name, StringComparison.Ordinal))
        .SelectMany(entity => entity.Properties
            .Where(property => property.Role == DataVaultPropertyRole.Payload)
            .Select(property => property.Name))
        .ToArray();
  }

  private static IReadOnlyList<DataVaultReadShapeColumnSet> CreateBridgeProjectedColumns(
      IReadOnlyList<DataVaultBridgeEndpointReadShapeDiagnostics> endpoints,
      bool includeDepthProjection) {
    var columnSets = new List<DataVaultReadShapeColumnSet>
    {
        new("endpointProjection", endpoints.Select(endpoint => endpoint.ColumnName).ToArray()),
    };

    if (includeDepthProjection) {
      columnSets.Add(new DataVaultReadShapeColumnSet("depthProjection", [DataVaultBridgeProjectionRow.TraversalDepthName]));
    }

    return columnSets;
  }

  private static DataVaultEntityExplain? FindEntityExplain(
      DataVaultExplainDiagnostics explain,
      DataVaultTableKind tableKind,
      string metadataName,
      string tableName) {
    return explain.Entities.FirstOrDefault(entity =>
        entity.TableKind == tableKind &&
        string.Equals(entity.MetadataName, metadataName, StringComparison.Ordinal) &&
        string.Equals(entity.TableName, tableName, StringComparison.Ordinal));
  }

  private static IReadOnlyList<DataVaultReadShapeIndexBaseline> CreateIndexBaseline(
      DataVaultEntityExplain? entity) {
    if (entity is null) {
      return Array.Empty<DataVaultReadShapeIndexBaseline>();
    }

    var baselines = new List<DataVaultReadShapeIndexBaseline>();
    if (!string.Equals(entity.PrimaryKey.Name, "<none>", StringComparison.Ordinal)) {
      baselines.Add(new DataVaultReadShapeIndexBaseline(
          entity.PrimaryKey.Name,
          "primary-key",
          entity.PrimaryKey.PropertyNames,
          IsUnique: true,
          DescendingColumnNames: Array.Empty<string>(),
          IncludedColumnNames: Array.Empty<string>()));
    }

    baselines.AddRange(entity.Indexes.Select(index => new DataVaultReadShapeIndexBaseline(
        index.Name,
        "secondary-index",
        index.PropertyNames,
        index.IsUnique,
        index.DescendingPropertyNames,
        index.IncludedPropertyNames)));

    return baselines;
  }

  private static IReadOnlyList<string> GetSupportedBridgeEndpointRules(DataVaultBridgeKind bridgeKind) {
    return bridgeKind switch {
      DataVaultBridgeKind.ManyToMany => [
          "Many-to-many bridge reads support From and To endpoint filters.",
      ],
      DataVaultBridgeKind.Hierarchy => [
          "Hierarchy bridge reads support Ancestor and Descendant endpoint filters.",
          "Hierarchy bridge reads require a bounded maximumDepth predicate.",
      ],
      _ => Array.Empty<string>(),
    };
  }

  private static DataVaultBridgeTraversalEndpoint ToPublicEndpoint(DataVaultBridgeEndpointRole endpointRole) {
    return endpointRole switch {
      DataVaultBridgeEndpointRole.From => DataVaultBridgeTraversalEndpoint.From,
      DataVaultBridgeEndpointRole.To => DataVaultBridgeTraversalEndpoint.To,
      DataVaultBridgeEndpointRole.Ancestor => DataVaultBridgeTraversalEndpoint.Ancestor,
      DataVaultBridgeEndpointRole.Descendant => DataVaultBridgeTraversalEndpoint.Descendant,
      _ => throw new ArgumentOutOfRangeException(nameof(endpointRole), endpointRole, "Unsupported bridge endpoint role."),
    };
  }

  private static string GetPitTableName(string pitName) {
    return "Pit" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(pitName);
  }

  private static string GetBridgeTableName(DataVaultBridgeMetadata bridge) {
    return "Bridge" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(bridge.Name);
  }

  private static string GetBridgeEndpointHashKeyColumnName(DataVaultBridgeEndpointMetadata endpoint) {
    var baseName = endpoint.Role switch {
      DataVaultBridgeEndpointRole.Ancestor => "Ancestor" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      DataVaultBridgeEndpointRole.Descendant => "Descendant" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      _ => endpoint.HubReference.Name,
    };

    return DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(baseName) + "HashKey";
  }

  private static DataVaultDiagnosticsResult CreateResult(
      DataVaultExplainDiagnostics explain,
      DataVaultSaveStrategyDiagnostics strategy,
      DataVaultReadStrategyDiagnostics readStrategy,
      IReadOnlyList<DataVaultDiagnosticsIssue> issues,
      DataVaultReadShapeDiagnostics? readShape = null,
      DataVaultProviderTuningDiagnostics? providerTuning = null) {
    var issueArray = issues.ToArray();
    var validationIssues = issueArray
        .Where(issue => issue.Severity == DataVaultDiagnosticsIssueSeverity.Error)
        .ToArray();
    var validation = new DataVaultValidationDiagnostics(validationIssues.Length == 0, validationIssues);

    return new DataVaultDiagnosticsResult(validation, explain, strategy, issueArray) {
      ReadStrategy = readStrategy,
      ReadShape = readShape,
      ProviderTuning = providerTuning,
    };
  }

  private DataVaultDiagnosticsResult CreateFailureResult(
      string sourceKind,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultDiagnosticsIssue issue) {
    return CreateResult(
        CreateEmptyExplain(
            sourceKind,
            sourceFingerprint: null,
            providerName: null,
            providerCapabilities,
            DataVaultProviderBehaviorProfiles.ProviderNeutral,
            capabilityProfileDefaulted: false,
            providerBehaviorDefaulted: false),
        NotEvaluatedStrategy,
        NotEvaluatedReadStrategy,
        [issue]);
  }

  private DataVaultExplainDiagnostics CreateExplain(
      IReadOnlyModel model,
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var loadTimestampMapping = GetLoadTimestampMapping(providerCapabilities);
    var satelliteSnapshotReferenceMapping = GetSatelliteSnapshotReferenceMapping(providerCapabilities);
    var entities = model
        .GetEntityTypes()
        .Where(IsDataVaultEntity)
        .Select(CreateEntityExplain)
        .OrderBy(entity => GetEntityKindSortKey(entity.TableKind))
        .ThenBy(entity => entity.MetadataName, StringComparer.Ordinal)
        .ThenBy(entity => entity.TableName, StringComparer.Ordinal)
        .ToArray();

    return new DataVaultExplainDiagnostics(
        sourceKind,
        sourceFingerprint,
        providerName,
        providerCapabilities.ProfileName,
        capabilityProfileDefaulted,
        loadTimestampMapping.ValueFormat,
        loadTimestampMapping.NativeStoreType,
        providerBehaviorProfile.ProfileName,
        providerBehaviorDefaulted,
        entities) {
      SatelliteSnapshotReferenceValueFormat = satelliteSnapshotReferenceMapping.ValueFormat,
      SatelliteSnapshotReferenceStoreType = satelliteSnapshotReferenceMapping.NativeStoreType,
      TypeMappings = CreateTypeMappingExplain(providerCapabilities),
      MaximumIdentifierLength = providerCapabilities.MaximumIdentifierLength,
      AllowsIndexesCoveredByPrimaryKey = providerCapabilities.AllowsIndexesCoveredByPrimaryKey,
      UnsupportedIncludedIndexColumnMode = providerCapabilities.UnsupportedIncludedIndexColumnMode,
      SqlFunctionSupport = providerCapabilities.SqlFunctionSupport,
      ConcurrencySupport = providerCapabilities.ConcurrencySupport,
      StableHash = CreateStableHashExplain(),
    };
  }

  private DataVaultExplainDiagnostics CreateEmptyExplain(
      string sourceKind,
      string? sourceFingerprint,
      string? providerName,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderBehaviorProfile providerBehaviorProfile,
      bool capabilityProfileDefaulted,
      bool providerBehaviorDefaulted) {
    var loadTimestampMapping = GetLoadTimestampMapping(providerCapabilities);
    var satelliteSnapshotReferenceMapping = GetSatelliteSnapshotReferenceMapping(providerCapabilities);
    return new DataVaultExplainDiagnostics(
        sourceKind,
        sourceFingerprint,
        providerName,
        providerCapabilities.ProfileName,
        capabilityProfileDefaulted,
        loadTimestampMapping.ValueFormat,
        loadTimestampMapping.NativeStoreType,
        providerBehaviorProfile.ProfileName,
        providerBehaviorDefaulted,
        Array.Empty<DataVaultEntityExplain>()) {
      SatelliteSnapshotReferenceValueFormat = satelliteSnapshotReferenceMapping.ValueFormat,
      SatelliteSnapshotReferenceStoreType = satelliteSnapshotReferenceMapping.NativeStoreType,
      TypeMappings = CreateTypeMappingExplain(providerCapabilities),
      MaximumIdentifierLength = providerCapabilities.MaximumIdentifierLength,
      AllowsIndexesCoveredByPrimaryKey = providerCapabilities.AllowsIndexesCoveredByPrimaryKey,
      UnsupportedIncludedIndexColumnMode = providerCapabilities.UnsupportedIncludedIndexColumnMode,
      SqlFunctionSupport = providerCapabilities.SqlFunctionSupport,
      ConcurrencySupport = providerCapabilities.ConcurrencySupport,
      StableHash = CreateStableHashExplain(),
    };
  }

  private DataVaultStableHashExplain CreateStableHashExplain() {
    var digest = _stableHashService.ComputeHash(string.Empty);
    return new DataVaultStableHashExplain(
        _stableHashService.AlgorithmId,
        digest.DigestByteLength,
        "lowercase-hex-no-prefix");
  }

  private static DataVaultEntityExplain CreateEntityExplain(IReadOnlyEntityType entityType) {
    var producedName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ProducedName) ??
        entityType.GetTableName() ??
        entityType.Name;
    var tableName = entityType.GetTableName() ?? producedName;
    var tableIdentifier = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());
    var tableKind = GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind);
    var metadataName = GetStringAnnotation(entityType, DataVaultAnnotationNames.MetadataName) ?? tableName;
    var parentKind = GetNullableAnnotationValue<DataVaultMetadataReferenceKind>(
        entityType,
        DataVaultAnnotationNames.ParentReferenceKind);
    var parentName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ParentReferenceName);
    var parentReference = parentKind.HasValue && parentName is not null
        ? new DataVaultParentReferenceExplain(parentKind.Value, parentName)
        : null;
    var properties = entityType
        .GetProperties()
        .Select(property => CreatePropertyExplain(property, tableIdentifier))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .ToArray();
    var primaryKey = entityType.FindPrimaryKey();
    var primaryKeyExplain = primaryKey is null
        ? new DataVaultKeyExplain("<none>", Array.Empty<string>())
        : CreateKeyExplain(primaryKey, tableIdentifier, producedName);
    var indexes = entityType
        .GetIndexes()
        .Select(index => CreateIndexExplain(index, tableIdentifier))
        .OrderBy(index => index.Name, StringComparer.Ordinal)
        .ToArray();
    var constraints = primaryKey is null
        ? Array.Empty<DataVaultConstraintExplain>()
        : [new DataVaultConstraintExplain(
            primaryKeyExplain.Name,
            DataVaultConstraintKind.PrimaryKey,
            primaryKeyExplain.PropertyNames) {
          ProducedName = primaryKeyExplain.ProducedName,
        }];

    return new DataVaultEntityExplain(
        tableName,
        tableKind,
        metadataName,
        parentReference,
        properties,
        primaryKeyExplain,
        indexes,
        constraints) {
      ProducedName = producedName,
    };
  }

  private static DataVaultPropertyExplain CreatePropertyExplain(
      IReadOnlyProperty property,
      StoreObjectIdentifier tableIdentifier) {
    var producedName = GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ?? property.Name;
    return new DataVaultPropertyExplain(
        property.GetColumnName(tableIdentifier) ?? producedName,
        GetAnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole),
        GetNullableAnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole),
        GetStringAnnotation(property, DataVaultAnnotationNames.MetadataName) ?? property.Name,
        GetNullableAnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal) ?? property.GetColumnOrder() ?? 0,
        GetAnnotationValue<DataVaultLogicalPropertyKind>(property, DataVaultAnnotationNames.ProviderLogicalPropertyKind),
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderProfile) ?? string.Empty,
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderStorageType) ?? property.GetColumnType() ?? string.Empty,
        GetAnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat)) {
      ClrTypeName = property.ClrType.FullName ?? property.ClrType.Name,
      IsNullable = property.IsNullable,
      ProducedName = producedName,
    };
  }

  private static DataVaultKeyExplain CreateKeyExplain(
      IReadOnlyKey key,
      StoreObjectIdentifier tableIdentifier,
      string producedTableName) {
    var producedName = GetStringAnnotation(key, DataVaultAnnotationNames.ProducedName) ??
        key.GetName() ??
        "Pk" + producedTableName;

    return new DataVaultKeyExplain(
        key.GetName() ?? producedName,
        key.Properties.Select(property => GetPhysicalColumnName(property, tableIdentifier)).ToArray()) {
      ProducedName = producedName,
    };
  }

  private static DataVaultIndexExplain CreateIndexExplain(
      IReadOnlyIndex index,
      StoreObjectIdentifier tableIdentifier) {
    var propertyNames = index.Properties
        .Select(property => GetPhysicalColumnName(property, tableIdentifier))
        .ToArray();
    var descendingPropertyNames = GetDescendingPropertyNames(index)
        .Select(property => GetPhysicalColumnName(property, tableIdentifier))
        .ToArray();
    var includedPropertyNames = GetIncludedPropertyNames(index, tableIdentifier);
    var producedName = GetStringAnnotation(index, DataVaultAnnotationNames.ProducedName) ??
        index.GetDatabaseName() ??
        string.Join("_", propertyNames);

    return new DataVaultIndexExplain(
        index.GetDatabaseName() ?? producedName,
        propertyNames,
        index.IsUnique,
        descendingPropertyNames,
        includedPropertyNames) {
      ProducedName = producedName,
    };
  }

  private static IEnumerable<IReadOnlyProperty> GetDescendingPropertyNames(IReadOnlyIndex index) {
    if (index.IsDescending is null) {
      yield break;
    }

    for (var ordinal = 0; ordinal < index.Properties.Count && ordinal < index.IsDescending.Count; ordinal++) {
      if (index.IsDescending[ordinal]) {
        yield return index.Properties[ordinal];
      }
    }
  }

  private static IReadOnlyList<string> GetIncludedPropertyNames(
      IReadOnlyIndex index,
      StoreObjectIdentifier tableIdentifier) {
    foreach (var annotationName in new[] { "SqlServer:Include", "Npgsql:IndexInclude" }) {
      var value = index.FindAnnotation(annotationName)?.Value;
      if (value is string[] stringArray) {
        return stringArray
            .Select(propertyName => GetPhysicalColumnName(index.DeclaringEntityType.FindProperty(propertyName), propertyName, tableIdentifier))
            .ToArray();
      }

      if (value is IEnumerable<string> stringValues) {
        return stringValues
            .Select(propertyName => GetPhysicalColumnName(index.DeclaringEntityType.FindProperty(propertyName), propertyName, tableIdentifier))
            .ToArray();
      }
    }

    var dataVaultValue = index.FindAnnotation(DataVaultInternalAnnotationNames.ProviderIncludedIndexPropertyNames)?.Value;
    if (dataVaultValue is string[] dataVaultStringArray) {
      return dataVaultStringArray
          .Select(propertyName => GetPhysicalColumnName(index.DeclaringEntityType.FindProperty(propertyName), propertyName, tableIdentifier))
          .ToArray();
    }

    if (dataVaultValue is IEnumerable<string> dataVaultStringValues) {
      return dataVaultStringValues
          .Select(propertyName => GetPhysicalColumnName(index.DeclaringEntityType.FindProperty(propertyName), propertyName, tableIdentifier))
          .ToArray();
    }

    return Array.Empty<string>();
  }

  private static string GetPhysicalColumnName(
      IReadOnlyProperty property,
      StoreObjectIdentifier tableIdentifier) {
    return property.GetColumnName(tableIdentifier) ??
        GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ??
        property.Name;
  }

  private static string GetPhysicalColumnName(
      IReadOnlyProperty? property,
      string fallbackName,
      StoreObjectIdentifier tableIdentifier) {
    return property is null
        ? fallbackName
        : GetPhysicalColumnName(property, tableIdentifier);
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> ValidateMetadataModel(DataVaultMetadataModel metadataModel) {
    var issues = new List<DataVaultDiagnosticsIssue>();
    AddDuplicateNameIssues(issues, "hub", metadataModel.Hubs.Select(hub => hub.Name), "metadata.hubs");
    AddDuplicateNameIssues(issues, "link", metadataModel.Links.Select(link => link.Name), "metadata.links");
    AddDuplicateNameIssues(
        issues,
        "point-in-time-table",
        metadataModel.PointInTimeTables.Select(pointInTime => pointInTime.Name),
        "metadata.pointInTimeTables");
    AddDuplicateNameIssues(issues, "bridge", metadataModel.Bridges.Select(bridge => bridge.Name), "metadata.bridges");
    AddDuplicateNameIssues(issues, "pit", metadataModel.Pits.Select(pit => pit.Name), "metadata.pits");

    var hubNames = metadataModel.Hubs.Select(hub => hub.Name).ToHashSet(StringComparer.Ordinal);
    var linkNames = metadataModel.Links.Select(link => link.Name).ToHashSet(StringComparer.Ordinal);
    var satelliteKeys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var satellite in metadataModel.Satellites) {
      var key = satellite.Parent.Kind + ":" + satellite.Parent.Name + ":" + satellite.Name;
      if (!satelliteKeys.Add(key)) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "duplicate-logical-name",
            "Duplicate satellite metadata logical name '" + satellite.Name + "' under " + FormatParent(satellite.Parent) + ".",
            "metadata.satellites"));
      }
    }

    foreach (var link in metadataModel.Links) {
      foreach (var participant in link.Participants) {
        if (!hubNames.Contains(participant.HubReference.Name)) {
          issues.Add(MissingReferenceIssue(
              "link",
              link.Name,
              "hub",
              participant.HubReference.Name,
              "metadata.links"));
        }
      }
    }

    foreach (var satellite in metadataModel.Satellites) {
      if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Hub && !hubNames.Contains(satellite.Parent.Name)) {
        issues.Add(MissingReferenceIssue(
            "satellite",
            satellite.Name,
            "hub",
            satellite.Parent.Name,
            "metadata.satellites"));
      }
      else if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Link && !linkNames.Contains(satellite.Parent.Name)) {
        issues.Add(MissingReferenceIssue(
            "satellite",
            satellite.Name,
            "link",
            satellite.Parent.Name,
            "metadata.satellites"));
      }
    }

    foreach (var bridge in metadataModel.Bridges) {
      if (!hubNames.Contains(bridge.SourceHubReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "hub", bridge.SourceHubReference.Name, "metadata.bridges"));
      }

      if (!hubNames.Contains(bridge.TargetHubReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "hub", bridge.TargetHubReference.Name, "metadata.bridges"));
      }

      if (!linkNames.Contains(bridge.LinkReference.Name)) {
        issues.Add(MissingReferenceIssue("bridge", bridge.Name, "link", bridge.LinkReference.Name, "metadata.bridges"));
      }
    }

    foreach (var pit in metadataModel.Pits) {
      if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Hub && !hubNames.Contains(pit.Parent.Name)) {
        issues.Add(MissingReferenceIssue("pit", pit.Name, "hub", pit.Parent.Name, "metadata.pits"));
      }
      else if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Link && !linkNames.Contains(pit.Parent.Name)) {
        issues.Add(MissingReferenceIssue("pit", pit.Name, "link", pit.Parent.Name, "metadata.pits"));
      }
    }

    return issues;
  }

  private static IEnumerable<DataVaultDiagnosticsIssue> ValidateProviderMappings(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var issues = new List<DataVaultDiagnosticsIssue>();
    var requiredKinds = GetRequiredLogicalPropertyKinds(metadataModel);
    foreach (var requiredKind in requiredKinds.OrderBy(kind => kind)) {
      try {
        providerCapabilities.GetRequiredTypeMapping(requiredKind);
      }
      catch (NotSupportedException exception) {
        issues.Add(new DataVaultDiagnosticsIssue(
            DataVaultDiagnosticsIssueSeverity.Error,
            "missing-provider-type-mapping",
            exception.Message,
            "capability-profile." + providerCapabilities.ProfileName));
      }
    }

    return issues;
  }

  private static IReadOnlySet<DataVaultLogicalPropertyKind> GetRequiredLogicalPropertyKinds(
      DataVaultMetadataModel metadataModel) {
    var kinds = new HashSet<DataVaultLogicalPropertyKind>
    {
        DataVaultLogicalPropertyKind.HashKey,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        DataVaultLogicalPropertyKind.RecordSource,
    };

    if (metadataModel.Hubs.Any(hub => hub.BusinessKeyColumns.Count > 0)) {
      kinds.Add(DataVaultLogicalPropertyKind.BusinessKey);
    }

    if (metadataModel.Links.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.ParticipantReference);
    }

    if (metadataModel.Satellites.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.HashDiff);
      kinds.Add(DataVaultLogicalPropertyKind.PayloadText);
      if (metadataModel.Satellites.Any(satellite => satellite.DrivingKeyNames.Count > 0)) {
        kinds.Add(DataVaultLogicalPropertyKind.DrivingKey);
      }
    }

    if (metadataModel.Bridges.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.ParticipantReference);
      if (metadataModel.Bridges.Any(bridge => bridge.Kind == DataVaultBridgeKind.Hierarchy)) {
        kinds.Add(DataVaultLogicalPropertyKind.BridgeDepth);
      }
    }

    if (metadataModel.Pits.Any()) {
      kinds.Add(DataVaultLogicalPropertyKind.SatelliteSnapshotReference);
    }

    return kinds;
  }

  private static void AddDuplicateNameIssues(
      ICollection<DataVaultDiagnosticsIssue> issues,
      string kind,
      IEnumerable<string> names,
      string path) {
    foreach (var group in names.GroupBy(name => name, StringComparer.Ordinal).Where(group => group.Count() > 1)) {
      issues.Add(new DataVaultDiagnosticsIssue(
          DataVaultDiagnosticsIssueSeverity.Error,
          "duplicate-logical-name",
          "Duplicate " + kind + " metadata logical name '" + group.Key + "'.",
          path));
    }
  }

  private static DataVaultDiagnosticsIssue MissingReferenceIssue(
      string sourceKind,
      string sourceName,
      string targetKind,
      string targetName,
      string path) {
    return new DataVaultDiagnosticsIssue(
        DataVaultDiagnosticsIssueSeverity.Error,
        "missing-reference",
        sourceKind + " metadata '" + sourceName + "' references missing " + targetKind + " metadata '" + targetName + "'.",
        path);
  }

  private static IReadOnlyList<DataVaultSaveStrategyFallbackCause> DistinctFallbackCauses(
      IEnumerable<DataVaultSaveStrategyFallbackCause> causes) {
    var values = new List<DataVaultSaveStrategyFallbackCause>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var cause in causes) {
      var key = cause.Kind + "\u001f" + cause.Message;
      if (keys.Add(key)) {
        values.Add(cause);
      }
    }

    return values;
  }

  private static IReadOnlyList<DataVaultReadStrategyFallbackCause> DistinctFallbackCauses(
      IEnumerable<DataVaultReadStrategyFallbackCause> causes) {
    var values = new List<DataVaultReadStrategyFallbackCause>();
    var keys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var cause in causes) {
      var key = cause.Kind + "" + cause.Message;
      if (keys.Add(key)) {
        values.Add(cause);
      }
    }

    return values;
  }

  private static DataVaultProviderTypeMapping GetLoadTimestampMapping(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return GetTypeMappingOrMissing(providerCapabilities, DataVaultLogicalPropertyKind.LoadTimestamp);
  }

  private static DataVaultProviderTypeMapping GetSatelliteSnapshotReferenceMapping(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return GetTypeMappingOrMissing(providerCapabilities, DataVaultLogicalPropertyKind.SatelliteSnapshotReference);
  }

  private static DataVaultProviderTypeMapping GetTypeMappingOrMissing(
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLogicalPropertyKind logicalPropertyKind) {
    try {
      return providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind);
    }
    catch (NotSupportedException) {
      return new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(DateTimeOffset),
          "<missing>",
          DataVaultProviderValueFormat.Text);
    }
  }

  private static IReadOnlyList<DataVaultProviderTypeMappingExplain> CreateTypeMappingExplain(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    return providerCapabilities.TypeMappings
        .OrderBy(mapping => mapping.LogicalPropertyKind)
        .Select(mapping => new DataVaultProviderTypeMappingExplain(
            mapping.LogicalPropertyKind,
            mapping.ModelClrType.FullName ?? mapping.ModelClrType.Name,
            mapping.NativeStoreType,
            mapping.ValueFormat))
        .ToArray();
  }

  private static string FormatParent(DataVaultMetadataReference parent) {
    return parent.Kind.ToString().ToLowerInvariant() + " '" + parent.Name + "'";
  }

  private static int GetEntityKindSortKey(DataVaultTableKind tableKind) {
    return tableKind switch {
      DataVaultTableKind.Hub => 0,
      DataVaultTableKind.Link => 1,
      DataVaultTableKind.Satellite => 2,
      DataVaultTableKind.Bridge => 3,
      DataVaultTableKind.Pit => 4,
      DataVaultTableKind.PointInTime => 5,
      _ => 99,
    };
  }

  private static string? GetStringAnnotation(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value as string;
  }

  private static T GetAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : default;
  }

  private static T? GetNullableAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : null;
  }

  private readonly record struct SaveStrategyRegistration(
      IDataVaultProviderSaveStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct ReadStrategyRegistration(
      IDataVaultProviderReadStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct PitReadStrategyRegistration(
      IDataVaultProviderPitReadStrategy Strategy,
      int RegistrationOrdinal);

  private readonly record struct BridgeReadStrategyRegistration(
      IDataVaultProviderBridgeReadStrategy Strategy,
      int RegistrationOrdinal);
}

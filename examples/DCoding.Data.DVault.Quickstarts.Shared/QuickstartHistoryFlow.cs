using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Quickstarts.Shared;

public sealed class QuickstartVaultContext(DbContextOptions<QuickstartVaultContext> options) : DbContext(options) {
}

public static class QuickstartHistoryFlow {
  public const string CustomerHubName = "Customer";
  public const string CustomerIdBusinessKeyName = "Customer Id";
  public const string CustomerProfileSatelliteName = "CustomerProfile";
  public const string ProfileNamePayloadName = "Profile Name";
  public const string CustomerStatusPayloadName = "Customer Status";

  private const string CustomerId = "C-100";
  private const string InitialRecordSource = "crm-import";
  private const string ChangedRecordSource = "crm-change";
  private const string InitialProfileName = "Alice Adams";
  private const string ChangedProfileName = "Alice Baker";
  private const string InitialCustomerStatus = "prospect";
  private const string ChangedCustomerStatus = "active";

  private static readonly DateTimeOffset InitialLoadTimestamp = new(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset ChangedLoadTimestamp = new(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);

  private static readonly DataVaultHubMetadata CustomerHub = new(CustomerHubName, [CustomerIdBusinessKeyName]);
  private static readonly DataVaultSatelliteMetadata CustomerProfile = new(
      CustomerProfileSatelliteName,
      CustomerHub.ToReference(),
      [ProfileNamePayloadName, CustomerStatusPayloadName]);

  public static readonly DataVaultMetadataModel MetadataModel = new(
      [CustomerHub],
      [],
      [CustomerProfile]);

  public static async Task RunAsync(
      IServiceProvider serviceProvider,
      string providerName,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(serviceProvider);
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<QuickstartVaultContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var readService = scope.ServiceProvider.GetRequiredService<IDataVaultReadService>();
    var diagnosticsService = scope.ServiceProvider.GetRequiredService<IDataVaultDiagnosticsService>();
    var readDiagnosticsService = scope.ServiceProvider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

    var hubSaveRequest = new DataVaultRegistrySaveRequest(
        InitialLoadTimestamp,
        InitialRecordSource,
        [
            new(CustomerHubName, [new(CustomerIdBusinessKeyName, CustomerId)]),
        ],
        []);
    var hubResult = await saveService.SaveAsync(
        context,
        hubSaveRequest,
        cancellationToken).ConfigureAwait(false);
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, CustomerHubName);

    var firstProfileSaveRequest = new DataVaultRegistrySaveRequest(
        InitialLoadTimestamp,
        InitialRecordSource,
        [],
        [],
        [
            new(
                DataVaultMetadataReference.Hub(CustomerHubName),
                CustomerProfileSatelliteName,
                customerHashKey,
                [
                    new(ProfileNamePayloadName, InitialProfileName),
                    new(CustomerStatusPayloadName, InitialCustomerStatus),
                ],
                "customer-profile-import"),
        ]);
    var firstProfileResult = await saveService.SaveAsync(
        context,
        firstProfileSaveRequest,
        cancellationToken).ConfigureAwait(false);

    var secondProfileSaveRequest = new DataVaultRegistrySaveRequest(
        ChangedLoadTimestamp,
        ChangedRecordSource,
        [],
        [],
        [
            new(
                DataVaultMetadataReference.Hub(CustomerHubName),
                CustomerProfileSatelliteName,
                customerHashKey,
                [
                    new(ProfileNamePayloadName, ChangedProfileName),
                    new(CustomerStatusPayloadName, ChangedCustomerStatus),
                ],
                "customer-profile-change"),
        ]);
    var saveDiagnostics = diagnosticsService.Analyze(context, secondProfileSaveRequest);
    var secondProfileResult = await saveService.SaveAsync(
        context,
        secondProfileSaveRequest,
        cancellationToken).ConfigureAwait(false);

    var latestReadRequest = new DataVaultRegistryLatestSatelliteReadRequest(
        DataVaultMetadataReference.Hub(CustomerHubName),
        CustomerProfileSatelliteName,
        [customerHashKey]);
    var asOfReadRequest = new DataVaultRegistryLatestSatelliteReadRequest(
        DataVaultMetadataReference.Hub(CustomerHubName),
        CustomerProfileSatelliteName,
        [customerHashKey],
        InitialLoadTimestamp);
    var latestReadDiagnostics = readDiagnosticsService.Analyze(context, latestReadRequest);
    var asOfReadDiagnostics = readDiagnosticsService.Analyze(context, asOfReadRequest);

    var latestRows = await readService.ReadLatestSatelliteAsync(
        context,
        latestReadRequest,
        ProjectProfile,
        cancellationToken).ConfigureAwait(false);
    var asOfRows = await readService.ReadLatestSatelliteAsync(
        context,
        asOfReadRequest,
        ProjectProfile,
        cancellationToken).ConfigureAwait(false);

    var latest = latestRows.Single();
    var asOf = asOfRows.Single();

    Console.WriteLine("DVault " + providerName + " quickstart completed.");
    Console.WriteLine("Scenario: one Customer hub and one CustomerProfile satellite with two ordered profile-state saves.");
    Console.WriteLine(
        "Load timestamps: " +
        InitialLoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
        " -> " +
        ChangedLoadTimestamp.ToString("O", CultureInfo.InvariantCulture));
    Console.WriteLine("Record sources: " + InitialRecordSource + " -> " + ChangedRecordSource);
    Console.WriteLine(
        "Rows written: hub=" +
        hubResult.RowsWritten.ToString(CultureInfo.InvariantCulture) +
        ", first profile=" +
        firstProfileResult.RowsWritten.ToString(CultureInfo.InvariantCulture) +
        ", second profile=" +
        secondProfileResult.RowsWritten.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine(
        "Latest typed read: payload fields=" +
        FormatPayloadPresence(latest) +
        ", load timestamp=" +
        latest.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture));
    Console.WriteLine(
        "As-of typed read at " +
        InitialLoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
        ": payload fields=" +
        FormatPayloadPresence(asOf) +
        ", load timestamp=" +
        asOf.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture));
    Console.WriteLine(
        "Save diagnostics: status=" +
        saveDiagnostics.SaveStrategy.Status +
        ", selected=" +
        FormatSelectedStrategy(saveDiagnostics.SaveStrategy.SelectedStrategyName) +
        ", fallback=" +
        FormatFallbackPresence(saveDiagnostics.SaveStrategy.FallbackCauses.Count));
    Console.WriteLine(
        "Latest read diagnostics: status=" +
        latestReadDiagnostics.ReadStrategy.Status +
        ", selected=" +
        FormatSelectedStrategy(latestReadDiagnostics.ReadStrategy.SelectedStrategyName) +
        ", shape=" +
        FormatReadShape(latestReadDiagnostics) +
        ", fallback=" +
        FormatFallbackPresence(latestReadDiagnostics.ReadStrategy.FallbackCauses.Count));
    Console.WriteLine(
        "As-of read diagnostics: status=" +
        asOfReadDiagnostics.ReadStrategy.Status +
        ", selected=" +
        FormatSelectedStrategy(asOfReadDiagnostics.ReadStrategy.SelectedStrategyName) +
        ", shape=" +
        FormatReadShape(asOfReadDiagnostics) +
        ", fallback=" +
        FormatFallbackPresence(asOfReadDiagnostics.ReadStrategy.FallbackCauses.Count));
  }

  private static CustomerProfileRead ProjectProfile(DataVaultSatelliteProjectionRow row) {
    return new CustomerProfileRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("HashDiff"),
        row.RequiredDateTimeOffset("LoadTimestamp"),
        row.RequiredString("RecordSource"),
        row.RequiredString(ProfileNamePayloadName),
        row.RequiredString(CustomerStatusPayloadName));
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords.Single(record =>
        record.Kind == kind &&
        string.Equals(record.MetadataName, metadataName, StringComparison.Ordinal)).HashKey;
  }

  private static string FormatPayloadPresence(CustomerProfileRead row) {
    return string.IsNullOrWhiteSpace(row.ProfileName) || string.IsNullOrWhiteSpace(row.CustomerStatus)
        ? "missing"
        : "profile name and customer status projected";
  }

  private static string FormatSelectedStrategy(string? selectedStrategyName) {
    return string.IsNullOrWhiteSpace(selectedStrategyName) ? "none" : selectedStrategyName;
  }

  private static string FormatFallbackPresence(int fallbackCauseCount) {
    return fallbackCauseCount == 0 ? "none" : "present";
  }

  private static string FormatReadShape(DataVaultDiagnosticsResult diagnostics) {
    if (diagnostics.ReadShape is not { } readShape) {
      return "none";
    }

    if (readShape.Satellite is null) {
      return readShape.Kind.ToString();
    }

    return readShape.Kind + "/" + readShape.Satellite.Semantics;
  }

  private sealed record CustomerProfileRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string ProfileName,
      string CustomerStatus);
}

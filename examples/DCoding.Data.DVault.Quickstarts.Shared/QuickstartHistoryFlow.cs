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

  private const string RecordSource = "quickstart";

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

    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

    var firstLoadTimestamp = DateTimeOffset.UtcNow;
    var secondLoadTimestamp = firstLoadTimestamp.AddMinutes(5);
    var customerId = "C-" + firstLoadTimestamp.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            RecordSource,
            [
                new(CustomerHubName, [new(CustomerIdBusinessKeyName, customerId)]),
            ],
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, CustomerHubName);

    var firstProfileResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            RecordSource,
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub(CustomerHubName),
                    CustomerProfileSatelliteName,
                    customerHashKey,
                    [
                        new(ProfileNamePayloadName, "Alice Adams"),
                        new(CustomerStatusPayloadName, "Prospect"),
                    ],
                    "customer-profile-prospect"),
            ]),
        cancellationToken).ConfigureAwait(false);

    var secondProfileResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            secondLoadTimestamp,
            RecordSource,
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub(CustomerHubName),
                    CustomerProfileSatelliteName,
                    customerHashKey,
                    [
                        new(ProfileNamePayloadName, "Alice Baker"),
                        new(CustomerStatusPayloadName, "Active"),
                    ],
                    "customer-profile-active"),
            ]),
        cancellationToken).ConfigureAwait(false);

    var latestRows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub(CustomerHubName),
            CustomerProfileSatelliteName,
            [customerHashKey]),
        ProjectProfile,
        cancellationToken).ConfigureAwait(false);
    var asOfRows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub(CustomerHubName),
            CustomerProfileSatelliteName,
            [customerHashKey],
            firstLoadTimestamp),
        ProjectProfile,
        cancellationToken).ConfigureAwait(false);

    var latest = latestRows.Single();
    var asOf = asOfRows.Single();

    Console.WriteLine("DVault " + providerName + " quickstart completed.");
    Console.WriteLine("Customer Id: " + customerId);
    Console.WriteLine("Customer hash key: " + customerHashKey);
    Console.WriteLine(
        "Rows written: hub=" +
        hubResult.RowsWritten.ToString(CultureInfo.InvariantCulture) +
        ", first profile=" +
        firstProfileResult.RowsWritten.ToString(CultureInfo.InvariantCulture) +
        ", second profile=" +
        secondProfileResult.RowsWritten.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine(
        "Latest profile: " +
        latest.ProfileName +
        " / " +
        latest.CustomerStatus +
        " at " +
        latest.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture));
    Console.WriteLine(
        "As-of profile at " +
        firstLoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
        ": " +
        asOf.ProfileName +
        " / " +
        asOf.CustomerStatus +
        " at " +
        asOf.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture));
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

  private sealed record CustomerProfileRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string ProfileName,
      string CustomerStatus);
}

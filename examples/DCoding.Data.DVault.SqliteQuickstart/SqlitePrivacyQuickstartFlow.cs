using System.Data;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.SqliteQuickstart;

public static class SqlitePrivacyQuickstartFlow {
  public const string CustomerProfileEmailEncryptedPayloadAlias = "CustomerProfileEmailEncrypted";

  private const string CustomerBusinessKey = "C-100";
  private const string DemoEmailAddress = "customer-profile@example.test";

  public static async Task RunAsync(
      IServiceProvider serviceProvider,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(serviceProvider);

    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SqliteQuickstartVaultContext>();

    var row = new CustomerProfilePrivacyProofRow {
      CustomerBusinessKey = CustomerBusinessKey,
      EmailAddress = DemoEmailAddress,
    };

    context.CustomerProfilePrivacyProofs.Add(row);
    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    context.ChangeTracker.Clear();

    var storedProviderValue = await ReadStoredEmailAddressAsync(
        context,
        row.Id,
        cancellationToken).ConfigureAwait(false);
    var roundTripped = await context.CustomerProfilePrivacyProofs
        .AsNoTracking()
        .SingleAsync(item => item.Id == row.Id, cancellationToken)
        .ConfigureAwait(false);

    Console.WriteLine(
        "Privacy proof: alias=" +
        CustomerProfileEmailEncryptedPayloadAlias +
        ", mapped property=EmailAddress" +
        ", provider value encrypted=" +
        FormatBoolean(IsDemoEncryptedProviderValue(storedProviderValue)) +
        ", decrypted round trip=" +
        FormatBoolean(string.Equals(roundTripped.EmailAddress, DemoEmailAddress, StringComparison.Ordinal)));
    Console.WriteLine(
        "Privacy boundary: conversion uses the opt-in EF Core value-converter seam; DVault save/read services remain caller-driven and provider-neutral.");
  }

  private static async Task<string> ReadStoredEmailAddressAsync(
      DbContext context,
      long id,
      CancellationToken cancellationToken) {
    var connection = context.Database.GetDbConnection();
    var shouldClose = connection.State != ConnectionState.Open;
    if (shouldClose) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    try {
      await using var command = connection.CreateCommand();
      command.CommandText =
          "SELECT \"EmailAddress\" FROM \"CustomerProfilePrivacyProof\" WHERE \"Id\" = $id";
      var idParameter = command.CreateParameter();
      idParameter.ParameterName = "$id";
      idParameter.Value = id;
      command.Parameters.Add(idParameter);

      var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
      return value as string
          ?? throw new InvalidOperationException("The SQLite privacy proof row did not return a provider value.");
    }
    finally {
      if (shouldClose) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static bool IsDemoEncryptedProviderValue(string value) {
    return value.StartsWith(
        "demo-encrypted:" + CustomerProfileEmailEncryptedPayloadAlias + ":",
        StringComparison.Ordinal);
  }

  private static string FormatBoolean(bool value) {
    return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
  }
}

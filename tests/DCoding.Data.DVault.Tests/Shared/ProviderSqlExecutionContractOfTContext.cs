using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Tests.Shared;

public sealed class ProviderSqlExecutionContract<TContext>
    where TContext : DbContext {
  private const string HubCustomerTableName = "HubCustomer";
  private const string CustomerHashKeyColumnName = "CustomerHashKey";
  private const string CustomerIdColumnName = "CustomerId";
  private const string RecordSource = "provider-contract";

  private static readonly DateTimeOffset LoadTimestamp =
      new(2026, 5, 4, 0, 0, 0, TimeSpan.Zero);

  private readonly Func<TContext> _createContext;
  private readonly Action<TContext> _createUnsupportedTrackedChange;
  private readonly IDataVaultProviderSaveStrategy _providerSaveStrategy;
  private readonly IDataVaultSaveService _saveService;

  public ProviderSqlExecutionContract(
      Func<TContext> createContext,
      IDataVaultSaveService saveService,
      IDataVaultProviderSaveStrategy providerSaveStrategy,
      Action<TContext> createUnsupportedTrackedChange) {
    ArgumentNullException.ThrowIfNull(createContext);
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(providerSaveStrategy);
    ArgumentNullException.ThrowIfNull(createUnsupportedTrackedChange);

    _createContext = createContext;
    _saveService = saveService;
    _providerSaveStrategy = providerSaveStrategy;
    _createUnsupportedTrackedChange = createUnsupportedTrackedChange;
  }

  public async Task PersistsValuesThroughParametersAsync(
      CancellationToken cancellationToken = default) {
    await using var context = _createContext();
    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

    var customerId = "C-100'); SELECT 'not-sql";
    var result = await _saveService.SaveAsync(
        context,
        CreateCustomerHubRequest(customerId),
        cancellationToken).ConfigureAwait(false);
    var hashKey = RequireSingleSavedHub(result);
    var row = RequireSingle(
        await LoadCustomerHubRowsAsync(context, cancellationToken).ConfigureAwait(false),
        "Provider SQL execution contract expected one inserted customer hub row.");

    RequireEqual(
        customerId,
        RequireValue<string>(row, CustomerIdColumnName),
        "Provider SQL execution contract expected the customer business key to be stored as a bound parameter value.");
    RequireEqual(
        hashKey,
        RequireValue<string>(row, CustomerHashKeyColumnName),
        "Provider SQL execution contract expected the generated hash key to be stored as a bound parameter value.");
  }

  public async Task ParticipatesInCurrentTransactionAsync(
      CancellationToken cancellationToken = default) {
    await using var context = _createContext();
    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

    var result = await _saveService.SaveAsync(
        context,
        CreateCustomerHubRequest("C-TX"),
        cancellationToken).ConfigureAwait(false);

    RequireEqual(
        1,
        result.RowsWritten,
        "Provider SQL execution contract expected the transaction-scoped save to insert one row.");
    RequireSingle(
        await LoadCustomerHubRowsAsync(context, cancellationToken).ConfigureAwait(false),
        "Provider SQL execution contract expected the inserted row to be visible inside the current transaction.");

    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

    await using var verificationContext = _createContext();
    var rowsAfterRollback = await LoadCustomerHubRowsAsync(verificationContext, cancellationToken).ConfigureAwait(false);
    RequireEqual(
        0,
        rowsAfterRollback.Count,
        "Provider SQL execution contract expected current transaction rollback to remove the optimized SQL row.");
  }

  public async Task PropagatesCancellationTokenAsync(
      CancellationToken cancellationToken = default) {
    await using var context = _createContext();
    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    using var canceled = new CancellationTokenSource();
    canceled.Cancel();

    try {
      await _saveService.SaveAsync(
          context,
          CreateCustomerHubRequest("C-CANCEL"),
          canceled.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException) {
      var rows = await LoadCustomerHubRowsAsync(context, cancellationToken).ConfigureAwait(false);
      RequireEqual(
          0,
          rows.Count,
          "Provider SQL execution contract expected cancellation to prevent optimized SQL row insertion.");
      return;
    }

    throw new InvalidOperationException(
        "Provider SQL execution contract expected the optimized SQL boundary to observe a canceled token.");
  }

  public async Task DeclinesUnsupportedTrackedChangesAsync(
      CancellationToken cancellationToken = default) {
    await using var context = _createContext();
    await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

    var request = CreateCustomerHubRequest("C-DIRTY");
    RequireTrue(
        _providerSaveStrategy.CanSave(context, [request]),
        "Provider SQL execution contract expected a clean context to be accepted by the optimized SQL strategy.");

    _createUnsupportedTrackedChange(context);

    RequireFalse(
        _providerSaveStrategy.CanSave(context, [request]),
        "Provider SQL execution contract expected pending EF tracked changes to make the optimized SQL strategy decline.");
  }

  private static DataVaultSaveRequest CreateCustomerHubRequest(string customerId) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        LoadTimestamp,
        RecordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", customerId)])],
        []);
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> LoadCustomerHubRowsAsync(
      DbContext context,
      CancellationToken cancellationToken) {
    return await context
        .Set<Dictionary<string, object>>(HubCustomerTableName)
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
  }

  private static string RequireSingleSavedHub(DataVaultSaveResult result) {
    RequireEqual(
        1,
        result.RowsWritten,
        "Provider SQL execution contract expected one optimized SQL row insertion.");
    var record = RequireSingle(
        result.SavedRecords,
        "Provider SQL execution contract expected one saved hub record.");

    RequireEqual(
        DataVaultTableKind.Hub,
        record.Kind,
        "Provider SQL execution contract expected the saved record to describe a hub.");
    RequireEqual(
        "Customer",
        record.MetadataName,
        "Provider SQL execution contract expected the saved record to describe the Customer metadata.");
    RequireEqual(
        HubCustomerTableName,
        record.TableName,
        "Provider SQL execution contract expected the saved record to describe the Customer hub table.");

    return record.HashKey;
  }

  private static T RequireSingle<T>(IReadOnlyList<T> values, string message) {
    RequireEqual(1, values.Count, message);

    return values[0];
  }

  private static T RequireValue<T>(IReadOnlyDictionary<string, object> row, string columnName) {
    if (!row.TryGetValue(columnName, out var value)) {
      throw new InvalidOperationException(
          "Provider SQL execution contract expected result row column '" + columnName + "'.");
    }

    if (value is T typedValue) {
      return typedValue;
    }

    throw new InvalidOperationException(
        "Provider SQL execution contract expected result row column '" +
        columnName +
        "' to be " +
        typeof(T).Name +
        ".");
  }

  private static void RequireTrue(bool value, string message) {
    if (!value) {
      throw new InvalidOperationException(message);
    }
  }

  private static void RequireFalse(bool value, string message) {
    if (value) {
      throw new InvalidOperationException(message);
    }
  }

  private static void RequireEqual<T>(T expected, T actual, string message) {
    if (!EqualityComparer<T>.Default.Equals(expected, actual)) {
      throw new InvalidOperationException(
          message +
          " Expected '" +
          expected +
          "', actual '" +
          actual +
          "'.");
    }
  }
}

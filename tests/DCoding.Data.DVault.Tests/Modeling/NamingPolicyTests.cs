using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Tests.Modeling;

internal static class NamingPolicyTests {
  internal static int Run() {
    var tests = new TestCase[]
    {
            new("default naming policy path builds deterministic names", DefaultNamingPolicyPathBuildsDeterministicNames),
            new("default naming policy path applies normalization and column collision rules", DefaultNamingPolicyPathAppliesNormalizationAndColumnCollisionRules),
            new("index and constraint names keep produced technical column names", IndexAndConstraintNamesKeepProducedTechnicalColumnNames),
            new("link declarations can fall back to participant order", LinkDeclarationsCanFallBackToParticipantOrder),
            new("point-in-time declarations build deterministic names and fields", PointInTimeDeclarationsBuildDeterministicNamesAndFields),
            new("point-in-time declarations validate references", PointInTimeDeclarationsValidateReferences),
            new("custom naming policy overrides each v1 name family", CustomNamingPolicyOverridesEachV1NameFamily),
            new("options can be configured fluently", OptionsCanBeConfiguredFluently),
    };

    var failures = 0;
    foreach (var test in tests) {
      try {
        test.Run();
        Console.WriteLine("PASS " + test.Name);
      }
      catch (Exception exception) {
        failures++;
        Console.Error.WriteLine("FAIL " + test.Name + ": " + exception.Message);
      }
    }

    return failures == 0 ? 0 : 1;
  }

  private static void DefaultNamingPolicyPathBuildsDeterministicNames() {
    var model = CreateModel();
    var repeatedModel = CreateModel();

    var hub = Single(model.Tables, table => table.Kind == DataVaultTableKind.Hub);
    var satellite = Single(model.Tables, table => table.Kind == DataVaultTableKind.Satellite);
    var link = Single(model.Tables, table => table.Kind == DataVaultTableKind.Link);
    var pointInTime = Single(model.Tables, table => table.Kind == DataVaultTableKind.PointInTime);

    AllProducedNamesArePresent(model);
    Contains(hub.Columns, column => column.Kind == DataVaultColumnKind.Technical);
    Contains(satellite.Columns, column => column.Kind == DataVaultColumnKind.Technical);
    Contains(link.Columns, column => column.Kind == DataVaultColumnKind.Technical);
    Contains(model.Tables.SelectMany(table => table.Indexes), index => !string.IsNullOrWhiteSpace(index.Name));
    Contains(model.Tables.SelectMany(table => table.Constraints), constraint => !string.IsNullOrWhiteSpace(constraint.Name));

    var producedNames = ProducedNames(model);
    Equal(string.Join("\n", producedNames), string.Join("\n", ProducedNames(repeatedModel)));
    Equal("HubCustomer", hub.Name);
    SequenceEqual(["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"], hub.Columns.Select(column => column.Name));
    SequenceEqual(["IxHubCustomerBusinessKeyCustomerId"], hub.Indexes.Select(index => index.Name));
    SequenceEqual(["PkHubCustomerCustomerHashKey"], hub.Constraints.Select(constraint => constraint.Name));

    Equal("SatCustomerContact", satellite.Name);
    SequenceEqual(
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        satellite.Columns.Select(column => column.Name));
    SequenceEqual(["IxSatCustomerContactSatelliteParentCustomerHashKey"], satellite.Indexes.Select(index => index.Name));
    SequenceEqual(["PkSatCustomerContactCustomerHashKeyLoadTimestamp"], satellite.Constraints.Select(constraint => constraint.Name));

    Equal("LinkCustomerOrder", link.Name);
    SequenceEqual(
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        link.Columns.Select(column => column.Name));
    SequenceEqual(["IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"], link.Indexes.Select(index => index.Name));
    SequenceEqual(["PkLinkCustomerOrderCustomerOrderHashKey"], link.Constraints.Select(constraint => constraint.Name));

    Equal("PitCustomerHistory", pointInTime.Name);
    SequenceEqual(
        ["CustomerHashKey", "PitLoadTimestamp", "ContactLoadTimestamp"],
        pointInTime.Columns.Select(column => column.Name));
    SequenceEqual(
        [
            DataVaultColumnKind.PointInTime,
                DataVaultColumnKind.PointInTime,
                DataVaultColumnKind.PointInTime,
            ],
        pointInTime.Columns.Select(column => column.Kind));
    SequenceEqual(Array.Empty<string>(), pointInTime.Indexes.Select(index => index.Name));
    SequenceEqual(["PkPitCustomerHistoryCustomerHashKeyPitLoadTimestamp"], pointInTime.Constraints.Select(constraint => constraint.Name));
    SequenceEqual(
        [
            "HubHashKeyReference:CustomerHashKey::0",
                "LoadTimestamp:PitLoadTimestamp::1",
                "SatelliteSnapshotLoadTimestampReference:ContactLoadTimestamp:Contact:",
            ],
        pointInTime.PointInTimeFields.Select(field =>
            field.Kind + ":" + field.Name + ":" + field.SatelliteName + ":" + field.KeyOrdinal));
  }

  private static void DefaultNamingPolicyPathAppliesNormalizationAndColumnCollisionRules() {
    var model = DataVaultModel.Create(modelBuilder => {
      modelBuilder.Hub("Customers", hub => {
        hub.BusinessKey("hash diff");
        hub.BusinessKey("customer hash key");
        hub.BusinessKey("customer id");
        hub.BusinessKey("customer-id");
        hub.Satellite("Contact", satellite => {
          satellite.Payload("load_timestamp");
          satellite.Payload("record-source");
          satellite.Payload("email address");
          satellite.Payload("email-address");
        });
      });
    });

    var hub = Single(model.Tables, table => table.Kind == DataVaultTableKind.Hub);
    var satellite = Single(model.Tables, table => table.Kind == DataVaultTableKind.Satellite);

    Equal("HubCustomer", hub.Name);
    SequenceEqual(
        [
            "CustomerHashKey",
                "LoadTimestamp",
                "RecordSource",
                "HashDiffValue",
                "CustomerHashKeyValue",
                "CustomerId",
                "CustomerId2",
            ],
        hub.Columns.Select(column => column.Name));
    SequenceEqual(
        [
            "CustomerHashKey",
                "HashDiff",
                "LoadTimestamp",
                "RecordSource",
                "LoadTimestampValue",
                "RecordSourceValue",
                "EmailAddress",
                "EmailAddress2",
            ],
        satellite.Columns.Select(column => column.Name));
  }

  private static void IndexAndConstraintNamesKeepProducedTechnicalColumnNames() {
    var policy = DefaultDataVaultNamingPolicy.Instance;

    Equal(
        "IxSatCustomerContactSatelliteParentHashDiff",
        policy.GetIndexName(
            new DataVaultIndexNameContext(
                DataVaultIndexKind.SatelliteParent,
                "SatCustomerContact",
                ["HashDiff"],
                IsUnique: false)));
    Equal(
        "PkSatCustomerContactHashDiff",
        policy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                "SatCustomerContact",
                ["HashDiff"])));
  }

  private static void LinkDeclarationsCanFallBackToParticipantOrder() {
    var model = DataVaultModel.Create(modelBuilder => {
      modelBuilder.Link(["Customers", "Orders"]);
    });

    var link = Single(model.Tables, table => table.Kind == DataVaultTableKind.Link);

    Equal("LinkCustomerOrder", link.Name);
  }

  private static void PointInTimeDeclarationsBuildDeterministicNamesAndFields() {
    var model = DataVaultModel.Create(modelBuilder => {
      modelBuilder.Hub("Customer", hub => {
        hub.BusinessKey("Customer Id");
        hub.Satellite("Contact", satellite => satellite.Payload("Email Address"));
        hub.Satellite("Preferences", satellite => satellite.Payload("Language Code"));
      });
      modelBuilder.PointInTime("Customer History", "Customer", pointInTime => {
        pointInTime.Satellite("Contact");
        pointInTime.Satellite("Preferences");
      });
    });
    var repeatedModel = DataVaultModel.Create(modelBuilder => {
      modelBuilder.Hub("Customer", hub => {
        hub.BusinessKey("Customer Id");
        hub.Satellite("Contact", satellite => satellite.Payload("Email Address"));
        hub.Satellite("Preferences", satellite => satellite.Payload("Language Code"));
      });
      modelBuilder.PointInTime("Customer History", "Customer", ["Contact", "Preferences"]);
    });

    var pointInTime = Single(model.Tables, table => table.Kind == DataVaultTableKind.PointInTime);

    Equal(string.Join("\n", ProducedNames(model)), string.Join("\n", ProducedNames(repeatedModel)));
    Equal("PitCustomerHistory", pointInTime.Name);
    SequenceEqual(
        ["CustomerHashKey", "PitLoadTimestamp", "ContactLoadTimestamp", "PreferencesLoadTimestamp"],
        pointInTime.Columns.Select(column => column.Name));
    SequenceEqual(
        ["PkPitCustomerHistoryCustomerHashKeyPitLoadTimestamp"],
        pointInTime.Constraints.Select(constraint => constraint.Name));
    SequenceEqual(
        ["CustomerHashKey", "PitLoadTimestamp"],
        pointInTime.Constraints.Single().ColumnNames);
    SequenceEqual(
        [
            "HubHashKeyReference:CustomerHashKey::0",
                "LoadTimestamp:PitLoadTimestamp::1",
                "SatelliteSnapshotLoadTimestampReference:ContactLoadTimestamp:Contact:",
                "SatelliteSnapshotLoadTimestampReference:PreferencesLoadTimestamp:Preferences:",
            ],
        pointInTime.PointInTimeFields.Select(field =>
            field.Kind + ":" + field.Name + ":" + field.SatelliteName + ":" + field.KeyOrdinal));
  }

  private static void PointInTimeDeclarationsValidateReferences() {
    ThrowsInvalidOperation(
        "missing hub",
        modelBuilder => modelBuilder.PointInTime("CustomerHistory", "Customer", ["Contact"]));
    ThrowsInvalidOperation(
        "at least one satellite",
        modelBuilder => {
          modelBuilder.Hub("Customer", hub => hub.Satellite("Contact"));
          modelBuilder.PointInTime("CustomerHistory", "Customer", []);
        });
    ThrowsInvalidOperation(
        "missing satellite",
        modelBuilder => {
          modelBuilder.Hub("Customer", hub => hub.Satellite("Contact"));
          modelBuilder.PointInTime("CustomerHistory", "Customer", ["Preferences"]);
        });
    ThrowsInvalidOperation(
        "does not belong to hub",
        modelBuilder => {
          modelBuilder.Hub("Customer", hub => hub.Satellite("Contact"));
          modelBuilder.Hub("Order", hub => hub.Satellite("OrderStatus"));
          modelBuilder.PointInTime("CustomerHistory", "Customer", ["OrderStatus"]);
        });
    ThrowsInvalidOperation(
        "more than once",
        modelBuilder => {
          modelBuilder.Hub("Customer", hub => hub.Satellite("Contact"));
          modelBuilder.PointInTime("CustomerHistory", "Customer", ["Contact", "Contact"]);
        });
  }

  private static void CustomNamingPolicyOverridesEachV1NameFamily() {
    var policy = new CustomNamingPolicy();
    var model = CreateModel(options => options.NamingPolicy = policy);

    var hub = Single(model.Tables, table => table.Kind == DataVaultTableKind.Hub);
    var satellite = Single(model.Tables, table => table.Kind == DataVaultTableKind.Satellite);
    var link = Single(model.Tables, table => table.Kind == DataVaultTableKind.Link);
    var pointInTime = Single(model.Tables, table => table.Kind == DataVaultTableKind.PointInTime);

    Equal("custom_hub_Customer", hub.Name);
    Equal("custom_sat_Customer_Contact", satellite.Name);
    Equal("custom_link_CustomerOrder", link.Name);
    Equal("custom_pit_CustomerHistory", pointInTime.Name);
    Contains(model.Tables.SelectMany(table => table.Columns), column => column.Name.StartsWith("custom_col_", StringComparison.Ordinal));
    Contains(pointInTime.Columns, column => column.Name.StartsWith("custom_pit_col_", StringComparison.Ordinal));
    Contains(model.Tables.SelectMany(table => table.Indexes), index => index.Name.StartsWith("custom_idx_", StringComparison.Ordinal));
    Contains(model.Tables.SelectMany(table => table.Constraints), constraint => constraint.Name.StartsWith("custom_constraint_", StringComparison.Ordinal));
  }

  private static void OptionsCanBeConfiguredFluently() {
    var policy = new CustomNamingPolicy();
    var options = new DataVaultModelOptions().UseNamingPolicy(policy);

    Equal(policy, options.NamingPolicy);
    Equal("custom_link_CustomerOrder", new DataVaultModelBuilder(options)
        .Link("CustomerOrder", ["Customer", "Order"])
        .Build()
        .Tables[0]
        .Name);
    Equal("custom_hub_Customer", DataVaultModel.Create(builder => {
      builder.Hub("Customer", hub => hub.BusinessKey("Customer Id"));
    }, configuredOptions => configuredOptions.UseNamingPolicy(policy)).Tables[0].Name);
  }

  private static DataVaultModel CreateModel(Action<DataVaultModelOptions>? configureOptions = null) {
    return DataVaultModel.Create(model => {
      model.Hub("Customer", hub => {
        hub.BusinessKey("Customer Id");
        hub.Satellite("Contact", satellite => satellite.Payload("Email Address"));
      });
      model.Link("CustomerOrder", ["Customer", "Order"]);
      model.PointInTime("CustomerHistory", "Customer", ["Contact"]);
    }, configureOptions);
  }

  private static void AllProducedNamesArePresent(DataVaultModel model) {
    foreach (var table in model.Tables) {
      NotWhiteSpace(table.Name);

      foreach (var column in table.Columns) {
        NotWhiteSpace(column.Name);
      }

      foreach (var index in table.Indexes) {
        NotWhiteSpace(index.Name);
      }

      foreach (var constraint in table.Constraints) {
        NotWhiteSpace(constraint.Name);
      }
    }
  }

  private static string[] ProducedNames(DataVaultModel model) {
    return model.Tables.SelectMany(table => new[]
    {
            "table:" + table.Kind + ":" + table.Name,
        }
    .Concat(table.Columns.Select(column => "column:" + column.Kind + ":" + column.Name))
    .Concat(table.PointInTimeFields.Select(field =>
        "pit-field:" + field.Kind + ":" + field.Name + ":" + field.SatelliteName + ":" + field.KeyOrdinal))
    .Concat(table.Indexes.Select(index => "index:" + index.Name))
    .Concat(table.Constraints.Select(constraint => "constraint:" + constraint.Name)))
    .ToArray();
  }

  private static T Single<T>(IEnumerable<T> values, Func<T, bool> predicate) {
    var matches = values.Where(predicate).ToArray();
    if (matches.Length != 1) {
      throw new InvalidOperationException("Expected one match but found " + matches.Length + ".");
    }

    return matches[0];
  }

  private static void NotWhiteSpace(string value) {
    if (string.IsNullOrWhiteSpace(value)) {
      throw new InvalidOperationException("Expected a non-empty name.");
    }
  }

  private static void Contains<T>(IEnumerable<T> values, Func<T, bool> predicate) {
    if (!values.Any(predicate)) {
      throw new InvalidOperationException("Expected matching value was not found.");
    }
  }

  private static void ThrowsInvalidOperation(string expectedMessage, Action<DataVaultModelBuilder> configureModel) {
    var exception = RecordException(() => DataVaultModel.Create(configureModel));

    if (exception is not InvalidOperationException invalidOperationException) {
      throw new InvalidOperationException("Expected InvalidOperationException but got " + exception?.GetType().Name + ".");
    }

    if (!invalidOperationException.Message.Contains(expectedMessage, StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "Expected exception message containing " + expectedMessage + " but got " + invalidOperationException.Message + ".");
    }
  }

  private static Exception? RecordException(Action action) {
    try {
      action();
      return null;
    }
    catch (Exception exception) {
      return exception;
    }
  }

  private static void Equal<T>(T expected, T actual) {
    if (!EqualityComparer<T>.Default.Equals(expected, actual)) {
      throw new InvalidOperationException("Expected " + expected + " but got " + actual + ".");
    }
  }

  private static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
    var expectedArray = expected.ToArray();
    var actualArray = actual.ToArray();

    if (expectedArray.Length != actualArray.Length) {
      throw new InvalidOperationException(
          "Expected " + expectedArray.Length + " values but got " + actualArray.Length + ".");
    }

    for (var index = 0; index < expectedArray.Length; index++) {
      if (!EqualityComparer<T>.Default.Equals(expectedArray[index], actualArray[index])) {
        throw new InvalidOperationException(
            "At index " + index + " expected " + expectedArray[index] + " but got " + actualArray[index] + ".");
      }
    }
  }

  private sealed class CustomNamingPolicy : IDataVaultNamingPolicy {
    public string GetHubTableName(DataVaultHubNameContext context) {
      return "custom_hub_" + context.EntityName;
    }

    public string GetLinkTableName(DataVaultLinkNameContext context) {
      return "custom_link_" + context.RelationshipName;
    }

    public string GetSatelliteTableName(DataVaultSatelliteNameContext context) {
      return "custom_sat_" + context.ParentEntityName + "_" + context.SatelliteName;
    }

    public string GetPointInTimeTableName(DataVaultPointInTimeNameContext context) {
      return "custom_pit_" + context.PointInTimeName;
    }

    public string GetTechnicalColumnName(DataVaultTechnicalColumnNameContext context) {
      return "custom_col_" + context.Kind + "_" + context.BaseName;
    }

    public string GetPointInTimeColumnName(DataVaultPointInTimeColumnNameContext context) {
      return "custom_pit_col_" + context.Kind + "_" + (context.SatelliteName ?? context.HubName);
    }

    public string GetIndexName(DataVaultIndexNameContext context) {
      return "custom_idx_" + context.Kind + "_" + context.TableName;
    }

    public string GetConstraintName(DataVaultConstraintNameContext context) {
      return "custom_constraint_" + context.Kind + "_" + context.TableName;
    }
  }

  private sealed record TestCase(string Name, Action Run);
}

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkProviderFilters {
  public const string All = "all";
  public const string Sqlite = "sqlite";
  public const string Postgres = "postgres";
  public const string SqlServer = "sqlserver";
  public const string MySql = "mysql";
  public const string Oracle = "oracle";
  public const string Db2 = "db2";

  public static IReadOnlyList<string> AllProviderFilters { get; } =
  [
      All,
      Sqlite,
      Postgres,
      SqlServer,
      MySql,
      Oracle,
      Db2,
  ];
}

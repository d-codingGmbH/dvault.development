using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

internal static class KnownProviderNames {
  public const string Sqlite = "Microsoft.EntityFrameworkCore.Sqlite";
  public const string Postgres = "Npgsql.EntityFrameworkCore.PostgreSQL";
  public const string SqlServer = "Microsoft.EntityFrameworkCore.SqlServer";
  public const string Oracle = "Oracle.EntityFrameworkCore";
  public const string Db2 = "IBM.EntityFrameworkCore";
  public const string MySqlPomelo = "Pomelo.EntityFrameworkCore.MySql";
  public const string MySqlOracle = "MySql.EntityFrameworkCore";
}

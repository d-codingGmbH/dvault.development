using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Tests.Shared;

public sealed class ProviderSqlExecutionContractTrackedEntity {
  public int Id { get; set; }

  public string Name { get; set; } = string.Empty;
}

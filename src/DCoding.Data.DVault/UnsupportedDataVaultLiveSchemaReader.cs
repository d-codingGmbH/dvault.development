using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class UnsupportedDataVaultLiveSchemaReader(string? explicitProviderName = null) : IDataVaultLiveSchemaReader {
  public Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    if (!string.IsNullOrWhiteSpace(explicitProviderName)) {
      return Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(explicitProviderName));
    }

    try {
      return Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(dbContext.Database.ProviderName));
    }
    catch (InvalidOperationException) {
      return Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName: null));
    }
  }
}

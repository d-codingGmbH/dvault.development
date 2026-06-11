using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CompiledEvidenceModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context, bool designTime) {
    return context is CompiledEvidenceContext evidenceContext
        ? (context.GetType(), evidenceContext.MetadataModel, evidenceContext.ModelCacheKey, designTime)
        : (object)(context.GetType(), designTime);
  }
}

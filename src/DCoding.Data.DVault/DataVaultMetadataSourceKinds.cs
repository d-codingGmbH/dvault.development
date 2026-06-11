using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultMetadataSourceKinds {
  public const string AppDefaultRegistry = "app-default-registry";
  public const string DbContextRegistry = "dbcontext-registry";
  public const string ModelMetadata = "model-metadata";
  public const string ModelRegistry = "model-registry";
  public const string ModelArtifact = "model-artifact";
}

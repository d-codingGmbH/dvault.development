using System.Reflection;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

internal sealed record DataVaultResolvedDbContextMetadataSource(
    string SourceKind,
    DataVaultMetadataRegistry MetadataRegistry);

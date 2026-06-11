using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Quickstarts.Shared;

public sealed class QuickstartVaultContext(DbContextOptions<QuickstartVaultContext> options) : DbContext(options) {
}

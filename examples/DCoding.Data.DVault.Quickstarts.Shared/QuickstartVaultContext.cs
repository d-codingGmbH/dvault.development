using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Quickstarts.Shared;

public class QuickstartVaultContext(DbContextOptions options) : DbContext(options) {
}

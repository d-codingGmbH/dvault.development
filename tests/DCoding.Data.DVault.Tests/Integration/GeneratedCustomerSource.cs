using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultHubMapping("Customer")]
[DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
[DataVaultBusinessKeyBinding(1, "Region Code", nameof(RegionCode))]
internal sealed record GeneratedCustomerSource(string CustomerId, string RegionCode);

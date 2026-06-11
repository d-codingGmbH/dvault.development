using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultHubSatelliteMapping("Customer", "Profile")]
[DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
[DataVaultSatellitePayloadBinding(0, "customer_name", nameof(CustomerName))]
[DataVaultSatellitePayloadBinding(1, "customer_status", nameof(CustomerStatus))]
[DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
internal sealed record GeneratedCustomerProfileSource(
    string CustomerHashKey,
    string CustomerName,
    string CustomerStatus,
    string HashDiff);

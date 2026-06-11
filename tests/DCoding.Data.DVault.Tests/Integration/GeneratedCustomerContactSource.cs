using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultHubSatelliteMapping("Customer", "ContactChannel")]
[DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
[DataVaultSatelliteDrivingKeyBinding(0, "Contact Type", nameof(ContactType))]
[DataVaultSatelliteDrivingKeyBinding(1, "Region Code", nameof(RegionCode))]
[DataVaultSatellitePayloadBinding(0, "Email Address", nameof(EmailAddress))]
[DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
internal sealed record GeneratedCustomerContactSource(
    string CustomerHashKey,
    string ContactType,
    string RegionCode,
    string EmailAddress,
    string HashDiff);

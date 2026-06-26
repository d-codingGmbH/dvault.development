using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultLinkMapping("CustomerIdentityMatch")]
[DataVaultLinkParticipantBinding(0, "SourceCustomer", nameof(SourceCustomerHashKey))]
[DataVaultLinkParticipantBinding(1, "MatchedCustomer", nameof(MatchedCustomerHashKey))]
internal sealed record GeneratedCustomerIdentityMatchSource(
    string SourceCustomerHashKey,
    string MatchedCustomerHashKey);

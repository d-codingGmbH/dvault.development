using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultLinkMapping("CustomerOrder")]
[DataVaultLinkParticipantBinding(0, "Customer", nameof(CustomerHashKey))]
[DataVaultLinkParticipantBinding(1, "Order", nameof(OrderHashKey))]
internal sealed record GeneratedCustomerOrderSource(string CustomerHashKey, string OrderHashKey);

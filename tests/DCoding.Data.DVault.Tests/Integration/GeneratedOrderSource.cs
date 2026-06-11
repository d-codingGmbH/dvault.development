using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[DataVaultHubMapping("Order")]
[DataVaultBusinessKeyBinding(0, "Order Id", nameof(OrderId))]
internal sealed record GeneratedOrderSource(string OrderId);

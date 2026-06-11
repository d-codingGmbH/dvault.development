using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record CustomerProfileStreamingScenario(
    IReadOnlyList<DataVaultSaveRequest> Requests,
    IReadOnlyList<string> CustomerHashKeys);

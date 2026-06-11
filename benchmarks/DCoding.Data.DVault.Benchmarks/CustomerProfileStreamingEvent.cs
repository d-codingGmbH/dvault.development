using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record CustomerProfileStreamingEvent(
    string CustomerBusinessKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);

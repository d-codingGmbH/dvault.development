using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record CustomerProfileBulkEvent(
    string CustomerBusinessKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);

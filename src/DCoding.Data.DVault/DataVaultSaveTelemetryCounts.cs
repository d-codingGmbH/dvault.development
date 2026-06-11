using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal readonly record struct DataVaultSaveTelemetryCounts(
    int RequestCount,
    int HubOperationCount,
    int LinkOperationCount,
    int SatelliteOperationCount);

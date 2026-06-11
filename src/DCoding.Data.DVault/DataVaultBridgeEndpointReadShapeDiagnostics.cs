using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable endpoint facts used by bridge read-shape diagnostics.
/// </summary>
public sealed record DataVaultBridgeEndpointReadShapeDiagnostics(
    DataVaultBridgeTraversalEndpoint Endpoint,
    string EndpointName,
    string ColumnName);

Inspect DCoding.Data.DVault.Analyzers for APIs, referenced Roslyn assemblies, code-fix dependencies, and packaging assumptions that force the current .NET 10 SDK host baseline.

Acceptance:
- The audit identifies whether netstandard2.0, net8.0, multi-targeting, or separate analyzer assets are viable.
- Source generator, diagnostic analyzer, and code-fix provider dependencies are checked separately.
- The audit lists any APIs that block .NET 8 SDK consumption and points to the affected source files or package assets.
- The result gives the implementation ticket a clear go/no-go path.
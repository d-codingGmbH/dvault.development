Update analyzer documentation after the compatibility implementation and verification are known.

Acceptance:
- README, Analyzer README, package compatibility docs, manual publication docs, and package verifier guidance state the exact build-host support matrix.
- The docs distinguish release label v0.50.0 from package versions 8.50.0 and 10.50.0.
- Docs keep analyzer references local with PrivateAssets="all" and do not imply runtime package usage.
- No stale .NET 10-only warning remains if .NET 8 SDK host support is actually verified; no .NET 8 support claim appears if the audit rejects it.
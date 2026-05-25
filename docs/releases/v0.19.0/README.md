# DVault v0.19.0 Release Evidence Index

This directory anchors the repository-visible `docs/releases/v0.19.0` release evidence path. The authoritative release notes remain [DVault v0.19.0 Release Notes](../v0.19.0.md).

## Evidence Sources

- Release notes: [DVault v0.19.0 Release Notes](../v0.19.0.md)
- Streaming save contract: [DVault V1 Streaming Explicit Save Contract](../../architecture/dvault-v1-streaming-explicit-save-contract.md)
- Explicit save service: [DVault V1 Explicit Save Service](../../architecture/dvault-v1-explicit-save-service.md)
- Root benchmark triplet: [benchmark-summary.md](../../../benchmark-summary.md), [benchmark-summary.csv](../../../benchmark-summary.csv), and [benchmark-summary.json](../../../benchmark-summary.json)
- Benchmark artifact contract: [Performance Evidence And Benchmark Artifact Contract](../../plans/performance-evidence-benchmark-artifact-contract.md)
- Repository validation command baseline: [README local validation](../../../README.md#local-validation)
- Publication evidence and package verification baseline: [Manual NuGet Publication Checklist](../../manual-nuget-publication.md)

## Boundary

- The v0.19.0 streaming-save evidence is the root `customer-profile-streaming-save` rows in the benchmark summary triplet, not a dedicated streaming-specific before-and-after bundle.
- Provider-native chunk execution, staged provider bulk ingestion, release automation, package publication, relation-state changes, and product-code changes remain outside the v0.19.0 public claim set.

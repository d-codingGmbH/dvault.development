Define and deliver the smallest compatible analyzer packaging path for .NET 8 build hosts if feasible. The current baseline requires a .NET 10 SDK host even for net8.0 consumer projects; this story should either remove that friction for the 8.x package line or produce a precise, tested no-go contract.

Acceptance:
- Analyzer Roslyn, code-fix, source-generator, and MSBuild asset dependencies are audited against a pure .NET 8 SDK build-host baseline.
- If feasible, the analyzer package ships an asset layout that can be consumed by net8.0 projects on a .NET 8 SDK host without adding runtime dependencies or widening the DVault runtime surface.
- If not feasible, the blocker is documented with exact API/tooling constraints and package verifier guidance stays explicit.
- Package verification and docs reflect exactly the supported build-host matrix for 8.50.0 and 10.50.0.

Non-goals:
- Supporting arbitrary old compiler versions.
- Adding analyzer runtime dependencies to consumer applications.
- Rewriting the analyzers or generators outside compatibility needs.
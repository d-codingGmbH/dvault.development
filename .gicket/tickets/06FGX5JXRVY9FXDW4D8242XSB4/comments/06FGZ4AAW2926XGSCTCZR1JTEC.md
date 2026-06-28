[gicket-bot] PO refinement contract

Summary
- Refinement ratified the current analyzer compatibility baseline: one net10.0 analyzer asset for both package lines, verifier coverage for README/asset layout, and smoke coverage only for the supported .NET 10 SDK host path.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v0.49.0 baseline: `DCoding.Data.DVault.Analyzers` ships one `net10.0` analyzer asset for both visible package lines (`8.50.0` and `10.50.0`), and supported consumption is a `.NET 10 SDK` build host.
- The ticket does not add or claim pure `.NET 8 SDK` analyzer-host support. The stale acceptance wording about `.NET 8 SDK-host behavior` is refined to smoke coverage for the supported `.NET 10 SDK` host, including `net8.0` consumer projects on the `8.50.0` line.
- Unsupported host combinations may be represented by deterministic package-verifier and documentation evidence; the repository does not need to invent a new pure `.NET 8 SDK` support claim or negative CI lane for this ticket.

Scope In
- Deterministic package verification for analyzer README/build-host guidance and analyzer asset layout on both coordinated package lines.
- A bounded smoke proof for supported analyzer consumption on the repository `.NET 10 SDK` host baseline, especially the non-obvious `8.50.0` / `net8.0` consumer case.
- Alignment of README/package-verifier/test-lane behavior so unsupported pure `.NET 8 SDK` analyzer consumption is explicit rather than implied.

Scope Out
- Retargeting `DCoding.Data.DVault.Analyzers` away from `net10.0`.
- Adding pure `.NET 8 SDK` analyzer-host support, CI, or package claims.
- Redesigning analyzer asset selection, multi-target packaging, or splitting analyzer/code-fix assets/packages.

Open questions
- none

Follow-up questions
- If product support for pure `.NET 8 SDK` analyzer hosts becomes required, should it be scheduled as a separate follow-up covering analyzer target/asset strategy, Roslyn dependency normalization, CI, and release-surface documentation?

Risks
- A deterministic negative pure `.NET 8 SDK` host lane may remain outside the current validation baseline, so unsupported-host proof may rely on verifier/documentation evidence instead of an executed failure test.
- The analyzer project still depends on SDK-local Roslyn/Workspaces/composition assemblies, which keeps future host-support expansion higher risk until those dependencies are normalized.

Split recommendations
- No split is needed for the current bounded verifier/smoke/documentation-alignment work.
- If pure `.NET 8 SDK` analyzer-host support is later required, split it into one implementation ticket for retargeting or package-shape changes plus dependency normalization, then one validation/documentation/release-surface ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
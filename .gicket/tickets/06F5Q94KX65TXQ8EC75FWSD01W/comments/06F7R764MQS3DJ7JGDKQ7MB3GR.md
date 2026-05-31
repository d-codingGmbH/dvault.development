[gicket-bot] PO refinement contract

Summary
- Refined as one bounded docs story: create a dedicated adopter-facing performance profile guide grounded in the checked-in benchmark triplet, preserve the existing epic and blocking relations, and avoid broad repo-wide release-doc consolidation in this ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository inspection showed no human clarification comments on the ticket; the only current comments are bot lease and claim markers.
- Persisted relation state already matches the intended flow: epic `06F5Q93R4633D41Z21WQW3SVGR` is the parent, and this story blocks documentation task `06F5Q94SQ086B2DZ1AKFDXGV94`; no relation write is needed.
- Treat this story as the detailed performance-profile guidance owner; the downstream documentation task should summarize and cross-link this work rather than duplicate the benchmark interpretation.
- Use only currently shipped observability surfaces in this story: `AddDVaultTelemetry()` plus existing save/read diagnostics and benchmark reruns. Do not couple the work to pending Activity tracing tickets.
- The checked-in root benchmark run is SQLite-required with all optional PostgreSQL, SQL Server, MySQL, and Oracle rows present as `executionStatus=skipped`, so provider-specific sections must describe eligibility boundaries and skip posture, not measured external-provider wins.

Scope In
- Create or update one adopter-facing performance-profile guide under `docs/` as the canonical detailed guidance surface, with narrow benchmark-doc cross-links if needed.
- Document the small app-local vault profile using the SQLite local baseline and existing explicit save/read service posture.
- Document the medium chunked-ingestion profile using `customer-profile-streaming-save` evidence and `DataVaultChunkedSaveRequest` guidance.
- Document the staged provider ingestion profile using the visible provider-native bulk rows and existing provider gate and boundary docs for PostgreSQL, SQL Server, MySQL, and Oracle.
- Document the read-model-heavy profile using `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` evidence plus explicit PIT and bridge maintenance boundaries.
- For every profile, include workload shape, registration starting point, diagnostics or telemetry to inspect, benchmark rows cited, and stop conditions or rerun triggers.

Scope Out
- No product code, benchmark harness changes, or new benchmark scenarios in this story.
- No dashboards, exporters, collectors, alerting, hosting, database or container provisioning, scheduler templates, or credential-management guidance.
- No tracing-contract or Activity instrumentation guidance beyond the already shipped `AddDVaultTelemetry()` and diagnostics surfaces.
- No absolute performance guarantees detached from artifact run context.
- No broad README, production-checklist, and release-note consolidation beyond the minimal cross-links needed for the detailed guide; coordinated public-doc rollup remains ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

Open questions
- none

Follow-up questions
- When ticket `06F5Q94SQ086B2DZ1AKFDXGV94` performs the coordinated v0.23.0 documentation rollup, should the new profile guide be summarized from both `README.md` and `docs/production-adoption-checklist.md`, or should one of those stay link-only?
- If a later checked-in benchmark run includes completed external-provider rows, should a follow-up docs pass add measured provider-specific examples that go beyond the current skipped-row boundary guidance?

Risks
- Because the checked-in optional-provider rows are all skipped, provider-specific sections can easily overclaim unless they stay disciplined about describing gates, fallback behavior, and skip reasons rather than measured wins.
- Timing values are machine-specific and must stay attached to the artifact run context; copying raw numbers without iterations, provider filter, and hardware/runtime context would violate the benchmark evidence contract.
- This story already blocks ticket `06F5Q94SQ086B2DZ1AKFDXGV94`, so expanding it into full coordinated README or release-note consolidation would create unnecessary schedule coupling.

Split recommendations
- No split recommended. Keep one detailed performance-guidance story here and leave the broader repo-wide documentation summary work to ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 6
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
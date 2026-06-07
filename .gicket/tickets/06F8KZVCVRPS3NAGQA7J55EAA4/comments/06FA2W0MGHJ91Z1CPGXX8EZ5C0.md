[gicket-bot] PO refinement contract

Summary
- Refined the artifact evidence ticket against the shared benchmark artifact contract, the landed SQL Server dry-run prototype, and current relation state; the existing split remains sufficient and no persistent planning write was required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified live context: epic 06F8KZTCEMNNFBFTVMFXEN268M parents this task, architecture story 06F8KZTNG44XDPMVTVCV4WJSHG is done, dry-run prototype story 06F8KZV18BQ0GN3CE4G02ATVA0 is done historical evidence, and this task still blocks docs ticket 06F8KZVRARQPG482YKCQ686PNM plus all-provider baseline task 06F9XD26D2MHVAKZ2GCZ67BEFC.
- The authoritative benchmark evidence contract is docs/plans/performance-evidence-benchmark-artifact-contract.md; this ticket does not invent ticket-specific benchmark filenames or schemas.
- Each provider-specific artifact proposal is assessed one exact provider and one representative workload at a time, using matched-input request diagnostics plus comparable benchmark artifacts.
- The semantic parity checklist is ratified from current landed contracts and prototype code: ordering, load timestamp, record source, hash key, hash diff, latest-state behavior, cancellation, cleanup boundary, and caller-owned transaction behavior are required, with PIT/bridge maintenance added when the workload exercises those surfaces.
- No child-ticket creation, relation cleanup, description update, attachment, or planning-document write was materialized in this pass.

Scope In
- Define the mandatory benchmark artifact set and before/after comparison rules for provider-specific artifact proposals.
- Ratify the current required local SQLite baseline rows and optional external-provider visibility rules as the shared performance evidence floor.
- Define the required semantic parity checklist and the repository evidence anchors that future prototype or implementation tickets must cite.
- Distinguish prototype or documentation evidence from implementation-ready provider claims.

Scope Out
- Running new benchmarks or completing the all-provider Podman baseline itself.
- Generating or updating SQL artifact manifests, sidecar SQL payloads, or runtime dispatch code.
- Changing benchmark harness schemas, adding new providers, or widening the workload matrix beyond one provider/workload proposal at a time.
- Automatic deployment, invocation, migration synchronization, or operational ownership.

Open questions
- none

Follow-up questions
- After 06F9XD26D2MHVAKZ2GCZ67BEFC lands, should the docs task cite exact completed all-provider rows or keep the current prototype-era skipped-row examples in v0.32 guidance?
- If the next artifact prototype is not SQL Server, should the team prefer PostgreSQL staged COPY or MySQL staged bulk as the next contrasting provider boundary after the current dry-run example?
- Should a later ticket define provider-specific parity additions for PIT/bridge-focused artifact workloads beyond the save-oriented checklist ratified here?

Risks
- The checked-in root benchmark triplet still shows optional external-provider rows as skipped when connection strings are unset, so this refinement must not be misread as completed live all-provider evidence.
- If future tickets omit matched-input diagnostics or hide skipped optional-provider rows, they will undermine the comparability rules this ticket is supposed to lock down.
- If provider-specific artifact work substitutes provider-side hashing, changes request ordering, or suppresses caller transaction ownership, it will violate the parity boundary already documented in current contracts and tests.
- Because the landed prototype is SQL Server-specific, teams may overgeneralize its workload facts unless this ticket keeps the one-provider/one-workload rule explicit.

Split recommendations
- No new split is justified; the current evidence/prototype/documentation/all-provider-baseline separation is sufficient.
- Create separate follow-up tickets instead of widening this task if the team wants deployable sidecar SQL payloads, runtime invocation helpers, provider-specific cleanup validators, or multi-workload/provider-matrix parity suites.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
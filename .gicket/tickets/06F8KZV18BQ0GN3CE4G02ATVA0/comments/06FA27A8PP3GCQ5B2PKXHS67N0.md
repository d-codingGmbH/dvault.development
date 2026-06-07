[gicket-bot] PO refinement contract

Summary
- Verified the live ticket and repository state, narrowed the story to a single SQL Server `provider-native-bulk-ingestion` dry-run manifest slice, and did not materialize any ticket or planning writes in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The first prototype is narrowed to the existing SQL Server external-provider `provider-native-bulk-ingestion` scenario: 20 order-product pairs, 20 order-product links, and 3 ordered fulfillment satellite operations including one unchanged replay in one provider-eligible bulk request.
- The manifest binds one exact provider/workload pair only: SQL Server external provider using the existing `SqlServerDataVaultSaveStrategy` boundary, not a multi-provider or multi-workload matrix.
- The dry-run manifest must record exact provider/profile identity, including the SQL Server provider name and the existing `sqlserver-v1` capability profile, plus metadata-source kind/fingerprint and explicit dry-run status.
- A valid first prototype may contain zero sidecar SQL payload files; if payload files are present later, they must be manifest-relative and content-hashed.
- Output path selection remains consumer-owned and caller-supplied through the existing design-time command boundary; this ticket does not standardize a repository storage convention.
- Verified live split context: epic `06F8KZTCEMNNFBFTVMFXEN268M` parents this ticket, the parent architecture contract ticket `06F8KZTNG44XDPMVTVCV4WJSHG` is done, and no new child tickets, relation changes, attachments, or planning documents were required in this pass.

Scope In
- Prototype one deterministic dry-run `dvault.sql-artifact.v1` manifest for the SQL Server `provider-native-bulk-ingestion` workload only.
- Reuse the existing consumer-owned design-time command/host boundary to emit the manifest with a caller-supplied output path.
- Capture manifest metadata for exact provider/profile binding, workload identity, metadata-source traceability, benchmark-evidence references, semantic-parity references, and dry-run review status.
- Keep the prototype bounded to design-time review output and optional manifest-relative payload metadata only.

Scope Out
- Runtime dispatch, automatic invocation, registration, background execution, or a second default DVault runtime path.
- Automatic deployment, automatic cleanup, EF migration mutation or synchronization, live-schema mutation, or support-bundle refresh automation.
- Additional providers, additional workload shapes, or a full provider matrix in this ticket.
- Collecting or approving the benchmark artifact triplet and semantic-parity evidence itself; that remains downstream work in `06F8KZVCVRPS3NAGQA7J55EAA4`.

Open questions
- none

Follow-up questions
- After the SQL Server prototype is stable, should the next provider example be PostgreSQL staged COPY or MySQL staged bulk to cover a second provider boundary?
- Should adopter-facing documentation later recommend one example repository layout for reviewed manifests and future sidecar SQL files, even though output-path selection stays consumer-owned?
- After the dry-run prototype and evidence ticket land, should a later lane allow deployable sidecar SQL payload emission, or should the artifact lane remain review-only longer?

Risks
- The SQL Server benchmark rows exist in the checked-in artifact triplet but are currently skipped because the external provider is not configured locally, so this ticket must not be treated as evidence completion or production-ready artifact approval.
- If implementation starts inferring provider/workload evidence when request-bound diagnostics are absent, the manifest could create unreviewed provider-specific claims.
- If developers widen this slice into automatic invocation, deployment, or migration synchronization, they will violate the parent contract already marked done in `06F8KZTNG44XDPMVTVCV4WJSHG`.
- A metadata-only dry-run manifest can be misread as a deployable artifact unless the explicit dry-run indicator and consumer-owned operational boundary stay visible.

Split recommendations
- No new split is justified now; the existing epic/parent/evidence/prototype/documentation separation is sufficient for this refinement pass.
- If later work wants provider-matrix coverage, deployable sidecar SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this first prototype.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
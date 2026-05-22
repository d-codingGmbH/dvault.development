[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492B9PR036PDNN52S06S9BC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B9PR036PDNN52S06S9BC`.
- Optimistic claim succeeded (`expectedRevision=06F4NV0M7YP39DKTFV28EF14YM`, `currentRevision=06F4T188TX0GXZCJ52BC0WAZTW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea' from source 'a37c71d3a2901cb217c47f2af5aeafe9ec4fc7d7'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- `DataVaultDiagnosticsResult` and `dvault.support-bundle.v1` are stable public surfaces, so the new shape model must be additive and version-safe.
- If the ticket promises raw SQL or provider-specific execution details, routine internal query rewrites will create avoidable churn; the contract should stay semantic and generated-name based.
- Index guidance must stay derived from current translated metadata so diagnostics do not claim consumer-managed physical indexes that DVault does not actually project today.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9614`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `143e9415c1ee483d805337c3a9049232`
- completed-at-utc: `<redacted>-22T00:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B9PR036PDNN52S06S9BC/runs/20260522T002602802Z-143e9415c1ee483d805337c3a9049232.json`
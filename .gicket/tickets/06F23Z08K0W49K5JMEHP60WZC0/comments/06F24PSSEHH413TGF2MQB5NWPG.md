[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F23Z08K0W49K5JMEHP60WZC0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F23Z08K0W49K5JMEHP60WZC0`.
- Optimistic claim succeeded (`expectedRevision=06F24GF1BS49TN89FKCKP17S2G`, `currentRevision=06F24NGWG31PD8SX5KCT2FYPZ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F23Z08K0W49K5JMEHP60WZC0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F23Z08K0W49K5JMEHP60WZC0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F23Z08K0W49K5JMEHP60WZC0-task-add-v0-8-0-lifecycle-guardrails-release-sum' from source '8f011c136ea05ea88a123238a555af74b4987a78'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F23Z08K0W49K5JMEHP60WZC0-task-add-v0-8-0-lifecycle-guardrails-release-sum` as `a2728cc160ce`.

Open questions / Risiken
- The parent epic still uses older `design-time services` wording; if the release note repeats that shorthand without the refined boundary, readers may wrongly infer a DVault-owned `IDesignTimeServices` feature.
- If the release note overstates drift support, readers may assume broader multi-provider live-schema coverage or a separate public CLI flow that the repository does not currently prove.
- If migration preflight is described as EF CLI interception or automatic migration execution, the note will over-promise behavior outside the documented workflow.
- Split recommendation: No further split recommended; this ticket is already bounded to one missing release-note artifact, and the underlying design-time and drift implementation work is already covered by completed related stories.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7538`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d39496364eb5427894a3c7a4dc81f0f1`
- completed-at-utc: `<redacted>-13T17:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F23Z08K0W49K5JMEHP60WZC0/runs/20260513T172103013Z-d39496364eb5427894a3c7a4dc81f0f1.json`
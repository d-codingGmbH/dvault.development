[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NTB26CCYQ7FCN2REEGDGW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NTB26CCYQ7FCN2REEGDGW`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4C035R8MCRYR5RK5EDS8`, `currentRevision=06EZPW5113GGCAJJVMTWC6ESH8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp' from source 'fd66ee8eb4c98d03c52e4213e49040b5c2a382ed'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp` as `a9e9fa710fbe`.

Open questions / Risiken
- If PIT naming or key semantics drift from sibling task 06EZ0NT4FDPC7XTQH40PQS942M, translator tests and public snapshots can diverge across the same story.
- PIT work can accidentally expand into refresh/materialization or provider-specific optimization scope unless the additive EF-mapping boundary stays enforced.
- New public PIT-facing enums, annotations, or table-shape surface may ripple into existing snapshot and compatibility checks beyond the immediate translator code.
- Split recommendation: No additional split is recommended. The existing PIT story 06EZ0NSXY2Y1JZ8SSCX177C770 already has bounded child tasks for metadata API (06EZ0NT4FDPC7XTQH40PQS942M), EF mapping (06EZ0NTB26CCYQ7FCN2REEGDGW), and docs/examples (06EZ0NTJZEMVA5RPR01V0KNVMR).

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9473`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4b5e57c9e92b421ebc39df01500c2cf4`
- completed-at-utc: `<redacted>-06T04:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/runs/20260506T041316678Z-4b5e57c9e92b421ebc39df01500c2cf4.json`
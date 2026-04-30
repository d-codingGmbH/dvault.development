[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB76NNRDP7WH1F2R5VYYPMR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB76NNRDP7WH1F2R5VYYPMR`.
- Optimistic claim succeeded (`expectedRevision=06EXQHG5TKAYMHTA2VFNJK8MAG`, `currentRevision=06EXQMD8V97ZSS0ZACAW5KGVQM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB76NNRDP7WH1F2R5VYYPMR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB76NNRDP7WH1F2R5VYYPMR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB76NNRDP7WH1F2R5VYYPMR-task-test-null-culture-ordering-and-binary-norma' from source '37c9d760919b2fe959b461c8d315648ff3ef7e8e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB76NNRDP7WH1F2R5VYYPMR-task-test-null-culture-ordering-and-binary-norma` as `b0d665b727b8`.

Open questions / Risiken
- The scope is broader than the prior test-only wording; developers must keep production changes limited to the stable-hashing contract and avoid implementing the full parent hash key/hash diff story here.
- Public API additions can fail build policy if XML documentation is missing because the library treats CS1591 as an error.
- Culture tests can leak process-global state if CurrentCulture and CurrentUICulture are not restored.
- Timestamp and decimal normalization can drift if implementation accidentally uses current culture, local time, serializer defaults, or platform-default encoding.
- Split recommendation: No child split or prerequisite relation is recommended now because this ticket is the bounded implementation-and-test slice for the default stable hash service and canonical normalizer.
- Split recommendation: Split full Data Vault hash key/hash diff entity services, persistence integration, and participating-field selection under the parent story or later follow-up tickets.
- Split recommendation: Create a separate binary scalar canonicalization ticket only if product approves byte array, stream, or base64 normalization beyond UTF-8 materialization of normalized strings.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `69033`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0352`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3610548698ea4d4f81468d53cf78031f`
- completed-at-utc: `<redacted>-30T00:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB76NNRDP7WH1F2R5VYYPMR/runs/20260430T004153415Z-3610548698ea4d4f81468d53cf78031f.json`
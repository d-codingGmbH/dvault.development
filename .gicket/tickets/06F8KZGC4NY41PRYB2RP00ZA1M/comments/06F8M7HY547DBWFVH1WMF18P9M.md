[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZGC4NY41PRYB2RP00ZA1M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGC4NY41PRYB2RP00ZA1M`.
- Optimistic claim succeeded (`expectedRevision=06F8M00CYRQXAC64J54P7TQTP4`, `currentRevision=06F8M597T06HFHRN5WG87K8SDW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZGC4NY41PRYB2RP00ZA1M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZGC4NY41PRYB2RP00ZA1M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract' from source '698174fa6d47078076adf67bcbeafa937c80ed98'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZGC4NY41PRYB2RP00ZA1M-story-define-ef-lifecycle-analyzer-contract` as `7a25540ddc29`.

Open questions / Risiken
- If the implementation tries to infer more than direct source-visible evidence, it is likely to regress existing safe compiled-model and registry-backed fixtures with false positives.
- If the implementation stays intentionally high-confidence, some caller-owned variable-shape cases will remain guidance-only; the documentation task must say that skipped ambiguous cases are intentional rather than a runtime guarantee.
- Split recommendation: No further split is recommended. The epic already separates contract (06F8KZGC4NY41PRYB2RP00ZA1M), implementation (06F8KZGNRG5FY4WWCY3FAX2NS4), fixtures (06F8KZGZND5ZCH147PVBRWXYN4), and docs (06F8KZHAB717MJJNAWWK7S0A5W).

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9483`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `993c66d28e66426f9704e4b99398d466`
- completed-at-utc: `<redacted>-02T20:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGC4NY41PRYB2RP00ZA1M/runs/20260602T205458329Z-993c66d28e66426f9704e4b99398d466.json`
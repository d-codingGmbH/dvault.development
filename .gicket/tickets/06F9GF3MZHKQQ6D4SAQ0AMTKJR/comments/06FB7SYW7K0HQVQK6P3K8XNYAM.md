[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9GF3MZHKQQ6D4SAQ0AMTKJR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF3MZHKQQ6D4SAQ0AMTKJR`.
- Optimistic claim succeeded (`expectedRevision=06F9GF4XQ68EM3DZPF337J1EFM`, `currentRevision=06FB7Q9N9C3TJPYKND797MFMR4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9GF3MZHKQQ6D4SAQ0AMTKJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9GF3MZHKQQ6D4SAQ0AMTKJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9GF3MZHKQQ6D4SAQ0AMTKJR-story-define-variable-hash-algorithm-and-digest' from source 'a92f5eb69da4f8af599ca8beab894568226eb390'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Broadening `StableHashDigest` is a public API behavior change and may break callers or tests that currently assume every digest is 64 lowercase hex characters.
- Without explicit wording, teams could confuse stable model and key hashing with persisted `content_hash` integrity rules and accidentally weaken storage expectations.
- Allowing SHA-1 or truncated digests without prominent caveats could be misread as a security recommendation instead of a bounded non-adversarial identity trade-off.
- Split recommendation: If implementation effort grows beyond the base contract and API widening, split built-in `sha1-v1` or truncated-SHA-256 registrations and their full compatibility-vector coverage into one or more follow-up tickets after the `sha256-v1`-compatible digest-c...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8594`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e470e3d9534647d9a0ab8f6b4e177001`
- completed-at-utc: `<redacted>-10T23:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF3MZHKQQ6D4SAQ0AMTKJR/runs/20260610T233937382Z-e470e3d9534647d9a0ab8f6b4e177001.json`
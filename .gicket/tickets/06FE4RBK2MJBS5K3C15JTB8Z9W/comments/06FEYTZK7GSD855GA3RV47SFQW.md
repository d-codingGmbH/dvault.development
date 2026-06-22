[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4RBK2MJBS5K3C15JTB8Z9W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RBK2MJBS5K3C15JTB8Z9W`.
- Optimistic claim succeeded (`expectedRevision=06FEYS2GYGGHNM05DQABJ7FZHW`, `currentRevision=06FEYSBQP954RR4R83SGPP7074`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4RBK2MJBS5K3C15JTB8Z9W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4RBK2MJBS5K3C15JTB8Z9W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4RBK2MJBS5K3C15JTB8Z9W-task-add-privacy-extension-example-and-documenta' from source '17614b80057d014407684440d2b06e9fd8060197'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4RBK2MJBS5K3C15JTB8Z9W-task-add-privacy-extension-example-and-documenta` as `354e51a7d667`.

Open questions / Risiken
- If the docs blur the difference between UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider) and the converter's runtime requirement for IDataVaultEncryptedPayloadKeyProvider, adopters may wire a provider that compiles but still fails at runtime.
- If example prose drifts from the privacy boundary, readers may infer GDPR/DSGVO compliance or provider-native encryption guarantees that the package does not make.
- A toy key-provider example can be mistaken for production cryptography guidance unless it is explicitly labeled as caller-owned demo code only.
- Split recommendation: No split recommended; the current branch already ships the public privacy proof APIs, boundary docs, and test-backed example pattern, so one bounded docs/example ticket remains appropriate.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8593`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `17b7c61256514dc1a37703214486e0b4`
- completed-at-utc: `<redacted>-22T13:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RBK2MJBS5K3C15JTB8Z9W/runs/20260622T130129781Z-17b7c61256514dc1a37703214486e0b4.json`
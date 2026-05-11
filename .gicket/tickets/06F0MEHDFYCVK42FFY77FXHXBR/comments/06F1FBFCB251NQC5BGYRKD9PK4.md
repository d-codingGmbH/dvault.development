[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEHDFYCVK42FFY77FXHXBR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEHDFYCVK42FFY77FXHXBR`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3Q2BGD2NWNC1YB4J264G`, `currentRevision=06F1FAJR5MEM1V9V2K9JSSGKJ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEHDFYCVK42FFY77FXHXBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEHDFYCVK42FFY77FXHXBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEHDFYCVK42FFY77FXHXBR-task-define-bridge-traversal-query-helper-contra' from source 'ff4af3f977422843f698ec7deb83d193b896a353'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEHDFYCVK42FFY77FXHXBR-task-define-bridge-traversal-query-helper-contra` as `bec5b31a052b`.

Open questions / Risiken
- Over-specifying graph behavior in this contract would conflict with the deferred-capabilities decision record and make the implementation ticket larger than intended.
- Provider-specific assumptions in the contract would make the provider-neutral fallback and later strategy-hook work harder to preserve.
- Ambiguous hierarchy depth semantics could lead to partial or misleading traversal results, so unsupported depth must fail clearly.
- Split recommendation: No split is recommended for this contract ticket; the implementation and provider-strategy work are already represented by related tickets 06F0MEHKYTBJEJH2DVZ2CFH9Z0 and 06F0MEJ7NANHCP64VR1SH3S3G8.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `33182`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0733`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `aabf2836784c4c049c1d2fe9081276f5`
- completed-at-utc: `<redacted>-11T15:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEHDFYCVK42FFY77FXHXBR/runs/20260511T153533814Z-aabf2836784c4c049c1d2fe9081276f5.json`
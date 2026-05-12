[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEGPPETJD4ZDEN5ESGR7JW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGPPETJD4ZDEN5ESGR7JW`.
- Optimistic claim succeeded (`expectedRevision=06F0QH32B69Z6DPG0BHPSKYC2C`, `currentRevision=06F1H1RXMSYJHAWFWPKR38A3CM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers' from source '138e66e6bfee49edbb5888d05751ffefb9bf566c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` as `c917b15e1ebe`.

Open questions / Risiken
- Consumers may expect PIT or bridge helpers to populate maintenance tables; diagnostics and documentation must keep the read-only, source-backed boundary explicit.
- Generated shared-type metadata drift could produce confusing runtime failures unless validation remains deterministic and names the metadata/table/property involved.
- Hierarchy bridge reads depend on precomputed rows and bounded maximum-depth filtering; they do not prove recursive traversal correctness beyond the generated table contents.
- PIT timestamp conversion must stay provider-neutral and caller-facing; storage-mode regressions can create subtle as-of boundary errors.
- Split recommendation: No additional split is recommended now; the story already has four parentOf child tickets materialized and the current repository layout gives bounded PIT, bridge, and test ownership surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `50534`
- cached-tokens: `12160`
- effective-cache-ratio: `0.2406`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `56c2fe044fed4c82ad158120434134ad`
- completed-at-utc: `<redacted>-11T19:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/runs/20260511T193556476Z-56c2fe044fed4c82ad158120434134ad.json`
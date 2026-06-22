[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4RJP5KG02DF7AEMCQYGNVW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJP5KG02DF7AEMCQYGNVW`.
- Optimistic claim succeeded (`expectedRevision=06FE4RMBMY8EARFXKVKK7R3ZDC`, `currentRevision=06FF03K9KVJW70P3PV1RQ6AVDG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4RJP5KG02DF7AEMCQYGNVW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4RJP5KG02DF7AEMCQYGNVW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel' from source 'e65395007316a1f9805fa33e2f0f46232bd18d09'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel` as `3902b3fd2f17`.

Open questions / Risiken
- Because provider packages currently have no PIT maintenance strategy registration seam, this ticket can sprawl into shared-core API design unless the work stays internal and provider-owned.
- If execution gating drifts from the approved dry-run contract, PostgreSQL behavior may silently diverge from fallback expectations or from the forthcoming SQL Server sibling.
- If the prototype grows beyond full rebuild into parent maintenance or bridge maintenance, it will reopen scope the parent story deliberately split out.
- External PostgreSQL integration coverage is opt-in through the existing environment-based harness; without that proof, provider-path regressions may survive unit-only validation.
- Split recommendation: Keep the existing decomposition unchanged: 06FE4RJD5Z6MWC2E66YB3EZ5YW for dry-run contract context, this ticket for the PostgreSQL prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for SQL Server, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and 06FE4RKGASKV6F7...
- Split recommendation: If implementation evidence shows one supported PIT baseline shape cannot safely share the same PostgreSQL INSERT SELECT prototype, create a shape-specific follow-up ticket instead of reopening PO scope here.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9582`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ee8f20404e194ca0bfd62905203ec628`
- completed-at-utc: `<redacted>-22T16:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/runs/20260622T161119670Z-ee8f20404e194ca0bfd62905203ec628.json`
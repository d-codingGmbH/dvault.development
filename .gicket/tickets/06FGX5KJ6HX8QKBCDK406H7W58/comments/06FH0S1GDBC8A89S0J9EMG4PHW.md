[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX5KJ6HX8QKBCDK406H7W58'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5KJ6HX8QKBCDK406H7W58`.
- Optimistic claim succeeded (`expectedRevision=06FGX6P5WMH0HR1T25Q3KDP7XG`, `currentRevision=06FH0Q4MEE94ZZ03JE784WKZH0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX5KJ6HX8QKBCDK406H7W58': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX5KJ6HX8QKBCDK406H7W58': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX5KJ6HX8QKBCDK406H7W58-task-update-analyzer-compatibility-documentation' from source '355fc69250a3dc26481c14ee97b518e03df33bdb'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX5KJ6HX8QKBCDK406H7W58-task-update-analyzer-compatibility-documentation` as `a9809b38ba23`.

Open questions / Risiken
- If the v0.50.0 release-note or changelog artifact lands separately from this ticket, README/package-compatibility/manual-publication cross-references can remain inconsistent even after analyzer wording is corrected.
- PackageVerifier guards packaged README content, but the broader documentation set can still drift unless the in-scope docs are reviewed together in the same change.
- Split recommendation: No split is needed for the current ticket; the current scope is bounded to documentation and verifier-alignment surfaces.
- Split recommendation: If pure .NET 8 SDK analyzer-host support is later required, split it into one implementation ticket for analyzer asset/target/dependency changes and one follow-up ticket for CI, package verification, and documentation rollout.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8732`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0ef33313e23c4edf9d2573226e5cfe60`
- completed-at-utc: `<redacted>-28T22:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5KJ6HX8QKBCDK406H7W58/runs/20260628T224029282Z-0ef33313e23c4edf9d2573226e5cfe60.json`
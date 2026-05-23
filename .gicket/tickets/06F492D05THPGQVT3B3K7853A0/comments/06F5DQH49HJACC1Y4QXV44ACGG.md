[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492D05THPGQVT3B3K7853A0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492D05THPGQVT3B3K7853A0`.
- Optimistic claim succeeded (`expectedRevision=06F5DM4RG3ZK2P27Z6HNHPG6FC`, `currentRevision=06F5DN0A5CV912J5BG8ZEVCT1M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no' from source '980857a7f42861ce99f1c0f120d5118000801740'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no` as `983a739a5ec1`.

Open questions / Risiken
- Blocking finding: Acceptance Criteria require `docs/releases/v0.18.0.md` to record an intended release date, but the persisted ticket contract does not supply one, and local release metadata does not fill the gap: `.gicket/releases/06F492A0EZ3N8E2T605F7ZHHB0.json` and `.gicket...
- Required PO action: Add the exact intended release date for `v0.18.0` to the delivery contract, or point to one authoritative local release-planning artifact that supplies the date the release note must copy.
- Required PO action: If no exact date is currently approved, relax the acceptance criterion so the release note can use an explicitly authorized placeholder or cross-reference instead of forcing the developer to guess.
- Risky assumption: Assuming the intended `v0.18.0` release date can be inferred from ticket creation time, release object creation time, prior release cadence, or the current calendar date.
- Risky assumption: Assuming only the three named files need baseline-pointer edits when the repo already shows additional current-baseline prose in `README.md`.
- Split recommendation: No split recommended; the benchmark/profiling work is already isolated in done sibling tickets, and this ticket is appropriately scoped as one documentation and release-note rollup once the release-date gap is resolved.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6438`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `51fa268c6cdc4c61b4ff62cb585e70c8`
- completed-at-utc: `<redacted>-23T22:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492D05THPGQVT3B3K7853A0/runs/20260523T220419395Z-51fa268c6cdc4c61b4ff62cb585e70c8.json`
[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZQAWZ7QRGB68KB21C9B0R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZQAWZ7QRGB68KB21C9B0R`.
- Optimistic claim succeeded (`expectedRevision=06F9BGRD1Q102XFF8EK98AK8P0`, `currentRevision=06F9BGZ8KX4A1771AZ5W0A3X5M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum' from source '42322633eba1e35ded4f62b5c499d55cf8774eab'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum` as `f6cf378d8dce`.

Open questions / Risiken
- Blocking finding: The prompt marks this as a closure-only audit, but the persisted contract and current repository state still require net-new documentation work. Closure-only approval is therefore unsupported on the current routing.
- Blocking finding: The repository does not yet satisfy the ticket's own closure evidence: `docs/releases/v0.30.0.md` is absent, `README.md:371-390` does not yet describe the refresh/recovery workflow after bundle or fingerprint changes, and `docs/architecture/dvault-dotnet-ef-d...
- Required PO action: Remove or correct the closure-only posture for `06F8KZQAWZ7QRGB68KB21C9B0R` and reroute it as a normal documentation implementation ticket for developer handoff.
- Required PO action: If Product wants this to remain closure-only, first land repository evidence for the scoped documentation changes, including a new `docs/releases/v0.30.0.md`, then resubmit the ticket with that landed evidence.
- Risky assumption: Assuming the ticket can close because the sibling freshness/fingerprint implementation tickets are done; those tickets do not replace this ticket's own documentation deliverables.
- Risky assumption: Assuming relation cleanup is fully materialized already; `.gicket/relations/H8/0R/06F8KZPZZE8VZEBANP5MPN8HH8--06F8KZQAWZ7QRGB68KB21C9B0R--blocks.json` still exists even though `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json` shows `is-blocked: false` ...
- Split recommendation: No split recommendation; fix the routing mismatch first.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9186`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `97397827906348888c658e04dd81ec71`
- completed-at-utc: `<redacted>-05T03:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/runs/20260605T031907172Z-97397827906348888c658e04dd81ec71.json`
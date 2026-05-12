[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEGPPETJD4ZDEN5ESGR7JW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEGPPETJD4ZDEN5ESGR7JW`.
- Optimistic claim succeeded (`expectedRevision=06F1H75DJP2AT1FMFX3DVEAK4G`, `currentRevision=06F1H7G5VFRYTX3KNZXTV6HVX8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers' from source '4190a1d690937a99a8af8d9c25261fb0c8063593'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` as `50e408a99eec`.

Open questions / Risiken
- Risky assumption: Approval assumes the downstream dev workflow can accept a completion/consistency umbrella story even though `git diff develop...ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` shows no source/test delta on the parent branch.
- Risky assumption: Approval assumes the stale v0.6.0 limitation text is acceptable temporarily because dedicated downstream docs and benchmark tickets already exist.
- Split recommendation: No further split recommended; the four child tickets are done and the remaining docs/benchmark work already exists as separate downstream tickets.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8836`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `12c9003123014f5da96f0f43a117a83e`
- completed-at-utc: `<redacted>-11T20:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/runs/20260511T200522492Z-12c9003123014f5da96f0f43a117a83e.json`
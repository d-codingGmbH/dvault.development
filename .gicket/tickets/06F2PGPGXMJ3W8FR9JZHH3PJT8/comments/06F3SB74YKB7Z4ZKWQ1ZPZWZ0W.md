[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPGXMJ3W8FR9JZHH3PJT8`.
- Optimistic claim succeeded (`expectedRevision=06F3S9W6T01Z36M5Y0GDRM0EF4`, `currentRevision=06F3SA2BQ79QDK5JVR2FQVYNX8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service' from source '44b931f2e58bf2af639faf78a071aed18c7d95a8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service` as `a6352fee6649`.

Open questions / Risiken
- Risky assumption: Assumes shortest-path semantics plus the `no implicit self rows` rule are sufficient for cyclic source data without another refinement ticket.
- Risky assumption: Assumes the v0.15.0 release-note delta will be created during implementation; `docs/releases/` currently stops at `v0.14.0`.
- Risky assumption: Assumes the new maintenance API can be introduced beside the current extension-based bridge read surface without forcing a broader public API redesign.
- Split recommendation: No split recommended; sibling tickets already isolate PIT maintenance, query API follow-up, provider-aware read optimization, and broader v0.15.0 documentation.
- Split recommendation: If delete-aware or topology-shrinking incremental hierarchy maintenance becomes necessary, track it as a separate follow-up instead of widening this v1 story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8141`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d7e35d30ba154919804bab461ea3ec1a`
- completed-at-utc: `<redacted>-18T20:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/runs/20260518T200024078Z-d7e35d30ba154919804bab461ea3ec1a.json`
[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q900FC0P3HBZP81CVK7264'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q900FC0P3HBZP81CVK7264`.
- Optimistic claim succeeded (`expectedRevision=06F6A6WJE22C229BMX15EER4NG`, `currentRevision=06F6A752F3BR92QPTRPY4FD088`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre' from source '2372a536d601e3f1bd0a1beb7333fd04dc0288ec'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre` as `0aef439c82a7`.

Open questions / Risiken
- Risky assumption: The contract assumes the targeted regression rows are the new staged-bulk comparison rows added to `provider-native-bulk-ingestion`; it does not enumerate final baseline ids, so dev/test must keep row identities explicit in the artifact set.
- Risky assumption: The contract assumes unattended runs may still produce skipped optional-provider rows only, consistent with the current artifact contract; live provider regression claims will still depend on configured external database lanes.
- Split recommendation: No split is needed for PO refinement if the work stays on benchmark harness, benchmark evidence, and benchmark-contract documentation for staged-bulk comparisons.
- Split recommendation: If future work wants cross-scenario regression-budget policy changes beyond `provider-native-bulk-ingestion`, keep that as a separate artifact-governance ticket instead of widening this story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8755`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `76e6cf442adb4e539398326ed37d340f`
- completed-at-utc: `<redacted>-26T16:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q900FC0P3HBZP81CVK7264/runs/20260526T163353281Z-76e6cf442adb4e539398326ed37d340f.json`
[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSC4HSXFJ5FM6GWECH2CTGG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4HSXFJ5FM6GWECH2CTGG`.
- Optimistic claim succeeded (`expectedRevision=06FCSXJEW4B35RREWFRKWT6CBC`, `currentRevision=06FCSXN4N1CH90T598CQ1NKZXG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSC4HSXFJ5FM6GWECH2CTGG-story-publish-provider-optimization-gap-matrix' from source 'ce1c6e3585f5b015a6fc503026a166dc17dbcd66'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSC4HSXFJ5FM6GWECH2CTGG-story-publish-provider-optimization-gap-matrix` as `0f0a809d3a36`.

Open questions / Risiken
- Risky assumption: A developer could incorrectly treat all non-SQLite rows as evidence gaps; repository evidence says latest-satellite is a capability gap outside SQLite because no provider-specific latest-satellite read strategy is registered.
- Risky assumption: A developer could overstate DB2 from smoke/diagnostics evidence; the current baseline allows clean-context save and PIT/bridge candidate wording only, not completed DB2 timing, latest-satellite optimization, staged bulk, or provider-native chunk execution.
- Split recommendation: No split is needed for this story before developer handoff.
- Split recommendation: If follow-up implementation tickets are created from the published matrix, split by gap family: non-SQLite latest-satellite capability work, external-provider read evidence work, and external-provider save evidence work.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8859`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d1502ee4e96b45d28546dc63dfa5739e`
- completed-at-utc: `<redacted>-15T20:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4HSXFJ5FM6GWECH2CTGG/runs/20260615T203217983Z-d1502ee4e96b45d28546dc63dfa5739e.json`
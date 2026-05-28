[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q92YGB53W7YG6VCMA3FZJR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92YGB53W7YG6VCMA3FZJR`.
- Optimistic claim succeeded (`expectedRevision=06F6VQ5NW47XR4MYKEB64MYB3R`, `currentRevision=06F6VQE1AHKRJC20GYD4MBHE00`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea' from source 'dc11bdbbfd965a0c54c57209342757666b467443'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea` as `c60799c50fb5`.

Open questions / Risiken
- Blocking finding: The ticket no longer states a distinct remaining developer-owned deliverable after the implementation slices were assigned to `06F5Q92AHG0ZCTVQGC6NAYVP9C` and `06F5Q92R02HB7FCE1AWKXPTMRW`, and both related tickets now read `done`.
- Blocking finding: Documentation ownership is ambiguous: this ticket's acceptance criteria include documentation work, but separate blocked task `06F5Q93H60W6X8FJ88PWTR6NG4` also owns the typed-read documentation rollup.
- Blocking finding: The residual decision on whether this ticket still ships any deterministic typed-read code fix is not explicit; related ticket text still treats that as a follow-up question.
- Required PO action: Re-baseline this ticket to one explicit residual deliverable that is not already owned by `06F5Q92AHG0ZCTVQGC6NAYVP9C`, `06F5Q92R02HB7FCE1AWKXPTMRW`, or docs task `06F5Q93H60W6X8FJ88PWTR6NG4`.
- Required PO action: Decide documentation ownership explicitly: either keep docs in this ticket and narrow/remove the overlap on `06F5Q93H60W6X8FJ88PWTR6NG4`, or strip docs AC from this story and let the docs task own the rollup.
- Required PO action: State explicitly whether zero new typed-read code fixes is acceptable for this ticket; if not, name the exact diagnostic/edit pair that this ticket must ship.
- Risky assumption: Assuming a meaningful `code fixes` deliverable still exists here even though the contract mostly defines negative fixer guardrails and no positive typed-read fixer target.
- Risky assumption: Assuming the existing README and analyzer README coverage is not already the documentation work, even though a separate blocked docs task carries that ownership.
- Split recommendation: Keep satellite generator work on `06F5Q92AHG0ZCTVQGC6NAYVP9C` and PIT/bridge generator work on `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Split recommendation: Treat this ticket as residual integration/closure-only work only after PO explicitly states what remains and how it differs from docs task `06F5Q93H60W6X8FJ88PWTR6NG4`.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9087`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `de6d3fd82ac440e7b548d2dd30229aac`
- completed-at-utc: `<redacted>-28T09:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/runs/20260528T092343589Z-de6d3fd82ac440e7b548d2dd30229aac.json`
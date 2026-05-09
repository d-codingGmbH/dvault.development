[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0ME8NFJX6CD20MEA10J761R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0ME8NFJX6CD20MEA10J761R`.
- Optimistic claim succeeded (`expectedRevision=06F0XVFM1A7S32YSZPSAWJRY14`, `currentRevision=06F0XVRNYFZ5EA255VPCKA1YK0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0ME8NFJX6CD20MEA10J761R-story-add-fluent-ef-code-first-modeling-api' from source '1b0111730eabdd83c292937c457c598b3341005c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0ME8NFJX6CD20MEA10J761R-story-add-fluent-ef-code-first-modeling-api` as `24cbc4be3a15`.

Open questions / Risiken
- Risky assumption: Consumers who need non-CLR logical hub names can tolerate the temporary metadata-first fallback until a dedicated follow-up ticket exists.
- Risky assumption: Future work will not silently widen selector parsing beyond direct readable scalar members without updating both the contract and parity coverage.
- Risky assumption: Any future need for link-parent satellites or same-hub/recursive participant roles will be split into dedicated tickets instead of being added implicitly under this story.
- Split recommendation: No additional split is needed for this story; keep the existing parentOf structure to 06F0ME976PM5455JK04S6GPNNW, 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- Split recommendation: If scope later expands to hub-name overrides, link-parent satellites, or same-hub/recursive participant roles, create dedicated follow-up tickets instead of reopening this bounded story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8830`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `dfa249555b7540e8801261372e9be2c5`
- completed-at-utc: `<redacted>-09T22:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0ME8NFJX6CD20MEA10J761R/runs/20260509T225600047Z-dfa249555b7540e8801261372e9be2c5.json`
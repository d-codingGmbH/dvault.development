[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q8Z0Y0ADE5H37DAPA1ADQM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8Z0Y0ADE5H37DAPA1ADQM`.
- Optimistic claim succeeded (`expectedRevision=06F627THCGV5PNVMYDDM190FG8`, `currentRevision=06F63FNSE3Y1NJAASYWA4YN6RW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno' from source 'f860f2e51c1088c727fa407996ee89b2a6dfd026'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno` as `99f2492b51f4`.

Open questions / Risiken
- Risky assumption: The ticket assumes one finite staged-provider caveat taxonomy can serve all providers; if a provider needs materially different caveats, the contract already points to provider-specific follow-up tickets.
- Risky assumption: The ticket assumes staged fallback/decline details can be added as additive extensions to existing public diagnostics and telemetry types without reopening the public save boundary.
- Risky assumption: Automation may still see the stale relation file `.gicket/relations/QW/QM/06F5Q8YKR31DXGRXVPJ9031BQW--06F5Q8Z0Y0ADE5H37DAPA1ADQM--blocks.json`, but the source ticket is `done`, current ticket `isBlocked` is `false`, and the PO contract/comment trail already t...
- Split recommendation: No split recommended at this gate; the current contract is bounded to additive diagnostics work and already keeps provider implementation, benchmark, and documentation work in separate tickets.
- Split recommendation: If implementation evidence later shows materially different provider-specific caveat taxonomies or lifecycle outcomes, split those into provider-specific follow-up tickets rather than widening this shared diagnostics story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8635`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e767e3378e8440e5a5d1fb008551db67`
- completed-at-utc: `<redacted>-26T00:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8Z0Y0ADE5H37DAPA1ADQM/runs/20260526T005208764Z-e767e3378e8440e5a5d1fb008551db67.json`
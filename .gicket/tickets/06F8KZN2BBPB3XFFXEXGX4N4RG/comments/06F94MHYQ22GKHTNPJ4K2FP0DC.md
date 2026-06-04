[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZN2BBPB3XFFXEXGX4N4RG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZN2BBPB3XFFXEXGX4N4RG`.
- Optimistic claim succeeded (`expectedRevision=06F94J9X814RCD9BBZDTR0P438`, `currentRevision=06F94JGPE7MG4DZAZ50FZMXPK4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZN2BBPB3XFFXEXGX4N4RG-story-add-provider-identifier-preflight-checks' from source '268fc0b4f0968f4fdb6bc38bfe6d1ae107fba524'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZN2BBPB3XFFXEXGX4N4RG-story-add-provider-identifier-preflight-checks` as `64e556503024`.

Open questions / Risiken
- Risky assumption: Developers will source reserved-word catalogs and unquoted-identifier rules from finite repository-controlled profile data, not live vendor docs or implicit EF behavior.
- Risky assumption: Collision scope will follow the contract's provider plus object-class plus natural EF relational scope and will not use declaration order as the uniqueness key.
- Risky assumption: Existing diagnostics and report surfaces can expose attempted physical names and remediation boundaries without introducing a second public naming API.
- Split recommendation: No PO split is needed; keep this ticket limited to provider identifier preflight and leave broader provider-specific migration guardrails in '06F8KZNBGB8FPW6TK5A8SAJMVC'.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9213`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `638e3754be704ede8cab8326ef449ab5`
- completed-at-utc: `<redacted>-04T11:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZN2BBPB3XFFXEXGX4N4RG/runs/20260604T110844079Z-638e3754be704ede8cab8326ef449ab5.json`
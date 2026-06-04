[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F988YVCF40G2DCB8F2ZT5N6C`, `currentRevision=06F9893VMRF7G41G4A3HSEP5FC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '3b9190179138aec82b2c143d74756680dbeaebbe'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `caee1fbbcd80`.

Open questions / Risiken
- Blocking finding: The epic does not state what developer-owned work remains after those four child tickets landed on `develop`; sending this ticket to `dev` now would be ambiguous and risks duplicate or no-op execution.
- Required PO action: Decide whether 06F8KZM6KFZ3WC5MY5NC12B0TW is now a closure-only/roll-up ticket over 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68, or whether there is specific residual scope that still ne...
- Risky assumption: That the four done child tickets fully satisfy the epic even though the epic never explicitly states whether any cross-ticket integration or closure task remains.
- Split recommendation: No new split is recommended right now; first reconcile the epic against the already-existing done child tickets, and only create another child ticket if PO can name residual work that is not already covered by those four tickets.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8942`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `95e8f47dffc74944afa34a09a99bc400`
- completed-at-utc: `<redacted>-04T19:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T194543546Z-95e8f47dffc74944afa34a09a99bc400.json`
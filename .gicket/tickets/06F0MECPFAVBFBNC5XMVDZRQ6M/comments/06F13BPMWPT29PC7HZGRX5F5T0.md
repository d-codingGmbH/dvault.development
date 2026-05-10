[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MECPFAVBFBNC5XMVDZRQ6M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MECPFAVBFBNC5XMVDZRQ6M`.
- Optimistic claim succeeded (`expectedRevision=06F105BBSMTVZ6Y4DAC6P8NWZR`, `currentRevision=06F139RJEAN6NCD37V6VXC8RFR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' from source 'aa1779d8610ba94388b85ac24677bb0c9af933aa'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p` as `4974121128ad`.

Open questions / Risiken
- Blocking finding: The ticket does not pin the actual public typed-read contract shape. `description.md:15,21,34-47,49-55` requires a `thin typed projection contract` plus public API/XML-doc/snapshot updates, but it never states whether callers use an interface, delegate, build...
- Blocking finding: Nullability behavior is acceptance-critical, but the contract does not say how required versus nullable fields are declared once ambient DTO inference and reflection-discovered binding are explicitly out of scope. `description.md:14-15,25,31,39,45,53` defines...
- Blocking finding: Exact-name collisions between technical fields and logical payload/driving-key names are unresolved. The contract wants one exact-name projection space over `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, payloads, and driving keys, while curren...
- Required PO action: Amend the ticket with one concrete v1 typed-read contract example and explicitly choose the baseline public surface for callers (for example interface-based, delegate-based, or builder-based), including how both explicit and registry-backed helper APIs cons...
- Required PO action: State how the manual projection contract expresses required versus nullable fields, and what deterministic diagnostic shape is expected when a required value is missing or null.
- Required PO action: Define the collision rule for technical field names versus payload/driving-key names: either reserve/disallow those overlaps up front or specify deterministic precedence/aliasing.
- Risky assumption: Assuming the developer will infer the intended read-side public API from the save-side typed mapper pattern even though no read-side equivalent contract is currently documented in the repo.
- Risky assumption: Assuming nullability can be derived from CLR DTO metadata without violating the ticket's explicit `no DTO CLR-type lookup / no reflection-discovered binder` boundary.
- Risky assumption: Assuming no real satellite metadata will use logical names that collide with technical field names.
- Split recommendation: No split recommended. Keep this as one ticket, but refine the same ticket with a concrete public projection contract example, a nullability-declaration rule, and a technical-name collision rule before dev handoff.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9277`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6fc6359a9f3b4ab1ac56c98a942ea9e8`
- completed-at-utc: `<redacted>-10T11:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/runs/20260510T113849948Z-6fc6359a9f3b4ab1ac56c98a942ea9e8.json`
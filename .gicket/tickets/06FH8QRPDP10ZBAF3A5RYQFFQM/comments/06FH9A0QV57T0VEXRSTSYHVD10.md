[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8QRPDP10ZBAF3A5RYQFFQM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8QRPDP10ZBAF3A5RYQFFQM`.
- Optimistic claim succeeded (`expectedRevision=06FH8SAYSA93WE448MP9X13Q24`, `currentRevision=06FH97G5V0XVQ0TN0T8ZGWDM70`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8QRPDP10ZBAF3A5RYQFFQM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8QRPDP10ZBAF3A5RYQFFQM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8QRPDP10ZBAF3A5RYQFFQM-task-design-analyzer-asset-and-dependency-strate' from source '7493a70dbf8750b94bb7e95e5be6acc367fe4d27'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Detected directly written bounded PO planning artifact for transactional writeback: docs/plans/analyzer-dotnet8-host-strategy-refinement.md.
- 3 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Retargeting to `netstandard2.0` is not a csproj-only change: analyzer sources currently use modern BCL APIs and framework assumptions that will need bounded compatibility work.
- The package-verifier and README baselines currently hard-code the `.NET 10 SDK` host claim and a flat single-analyzer-asset expectation; those guardrails must change in lockstep with implementation or they will misreport the new package shape.
- If the reviewed analyzer companion-assembly strategy under `analyzers/dotnet/cs/` proves insufficient on actual `.NET 8 SDK` or IDE hosts, the later implementation may still need a narrower asset split despite this design decision.
- Split recommendation: No additional split is justified inside this design ticket; use `docs/plans/analyzer-dotnet8-host-strategy-refinement.md` as the bounded handoff artifact for the later implementation ticket that changes project references, packing, verifier coverage, test...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9417`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9633a4903f9b4f2a806cb05418010e56`
- completed-at-utc: `<redacted>-29T18:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8QRPDP10ZBAF3A5RYQFFQM/runs/20260629T183308298Z-9633a4903f9b4f2a806cb05418010e56.json`
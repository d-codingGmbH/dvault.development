[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB82RW6PV2NFG088G6BPFHC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB82RW6PV2NFG088G6BPFHC`.
- Optimistic claim succeeded (`expectedRevision=06EYXEF9619RXH42ZFQPVN6BAW`, `currentRevision=06EYXGZABJFRR6RDM29KJS7GYM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB82RW6PV2NFG088G6BPFHC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB82RW6PV2NFG088G6BPFHC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting' from source '24db1ca5c2a95fd068dc3d82b0f514874862c412'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB82RW6PV2NFG088G6BPFHC-task-add-ci-workflow-for-build-tests-formatting` as `0afc7ee4d91b`.

Open questions / Risiken
- If workflow test filters drift from the provider-category contract, default CI could either miss required SQLite coverage or accidentally execute unconfigured external-provider tests.
- Any future change to the packable package matrix or package metadata baseline will require the CI package-verification step to be updated in lockstep.
- CI runner images must continue providing the expected .NET SDK and shell support for the repository scripts; otherwise failures will present as environment drift rather than product regressions.
- Split recommendation: No new split is recommended; current repository and ticket evidence keep this work bounded to wiring the existing validation commands into the first CI workflow.
- Split recommendation: If configured external-provider jobs or release automation are needed later, capture them as separate follow-up tickets instead of expanding this ticket beyond the default validation workflow.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `67696`
- cached-tokens: `10624`
- effective-cache-ratio: `0.1569`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `66b31f9bdae042abb3bcba31c69a8058`
- completed-at-utc: `<redacted>-03T17:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB82RW6PV2NFG088G6BPFHC/runs/20260503T170149618Z-66b31f9bdae042abb3bcba31c69a8058.json`
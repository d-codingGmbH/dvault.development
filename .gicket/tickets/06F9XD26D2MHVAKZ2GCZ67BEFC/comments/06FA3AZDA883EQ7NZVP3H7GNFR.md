[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9XD26D2MHVAKZ2GCZ67BEFC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9XD26D2MHVAKZ2GCZ67BEFC`.
- Optimistic claim succeeded (`expectedRevision=06F9XD3WH589HVK3HVSM5E2Q1R`, `currentRevision=06FA36X8PW2P6J8CDTTGHG2780`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9XD26D2MHVAKZ2GCZ67BEFC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9XD26D2MHVAKZ2GCZ67BEFC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma' from source '925adf1b2c5c4011abf2cf3cfbbfd3d7378da7aa'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma` as `6492171cd43d`.

Open questions / Risiken
- The checked-in root benchmark-summary triplet still reflects a skipped external-provider posture, so downstream work may cite the wrong baseline if the new v0.32.0 bundle is not explicitly referenced.
- Because --scale does not execute bridge or PIT read rows, relying on the scale artifact alone would miss the SatCustomerStatu cleanup verification for bridge-traversal-read.
- External-provider results depend on live Podman endpoints and conditional provider packages; misconfigured connection strings or running PostgreSQL outside the Podman network can produce false skips or failures unrelated to product behavior.
- If the implementer tweaks thresholds or provider code while capturing this baseline, the evidence becomes unusable as a pre-tuning snapshot and undermines the downstream comparison tasks.
- Split recommendation: No new split is justified; keep this ticket as the evidence-capture prerequisite for the existing tuning story 06F9XD1T3TJK7NEBYNVT2JEPZW and tasks 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- Split recommendation: If bridge-traversal cleanup verification expands beyond a quick bounded smoke/read rerun, create a separate validation-only follow-up instead of widening this baseline ticket into product-code or diagnostics work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8914`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b365619bda694972a619e351a016d9af`
- completed-at-utc: `<redacted>-07T10:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/runs/20260607T104059722Z-b365619bda694972a619e351a016d9af.json`
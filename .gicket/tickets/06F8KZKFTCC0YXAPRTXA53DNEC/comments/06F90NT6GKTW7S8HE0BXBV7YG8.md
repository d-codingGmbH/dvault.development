[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZKFTCC0YXAPRTXA53DNEC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZKFTCC0YXAPRTXA53DNEC`.
- Optimistic claim succeeded (`expectedRevision=06F8M03BYV6WSRBJKPGQK8S8Z8`, `currentRevision=06F90K52WEN685WC0H140NSFD4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZKFTCC0YXAPRTXA53DNEC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZKFTCC0YXAPRTXA53DNEC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d' from source 'a6714d403e0d05a5cb9bae30a4fbf546a973b667'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d` as `c9479166b7da`.

Open questions / Risiken
- If the v0.28.0 docs overstate skipped optional-provider rows as measured live benchmarks, the release note will misrepresent current repository evidence.
- If only README and performance profiles are updated while active architecture/checklist guidance remains unchanged, adopters will continue to receive conflicting provider-matrix instructions.
- External-provider read behavior still depends on consumer-managed provider configuration and explicit PIT/bridge maintenance; the docs must avoid suggesting turnkey runtime enablement where the repository only documents diagnostics-gated and maintenance-dependent paths.
- Split recommendation: No split recommended; current evidence supports one coordinated documentation-baseline update across the existing current-baseline surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8065`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `675e3d6e9d764250959707fb033fba19`
- completed-at-utc: `<redacted>-04T01:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/runs/20260604T015459324Z-675e3d6e9d764250959707fb033fba19.json`
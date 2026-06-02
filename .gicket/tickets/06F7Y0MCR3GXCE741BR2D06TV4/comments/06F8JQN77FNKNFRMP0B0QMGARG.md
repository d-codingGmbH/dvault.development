[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F7Y0MCR3GXCE741BR2D06TV4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0MCR3GXCE741BR2D06TV4`.
- Optimistic claim succeeded (`expectedRevision=06F8JNRGR1B3F0CES8F86HQ6GC`, `currentRevision=06F8JP2XRD6SV15DAD9M9F8MX0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7Y0MCR3GXCE741BR2D06TV4-task-document-stored-procedure-artifact-boundary' from source 'afb8f3eaa1a945f2df018ab3ac8a01173ab687dd'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F7Y0MCR3GXCE741BR2D06TV4-task-document-stored-procedure-artifact-boundary` as `ca04e5178595`.

Open questions / Risiken
- Risky assumption: This handoff assumes the developer will turn the currently distributed guidance across release notes, checklist, and performance docs into one authoritative reference without leaving conflicting duplicate wording behind.
- Risky assumption: This handoff assumes downstream consumers will treat the new boundary doc as normative and the older `v0.20.0` stored-procedure caveats as supporting history rather than a second competing source of truth.
- Split recommendation: No split needed now; keep this ticket as the single generic artifact-boundary/evidence-gate document, then let downstream provider-documentation tickets cite it rather than duplicating the policy.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8317`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f0117329e0484d28b48775e96e668d8e`
- completed-at-utc: `<redacted>-02T17:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0MCR3GXCE741BR2D06TV4/runs/20260602T172542325Z-f0117329e0484d28b48775e96e668d8e.json`
[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q91V0YGSA6SH9WDS02GH0M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q91V0YGSA6SH9WDS02GH0M`.
- Optimistic claim succeeded (`expectedRevision=06F6XBRM8NFRJSMB0HTDFMK1BR`, `currentRevision=06F6XC1T4A3Z2CWY3K08TY365W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance' from source '15b361b981bdb59e0d05778b451cb92f10f2c373'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance` as `b9d04155e21f`.

Open questions / Risiken
- Blocking finding: The epic's authoritative story is internally inconsistent. The parent epic, release notes, analyzer README, model-first guidance, and generator tests all say the current shipped boundary is satellite-only, but `docs/plans/typed-read-model-generator-contract.m...
- Blocking finding: The epic explicitly says to treat `06F5Q922T5B21GJN49FYN6DJH0` as the contract baseline, but that done child story's persisted delivery contract still scopes PIT/bridge helper generation. As written, the parent points to a baseline contract that disagrees wit...
- Required PO action: Refine the epic so all authoritative contract surfaces tell one story: either explicitly supersede `docs/plans/typed-read-model-generator-contract.md` and the `06F5Q922T5B21GJN49FYN6DJH0` baseline story for v0.22, or reopen/create a follow-up ticket that re...
- Required PO action: Update the epic's baseline references or closure evidence so reviewers do not need to infer that the old PIT/bridge helper contract text is historical while `docs/plans/README.md` still marks it as a current contract.
- Risky assumption: Assuming readers will treat `docs/plans/typed-read-model-generator-contract.md` as historical is risky because `docs/plans/README.md` still lists it under `Current Contracts`.
- Risky assumption: Assuming the done `06F5Q922T5B21GJN49FYN6DJH0` child can remain the active contract baseline is risky while its delivery contract still promises PIT/bridge helper generation.
- Split recommendation: Do not reopen implementation scope for PIT/bridge helper generation inside this epic; keep any future shipped PIT/bridge helper work additive.
- Split recommendation: If reconciling the durable contract surfaces needs work beyond this epic's ticket text, create a small additive follow-up ticket dedicated to superseding or correcting the PIT/bridge helper promises in the current planning contract/baseline story.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9361`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1cca8214ab7744e3991a1a6a93bd2488`
- completed-at-utc: `<redacted>-28T13:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/runs/20260528T131428327Z-1cca8214ab7744e3991a1a6a93bd2488.json`
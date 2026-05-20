[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGQ27NWVZ1B1R651S7SM4M/description.md contains `## Open Questions` -> `- none` and PO handoff `decision: ready_for_po_critic`, but no explicit `tracking-only`, `closure-only`, or `no-work-required` designation.
- The local relation files `.gicket/relations/4M/84/06F2PGQ27NWVZ1B1R651S7SM4M--06F2PGQ6T5TGNWCBQBX3700D84--parentOf.json`, `.gicket/relations/4M/24/06F2PGQ27NWVZ1B1R651S7SM4M--06F2PGQBGNZPEEJE4KBET4JG24--parentOf.json`, `.gicket/relations/4M/74/06F2PGQ27NWVZ1B1R651S7SM4M--06F2PGQJ7THHNSYYBFFPBG4174--parentOf.json`, and `.gicket/relations/4M/CM/06F2PGQ27NWVZ1B1R651S7SM4M--06F2PGQQJB5FJGDB16M2G7CPCM--parentOf.json` bind the epic to the four named child tickets.
- `git log --oneline --max-count=15` on branch `ticket/06F2PGQ27NWVZ1B1R651S7SM4M-epic-observability-and-operations` shows the last substantive commits are child auto-integrations `0a462e934`, `08b515c47`, `f60212a7e`, and `800d3512d`; the newer commits are only PO/PO-critic handoff and lease workflow commits.
- `git rev-parse HEAD` and `git rev-parse 2bb7f91ff458c271af65d197d23cca9bcd8d7d65` both resolve to `2bb7f91ff458c271af65d197d23cca9bcd8d7d65`, so there is no additional parent-owned implementation diff beyond the reviewed branch snapshot.
- Repository evidence matches the stated observability boundary: `README.md` documents opt-in telemetry and consumer-owned `support-bundle`; `docs/model-first-governance.md` and `docs/production-adoption-checklist.md` point to v0.16.0 as the current baseline; `src/DCoding.Data.DVault/DataVaultTelemetryServiceCollectionExtensions.cs` exposes `AddDVaultTelemetry()`, `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` parses `support-bundle`, and `src/DCoding.Data.DVault/DataVaultSupportBundle.cs` sets `CurrentSchemaVersion = "dvault.support-bundle.v1"`.

Blocking findings
- none

Required PO actions
- Rewrite the delivery contract to state explicitly that ticket 06F2PGQ27NWVZ1B1R651S7SM4M is a tracking-only closure/no-work-required epic and that no parent-owned implementation slice remains beyond the four named child tickets.
- If any work still belongs to the parent epic beyond the four done children, materialize that work as a separate child or follow-up ticket before resubmitting to PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite the delivery contract to state explicitly that ticket 06F2PGQ27NWVZ1B1R651S7SM4M is a tracking-only closure/no-work-required epic and that no parent-owned implementation slice remains beyond the four named child tickets.
- critic-item-2 [required-po-action] If any work still belongs to the parent epic beyond the four done children, materialize that work as a separate child or follow-up ticket before resubmitting to PO-critic.

Missing examples / edge cases
- The contract does not include an explicit closure-only example for the case where all child tickets are done and the parent epic has no direct implementation work.
- If the listed Follow-Up Questions are intended deferred scope rather than optional future ideas, they need concrete follow-up ticket ids instead of remaining as prose questions.

Risky assumptions
- The current contract assumes the four done children are the complete epic scope even though the parent ticket never explicitly states that the parent has zero remaining implementation work.
- The current contract assumes the historical `blocks` relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 is harmless hygiene noise and will not confuse later closure/reporting automation.

AC / test suggestions
- Add an epic-level acceptance criterion that explicitly says this is a tracking-only closure epic and is complete when the four named child tickets are `done` and the cited repository baseline still matches.
- Record the audit anchors directly in the contract for future closure review: the four child ids plus repository/commit evidence such as `README.md`, `docs/releases/v0.16.0.md`, `docs/model-first-governance.md`, `docs/production-adoption-checklist.md`, `0a462e934`, `08b515c47`, `f60212a7e`, and `800d3512d`.

Implementation watchouts
- Future observability follow-ups should preserve the current documented boundary: `AddDVault()` stays telemetry-free by default, telemetry stays explicit opt-in, and `support-bundle` stays consumer-owned.
- Any future maintenance-service telemetry or operator troubleshooting work should stay outside this epic and should reuse the diagnostics fallback vocabulary already exposed in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`.

Non-blocking notes
- The repository evidence for the shipped v0.16.0 observability slice is strong; the blocker here is contract/routing clarity, not missing source or missing docs.
- The historical relation `.gicket/relations/B4/4M/06F2PGP7HM8F39K3J0H5JHB3B4--06F2PGQ27NWVZ1B1R651S7SM4M--blocks.json` still exists while source epic 06F2PGP7HM8F39K3J0H5JHB3B4 is `done`; that is ticket hygiene noise, not a current blocker by itself.

Split recommendations
- No new child split is needed for the shipped v0.16.0 observability work itself.
- If the PO decides the follow-up questions are required scope, create separate follow-up tickets for troubleshooting examples, PIT/bridge maintenance telemetry, or historical relation cleanup instead of reopening this epic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the latest refinement removed the unsupported inferred-API blocker and the remaining contract is now source-backed, bounded, and has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/description.md` persists `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `- none`, so the explicit dev-handoff gate is satisfied.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `8e481e8d5e71a0c182c74f3a9bfc9a8dc6f58882`, and `git log --oneline --decorate -8 -- .gicket/tickets/06FF43Y6JE9NQWTAQRQXV2YS80` shows the current PO handoff commit `e7db2392ac` after the earlier PO-critic return commit `d9408d1340`, so this review is evaluating a revised contract after the prior blocker.
- `.gicket/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/comments/06FG7CZ2V8G2VVC9571GBFX0K8.md` explicitly marks `critic-item-1`, `critic-item-2`, and `critic-item-3` as `answered` and states that the ticket no longer assumes an already-existing participant-specific public explain API.
- `git -C /mnt/c/Projects/DVault diff --name-only d9408d1340..e7db2392ac -- .gicket/tickets/06FF43Y6JE9NQWTAQRQXV2YS80` shows only ticket description, ticket metadata, comments, and events changed in the latest refinement cycle, which is expected for a pre-development PO gate.
- `src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs` `WriteLinks(...)` writes ordered link `participants` with `hub` plus optional `role`, and the same file's `ValidateLinkParticipants(...)` rejects repeated same-hub participants that lack role-bearing metadata.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs` verifies `CustomerIdentityMatch` with logical participant names `SourceCustomer` and `MatchedCustomer`, produced columns `SourceCustomerHashKey` and `MatchedCustomerHashKey`, and deterministic rejection of missing or duplicate repeated same-hub roles.
- `src/DCoding.Data.DVault/DataVaultExplainDiagnostics.cs` exposes the public explain section through `IReadOnlyList<DataVaultEntityExplain> Entities`, and `src/DCoding.Data.DVault/DataVaultEntityExplain.cs` exposes entity/property/index/constraint data plus `ProducedName`; no ordered participant member is present on these visible public types today.
- `rg -n "type public sealed class DCoding\.Data\.DVault\..*Explain|ParticipantExplain|LinkParticipantExplain" tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt src/DCoding.Data.DVault` returned public explain types such as `DataVaultEntityExplain` and `DataVaultExplainDiagnostics` but no participant-specific explain type, so the revised ticket correctly frames that surface as additive work to create.
- `src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs` exports `DataVaultDiagnosticsResult` directly into deterministic redacted `dvault.support-bundle.v1` JSON, which matches the contract's framing around exported runtime explain output rather than checked-in artifact files.
- `docs/architecture/dvault-v1-typed-row-mapper-contract.md`, `src/DCoding.Data.DVault/IDataVaultLinkMapper.cs`, and `src/DCoding.Data.DVault/DataVaultLinkParticipantBindingAttribute.cs` still keep same-hub typed link mappings out of scope, which aligns with the ticket's fact-only boundary.
- Repository inspection confirmed `dvault.model.v1`, `dvault.support-bundle.v1`, `diagnostics.explain`, and `diagnostics.readShape` are absent at the repository root, matching the contract's note that this work targets exported runtime explain output rather than a checked-in artifact baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete `dvault.support-bundle.v1` JSON example for one repeated same-hub link and one ordinary distinct-hub link would better lock field names and nesting for the new participant facts.
- A collision example where two logical participant roles normalize to the same produced name or column name would make the deterministic failure behavior more explicit.
- An explicit compatibility example showing how existing consumers can ignore the new additive participant facts while continuing to read current explain entity/property data would reduce downstream ambiguity.

Risky assumptions
- Implementers will preserve both logical participant identity and produced-name linkage; exporting only one of those dimensions would leave repeated same-hub links ambiguous.
- Existing support-bundle consumers tolerate additive explain-surface growth without depending on exact fixed property sets.
- Downstream teams will keep same-hub typed mapper or generator parity as a separate follow-up instead of widening this ticket's scope into runtime mapper behavior.

AC / test suggestions
- Add support-bundle serialization coverage for the same repeated same-hub link across code-first, metadata-first, and model-first paths and assert identical participant order and logical naming.
- Add one regression for an ordinary distinct-hub link to prove the new participant facts are additive and backward compatible for existing explain consumers.
- Add deterministic collision and redaction tests that prove ambiguous repeated same-hub logical names fail cleanly and that no raw hash-key values leak through the new participant facts.

Implementation watchouts
- Do not widen `IDataVaultLinkMapper`, `DataVaultLinkParticipantBindingAttribute`, or current compile-time same-hub typed link-mapping behavior in this ticket.
- Do not route this work into request-bound `diagnostics.readShape`, PIT, or bridge helper generation; the current contract keeps it on the support-bundle explain surface only.
- Because `DataVaultSupportBundleExporter` serializes `DataVaultDiagnosticsResult`, the safest contract growth is an additive public explain-surface extension or adjacent public explain contract reachable from it, while keeping participant identity provider-neutral.

Non-blocking notes
- The latest visible ticket comments after the current handoff are PO refinement and bot claim/lease entries; there is no new human discussion debt that changes scope.
- The latest refinement cycle changed ticket metadata only, not implementation files, which is expected at this pre-development gate and is not itself a reason to return the ticket to PO.

Split recommendations
- No split is required for the additive support-bundle explain fact work itself.
- If same-hub typed link-mapper or generator parity is wanted later, keep it as a separate follow-up ticket that consumes these new facts instead of widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
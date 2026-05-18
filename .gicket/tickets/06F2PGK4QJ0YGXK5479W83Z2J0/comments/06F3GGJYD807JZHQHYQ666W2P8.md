[gicket-bot] PO refinement contract

Summary
- Verified local `.gicket` and repository evidence: epic `06F2PGK4QJ0YGXK5479W83Z2J0` already has four done child tickets for same-hub roles, link-parent satellites, effectivity ratification, and v0.13 documentation closure; no new planning writes or relation changes are needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Direct local `.gicket` state shows four existing `parentOf` children, all `done`: `06F2PGKAQVVF8GEZVVC8SHFASG` (Code-First link-parent satellites), `06F2PGKV9AFAMKGJEKKZ3AXHGC` (effectivity ratification on the generic link-parent satellite surface), `06F2PGM1HQ5W1M2H8T50MZ3EEC` (same-hub role-bearing links; dependent child keys explicitly scoped back out), and `06F2PGM9038RXVJH0RJFYEJEV0` (v0.13 documentation and release-note closure).
- The epic still carries forward `blocks` relations to the v0.14 provider bulk-ingestion epic `06F2PGMFWSEC95ATBCGZ6HYT5W` and its scoped tickets `06F2PGMSQ4D4FV8W5ZERD4GS8C`, `06F2PGNGVQ3TZZWSABAK5SNFK4`, `06F2PGN4GPQCGC5WHZQBGP4SD0`, `06F2PGNT7DF4DVNKYWDFZC8DEM`, `06F2PGNZBRNCQ1SV2KKP6F3BA8`, and `06F2PGP2B2RZGGK3CVKK5WRRP8`; those release-ordering links remain valid and were not changed.
- Repository evidence already matches the shipped v0.13 scope: `README.md` and `docs/releases/v0.13.0.md` document repeated same-hub roles and link-parent satellites, `DataVaultCodeFirstLinkBuilder` exposes `Participant<TEntity>(string role)` and `Satellite<TSatellite>(...)`, and tests cover role-based column naming, explicit save behavior, and link-parent satellite projection.
- The prompt snapshot lists no recent human comments, and local `.gicket` search found no attachment bound directly to this epic; no new attachment or planning document was needed for refinement.

Scope In
- Aggregate the existing v0.13 Code-First parity child scope: repeated same-hub links through explicit relationship names plus distinct `Participant<TEntity>(string role)` roles.
- Aggregate Code-First link-parent satellite support through `Link(...).Satellite<TSatellite>(...)` with the existing `Payload(...)` and optional `DrivingKey(...)` selector rules.
- Ratify effectivity for v0.13 as a caller-owned modeling pattern on top of generic link-parent satellites rather than as a separate runtime surface.
- Keep the release-facing documentation baseline aligned to v0.13 across `README.md`, `docs/model-first-governance.md`, and `docs/releases/v0.13.0.md` while preserving metadata-first and model-first as supported alternatives.

Scope Out
- Dependent child key modeling, including any new link-key metadata, hash contract, public API, or v0.13 release claim for that capability.
- Any effectivity-specific fluent API, metadata kind, annotation family, validator, or technical-column expansion beyond the existing generic link-parent satellite baseline.
- Same-hub typed link-mapper or source-generator parity beyond the shipped role-bearing metadata and explicit save path.
- Provider bulk-ingestion work tracked by the separate v0.14 epic and its child tickets.
- Retrofitting the runnable quickstarts away from their current metadata-first posture unless a later separate examples ticket explicitly scopes that change.

Open questions
- none

Follow-up questions
- If product still wants dependent child key modeling, should it be created as a separate post-v0.13 story with its own metadata, hashing, save, and documentation contract?
- Should same-hub typed link-mapper and source-generator parity be tracked as a separate follow-on ticket instead of being inferred from the shipped role-bearing metadata support?
- Should a later examples ticket add runnable Code-First same-as or link-parent/effectivity samples beyond the README and release-note coverage already shipped in v0.13?

Risks
- The done child story `06F2PGM1HQ5W1M2H8T50MZ3EEC` still has a broader title that mentions dependent child keys; without this epic-level clarification, reviewers could overread the v0.13 public claim set.
- Same-hub typed mapper/source-generator parity and effectivity-specific APIs are easy to over-assume because the underlying role-bearing metadata and generic link-parent satellite support now exist.
- Future cleanup that removes the valid forward `blocks` relations to the v0.14 bulk-ingestion work would weaken the intended release-ordering signal even though those downstream tickets are outside this epic's delivery scope.

Split recommendations
- No additional split is recommended; the epic already has the necessary direct children for same-hub roles, link-parent satellites, effectivity ratification, and v0.13 documentation closure.
- If dependent child key modeling remains desired, create a separate follow-on ticket instead of reopening `06F2PGM1HQ5W1M2H8T50MZ3EEC` or widening this epic.
- Track same-hub typed mapper/source-generator parity or runnable same-as/effectivity examples as separate follow-on work rather than extending the v0.13 parity epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
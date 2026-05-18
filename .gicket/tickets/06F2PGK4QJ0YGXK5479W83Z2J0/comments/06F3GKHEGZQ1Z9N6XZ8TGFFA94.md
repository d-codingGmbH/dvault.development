[gicket-bot] PO refinement contract

Summary
- Reframed the parent epic as a tracking-only closure ticket with no parent-owned implementation slice, anchored to four already-done child tickets and unchanged v0.14 release-ordering relations.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved the tracking-epic closure audit by explicitly treating this parent as closure/tracking only: its audited delivery boundary is the four already-done direct children, the shipped v0.13 parity surface is already present in repository code, tests, and docs, and the forward `blocks` relations to the v0.14 bulk-ingestion work remain valid release-ordering context rather than unfinished parent work.
- critic-item-2: `answered` - The contract now explicitly marks this ticket as a tracking-only closure epic with no direct parent-owned implementation, test, or documentation slice. The parent aggregates and closes already-delivered child work only; it does not own an additional implementation slice beyond that tracking boundary.

Clarifications
- This ticket is a tracking-only closure epic for the v0.13 Code-First parity expansion and owns no direct parent-level implementation, test, documentation, attachment, or relation-edit slice.
- The completed delivery boundary is the four existing done direct child tickets: `06F2PGKAQVVF8GEZVVC8SHFASG` (Code-First link-parent satellites), `06F2PGKV9AFAMKGJEKKZ3AXHGC` (effectivity ratification on the generic link-parent satellite surface), `06F2PGM1HQ5W1M2H8T50MZ3EEC` (same-hub role-bearing links; dependent child keys remain deferred), and `06F2PGM9038RXVJH0RJFYEJEV0` (v0.13 documentation and release-note closure).
- The existing forward `blocks` relations to the v0.14 provider bulk-ingestion epic `06F2PGMFWSEC95ATBCGZ6HYT5W` and its scoped tickets `06F2PGMSQ4D4FV8W5ZERD4GS8C`, `06F2PGNGVQ3TZZWSABAK5SNFK4`, `06F2PGN4GPQCGC5WHZQBGP4SD0`, `06F2PGNT7DF4DVNKYWDFZC8DEM`, `06F2PGNZBRNCQ1SV2KKP6F3BA8`, and `06F2PGP2B2RZGGK3CVKK5WRRP8` remain valid release-ordering context and were not changed.
- Repository evidence already matches the shipped v0.13 baseline in `README.md`, `docs/releases/v0.13.0.md`, `docs/model-first-governance.md`, `src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs`, `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

Scope In
- Tracking and closure of the already-delivered v0.13 parity work across four done direct child tickets for same-hub role-bearing links, link-parent satellites, effectivity ratification, and v0.13 documentation closure.
- Ratifying the shipped repository baseline for `Participant<TEntity>(string role)`, `Link(...).Satellite<TSatellite>(...)`, explicit repeated same-hub naming plus roles, and effectivity as caller-owned link-parent satellite state.
- Preserving the valid forward release-ordering context to the separate v0.14 bulk-ingestion epic and its child tickets.

Scope Out
- Any new parent-owned API, code, tests, docs, attachment, planning-document, or relation-edit implementation work.
- Dependent child key modeling, effectivity-specific APIs, same-hub typed link-mapper or source-generator parity, and runnable Code-First same-as or effectivity examples.
- Provider bulk-ingestion work tracked under epic `06F2PGMFWSEC95ATBCGZ6HYT5W` and its scoped tickets.

Open questions
- none

Follow-up questions
- If product still wants dependent child key modeling, should it be created as a separate post-v0.13 story with its own metadata, hashing, save, and documentation contract?
- Should same-hub typed link-mapper and source-generator parity be tracked as a separate follow-on ticket instead of being inferred from the shipped role-bearing metadata support?
- Should a later examples ticket add runnable Code-First same-as or link-parent/effectivity samples beyond the README and release-note coverage already shipped in v0.13?

Risks
- The broader child title on `06F2PGM1HQ5W1M2H8T50MZ3EEC` can still be overread as including dependent child keys unless the tracking-only boundary is preserved.
- Future edits that reintroduce parent-owned implementation asks into this epic would blur closure tracking versus child-owned delivery.
- Removing the valid forward `blocks` relations to the v0.14 bulk-ingestion work would weaken the intended release-ordering signal.

Split recommendations
- No additional split is recommended; the parent is now explicitly a closure/tracking epic over four completed direct children.
- If dependent child key modeling remains desired, create a separate follow-on ticket instead of reopening `06F2PGM1HQ5W1M2H8T50MZ3EEC` or widening this epic.
- Track same-hub typed mapper/source-generator parity or runnable Code-First same-as or effectivity examples as separate follow-on work.

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
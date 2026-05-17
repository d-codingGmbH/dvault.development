[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a bounded ratification: v0.13 effectivity uses the existing Code-First link-parent satellite surface and generic satellite metadata, with documentation cleanup left on the existing documentation task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified live `.gicket` state directly: the story remains under epic `06F2PGK4QJ0YGXK5479W83Z2J0`, still blocks doc task `06F2PGM9038RXVJH0RJFYEJEV0` and same-as/dependent-child story `06F2PGM1HQ5W1M2H8T50MZ3EEC`, and still has incoming `blocks` relations from done tickets `06F2PGKAQVVF8GEZVVC8SHFASG` and `06F2PGHJAFMH80TZAMANQWH9PW`; no relation cleanup was materialized in this pass.
- Current branch source already exposes `DataVaultCodeFirstLinkBuilder.Satellite<TSatellite>(...)`, and `DataVaultCodeFirstModelBuilder` projects those declarations into `DataVaultSatelliteMetadata` with `Parent.Kind = Link`.
- Current model/runtime metadata is generic rather than effectivity-specific: `DataVaultSatelliteMetadata` only carries payload names, optional driving-key names, and the existing `HashDiff`/`LoadTimestamp`/`RecordSource` technical columns, and `DataVaultPropertyRole` has no effectivity-specific role.
- Model-first and artifact contracts already align with that generic stance: `dvault.model.v1` satellites are hub-parent or link-parent declarations, not a separate effectivity category.
- Repository evidence already covers the reusable pattern across code-first link-satellite translation, model-artifact export/import, explicit save requests, and latest-satellite read paths for link-parent satellites.
- `README.md` and `docs/plans/fluent-code-first-api-contract.md` still describe a narrower Code-First surface; updating that v0.13 narrative remains on `06F2PGM9038RXVJH0RJFYEJEV0`, and no child tickets, attachments, or planning documents were created in this run.

Scope In
- Ratify effectivity satellites in v0.13 as caller-owned link-parent satellites declared through existing `Link(...).Satellite<TSatellite>(...)` plus `Payload(...)` and optional `DrivingKey(...)` selectors.
- Keep effectivity on the existing generic satellite metadata and persistence surface: parent kind `Link`, standard satellite technical columns, explicit registry save requests, and generic latest/as-of satellite read APIs.
- Give downstream work one bounded architecture stance: effectivity is a modeling pattern on top of existing link-parent satellite support, not a separate DVault entity family.

Scope Out
- A new `EffectivitySatellite(...)` fluent API, new effectivity-specific metadata/entity kinds, or new technical columns/annotations beyond the current satellite baseline.
- A mandated effectivity payload schema or fixed member names; caller-owned CLR types and selector names remain the model boundary.
- Widening `DataVaultSaveServiceTypedExtensions.CreateOrdinaryHubSatelliteRegistrySaveRequest(...)` to link-parent or driving-key satellite shapes.
- Same-as links, dependent child keys, repeated same-hub participant roles/aliases, PIT/bridge interactions, or documentation authoring already tracked on `06F2PGM9038RXVJH0RJFYEJEV0`.

Open questions
- none

Follow-up questions
- Should `06F2PGM9038RXVJH0RJFYEJEV0` add one canonical docs example that names the effectivity pattern explicitly, even though the runtime surface stays generic?
- If adopters want convenience beyond explicit registry save requests, should link-parent and/or driving-key typed satellite save helpers become a separate post-v0.13 ticket?
- If a future release needs effectivity-specific validation or sugar, should that be introduced as a separate additive API instead of broadening the generic satellite contract?

Risks
- The main immediate risk is documentation drift: `README.md` and the old fluent planning doc still understate the live Code-First surface and could make reviewers think effectivity is unsupported.
- The story title can invite over-design; without this contract, implementation could incorrectly introduce effectivity-specific metadata, columns, or builder verbs that the current repository architecture does not need.
- Typed save-helper limitations are easy to over-assume because generic link-parent satellite save/read support exists while the convenience helper still rejects link-parent and driving-key shapes.

Split recommendations
- No additional split is recommended from current evidence; keep this ticket as a bounded contract/ratification story around the existing generic link-parent satellite surface.
- If product later wants first-class effectivity-specific APIs, validators, or typed-helper convenience, create separate follow-on tickets instead of reopening the generic satellite baseline.
- Keep README/release-note cleanup on `06F2PGM9038RXVJH0RJFYEJEV0`.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
[gicket-bot] PO-critic review contract

Summary
- Ticket 06F5Q922T5B21GJN49FYN6DJH0 is sufficiently refined for developer handoff as a contract-authoring story: the persisted delivery contract is structured, `## Open Questions` is `none`, and the repository already contains the PIT/bridge, compiled-model, model-first, and metadata-annotation evidence the story says the developer must synthesize into the authoritative v1 generator contract.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q922T5B21GJN49FYN6DJH0/description.md:31-42 defines acceptance criteria and definition-of-done for a contract-authoring task, and description.md:51-52 shows `## Open Questions` = `none`.
- git log --oneline -n 6 on `ticket/06F5Q922T5B21GJN49FYN6DJH0-story-define-typed-read-model-generator-contract` shows the PO refinement/handoff chain `c0bc8cc29 -> 0b30ed2a8 -> 5da7a48e2 -> 831ef955a`; `git diff c0bc8cc29..0b30ed2a8 -- .gicket/tickets/06F5Q922T5B21GJN49FYN6DJH0/description.md` shows the ticket was expanded from a 1-line goal into the current 72-line delivery contract.
- docs/architecture/dvault-v1-pit-bridge-boundary.md:26-36 and :52-60 already define the bounded PIT and bridge read surfaces the story references, including runtime link-parent PIT limits, hub-parent-only `dvault.model.v1` PIT artifacts, hierarchy `maximumDepth`, exact generated bridge column names, and provider-neutral diagnostics boundaries; lines 91-106 enumerate the current unsupported v1 cases.
- docs/architecture/dvault-ef-compiled-compatibility.md:8-12 and :49-79 already define the compiled-model and compiled-query boundary the story references, including `UseModel(...)`, stable generated table/column names, and the explicit exclusion of dynamic request-built `IDataVaultReadService` compilation.
- docs/model-first-governance.md:11-19 plus src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:13-80 and src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs:7-61 / DataVaultReadServiceBridgeExtensions.cs:17-75 show the local evidence base for input normalization, metadata-source fingerprints, current/as-of helper semantics, and exact-name bridge projection helpers that the contract story is supposed to consolidate.
- .gicket/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/ticket.json and .gicket/tickets/06F5Q92R02HB7FCE1AWKXPTMRW/ticket.json both exist as downstream `todo` stories, which matches the parent/child split described in description.md:5, :49, and :64.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add an explicit example showing how repeated same-hub participants or endpoint roles affect generated member names for bridge projections.
- Add an explicit example covering the runtime-only link-parent PIT case versus hub-parent-only `dvault.model.v1` PIT artifacts so cross-input-mode validation is easy to review.
- Add an explicit example for hierarchy bridge `maximumDepth` plus `TraversalDepth` mapping.
- Add an explicit accepted/rejected example for shared-driving-key multi-active PITs versus incompatible driving-key families.

Risky assumptions
- The developer implementing this story will carry forward the asymmetry from docs/architecture/dvault-v1-pit-bridge-boundary.md:30-36 and :91-106 instead of silently treating all PIT shapes as available from all input modes.
- The eventual contract will treat `current` and `asOf` helpers as convenience forms over the latest-satellite pipeline, consistent with src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs:7-61, rather than inventing new satellite semantics.
- The stale-fingerprint diagnostics can be specified against the existing `MetadataSourceKind` and `MetadataSourceFingerprint` annotations in src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:72-80 without requiring a separate new PO story first.

AC / test suggestions
- Include an acceptance example that registry-backed PIT as-of reads stay unsupported even though registry-backed PIT maintenance resolution exists.
- Include an acceptance example for stale authoritative metadata fingerprint mismatch across metadata-first, model-first, and `UseModel(...)` compiled-model code-first flows.
- Include an acceptance example showing exact generated column-name access for bridge projectors, including `TraversalDepth` on hierarchy bridges.

Implementation watchouts
- Keep the work at contract-authoring scope; do not let downstream implementation expand into automatic PIT/bridge maintenance, background refresh, or `SaveChanges` orchestration.
- Keep dynamic request-built `IDataVaultReadService` compilation out of the generated-helper contract; the compiled-query note explicitly leaves those cases on the dynamic read APIs.
- Preserve the distinction between logical metadata names and exact generated table/column names when the contract defines naming and projection rules.

Non-blocking notes
- Branch history is ticket-metadata-only so far: `git diff --name-only c0bc8cc29..HEAD` lists `.gicket/tickets/06F5Q922T5B21GJN49FYN6DJH0/**` changes only. That is acceptable for this pre-development contract-authoring story.

Split recommendations
- Keep the current split: this parent story defines the contract, 06F5Q92AHG0ZCTVQGC6NAYVP9C handles latest/as-of satellite projector implementation, and 06F5Q92R02HB7FCE1AWKXPTMRW handles PIT/bridge projector implementation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
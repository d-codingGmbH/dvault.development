[gicket-bot] PO-critic review contract

Summary
- Persisted PO contract is bounded and repo evidence confirms this is an additive expansion over a currently unsupported PIT baseline; no unresolved PO-level questions remain, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q9102970H1VQN16QWRGQX0/description.md contains PO Handoff decision ready_for_po_critic and '## Open Questions' set to '- none'.
- Comments under .gicket/tickets/06F5Q9102970H1VQN16QWRGQX0/comments/ are workflow/refinement records only; 06F6JEWSEFRQRQ9S3JA0590MHC.md publishes the bounded PO refinement contract and no later comment adds unresolved design questions.
- git diff --name-only 1b9f305ce..HEAD returns only .gicket/tickets/06F5Q9102970H1VQN16QWRGQX0/** paths, so the owner branch currently changes ticket metadata only and no repository implementation files yet.
- README.md currently documents PIT reads as exposing ParentHashKey, LoadTimestamp, and declared satellite segments (around line 325) and still says the supported PIT maintenance baseline does not add multi-active PITs (around line 788).
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, src/DCoding.Data.DVault/DataVaultPitMaintenanceShapeValidator.cs, and src/DCoding.Data.DVault/DataVaultPitReadPipeline.cs all still reject multi-active PIT references, confirming the story is a real contract expansion over the current baseline.
- src/DCoding.Data.DVault/DataVaultPitProjectionRow.cs currently exposes only ParentHashKey and LoadTimestamp in the PIT-row exact-name space, and src/DCoding.Data.DVault/DataVaultPitReadPipeline.cs keys matched PIT rows by ParentHashKey, which matches the ticket risk note that current PIT reads assume at most one visible row per parent.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add an explicit example/test where a tuple first appears in one multi-active satellite while ordinary satellites already have parent-wide history, to confirm no PIT row exists for that tuple before first visibility and parent-wide snapshots still join correctly afterward.
- Add an explicit example/test for two visible tuples under the same parent at one asOf cutoff so record multiplicity and expected deterministic ordering are unambiguous.
- Add an explicit rejection example for reference/metadata contradictions such as IsMultiActive=false on a resolved driving-key satellite, or identical driving-key names with incompatible canonical order.

Risky assumptions
- The bounded v1 decision to keep DataVaultPitAsOfReadRequest parent-hash-key only is acceptable for initial consumers even when one parent fan-outs into many tuple rows.
- Automation or downstream workflow will not mis-handle the historical incoming blocks relation from done story 06F5Q90KC6JGQPSP285XQYSPK8, since the live ticket snapshot itself shows isBlocked=false.
- Updating README, PIT guidance, production-adoption guidance, and the active release notes will be enough to retire the current multi-active-PIT-unsupported message consistently across public docs.

AC / test suggestions
- Verify tuple-aware PIT primary key/index shape and typed projection exact-name exposure with public API snapshot coverage, not only behavioral tests.
- Add SQLite integration coverage for mixed ordinary-plus-multi-active PITs where late-arriving rows revise only the targeted tuple series during MaintainParentsAsync(...).
- Assert deterministic failures for duplicate references, incompatible driving-key families/order, cross-product-requiring shapes, and reserved-name collisions with ParentHashKey or LoadTimestamp.

Implementation watchouts
- Current PIT support is cross-cutting: translation, maintenance, read pipeline, typed projection, diagnostics, docs, and public API snapshots all presently encode the non-multi-active baseline.
- DataVaultPitReadRecord and DataVaultPitProjectionRow currently center PIT-row identity on ParentHashKey plus LoadTimestamp; tuple identity exposure must stay additive so ordinary PIT callers do not regress.
- DataVaultPitReadPipeline.ReadMatchedPitRowsAsync currently stores one matched row per ParentHashKey and orders output only by ParentHashKey, so tuple-aware reads need a deterministic multi-row-per-parent identity/order rule.

Non-blocking notes
- The branch is still pre-development: direct inspection shows only .gicket ticket metadata changed since develop, which is acceptable at this gate because the persisted contract is the artifact under review.
- The contract already preserves a clear split boundary: tuple filters, incompatible driving-key families, cross-product semantics, and provider-specific optimization remain out of scope for this story.

Split recommendations
- No split is needed if this story stays bounded to one shared canonical driving-key family across referenced multi-active satellites.
- If consumers need tuple-filter request parameters, incompatible driving-key-family support, cross-product semantics, or provider-specific optimization, keep those as separate follow-up tickets rather than expanding this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
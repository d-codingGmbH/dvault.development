[gicket-bot] PO-critic review contract

Summary
- Ticket contract is ready for developer handoff: it has no open questions, names the five-provider baseline directly against repository evidence, and anchors naming, provider-profile, load-timestamp, and diagnostics work to existing repository contracts and source surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments; the visible thread is bot claim/lease/refinement/handover automation and does not add human scope changes or reopen the story.
- `git diff --name-status b9dcfcc2d712e922e9e3089fb1df1bf192572828..HEAD -- .` returned no repository changes in the review worktree.
- `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs` creates provider capability profiles for `Sqlite`, `Oracle`, `Postgres`, `SqlServer`, and `MySql`, matching the ticket's explicit finite supported-provider baseline.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` already exposes provider guardrail-relevant fields `MaximumIdentifierLength`, `AllowsIndexesCoveredByPrimaryKey`, and `UnsupportedIncludedIndexColumnMode`; concrete profiles already differ (`mysql-pomelo-v1` sets `maximumIdentifierLength: 64` and ignore-included-columns, `oracle-v1` sets `allowsIndexesCoveredByPrimaryKey: false`).
- `docs/naming/default-naming-policy.md` defines the current logical naming baseline with deterministic PascalCase normalization, finite reserved-word sets, and deterministic collision suffixing, which matches the ticket's instruction to ratify rather than reopen naming policy.
- `docs/plans/dvault-model-v1-schema-contract.md` fixes the only current provider-relevant schema tokens to `provider-default`, `iso-8601-utc-text`, and `utc-ticks`, and already requires naming collision diagnostics (`DMV1401`).
- `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` expose the traceability/diagnostics anchors named in the ticket (`ProducedName`, `MetadataName`, `ProviderProfile`, `ProviderLogicalPropertyKind`, `ProviderStorageType`, `ProviderValueFormat`, plus explain fields such as `MaximumIdentifierLength`, `AllowsIndexesCoveredByPrimaryKey`, and `UnsupportedIncludedIndexColumnMode`).
- `src/DCoding.Data.DVault/DataVaultActivityTracing.cs` already carries provider tags plus bounded failure kind/class vocabulary, supporting the ticket's diagnostics boundary.
- Related blocker ticket `06F8KZKFTCC0YXAPRTXA53DNEC` was read directly and is `done`, so the sequencing risk noted in this ticket is historical rather than a current PO blocker.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not include a worked example for a logical name that is reserved, then over-length, then collides after truncation; one example would make rewrite ordering easier to verify in tests.
- The contract does not illustrate a provider that distinguishes identifier limits by object class (table vs index vs constraint), even though Scope In correctly says those distinctions must be covered.
- The contract does not show an explicit example of when a generated key/index/constraint should be renamed versus fail fast after provider validation.

Risky assumptions
- Implementers will treat `docs/naming/default-naming-policy.md` as the logical-name source for Data Vault tables/columns and will not conflate it with the separate snake_case record-persistence artifact names in `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- The existing provider capability profile surface can absorb any additional reserved-word and per-object-limit facts without needing a new public override API, consistent with Scope Out.
- The finite five-provider matrix can be implemented against current package behavior without first creating a separate version-pinning maintenance contract, despite the follow-up drift question.

AC / test suggestions
- Add one acceptance-test example per supported provider for a reserved word, an over-length generated name, and a post-truncation collision, with expected logical-to-physical mapping and expected fail-fast output when mapping is unsafe.
- Add explicit test coverage for object-class-specific identifier limits so tables, indexes, keys, and constraints are each validated against the right provider rule.
- Verify all three `loadTimestampStorage` tokens across the five provider profiles, including load timestamp and satellite snapshot reference mapping plus any provider-specific DDL caveat wording.
- Assert diagnostics against existing bounded vocabulary: metadata/logical name, provider profile, failure class, and remediation boundary.

Implementation watchouts
- Keep logical names provider-neutral and reversible; provider-specific quoting or shortening should only affect physical names and must not break `ProducedName`/`MetadataName` traceability.
- Do not widen scope into automatic rewriting of consumer-authored migrations, raw SQL, or a new public override surface; the ticket explicitly excludes those paths.
- Current provider profiles are already asymmetric: MySql has a 64-character identifier limit and ignores included index columns, while Oracle disallows indexes covered by primary keys; the contract needs to account for those existing differences.
- Build on the existing logical naming baseline and collision rules instead of replacing them with provider-specific logical naming.

Non-blocking notes
- No unresolved `## Open Questions` remain in the persisted delivery contract.
- No PO-level split is required; if engineering sizing grows, splitting after handoff by provider-profile data, validation/migration enforcement, and diagnostics/tests is still reasonable.

Split recommendations
- No PO split required for this ticket.
- If development sizing expands, split downstream implementation into provider-profile data, EF/migration guardrail enforcement, and diagnostics/test coverage.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
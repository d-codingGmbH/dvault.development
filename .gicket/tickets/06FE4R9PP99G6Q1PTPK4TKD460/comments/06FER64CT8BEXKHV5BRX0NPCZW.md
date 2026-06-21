[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the ticket is a clear definition-only architecture-contract story with closed open questions and repository-backed baseline seams.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R9PP99G6Q1PTPK4TKD460/description.md marks the story as definition-only, not product-code implementation, and its Open Questions section says none.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers provider-neutral AddDVault() defaults including IDataVaultSaveService and IDataVaultReadDiagnosticsService.
- src/DCoding.Data.DVault/IDataVaultSaveService.cs and src/DCoding.Data.DVault/IDataVaultReadDiagnosticsService.cs expose the explicit save and read-diagnostics seams that the ticket says the privacy add-on must compose with.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs both call services.AddDVault() and then add provider-specific strategy registrations, matching the additive provider-neutral seam described in the ticket.
- docs/architecture/dvault-v1-explicit-save-service.md states the default write boundary is explicit IDataVaultSaveService and that SaveChanges interceptors remain outside the default v1 persistence path.
- docs/architecture/dvault-v1-pit-bridge-boundary.md states provider-specific read strategies are diagnostics-gated and unsupported providers or shapes fall back to provider-neutral reads.
- git diff --name-only da6a5676c..HEAD lists only .gicket/tickets/06FE4R9PP99G6Q1PTPK4TKD460/*, so the branch currently contains ticket and handoff metadata only, which is acceptable for this pre-development gate.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The future architecture contract should show one concrete explicit opt-in activation example so downstream work does not reinterpret the add-on as a default runtime behavior change.
- The future contract should include at least one negative example of out-of-scope responsibilities such as key lifecycle orchestration, retention scheduling, or compliance attestation to keep later tickets from widening the boundary.
- Terminology choice between GDPR, DSGVO, or dual wording is still a follow-up question and should be normalized in the eventual document.

Risky assumptions
- The story assumes privacy capabilities can stay additive to the existing AddDVault() and metadata/service seams without forcing a new platform layer or implicit persistence path.
- The story assumes any provider-specific privacy behavior can remain behind provider package seams without weakening the shared provider-neutral contract.
- The story assumes consumers will accept application-owned responsibility for credentials, key lifecycle, deployment, transactions, and deletion or retention operations.

AC / test suggestions
- Have the architecture contract cite the current baseline paths directly: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs, src/DCoding.Data.DVault/IDataVaultSaveService.cs, src/DCoding.Data.DVault/IDataVaultReadDiagnosticsService.cs, docs/architecture/dvault-v1-explicit-save-service.md, and docs/architecture/dvault-v1-pit-bridge-boundary.md.
- Require the deliverable to state both sides of the opt-in rule: what explicit enablement looks like and what unchanged default behavior looks like for existing callers.
- Require a review check that the document makes no compliance-guarantee, KMS-ownership, automatic-deletion, background-scheduler, or provider-specific DDL promises.

Implementation watchouts
- Keep the privacy lane caller-driven and explicit; do not let later work redefine the feature primarily through SaveChanges interception, background jobs, or implicit orchestration.
- Do not recast stable hashing, telemetry, or diagnostics surfaces as encryption, key management, or compliance controls.
- Do not promise provider-specific storage behavior on the shared surface until separate provider evidence exists.

Non-blocking notes
- git log --oneline --decorate -n 12 -- .gicket/tickets/06FE4R9PP99G6Q1PTPK4TKD460 docs/architecture src/DCoding.Data.DVault shows the PO handoff commits bb96ee459 and 3e58a1cb0, followed by the PO-critic lease claim 92ad85a6b.
- No repository doc or product-code implementation for this ticket is present yet, which is acceptable because the ticket is explicitly a definition-only boundary story.

Split recommendations
- No additional split is required before developer handoff; keep this ticket as the single privacy-boundary contract lane.
- Use the already-separated follow-on tasks or new capability-specific tickets for concrete features such as field-level encryption, pseudonymization, redaction or export controls, retention metadata, or provider-native encryption investigation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined as a bounded documentation-alignment ticket anchored to the existing opt-in privacy proof, privacy diagnostics/preflight facts, and finite provider-boundary language; no child tickets, relation changes, attachments, or planning documents were materialized during refinement.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already ratifies the v1 default: `DCoding.Data.DVault.Privacy` is an explicit opt-in, provider-neutral package for alias-driven encrypted payload conversion over ordinary EF Core mapped payload properties, not a compliance or automatic privacy feature.
- For this ticket, the release-note trail should stay historically consistent: `docs/releases/v0.48.0.md` carries the concrete privacy adoption/preflight improvements, while `docs/releases/v0.49.0.md` is the current package/support-bundle baseline and still keeps automatic privacy execution out of scope.
- Provider-native encryption language stays guidance-only for the finite repository-backed provider baseline of SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2; MySQL follows the repository MySQL profile rather than a separate MariaDB capability profile.
- No bounded ticket writes were needed during refinement.

### Scope In
- Align README, `docs/getting-started.md`, `examples/README.md`, `docs/package-compatibility.md`, and `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` so they use the same bounded description of the optional privacy seam.
- Keep public diagnostics/adoption language aligned with the existing privacy proof facts: alias coverage statuses, key-provider posture, and advisory versus fail-closed diagnostics.
- Keep release-note and changelog wording concrete about privacy adoption improvements while preserving the no-compliance-automation and no-provider-native-encryption boundary.
- Update package-verification expectations only if packaged README installation or analyzer guidance changes.

### Scope Out
- New runtime privacy behavior, new `DCoding.Data.DVault.Privacy` APIs, or changes to save/read execution semantics.
- Any GDPR/DSGVO compliance guarantee, legal attestation, or automation claim.
- Provider-specific native encryption implementation, encrypted DDL, provider SQL crypto integration, capability probing, or runtime dispatch based on native encryption availability.
- Deletion, retention, backup purge, legal-erasure, PIT cleanup, bridge cleanup, or DVault-owned key lifecycle workflows.
- NuGet publication automation or package-line/version changes beyond keeping docs consistent with the current `8.50.0` and `10.50.0` baseline.

## Acceptance Criteria
- README, `docs/getting-started.md`, `examples/README.md`, `docs/package-compatibility.md`, and `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` consistently describe `DCoding.Data.DVault.Privacy` as optional, explicit opt-in, provider-neutral, and alias-driven over ordinary EF Core mapped payload properties.
- Public docs that describe privacy diagnostics or adoption use the existing bounded facts: alias coverage `covered`/`registered-but-unmapped`, key-provider posture `none`/`marker-only`/`encrypted-payload-capable`, and advisory `personal-data-privacy-proof-missing` versus fail-closed `personal-data-privacy-coverage-unusable` behavior.
- All privacy-facing doc surfaces keep provider-native encryption references as guidance-only for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2, and explicitly avoid claims about encrypted DDL, provider SQL crypto calls, capability probing, native-encryption runtime routing, or GDPR/DSGVO compliance automation.
- The release-note/changelog trail describes the concrete privacy adoption improvements already evidenced in the repository, including privacy preflight coverage reporting, the quickstart privacy proof, and adoption-checklist guidance, without implying default runtime privacy behavior or compliance ownership.
- If README install or analyzer guidance changes, package-verification expectations remain aligned with the shipped README wording for the `8.50.0` / `net8.0` and `10.50.0` / `net10.0` package lines.

## Definition of Done
- All in-scope documentation surfaces and the relevant release-note/changelog entries are internally consistent on privacy scope, provider boundary, and non-goals.
- Any packaged README wording change is reflected in `tools/DCoding.Data.DVault.PackageVerification` so the package-verification lane still validates current install guidance.
- No public doc in this ticket claims automatic privacy execution, provider-native encryption behavior, or GDPR/DSGVO compliance automation.
- The ticket can proceed without additional PO decisions because the bounded privacy baseline, provider list, and package-line baseline are already ratified by repository evidence.

## Implementation Notes
- Use the current repository baseline rather than reopening architecture choices: README, Getting Started, Examples README, Package Compatibility, Production Adoption Checklist, and the privacy boundary document already converge on the opt-in provider-neutral privacy proof.
- Treat `docs/releases/v0.48.0.md` as the concrete privacy adoption/preflight source and `docs/releases/v0.49.0.md` as the current package/support-bundle baseline that keeps automatic privacy execution out of scope.
- If install blocks or analyzer guidance in README change, update the package verifier expectations that currently enforce dual-line install commands, stale-version rejection, and `.NET 10 SDK` analyzer build-host guidance.
- No bounded child tickets, relation updates, description rewrites, attachments, or planning documents were created during this refinement.

## Open Questions
- none

## Follow-Up Questions
- If a future provider-specific native encryption capability is approved, document it through a separate provider-owned ticket and release-note lane instead of widening this provider-neutral documentation ticket.
- When the next package-line baseline changes after `8.50.0` / `10.50.0`, keep README, package compatibility, release notes, and package-verifier expectations updated in one pass to avoid stale install guidance regressions.

## Risks
- README wording drift without a matching package-verifier update will break the packaging/verification lane because packaged README content is validated.
- Future doc edits could blur the repository release label `v0.49.0` with consumer package versions `8.50.0` and `10.50.0`, reintroducing stale-version guidance errors.
- Future privacy doc edits may overstate provider-native encryption or compliance unless the current guidance-only boundary and finite provider list remain synchronized across surfaces.
- Because privacy adoption details are anchored in `v0.48.0` while `v0.49.0` is the current package/support-bundle baseline, careless edits could create contradictory release-note history if that split is not preserved.

## Split Recommendations
- No split recommended; this is a bounded documentation-alignment task with an optional package-verifier touch-up only if README wording changes.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update public privacy documentation after the boundary matrix, support-bundle facts, and example are in place.

Acceptance:
- README, Getting Started, examples README, package compatibility docs, and architecture boundary docs agree on the optional privacy scope.
- Docs mention provider-native encryption only as caller-owned/deployment-owned capability guidance unless a future provider-specific feature implements it.
- The release notes describe concrete privacy adoption improvements without claiming GDPR/DSGVO compliance automation.
- Package verifier expectations are updated if packaged README guidance changes.
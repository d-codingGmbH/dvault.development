[gicket-bot] PO refinement contract

Summary
- Refined as a bounded documentation-alignment ticket anchored to the existing opt-in privacy proof, privacy diagnostics/preflight facts, and finite provider-boundary language; no child tickets, relation changes, attachments, or planning documents were materialized during refinement.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already ratifies the v1 default: `DCoding.Data.DVault.Privacy` is an explicit opt-in, provider-neutral package for alias-driven encrypted payload conversion over ordinary EF Core mapped payload properties, not a compliance or automatic privacy feature.
- For this ticket, the release-note trail should stay historically consistent: `docs/releases/v0.48.0.md` carries the concrete privacy adoption/preflight improvements, while `docs/releases/v0.49.0.md` is the current package/support-bundle baseline and still keeps automatic privacy execution out of scope.
- Provider-native encryption language stays guidance-only for the finite repository-backed provider baseline of SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2; MySQL follows the repository MySQL profile rather than a separate MariaDB capability profile.
- No bounded ticket writes were needed during refinement.

Scope In
- Align README, `docs/getting-started.md`, `examples/README.md`, `docs/package-compatibility.md`, and `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` so they use the same bounded description of the optional privacy seam.
- Keep public diagnostics/adoption language aligned with the existing privacy proof facts: alias coverage statuses, key-provider posture, and advisory versus fail-closed diagnostics.
- Keep release-note and changelog wording concrete about privacy adoption improvements while preserving the no-compliance-automation and no-provider-native-encryption boundary.
- Update package-verification expectations only if packaged README installation or analyzer guidance changes.

Scope Out
- New runtime privacy behavior, new `DCoding.Data.DVault.Privacy` APIs, or changes to save/read execution semantics.
- Any GDPR/DSGVO compliance guarantee, legal attestation, or automation claim.
- Provider-specific native encryption implementation, encrypted DDL, provider SQL crypto integration, capability probing, or runtime dispatch based on native encryption availability.
- Deletion, retention, backup purge, legal-erasure, PIT cleanup, bridge cleanup, or DVault-owned key lifecycle workflows.
- NuGet publication automation or package-line/version changes beyond keeping docs consistent with the current `8.50.0` and `10.50.0` baseline.

Open questions
- none

Follow-up questions
- If a future provider-specific native encryption capability is approved, document it through a separate provider-owned ticket and release-note lane instead of widening this provider-neutral documentation ticket.
- When the next package-line baseline changes after `8.50.0` / `10.50.0`, keep README, package compatibility, release notes, and package-verifier expectations updated in one pass to avoid stale install guidance regressions.

Risks
- README wording drift without a matching package-verifier update will break the packaging/verification lane because packaged README content is validated.
- Future doc edits could blur the repository release label `v0.49.0` with consumer package versions `8.50.0` and `10.50.0`, reintroducing stale-version guidance errors.
- Future privacy doc edits may overstate provider-native encryption or compliance unless the current guidance-only boundary and finite provider list remain synchronized across surfaces.
- Because privacy adoption details are anchored in `v0.48.0` while `v0.49.0` is the current package/support-bundle baseline, careless edits could create contradictory release-note history if that split is not preserved.

Split recommendations
- No split recommended; this is a bounded documentation-alignment task with an optional package-verifier touch-up only if README wording changes.

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
[gicket-bot] PO refinement contract

Summary
- Refined the DB2 provider-capability contract story as the architecture gate for epic 06F9G8GH969DQXD7WZ8JHD1GRR. Repository evidence shows DB2 is not part of the current five-provider baseline, the epic is already adequately split across downstream children, and no persistent planning writes were applied in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket remains the contract-definition prerequisite under epic 06F9G8GH969DQXD7WZ8JHD1GRR and continues to block package story 06F9G8GZ384VKA7RVF039WKX1M until the DB2 dependency and capability contract is defined.
- Repository evidence is finite today: DataVaultProviderCapabilityProfiles, DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles, DataVaultProviderCapabilityProfileSelection, and KnownProviderNames only cover SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, so DB2 must be introduced as an explicit supported profile and not by fallback.
- The incoming blocks relation from done epic 06F9G8EE7ZA666MW8YEB2QP8BW is treated as historical compatibility-baseline evidence, not as a remaining PO blocker for this story.
- The DB2 epic is already adequately decomposed into this contract story plus package, schema/guardrail, integration, package-verification, and documentation children; no new child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

Scope In
- Define the authoritative DB2 dependency contract for DVault: IBM.EntityFrameworkCore 8.0.0.400 for net8.0 and 10.0.0.100 for net10.0, aligned with the downstream 8.34.0 and 10.34.0 DVault DB2 package lines described by sibling tickets.
- Define the exact DB2 provider-name detection contract used by registration, capability selection, diagnostics, and model-artifact/profile selection.
- Define the DB2 capability-profile facts expected by DataVaultProviderCapabilityProfile: profile name, logical-property type mappings, load-timestamp mapping behavior, identifier-length or escaping caveats, included-index handling, and duplicate-index or primary-key behavior.
- Define the DB2 compatibility posture for DVault-owned schema generation, migration guardrails, diagnostics, provider-neutral save and read behavior, and live-schema proof, including any explicit unsupported boundaries that must fail fast or stay documented as unsupported.
- Define the external DB2 test posture as opt-in external-provider evidence only, with developer-managed database and container lifecycle outside DVault.

Scope Out
- Implementing the DB2 provider package, service registration, or solution and project wiring; that belongs to story 06F9G8GZ384VKA7RVF039WKX1M.
- Implementing DB2 schema, naming, live-schema, or migration-guardrail code and tests; that belongs to story 06F9G8H5HE1CJHQXGC2C2YK7P8.
- Implementing DB2 save and read integration coverage; that belongs to story 06F9G8HBXS7Y42J7XFSQKZ2AZ8.
- Updating package-verifier expectations; that belongs to task 06F9G8HJJDJH4KF9VK6TZ8B1Z0.
- Updating README, release notes, and adoption guidance; that belongs to task 06F9G8HRZ72XP5Z7FNWM6MBMQC.
- Adding DB2-specific benchmark claims, provider-specific SQL artifact support, platform provisioning, migration execution, or runtime orchestration beyond the documented DVault provider patterns.

Open questions
- none

Follow-up questions
- After the contract lands, should the documentation task standardize the exact DB2 opt-in environment-variable name and example local connection-string workflow so it matches the existing external-provider README pattern?
- If DB2 live-schema proof cannot match the existing Postgres, SqlServer, Oracle, and MySql reader boundary in v1, should a later follow-up ticket add DB2 live-schema reader support after baseline provider support ships?
- After baseline DB2 support is implemented, is there any need for DB2-specific performance or provider-specific SQL-artifact planning, or should DB2 remain provider-neutral outside the core support lane?

Risks
- IBM DB2 provider behavior may diverge from the existing five-provider assumptions on identifier length, generated DDL, included indexes, or live-schema introspection, so the contract must record explicit caveats instead of implying parity.
- Because the repository currently treats unknown providers as fallback rather than explicit support, an incomplete DB2 contract could let downstream implementation accidentally inherit unsupported SQLite-oriented behavior or misleading diagnostics.
- DB2 validation will depend on opt-in external database availability and developer-managed lifecycle, so proof beyond default local SQLite and smoke coverage may remain environment-sensitive even after the contract is defined.
- The live relation set still includes a historical incoming blocks edge from done epic 06F9G8EE7ZA666MW8YEB2QP8BW; if tracker automation interprets done-source blocks strictly, that relation may need later housekeeping even though it is not a PO blocker here.

Split recommendations
- No additional split is recommended. Epic 06F9G8GH969DQXD7WZ8JHD1GRR already separates the DB2 work into this contract story plus package, schema and guardrail, integration, package-verification, and documentation children.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
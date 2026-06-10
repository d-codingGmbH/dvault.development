[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket 06F9G8GS08VNH0DT09Q4PC2HRC is done and is now the authoritative DB2 contract baseline for this story, so dependency, provider-name, and package-line decisions are already settled for PO purposes.
- The live child split under epic 06F9G8GH969DQXD7WZ8JHD1GRR already covers package, schema and guardrails, integration, package verification, and documentation, so no additional child tickets were created in this run.
- Repository evidence shows a current seven-package solution and five-provider capability baseline; this story is the bounded lane that introduces the eighth provider package and its registration surface, not a reopen of the DB2 architecture contract.

Scope In
- Create the packable multi-target provider project DCoding.Data.DVault.Db2 and include it in DVault.slnx using the same package metadata, readme, license, and symbol conventions as the existing provider packages.
- Add an AddDVaultDb2() startup extension that registers IBM.EntityFrameworkCore against the DB2 capability profile, calls AddDVault(), and adds the DB2 provider behavior or registration services needed for provider selection and diagnostics.
- Wire the current codebase's explicit-provider surfaces for DB2 where this package story owns them: provider capability profile exposure, provider-name selection and registration, known-provider diagnostics, and model-artifact or provider-profile availability required by the DB2 package.
- Pin the provider dependency per target framework with conditional IBM.EntityFrameworkCore references: 8.0.0.400 for net8.0 and 10.0.0.100 for net10.0, aligned with the planned 8.34.0 and 10.34.0 DVault DB2 package lines.
- Make the DB2 package and dependency shape explicit enough that the downstream package-verification task can validate the new artifact without reopening package identity or version decisions.

Scope Out
- DB2 identifier rules, DDL guardrails, migration-operation diagnostics, and live-schema reader behavior; those stay with story 06F9G8H5HE1CJHQXGC2C2YK7P8.
- DB2 save and read execution proof, opt-in external database coverage, and strategy behavior evidence; those stay with story 06F9G8HBXS7Y42J7XFSQKZ2AZ8.
- Comprehensive package verifier updates such as package counts, README or XML documentation checks, symbol checks, and dependency assertions; those stay with task 06F9G8HJJDJH4KF9VK6TZ8B1Z0.
- README, release-note, adoption-guide, and external DB2 setup documentation changes; those stay with task 06F9G8HRZ72XP5Z7FNWM6MBMQC.
- Any DB2-specific benchmark, provider-specific SQL artifact, provisioning, container lifecycle, or CI-infrastructure commitments.

Open questions
- none

Follow-up questions
- none

Risks
- If the package adds partial DB2 wiring but misses one of the finite provider-name or profile lists, the repository can fall back to SQLite-oriented defaults or incomplete diagnostics for DB2 contexts.
- Current packaging and version-matrix surfaces still encode a seven-package, 8.33.0 / 10.33.0 baseline, so DB2 package work and the dedicated verification task must land coherently to avoid broken package validation.
- The existing outgoing blocks relation from this ticket to 06F9G8H5HE1CJHQXGC2C2YK7P8 means downstream schema and live-schema guardrail work remains sequenced after this package lane even though PO refinement is complete.
- DB2 live execution remains external opt-in and environment-sensitive, so package-level success alone will not prove live schema or provider-read behavior until the sibling integration and schema tickets land.

Split recommendations
- No additional split is recommended; epic 06F9G8GH969DQXD7WZ8JHD1GRR already separates DB2 work into contract, package, schema and guardrails, integration, package verification, and documentation tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
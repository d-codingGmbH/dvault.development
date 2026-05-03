[gicket-bot] PO refinement contract

Summary
- Refined the parent automated-test-strategy story against the current repository baseline; the existing child split (06EXB80FPE3REH11RQ1YR6BW1G, 06EXB80QQHAYH61RY4X3T1E8S0) remains sufficient and no new planning artifacts were required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already establishes the v1 test layout under tests/DCoding.Data.DVault.Tests with Unit, Integration, and Shared slices, so the story should ratify that structure rather than invent a new taxonomy.
- This parent story already has child relations to 06EXB80FPE3REH11RQ1YR6BW1G and 06EXB80QQHAYH61RY4X3T1E8S0 and currently blocks 06EXB8202A88KJJP7WEGBESBYM; refinement should treat those links as the active delivery split.
- Visible repository evidence shows local SQLite-focused integration coverage and a Postgres-specific opt-in integration configuration pattern; external-provider checks stay optional and must not be part of the default no-service test run.
- Provider packages without configured external database coverage still need bounded smoke coverage such as registration, API surface, discovery, or package-load validation.

Scope In
- Document and enforce the v1 automated test categories for DVault: unit coverage, local SQLite-backed integration coverage, shared provider-test support, and provider-package smoke coverage.
- Cover the repository baseline called out in the ticket: metadata/model translation, stable hashing behavior, EF model building, convention-first registration, explicit save flows, SQLite integration behavior, and provider registration/package smoke checks.
- Keep opt-in external provider verification within scope only as configuration-gated coverage that is clearly separated from default local automation.

Scope Out
- Requiring PostgreSQL, SQL Server, Oracle, or MySQL servers for the default automated test run.
- Expanding this story into provider-specific performance tuning, non-MVP Data Vault capabilities, or broad product-code changes outside test strategy and test coverage.
- CI environment provisioning or rollout policy beyond making the intended default-versus-opt-in test boundary explicit.

Open questions
- none

Follow-up questions
- After the parent story lands, should SQL Server, Oracle, and MySQL each get their own opt-in external integration harness ticket, or should they remain smoke-only until provider priorities change?
- Does blocked ticket 06EXB8202A88KJJP7WEGBESBYM need a later CI or developer-documentation follow-up to describe how optional provider runs are invoked when environments are available?

Risks
- Because the story mixes strategy documentation with broad test implementation scope, it can sprawl unless contributors keep the work constrained to the existing child-ticket split.
- External-provider expectations may become inconsistent across packages if the repository does not clearly label which checks are smoke-only and which are true configured integration tests.

Split recommendations
- No additional split is required in this refinement pass because the parent story already has two child tickets; keep new provider-specific live-database work in separate future tickets instead of widening this story.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
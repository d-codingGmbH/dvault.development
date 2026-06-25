[gicket-bot] PO-critic review contract

Summary
- Repository onboarding docs are aligned with the library-first adoption story, but the parent ticket still has an unresolved child-decomposition mismatch around archived duplicate child 06FF43V3NVWER898D8CKXJ74D8, so the tracking parent is not ready for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- README.md:68,97 and docs/getting-started.md:20-21,37,79,134 keep the shortest onboarding path SQLite-first and binary-first, gate PostgreSQL behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, and keep writes/reads explicit through `IDataVaultSaveService` and `IDataVaultReadService`.
- examples/README.md:5,8,167,177,196 positions the SQLite and PostgreSQL quickstarts as runnable companion proofs, with PostgreSQL opt-in behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING` and explicit save/read flow.
- src/DCoding.Data.DVault.Analyzers/README.md installation guidance states the analyzer is optional, version-aligned to `8.47.0`/`10.47.0`, and built with a `.NET 10 SDK` host; docs/architecture/dvault-dotnet-ef-design-time-workflow.md:10 says DVault does not provide `IDesignTimeServices` or a custom `dotnet ef` shim; docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:10 keeps privacy opt-in.
- The live relation file .gicket/relations/A0/D8/06FF43REXXX4R9WKNCKDXP4RA0--06FF43V3NVWER898D8CKXJ74D8--parentOf.json still makes archived ticket 06FF43V3NVWER898D8CKXJ74D8 a parent child, so the parent has four `parentOf` children in the repository even though the contract names only three.
- Branch history on 2026-06-25 includes commit `dccc7f5405` with subject `[06FF43V3NVWER898D8CKXJ74D8] audit-only mutation outbox po (duplicate-retirement)`, and .gicket/archive/06FF43V3NVWER898D8CKXJ74D8/comments/06FFY7FT6TEW3YPY4CJHZS0XQR.md says that ticket duplicates done story `06FBSBW6HDT15D1KGVD7XBQXM8`.
- Despite that archive evidence, the current parent comment .gicket/tickets/06FF43REXXX4R9WKNCKDXP4RA0/comments/06FFYW995ZK15E5K8S5980AJQ8.md reports the `parentOf` follow-up for 06FF43V3NVWER898D8CKXJ74D8 as `[blocked]` because the ticket could not be read.

Blocking findings
- This tracking-parent ticket still has unresolved child coverage hygiene: the live parent relation set includes archived duplicate child 06FF43V3NVWER898D8CKXJ74D8, while the delivery contract claims the story is fully materialized by three other children. That mismatch leaves the completion tree ambiguous.

Required PO actions
- Clean up or supersede the live `parentOf` relation from 06FF43REXXX4R9WKNCKDXP4RA0 to archived duplicate 06FF43V3NVWER898D8CKXJ74D8 so the parent's child set matches the intended tracked decomposition.
- Add explicit parent-level ticket evidence explaining how the retired duplicate maps to the accepted analyzer coverage, including whether done story 06FBSBW6HDT15D1KGVD7XBQXM8 is only historical evidence or must be tracked as a formal related dependency.
- After the relation/evidence cleanup, rerun PO-critic so the tracking-parent closure audit can evaluate one unambiguous child set.

Open issues ledger
- critic-item-1 [required-po-action] Clean up or supersede the live `parentOf` relation from 06FF43REXXX4R9WKNCKDXP4RA0 to archived duplicate 06FF43V3NVWER898D8CKXJ74D8 so the parent's child set matches the intended tracked decomposition.
- critic-item-2 [required-po-action] Add explicit parent-level ticket evidence explaining how the retired duplicate maps to the accepted analyzer coverage, including whether done story 06FBSBW6HDT15D1KGVD7XBQXM8 is only historical evidence or must be tracked as a formal related dependency.
- critic-item-3 [required-po-action] After the relation/evidence cleanup, rerun PO-critic so the tracking-parent closure audit can evaluate one unambiguous child set.
- critic-item-4 [blocking-finding] This tracking-parent ticket still has unresolved child coverage hygiene: the live parent relation set includes archived duplicate child 06FF43V3NVWER898D8CKXJ74D8, while the delivery contract claims the story is fully materialized by three other children. That mismatch leaves the completion tree ambiguous.

Missing examples / edge cases
- The parent contract does not say how duplicate-retired children are treated during tracking-parent completion checks, which is the exact edge case now affecting 06FF43V3NVWER898D8CKXJ74D8.

Risky assumptions
- Assuming reviewers will infer 06FBSBW6HDT15D1KGVD7XBQXM8 as the replacement analyzer evidence is risky because the parent ticket does not cite that story in its accepted child set.

AC / test suggestions
- Add one parent-level acceptance or closure note that duplicate-retired child tickets do not count toward required child completion once a superseding ticket or archived outcome is cited explicitly.
- If 06FBSBW6HDT15D1KGVD7XBQXM8 is meant to remain relevant evidence, name it directly in the parent contract or closure evidence instead of relying on archived duplicate breadcrumbs.

Implementation watchouts
- If the ticket returns after PO cleanup, keep the current scope boundary intact: docs/examples/analyzers only, PostgreSQL opt-in, consumer-owned design-time flow, explicit save/read boundaries, and privacy remaining optional.

Non-blocking notes
- The repository documentation itself is already aligned with the requested product posture; the blocker is ticket decomposition/traceability, not missing repository content.
- The current branch head is `4cd415fc325112bd8f3cf5c6c5c618dc242e4d60` on `ticket/06FF43REXXX4R9WKNCKDXP4RA0-story-define-library-focused-adoption-path-witho`, and the recent branch-local commits are lease/handoff commits rather than new repository-scope implementation changes.

Split recommendations
- No new implementation split is needed based on repository content. The only required split-level action is to resolve or document away the stale archived-duplicate child relation on the parent ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
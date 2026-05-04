[gicket-bot] PO refinement contract

Summary
- Verified the existing PostgreSQL optimization hierarchy: story 06EZ0N9TJSXFXH0YZRA3QN2S14 remains the parent, sibling task 06EZ0NA180RA0FQ64KXQTHEVZW owns optimized write implementation, and this ticket is narrowed to post-implementation opt-in integration proof with an explicit optimized-path observable. No new child tickets or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Exact parent ticket is story 06EZ0N9TJSXFXH0YZRA3QN2S14 (Story: Optimize PostgreSQL provider save strategy), already linked to this task via parentOf. This child is not independently shippable before the optimized provider exists; within that parent story it is sequenced after sibling task 06EZ0NA180RA0FQ64KXQTHEVZW (Task: Implement PostgreSQL optimized hub/link/satellite writes) and may ship in the same delivery unit only after that implementation is available.
- critic-item-2: `answered` - Acceptance Criteria and Definition of Done are refined so tests must directly prove optimized-path selection, not only persisted behavior: when PostgreSQL is configured and the optimized implementation is present, the suite must confirm AddDVaultPostgres() exposes a compatible IDataVaultProviderSaveStrategy for the clean Npgsql context and that the save leaves no fallback-tracked hub, link, or satellite entities in the active DbContext.
- critic-item-3: `answered` - The overlap is resolved by narrowing this ticket to the child integration-coverage slice under story 06EZ0N9TJSXFXH0YZRA3QN2S14. Story 06EZ0N9TJSXFXH0YZRA3QN2S14 stays the umbrella deliverable, sibling task 06EZ0NA180RA0FQ64KXQTHEVZW owns optimized write implementation, and benchmark evidence remains story-level or later follow-up work rather than part of this ticket.
- critic-item-4: `answered` - The sequencing rule is now explicit: this ticket remains blocked until sibling task 06EZ0NA180RA0FQ64KXQTHEVZW delivers a real PostgreSQL optimized save strategy behind AddDVaultPostgres(). Only after that implementation exists does this child task add green opt-in integration coverage for the optimized path.
- critic-item-5: `answered` - The observable that proves optimized execution is the existing repository pattern from strategy-selection coverage: a compatible provider strategy accepts the clean context before save, and after save the same DbContext shows no fallback-tracked entries that the provider-neutral EF writer would have left behind. Persisted rows and RowsWritten remain necessary behavior assertions, but they are not sufficient proof by themselves.

Clarifications
- Existing parentOf relations already connect story 06EZ0N9TJSXFXH0YZRA3QN2S14 to this ticket and to sibling task 06EZ0NA180RA0FQ64KXQTHEVZW; this ticket is the integration child within that existing split.
- This ticket is blocked on the sibling implementation task inside the parent story, not on new architecture work and not on re-opening provider-neutral fallback semantics.
- The bounded PostgreSQL test harness is already established by DVAULT_TEST_POSTGRES_CONNECTION_STRING, PostgresIntegrationTestConfiguration, NpgsqlProviderReflection, PostgresDataVaultSchemaTests, and the ProviderIntegration.ExternalOptIn category.
- For this ticket, optimized-path proof means strategy compatibility plus no fallback-tracked entries in the active context; it does not require SQL-text snapshot assertions.

Scope In
- Add opt-in PostgreSQL integration tests in tests/DCoding.Data.DVault.Tests/Integration for the optimized save path once the sibling implementation task exposes it.
- Verify representative hub, link, and satellite saves through AddDVaultPostgres() against live PostgreSQL by reusing the existing external opt-in harness.
- Assert unchanged satellite replays do not append a row and changed satellite hash diffs append exactly one new history row while preserving earlier history.
- Assert optimized-path selection directly by checking compatible strategy acceptance and absence of fallback-tracked entries in the same DbContext.

Scope Out
- Implementing the PostgreSQL optimized save strategy code itself; that belongs to sibling task 06EZ0NA180RA0FQ64KXQTHEVZW.
- Benchmark evidence or performance publication; that remains within story 06EZ0N9TJSXFXH0YZRA3QN2S14 or a later child.
- Provider-neutral fallback, unknown-provider, or priority-dispatch coverage already handled by DataVaultSaveStrategySelectionTests.
- Docker or database provisioning automation, mandatory local PostgreSQL validation, or any change that makes Npgsql a default local dependency.

Open questions
- none

Follow-up questions
- After tasks 06EZ0NA180RA0FQ64KXQTHEVZW and 06EZ0NA7CWDYJ7ZS3K5GM0187M land, should story 06EZ0N9TJSXFXH0YZRA3QN2S14 split benchmark evidence into a dedicated child if environment-dependent performance work would otherwise delay story closure?
- Should future non-SQLite provider optimization tickets standardize the same optimized-path observable of compatible strategy acceptance plus no fallback-tracked entries?

Risks
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs currently only delegates to AddDVault(), so this ticket cannot go green until sibling task 06EZ0NA180RA0FQ64KXQTHEVZW lands provider-specific strategy registration.
- Live PostgreSQL validation still depends on externally supplied connectivity and clean per-run isolation; tests must create deterministic schema or data boundaries to avoid flakiness.
- If the suite asserts provider-specific SQL text instead of the selected-path observable and persisted contract, the tests will become brittle without improving the product-level guarantee.

Split recommendations
- No new split is recommended. The existing structure already separates umbrella story 06EZ0N9TJSXFXH0YZRA3QN2S14, sibling implementation task 06EZ0NA180RA0FQ64KXQTHEVZW, and this integration-coverage task.
- If benchmark evidence grows beyond a lightweight story-level activity, create a later benchmark child under story 06EZ0N9TJSXFXH0YZRA3QN2S14 instead of widening this ticket again.

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
[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded opt-in SaveChanges guard interceptor slice for DVault hub/link/satellite direct-write misuse, with no planning writes or relation changes materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes IDataVaultSaveService as the default write boundary, UseDataVaultSaveChangesMetadataInterceptor(...) as an optional metadata-only EF lane, and ordinary EF SaveChanges as something consumers should keep separate from DVault persistence unless they deliberately own generated DVault rows.
- This story should use a new opt-in EF SaveChanges interceptor mode as the implementation lane, not a separate design-time command or broad validation-hook platform.
- The first bounded guard scope is generated DVault hub, link, and satellite entities only; PIT and bridge maintenance stay on their explicit services and are not part of this slice.
- When guard and metadata-fill interceptors are both enabled, guard evaluation must consider the post-fill state so missing LoadTimestamp or RecordSource values that are intentionally auto-populated are not treated as violations.
- Current persisted relations remain one incoming parentOf from epic 06F492A3MPSGP3KXDNZECN01QM and one outgoing blocks to docs task 06F492BNDPWS9P4EDSV0W7G6VM; no relation changes, child tickets, description updates, attachments, or planning documents were materialized in this pass.

Scope In
- Add explicit opt-in DbContextOptionsBuilder registration for a runtime guard interceptor mode that is off by default and separate from AddDVault().
- Detect high-confidence unsafe direct SaveChanges patterns on generated DVault hub, link, and satellite rows, especially direct Modified or Deleted states and Added rows missing required DVault-owned structural values after any configured metadata fill.
- Support at least blocking and warning modes with deterministic explanations consumers can inspect or surface.
- Preserve the documented caller-owned generated-row lane where the row already has required structural data and only relies on optional metadata fill for LoadTimestamp and or RecordSource.
- Add unit and integration coverage for default non-registration, guard findings, warning and block behavior, metadata-fill coexistence, and explicit save-service compatibility.

Scope Out
- No default guard registration on AddDVault() or provider package startup.
- No hash computation, hash-diff computation, row creation, ordering, or replacement of IDataVaultSaveService.
- No PIT or bridge runtime guard coverage in this first slice.
- No analyzer implementation, design-time preflight command work, migration or drift guardrails, or logging platform integration.
- No broad business or payload validation beyond high-confidence DVault structural guard checks.

Open questions
- none

Follow-up questions
- After this slice lands, should PIT and bridge explicit maintenance paths get their own opt-in runtime guard story rather than broadening this story now?
- Should later docs and analyzer work align warning and error wording between this runtime guard story and analyzer story 06F492ARW2N6SNYJH15RHMZEN8?
- If consumers want built-in console or logging integration for warning mode later, should that be a separate observability ticket rather than expanding the core interceptor contract?

Risks
- If guard evaluation runs before metadata-fill behavior or ignores companion interceptor state, it will produce false positives for the documented caller-owned generated-row lane.
- If the guard tries to validate too much business or payload shape instead of high-confidence DVault structural invariants, it will become noisy and undermine the opt-in safety goal.
- If the explanation surface is only exception text with no deterministic structure, docs and tests will struggle to keep warning and blocking modes aligned.

Split recommendations
- No split recommended for this story; current repository evidence supports one bounded runtime guard slice around opt-in hub, link, and satellite SaveChanges misuse detection plus deterministic warning and blocking explanations.
- If future work expands into PIT or bridge guard coverage, richer observability sinks, or analyzer and runtime diagnostic unification, split that into follow-up tickets instead of widening this story.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
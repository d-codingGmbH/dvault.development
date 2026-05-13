[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded compatibility-test task: prove one DVault metadata registration path works when used from an EF Core compiled model and one deterministic supported read path works through EF Core compiled queries, while documenting limitations as test intent rather than expanding scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the repository's visible DVault test layout as the v1 default location for these tests, with the developer choosing the exact test file/class names to match existing conventions.
- The v1 baseline is provider-neutral first; SQLite may be used only where the existing EF test fixture needs a relational provider to exercise the path deterministically.
- The compiled model test is a compatibility proof for DVault metadata/annotations on a compiled EF model, not a full design-time compiled-model generation workflow test.
- The compiled query test should cover one supported read path that already exists in the public/runtime surface, not introduce a new query API.

Scope In
- Add one focused compiled model compatibility test proving DVault metadata registration and produced annotations remain available when the model is consumed as a compiled EF Core model.
- Add one focused compiled query compatibility test proving a supported read path executes deterministically through EF Core compiled query usage.
- Keep tests in the normal automated test suite and aligned with existing test project conventions.
- Include explicit diagnostic assertions or test naming/comments that capture the intended limitations of the supported compiled paths.

Scope Out
- No benchmark matrix or performance measurement work.
- No provider matrix across SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- No provider-specific implementation changes unless an existing fixture requires a provider setup detail.
- No new runtime APIs, design-time services, EF CLI integration, or compiled-model generation tooling.
- No broad proof that every DVault query shape or metadata feature works with EF Core compiled queries/models.

Open questions
- none

Follow-up questions
- Should a later ticket add a provider matrix for compiled query/model coverage after provider release posture is fully aligned?
- Should a later ticket cover EF CLI compiled-model generation and design-time workflows in a consumer sample project?
- Should broader compiled query coverage be added for PIT, bridge, or model-first read paths once those features are stable?

Risks
- EF Core compiled model setup can become brittle if it relies on generated artifacts; keep this ticket focused on deterministic test fixtures and checked-in code only.
- Compiled query support can be overinterpreted as coverage for all LINQ/read shapes unless the supported shape and limitations are explicit.
- Provider-specific behavior may leak into the tests if the fixture setup is not kept close to the existing provider-neutral or SQLite baseline.

Split recommendations
- none

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
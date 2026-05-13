<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded compatibility-test task: prove one DVault metadata registration path works when used from an EF Core compiled model and one deterministic supported read path works through EF Core compiled queries, while documenting limitations as test intent rather than expanding scope.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the repository's visible DVault test layout as the v1 default location for these tests, with the developer choosing the exact test file/class names to match existing conventions.
- The v1 baseline is provider-neutral first; SQLite may be used only where the existing EF test fixture needs a relational provider to exercise the path deterministically.
- The compiled model test is a compatibility proof for DVault metadata/annotations on a compiled EF model, not a full design-time compiled-model generation workflow test.
- The compiled query test should cover one supported read path that already exists in the public/runtime surface, not introduce a new query API.

### Scope In
- Add one focused compiled model compatibility test proving DVault metadata registration and produced annotations remain available when the model is consumed as a compiled EF Core model.
- Add one focused compiled query compatibility test proving a supported read path executes deterministically through EF Core compiled query usage.
- Keep tests in the normal automated test suite and aligned with existing test project conventions.
- Include explicit diagnostic assertions or test naming/comments that capture the intended limitations of the supported compiled paths.

### Scope Out
- No benchmark matrix or performance measurement work.
- No provider matrix across SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- No provider-specific implementation changes unless an existing fixture requires a provider setup detail.
- No new runtime APIs, design-time services, EF CLI integration, or compiled-model generation tooling.
- No broad proof that every DVault query shape or metadata feature works with EF Core compiled queries/models.

## Acceptance Criteria
- A compiled model test verifies the relevant DVault model metadata/annotations are present and correct after the compiled model is used by a DbContext.
- A compiled query test executes one supported read path, returns deterministic results, and validates the expected row/projection values rather than only asserting no exception.
- Both tests run as part of the repository's normal test suite without special external services or manual generation steps.
- Failure messages or assertion structure identify whether the failure is in compiled model metadata availability, compiled query execution, or returned data shape.
- Tests remain provider-neutral where practical and use the existing SQLite-oriented baseline only when a relational fixture is required.

## Definition of Done
- The new tests are committed in the appropriate existing test project and pass with the normal test command used for the repository.
- The tests exercise EF Core compiled model and compiled query APIs directly enough to fail if those supported paths regress.
- Limitations are visible in test names, assertions, or nearby test documentation so future maintainers do not overread the coverage as a provider or query-shape matrix.
- No production behavior is changed except where a genuine defect must be fixed to make the supported compatibility tests pass.

## Implementation Notes
- Start from the existing DVault EF Core test fixtures and naming conventions rather than creating a new test harness.
- Prefer a minimal representative hub/link/satellite model already used in tests when selecting metadata for the compiled model assertion.
- For the read path, choose an already-supported deterministic query surface such as a provider-neutral projected/read model path available in the current codebase; keep setup data small and explicit.
- If SQLite is used, treat it as the existing local relational baseline, not as a SQLite-specific feature claim.
- Do not add Microsoft.EntityFrameworkCore.Design or EF CLI dependencies to the core DVault package for this ticket.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a provider matrix for compiled query/model coverage after provider release posture is fully aligned?
- Should a later ticket cover EF CLI compiled-model generation and design-time workflows in a consumer sample project?
- Should broader compiled query coverage be added for PIT, bridge, or model-first read paths once those features are stable?

## Risks
- EF Core compiled model setup can become brittle if it relies on generated artifacts; keep this ticket focused on deterministic test fixtures and checked-in code only.
- Compiled query support can be overinterpreted as coverage for all LINQ/read shapes unless the supported shape and limitations are explicit.
- Provider-specific behavior may leak into the tests if the fixture setup is not kept close to the existing provider-neutral or SQLite baseline.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Create focused tests proving supported compiled EF Core paths.

## Scope In

- Add one compiled model test for metadata registration.
- Add one compiled query test for a supported read path.
- Capture limitations in assertions or docs.

## Scope Out

- No broad benchmark matrix.
- No provider-specific code unless required for the fixture.

## Acceptance Criteria

- Tests are deterministic and run in the normal suite.
- Failures provide actionable diagnostics.

## Implementation Notes

- Start with provider-neutral tests.

## Open Questions

- none
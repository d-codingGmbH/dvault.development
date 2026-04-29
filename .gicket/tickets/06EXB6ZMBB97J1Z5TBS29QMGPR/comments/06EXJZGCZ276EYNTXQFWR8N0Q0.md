[gicket-bot] PO refinement contract

Summary
- PO clarification resolved: current branch evidence now shows an existing DVault library project, public AddDVault startup entry point, and DVault test structure. The smoke-test ticket is narrowed to the current public AddDVault optionless startup path; UseDataVault/EF provider integration is not a prerequisite for this ticket unless it lands separately before development starts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Do not expand this ticket into setup/API implementation work. The earlier setup blocker is cleared for the current v1 scope because the branch now has the DVault project, AddDVault entry point, and test structure. The ticket should proceed only as an AddDVault minimal-startup smoke test in the existing test suite.
- critic-item-2: `answered` - No new blocking/dependency relation is required for this narrowed smoke-test contract. Relations were verified: the ticket currently has only its incoming parentOf relation, and the previously missing prerequisite project/AddDVault/test scaffold is now present. A future UseDataVault/EF integration ticket may relate to this work, but it is not a blocker for the AddDVault-only v1 smoke test.
- critic-item-3: `answered` - The stale blocker is resolved by updating the handoff contract: this ticket is no longer waiting for a nonexistent broad setup ticket. Development may target the existing src/DVault project and tests/DVault.Tests suite, with the smoke test constrained to the public AddDVault minimal startup path and no repository scaffold work hidden inside the ticket.
- critic-item-4: `answered` - UseDataVault is not treated as a blocking prerequisite for this ticket. The current visible public startup API for v1 smoke coverage is AddDVault; any later public UseDataVault or EF-specific startup surface should be covered by a separate follow-up or an extension to this smoke test only after that API exists without requiring product setup work here.

Clarifications
- Current branch evidence supersedes the stale blocked baseline in the prior contract: src/DVault exists, AddDVault exists, and tests/DVault.Tests exists.
- This ticket remains a smoke-test task and must not create solution scaffolding, library projects, test-suite scaffolding, or new public startup APIs.
- The v1 public startup surface for this ticket is the optionless AddDVault(IServiceCollection) path in src/DVault/DVaultServiceCollectionExtensions.cs.
- UseDataVault is not required for this ticket unless a separate prerequisite implementation lands before development begins and can be exercised without expanding this ticket's scope.
- No child ticket, blocking relation, attachment, or planning document was materialized in this clarification pass because the narrowed current-branch baseline no longer needs a prerequisite setup ticket.

Scope In
- Add a self-contained smoke test under the existing tests/DVault.Tests test structure for the current public AddDVault minimal startup path.
- Verify that default DVault startup succeeds without additional mandatory configuration by building the service collection or host path used by the existing test style.
- Assert observable startup success through public DVault behavior, such as resolving documented convention services after AddDVault registration, without depending on private DI descriptor ordering.
- Keep the test deterministic and free of external databases, network services, and machine-specific infrastructure.

Scope Out
- Creating or renaming solution files, source projects, package metadata, or repository build configuration.
- Creating the production AddDVault implementation from scratch.
- Creating a UseDataVault API, EF provider integration, or DbContext-specific startup surface unless it already exists before development and can be used without product code expansion.
- Provider-matrix coverage, broad configuration regression testing, SQLite integration behavior, and external database integration tests.

Open questions
- none

Follow-up questions
- After the AddDVault smoke test lands, decide separately whether a UseDataVault or EF/provider startup API needs its own implementation ticket and follow-up smoke coverage.
- After the first smoke test lands, decide separately whether to add provider-specific startup coverage or broader configuration regression tests.

Risks
- A test that asserts private DI registration mechanics instead of public startup success may become brittle.
- If later product direction makes UseDataVault mandatory for the minimal startup story, that should be handled as a separate API/integration ticket before expanding this smoke test.

Split recommendations
- No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch now contains the source project, AddDVault entry point, and test structure.
- Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, or provider integration becomes part of the required public startup surface.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
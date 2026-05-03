[gicket-bot] PO refinement contract

Summary
- Verified the live ticket and repository baseline without materializing new child tickets, relations, attachments, or planning documents; the work is already tightly bounded to the six packable DVault packages and is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified persisted relation context: parent story `06EXB80ZNQTTGT6VN2DKEDGB0M` (`Story: Enforce public API quality`), outgoing `blocks` relation to `06EXB81FSWAA6N1HMYQ0CM4S8G` (`Task: Add API approval or compatibility snapshot tests`), and incoming `blocks` relations from `06EXB7HPGW3Y9MSP10DEC8RBK4` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- The two incoming blocker tickets are already `done`, so they do not leave a PO-level dependency question for this ticket.
- Recent persisted comments on this ticket are automation follow-ups only; no human comment or attachment changed scope in this refinement pass.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.

Scope In
- Documentation warning enforcement for the six packable packages: `src/DCoding.Data.DVault/`, `src/DCoding.Data.DVault.Sqlite/`, `src/DCoding.Data.DVault.Postgres/`, `src/DCoding.Data.DVault.SqlServer/`, `src/DCoding.Data.DVault.Oracle/`, and `src/DCoding.Data.DVault.MySql/`.
- Public and protected consumer-facing APIs in those packages, including the visible registration and modeling entry points such as `AddDVault*`, `UseDataVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, and provider capability contracts.
- Retention or consolidation of XML documentation generation and missing-doc enforcement so the package API surface fails clearly when required XML comments are absent.
- Verification that each packable package ships its generated XML documentation file in pack output.

Scope Out
- `src/DCoding.Data/DCoding.Data.csproj`, the unit/integration/shared test projects, and `benchmarks/DCoding.Data.DVault.Benchmarks/`, because they are non-packable and not the release package surface for this ticket.
- API approval or compatibility snapshot testing, which is already separated into downstream ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.
- Provider-specific writer behavior, persistence semantics, or additional provider capability design beyond the existing public API documentation surface.
- Blanket repository-wide suppression of `CS1591` or broad global exceptions for undocumented public APIs.

Open questions
- none

Follow-up questions
- If future examples, benchmarks, or other non-packable projects become externally published or packable, should the same XML-doc enforcement be promoted from the current package-scoped baseline to a broader repository convention?

Risks
- If the enforcement is moved into shared MSBuild files without a packable-project condition, non-packable tests or benchmarks could start failing on unrelated public APIs and create avoidable churn.
- If implementation stops at compile settings and never validates pack output, one package could still miss the shipped XML documentation artifact despite compiling with documentation generation enabled.

Split recommendations
- No additional split is recommended; the parent quality story already separates XML-doc enforcement from downstream API snapshot testing through ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
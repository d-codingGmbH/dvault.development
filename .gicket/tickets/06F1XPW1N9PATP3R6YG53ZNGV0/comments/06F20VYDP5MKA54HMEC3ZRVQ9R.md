[gicket-bot] PO refinement contract

Summary
- Refined the ticket around a focused model-first design-time workflow test and documentation update, using the existing public import, registration, and drift-report APIs with the default SQLite design-time provider baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing unit-test lane as the minimal sample harness; the repository already has public model-first APIs and SQLite-backed unit-test infrastructure, so no new package or standalone example project is needed for v1.
- Treat the happy-path workflow as public API evidence: import a valid dvault.model.v1 artifact, register it through the existing model-first metadata surfaces, and confirm design-time drift comparison reports no blocking differences.
- Use one bounded invalid-artifact baseline for the sample workflow: unsupported schemaVersion producing the stable DMV1002 schema-version diagnostic with logical source path and JSON Pointer evidence.

Scope In
- Add a focused repository test fixture in the existing unit-test project that demonstrates the model-first design-time workflow from import through design-time EF metadata validation.
- Exercise existing model-first registration surfaces already documented in the repo, including import-result-backed DVault registration and DbContext opt-in for design-time projection.
- Assert the design-time validation success path with DataVaultModelDriftReporter.Compare(importResult, context) using the repository's SQLite design-time provider baseline and no blocking drift differences.
- Capture the representative invalid-model outcome through public DataVaultModelImportResult diagnostics, including stable code/category/path evidence for the unsupported schemaVersion case.
- Document the exact repo-root test command and expected valid/invalid outcomes in the model-first governance guidance.

Scope Out
- No new public registration API, importer API, drift API, or diagnostic family.
- No first-party CLI command, build integration, CI workflow, or live database validation lane.
- No new runnable example project under examples unless a later ticket explicitly wants consumer-facing quickstarts beyond the test harness.
- No YAML ingestion work, exporter changes, or broader invalid-artifact matrix beyond the single representative invalid-model case.
- No external service dependency or database creation/query requirement for the sample workflow.

Open questions
- none

Follow-up questions
- Should a later ticket add a runnable model-first quickstart under examples once the team wants consumer-facing sample code in addition to the test harness?
- Should later model-first workflow documentation also include a representative projection-failure case such as DMV1801, or is the initial v1 sample intentionally limited to one valid path plus one invalid schemaVersion path?

Risks
- If the sample uses internal helpers or internal formatting shortcuts instead of public Diagnostics and Compare results, it will be less useful as consumer-facing workflow evidence.
- If the documented command targets brittle individual test names rather than a stable focused workflow test/class, the docs may drift unnecessarily during routine test refactors.
- If the workflow opens or initializes a database instead of staying design-time-only, it will expand beyond the ticket's non-invasive validation intent.

Split recommendations
- If stakeholders also want a consumer-facing quickstart or CLI/build-lane automation, keep that as a follow-up ticket separate from this focused test-harness-and-documentation refinement.

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
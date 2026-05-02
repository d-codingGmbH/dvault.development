[gicket-bot] PO refinement contract

Summary
- Verified persisted ticket state, README, source/test API evidence, and relation context; this parent story already has child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC, no new planning writes were needed, and the story is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Persisted ticket state shows the required PO labels and only automation lease/claim comments; no human clarification comments or ticket attachments currently add competing scope.
- README.md is the owning getting-started surface and is also packed as the package README by src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, so this story should stay centered on that file rather than create a second quickstart document.
- The current v1 baseline is fixed by repository evidence: .NET 10, pre-publication source consumption through src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, optionless AddDVault(), ApplyDataVaultMetadata(...), explicit IDataVaultSaveService/DataVaultSaveRequest writes, and EF shared-type Dictionary<string, object> reads.
- NuGet installation remains future post-publication guidance only; the known package identity is DCoding.Data.DVault, but this story must not claim that a live package install path already exists.
- No new child tickets, relations, attachments, or planning documents were materialized in this refinement run.

Scope In
- Own the root README getting-started path from current pre-publication project reference prerequisite to first DVault-backed DbContext usage.
- Document the current source-consumption prerequisite with a project reference that targets src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and tells consumers to adjust the relative path to their own solution layout.
- Show the minimal current API flow with AddDVault(), ApplyDataVaultMetadata(...), IDataVaultSaveService, DataVaultSaveRequest, and a simple EF shared-type query against generated DVault tables.
- Keep all getting-started text in English and consistent with the current package identity DCoding.Data.DVault and the repository net10.0 baseline.
- Preserve the convention-first, minimal-configuration message and stay within the already-planned child-ticket split between quickstart usage and installation framing.

Scope Out
- Package publication, release automation, live dotnet add package commands, version numbers, feed details, badges, or release-process instructions.
- New library APIs, SaveChanges interception, typed query abstractions, convenience wrappers, or any change to product code.
- Provider-specific walkthroughs, migrations, schema-generation guidance, Postgres-specific setup, or advanced configuration hook coverage.
- Runnable examples under examples/, docs-site expansion, or broader tutorial material beyond the root README getting-started slice.

Open questions
- none

Follow-up questions
- After story 06EXB8202A88KJJP7WEGBESBYM is complete and the package is published, should README installation guidance switch to NuGet-first and move source-reference guidance into a contributor or from-source section?
- Should a later documentation/examples ticket promote the README getting-started flow into a runnable sample under examples/?
- If DVault later gains a higher-level read API, should the README quickstart replace the current shared-type query example?

Risks
- README snippets can drift from the tested API surface if later library changes update code without keeping docs aligned with integration tests.
- Project-reference guidance will confuse future package consumers unless it stays clearly framed as the pre-publication path.
- The shared-type query example may be misread as the long-term preferred consumer API if the docs do not keep calling it the current v1 baseline.
- If future NuGet wording becomes too concrete before publication, the story will emit false or failing install guidance.

Split recommendations
- No additional split recommended; this story already has the right decomposition through child task 06EXB7R6MTJW1PYRN172MW34DM for the README quickstart and child task 06EXB7REMY41DF7RE8J3N1RZYC for project-reference and future NuGet wording.
- Keep the parent story as the umbrella documentation contract and leave release/publication workflow to blocked story 06EXB8202A88KJJP7WEGBESBYM.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
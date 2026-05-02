[gicket-bot] PO refinement contract

Summary
- Verified the ticket against the current README, public DCoding.Data.DVault API, existing SQLite save-service tests, and the persisted parent/block relations. No new planning writes were needed; the ticket is bounded as a root README quickstart slice and is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket has no human clarification comments or persisted attachments beyond automation lease comments, so repository evidence is the authoritative refinement input for this run.
- The quickstart belongs in the root README.md because that is the visible repository documentation baseline on this branch and no separate user-facing quickstart page exists yet.
- The minimal v1 startup path is already fixed by repository evidence: IServiceCollection.AddDVault() registers DVault defaults without a DVault options object.
- The current model-configuration baseline is ModelBuilder.ApplyDataVaultMetadata(...) with a DataVaultMetadataModel composed from DataVaultHubMetadata, DataVaultLinkMetadata, and optional DataVaultSatelliteMetadata declarations; this ticket should ratify that surface instead of reopening a different first-use API.
- The current write boundary is IDataVaultSaveService plus DataVaultSaveRequest with explicit load timestamp and record source values, consistent with the accepted explicit-save-service decision in ticket 06EXB7H6KV753KM125XN3VDRTM.
- Verified relation context: this ticket is a child of story 06EXB7QYF1BB1REM7HQZ4WWVMM and blocks ticket 06EXB7REMY41DF7RE8J3N1RZYC; no new child tickets, relations, attachments, or planning documents were materialized in this refinement.
- Detailed project-reference and future NuGet installation wording is already split into ticket 06EXB7REMY41DF7RE8J3N1RZYC, so this ticket should focus on the usage quickstart and only include minimal transition text if needed.

Scope In
- Add the first minimal-configuration quickstart section to the root README.md.
- Show optionless DVault service registration through AddDVault().
- Show bounded EF model configuration through ApplyDataVaultMetadata(...) with a small DataVaultMetadataModel example.
- Show a small explicit save example through IDataVaultSaveService and DataVaultSaveRequest.
- Show a small follow-up read/query example using the current EF shared-type access pattern against generated DVault tables.
- Keep the quickstart text in English and aligned with the current package identity, namespace, and net10.0 repository baseline.
- Keep documentation snippets aligned with existing tests or add bounded sample-test coverage when a snippet is not already proven by current tests.

Scope Out
- Full project-reference guidance and future NuGet installation guidance beyond the minimal handoff needed for readability; ticket 06EXB7REMY41DF7RE8J3N1RZYC owns that slice.
- New public runtime APIs, typed query abstractions, SaveChanges interception, or other convenience layers beyond the current documented surfaces.
- Runnable example projects under examples/ or broader documentation-site work beyond the root README slice.
- Provider-specific setup, Postgres-specific walkthroughs, migrations, schema generation, or advanced configuration matrices.
- Advanced or extended Data Vault scenarios such as PIT tables, bridge tables, multi-active satellites, or broader satellite-heavy tutorials that are not required for the first-use flow.

Open questions
- none

Follow-up questions
- After ticket 06EXB7REMY41DF7RE8J3N1RZYC lands, should the README quickstart cross-link to a separate installation section or inline only a one-sentence prerequisite?
- Once a higher-level typed read or query surface exists, should the README quickstart be revised away from shared-type Dictionary<string, object> queries?
- Should a later documentation or examples ticket promote the README quickstart into a runnable sample under examples/ once that folder becomes active?

Risks
- If the README example drifts away from the existing tested API shape, the quickstart will become misleading faster than normal code review catches it.
- If the quickstart expands into installation, provider, or publication detail, it will overlap the sibling install ticket and weaken the minimal-configuration message.
- If the query example is presented as a polished long-term read API instead of the current shared-type baseline, users may infer capabilities the package does not yet provide.

Split recommendations
- No new split is recommended; the existing parent story 06EXB7QYF1BB1REM7HQZ4WWVMM and sibling ticket 06EXB7REMY41DF7RE8J3N1RZYC already cover the main planning boundaries.
- Keep this ticket focused on the README usage quickstart slice and leave installation and publication wording to ticket 06EXB7REMY41DF7RE8J3N1RZYC.
- Create a later follow-up only if the team intentionally wants a runnable example project or a new higher-level read API example that no longer fits a README-only change.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
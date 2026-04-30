[gicket-bot] PO refinement contract

Summary
- Refined ticket 06EXB7FPZRCFC33RF2M5SXZTK4 as a bounded v1 task to add the EF Core ModelBuilder entry point that applies existing DVault defaults; no child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Recent comments contain only bot claim and lease metadata, so there are no human clarification comments to resolve.
- Existing relations were verified: parent ticket 06EXB7FF1J9NR2849WKDR8DKPG is parentOf this ticket, and this ticket blocks 06EXB7FYXNBPMH8VGQCGP2R41R.
- No ticket attachments are currently persisted; the referenced repository planning documents are already sufficient context and no new attachment was needed.
- The repository already contains DCoding.Data.DVault as the owning package/root namespace, net10.0 as the current baseline, GenerateDocumentationFile enabled, and CS1591 included in warnings-as-errors.
- The repository already contains a DVaultServiceCollectionExtensions.AddDVault startup entry point and a DCoding.Data.DVault.Modeling.DataVaultModelBuilderExtensions.UseDataVault extension for DVault's internal model builder. This ticket is specifically for the EF Core Microsoft.EntityFrameworkCore.ModelBuilder entry point.

Scope In
- Add a public extension method exposed from the DCoding.Data.DVault package/namespace for EF Core model building, intended to be called from DbContext.OnModelCreating with Microsoft.EntityFrameworkCore.ModelBuilder.
- Apply the existing provider-neutral v1 DVault defaults from the Modeling layer, especially DataVaultConventions.Default and the default naming policy, rather than introducing a new options model.
- Keep the v1 path convention-first and optionless; the extension should be usable without custom configuration.
- Add XML documentation for every new public API so the current documentation-file and CS1591 build policy remains satisfied.
- Add focused unit coverage showing the EF ModelBuilder extension is available, null-safe, returns the same ModelBuilder for fluent use, and applies/records the default DVault conventions expected by the existing Modeling contracts.

Scope Out
- Provider-specific EF Core persistence behavior, SQL dialect choices, migrations, generated schemas, physical column types, and database-specific indexes.
- Advanced configuration hooks, custom naming overrides, custom hashing configuration, provider behavior matrices, or runtime options surfaces beyond the optionless default entry point.
- Changing the existing AddDVault service-registration contract or replacing the existing DCoding.Data.DVault.Modeling.DataVaultModelBuilder API.
- Implementing Data Vault load automation, hash computation beyond the existing stable-hash services, ingestion pipelines, or runtime record-source resolution.
- Creating or changing CI workflows; the existing formatting/build/test gates are sufficient for this ticket.

Open questions
- none

Follow-up questions
- A later provider-specific ticket should decide how these logical DVault conventions map to relational EF Core schema objects, migrations, and database indexes.
- A later advanced-configuration ticket should decide whether an overload accepting naming, hashing, record-source, timestamp, or provider hooks is needed once those hooks are implemented.
- A downstream documentation/API-usage ticket may add README examples for DbContext.OnModelCreating once the extension is implemented.

Risks
- EF Core package version selection must stay aligned with the net10.0 baseline to avoid introducing restore/build drift.
- There is a naming collision risk with the existing Modeling.DataVaultModelBuilderExtensions.UseDataVault method; placing the EF extension in the root namespace and typing it for Microsoft.EntityFrameworkCore.ModelBuilder should keep overload resolution clear.
- Because provider-specific persistence remains out of scope, tests should assert provider-neutral convention application rather than expecting generated SQL or migrations.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a model-level EF contract: ModelBuilder.UseDataVault() must record DataVaultConventions.Default on the EF model through the DVault-owned annotation DCoding.Data.DVault:Conventions, must not translate hubs, links, satellites, keys, indexes, or technical columns, and must update tests around that exact inspection surface. No child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is amended to require one exact observable EF-model artifact: ModelBuilder.UseDataVault() must set model annotation DCoding.Data.DVault:Conventions on modelBuilder.Model, and its value must be the same DataVaultConventions.Default instance. Tests must inspect that annotation directly and assert same-instance wiring.
- critic-item-2: `answered` - This ticket does not perform EF metadata translation for hubs, links, satellites, keys, indexes, or technical columns. Its EF responsibility stops at adding the optionless ModelBuilder entry point and recording the model-level conventions marker. Per-structure EF translation remains owned by blocked ticket 06EXB7FYXNBPMH8VGQCGP2R41R.
- critic-item-3: `answered` - The acceptance criteria and test expectations are updated to match that contract: the EF extension must be discoverable, null-safe, fluent, must set annotation DCoding.Data.DVault:Conventions to DataVaultConventions.Default, and must not materialize per-entity EF metadata from a bare UseDataVault() call.
- critic-item-4: `answered` - The minimal observable EF-model effect is now explicit and bounded: a provider-neutral model annotation on modelBuilder.Model with key DCoding.Data.DVault:Conventions and value DataVaultConventions.Default. That gives dev and test a concrete EF inspection surface without pulling this ticket into downstream metadata translation.

Clarifications
- Repository evidence shows the current non-EF path already defines the default-convention singleton in src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs and applies it by reference in src/DCoding.Data.DVault/Modeling/DataVaultModelBuilderExtensions.cs plus src/DCoding.Data.DVault/Modeling/DataVaultModel.cs.
- The refined contract uses the existing public DataVaultConventions.Default object as the exact EF-facing payload, so the EF entry point mirrors the current internal builder behavior instead of inventing a second defaults object or options surface.
- The required EF inspection surface for this ticket is the provider-neutral modelBuilder.Model annotation surface; no relational or provider-specific annotations, migrations, or schema objects are part of this ticket.
- Blocked ticket 06EXB7FYXNBPMH8VGQCGP2R41R remains the owner of translating hubs, links, satellites, keys, indexes, and technical columns into EF metadata.
- Recent comments contain only bot orchestration metadata; no human clarification comment changed scope, and no child tickets, relations, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add a public optionless extension method in the DCoding.Data.DVault namespace for Microsoft.EntityFrameworkCore.ModelBuilder, intended for DbContext.OnModelCreating.
- Introduce the minimal provider-neutral EF model mutation required by this ticket: set model annotation DCoding.Data.DVault:Conventions on modelBuilder.Model with value DataVaultConventions.Default.
- Keep the path convention-first and zero-configuration; the EF entry point reuses existing DVault defaults rather than introducing custom options, naming overrides, or provider hooks.
- Add XML documentation for every new public API so GenerateDocumentationFile and CS1591 warnings-as-errors remain satisfied.
- Add focused tests that inspect the EF model annotation surface for null-guard behavior, fluent return behavior, and same-instance wiring to DataVaultConventions.Default.

Scope Out
- Translating hubs, links, satellites, business keys, indexes, or technical metadata columns into EF entity, property, key, or index metadata.
- Provider-specific relational annotations, migrations, generated schemas, SQL dialect behavior, physical column types, or database-specific indexes.
- Advanced configuration overloads or hook surfaces for naming, hashing, record-source, timestamp, or provider behavior.
- Changes to the existing AddDVault() service-registration contract or to the non-EF DCoding.Data.DVault.Modeling.DataVaultModelBuilder API.
- Load automation, ingestion pipelines, runtime record-source resolution, or other downstream Data Vault runtime behavior.

Open questions
- none

Follow-up questions
- When ticket 06EXB7FYXNBPMH8VGQCGP2R41R resumes, should its EF translation layer consume the DCoding.Data.DVault:Conventions annotation directly as its upstream guardrail, or only treat it as a verification marker?
- A later provider-specific ticket should decide how provider-neutral DVault metadata maps to relational schema objects, migrations, and database-specific indexes.
- A later advanced-configuration ticket should decide whether an overload accepting naming, hashing, record-source, timestamp, or provider hooks is needed after those hooks exist.
- A downstream documentation ticket may add DbContext.OnModelCreating usage examples once the EF entry point ships.

Risks
- The new EF Core package reference must stay aligned with the repository's net10.0 baseline to avoid restore or build drift.
- Once shipped, annotation key DCoding.Data.DVault:Conventions becomes a public contract and should not be renamed casually because tests and downstream EF work may rely on it.
- There is still a namespace and overload-resolution risk alongside the existing Modeling.DataVaultModelBuilderExtensions.UseDataVault, so the EF extension must remain typed specifically for Microsoft.EntityFrameworkCore.ModelBuilder in the root namespace.

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
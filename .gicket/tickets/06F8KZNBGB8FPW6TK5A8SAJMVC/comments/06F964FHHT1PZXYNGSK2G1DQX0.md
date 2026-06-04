[gicket-bot] PO refinement contract

Summary
- Refined the provider-specific migration-guardrail story against the landed provider DDL contract and the current translator/diagnostics baseline; scope stays in one implementation story plus the existing downstream documentation task, and the ticket is ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The supported provider baseline is the finite repository set already fixed by docs/plans/provider-identifier-ddl-guardrail-contract.md: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, and mysql-pomelo-v1.
- The visible provider-fact baseline is already present in src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs and diagnostics explain output: maximum identifier length, duplicate-index-covered-by-primary-key allowance, included-index fallback mode, and load timestamp store type/value-format mappings.
- Load timestamp storage remains the finite token set provider-default, iso-8601-utc-text, and utc-ticks; PIT snapshot references inherit the selected provider profile mapping and belong to this story's guardrail scope.
- No child-ticket, relation, attachment, or planning-document write was needed for this refinement pass.

Scope In
- Provider-specific migration guardrail checks for generated secondary indexes, primary keys, and generated unique secondary-index or unique-constraint surfaces against the effective provider shape selected by the translator.
- Guardrail validation of included-column behavior per provider baseline: native includes for SQL Server and Postgres, append-to-key fallback for Sqlite and Oracle, ignore-includes fallback for MySql, and Oracle omission of redundant secondary indexes covered by the primary key.
- Guardrail validation of generated load timestamp and PIT satellite snapshot reference columns for required presence, nullability, store type, and provider value-format compatibility with the selected loadTimestampStorage token.
- Provider-aware destructive-change and suspicious-replacement detection for DVault-owned generated tables, columns, keys, indexes, and bounded uniqueness surfaces before migration application.
- Structured diagnostics and report output through existing DataVaultDiagnostics, DataVaultMigrationGuardrailReport, DataVaultPreflight, and guardrail command surfaces with provider/profile and effective-shape evidence.
- Automated coverage across the finite five-provider baseline and representative loadTimestampStorage variants.

Scope Out
- Changing docs/naming/default-naming-policy.md, the provider-neutral logical naming baseline, or the finite supported-provider baseline.
- Automatic migration execution, migration rewriting, schema repair, or interception of consumer-owned EF command execution.
- Broad live-schema or idempotency feature expansion beyond the migration-guardrail lane needed to explain provider-specific index and timestamp drift.
- New provider packages, open-ended vendor keyword research, or a consumer-visible override API for physical naming or provider-specific guardrail policy.
- Release-note, adoption-guide, and public documentation work already scoped to task 06F8KZNNS76TD9Z7ESB173FZ68.

Open questions
- none

Follow-up questions
- After implementation, should task 06F8KZNNS76TD9Z7ESB173FZ68 explicitly document the per-provider effective index-shape differences now enforced by guardrails?
- Should a later follow-up widen provider profile data from the current shared MaximumIdentifierLength surface to object-class-specific limits where a supported provider proves narrower constraint classes?

Risks
- Provider package upgrades can change emitted EF migration operation shapes, so tests must lock the bounded supported operation set or provider-specific drift may evade guardrails.
- Tightening timestamp nullability and store-type checks can surface previously tolerated migrations as blocking findings, which is correct but may require clearer upgrade guidance for consumers.
- Any future change to provider-effective index shaping or load timestamp mappings can churn migration output and guardrail expectations across existing consumer projects.

Split recommendations
- No PO split is required; the visible repository baseline keeps provider-specific migration guardrails bounded to one implementation story plus the already-separate documentation task.
- If implementation uncovers work that needs new provider packages, object-class-specific provider governance, or broad override APIs, queue that as a follow-up ticket instead of widening this story.

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
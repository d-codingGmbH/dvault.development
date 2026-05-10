[gicket-bot] PO refinement contract

Summary
- Refined the diagnostics task against the current registry and provider baseline; no new split, relation write, or planning document was needed, and the ticket remains the diagnostics child under story 06F0MECWYMPQ4R0KWV1R637RT0 that blocks docs task 06F0MEDJC732GDD77H60R259P0.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This task builds on completed registry ticket 06F0MEAXT99V0P115P0WEJD4P0; validation should extend the existing DataVaultMetadataRegistry, DataVaultMetadataClrMapping, and code-first builder diagnostics instead of redesigning registry lookup rules.
- Current source baseline for provider capability-profile auto-registration is the visible five-package set, not the older narrower doc posture: AddDVaultSqlite, AddDVaultPostgres, AddDVaultSqlServer, AddDVaultOracle, and AddDVaultMySql all register provider names into DataVaultProviderCapabilityProfileSelection, with MySQL covering both Pomelo and Oracle MySQL EF provider names.
- Explain output must distinguish three separate current surfaces: selected capability profile (sqlite-v1, postgres-v1, sqlserver-v1, oracle-v1, mysql-pomelo-v1 plus WithLoadTimestampStorage variants), selected provider-behavior profile (provider-neutral-v1 or the provider package override), and actual save-strategy dispatch result (named provider strategy or provider-neutral fallback writer).
- Unknown or unregistered EF provider names currently fall back to sqlite-v1 capability selection; diagnostics should surface that fallback explicitly as a risky defaulted state instead of presenting it as intentional SQLite configuration.
- Live relations were left unchanged: parent story 06F0MECWYMPQ4R0KWV1R637RT0 remains the parent, this task still blocks docs task 06F0MEDJC732GDD77H60R259P0, and the incoming blocks relation from done registry ticket 06F0MEAXT99V0P115P0WEJD4P0 is treated as satisfied upstream context rather than a new blocker.

Scope In
- Add a public machine-readable diagnostics contract that can validate current DataVaultMetadataModel, DataVaultMetadataRegistry, and code-first declarations before save or runtime execution.
- Cover deterministic validation for duplicate logical names, missing referenced hub/link/satellite metadata, ambiguous CLR mappings, unsupported or missing provider profile mappings, and risky provider/profile fallback states.
- Add public explain output for generated tables, ordered columns, property roles, provider storage and value formats, primary keys, secondary indexes, constraints, metadata source kind and fingerprint, load-timestamp storage shape, capability profile, provider-behavior profile, and save-strategy selection result.
- Provide a concise human-readable rendering derived from the structured diagnostics result for README examples and developer troubleshooting.
- Add tests that assert stable structured payloads and deterministic ordering across the built-in provider profile set and load-timestamp storage variants.

Scope Out
- CLI command implementation beyond keeping the structured diagnostics payload reusable for future CLI tooling.
- Provider-specific save SQL or optimization behavior changes.
- Registry architecture redesign or new metadata-authoring surfaces outside the existing metadata-model, registry, and code-first APIs.
- Runnable example authoring and README or release-document updates, which remain on sibling tickets 06F0MEDBFZ25YA1M7RJ71Z7ZCM and 06F0MEDJC732GDD77H60R259P0.

Open questions
- none

Follow-up questions
- After this API lands, should a future CLI wrapper print the same structured diagnostics payload directly, or should CLI-specific shaping stay outside the core library?
- Once sibling docs and examples tickets land, does the team want explicit documentation that unknown EF provider names default capability selection to sqlite-v1 unless a provider package or override is registered?

Risks
- If explain logic duplicates translator or selection rules instead of reusing them, diagnostics will drift from actual projected table, index, and provider behavior.
- Structured fallback reasons are harder than the rest of the task because current save-strategy compatibility is exposed only as CanSave returning bool; careless design could either leak provider-specific internals or weaken deterministic reporting.
- Older planning and docs in the repo still reflect pre-change provider-registration assumptions in places; docs task 06F0MEDJC732GDD77H60R259P0 will need to align with the current five-provider auto-registration baseline once this diagnostics contract is implemented.

Split recommendations
- No new split is recommended; the parent story already separates diagnostics (06F0MED4P7HMBDZVMPWQZ5A7PC) from runnable examples (06F0MEDBFZ25YA1M7RJ71Z7ZCM) and durable docs or release updates (06F0MEDJC732GDD77H60R259P0).
- Keep the completed registry ticket 06F0MEAXT99V0P115P0WEJD4P0 as upstream context only and do not reopen registry redesign work inside this diagnostics task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
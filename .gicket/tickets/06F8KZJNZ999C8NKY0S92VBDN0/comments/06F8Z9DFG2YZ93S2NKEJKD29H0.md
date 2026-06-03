[gicket-bot] PO refinement contract

Summary
- Refined the story around bounded MySQL and Oracle PIT/bridge read candidate work on the existing provider-neutral read boundary; no persistent planning writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Related done stories 06F8KZHZ27SDTNCFNMFDQRVCKM and 06F8KZJAKN7Q2QXXP9PRK2V94G already fix the provider read evidence contract and the PostgreSQL/SQL Server candidate pattern, so this ticket should extend that baseline rather than reopen PIT/bridge API shape.
- Current repository evidence already fixes the v1 boundary: IDataVaultReadService remains the PIT/bridge read surface over caller-maintained read-model tables, and this ticket does not add automatic PIT/bridge maintenance, SaveChanges refresh, or background orchestration.
- Current source and docs show only AddDVaultSqlite(), AddDVaultPostgres(), and AddDVaultSqlServer() register PIT/bridge read strategies today; AddDVaultMySql() and AddDVaultOracle() currently register save strategies only, so this story is additive provider-package work or an explicit provider-local decline.
- MySQL provider matching must stay dual-provider (Pomelo.EntityFrameworkCore.MySql and MySql.EntityFrameworkCore) consistent with existing capability-profile and save-strategy registration, while Oracle matching stays Oracle.EntityFrameworkCore.
- Benchmark row work and broad release, README, and performance-profile wording are already split into sibling tasks 06F8KZK2MSFQP9G2DBM61ZVGD4 and 06F8KZKFTCC0YXAPRTXA53DNEC; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

Scope In
- MySQL PIT read candidate evaluation and outcome within the existing maintained PIT boundary.
- MySQL bridge read candidate evaluation and outcome within the existing maintained bridge boundary.
- Oracle PIT read candidate evaluation and outcome within the existing maintained PIT boundary.
- Oracle bridge read candidate evaluation and outcome within the existing maintained bridge boundary.
- Read-strategy registration, gate evaluation, diagnostics, parity tests, and fail-closed fallback behavior for any implemented MySQL or Oracle candidate paths.

Scope Out
- Latest-satellite read optimization for MySQL or Oracle.
- New PIT or bridge metadata shapes, request semantics, public IDataVaultReadService APIs, or typed helper contract changes.
- Automatic PIT or bridge maintenance, read-time refresh, SaveChanges orchestration, or provider-specific maintenance strategies.
- Benchmark artifact row expansion or verifier changes owned by task 06F8KZK2MSFQP9G2DBM61ZVGD4.
- Broad documentation and publication work owned by task 06F8KZKFTCC0YXAPRTXA53DNEC.
- Provider-specific save-strategy or bulk-write changes.

Open questions
- none

Follow-up questions
- If one provider is deliberately declined, should task 06F8KZKFTCC0YXAPRTXA53DNEC publish it as a current non-goal or as future-candidate wording in the v0.28.0 docs?
- After the provider matrix is finalized here, should task 06F8KZK2MSFQP9G2DBM61ZVGD4 emit skipped read rows for unmeasured providers or only measured rows for implemented candidates?
- If only one of MySQL or Oracle ships a candidate now, should the remaining provider stay in this epic or move to a later follow-up story?

Risks
- MySQL and Oracle currently have no PIT or bridge read strategy registrations and no read-focused opt-in integration classes, so scope can sprawl if benchmark or documentation follow-through leaks into this story.
- MySQL dual-provider identity and Oracle-specific parameter or identifier behavior can drift from provider-neutral parity unless raw-row and typed-projection parity coverage stays first-class.
- A deliberate decline without explicit tests, diagnostics, and handoff notes would leave the public provider matrix easier to overstate than the visible source proves.
- This story currently blocks benchmark task 06F8KZK2MSFQP9G2DBM61ZVGD4, so unresolved provider outcome here will cascade into downstream evidence work.

Split recommendations
- Keep the story whole if implementation stays limited to candidate evaluation, provider-package registration, gate coverage, and explicit decline evidence inside the existing PIT and bridge architecture.
- Split by provider only if MySQL and Oracle diverge enough that one ships a candidate path while the other needs a decline-only outcome or materially different live-provider validation work.

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
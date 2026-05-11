[gicket-bot] PO refinement contract

Summary
- Refined the quickstart-example ticket around two bounded runnable examples that prove the implemented code-first, registry, explicit save-service, and typed latest/as-of read flow; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the baseline APIs: code-first builders exist in src/DCoding.Data.DVault/DataVaultCodeFirst*, the write boundary is the DI-resolved IDataVaultSaveService, and typed satellite read projections are already defined on done ticket 06F0MECPFAVBFBNC5XMVDZRQ6M; this ticket should demonstrate those implemented surfaces rather than reopen API selection.
- The SQLite example is the zero-infrastructure happy path and must run end-to-end locally.
- The PostgreSQL example stays operator-provided infrastructure only: environment-variable configuration, clear prerequisite/setup guidance, and a bounded skip or fail-fast path when configuration is absent.
- The examples should prove actual historical behavior by writing at least two timestamped versions so latest and as-of reads are visibly different, not just call the same read path twice.
- Example-local docs are in scope, but the broader README or release-note narrative remains on downstream blocked ticket 06F0MEDJC732GDD77H60R259P0.
- No child tickets, relation edits, attachments, or planning documents were created in this refinement pass.

Scope In
- Add one runnable SQLite quickstart example that uses the current code-first plus registry-backed DVault setup and runs without external services.
- Add one runnable PostgreSQL quickstart example that uses the same logical flow with environment-driven connection configuration and explicit prerequisite and skip guidance.
- Demonstrate schema creation, at least two saves that produce a meaningful history point, one typed latest read, and one typed as-of read against a minimal bounded model.
- Add build and run instructions from the repository surface so developers can discover and execute both examples without guessing commands or setup.

Scope Out
- Provisioning or automating every database provider.
- Committing credentials, machine-specific paths, or team-specific connection strings.
- Replacing the broader repository README or release narrative that is already downstream of this ticket.
- Expanding examples into performance benchmarks, migration guidance, or advanced provider tuning.

Open questions
- none

Follow-up questions
- After these runnable examples land, should the broader root README quickstart be replaced or primarily linked to these examples on ticket 06F0MEDJC732GDD77H60R259P0?
- Once PostgreSQL example guidance is stable, should CI or a documented optional local harness be added later to exercise the provider-backed quickstart automatically?
- If additional provider quickstarts are desired after v0.6, should they reuse the same shared example story and only swap provider wiring?

Risks
- If the examples use legacy metadata-model-only or raw read paths because the existing README still shows older patterns, the quickstarts will drift from the implemented branch surface this ticket is meant to demonstrate.
- If the PostgreSQL path lacks a clear environment-variable guard and setup contract, the example will look broken on ordinary developer machines and undermine the quickstart goal.
- If the example saves only one version of the data, the as-of read will not prove distinct historical semantics and the ticket goal will be only partially met.
- If SQLite and PostgreSQL examples fork into separate domain stories, future maintenance and docs parity will drift quickly.

Split recommendations
- No split recommended. Repository evidence keeps this bounded to two closely related provider examples with one shared domain story, and no child tickets or planning documents were materialized in this pass.

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
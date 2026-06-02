[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing `guardrail --migration` preflight: strengthen blocking and suspicious diagnostics for destructive DVault-generated structure changes using metadata and produced-name evidence; no bounded planning writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository docs already establish `guardrail --migration <name>` as the existing consumer-owned preflight entrypoint, so this story strengthens that analyzer rather than adding a new CLI, `dotnet ef` interception, or deployment-time enforcement.
- DVault-generated ownership should be identified from provider-neutral metadata such as produced names, metadata names, entity kinds, property roles, and related generated-structure annotations instead of raw SQL or arbitrary object names.
- PIT and bridge remain first-class generated structures in the current repository context and stay in scope alongside hub, link, and satellite guardrails.
- Live ticket evidence shows no human comments and no attachments changing scope; live relations show this ticket as a child of `06F7Y0J8PRFRSSWZ3GGT91S0TW`, blocked by `06F7Y0HZKHBHMYX9EYDYFRYXZ0`, and blocking `06F7Y0KVHGTTVS216ERSG4XNMM`.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Strengthen the existing migration guardrail preflight that analyzes scaffolded EF migration operations before apply time.
- Detect destructive or suspicious changes to DVault-owned generated hub, link, satellite, PIT, and bridge tables plus their generated columns, secondary indexes, and named generated constraints.
- Differentiate explicit intentional evolution operations, such as true EF rename flows, from suspicious drop-and-add patterns that imply metadata or naming drift.
- Keep diagnostics provider-neutral and expressed in DVault vocabulary with actionable remediation guidance.

Scope Out
- No automatic migration rewrite, schema repair, or deployment.
- No new CLI or tooling surface beyond the existing consumer-owned `guardrail` preflight command path.
- No guardrail coverage for arbitrary non-DVault or consumer-authored database objects.
- No live-schema drift reader expansion or provider physical-plan analysis in this story.

Open questions
- none

Follow-up questions
- After this story lands, decide whether a later ticket should extend similar guardrails to consumer-authored or provider-specific objects that are outside current DVault-generated ownership.
- Reconfirm downstream sequencing with related tickets `06F7Y0HZKHBHMYX9EYDYFRYXZ0` and `06F7Y0KVHGTTVS216ERSG4XNMM` once their scopes settle, but do not reopen this refinement unless they change the `guardrail` preflight boundary.

Risks
- Complex provider-specific scaffolding can decompose one logical rename into multiple migration operations, so some legitimate changes may still be classified as suspicious unless the migration preserves enough continuity evidence.
- Broader structure coverage across tables, columns, indexes, and named constraints increases the test matrix needed to prove provider-neutral behavior.
- The live dependency chain around the current `blocks` relations remains a delivery-sequencing risk even though refinement itself is ready.

Split recommendations
- No split recommended: strengthening destructive-change classification, diagnostics, and tests on the existing `guardrail --migration` surface is one cohesive story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
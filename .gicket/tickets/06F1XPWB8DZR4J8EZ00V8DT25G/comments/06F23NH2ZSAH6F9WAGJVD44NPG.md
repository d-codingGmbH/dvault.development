[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The canonical comparison target is the existing dvault.model.v1 artifact contract already fixed by docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md; this story does not define a new snapshot or schema dialect.
- Completed prerequisite stories 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPVPKVGYKCV04PY98TSS78 remain the authoritative diagnostic-code and design-time workflow baselines; their preserved PO handoff text is historical context only.
- Repository evidence already fixes the design-time boundary: ModelSnapshot comparison stays metadata-only, while live schema comparison is optional evidence rather than migration generation or repair.
- No new child tickets, attachments, or planning documents were materialized in this pass.

Scope In
- Deliver one coherent drift-comparison story across canonical dvault.model.v1 artifacts, EF ModelSnapshot metadata, and optional live database schema metadata.
- Keep ModelSnapshot comparison provider-neutral and based on the existing DVault EF annotation surface, with deterministic ordering and stable drift diagnostics.
- Keep live schema comparison provider-neutral at the contract level, with SQLite as the required v1 supported lane and explicit unsupported or unavailable results for non-implemented or unavailable providers.
- Document when ModelSnapshot comparison is sufficient and when live schema comparison is needed for additional physical-schema evidence.
- Preserve coverage for DVault table kinds already in the visible v0.7.0 baseline, including hubs, links, satellites, PITs, and bridges.

Scope Out
- No migration generation, migration-operations diff engine, or automatic repair.
- No repo-owned EF CLI shim, design-time services package, or expansion beyond the established consumer-owned design-time factory boundary.
- No requirement that every provider package ship first-class live-schema readers in this story.
- No provider-specific DDL diff engine, foreign-key graph diffing, or arbitrary non-DVault catalog comparison.
- No dvault.model.v1 contract expansion beyond the current documented artifact baseline.

Open questions
- none

Follow-up questions
- After this story ships, should the design-time preflight workflow expose one first-class command or report format that can invoke artifact-only, ModelSnapshot, and live-schema comparisons through a shared operator entrypoint?
- Which provider should be the next live-schema implementation after the SQLite-first baseline: Postgres, SQL Server, Oracle, or MySQL?
- Should the repository later publish one consolidated public guide covering when to use artifact review, ModelSnapshot comparison, and live-schema comparison together?

Risks
- If the ModelSnapshot and live-schema lanes normalize names, ordinals, or ordering differently, users may see false drift between logically equivalent metadata.
- Documentation may over-promise support if it implies broad multi-provider live-schema coverage instead of the SQLite-first baseline and explicit unsupported or unavailable results.
- If future dvault.model.v1 fields exceed what the current EF projection surface exposes, snapshot comparison must keep surfacing explicit unsupported gaps rather than implying full coverage.

Split recommendations
- No further split recommended. The bounded implementation split already exists as child tickets 06F1XPWNAWWMDBRK315S66P7AM and 06F1XPWYZTWE9E46GNPFB8F804, and both are already done.
- If broader provider-by-provider live-schema support is needed later, track each provider expansion in separate follow-up tickets rather than widening this parent story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F8KZY9HXF26TRPHVEPE35Y64`, `currentRevision=06F987BS0QJW5R063KABD9FT64`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '355e49b0904ebcf729375c3345c54ea4ab3da1e2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `ec9cc2f264ca`.

Open questions / Risiken
- The epic spans provider profiles, relational naming, migration analysis, diagnostics, and tests; without decomposition it is likely too large for a single dev execution ticket.
- Current code surfaces expose only part of the required provider safety matrix, so implementation will need additional internal profile facts without accidentally widening the public API surface.
- Static reserved-word and identifier-safety catalogs can drift from vendor behavior; v1 must treat them as finite tested inputs rather than evergreen vendor guarantees.
- Split recommendation: Child ticket: extend provider capability and profile data to cover the full identifier-safety matrix, reserved-word inputs, quoting or no-rewrite rules, and load-timestamp storage facts for the five supported providers.
- Split recommendation: Child ticket: implement deterministic logical-to-physical name projection and collision handling for DVault-owned tables, columns, keys, indexes, and constraints while preserving logical traceability annotations.
- Split recommendation: Child ticket: enforce provider-shaped index, key, constraint, and load-timestamp guardrails in validation and migration-operation analysis, including fail-fast handling for unsafe generated shapes.
- Split recommendation: Child ticket: add bounded diagnostics, explain or report output, activity classification, and provider-specific tests that prove the contract across all supported profiles.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `42104`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1794`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8142e087d173472085610d0da80bad6c`
- completed-at-utc: `<redacted>-04T19:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T193638268Z-8142e087d173472085610d0da80bad6c.json`
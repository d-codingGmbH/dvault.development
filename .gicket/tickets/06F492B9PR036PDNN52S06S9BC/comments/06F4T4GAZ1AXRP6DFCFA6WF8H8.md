[gicket-bot] PO refinement contract

Summary
- Refined the story around additive, request-bound query-shape diagnostics for satellite current/as-of, PIT, and bridge reads using the existing diagnostics and support-bundle surfaces.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing request-bound diagnostics path (`IDataVaultReadDiagnosticsService.Analyze(...)` -> `DataVaultDiagnosticsResult` -> `DataVaultSupportBundle`) as the delivery surface; do not invent a separate telemetry-only or standalone explain API for this ticket.
- Treat current and as-of satellite helpers as the existing latest-satellite read family: `AsOf == null` is the current/latest shape and `AsOf != null` is the as-of shape. No separate read-service contract is needed for `current`.
- Keep query-shape diagnostics deterministic and redaction-safe: expose generated table/column names, joins or no-joins, predicates, ordering rules, strategy/provider caveats, and expected indexes, but do not emit actual hash-key values, row payloads, or live query-plan output.
- Ratify the visible index baseline from translated EF metadata: single-active satellite reads use the existing satellite parent/load-timestamp lookup index with hash-diff suffix, multi-active satellite reads use the existing parent/driving-key/load-timestamp/hash-diff index, PIT tables have only the `[ParentHashKey, LoadTimestamp]` primary key and no secondary indexes, and bridge reads use the existing traversal indexes already projected for many-to-many and hierarchy bridges.
- Provider caveat baseline is the current repository baseline: SQLite is the only provider with optimized read-strategy selection today; unsupported providers or unsupported shapes fall back to the provider-neutral pipelines.

Scope In
- Add structured query-shape diagnostics for latest/current/as-of satellite requests, PIT as-of requests, and bridge requests.
- Cover the existing explicit and registry-backed read-diagnostics entry points where public APIs already exist, including `DataVaultRegistryLatestSatelliteReadRequest` and `DataVaultRegistryBridgeReadRequest`.
- Expose the generated entities involved in the read, applied filter columns and predicates, ordering and row-selection assumptions, join or follow-up lookup behavior, expected translated indexes, and provider caveats for the request being analyzed.
- Serialize the new read-shape diagnostics through the existing support-bundle JSON path as additive structured evidence.
- Add documentation and tests for the new diagnostics contract and its request-bound behavior.

Scope Out
- No changes to read execution semantics, result payloads, or automatic PIT or bridge maintenance behavior.
- No live-database `EXPLAIN`, execution-plan capture, raw SQL dump contract, or serialization of actual request hash-key values.
- No new non-SQLite optimized read strategies or provider-specific performance work beyond describing current caveats.
- No telemetry backend, metric-export, or support-bundle routing/upload workflow changes.
- No new registry-backed PIT read request API in this ticket.

Open questions
- none

Follow-up questions
- Should a later ticket surface a condensed query-shape identifier or summary through telemetry or CLI text once the structured diagnostics contract is stable?
- Should future non-SQLite provider-specific read strategies add provider-native shape explainers when those optimized strategies exist?

Risks
- `DataVaultDiagnosticsResult` and `dvault.support-bundle.v1` are stable public surfaces, so the new shape model must be additive and version-safe.
- If the ticket promises raw SQL or provider-specific execution details, routine internal query rewrites will create avoidable churn; the contract should stay semantic and generated-name based.
- Index guidance must stay derived from current translated metadata so diagnostics do not claim consumer-managed physical indexes that DVault does not actually project today.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
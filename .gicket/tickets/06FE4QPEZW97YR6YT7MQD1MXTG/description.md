<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the already-applied durable description rewrite, confirmed the conservative DB2 guardrail scope and downstream dependency remain unchanged, and found the ticket ready to return to PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The durable ticket description rewrite is already applied on the owner branch; no additional relation, child-ticket, attachment, or planning-document writes were needed in this pass.
- The active downstream dependency remains this ticket's `blocks` relation to 06FE4QR3DD7EFZ4F35SBTFGWSR; the done story 06FE4QNWP9606HTB92MTVQMYDG remains historical routing context through its existing links and does not make this ticket blocked.
- `benchmark-summary.json` keeps DB2 optimized save, latest-satellite, PIT, and bridge rows as skipped placeholders with `iterations=0`, null metrics, and `persistedOutcome=not executed` when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset.
- `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md` remain the canonical lookup surfaces for DB2 row identity, posture, and follow-up gating.

### Scope In
- Lock the DB2 promotion rules for provider-native save, latest-satellite read, PIT as-of read, and bridge traversal read evidence.
- Define how completed, skipped, failed, diagnostics-only, and smoke-only DB2 evidence may be represented without overstating support.
- Preserve the existing provider-neutral fallback conditions for unset DB2 configuration, dirty save context, provider mismatch, unsupported or incomplete read shapes, stale PIT or bridge maintenance, or diagnostics that do not select the DB2 strategy.
- Carry the existing downstream dependency boundary where this guardrail ticket precedes 06FE4QR3DD7EFZ4F35SBTFGWSR for provider-configured DB2 tuning and evidence collection.

### Scope Out
- Collecting new provider-configured DB2 benchmark evidence or retuning DB2 execution paths.
- Claiming staged DB2 bulk, provider-native chunk execution, completed DB2 latest-satellite, PIT, or bridge timing, or DB2 live-schema reading without new completed evidence.
- Widening DB2 latest-satellite beyond the supported hub-parent, non-multi-active shapes or adding automatic PIT or bridge maintenance behavior.
- Creating new planning artifacts or relation rewrites for this ticket when the existing split already covers the downstream work.

## Acceptance Criteria
- DB2 claims cite the canonical matrix row identity by scenario, provider, baseline, and evidence posture, and only `completed-timing` rows with a preserved provider-configured artifact triplet and run context may support measured timing claims.
- When `DVAULT_TEST_DB2_CONNECTION_STRING` is unset, the root DB2 save, latest, PIT, and bridge rows remain skipped placeholders with `executionStatus=skipped`, a non-empty skip reason, `iterations=0`, null metrics, and `persistedOutcome=not executed`.
- Any failed DB2 benchmark row uses the same conservative non-timing boundary as skipped rows: a recorded failure reason, `iterations=0`, null metrics, and `persistedOutcome=not executed`.
- Strategy registration, diagnostics selection, and smoke coverage from `AddDVaultDb2()` and the DB2 smoke tests may justify only diagnostics-only or smoke-only candidate posture unless a completed provider-configured benchmark row exists.
- DB2 save promotion stays limited to clean-context set-based save, and DB2 read promotion stays limited to diagnostics-gated latest-satellite, PIT, and bridge candidates on the already documented supported shapes.
- Provider-neutral fallback remains the public behavior whenever DB2 is unconfigured, the context is dirty for save work, the provider mismatches, the read shape is unsupported or incomplete, PIT or bridge maintenance is stale, or diagnostics do not select `Db2DataVaultSaveStrategy` or `Db2DataVaultReadStrategy`.

## Definition of Done
- Downstream implementation can use one shared rule set for DB2 completed-timing, skipped-placeholder, failed, diagnostics-only, and smoke-only evidence without reopening the model.
- No ticket-driven document, manifest, or benchmark interpretation produced from this contract treats DB2 `plannedPath`, `plannedReadStrategy`, or `selectedStrategy` tokens as measured timing by themselves.
- The conservative DB2 non-goals remain explicit: no staged bulk claim, no provider-native chunk execution claim, no completed PIT, bridge, or latest timing claim, and no live-schema-reading claim without new configured evidence.
- No additional PO split is needed because the live downstream boundary already leaves provider-configured DB2 tuning and evidence collection in 06FE4QR3DD7EFZ4F35SBTFGWSR.

## Implementation Notes
- `BenchmarkScenarioExecutionTests` already enforce the artifact contract for skipped and failed rows: only completed rows may carry metrics; skipped or failed rows must keep `iterations=0`, a reason, blank metrics, and `persistedOutcome=not executed`.
- `DVaultDb2ServiceCollectionExtensions` registers `Db2DataVaultSaveStrategy` and `Db2DataVaultReadStrategy`, and `Db2DataVaultSmokeTests` prove candidate selection and representative configured execution behavior; those are strategy-candidate facts, not completed timing claims.
- This refinement pass's durable change was the description rewrite for contract accuracy; relations, child tickets, attachments, and planning documents were intentionally left unchanged.
- Older automation comments remain immutable run history. The rewritten Delivery Contract block in the ticket description is the authoritative handoff surface.

## Open Questions
- none

## Follow-Up Questions
- After 06FE4QR3DD7EFZ4F35SBTFGWSR lands a provider-configured DB2 artifact triplet, which current DB2 rows should move from skipped-placeholder or diagnostics-only posture to completed-timing and which should remain non-promoted historical guidance?

## Risks
- The root quick benchmark triplet currently contains only skipped DB2 external-provider rows, so downstream docs can still overread planned execution-path or selected-strategy tokens as completed timing evidence.
- DB2 support stays narrow and conditional on clean context, explicit PIT or bridge maintenance, supported read shapes, diagnostics selection, and a reachable `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The repository already documents DB2 live-schema reading as out of scope; inconsistent follow-on wording could still imply support that this guardrail contract forbids.
- This ticket only fixes the promotion boundary; measurable DB2 evidence still depends on downstream delivery in 06FE4QR3DD7EFZ4F35SBTFGWSR.

## Split Recommendations
- No additional split is recommended. The live downstream `blocks` relation to 06FE4QR3DD7EFZ4F35SBTFGWSR already captures the provider-configured DB2 tuning and evidence work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: make DB2 save/latest/PIT/bridge benchmark promotion explicit and conservative. Acceptance: completed, skipped, and failed DB2 evidence is represented without overstating support.
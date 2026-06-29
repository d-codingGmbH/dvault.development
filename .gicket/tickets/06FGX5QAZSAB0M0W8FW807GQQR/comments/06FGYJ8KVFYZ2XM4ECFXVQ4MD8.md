[gicket-bot] PO refinement contract

Summary
- Fresh repository and .gicket inspection show that v0.48 already shipped alias-coverage and personal-data privacy diagnostics, and upstream ticket 06FGX5NTKQX87FWCZ2GDDVCXEW is done; this ticket can now be refined as an additive structured diagnostics/support-bundle contract and is ready for PO critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No planning writes, description updates, attachments, or relation changes were materialized in this run; refinement is based on live ticket, comment, relation, and repository evidence only.
- Upstream blocker 06FGX5NTKQX87FWCZ2GDDVCXEW is done and already fixes the provider-native encryption boundary; this ticket should consume that unmanaged guidance-only boundary instead of reopening provider capability scope.
- This ticket remains the diagnostics child under story 06FGX5KZHC9ZAKAT71C89MEYV8 and continues to block docs-alignment ticket 06FGX5S4FTGBE7YQ897BMY1974.
- Use the existing diagnostics-to-support-bundle flow as the single implementation path: once privacy adoption facts exist on DataVaultDiagnosticsResult, DataVaultSupportBundle should serialize the same facts under diagnostics rather than inventing a separate privacy-only export path.

Scope In
- Add additive structured privacy adoption facts to the existing diagnostics and support-bundle surfaces.
- Expose alias-centric facts for registered encrypted-payload aliases, mapped EF properties, coverage status, and key-provider posture.
- Expose marker-centric facts for each personalData satellite payload field, its encryptedPayloadAlias, and its coverage status or cause against the analyzed EF model or metadata.
- Expose an active-provider guidance fact derived from the done provider-native boundary matrix that states the provider-native encryption boundary is unmanaged and guidance-only for DVault, without database probing.
- Add tests for object-model results and serialized support-bundle JSON across the bounded privacy coverage cases.

Scope Out
- Do not implement provider-native encryption, SQL crypto dispatch, encrypted DDL, provider capability probing, or runtime branching based on native encryption availability.
- Do not take ownership of quickstart or example work; sibling ticket 06FGX5R67T2G0FEGMWE0JBEKJ8 keeps that scope.
- Do not broaden into repository-wide documentation alignment or release-note copy updates beyond minimal code-local comments or tests; sibling ticket 06FGX5S4FTGBE7YQ897BMY1974 owns that.
- Do not introduce a new standalone artifact format, CLI command, or schema fork beyond additive dvault.support-bundle.v1 and diagnostics changes.

Open questions
- none

Follow-up questions
- After structured facts land, should ticket 06FGX5S4FTGBE7YQ897BMY1974 include a small JSON excerpt in the docs, or keep documentation at the prose and boundary level only?
- If future provider-specific native encryption work is approved, should each provider ticket extend this boundary vocabulary only for its named capability instead of widening the shared provider-neutral contract?

Risks
- The current alias report lives in the optional privacy package while diagnostics and support-bundle live in core; a careless implementation could invert package dependencies or leak optional-package types into the core public API.
- If structured statuses are not clearly separated between alias-centric and marker-centric coverage, consumers may confuse registered-but-unmapped alias facts with fail-closed personalData coverage failures.
- Any non-additive JSON change or accidental inclusion of provider settings or connection details would break the redacted support-bundle contract and downstream consumers.

Split recommendations
- No further split is needed for this ticket: the parent story already isolates provider-boundary work in 06FGX5NTKQX87FWCZ2GDDVCXEW, quickstart work in 06FGX5R67T2G0FEGMWE0JBEKJ8, and documentation alignment in 06FGX5S4FTGBE7YQ897BMY1974.
- If later work moves beyond structured facts into actual native encryption behavior, create one provider-specific follow-up ticket per exact capability rather than widening this diagnostics task.

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
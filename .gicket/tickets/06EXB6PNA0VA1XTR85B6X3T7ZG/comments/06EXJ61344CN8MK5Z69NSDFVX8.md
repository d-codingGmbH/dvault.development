[gicket-bot] PO-critic review contract

Summary
- Return to PO: the scope boundary itself is well formed, but the contract relies on existing source API evidence that is not visible in the provided repository documents or branch snapshot.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract Open Questions section contains only 'none'.
- docs/architecture/mvp-data-vault-concepts.md states the MVP concept set is limited to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- docs/plans/deferred-data-vault-capabilities.md lists PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations as deferred capabilities.
- docs/naming/default-naming-policy.md defines Hub, Link, and Sat table prefixes and HashKey, HashDiff, LoadTimestamp, and RecordSource technical column naming expectations.
- docs/plans/stable-hashing-contract.md defines shared stable hashing boundaries and says domain-specific tickets decide which entity fields participate in a given model hash.
- The branch snapshot shows src-roots include src/DVault, but it does not show source definitions for DataVaultConventions.ModelConcepts or DataVaultModelConcept.
- The branch snapshot shows tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs has a test named 'default conventions expose MVP vocabulary and hash defaults', but tests are not source evidence for an existing public API/type under the decision rule.

Blocking findings
- The delivery contract claims existing source modeling conventions expose the finite MVP concept vocabulary through DataVaultModelConcept/DataVaultConventions, and Implementation Notes direct developers to use DataVaultConventions.ModelConcepts and DataVaultModelConcept as implementation evidence. The provided repository documents and branch snapshot do not include visible source definitions for those public types or members; only test/prose references are visible.

Required PO actions
- Restate the contract so it is grounded in visible repository evidence, or add explicit wording that DataVaultModelConcept and DataVaultConventions.ModelConcepts may be created or adjusted by downstream implementation if they are not already present in source.
- Avoid presenting DataVaultConventions.ModelConcepts and DataVaultModelConcept as existing implementation evidence unless the ticket includes visible source evidence for those definitions.

Open issues ledger
- critic-item-1 [required-po-action] Restate the contract so it is grounded in visible repository evidence, or add explicit wording that DataVaultModelConcept and DataVaultConventions.ModelConcepts may be created or adjusted by downstream implementation if they are not already present in source.
- critic-item-2 [required-po-action] Avoid presenting DataVaultConventions.ModelConcepts and DataVaultModelConcept as existing implementation evidence unless the ticket includes visible source evidence for those definitions.
- critic-item-3 [blocking-finding] The delivery contract claims existing source modeling conventions expose the finite MVP concept vocabulary through DataVaultModelConcept/DataVaultConventions, and Implementation Notes direct developers to use DataVaultConventions.ModelConcepts and DataVaultModelConcept as implementation evidence. The provided repository documents and branch snapshot do not include visible source definitions for those public types or members; only test/prose references are visible.

Missing examples / edge cases
- No blocking examples are missing for the planning-level scope boundary; the unresolved issue is source-evidence framing for claimed existing APIs.

Risky assumptions
- Assuming DataVaultConventions.ModelConcepts and DataVaultModelConcept already exist as public source APIs based only on contract prose or tests could constrain developers against source that is not visible in the provided evidence.
- The follow-up question about link satellites notes current builder evidence may not expose a link-satellite declaration surface, so downstream API tickets should not infer that surface exists without source evidence.

AC / test suggestions
- Keep the acceptance criteria focused on ratifying the MVP concept boundary and deferred-capability boundary; later implementation tickets can add source-level AC for any DataVaultModelConcept/DataVaultConventions API once source evidence or creation scope is explicit.

Implementation watchouts
- Do not implement PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, schema generation, loading automation, migrations, validation tooling, or hash computation under this story unless a later ticket explicitly scopes them.
- Use SQLite-oriented examples as the MVP validation path and keep hash-key/hash-diff algorithm and normalization choices out of this story.

Non-blocking notes
- The documented MVP and deferred-capability boundaries are otherwise coherent and align with docs/architecture/mvp-data-vault-concepts.md and docs/plans/deferred-data-vault-capabilities.md.
- The README layout risk is already called out in the contract; downstream tickets should follow active branch evidence such as src/DVault and tests/DVault.Tests.

Split recommendations
- No additional split is required for the scope-boundary story once the claimed existing API evidence is corrected or reframed.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
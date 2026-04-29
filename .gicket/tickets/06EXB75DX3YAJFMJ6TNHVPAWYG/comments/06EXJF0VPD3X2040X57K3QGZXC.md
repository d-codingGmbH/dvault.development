[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06EXB75DX3YAJFMJ6TNHVPAWYG' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06EXB75DX3YAJFMJ6TNHVPAWYG`.
- Optimistic claim succeeded (`expectedRevision=06EXJDRASZ3MK34W57NFJHDD8G`, `currentRevision=06EXJDWWY47KHXYXMF2B3QGDN8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Selected verification source branch 'ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions' and commit 'a49b131ff0d9' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions' from source 'a49b131ff0d9'.
- Interactive tester tool loop completed review for branch 'ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions'.
- Evidence: git show --name-status --oneline -1 a49b131ff0d9 shows implementation commit a49b131f modifying src/DVault/Modeling/DataVaultModel.cs, DataVaultModelBuilder.cs, DefaultDataVaultNamingPolicy.cs, DefaultNamingPolicy.cs, IDataVaultNamingPolicy.cs, and tests/DVault.Tests...
- Evidence: git diff --name-status develop...a49b131ff0d9 -- src/DVault/Modeling tests/DVault.Tests docs/naming lists only modeling source and test changes; docs/naming/default-naming-policy.md exists and was used as the normative contract.
- Evidence: docs/naming/default-naming-policy.md lines 15 and 43 specify explicit link names when provided and object fallback @@@ -> Entity.
- Evidence: src/DVault/Modeling/DefaultNamingPolicy.cs lines 82-97 calls TryNormalizeObjectName for relationshipName and falls back to participant names when no semantic token is found.
- Evidence: src/DVault/Modeling/DefaultNamingPolicy.cs lines 234-252 show NormalizeObjectNameCore would return Entity for no semantic token, but TryNormalizeObjectName returns false for that case.
- Evidence: tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs lines 94-101 test @@@ fallback for hubs and invalid property fallback, but no explicit unsafe link relationship case is present.
- 47 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: Default hub, link, and satellite table names follow docs/naming/default-naming-policy.md, including PascalCase normalization, finite object singularization, documented fallbacks, and unsafe object token handling. (docs/naming/default-naming-policy.md says expl...
- AC check failed: Tests demonstrate deterministic output, documented normalization examples, singular/plural object equivalence, reserved-word handling, collision behavior, index and constraint naming, and the custom naming-policy override path. (Tests cover many required categ...
- DoD check failed: Relevant .NET build/test commands and repository formatting checks pass, or unavailable local tooling is explicitly reported with the attempted command. (dotnet test --nologo was not run in this read-only tester session because it would require build/test out...
- Blocking: explicit link relationship names that normalize to the object fallback are treated as missing and replaced by participant-order naming, violating the documented Link{ParticipantOrRelationshipName} rule and unsafe object token handling.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Change DefaultNamingPolicy.GetLinkTableName so a non-null/non-whitespace relationship name always uses NormalizeObjectName, including the Entity fallback, and only null/whitespace relationship names use participant fallback.
- Add tests for explicit unsafe link relationship names such as @@@ producing LinkEntity through DefaultNamingPolicy and the model builder path.
- After the fix, run deterministic verification for dotnet test --nologo in a writable legacy/test environment.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8741`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `bd77f33e38814ac3b105d0e3b4157f0b`
- completed-at-utc: `<redacted>-29T12:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06EXB75DX3YAJFMJ6TNHVPAWYG/runs/20260429T123447733Z-bd77f33e38814ac3b105d0e3b4157f0b.json`
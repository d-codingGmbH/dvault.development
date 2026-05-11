[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F0MEE8T9PKPKQH8EPWNQ2CRW' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.3` was applied to ticket `06F0MEE8T9PKPKQH8EPWNQ2CRW`.
- Optimistic claim succeeded (`expectedRevision=06F1FRFFSMV9JYA983DQKJS1AC`, `currentRevision=06F1FRXNBJGSG6B3F9A9APSBK4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.3`.
- Selected verification source branch 'ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va' and commit '2db20554927c' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va' from source '2db20554927c'.
- Interactive tester tool loop completed review for branch 'ticket/06F0MEE8T9PKPKQH8EPWNQ2CRW-task-define-versioned-dvault-model-schema-and-va'.
- Evidence: Requested bounded repository inspection via shell-command and repository-list-directory, but no tool results were available before final response.
- Evidence: Ticket status at verification time is 'todo'.
- Evidence: Ticket labels at verification time: [area/api, area/docs, area/model-first, area/validation, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-<redacted>.3].
- Evidence: Configured tester success handoff role is 'integrator'.
- Evidence: Ticket description contains a persisted delivery contract block.
- Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.
- 28 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: A v1 schema contract is documented or encoded clearly enough for downstream parser, diagnostics, and projection work to proceed without reopening top-level field names, token names, or compatibility policy. (Repository evidence was not available; the schema co...
- AC check failed: Valid examples cover at minimum a customer hub with ordered business keys, a hub-parent satellite, a link with ordered participants, a multi-active satellite with ordered driving keys, a PIT declaration over hub satellites, a many-to-many bridge, a hierarchy b...
- AC check failed: Invalid examples cover at minimum missing or unsupported schemaVersion, duplicate declaration names or roles, missing references, wrong reference kinds, ambiguous link participants, repeated-hub link participants without roles where needed, satellite driving-k...
- AC check failed: Diagnostics are structured with severity, stable category/code, message, and JSON Pointer-style path where feasible; invalid documents return diagnostics without partial model application. (Repository evidence was not available; diagnostics contract content co...
- AC check failed: The contract avoids provider-specific leakage except the explicit loadTimestampStorage capability choice and maps accepted documents into registry-compatible metadata semantics only where those semantics are visible, while permitting additive missing model-fir...
- DoD check failed: The v1 artifact contract identifies required and optional top-level fields, default values, supported token values, and schemaVersion compatibility behavior. (Repository evidence was not available; top-level fields, defaults, tokens, and schemaVersion behavio...
- DoD check failed: The validation taxonomy is explicit enough for downstream tests to assert stable categories for schema/version, shape, reference, duplicate, naming, capability, provider-choice, and recursive participant binding failures. (Repository evidence was not availabl...
- DoD check failed: Representative fixture names and scenarios are available to parser/projection implementers, either in tests/fixtures or in a durable planning/spec document created by the implementation work. (Repository evidence was not available; durable fixture names and s...
- DoD check failed: Downstream implementation can project valid model-first documents into existing metadata semantics where current-branch evidence shows those semantics exist, and can add narrow missing model-first/PIT/bridge metadata adapters where visible current-branch publ...
- Tester gate cannot pass without direct branch-diff and file-inspection evidence.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Run bounded repository inspection on target branch commit 2db20554927c against develop, inspect docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md, then request legacy verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh if str...

Prompt cache usage
- prompt-tokens: `24817`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0980`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `f78caabea19a4859af5bf0fe50648207`
- completed-at-utc: `<redacted>-11T16:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/77ab9562dde05301902c1ce959c54a9e729a6376a305f47811212a4df17a5a96/tickets/06F0MEE8T9PKPKQH8EPWNQ2CRW/runs/20260511T163514874Z-f78caabea19a4859af5bf0fe50648207.json`
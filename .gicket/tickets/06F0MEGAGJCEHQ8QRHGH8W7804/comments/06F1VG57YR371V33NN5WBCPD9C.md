[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' for ticket '06F0MEGAGJCEHQ8QRHGH8W7804'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGAGJCEHQ8QRHGH8W7804`.
- Optimistic claim succeeded (`expectedRevision=06F1VDRP24C11FYP8W8HGE5GMM`, `currentRevision=06F1VDZHWH5B649P796EZDQACW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Selected verification source branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' and commit 'ea6cce0a600d' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' from source 'ea6cce0a600d'.
- Interactive tester tool loop completed review for branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow'.
- Evidence: git branch --show-current reported ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow.
- Evidence: git show --no-patch --oneline ea6cce0a reported ea6cce0a [06F0MEGAGJCEHQ8QRHGH8W7804] handoff dev->test (DEV-IMPLEMENTATION implementation).
- Evidence: git diff --stat develop...ea6cce0a -- README.md docs/model-first-governance.md reported 2 files changed, 187 insertions, 4 deletions.
- Evidence: git diff --name-status develop...ea6cce0a -- README.md docs/model-first-governance.md reported M README.md and A docs/model-first-governance.md.
- Evidence: git diff --name-status develop...ea6cce0a -- . ':!README.md' ':!docs/model-first-governance.md' ':!.gicket/**' returned no paths, supporting docs-only product change scope.
- Evidence: git diff --name-only ea6cce0a..HEAD -- README.md docs/model-first-governance.md returned no paths, so later branch metadata commits did not alter the reviewed docs files.
- 51 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator gate. Run the policy verification commands in the normal writable/restore-capable environment if that gate requires executable verification: dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8647`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `82f308f29d764f4b81a07f41c45ff1a3`
- completed-at-utc: `<redacted>-12T19:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/runs/20260512T195344269Z-82f308f29d764f4b81a07f41c45ff1a3.json`
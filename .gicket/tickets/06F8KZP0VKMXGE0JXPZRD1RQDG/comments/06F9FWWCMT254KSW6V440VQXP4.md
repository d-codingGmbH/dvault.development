[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' for ticket '06F8KZP0VKMXGE0JXPZRD1RQDG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- Optimistic claim succeeded (`expectedRevision=06F9FT8NKY6CQM394VTW46XRMW`, `currentRevision=06F9FTFNXFTXGQVDCER74FAKAW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' and commit '69422bf7de10' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' from source '69422bf7de10'.
- Interactive tester tool loop completed review for branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag'.
- Evidence: Inspected commit `69422bf7de1002fa1a6767af600c420945fb3141`, which is contained by branch `ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag`.
- Evidence: `git diff --name-only develop...69422bf7de10` shows only `.gicket` metadata or comment changes plus `README.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `docs/releases/v0.30.0.md`; no `src/`, `tests/`, or `tools/` paths changed.
- Evidence: `git diff --check develop...69422bf7de10 -- README.md docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/releases/v0.30.0.md` returned no whitespace errors.
- Evidence: `README.md:386` covers authoritative support-bundle regeneration plus stale fingerprint recovery, and `README.md:731-739` adds the request-bound `ReadShape` refresh checklist.
- Evidence: `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:184-225` contains `Support Bundle Freshness Troubleshooting`, including support-bundle re-export and representative `CreateSupportBundleDiagnostics` PIT or bridge examples.
- Evidence: `docs/releases/v0.30.0.md:30-75` adds `Authoritative Support-Bundle Refresh`, `Request-Bound ReadShape Recovery`, and `Adopter Recovery Checklist`; `docs/releases/v0.30.0.md:118` says closure-stage relation housekeeping stays outside the repository release note.
- 65 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to `integrator`.
- When replay exposes the replacement ticket ULID during closure preparation, add or verify the new `parentOf` link and then remove or explicitly supersede `.gicket/relations/0R/DG/06F8KZQAWZ7QRGB68KB21C9B0R--06F8KZP0VKMXGE0JXPZRD1RQDG--blocks.json`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8769`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `d2fd7a4a49434fda9b7303014fc2f53f`
- completed-at-utc: `<redacted>-05T13:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/runs/20260605T132301410Z-d2fd7a4a49434fda9b7303014fc2f53f.json`
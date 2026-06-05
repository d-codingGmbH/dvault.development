[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F8KZP0VKMXGE0JXPZRD1RQDG' for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- Optimistic claim succeeded (`expectedRevision=06F9FEFH28Q3BSH75ZVKT7QF5M`, `currentRevision=06F9FEPKPHKWQPJT0WRKYJAFH4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' and commit '6d6c0cf0585f' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' from source '6d6c0cf0585f'.
- Interactive tester tool loop completed review for branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag'.
- Evidence: Commit 6d6c0cf0585f is on branch ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag, and git diff against develop excluding .gicket returns only README.md, docs/architecture/dvault-dotnet-ef-design-time-workflow.md, and docs/releases/v...
- Evidence: README.md line 386 documents regenerating the authoritative support bundle after metadata/request changes, updating or removing stale DVaultTypedReadModelMetadataSourceFingerprint values, and treating DMV1960/DMV1961 as stale-input recovery.
- Evidence: README.md lines 731-739 add the stale typed-helper input refresh checklist and representative CreateSupportBundleDiagnostics guidance.
- Evidence: README.md lines 910-924 move the current coordinated documentation baseline to v0.30.0 and link the new release note.
- Evidence: docs/architecture/dvault-dotnet-ef-design-time-workflow.md lines 184-225 add Support Bundle Freshness Troubleshooting, including re-export, fingerprint refresh, and representative PIT/bridge request guidance.
- Evidence: docs/releases/v0.30.0.md lines 30-75 add Authoritative Support-Bundle Refresh, Request-Bound ReadShape Recovery, and Adopter Recovery Checklist sections; lines 116-118 state closure-stage relation housekeeping remains outside the repository release note.
- 66 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: Before epic closure review, repository evidence shows the documentation carrier landed and the incoming `blocks` relation from `06F8KZQAWZ7QRGB68KB21C9B0R` is removed or explicitly superseded. (Current repository evidence still shows the incoming blocks relati...
- DoD check failed: The replacement documentation carrier is visible and linked from the epic, or the already queued replay has become visible as the active carrier by the time closure is attempted. (The queued replay is still only referenced by mutation id; relation listing sho...
- The repository documentation work itself is present and scoped correctly, but the persisted contract still includes closure-stage expectations that are not yet directly materialized as a visible replacement ticket link or an explicit blocks-relation supersession artifact.
- Because those remaining gaps are ticket/relation state rather than repository implementation defects, they do not justify developer code rework, but they prevent an unqualified tester pass under the current persisted wording.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Clarify whether acceptance criterion 5 and definition-of-done item 1 are intended to block tester pass before replay materializes the replacement ticket ULID.
- If those items are closure-stage or integrator-only gates, advance this branch without further repository changes.
- If those items must be satisfied before tester pass, provide repository-visible evidence of the replacement carrier link and the blocks-relation removal or explicit supersession.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8622`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `2e6101716392493cb23c1dd5de89dcba`
- completed-at-utc: `<redacted>-05T12:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/runs/20260605T122825176Z-2e6101716392493cb23c1dd5de89dcba.json`
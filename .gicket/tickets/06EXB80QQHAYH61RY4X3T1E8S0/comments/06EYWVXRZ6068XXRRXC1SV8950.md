[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB80QQHAYH61RY4X3T1E8S0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB80QQHAYH61RY4X3T1E8S0`.
- Optimistic claim succeeded (`expectedRevision=06EYWTR8CA9VF0757F81GGQ0N8`, `currentRevision=06EYWTW1G883NTWHV9WQBR6ZW8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB80QQHAYH61RY4X3T1E8S0-task-add-provider-integration-test-categories' from source '491221fbd4e3071cb20f06e52d98ac187fef0028'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB80QQHAYH61RY4X3T1E8S0-task-add-provider-integration-test-categories` as `ad07d52a2b0a`.

Open questions / Risiken
- Risky assumption: Implementation must keep the new integration-category contract aligned with the existing unit-project provider smoke coverage owned by `06EXB80FPE3REH11RQ1YR6BW1G` instead of duplicating or drifting from it.
- Risky assumption: Downstream CI work on `06EXB82RW6PV2NFG088G6BPFHC` will infer default-versus-opt-in behavior from this ticket, so relying on undocumented runner/filter semantics would be risky if the implementation does not leave a repository-visible proof.
- Split recommendation: No split recommended; the persisted contract already keeps unit grouping, Postgres opt-in, and downstream CI as separate tickets.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9048`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7b867424f64d47e7afb7aff4882c6e96`
- completed-at-utc: `<redacted>-03T15:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB80QQHAYH61RY4X3T1E8S0/runs/20260503T152311832Z-7b867424f64d47e7afb7aff4882c6e96.json`
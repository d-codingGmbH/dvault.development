[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4QXYQ0SWB1DPMGJJ5XX0`.
- Optimistic claim succeeded (`expectedRevision=06FCT69Z34R241VGP6NBVDC0PM`, `currentRevision=06FCT6CRY5GXZKNJCN7SE70MQR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSC4QXYQ0SWB1DPMGJJ5XX0-task-update-performance-docs-with-provider-evide' from source 'ee9d52278b88d5e1dc2202826f639429344d7855'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FBSC4QXYQ0SWB1DPMGJJ5XX0-task-update-performance-docs-with-provider-evide` as `808332131b7a`.

Open questions / Risiken
- Blocking finding: The delivery contract instructs the release note to preserve the `v0.39.0` over visible `8.39.0` / `10.39.0` package-line pattern, but direct repo evidence still exposes only `8.38.0` / `10.38.0`. The ticket does not say whether the developer should avoid pac...
- Required PO action: Clarify whether this is allowed to be a docs-only `v0.39.0` release note that does not assert new consumer package-version lines.
- Required PO action: If `v0.39.0` must mention consumer package lines, link or sequence the authoritative release-planning/version-alignment work that introduces repo-backed `8.39.0` and `10.39.0` evidence, or explicitly add those repo surfaces to this ticket's scope.
- Risky assumption: Assuming the branch can document `8.39.0` / `10.39.0` consumer package lines even though no direct repository evidence for those versions exists yet.
- Risky assumption: Assuming a new top-of-trail `v0.39.0` release note can become the latest visible release record while other current-baseline package/version documents remain on `v0.38.0` without explicit PO wording.
- Split recommendation: If PO wants repo-backed `8.39.0` / `10.39.0` release-line claims in this same handoff, split or link a separate release-planning/version-alignment ticket for surfaces such as `tools/pack-release-packages.sh`, package-compatibility docs, and related instal...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8718`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `14dc1b3e7c764bd09a7a6c7152065820`
- completed-at-utc: `<redacted>-15T21:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/runs/20260615T210910201Z-14dc1b3e7c764bd09a7a6c7152065820.json`
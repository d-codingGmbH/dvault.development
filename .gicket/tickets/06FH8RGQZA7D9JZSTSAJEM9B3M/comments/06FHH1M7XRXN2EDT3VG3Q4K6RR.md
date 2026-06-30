[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RGQZA7D9JZSTSAJEM9B3M`.
- Optimistic claim succeeded (`expectedRevision=06FHGYCRAHPNMCTKVSE96CS8Z4`, `currentRevision=06FHGYS3M0B6YADQGHV7DFT9FR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co' from source 'dc17993f85022edfdf0f8513a70af49003dd4ad1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co` as `3d890b54440e`.

Open questions / Risiken
- Blocking finding: The delivery contract states that current code already includes `DataVaultProviderNativeEncryptionBoundaryFact`, but the cited source path `src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs` could not be read because the file do...
- Required PO action: Update the implementation note and any dependent acceptance/DoD wording so the provider-native boundary diagnostics surface is backed by a directly verifiable source reference. Either cite the actual existing type/path or explicitly state that this ticket m...
- Required PO action: If the intended diagnostics carrier is not a public type named `DataVaultProviderNativeEncryptionBoundaryFact`, rewrite the ticket to name the actual source-backed contract so devs do not have to infer the target from prose.
- Risky assumption: The current contract assumes the named provider-native boundary fact is already present and citable in source, but direct source verification only confirmed the release-note prose and the other privacy APIs.
- Risky assumption: If PO leaves the incorrect type citation in place, developers may either search for a non-existent API or widen scope by inventing a new diagnostics surface without a clear contract anchor.
- Split recommendation: Keep future provider-native encryption work split one provider and one exact capability per ticket, as the current contract already recommends.
- Split recommendation: If the missing provider-native boundary diagnostics surface turns out to be separable from the alias-driven privacy seam, consider a narrow follow-up ticket for that diagnostics contract rather than broadening this story.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7416`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0023b3da7db04c67b823c2ee4a0ce2ba`
- completed-at-utc: `<redacted>-30T12:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RGQZA7D9JZSTSAJEM9B3M/runs/20260630T123457640Z-0023b3da7db04c67b823c2ee4a0ce2ba.json`
[gicket-bot] PO-critic review contract

Summary
- The story contract is now consistent as a tracking umbrella: the two implementation child tickets are done, the cited EF Core repository surface exists, and the current parent-branch activity is ticket-metadata-only rather than new source work.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7FF1J9NR2849WKDR8DKPG/description.md shows '## Open Questions' = '- none' and explicitly states the story is a tracking umbrella with no remaining developer-owned work.
- .gicket/relations/PG/K4/06EXB7FF1J9NR2849WKDR8DKPG--06EXB7FPZRCFC33RF2M5SXZTK4--parentOf.json and .gicket/relations/PG/1R/06EXB7FF1J9NR2849WKDR8DKPG--06EXB7FYXNBPMH8VGQCGP2R41R--parentOf.json persist the parentOf links from the story to those two done child tickets.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs contains public UseDataVault() and ApplyDataVaultMetadata(DataVaultMetadataModel) entry points, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs contains the provider-neutral EF metadata translation implementation cited by the contract.
- git log --oneline -- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs includes child-ticket integration commits 4395dbbd [06EXB7FPZRCFC33RF2M5SXZTK4] and 56e1558a [06EXB7FYXNBPMH8VGQCGP2R41R].
- git show --stat --oneline fb0f0dcd2f8e99a39b82d87b0f82a735599eb749 and git diff --name-only 20ea6fa07e948ad57f21654cf59c934b666b2f08..fb0f0dcd2f8e99a39b82d87b0f82a735599eb749 -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests .gicket/tickets/06EXB7FF1J9NR2849WKDR8DKPG show the current parent branch work only changed .gicket ticket metadata, not source or tests.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If this umbrella advances, the remaining edge case is relation hygiene for blocked tickets 06EXB7G6YE4X0GA0CT7EPEFMPR and 06EXB7HYG17X73GH0K535GYJH8; the current contract flags that follow-up but does not materialize a re-pointing example.

Risky assumptions
- Assumes the workflow can treat an approve_for_dev outcome on a non-executable umbrella as administrative progression rather than reopening implementation on the parent story.
- Assumes the existing blocks relations from this story to 06EXB7G6YE4X0GA0CT7EPEFMPR and 06EXB7HYG17X73GH0K535GYJH8 will be cleaned up if they become stale after the umbrella advances.

AC / test suggestions
- For future umbrella stories, prefer relation/status-based acceptance criteria over execution-oriented language so approval does not depend on workflow interpretation.
- If automation needs a machine-clear close condition, state explicitly that the story is complete once child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R are done and the parentOf relations remain intact.

Implementation watchouts
- Do not hand this parent story to developers as fresh coding work; the code-bearing slices are already captured by child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R.
- Do not reopen provider-specific or advanced-configuration scope on the umbrella; the observed repository surface already matches the bounded EF Core integration scope described by the contract.

Non-blocking notes
- The live blocks relations to 06EXB7G6YE4X0GA0CT7EPEFMPR and 06EXB7HYG17X73GH0K535GYJH8 remain present in .gicket/relations/PG/PR/... and .gicket/relations/PG/H8/...; that is follow-up relation hygiene, not a remaining PO refinement gap.

Split recommendations
- No additional split is warranted; the implementation split already exists as done child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R.
- If any residual work is discovered later, capture it as a new ticket instead of reopening this umbrella story as executable dev scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
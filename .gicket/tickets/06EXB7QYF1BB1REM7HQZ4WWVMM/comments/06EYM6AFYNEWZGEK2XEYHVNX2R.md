[gicket-bot] PO refinement contract

Summary
- Verified that parent story 06EXB7QYF1BB1REM7HQZ4WWVMM is now an umbrella documentation story already satisfied by completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC; README and package-readme evidence match that outcome, so no new planning writes were needed and the ticket should return to PO-critic on a close/advance path rather than dev.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Treat 06EXB7QYF1BB1REM7HQZ4WWVMM as an umbrella/aggregation story now. The substantive documentation work is already present in README.md and was delivered through completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC, so this parent should advance or close from PO instead of being handed to dev for new implementation work.
- critic-item-2: `answered` - The parent is no longer expected to go to dev. There is no remaining parent-only developer work beyond aggregating the already-completed child outcomes into the umbrella story contract, so no new dev scope should be added to this parent ticket.
- critic-item-3: `answered` - The current parent status and labels reflect the earlier PO-critic return, not the actual post-child-completion state. This contract now ratifies the ticket as an umbrella story with no remaining dev or test implementation scope; runtime and PO-critic should therefore align metadata to the non-dev close or advance path instead of keeping blocked/dev, blocked/test, or needs-po semantics on the parent.

Clarifications
- The parent story now serves as the umbrella documentation contract for completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC rather than as a separate developer work item.
- README.md is already the canonical and packaged getting-started document, and src/DCoding.Data.DVault/DCoding.Data.DVault.csproj packs it as the package README.
- Current repository evidence fixes the v1 baseline as .NET 10, source consumption through src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, optionless AddDVault(), ApplyDataVaultMetadata(...), explicit IDataVaultSaveService/DataVaultSaveRequest writes, and EF shared-type Dictionary<string, object> reads.
- The current parent ticket metadata still shows blocked/dev, blocked/test, and needs-po from the earlier PO-critic return, but that metadata is stale relative to the completed child-ticket state and should be aligned by runtime after this contract is accepted.
- No new child tickets, relations, attachments, or planning documents were created in this refinement run; the existing parentOf relations and the blocks relation to 06EXB8202A88KJJP7WEGBESBYM remain sufficient.

Scope In
- Own the umbrella story contract that aggregates the completed README quickstart work from 06EXB7R6MTJW1PYRN172MW34DM and the completed installation framing from 06EXB7REMY41DF7RE8J3N1RZYC.
- Ratify README.md as the single canonical getting-started and package-readme surface for this story outcome.
- Confirm that the delivered documentation stays aligned with the current repository baseline: net10.0, AddDVault(), ApplyDataVaultMetadata(...), IDataVaultSaveService/DataVaultSaveRequest, and EF shared-type query examples.
- Preserve the blocked follow-up to 06EXB8202A88KJJP7WEGBESBYM for future post-publication NuGet-first guidance.

Scope Out
- Any new parent-only developer work on README.md, src/DCoding.Data.DVault, or tests/DCoding.Data.DVault.Tests beyond the already completed child outcomes.
- Package publication, release automation, live dotnet add package commands, version numbers, feed details, badges, or release-process instructions.
- New library APIs, SaveChanges interception, typed query abstractions, convenience wrappers, or any product-code change.
- Additional documentation splits, planning documents, attachments, or runnable examples that are not required to close this parent as an aggregation story.

Open questions
- none

Follow-up questions
- After 06EXB8202A88KJJP7WEGBESBYM is complete and the package is published, should README installation guidance switch to NuGet-first and move source-reference guidance into a contributor or from-source section?
- Should a later documentation or examples ticket promote the README getting-started flow into a runnable sample under examples/?
- If DVault later gains a higher-level read API, should the README quickstart replace the current shared-type query example?

Risks
- README snippets can still drift from the tested API surface over time if later library changes do not keep docs aligned with integration tests.
- Project-reference guidance will confuse future package consumers unless post-publication work under 06EXB8202A88KJJP7WEGBESBYM clearly flips the README to the published-install baseline.

Split recommendations
- No additional split recommended; the parent story should now remain only as the aggregation record over completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC.
- Do not create a new parent-only dev ticket from this story; any future publication or examples work should be scheduled separately, with 06EXB8202A88KJJP7WEGBESBYM already carrying the release-gate follow-up.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
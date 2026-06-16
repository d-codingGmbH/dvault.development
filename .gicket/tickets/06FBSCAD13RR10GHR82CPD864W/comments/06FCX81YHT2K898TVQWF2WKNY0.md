[gicket-bot] PO refinement contract

Summary
- Done MySQL bulk-gap evaluation ticket `06FBSC9JK29P1PVTCF6H3ZTEM8` already concluded the current MySQL bulk baseline is accepted as-is, so this follow-up should close as no-work-required rather than reopen implementation work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done evaluation ticket `06FBSC9JK29P1PVTCF6H3ZTEM8` concluded the repository already has the accepted MySQL bulk baseline: retained multi-row saves below the staged boundary, staged temporary-table bulk at 60-plus operations, and no current `LOAD DATA` lane.
- The visible MySQL gates are already bounded in code and tests: provider-native candidacy starts at 50 total operations, staged bulk starts at 60 total operations, and tiny satellite-only history batches deliberately fall back to the provider-neutral writer at 10 or fewer operations in one request or 100 or fewer across multiple requests.
- Root v0.39 MySQL bulk rows remain skipped placeholders when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset; completed local timing evidence lives in the earlier checked-in MySQL bundles, so skipped root placeholders must not be treated as missing MySQL bulk functionality.
- Earlier delivered MySQL tuning work already adjusted tiny-workload behavior and benchmark clarity; the completed gap evaluation found no distinct remaining implementation to accept inside this ticket.

Scope In
- Close this ticket as no-work-required based on the completed MySQL bulk-gap evaluation and the current repository-backed MySQL bulk baseline.
- Capture in the ticket handoff or closure note that the accepted MySQL save posture is the existing dual-lane baseline: retained multi-row below the staged boundary, staged temporary-table bulk at 60-plus operations, and provider-neutral fallback for tiny satellite-history batches.
- Point downstream documentation work to the existing evidence and caveats so MySQL bulk support is documented accurately without implying a missing implementation task.

Scope Out
- Any new MySQL provider code, threshold retune, or save-strategy selection change in this ticket.
- Any `LOAD DATA` or `LOAD DATA INFILE` experiment, deployment concern, or operational bulk-artifact work.
- Benchmark reruns or new benchmark artifact generation in this ticket.
- MySQL latest-satellite, PIT, or bridge read work.

Open questions
- none

Follow-up questions
- Should documentation ticket `06FBSCAX98ZFQZWBYEQMB8WF18` explicitly call out that MySQL already ships retained multi-row and staged temporary-table bulk lanes and that this implementation ticket closed no-work-required?
- If maintainers still want to explore `LOAD DATA` or revisit the 50 and 60 operation thresholds, which representative mixed hub, link, and satellite workloads should define that future ticket's evidence gate?

Risks
- If the closure note omits the completed evaluation and existing local evidence, readers may misread skipped v0.39 root MySQL rows as proof that MySQL bulk support is still missing.
- Reopening threshold or `LOAD DATA` work inside this ticket would blur a resolved no-work decision and bypass the fresh provider-configured evidence the completed evaluation said is required.
- Because this ticket blocks documentation task `06FBSCAX98ZFQZWBYEQMB8WF18`, leaving the no-work rationale implicit could keep downstream provider-bulk docs ambiguous.

Split recommendations
- Do not split within this ticket; close it as no-work-required.
- If future MySQL bulk experimentation is desired, create one separate task for `LOAD DATA` or threshold-retune benchmarking rather than reviving this ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
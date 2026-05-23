[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F54J1MZC6TVZENPAS13Y6V94`, `currentRevision=06F54JET4BDWBEQDNEZNP9HZXG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'e9b683a63d2bfab149af597149ad37b7181e295e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `a573d00b0888`.

Open questions / Risiken
- Blocking finding: The persisted contract still misstates the baseline: it says the repo does not prove a public `ReadShape` type or API, but current source, docs, and the API snapshot directly prove that `ReadShape` is already public and request-bound. Compatibility therefore ...
- Blocking finding: The acceptance criteria bundle already-shipped behavior together with possible new asks. Current `ReadShape` already covers filter columns, row-selection or cutoff rules, ordering, index baselines, provider caveats, registry equivalence, and redacted serializ...
- Blocking finding: The contract gives no source-backed before or after example for the remaining likely delta around projected-column, join-shape or count, or predicate-shape facts. Without that, developer handoff is ambiguous and risks duplicating or reshaping existing public ...
- Required PO action: Rewrite the story against the current source-backed baseline and explicitly acknowledge that `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService` already exist.
- Required PO action: Define the net-new additive delta in concrete terms per read family, for example exact new properties or records for projected columns, join shape or count, or predicate shape, and state which existing `ReadShape` members remain unchanged.
- Required PO action: Replace broad AC and DoD language with exact expected outputs or examples so already-shipped filter, order, index, provider, and redaction behavior is treated as baseline rather than new work.
- Required PO action: If Product cannot name a concrete additive delta beyond the shipped baseline, close or reclassify this ticket, or split a narrower follow-up instead of sending it to development as written.
- Risky assumption: It assumes the repo does not prove a public `ReadShape` API even though `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` and the approved API snapshot already do.
- Risky assumption: It assumes README and release-note documentation for `ReadShape` is still net-new or optional even though `README.md:527` and `docs/releases/v0.16.0.md:62` already document that payload.
- Risky assumption: It assumes explicit or registry equivalence and redacted serialization still need to be established even though current unit and integration tests already cover them.
- Split recommendation: No split is needed if PO can rewrite this into one narrow additive `ReadShape` enhancement story.
- Split recommendation: If Product wants separate workstreams, split only by concrete new outcome, for example one story for projected-column or predicate facts and a separate follow-up for any heavier provider-specific tuning guidance.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9279`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0bdb2729bf2943e38ea93ab60e8e7212`
- completed-at-utc: `<redacted>-23T00:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T005027230Z-0bdb2729bf2943e38ea93ab60e8e7212.json`
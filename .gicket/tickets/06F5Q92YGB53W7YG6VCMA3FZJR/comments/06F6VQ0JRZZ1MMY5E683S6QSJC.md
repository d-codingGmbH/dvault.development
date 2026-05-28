[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q92YGB53W7YG6VCMA3FZJR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92YGB53W7YG6VCMA3FZJR`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99HNNHKJGE7JEYC3C8VGC`, `currentRevision=06F6VK9TH1B94WGAMHXKT3NMT4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q92YGB53W7YG6VCMA3FZJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q92YGB53W7YG6VCMA3FZJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea' from source 'e4f7faacbfa8162e407a78db51922eb8f0eb139c'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the analyzer tries to infer authoritative metadata from anything broader than the projected support-bundle explain descriptor, false positives and contract drift are likely.
- Unsafe or low-confidence code fixes could mis-edit project files or conceal required design changes; most typed-read and generated-table misuse diagnostics should remain diagnostic-only unless one exact edit target is provable.
- Consumer docs can overpromise PIT/bridge/helper coverage unless the root README, analyzer README, and typed read contract language stay synchronized.
- Split recommendation: Keep the satellite typed read analyzer/generator slice aligned with existing downstream ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Split recommendation: Keep PIT/bridge-specific `DMV1963`/`DMV1964`/`DMV1967`/`DMV1969` behavior and tests aligned with existing downstream ticket `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Split recommendation: Do not widen this story into broader EF Core dataflow or runtime write-boundary enforcement beyond the existing source-visible `DMV1910`/`DMV1911` patterns; treat that as a separate future ticket if needed.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8216`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `dbd7f4efc8694f90b3eb62510de7a8be`
- completed-at-utc: `<redacted>-28T09:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/runs/20260528T091319809Z-dbd7f4efc8694f90b3eb62510de7a8be.json`
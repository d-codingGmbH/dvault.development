[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the current v0.47.0 analyzer compatibility baseline: keep the analyzer package on one `net10.0` asset, require a `.NET 10 SDK` build host for both `8.47.0` and `10.47.0` consumers, and treat pure `.NET 8 SDK` analyzer consumption as separate future scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No new planning writes or relation changes were needed for refinement; the current branch already contains `docs/plans/analyzer-package-compatibility-audit.md` plus aligned README and package-verifier guidance for the v0.47.0 baseline.
- For this ticket, the only ratified current recommendation is to keep `DCoding.Data.DVault.Analyzers` on one `net10.0` analyzer asset and make the `.NET 10 SDK` build-host requirement explicit for both coordinated package lines.
- Pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim; any lower-target or multi-target analyzer asset option belongs to separate additive work only if that product requirement is explicitly adopted.

Scope In
- Audit the current build-host compatibility baseline for `DCoding.Data.DVault.Analyzers` when `net8.0` consumers use the `8.47.0` package line.
- Record the supported analyzer asset shape, the blocker to broader host comfort, and the resulting package-verification expectations.
- Ratify the repository's current bounded recommendation from checked-in evidence instead of reopening already-fixed baseline decisions.
- Capture the recommendation, risks, and follow-up boundary so downstream work does not overstate compatibility.

Scope Out
- Retargeting the analyzer package to `net8.0`, `netstandard2.0`, or multi-target assets in this ticket.
- Changing runtime or provider package target frameworks, dependency lines, or coordinated package-family structure.
- General analyzer feature, diagnostic, or source-generator work unrelated to build-host compatibility.
- Claiming pure `.NET 8 SDK` analyzer consumption support without an explicit new verification lane.

Open questions
- none

Follow-up questions
- If the product requirement becomes `net8.0` projects built on a pure `.NET 8 SDK` host, should that be tracked as a separate compatibility commitment with its own analyzer asset-target change and smoke/verification lane?

Risks
- The current recommendation documents rather than removes `.NET 10 SDK` build-host friction for `net8.0` consumers; teams pinned to pure `.NET 8 SDK` toolchains still need separate product guidance or future work.
- Because both coordinated package lines ship the same analyzer asset, copied installation snippets can overstate compatibility unless the host-SDK caveat stays attached everywhere README content is surfaced.
- If a future change retargets analyzer assets without extending the verification lane, the repository could regress source-generator or analyzer behavior while appearing to broaden compatibility.

Split recommendations
- Do not split this audit further for current refinement; the bounded default is already clear from checked-in evidence.
- Create a separate additive ticket only if the team chooses to promise pure `.NET 8 SDK` analyzer consumption or another lower-friction host baseline.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
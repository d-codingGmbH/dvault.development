[gicket-bot] PO refinement contract

Summary
- Refined this ticket as the bounded version-line policy gate for v0.33.0: keep v0.32.0-and-earlier packages on 0.x, introduce same-ID 8.33.0 and 10.33.0 lines at v0.33.0, preserve coordinated pack and verification behavior, and rely on the already-existing sibling tickets for multitargeting, verifier, and documentation implementation; no additional split or attachment work was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the current consumer baseline to seven coordinated package IDs, README installation examples at 0.32.0, solution-level dotnet pack DVault.slnx --configuration Release --nologo, and MinVer v-prefixed planning tags; this ticket defines how that baseline branches into 8.x and 10.x package lines without changing package IDs.
- Planning release v0.33.0 is the first dual-line release: it maps to NuGet package versions 8.33.0 for the net8.0 and EF Core 8 line and 10.33.0 for the net10.0 and EF Core 10 line, while v0.32.0 and earlier stay on the historical 0.x line.
- Later planning releases follow the same rule: planning release v0.N.0 maps to package versions 8.N.0 and 10.N.0; documentation must distinguish planning release numbers from consumer-facing NuGet package versions.
- Keep the existing seven package IDs unchanged across all lines; do not introduce split artifact IDs such as line-specific package names. The compatibility line is expressed through the package major version, not the package ID.
- No human clarification comments or attachments were present on the ticket; the only live non-parent relation anomaly is an incoming blocks edge from done ticket 06F8KZVRARQPG482YKCQ686PNM, which should be treated as historical stale workflow state rather than an active product dependency.
- No new child tickets, attachments, or planning documents were materialized in this pass because the epic already carries the necessary implementation split.

Scope In
- Define the authoritative mapping from planning releases to historical 0.x, 8.x, and 10.x NuGet package versions.
- Define same-package-ID policy and reject line-specific package-ID splits.
- Define expected NuGet update behavior for consumers staying within a line versus moving between 0.x, 8.x, and 10.x.
- Define the solution-level pack and release shape needed to emit one coordinated seven-package family per line.
- Define package-verification expectations that reject mixed compatibility lines and mismatched provider and core package dependencies.
- Define documentation wording that separates planning release numbers from published package versions and preserves the current manual publication boundary.

Scope Out
- Implementing net8.0 and net10.0 multitargeting, conditional package references, or provider and version pinning changes already covered by sibling implementation tickets.
- Rewriting CI, package-verifier code, or documentation files in this ticket; this ticket only fixes the policy those tickets must follow.
- Introducing new runtime behavior, provider provisioning, or platform and tooling responsibilities.
- Backporting pre-v0.33.0 releases onto new package majors or republishing historical 0.x packages under different IDs.

Open questions
- none

Follow-up questions
- After 8.x and 10.x are established, should a later release define how future 11.x or other compatibility lines are introduced without repeating a full policy rewrite?
- Should release automation later standardize a named line-selection property or artifact-directory convention for the separate 8.x and 10.x pack runs, or keep that as implementation-owned build detail?

Risks
- Keeping the same package IDs across multiple major lines is the least disruptive continuation of the current repository baseline, but it increases the chance of consumer confusion if docs or examples blur planning release numbers and NuGet package versions.
- The current repository versioning baseline is single-line MinVer from v tags; sibling implementation work must add a line-selection mechanism carefully so pack outputs and approval evidence cannot accidentally mix 8.x and 10.x artifacts.
- The live relation graph still contains a historical incoming blocks edge from a done v0.32 ticket; until relation cleanup is replayed, workflow views may overstate active blocking state.

Split recommendations
- No additional split is recommended. The broader work is already adequately decomposed under epic 06F9G8EE7ZA666MW8YEB2QP8BW into this policy ticket, the compatibility-contract story, the multitargeting story, the verifier and CI task, and the documentation task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
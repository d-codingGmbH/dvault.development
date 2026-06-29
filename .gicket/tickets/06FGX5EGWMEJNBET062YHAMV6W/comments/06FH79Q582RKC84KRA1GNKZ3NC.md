[gicket-bot] PO refinement contract

Summary
- Fresh repository inspection shows the current branch already documents a pure .NET 8 SDK analyzer-host no-go; this refinement narrows the story to ratifying the existing .NET 10 SDK build-host baseline and keeping any true .NET 8 host enablement as follow-up split work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current branch evidence does not support a pure `.NET 8 SDK` analyzer-host claim. For this story, the bounded v1 outcome is the explicit no-go contract and support-matrix wording, not analyzer retargeting.
- Supported analyzer consumption remains one `net10.0` analyzer asset on a `.NET 10 SDK` build host for both coordinated package lines, including `net8.0` consumer projects on `8.50.0`.
- No bounded ticket writes, relation changes, attachments, or planning-document writes were materialized in this refinement run.

Scope In
- Ratify the current supported analyzer-host baseline for `DCoding.Data.DVault.Analyzers`: one `net10.0` analyzer asset consumed on a `.NET 10 SDK` build host for `8.50.0` and `10.50.0`.
- Keep package-verifier and documentation surfaces explicit that pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim.
- Capture the bounded blocker facts from current repository evidence: analyzer target/framework, SDK-local Roslyn/Workspaces/composition references, analyzer asset layout, dependency suppression, and absence of a `.NET 8 SDK` validation lane.

Scope Out
- Retargeting the analyzer package to `net8.0` or `netstandard2.0`, adding multi-target analyzer assets, or splitting code-fix assets/packages in this ticket.
- Adding analyzer runtime dependencies to consumer applications or widening the DVault runtime surface.
- Introducing a new `.NET 8 SDK` CI or package-verification lane as part of this ticket's current no-go outcome.

Open questions
- none

Follow-up questions
- If pure `.NET 8 SDK` analyzer consumption becomes a product requirement, should the preferred technical direction be a single `net8.0` asset, a broader `netstandard2.0` asset, or a split that isolates code fixes from analyzers and generators?
- If that work is approved, should it be scheduled as two tickets: first analyzer asset/dependency strategy, then `.NET 8 SDK` proof plus CI/package-verifier/documentation updates?

Risks
- Treating this as a direct implementation story without a split would hide two coupled workstreams: analyzer retargeting/dependency normalization and new `.NET 8 SDK` validation plus release-surface updates.
- Any attempt to claim pure `.NET 8 SDK` analyzer support before changing verifier and CI evidence would contradict the repository's current documentation and package checks.
- The code-fix provider's Workspaces and `System.Composition` coupling is the main technical risk for future host retargeting.

Split recommendations
- Follow-up ticket 1: retarget or split `DCoding.Data.DVault.Analyzers` for a supported `.NET 8 SDK` host, including Roslyn/Workspaces/composition/`System.Text.Json` dependency strategy and an explicit code-fix packaging decision.
- Follow-up ticket 2: after the analyzer asset strategy lands, add `.NET 8 SDK` proof surfaces across CI, package verifier, pack/release validation, analyzer README, root README, package-compatibility guidance, local validation, manual publication guidance, and release notes.

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
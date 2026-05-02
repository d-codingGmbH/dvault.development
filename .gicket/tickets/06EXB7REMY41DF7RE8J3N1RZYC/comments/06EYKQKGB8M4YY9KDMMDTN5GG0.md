[gicket-bot] PO refinement contract

Summary
- Refinement confirms this is a bounded consumer-documentation task: add current project-reference guidance on top of the existing README quickstart, keep NuGet installation clearly deferred, and hand off with no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The incoming `blocks` relation from `06EXB7R6MTJW1PYRN172MW34DM` matches repository evidence: the README quickstart already exists, so this ticket should extend that baseline rather than create a new quickstart.
- Current consumer docs in `README.md` say a .NET project already references `DCoding.Data.DVault`, but repository search found no current project-reference instructions and no live NuGet install instructions yet.
- The current library project is `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, the eventual package id is `DCoding.Data.DVault`, and the root `README.md` is reused as the package README through the project packing configuration.

Scope In
- Document how to consume DVault before publication by referencing the current library project from source.
- Add or refine consumer-facing installation wording so the existing quickstart no longer assumes an unexplained prior reference.
- Reserve NuGet installation guidance as clearly future or post-publication text that uses the known package id without claiming availability.

Scope Out
- Publishing `DCoding.Data.DVault` to NuGet or setting up release automation.
- Changing library code, public APIs, or the existing quickstart behavior beyond the installation and consumption framing needed for this ticket.
- Finalizing live `dotnet add package` commands, version numbers, feed details, badges, or release-process instructions before publication exists.

Open questions
- none

Follow-up questions
- After the first package publication, should the README switch to NuGet-first installation guidance and move project-reference usage into a separate from-source or contributor section?
- Which later ticket will own the exact published-package install commands, versioning examples, and any nuget.org-specific badges or release notes?

Risks
- If the future NuGet section includes executable commands before publication, the docs will immediately become false guidance.
- Because the root README is also reused as the package README, project-reference instructions must be clearly framed as pre-publication or from-source guidance so they do not confuse later package consumers.

Split recommendations
- No split recommended; the current evidence supports a single documentation ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
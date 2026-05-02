<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms this is a bounded consumer-documentation task: add current project-reference guidance on top of the existing README quickstart, keep NuGet installation clearly deferred, and hand off with no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The incoming `blocks` relation from `06EXB7R6MTJW1PYRN172MW34DM` matches repository evidence: the README quickstart already exists, so this ticket should extend that baseline rather than create a new quickstart.
- Current consumer docs in `README.md` say a .NET project already references `DCoding.Data.DVault`, but repository search found no current project-reference instructions and no live NuGet install instructions yet.
- The current library project is `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, the eventual package id is `DCoding.Data.DVault`, and the root `README.md` is reused as the package README through the project packing configuration.

### Scope In
- Document how to consume DVault before publication by referencing the current library project from source.
- Add or refine consumer-facing installation wording so the existing quickstart no longer assumes an unexplained prior reference.
- Reserve NuGet installation guidance as clearly future or post-publication text that uses the known package id without claiming availability.

### Scope Out
- Publishing `DCoding.Data.DVault` to NuGet or setting up release automation.
- Changing library code, public APIs, or the existing quickstart behavior beyond the installation and consumption framing needed for this ticket.
- Finalizing live `dotnet add package` commands, version numbers, feed details, badges, or release-process instructions before publication exists.

## Acceptance Criteria
- Primary consumer docs describe the current pre-publication installation path via local project reference to the DVault library project rather than implying a published package.
- Documentation does not state or imply that `DCoding.Data.DVault` is already available on NuGet.
- Any mention of NuGet installation is explicitly marked as future or post-publication guidance and is non-misleading for current users.
- Documentation stays consistent with the current README quickstart, repository layout, and the established package identity `DCoding.Data.DVault`.

## Definition of Done
- The chosen consumer-facing documentation surface is updated where users discover how to start using DVault, with `README.md` the preferred primary surface because it also feeds the packaged README.
- The project-reference guidance clearly points consumers at the current library project and does not contradict the visible repository structure around `DVault.slnx` and `src/DCoding.Data.DVault/`.
- Future NuGet wording remains clearly deferred and does not present false-current or failing installation steps.
- The documentation change follows shared formatting and documentation standards, including the repository formatting gate expectations.

## Implementation Notes
- Prefer updating `README.md` because current consumer guidance lives there and `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` packs that same README as the package README.
- Add a short installation section before or adjacent to Quickstart that explains the current prerequisite is a project reference to the DVault library project; the existing quickstart can then continue assuming the reference already exists.
- A representative `<ProjectReference>` example is acceptable; the exact relative path in the snippet is an implementation detail, but it should clearly target `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` as the current source project.
- Keep the future NuGet note intentionally minimal, naming `DCoding.Data.DVault` but deferring executable package-install commands until a publication or release ticket exists.
- No split is justified by the current evidence; this remains one bounded documentation ticket.

## Open Questions
- none

## Follow-Up Questions
- After the first package publication, should the README switch to NuGet-first installation guidance and move project-reference usage into a separate from-source or contributor section?
- Which later ticket will own the exact published-package install commands, versioning examples, and any nuget.org-specific badges or release notes?

## Risks
- If the future NuGet section includes executable commands before publication, the docs will immediately become false guidance.
- Because the root README is also reused as the package README, project-reference instructions must be clearly framed as pre-publication or from-source guidance so they do not confuse later package consumers.

## Split Recommendations
- No split recommended; the current evidence supports a single documentation ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Document how to consume the library before and after NuGet publication.

## Scope
- Show project reference usage now.
- Reserve NuGet installation text for the later release gate.

## Acceptance Criteria
- Docs do not claim a NuGet package is already published.
- Future package instructions are marked appropriately.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.
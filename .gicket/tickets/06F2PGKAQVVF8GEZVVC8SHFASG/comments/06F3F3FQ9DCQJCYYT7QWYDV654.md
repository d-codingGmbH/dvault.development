[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites\u0027 at commit \u00274db8a56e2cf6\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites",
    "commitSha": "4db8a56e2cf6",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "DataVaultCodeFirstLinkBuilder gains an additive generic Satellite\u003CTSatellite\u003E(...) fluent entry point, and existing Link(...) overloads plus Participant\u003CTEntity\u003E() behavior remain unchanged for current callers.",
      "satisfied": true,
      "reason": "\u0060git diff develop...4db8a56e2cf6\u0060 adds \u0060DataVaultCodeFirstLinkBuilder.Satellite\u003CTSatellite\u003E(string, Action\u003C...\u003E?)\u0060 in \u0060src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs\u0060; the existing \u0060Participant\u003CTEntity\u003E()\u0060 body and both \u0060Link(...)\u0060 overloads in \u0060DataVaultCodeFirstModelBuilder\u0060 are unchanged."
    },
    {
      "expectation": "The link-level satellite configure callback supports the established Code-First satellite verbs needed by this story: payload member declaration and optional driving-key declaration in caller order.",
      "satisfied": true,
      "reason": "The new link entry point reuses \u0060DataVaultCodeFirstSatelliteBuilder\u003CTSatellite\u003E\u0060, where \u0060DrivingKey(...)\u0060 and \u0060Payload(...)\u0060 append names to ordered declaration lists; \u0060DataVaultCodeFirstLinkTests.ApplyDataVaultMetadataProjectsDerivedNameLinkParentSatelliteThroughMetadataTranslator\u0060 declares \u0060DrivingKey\u0060 then two \u0060Payload\u0060 calls and asserts the resulting metadata order."
    },
    {
      "expectation": "Code-First metadata projection carries link-parent satellite declarations alongside ordered link participants and emits each link-parent satellite with the resolved link as parent.",
      "satisfied": true,
      "reason": "\u0060BuildMetadataModel()\u0060 now carries \u0060LinkDeclaration.Satellites\u0060 through \u0060_links.Zip(links, ...)\u0060 and creates each satellite with \u0060link.ToReference()\u0060 as parent, so link participants stay ordered while link-parent satellites enter the projected metadata model with parent kind \u0060Link\u0060."
    },
    {
      "expectation": "Regression tests show a caller-owned CLR type can declare a link-parent satellite and that the declaration reaches metadata projection plus at least one downstream contract surface.",
      "satisfied": true,
      "reason": "Regression coverage was added in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs\u0060 for code-first declaration plus EF metadata translation and in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs\u0060 for downstream \u0060dvault.model.v1\u0060 export/import; the public API snapshot was updated in \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "A developer can declare a link-parent satellite from the existing link builder without regressing current participant declaration behavior.",
      "satisfied": true,
      "reason": "A caller can now declare \u0060link.Satellite\u003CCustomerOrderState\u003E(...)\u0060 directly from the existing link builder, and the committed diff leaves current participant declaration code untouched while existing link tests for explicit and derived participant ordering remain present."
    },
    {
      "expectation": "Projected metadata includes link-parent satellite output with preserved caller order for payload and optional driving-key declarations.",
      "satisfied": true,
      "reason": "Projected metadata includes a link-parent satellite whose parent is \u0060CustomerOrder\u0060; the committed test asserts \u0060Parent.Kind = Link\u0060, \u0060Parent.Name = CustomerOrder\u0060, \u0060DrivingKeyNames = [StateSource]\u0060, and payload order \u0060[StatusCode, StateChangedAt]\u0060, matching the ordered list behavior in \u0060SatelliteDeclaration\u0060 and \u0060DataVaultSatelliteMetadata\u0060."
    },
    {
      "expectation": "Automated tests cover API shape, metadata translation, and one downstream output path for link-parent satellites.",
      "satisfied": true,
      "reason": "Automated coverage is present for API shape via \u0060ApiSurfaceSnapshotTests\u0060 plus the updated approved snapshot, for metadata translation via \u0060DataVaultCodeFirstLinkTests\u0060, and for a downstream artifact path via \u0060DataVaultModelArtifactExporterTests\u0060."
    },
    {
      "expectation": "Documentation and release-note follow-through stays on ticket 06F2PGM9038RXVJH0RJFYEJEV0.",
      "satisfied": true,
      "reason": "Documentation and release-note follow-through remains separated on ticket \u006006F2PGM9038RXVJH0RJFYEJEV0\u0060: \u0060.gicket/relations/SG/V0/06F2PGKAQVVF8GEZVVC8SHFASG--06F2PGM9038RXVJH0RJFYEJEV0--blocks.json\u0060 still records that dependency, and \u0060git diff --name-only develop...4db8a56e2cf6\u0060 shows no README or docs files changed for this ticket."
    }
  ],
  "evidence": [
    "\u0060git rev-parse --verify 4db8a56e2cf6\u0060 resolved the claimed implementation commit, and because repository HEAD is later (\u00603148957db099554cffcef163a07e96117c3e118f\u0060), the review used \u0060git show 4db8a56e2cf6:path\u0060 plus \u0060git diff develop...4db8a56e2cf6\u0060 to avoid later branch drift.",
    "\u0060git diff --name-only develop...4db8a56e2cf6 -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests\u0060 shows exactly five product-facing changes: \u0060src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
    "\u0060git diff develop...4db8a56e2cf6 -- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs\u0060 shows an additive \u0060Satellite\u003CTSatellite\u003E(string satelliteName, Action\u003CDataVaultCodeFirstSatelliteBuilder\u003CTSatellite\u003E\u003E? configure = null)\u0060 method appended below \u0060Participant\u003CTEntity\u003E()\u0060.",
    "\u0060git show 4db8a56e2cf6:src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs\u0060 shows \u0060LinkDeclaration.Satellites\u0060, \u0060linkSatellites = _links.Zip(links, ...)\u0060, and \u0060CreateSatelliteMetadata(DataVaultLinkMetadata link, SatelliteDeclaration satellite)\u0060 calling \u0060link.ToReference()\u0060.",
    "\u0060git show 4db8a56e2cf6:tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs\u0060 adds \u0060ApplyDataVaultMetadataProjectsDerivedNameLinkParentSatelliteThroughMetadataTranslator\u0060, which asserts participant order \u0060Customer, Order\u0060, satellite parent kind/name \u0060Link/CustomerOrder\u0060, driving key \u0060StateSource\u0060, payload order \u0060StatusCode, StateChangedAt\u0060, and relational entity/index shape \u0060SatCustomerOrderState\u0060.",
    "\u0060git show 4db8a56e2cf6:tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs\u0060 adds \u0060ExportJsonFromCodeFirstDeclarationsIncludesLinkParentSatellites\u0060, which exports code-first declarations, checks for \u0060\u0022kind\u0022: \u0022link\u0022\u0060 in the JSON, and round-trips the imported satellite metadata.",
    "\u0060git show 4db8a56e2cf6:tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs\u0060 already contains \u0060CorePublicApiMatchesApprovedSnapshot\u0060, and the updated approved snapshot now includes the new \u0060DataVaultCodeFirstLinkBuilder.Satellite\u003CTSatellite\u003E(...)\u0060 signature.",
    "\u0060git diff --check develop...4db8a56e2cf6 -- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 returned no diff-format violations.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/code-first, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 4 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F2PGKV9AFAMKGJEKKZ3AXHGC-story-add-code-first-effectivity-satellite-suppo\u0027.",
    "Ticket history references implementation commit \u00274db8a56e2cf6\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed to the integrator gate on the reviewed commit \u00604db8a56e2cf6\u0060.",
    "If policy still requires executable confirmation outside this read-only tester session, run legacy verification for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 in the supported host environment."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F2PGKAQVVF8GEZVVC8SHFASG`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' at commit '4db8a56e2cf6'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites`
- implementation-commit: `4db8a56e2cf6`
- implementation-pr: `<none>`
- implementation-change: `<none>`
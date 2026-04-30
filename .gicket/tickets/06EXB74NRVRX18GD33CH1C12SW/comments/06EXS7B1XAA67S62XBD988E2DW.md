[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "dev",
  "targetRole": "po",
  "resumeRole": "dev",
  "returnKind": "clarification_needed",
  "returnCategory": "missing_repo_state",
  "summary": "Stopped before making repository changes because the bot-provided interactive tool loop must execute declared repository tools for this unattended Gicket run.",
  "changesApplied": [
    "Prepared interactive developer scratch worktree for target branch \u0027ticket/06EXB74NRVRX18GD33CH1C12SW-story-model-data-vault-building-blocks\u0027 from source \u0027ticket/06EXB74NRVRX18GD33CH1C12SW-story-model-data-vault-building-blocks\u0027.",
    "Triggered developer repair attempt 1/3 after isolated workspace test failure.",
    "Triggered developer repair attempt 2/3 after isolated workspace test failure.",
    "Update labels for handoff to role \u0027po\u0027.",
    "Ticket already in configured handoff status \u0027todo\u0027."
  ],
  "findings": [
    "Open question: Expose the declared repository tools through the adapter tool surface, or rerun with direct repository mutation enabled for this role so the implementation artifacts can be repaired and verified.",
    "Risk: Returning source artifacts without re-reading and verifying the current repository state would risk repeating the prior failing format-gate plan unchanged.",
    "Clarification category: missing_repo_state.",
    "Return routing requested: clarification_needed."
  ],
  "evidence": [],
  "nextSteps": [
    "Clarify before implementation: Expose the declared repository tools through the adapter tool surface, or rerun with direct repository mutation enabled for this role so the implementation artifacts can be repaired and verified."
  ],
  "branchName": null,
  "commitSha": null
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-po`
- transaction-point: `TP9`
- ticket-id: `06EXB74NRVRX18GD33CH1C12SW`
- target-role: `po`
- return-kind: `clarification_needed`

- return-category: `missing_repo_state`
- resume-role: `dev`
- branch: `<none>`
- return-commit: `<none>`
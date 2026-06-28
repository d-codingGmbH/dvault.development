Update the Hash-Key Storage Migration Guide and related docs for the machine-checkable manifest validation path.

Acceptance:
- Docs show how to produce and validate a dry-run manifest before changing EF migrations or data.
- Docs state that DVault does not execute the migration or automatically rewrite persisted keys.
- README and release notes point existing HexString users to the validated dry-run flow.
- Package verifier expectations are updated only if packaged README guidance changes.
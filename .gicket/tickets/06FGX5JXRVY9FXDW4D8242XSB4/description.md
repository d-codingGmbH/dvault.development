Add deterministic verification for the analyzer build-host claim selected by this release.

Acceptance:
- Package verification checks the analyzer asset layout and README claim for both package lines.
- A small smoke project or test proves the supported .NET 8 SDK-host behavior when the audit and implementation make it feasible.
- Unsupported host combinations fail with clear package-verifier or documentation evidence rather than silent assumptions.
- The test lane remains compatible with the repository validation commands.
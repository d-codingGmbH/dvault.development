## Goal

Add the first high-confidence analyzer rules with tests.

## Scope In

- Set up analyzer test infrastructure.
- Implement at least two rules with diagnostic ids and messages.
- Add positive and negative code samples.

## Scope Out

- No code fixes unless trivial and safe.
- No broad semantic analysis that creates noisy warnings.

## Acceptance Criteria

- Analyzer tests cover true positive and false positive guards.
- Diagnostics use documented ids and categories.
- Package/project layout matches repository conventions.

## Implementation Notes

- Keep analyzer scope tight.

## Open Questions

- none
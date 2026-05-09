## Goal

Benchmark and optimize the highest-impact read paths after provider-neutral latest/as-of/PIT/bridge correctness is in place.

## Scope In

- Benchmark matrix for latest satellite, PIT, and bridge reads across available providers.
- Provider-specific read strategy hook surface where beneficial.
- First provider-specific read optimization selected by measured impact.

## Scope Out

- Optimizing every provider/read shape in one story.
- Changing write strategy behavior unless required by read correctness.

## Acceptance Criteria

- Baseline and optimized measurements are documented reproducibly.
- Provider-specific read optimization is selected by evidence, not assumption.
- Fallback read behavior remains correct and available.
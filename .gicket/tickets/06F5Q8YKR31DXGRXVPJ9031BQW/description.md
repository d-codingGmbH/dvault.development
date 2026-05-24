Goal: Define the internal provider staging contract for staged bulk ingestion.

Acceptance criteria:
- Specifies staging lifecycle, naming, transaction ownership, cleanup, cancellation, and concurrency behavior.
- Defines normalized hub/link/satellite row handoff to provider implementations without unstable public APIs.
- Adds diagnostics gates for unsupported providers, dirty contexts, multi-active shapes, oversized batches, and schema limitations.
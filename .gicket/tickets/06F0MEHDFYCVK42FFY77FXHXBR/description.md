## Goal

Design a bounded bridge traversal helper API for the bridge metadata baseline without promising a full graph-query engine.

## Scope In

- Many-to-many bridge traversal request/response shape.
- Bounded hierarchy traversal semantics using existing bridge metadata fields.
- Failure modes for unsupported traversal depth or missing bridge rows.

## Scope Out

- Full recursive graph engine.
- Provider-specific query tuning.

## Acceptance Criteria

- Contract distinguishes implemented baseline traversal from future advanced graph behavior.
- Request and response types can support typed projection later.
- Examples use current bridge metadata concepts only.
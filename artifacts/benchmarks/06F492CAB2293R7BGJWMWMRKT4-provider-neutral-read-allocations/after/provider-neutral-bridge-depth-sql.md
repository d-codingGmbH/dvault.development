# Provider-Neutral Bridge Depth SQL Capture

Ticket: `06F492CAB2293R7BGJWMWMRKT4`
Evidence set: `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/after`
Benchmark row: `bridge-traversal-read` / `dvault-adddvault-fallback`

The provider-neutral bridge read pipeline now applies the hierarchy maximum-depth predicate before EF Core materializes shared-type dictionary rows. For the benchmark request:

- table: `BridgeSalesRegionHierarchy`
- endpoint filter column: `AncestorSalesRegionHashKey`
- endpoint key: `region-root`
- maximum depth: `3`

Representative SQL shape:

```sql
SELECT "b"."AncestorSalesRegionHashKey", "b"."DescendantSalesRegionHashKey", "b"."TraversalDepth"
FROM "BridgeSalesRegionHierarchy" AS "b"
WHERE "b"."AncestorSalesRegionHashKey" = @__endpointHashKey_0
  AND "b"."TraversalDepth" <= @__maximumDepth_1;
```

The in-memory traversal-depth guard remains in the pipeline as a correctness backstop, but the targeted allocation improvement comes from avoiding materialization of rows with `TraversalDepth` greater than the requested bound.

# DVault V2 Redacted Read-Plan Explain Contract

Status: v2 contract
Ticket: 06F7Y0FZXX5J0G7G15681HVEBR
Current public baseline: [DVault v0.26.0 Release Notes](../releases/v0.26.0.md)

## Decision

DVault v2 read-plan explainability is the request-bound diagnostics surface exposed by `IDataVaultReadDiagnosticsService.Analyze(...)`. The service returns the existing `DataVaultDiagnosticsResult` shape:

- `ReadStrategy` is the authoritative provider strategy section.
- `ReadShape` is an additive, nullable, request-bound shape section for translated read facts.
- Support-bundle export serializes the same bounded data as deterministic camelCase JSON under `readShape` when callers supply representative request-bound read diagnostics.

This contract formalizes the current diagnostics and support-bundle shape. It does not create a new query execution API, query planner, LINQ provider, raw-SQL advisor, automatic-index advisor, or provider physical-plan promise.

The same redacted `ReadShape` payload is also the reviewed support-bundle evidence consumed by the typed read-model generator for PIT and bridge helpers. That generator remains a separate compile-time surface documented by [DVault V1 Typed PIT And Bridge Helper Contract](dvault-v1-typed-pit-bridge-helper-contract.md); this diagnostics contract defines the value-free facts it can rely on.

## Closed Vocabularies

The read-shape kind vocabulary is closed for this v2 contract:

- `LatestSatellite`
- `PitAsOf`
- `Bridge`

Satellite request semantics use:

- `Current`
- `AsOf`

Bridge traversal requests use the existing traversal vocabulary and endpoint enums. User-facing guidance may also use the existing Activity tracing read-mode term `Traversal` for bridge reads.

Read-strategy status values are:

- `NotEvaluated`
- `ProviderStrategySelected`
- `ProviderNeutralFallback`

Read-strategy fallback causes are the finite `DataVaultReadStrategyFallbackCauseKind` values:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `UnsupportedSatelliteParent`
- `MultiActiveSatelliteUnsupported`
- `StrategyDeclined`
- `UnsupportedPitShape`
- `UnsupportedBridgeShape`

Fallback causes stay machine-readable enum values with bounded messages. Consumers should not parse fallback prose as a compatibility contract.

## Provider Facts

Provider facts are deterministic diagnostics, not observed provider execution plans. The serialized diagnostics result exposes:

- `readStrategy.providerName`
- `readStrategy.status`
- `readStrategy.selectedStrategyName` when a provider-specific strategy is selected
- `readStrategy.fallbackCauses`
- `readShape.provider.providerName`
- `readShape.provider.capabilityProfileName`
- `readShape.provider.capabilityProfileDefaulted`
- `readShape.provider.providerBehaviorProfileName`
- `readShape.provider.providerBehaviorDefaulted`
- `readShape.provider.readStrategyStatus`
- `readShape.provider.selectedStrategyName` when a provider-specific strategy is selected
- `readShape.provider.readStrategyFallbackCauses`

`selectedStrategyName` is omitted when no provider-specific strategy is selected. Other non-applicable optional fields are also omitted rather than populated with placeholder text or sentinel values.

## Latest And As-Of Satellite Shape

For `DataVaultReadShapeKind.LatestSatellite`, `readShape.satellite` contains:

- `semantics`: `Current` for current/latest reads or `AsOf` for cutoff-bound reads.
- `satellite`: translated satellite metadata name, table kind, and table name.
- `parentReference`: translated parent reference kind and name.
- `filterColumns`: `parentHashKeyFilter` and, for as-of reads, `asOfCutoff`.
- `seriesSelectionRule`: bounded guidance describing latest-row selection per parent hash key and driving-key series.
- `cutoffRule`: bounded guidance describing whether an as-of cutoff applies, without the supplied cutoff value.
- `deterministicOrdering`: the generated column groups that make result ordering deterministic.
- `projectedColumns`: technical, payload, and optional driving-key projection groups.
- `expectedIndexBaseline`: translated key and secondary-index baselines from metadata.

The shape may include generated metadata identifiers, table names, column names, index names, and enum values. It must not include raw parent hash-key request values, raw business keys, payload values, as-of timestamp values, SQL text, provider query plans, or provider errors.

## PIT As-Of Shape

For `DataVaultReadShapeKind.PitAsOf`, `readShape.pit` contains:

- `pit`: translated PIT metadata name, table kind, and table name.
- `parentReference`: translated parent reference kind and name.
- `referencedSatellites`: referenced satellite metadata names, table names, PIT snapshot reference columns, satellite parent hash-key columns, satellite load-timestamp columns, and driving-key columns.
- `filterColumns`: `parentHashKeyFilter` and `asOfCutoff`.
- `rowIdentityColumns`: PIT row identity columns used for selection and disambiguation.
- `pitRowSelectionRule`: bounded guidance for selecting the latest PIT row per parent, or per parent and driving-key tuple, before the supplied cutoff.
- `snapshotLookupBehavior`: bounded guidance for resolving referenced satellite snapshots from selected PIT row references.
- `noLatestFallbackBehavior`: the explicit no-latest-satellite-fallback rule for missing PIT rows or null snapshot references.
- `maintainedPitPrerequisite`: the prerequisite that PIT rows already be maintained.
- `projectedColumns`: PIT technical, optional PIT driving-key, snapshot-reference, and satellite-payload projection groups.
- `referencedSatelliteLookupCount`: the number of referenced satellite snapshot lookups required by the shape.
- `expectedIndexBaseline`: translated key and secondary-index baselines from metadata.

PIT explain output describes maintained PIT access and snapshot binding. It does not imply read-time PIT maintenance, latest-satellite fallback, automatic PIT rebuilds, or provider-specific physical design.

## Bridge Shape

For `DataVaultReadShapeKind.Bridge`, `readShape.bridge` contains:

- `bridgeKind`: the bridge kind, such as `ManyToMany` or `Hierarchy`.
- `bridge`: translated bridge metadata name, table kind, and table name.
- `endpoints`: endpoint roles, endpoint names, and endpoint hash-key columns.
- `filterEndpoint`: the selected filter endpoint.
- `endpointFilter`: the selected endpoint hash-key filter column group.
- `depthPredicate`: an optional maximum-depth predicate column group for bounded hierarchy traversal.
- `deterministicOrdering`: generated endpoint and optional traversal-depth columns used for result ordering.
- `supportedEndpointRules`: bounded guidance for endpoint rules supported by the bridge kind.
- `projectedColumns`: endpoint and optional depth projection groups.
- `expectedTraversalIndexBaseline`: translated key and secondary-index baselines for traversal.

Bridge explain output describes one maintained bridge table and one endpoint-filtered traversal request. It does not add graph traversal APIs, path payload contracts, closure-state contracts, automatic bridge maintenance, or provider physical-plan inspection.

## Support-Bundle Helper Evidence Example

When application code supplies representative request-bound diagnostics through `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics`, `dvault.support-bundle.v1` can carry the same redacted `readShape` section that typed helper generation consumes. The generic design-time command runner does not invent these requests.

This deterministic example includes translated table and column facts, read-strategy status, and fallback data while omitting raw endpoint hash keys, request timestamps, SQL text, provider physical plans, credentials, and provider error text:

```json
{
  "diagnostics": {
    "readStrategy": {
      "status": "ProviderNeutralFallback",
      "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
      "fallbackCauses": [
        {
          "kind": "UnsupportedBridgeShape"
        }
      ]
    },
    "readShape": {
      "kind": "Bridge",
      "provider": {
        "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
        "capabilityProfileName": "sqlite",
        "capabilityProfileDefaulted": false,
        "providerBehaviorProfileName": "sqlite",
        "providerBehaviorDefaulted": false,
        "readStrategyStatus": "ProviderNeutralFallback",
        "readStrategyFallbackCauses": [
          {
            "kind": "UnsupportedBridgeShape"
          }
        ]
      },
      "bridge": {
        "bridgeKind": "Hierarchy",
        "bridge": {
          "metadataName": "SalesRegionHierarchy",
          "tableKind": "Bridge",
          "tableName": "BridgeSalesRegionHierarchy"
        },
        "endpoints": [
          {
            "endpoint": "Ancestor",
            "endpointName": "SalesRegion",
            "columnName": "AncestorSalesRegionHashKey"
          },
          {
            "endpoint": "Descendant",
            "endpointName": "SalesRegion",
            "columnName": "DescendantSalesRegionHashKey"
          }
        ],
        "filterEndpoint": "Ancestor",
        "endpointFilter": {
          "role": "endpointHashKeyFilter",
          "columnNames": [
            "AncestorSalesRegionHashKey"
          ]
        },
        "depthPredicate": {
          "role": "maximumDepthPredicate",
          "columnNames": [
            "TraversalDepth"
          ]
        },
        "projectedColumns": [
          {
            "role": "endpointProjection",
            "columnNames": [
              "AncestorSalesRegionHashKey",
              "DescendantSalesRegionHashKey"
            ]
          },
          {
            "role": "depthProjection",
            "columnNames": [
              "TraversalDepth"
            ]
          }
        ]
      }
    }
  }
}
```

## Redaction Rules

Read-plan explain output is value-free diagnostics. Diagnostics, support bundles, logs, telemetry, and user-facing guidance for this contract must not include:

- raw request keys.
- raw business keys.
- raw hash-key values.
- raw as-of, cutoff, load-timestamp, or other timestamp values supplied by a request.
- payload values.
- record-source values.
- SQL text.
- provider query plans or physical execution plans.
- credentials.
- connection strings.
- provider error text.
- exception text or stack traces.
- secret-bearing dumps or high-cardinality request data.

Allowed output is limited to deterministic schema and metadata facts, finite enum vocabularies, generated table and column identities, row-selection and ordering rules, expected key/index baselines, provider names, provider profile names, defaulting flags, selected strategy name when present, read-strategy status, and finite fallback causes.

## Omission Rules

Optional facts are omitted when they do not apply:

- `readShape` is omitted or null for diagnostics that are not request-bound read diagnostics.
- `selectedStrategyName` is omitted when no provider-specific read strategy is selected.
- satellite `asOfCutoff` appears only for as-of satellite requests.
- satellite driving-key projection appears only for multi-active satellite shapes.
- PIT driving-key projection and driving-key row identity columns appear only for PIT shapes that include driving-key tuples.
- bridge `depthPredicate` and depth projection appear only for bounded hierarchy requests.

Support-bundle JSON should keep finite arrays as arrays, including empty fallback lists, rather than collapsing them into prose.

## Non-Goals

This contract does not provide:

- new runtime read execution APIs.
- direct typed helper emission or new helper shapes by this diagnostics contract; helper generation consumes this evidence through the separate generator contract.
- LINQ provider behavior or query-planner behavior.
- raw SQL capture.
- provider query-plan export.
- provider physical-plan inspection.
- automatic index creation.
- automatic index recommendations.
- provider-specific physical-design promises.
- PIT or bridge maintenance changes.
- strategy dispatch algorithm changes.
- credential-bearing diagnostics.

Consumers that need raw SQL or physical execution-plan evidence should capture that in a separate consumer-owned workflow with its own redaction, storage, and review rules.

## Evidence

Focused repository evidence:

- [DataVaultDiagnostics.cs](../../src/DCoding.Data.DVault/DataVaultDiagnostics.cs) defines `IDataVaultReadDiagnosticsService`, `DataVaultDiagnosticsResult.ReadStrategy`, `DataVaultDiagnosticsResult.ReadShape`, read-shape payload records, and finite read-strategy vocabularies.
- [DataVaultSupportBundleExporter.cs](../../src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs) serializes deterministic camelCase support-bundle JSON and omits null optional fields.
- [DataVaultDiagnosticsTests.cs](../../tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs) covers representative satellite, PIT, and bridge read-shape serialization, provider-selected and provider-neutral fallback exposure, and request-value redaction.
- [DataVaultPitReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs) covers SQLite PIT as-of read-shape diagnostics and provider-selected versus provider-neutral fallback states.
- [DataVaultBridgeReadServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs) covers SQLite bridge read-shape diagnostics and provider-selected versus provider-neutral fallback states.

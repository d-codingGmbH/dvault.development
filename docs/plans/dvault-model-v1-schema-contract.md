# dvault.model.v1 Schema And Validation Contract

Status: v1 planning contract
Ticket: 06F0MEE8T9PKPKQH8EPWNQ2CRW
Consumers: 06F0MEEGJE9QCHC8YN4FEXYX10, 06F0MEERJ7D5Q4WYBQAJD3GFVC, 06F0MEF08AJ1K52STF42T74B04, 06F0MEGAGJCEHQ8QRHGH8W7804

## Purpose

This document defines the durable JSON-first `dvault.model.v1` artifact contract for model-first Data Vault declarations. It fixes field names, token names, default values, compatibility behavior, validation categories, and representative fixture expectations before parser, diagnostics, YAML boundary, projection, and governance implementation tickets consume the contract.

The contract stays provider-neutral except for one explicit load timestamp storage choice. It maps valid documents to visible DVault metadata semantics where those semantics exist today and explicitly permits downstream implementation tickets to add narrow model-first adapters for role-bearing recursive links, PIT metadata, bridge metadata, diagnostics, or projection gaps that are not exposed by the current public API.

## Non-Goals

- No parser, importer, exporter, command-line interface, build integration, code generation, drift tooling, runtime model mutation, or YAML dependency is defined here.
- No provider-specific DDL, SQL, migration behavior, table name override surface, column name override surface, or provider-specific capability section is defined in v1.
- No promise is made that existing code-first link APIs can represent repeated same-hub participants with roles. Model-first implementation may add a narrow adapter for that shape.

## Document Envelope

The artifact is a JSON object. The only required top-level field is `schemaVersion`. All declaration arrays are optional and default to empty arrays. Unknown fields at any object level are validation errors.

```json
{
  "schemaVersion": "dvault.model.v1",
  "naming": {
    "policy": "default"
  },
  "loadTimestampStorage": "provider-default",
  "hubs": [],
  "links": [],
  "satellites": [],
  "pits": [],
  "bridges": []
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `schemaVersion` | yes | none | Must be the exact string `dvault.model.v1`. Missing values, non-string values, unsupported major versions, unsupported minor versions, and alternate dialect strings are errors. |
| `naming` | no | `{ "policy": "default" }` | When present, must be an object containing only `policy`. |
| `naming.policy` | no | `default` | The only v1 token is `default`, meaning the repository default naming policy. |
| `loadTimestampStorage` | no | `provider-default` | Supported tokens are `provider-default`, `iso-8601-utc-text`, and `utc-ticks`. |
| `hubs` | no | `[]` | Ordered hub declarations. |
| `links` | no | `[]` | Ordered link declarations. |
| `satellites` | no | `[]` | Ordered hub-parent or link-parent satellite declarations. |
| `pits` | no | `[]` | Ordered point-in-time declarations over one hub and that hub's satellites. |
| `bridges` | no | `[]` | Ordered many-to-many or hierarchy bridge declarations. |

Version compatibility is strict for v1. A consumer that only implements this contract must reject any `schemaVersion` other than `dvault.model.v1` and must not silently accept `dvault.model`, `dvault.model.v1.0`, `dvault.model.v2`, or vendor-prefixed dialect names.

## YAML Authoring Boundary

`dvault.model.v1` ingestion is JSON-first. DVault v1 accepts the canonical JSON artifact described by this contract and does not define a direct YAML parser, YAML ingestion API, YAML fixture contract, or core package YAML dependency.

YAML may be used as an authoring convenience only when conversion happens outside DVault before ingestion. The converted artifact must be the same JSON object shape described in the document envelope and must use the exact `schemaVersion` value `dvault.model.v1`. After conversion, the JSON artifact uses the same token values, declaration ordering, default values, unknown-field behavior, ordinal string comparisons, diagnostics, and validation-to-projection path as hand-authored JSON.

YAML-specific behavior is outside the v1 contract. Conversion must not add YAML-only fields, merge semantics, anchors, tags, comment preservation, duplicate-key handling rules, or YAML-specific diagnostics to DVault. If a future release adds first-party YAML ingestion, it must do so through a separate additive contract instead of changing the authoritative v1 JSON artifact shape.

## Token Registry

All token comparisons and declaration-name comparisons use ordinal string semantics.

| Token family | Supported values |
| --- | --- |
| `schemaVersion` | `dvault.model.v1` |
| `naming.policy` | `default` |
| `loadTimestampStorage` | `provider-default`, `iso-8601-utc-text`, `utc-ticks` |
| `satellites[].parent.kind` | `hub`, `link` |
| `bridges[].kind` | `many-to-many`, `hierarchy` |
| Diagnostic severity | `error`, `warning` |

## Hub Declarations

Hub declarations create stable business-identity metadata.

```json
{
  "name": "Customer",
  "businessKeys": ["TenantId", "CustomerNumber"]
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `name` | yes | none | Stable logical hub name. Must be a non-empty string. Duplicate hub names are errors. |
| `businessKeys` | yes | none | Non-empty ordered array of non-empty strings. Order is the canonical business-key order. Duplicate names within one hub are errors. |

Projection should map ordinary hub declarations to `DataVaultHubMetadata` or the equivalent registry-backed metadata surface. The existing metadata baseline carries hash key, load timestamp, record source, and ordered business-key semantics.

## Link Declarations

Link declarations create ordered relationships over hub participants.

```json
{
  "name": "CustomerOrder",
  "participants": [
    { "hub": "Customer" },
    { "hub": "Order" }
  ]
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `name` | yes | none | Stable logical link name. Must be a non-empty string. Duplicate link names are errors. |
| `participants` | yes | none | Ordered array with at least two participant objects. Order is the canonical participant order. |
| `participants[].hub` | yes | none | Existing hub name. References to links or satellites are wrong-reference-kind errors. |
| `participants[].role` | no | none | Non-empty role used to disambiguate repeated hub participants and bridge endpoint binding. Roles must be unique within a link by ordinal comparison. |

Distinct-hub links may omit participant roles. Repeated same-hub participants are valid only when every repeated occurrence has a role. A repeated same-hub link without roles is ambiguous and must fail validation. A role on a non-repeated participant is allowed and preserved.

Projection should map links with distinct hubs and no role-sensitive needs to current link metadata. Role-bearing or repeated-hub links may use a narrow additive model-first adapter so participant order, hub names, and role names survive validation and projection.

## Satellite Declarations

Satellite declarations attach descriptive payload to one hub or one link. A non-empty `drivingKeys` array opts the satellite into multi-active semantics.

```json
{
  "name": "CustomerProfile",
  "parent": {
    "kind": "hub",
    "name": "Customer"
  },
  "payload": ["Name", "EmailAddress"],
  "drivingKeys": []
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `name` | yes | none | Stable logical satellite name. Must be a non-empty string. Duplicate satellite names are errors. |
| `parent` | yes | none | Object reference to one existing hub or link declaration. |
| `parent.kind` | yes | none | Must be `hub` or `link`. |
| `parent.name` | yes | none | Existing declaration name of the referenced kind. |
| `payload` | yes | none | Non-empty ordered array of non-empty payload names. Duplicate payload names within one satellite are errors. |
| `drivingKeys` | no | `[]` | Ordered array of non-empty driving-key names. Duplicate driving keys are errors. Any overlap with `payload` is an error. |

Projection should map ordinary satellites to `DataVaultSatelliteMetadata`. `drivingKeys` maps to the existing multi-active driving-key metadata and preserves declaration order.

## PIT Declarations

PIT declarations model a point-in-time table over one hub and one or more satellites that belong to that hub.

```json
{
  "name": "CustomerPit",
  "hub": "Customer",
  "satellites": ["CustomerProfile", "CustomerContactByType"]
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `name` | yes | none | Stable logical PIT name. Must be a non-empty string. Duplicate PIT names are errors. |
| `hub` | yes | none | Existing hub name. |
| `satellites` | yes | none | Non-empty ordered array of existing satellite names. Duplicate satellite references are errors. |

Each referenced satellite must have `parent.kind` equal to `hub` and `parent.name` equal to the PIT `hub`. Link-parent satellites and satellites owned by a different hub are parent-mismatch errors. Projection should use existing PIT metadata where sufficient or add a narrow model-first PIT adapter if the existing public surface does not expose the exact required shape.

## Bridge Declarations

Bridge declarations model provider-neutral traversal metadata over one source link.

### Many-To-Many Bridge

```json
{
  "name": "CustomerOrderBridge",
  "kind": "many-to-many",
  "source": "CustomerOrder",
  "endpoints": {
    "from": {
      "hub": "Customer"
    },
    "to": {
      "hub": "Order"
    }
  }
}
```

### Hierarchy Bridge

```json
{
  "name": "SalesRegionHierarchyBridge",
  "kind": "hierarchy",
  "source": "SalesRegionParentChild",
  "endpoints": {
    "ancestor": {
      "hub": "SalesRegion",
      "role": "ParentRegion"
    },
    "descendant": {
      "hub": "SalesRegion",
      "role": "ChildRegion"
    }
  }
}
```

| Field | Required | Default | Contract |
| --- | --- | --- | --- |
| `name` | yes | none | Stable logical bridge name. Must be a non-empty string. Duplicate bridge names are errors. |
| `kind` | yes | none | Must be `many-to-many` or `hierarchy`. |
| `source` | yes | none | Existing link name. References to hubs or satellites are wrong-reference-kind errors. |
| `endpoints` | yes | none | Object whose required fields depend on `kind`. |
| `endpoints.from` | for `many-to-many` | none | Endpoint binding for the source side. |
| `endpoints.to` | for `many-to-many` | none | Endpoint binding for the target side. |
| `endpoints.ancestor` | for `hierarchy` | none | Endpoint binding for the ancestor side. |
| `endpoints.descendant` | for `hierarchy` | none | Endpoint binding for the descendant side. |
| `endpoints.*.hub` | yes | none | Hub name expected at the bound source-link participant. |
| `endpoints.*.role` | when needed | none | Source-link participant role. Required when the endpoint hub is ambiguous within the source link. |

Many-to-many bridges must bind exactly one source link and exactly two endpoint bindings named `from` and `to`. The endpoint hubs must resolve to source-link participants. If a hub appears more than once in the source link, an endpoint binding must include `role`.

Hierarchy bridges must bind exactly one two-participant self-link over one hub. Both source-link participants must declare distinct roles, and endpoint bindings must explicitly bind `ancestor` and `descendant` to those roles. Ambiguous or missing recursive role binding is a validation error.

Projection should align with the bridge metadata v1 contract: many-to-many bridge tables project endpoint hash-key columns, hierarchy bridge tables project ancestor and descendant hash-key columns plus traversal depth. Existing bridge metadata can be used where public APIs are sufficient; otherwise a narrow role-aware model-first adapter is permitted.

## Naming And Collision Policy

The default naming policy is used unless `naming.policy` is explicitly set to `default`, which produces the same result. V1 does not expose table, entity, column, index, constraint, or per-declaration naming overrides.

Validation must run collision checks after default normalization and before model application. At minimum, validators must detect collisions for produced tables or entities, produced technical columns within one table, produced business-key or payload columns within one table, produced PIT fields within one table, and produced indexes or constraints whose names would collide in the same model. Collisions are diagnostic failures rather than provider-specific behavior.

Implementations should reuse the visible default naming policy when possible. If a downstream projection path introduces a narrow model-first adapter, it must still preserve the same deterministic default naming behavior for v1.

## Load Timestamp Storage

`loadTimestampStorage` is the only provider-relevant v1 schema choice.

| Token | Mapping expectation |
| --- | --- |
| `provider-default` | Use the selected provider capability profile without changing load timestamp mappings. |
| `iso-8601-utc-text` | Use the provider capability profile transformed to ISO 8601 UTC text load timestamp and satellite snapshot reference mappings. |
| `utc-ticks` | Use the provider capability profile transformed to UTC tick load timestamp and satellite snapshot reference mappings. |

Unsupported values are provider-choice errors. The document must not contain arbitrary provider names, native store type names, provider options, SQL snippets, migration hints, or per-provider sections.

## Unknown Field Policy

Unknown fields are errors at every object level. This includes top-level objects, declaration objects, participant objects, parent reference objects, bridge endpoint objects, and nested `naming` objects. Validators must not ignore unknown fields because misspelled model-first artifacts would otherwise drift from intended metadata.

Externally converted authoring input is subject to the same unknown-field policy after conversion to JSON. YAML comments or YAML-only metadata that do not appear in the converted JSON artifact have no DVault model semantics.

## Diagnostic Contract

Diagnostics are structured and stable enough for parser and validation tests to assert categories and codes.

```json
{
  "severity": "error",
  "category": "reference",
  "code": "DMV1301",
  "message": "Link 'CustomerOrder' participant references missing hub 'Order'.",
  "path": "/links/0/participants/1/hub"
}
```

| Category | Code | Meaning |
| --- | --- | --- |
| `schema-version` | `DMV1001` | Missing, non-string, or blank `schemaVersion`. |
| `schema-version` | `DMV1002` | Unsupported `schemaVersion` value. |
| `shape` | `DMV1101` | Unknown field. |
| `shape` | `DMV1102` | Required field missing, null, wrong JSON type, or blank string. |
| `shape` | `DMV1103` | Required ordered collection is empty. |
| `duplicate` | `DMV1201` | Duplicate declaration name within the same declaration kind. |
| `duplicate` | `DMV1202` | Duplicate role, business key, payload, driving key, or other name within one declaration. |
| `duplicate` | `DMV1203` | Duplicate PIT satellite reference or duplicate bridge endpoint binding. |
| `reference` | `DMV1301` | Missing referenced declaration. |
| `reference` | `DMV1302` | Reference resolves to the wrong declaration kind. |
| `reference` | `DMV1303` | Referenced satellite parent does not match the PIT hub. |
| `naming` | `DMV1401` | Collision after default naming normalization. |
| `capability` | `DMV1501` | Valid schema asks for a model-first capability outside the v1 baseline. |
| `provider-choice` | `DMV1502` | Unsupported provider-relevant token or unsupported provider-specific field. |
| `recursive-participant-binding` | `DMV1601` | Ambiguous bridge endpoint binding or missing role for repeated hub participants. |
| `recursive-participant-binding` | `DMV1602` | Repeated hub link participant without the roles needed for disambiguation. |
| `shape` | `DMV1701` | Satellite driving key overlaps payload. |

Invalid documents return diagnostics without partial model application. When feasible, `path` is a JSON Pointer to the specific offending value. When a diagnostic covers a cross-document collision or missing reference, `path` should point to the declaration or reference that made the model invalid.

Validators may emit warnings only for future non-blocking compatibility guidance. Every invalid scenario listed in this contract is an `error`.

## Representative Valid Fixture Expectations

Downstream parser and projection work should materialize fixtures with these names or equivalent names that preserve the same scenarios.

### `valid/customer-hub-default.json`

Covers a customer hub with ordered business keys, a hub-parent satellite, default naming, and `provider-default` load timestamp storage.

```json
{
  "schemaVersion": "dvault.model.v1",
  "naming": {
    "policy": "default"
  },
  "loadTimestampStorage": "provider-default",
  "hubs": [
    {
      "name": "Customer",
      "businessKeys": ["TenantId", "CustomerNumber"]
    }
  ],
  "satellites": [
    {
      "name": "CustomerProfile",
      "parent": {
        "kind": "hub",
        "name": "Customer"
      },
      "payload": ["Name", "EmailAddress"]
    }
  ]
}
```

### `valid/customer-order-pit-bridge-iso-8601.json`

Covers a link with ordered participants, a multi-active satellite with ordered driving keys, a PIT declaration over hub satellites, a many-to-many bridge, and `iso-8601-utc-text` load timestamp storage.

```json
{
  "schemaVersion": "dvault.model.v1",
  "loadTimestampStorage": "iso-8601-utc-text",
  "hubs": [
    {
      "name": "Customer",
      "businessKeys": ["TenantId", "CustomerNumber"]
    },
    {
      "name": "Order",
      "businessKeys": ["OrderNumber"]
    }
  ],
  "links": [
    {
      "name": "CustomerOrder",
      "participants": [
        { "hub": "Customer" },
        { "hub": "Order" }
      ]
    }
  ],
  "satellites": [
    {
      "name": "CustomerProfile",
      "parent": {
        "kind": "hub",
        "name": "Customer"
      },
      "payload": ["Name", "EmailAddress"]
    },
    {
      "name": "CustomerContactByType",
      "parent": {
        "kind": "hub",
        "name": "Customer"
      },
      "drivingKeys": ["ContactType", "SourceSystem"],
      "payload": ["ContactValue", "VerifiedAt"]
    }
  ],
  "pits": [
    {
      "name": "CustomerPit",
      "hub": "Customer",
      "satellites": ["CustomerProfile", "CustomerContactByType"]
    }
  ],
  "bridges": [
    {
      "name": "CustomerOrderBridge",
      "kind": "many-to-many",
      "source": "CustomerOrder",
      "endpoints": {
        "from": {
          "hub": "Customer"
        },
        "to": {
          "hub": "Order"
        }
      }
    }
  ]
}
```

### `valid/sales-region-hierarchy-utc-ticks.json`

Covers a hierarchy bridge with role-bound recursive participants and `utc-ticks` load timestamp storage.

```json
{
  "schemaVersion": "dvault.model.v1",
  "loadTimestampStorage": "utc-ticks",
  "hubs": [
    {
      "name": "SalesRegion",
      "businessKeys": ["RegionCode"]
    }
  ],
  "links": [
    {
      "name": "SalesRegionParentChild",
      "participants": [
        {
          "hub": "SalesRegion",
          "role": "ParentRegion"
        },
        {
          "hub": "SalesRegion",
          "role": "ChildRegion"
        }
      ]
    }
  ],
  "bridges": [
    {
      "name": "SalesRegionHierarchyBridge",
      "kind": "hierarchy",
      "source": "SalesRegionParentChild",
      "endpoints": {
        "ancestor": {
          "hub": "SalesRegion",
          "role": "ParentRegion"
        },
        "descendant": {
          "hub": "SalesRegion",
          "role": "ChildRegion"
        }
      }
    }
  ]
}
```

## Representative Invalid Fixture Expectations

Downstream parser and diagnostics tests should assert the primary category and code for each invalid scenario. Implementations may return additional diagnostics when one malformed document has multiple independent errors, but the listed primary diagnostic must remain stable.

| Fixture name | Scenario | Primary category | Primary code |
| --- | --- | --- | --- |
| `invalid/missing-schema-version.json` | Top-level object omits `schemaVersion`. | `schema-version` | `DMV1001` |
| `invalid/unsupported-schema-version.json` | `schemaVersion` is `dvault.model.v2` or another unsupported value. | `schema-version` | `DMV1002` |
| `invalid/duplicate-hub-name.json` | Two hub declarations use the same `name`. | `duplicate` | `DMV1201` |
| `invalid/duplicate-participant-role.json` | One link repeats a participant `role`. | `duplicate` | `DMV1202` |
| `invalid/missing-link-hub-reference.json` | A link participant names a hub that is not declared. | `reference` | `DMV1301` |
| `invalid/wrong-parent-reference-kind.json` | A satellite parent declares `kind` that does not match the referenced declaration. | `reference` | `DMV1302` |
| `invalid/ambiguous-link-participant-binding.json` | A bridge endpoint over a repeated-hub link omits the role required to bind one participant. | `recursive-participant-binding` | `DMV1601` |
| `invalid/repeated-hub-link-without-roles.json` | A link repeats the same hub without participant roles. | `recursive-participant-binding` | `DMV1602` |
| `invalid/satellite-driving-key-payload-overlap.json` | A satellite lists the same name in `drivingKeys` and `payload`. | `shape` | `DMV1701` |
| `invalid/pit-satellite-parent-mismatch.json` | A PIT over `Customer` references an `Order` satellite. | `reference` | `DMV1303` |
| `invalid/invalid-bridge-endpoint.json` | A bridge endpoint does not resolve to the source link participants. | `reference` | `DMV1301` |
| `invalid/naming-collision-after-normalization.json` | Two accepted declarations normalize to the same produced table, column, index, or constraint name. | `naming` | `DMV1401` |
| `invalid/unknown-top-level-field.json` | The document includes an unknown top-level field. | `shape` | `DMV1101` |
| `invalid/unsupported-provider-field.json` | The document includes a provider-specific section such as `providers` or native store-type options. | `provider-choice` | `DMV1502` |
| `invalid/unsupported-load-timestamp-storage.json` | `loadTimestampStorage` uses a token outside the supported set. | `provider-choice` | `DMV1502` |

No YAML fixture family is required for v1. If downstream tests cover pre-conversion authoring examples, they should assert only the resulting JSON artifact and the ordinary JSON diagnostics listed above.

## Mapping Summary

| Model-first declaration | Existing visible semantics | Additive surface allowed when needed |
| --- | --- | --- |
| Hub | `DataVaultHubMetadata`, ordered business keys, hash key, load timestamp, record source. | Narrow DTO or adapter for parser-owned document projection. |
| Link | `DataVaultLinkMetadata` for ordinary distinct-hub links. | Role-aware participant metadata for repeated same-hub or role-bound links. |
| Satellite | `DataVaultSatelliteMetadata`, payload metadata, multi-active driving keys. | Parser DTOs that preserve ordered payload and driving-key names before registry projection. |
| PIT | Existing PIT and point-in-time metadata concepts. | Model-first PIT adapter when exact hub-satellite contract needs a narrower projection type. |
| Bridge | Existing bridge metadata concepts and bridge planning contract. | Role-aware hierarchy endpoint adapter where current public APIs do not expose recursive role binding. |
| Naming | Existing default naming policy. | Collision audit helpers that run before provider projection. |
| Load timestamp storage | `DataVaultLoadTimestampStorage` and provider capability profile transformations. | Parser token mapper from JSON strings to the enum. |

## Completion Boundary

This contract is complete when parser and projection implementers can build against the token registry, declaration shapes, diagnostic taxonomy, fixture expectations, and JSON-first YAML authoring boundary above without reopening top-level field names, compatibility policy, provider-choice policy, direct YAML ingestion, or recursive participant binding rules.

# Multi-Active Satellite Driving-Key Contract

Status: v1 shared contract
Tickets: 06EZ0NVX3RYPTFZKYCYEH9HB8W, 06EZ0NW61GFJN90PSB5N934G2G

## Purpose

This artifact finalizes the opt-in public contract for multi-active satellite driving keys so the persistence ticket can implement it without inventing caller-visible behavior.

## Modeling Contract

- Ordinary satellites remain the default.
- A satellite becomes multi-active only when one or more driving keys are declared.
- `DataVaultSatelliteBuilder.DrivingKey(string propertyName)` is the modeling opt-in. Each call appends one logical driving-key name in canonical declaration order.
- `DataVaultSatelliteMetadata` keeps the current constructor for ordinary satellites and adds `DataVaultSatelliteMetadata(string name, DataVaultMetadataReference parent, IEnumerable<string> descriptiveAttributeNames, IEnumerable<string> drivingKeyNames)` for opt-in multi-active satellites.
- `DataVaultSatelliteMetadata.DrivingKeyNames` exposes the declared logical driving-key names in canonical order. Ordinary satellites expose an empty list.

## Save Contract

- `DataVaultSatelliteSaveOperation` keeps the current constructor for ordinary satellites and adds `DataVaultSatelliteSaveOperation(DataVaultSatelliteMetadata metadata, string parentHashKey, IEnumerable<KeyValuePair<string, string>> drivingKeyValues, IEnumerable<KeyValuePair<string, string>> payloadValues, string hashDiff)` for opt-in multi-active saves.
- `DataVaultSatelliteSaveOperation.DrivingKeyValues` exposes the caller-supplied driving-key values keyed by logical driving-key name. Ordinary satellites expose an empty dictionary.
- Driving-key values remain separate from `payloadValues` and `hashDiff`.
- `hashDiff` continues to describe payload state only, not driving-key identity.

## Validation Rules

- A multi-active opt-in declaration requires at least one driving-key name.
- Driving-key names must be non-empty, unique by `StringComparer.Ordinal`, and must not overlap the satellite payload names by `StringComparer.Ordinal`.
- For a multi-active save, `drivingKeyValues` must contain exactly one value for each declared driving-key name and no extra names.
- Missing names, duplicate names, or `null` values are rejected.
- Empty-string values remain allowed under the same string-value rules as other explicit save dictionaries.
- For an ordinary satellite, the driving-key name list and driving-key value set are both empty.

## Deterministic Ordering Rules

- The canonical multi-column driving-key order is the declaration order from `DrivingKey(...)` or from `drivingKeyNames` in the metadata constructor.
- `drivingKeyValues` are matched by logical name, then projected, validated, indexed, and compared in that canonical order. Caller enumeration order does not change the canonical order.
- For opt-in multi-active satellites, the projected schema stores the driving-key columns immediately after the parent hash-key column and before `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- The persistence uniqueness and latest-state partition is `(parentHashKey, drivingKeyValue1, drivingKeyValue2, ..., drivingKeyValueN)`.
- `hashDiff` remains the change detector inside one ordered series.
- The opt-in satellite primary-key and index expansion for this capability is `(parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp)` so same-parent same-load-timestamp rows can coexist when their driving-key values differ.

## Acceptance Example

Use one customer contact satellite with payload `EmailAddress`, driving keys `ContactType` then `RegionCode`, and parent hash key `customer-hash`.

- Modeling opt-in: `satellite.DrivingKey("ContactType").DrivingKey("RegionCode").Payload("EmailAddress")`
- Save values: `drivingKeyValues = [("RegionCode", "DE"), ("ContactType", "billing")]` is valid because name matching reorders them to the canonical `ContactType`, `RegionCode` tuple before persistence and duplicate checks.
- Projected row identity and order: `[CustomerHashKey, ContactType, RegionCode, HashDiff, LoadTimestamp, RecordSource, EmailAddress]`
- Replay suppression compares the latest row for `(customer-hash, billing, DE)` only.
- A same-timestamp row for `(customer-hash, shipping, DE)` can coexist.
- A later changed hash diff for `(customer-hash, billing, DE)` inserts a new history row.

## Non-Goals

- This contract does not define same-series same-load-timestamp changed-row conflict resolution; that remains follow-up work.
- Provider-specific optimized strategies may either implement the same partitioning rules or decline multi-active batches so the provider-neutral writer handles them.

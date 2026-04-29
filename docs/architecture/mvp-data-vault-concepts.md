# MVP Data Vault Persistence Concepts

This document defines the MVP Data Vault 2.x persistence concepts for DVault architecture work. It is guidance for the first SQLite-focused persistence tests and does not claim that schema generation, loading automation, hash computation, migrations, or validation tooling already exist.

The MVP concept set is limited to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.

## Concept Model

Data Vault structures separate business identity, relationships, and descriptive history:

- Hubs store stable business identities.
- Links connect two or more hubs into a relationship.
- Satellites store descriptive or contextual attributes for a hub or link over time.

Every inserted vault record in the MVP model carries a load timestamp and record source. Hash keys and hash diffs are planned persistence conventions used to identify business entities, relationships, and satellite changes. The exact hash algorithm, input normalization, and generated naming conventions are future implementation decisions.

## In-Scope Concepts

### Hub

A hub represents one business concept that can be identified by a business key, such as a customer number or order number. The hub keeps the business key and metadata needed to trace when and where the identity first entered the vault.

Minimum MVP behavior:

- Each hub row has one hash key column used as the stable persistence identifier.
- Each hub row stores the source business key value or values in portable SQLite-friendly columns.
- Each hub row stores a load timestamp and record source.
- Descriptive attributes do not belong in the hub; they belong in satellites.

### Link

A link represents a relationship between hubs, such as a customer placing an order. A link stores the participating hub hash keys and metadata for the relationship record.

Minimum MVP behavior:

- Each link row has one hash key column used as the stable persistence identifier for the relationship.
- Each link row stores the hash keys of the participating hubs.
- Each link row stores a load timestamp and record source.
- Relationship descriptive attributes, if any, belong in a satellite attached to the link.

### Satellite

A satellite stores descriptive or contextual attributes for a parent hub or link. Satellites allow the vault to retain history as source values change over time.

Minimum MVP behavior:

- Each satellite row references its parent hub or link hash key.
- Each satellite row stores the descriptive payload columns for one point-in-time view of the parent.
- Each satellite row stores a hash diff, load timestamp, and record source.
- The parent hash key plus load timestamp is enough for initial SQLite examples to distinguish historical rows for the same parent.

### Hash Key

A hash key is the planned persistence convention for a stable technical key derived from the business key of a hub or the participating keys of a link. In the MVP documentation and SQLite examples, hash key values are represented as deterministic text placeholders.

The MVP does not prescribe the hash algorithm, casing, delimiter, null handling, or normalization rules. Those choices remain future implementation work.

### Hash Diff

A hash diff is the planned persistence convention for detecting changes in a satellite payload. It represents the payload state that should be compared for a parent record over time.

The MVP does not prescribe the hash diff algorithm or payload normalization rules. SQLite-focused tests can use explicit text values for hash diffs until hash computation is implemented.

### Load Timestamp

A load timestamp records when the vault row was accepted into the persistence model. The MVP treats it as required metadata on hub, link, and satellite rows.

SQLite examples represent load timestamps as ISO 8601 text values, such as `2026-04-29T10:15:00Z`, to stay portable and easy to assert in tests.

### Record Source

A record source identifies the source system, file, stream, or ingest path that supplied the row. The MVP treats it as required lineage metadata on hub, link, and satellite rows.

Record source values should be plain text that can be asserted in SQLite tests, such as `crm-import` or `orders-import`.

## SQLite-Oriented Examples

The following table shapes are illustrative only. They are intentionally small, portable, and not final naming standards.

```sql
CREATE TABLE hub_customer (
  customer_hk TEXT NOT NULL PRIMARY KEY,
  customer_business_key TEXT NOT NULL,
  load_ts TEXT NOT NULL,
  record_source TEXT NOT NULL
);

CREATE TABLE hub_order (
  order_hk TEXT NOT NULL PRIMARY KEY,
  order_business_key TEXT NOT NULL,
  load_ts TEXT NOT NULL,
  record_source TEXT NOT NULL
);

CREATE TABLE link_customer_order (
  customer_order_lk TEXT NOT NULL PRIMARY KEY,
  customer_hk TEXT NOT NULL,
  order_hk TEXT NOT NULL,
  load_ts TEXT NOT NULL,
  record_source TEXT NOT NULL
);

CREATE TABLE sat_customer_profile (
  customer_hk TEXT NOT NULL,
  load_ts TEXT NOT NULL,
  record_source TEXT NOT NULL,
  hash_diff TEXT NOT NULL,
  customer_name TEXT,
  customer_status TEXT,
  PRIMARY KEY (customer_hk, load_ts)
);
```

Example rows for initial SQLite tests can use literal text values:

| Table | Key Values | Payload | Metadata |
| --- | --- | --- | --- |
| `hub_customer` | `customer_hk = 'hk_customer_001'`, `customer_business_key = 'CUST-001'` | none | `load_ts = '2026-04-29T10:15:00Z'`, `record_source = 'crm-import'` |
| `hub_order` | `order_hk = 'hk_order_1001'`, `order_business_key = 'ORD-1001'` | none | `load_ts = '2026-04-29T10:16:00Z'`, `record_source = 'orders-import'` |
| `link_customer_order` | `customer_order_lk = 'lk_customer_001_order_1001'`, `customer_hk = 'hk_customer_001'`, `order_hk = 'hk_order_1001'` | none | `load_ts = '2026-04-29T10:17:00Z'`, `record_source = 'orders-import'` |
| `sat_customer_profile` | `customer_hk = 'hk_customer_001'` | `hash_diff = 'diff_customer_001_active'`, `customer_name = 'Ada Lake'`, `customer_status = 'active'` | `load_ts = '2026-04-29T10:18:00Z'`, `record_source = 'crm-import'` |

These examples avoid database-specific generated columns, computed hashes, sequences, triggers, or migration features. They are meant to support early SQLite assertions about concept coverage, required metadata columns, and the relationship between hubs, links, and satellites.

## MVP Guidance Versus Future Work

MVP guidance:

- Use hubs for business identity, links for relationships, and satellites for descriptive history.
- Treat load timestamp and record source as required metadata for inserted vault rows.
- Represent hash keys and hash diffs as persistence conventions with literal text values in early SQLite tests.
- Keep examples conceptual until source and test roots exist.

Future implementation work:

- Select hash algorithms and input normalization rules for hash keys and hash diffs.
- Define generated table and column naming conventions.
- Implement schema generation, loading automation, migrations, validation tooling, or database dialect support beyond the SQLite-oriented baseline.

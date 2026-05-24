Goal: Evaluate database-side hashing as a future provider-gated escape hatch without implementing it as a default path.

Acceptance criteria:
- Documents why .NET-side canonical hashing remains the default compatibility contract.
- Defines evidence required before any provider may offer database-side hashing.
- Avoids adding runtime database-side hashing behavior in this ticket.
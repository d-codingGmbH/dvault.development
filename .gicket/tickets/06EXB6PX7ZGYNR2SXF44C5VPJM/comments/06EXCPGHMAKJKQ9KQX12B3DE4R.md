[gicket-bot] PO refinement contract

Summary
- Refined the documentation task into a bounded MVP concept document for Data Vault 2.x persistence concepts, with SQLite-oriented examples and no implementation or automation promises.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This is a documentation-only task in the Foundation and architecture milestone context.
- The v1 document should cover the named MVP concepts only: hub, link, satellite, hash key, hash diff, load timestamp, and record source.
- SQLite is ratified as the example baseline because the existing ticket acceptance criteria already require examples to align with planned Sqlite tests.
- No repository source or test roots exist yet, so examples should stay conceptual and SQL-oriented rather than referencing concrete code APIs.

Scope In
- Create or update a concise MVP architecture/concept document for Data Vault 2.x-oriented persistence.
- Define the purpose and minimum expected behavior of hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Include small illustrative examples that are compatible with planned SQLite validation, such as simple table-shape or row examples.
- Explain which parts are conceptual MVP guidance versus future implementation work.

Scope Out
- Implementing persistence code, schema generators, migrations, or runtime automation.
- Creating or executing SQLite tests.
- Defining full enterprise Data Vault standards beyond the MVP concepts named in this ticket.
- Choosing final public API names, method names, or internal helper class names.

Open questions
- none

Follow-up questions
- Should later implementation tickets standardize the exact hash algorithm and normalization rules for hash keys and hash diffs?
- Should future architecture work define naming conventions for generated hub, link, and satellite tables once code generation or migrations are in scope?
- Should non-SQLite database dialect guidance be added after the MVP SQLite validation path is established?

Risks
- Because no source or test roots exist yet, examples may need minor adjustment when the first concrete test layout is introduced.
- If hash algorithm details are documented too specifically before implementation, future code may be constrained prematurely.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment
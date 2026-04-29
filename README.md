# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Layout

- `DVault.slnx`: Root solution file. It is intentionally projectless until sibling tickets add project files.
- `src/`: Source projects. The first library project is reserved for `src/DCoding.Data.DVault/`. `src/DCoding.Data/` is a tracked placeholder for the initial source scaffold.
- `tests/`: Test projects. Unit and integration projects are reserved for `tests/DCoding.Data.DVault.Tests/` and `tests/DCoding.Data.DVault.IntegrationTests/`. `tests/DCoding.Data.DVault/` is a tracked placeholder for the initial test scaffold.
- `examples/`: Future runnable examples for DVault APIs.
- `benchmarks/`: Future performance benchmark projects.
- `docs/`: Documentation and design notes.

Empty scaffold folders contain `.gitkeep` files so the layout is present in clean checkouts. Project files should be added to `DVault.slnx` when those projects are created.

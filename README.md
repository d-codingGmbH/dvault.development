# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `tests/DCoding.Data.DVault.Tests/`: Unit, integration, and shared test projects for DVault.
- `examples/`: Future runnable examples for DVault APIs.
- `benchmarks/`: Future performance benchmark projects.
- `docs/`: Documentation and design notes.

All current .NET projects are included in `DVault.slnx`. Empty future-use folders contain `.gitkeep` files so the layout is present in clean checkouts.

## Local Validation

```sh
dotnet build
dotnet test
dotnet pack src/DCoding.Data.DVault/DCoding.Data.DVault.csproj --configuration Release --nologo
bash tools/check-format.sh
```

## License

DVault uses the Apache License 2.0. See `LICENSE`.

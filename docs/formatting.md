# Formatting Enforcement

DVault uses a repository-level formatting gate before an application stack or CI workflow exists. The root `DVault.slnx` is the repository-level .NET entry point so `dotnet build` and `dotnet test` automation have a stable target as projects are added.

## Canonical Policy

The root `.editorconfig` is the editor-facing formatting source for governed text files:

- two-space indentation with spaces by default
- LF line endings
- UTF-8 without BOM
- a final newline for non-empty files
- trailing whitespace trimming
- tab rejection except for Makefile syntax
- same-line opening braces for brace-based source files

The root `.gitattributes` normalizes governed text files to LF on checkout so the shell-based gate can run consistently on developer machines and CI runners. Future source, test, documentation, configuration, and workflow files inherit these defaults from the repository root.

## Automated Check

The non-mutating formatting check is the shared local and CI gate:

```sh
bash tools/check-format.sh
```

The command fails when governed text files contain invalid UTF-8, UTF-8 BOMs, CRLF line endings, trailing whitespace, missing final newlines, or tabs outside documented tab exceptions. It also fails if `.editorconfig` or `.gitattributes` no longer contain the required repository formatting rules, including LF normalization and same-line brace policy entries.

## Local Command

Developers should run the shared gate before committing:

```sh
bash tools/check-format.sh
```

The command reports every detected violation and exits non-zero without rewriting files.

## CI And Build Gate

The first CI workflow or application build definition added to the repository must call the same check as a blocking step:

```sh
bash tools/check-format.sh
```

Language-specific formatters introduced later must either read `.editorconfig` directly or be invoked from this same gate with equivalent results.

## Brace-Based Source Files

C# and C# script files are configured with `csharp_new_line_before_open_brace = none` and `dotnet_diagnostic.IDE0055.severity = error` so dotnet formatting can fail brace drift once C# projects exist. Common brace-based source file extensions are also marked with `brace_style = 1tbs` as the repository-level policy marker until the relevant language stack adds a formatter-specific configuration.

Future formatter integrations such as dotnet format, Prettier, clang-format, or another stack-specific checker must preserve same-line opening braces through formatter or checker configuration in the same `bash tools/check-format.sh` gate. Manual review is not an accepted enforcement mechanism for brace placement.

## Exceptions

The check intentionally excludes repository operational metadata, generated output, vendor and third-party trees, binary assets, build outputs, coverage output, dependency folders, and lock files. The same categories are documented in `.gitattributes` so line-ending normalization and the formatting scan have matching boundaries.

Makefiles and `*.mk` files are the only default tab exception because recipe lines require tabs. The script rejects tabs in every other governed text file with an explicit failure message. Any future generated or vendor directory must be added as an explicit exclusion before broad scans include it.

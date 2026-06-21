## Dev implementation handoff

Implemented the adopter-owned binary hash-key storage migration guide and linked it from the current adoption documentation path.

Repository artifacts written:

- `docs/hash-key-storage-migration.md`
- `README.md`
- `docs/getting-started.md`
- `docs/production-adoption-checklist.md`
- `hash-key-footprint.md`

The guide documents `Binary` as explicit opt-in physical storage, `HexString` as the compatible default, and lowercase hexadecimal strings as the public/request/diagnostic boundary. It defines support-bundle or translated metadata preflight checks for storage profile, stable-hash algorithm id, digest byte length, provider store type, provider value format, and conversion behavior; it keeps algorithm changes separate from storage-profile migration; it includes fail-closed handling, execution and rollback checkpoints, provider caveats, SQLite-only footprint evidence scope, and the `sha1-v1` versus `sha256-160-v1` same-width incompatibility example.

Verification:

- `timeout 30s git diff --check -- README.md docs/getting-started.md docs/production-adoption-checklist.md docs/hash-key-storage-migration.md hash-key-footprint.md` exited `0` for tracked edited paths.
- `rg -n "[[:blank:]]$" docs/hash-key-storage-migration.md` exited `1`, meaning no trailing whitespace matches were found in the new guide.
- `timeout 30s git status --short -- README.md docs/getting-started.md docs/production-adoption-checklist.md docs/hash-key-storage-migration.md hash-key-footprint.md` showed the intended four modified docs plus the new guide.
- `bash tools/check-format.sh` was attempted but produced no output for roughly two minutes and was interrupted to avoid leaving a stalled process.
- `timeout 300s dotnet build DVault.slnx --nologo` restored and partially compiled, then exited `124` on timeout. Output included `NU1900` warnings from a read-only NuGet vulnerability-cache path under `/home/davidullrich/.local/share/NuGet/http-cache/...`.
- `timeout 300s dotnet test DVault.slnx --nologo` restored, partially compiled, then exited `124` on timeout. As the timeout terminated MSBuild, it also reported `MSB4166` child-node shutdown diagnostics under `/tmp/MSBuildTempiHZOvL/`.
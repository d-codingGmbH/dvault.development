Post-cleanup status note after commit `ae81a4e`:

The technical metadata column contracts remain in the package namespace `DCoding.Data.DVault` after the project-layout cleanup. The implementation path is now `src/DCoding.Data.DVault/`, focused tests live under `tests/DCoding.Data.DVault.Tests/`, and the contract documentation was updated to reference `DVault.slnx` as the build/test entry point.
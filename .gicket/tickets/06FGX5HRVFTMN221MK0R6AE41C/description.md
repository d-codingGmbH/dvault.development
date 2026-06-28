Implement the package/project changes selected by the analyzer compatibility audit.

Acceptance:
- The analyzer package contains the supported analyzer asset(s) for the 8.50.0 and 10.50.0 package lines.
- Analyzer references remain local build-time references with PrivateAssets guidance and no runtime dependency leak.
- Existing analyzer and source-generator tests still pass.
- Any intentional no-go outcome updates the code/package surface minimally and records the reason in docs instead of making unsupported claims.
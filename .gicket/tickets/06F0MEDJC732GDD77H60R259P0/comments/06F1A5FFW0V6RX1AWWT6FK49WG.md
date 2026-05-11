﻿[gicket-bot] human MinVer release-version clarification

Summary
- DVault package versions are derived by MinVer from Git tags.
- The final `0.6.0` NuGet artifact version is expected only from the tagged `v0.6.0` release checkout.
- This ticket must not require `artifacts/packages` to contain final `0.6.0` package filenames before the release tag exists.

Clarification
- README.md and docs/releases/v0.6.0.md intentionally document the intended released consumer install guidance for `0.6.0`.
- Pre-tag package validation may legitimately produce MinVer prerelease artifact versions derived from the current tag history.
- A pre-tag validation pass is acceptable when the package verifier confirms the six-package matrix, matching symbols, README metadata, XML docs, and provider dependency alignment against the packed core package version.
- Final audited `0.6.0` package validation remains part of the manual publication checklist after the release tag is created.

Implementation direction
- Align PackageVerifier and PackageVerifierTests with the updated README install guidance.
- Do not treat stale `0.5.1-alpha` package filenames as proof that the documentation is wrong; treat them only as non-final pre-tag artifacts.
- Do not require the bot to create the release tag or publish packages for this ticket.
﻿[gicket-bot] runtime-orchestration template

- template: `handover-test`
- transaction-point: `TP3`
- ticket-id: `06EXB6XBV95E08R2W9ZQ1PRDPM`
- target-role: `test`
- branch: `ticket/06EXB6XBV95E08R2W9ZQ1PRDPM-story-create-net-10-solution-skeleton-using-slnx`
- commit: `bdaa0314f4f2`
- test-hint: Re-run tester verification after local policy repair. The configured tester commands now include `dotnet build DVault.slnx --nologo`, `bash tools/check-format.sh`, `dotnet build --nologo`, and `dotnet test --nologo`; `git grep*` is also allowed for read-only namespace inspection.
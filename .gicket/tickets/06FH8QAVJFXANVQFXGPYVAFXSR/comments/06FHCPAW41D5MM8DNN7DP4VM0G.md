[gicket-bot] runtime-orchestration template

- template: `handover-test`
- transaction-point: `TP3`
- ticket-id: `06FH8QAVJFXANVQFXGPYVAFXSR`
- target-role: `test`
- branch: `ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp`
- test-hint: This is a parent story closure. Verify the authoritative delivery contract and child integration evidence; do not require additional code changes in the parent branch. Re-run `bash tools/pack-release-packages.sh`, `bash tools/verify-packages.sh`, `bash tools/run-analyzer-package-smoke.sh 8`, and `bash tools/run-analyzer-package-smoke.sh 10` if fresh local proof is needed.
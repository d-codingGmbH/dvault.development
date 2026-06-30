[gicket-bot] transactional writeback failure

- reason: `push-failed`
- ticket-id: `06FH8QAVJFXANVQFXGPYVAFXSR`
- transaction-point: `TP10`
- write-group: `wg-return-dev`
- source-role: `test`
- target-role: `dev`
- error: Ticket writeback push failed after commit `8780f858d0da`: Command `git push --set-upstream --force-with-lease=refs/heads/ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp:12cb645340c9b3b997ae4ef5a203d0de9dd83056 origin ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp` failed with exit code 1: error: RPC failed; HTTP 401 curl 22 The requested URL returned error: 401. Branch-ref diagnostics for `origin/ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp`: local head `8780f858d0da998655259efc47eb7d6ed42a4583`, expected remote head `12cb645340c9b3b997ae4ef5a203d0de9dd83056`, actual remote head `<missing>`, expected-ref push `attempted`, rejection `push-failed`.

Next steps
- Manually push branch `ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp` and investigate remote policy.

<!-- gicket-semantic-idempotency-key: bot-writeback:06fh8qavjfxanvqfxgpyvafxsr:residual:writeback:post-workflow:tp10:wg-return-dev:test:dev:fbe0668424242c61:aux:audit-only:writeback-failure-comment:gicket-writeback-failure-reason-push-failed -->
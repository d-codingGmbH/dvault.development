Human cleanup note:

- Removed stale incoming `blocks` relation `06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks` in this ticket branch.
- Fresh relation read for `06F5Q93R4633D41Z21WQW3SVGR` now shows `incomingCount=0` and the five existing `parentOf` children only.
- Removed stale stop labels `blocked/dev` and `blocked/test`; kept `needs-po` so the bot can rerun PO against the cleaned live graph and update the durable contract normally.
- All five v0.23.0 child tickets are currently `done` in this branch.
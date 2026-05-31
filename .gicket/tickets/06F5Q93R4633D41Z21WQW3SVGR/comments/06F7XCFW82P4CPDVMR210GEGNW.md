Human metadata cleanup

The epic branch relation graph and child-ticket evidence are already clean, but the latest persisted labels still blocked every bot role. I removed stale labels `blocked/dev`, `blocked/test`, and `blocked/po`, replaced `needs-po` with `critic-needed`, and refreshed the durable contract to `ready_for_po_critic`.

Verification evidence before cleanup:
- `gicket ticket eligibility --id 06F5Q93R4633D41Z21WQW3SVGR --format json`: `relationReady=true`, no skip reasons.
- `gicket relation list 06F5Q93R4633D41Z21WQW3SVGR --format json`: `incomingCount=0`, five outgoing `parentOf` relations.
- All five v0.23.0 child tickets are `done`.
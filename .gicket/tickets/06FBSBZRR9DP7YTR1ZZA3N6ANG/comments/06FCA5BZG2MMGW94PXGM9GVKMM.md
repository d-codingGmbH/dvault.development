[gicket-maintenance] base scheduler state repaired

The active parent ticket branch already carries the tracking-parent correction, but `develop` still contained stale parent-to-child `blocks` relations. That left the executable child tickets blocked in the base scheduler inventory.

Applied to `develop`:
- removed parent-to-child `blocks` from this parent to `06FBSBZY1XEJYK1DRV4RV2ZN88`, `06FBSC03KAGDABNFGPK9D95QKR`, and `06FBSC08W24BJGFZ87RSFS21WC`;
- removed `needs-po` from the parent and marked it as `tracking/parent` plus `tracking/waiting-on-children`.

Expected result: the parent stays out of active role queues, and the first executable child tickets can be claimed normally.
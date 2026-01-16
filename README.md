#JSON Diff Lists Extension


This extension provides a powerful and flexible server-side action to compare two JSON arrays and detect differences. It identifies added, removed, changed, and optionally unchanged items, making it ideal for data synchronization, auditing, or reporting scenarios.

Key Features:

Compares two JSON arrays based on a configurable key field.

Detects Added, Removed, and Changed items automatically.

Returns Unchanged items optionally.

Allows ignoring specific fields from change detection.

Supports field renaming via a replace matrix.

Optionally humanizes field names by converting camelCase or PascalCase into readable strings.

Fully case-sensitive comparison.

Returns results as a structured JSON array, ready to use in your OutSystems logic.

Use Cases:

Data migration validation

Audit and logging of object changes

UI display of differences in a user-friendly way

Backend synchronization between systems

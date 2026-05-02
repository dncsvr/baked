# Unreleased

# Breaking Changes

- `CodeGeneration` namespace is changed to `Builtime`, along with its layer name
  and extensions
  - `configurator.CodeGeneration` is now `configurator.Buildtime`
  - `CodeGenerationLayer` is now `BuildtimeLayer`
- `FormPage` schema is completely redesigned, migrate your existing
  configurations to match the new one
- `configurator.Domain.ConfigureInspect` is now removed, `Inspect` instance can
  now be accessed directly from `builder.Inspect` within
  `ConfigureDomainModelBuilder(builder => ...)`
- `TabNameAttribute` is now removed, use `GroupAttribute` instead

## Improvements

- Inspection mechanism
  - It was not tracing changes from parent components, fixed
  - Add trace wasn't showing up when initial value is null, fixed
  - JSON serialization is restricted to only anonymous types to avoid
    unnecessarily long (and for some attributes failing) serializations

namespace Baked.Buildtime.Diagnostics;

public record DiagnosticsResult(
    IReadOnlyCollection<Exception> Errors,
    IReadOnlyCollection<DiagnosticMessage> Messages
);
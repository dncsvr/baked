namespace Baked.Runtime.Diagnostics;

public record DiagnosticsResult(
    IReadOnlyCollection<Exception> Errors,
    IReadOnlyCollection<DiagnosticMessage> Messages
);
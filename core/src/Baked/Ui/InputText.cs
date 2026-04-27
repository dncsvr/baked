namespace Baked.Ui;

public record InputText(string Label)
    : ILabeler, IComponentSchema
{
    public string Label { get; set; } = Label;
    public string? TargetProp { get; set; }
    public string? LabelMode { get; set; }
    public string? LabelVariant { get; set; }
}
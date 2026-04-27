namespace Baked.Ui;

public record InputNumber(string Label)
    : ILabeler, IComponentSchema
{
    public string Label { get; set; } = Label;
    public string? LabelMode { get; set; }
    public string? LabelVariant { get; set; }
}
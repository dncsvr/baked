namespace Baked.Ui;

public record InputNumber : ILabeler, IComponentSchema
{
    public string? Label { get; set; }
    public string? LabelMode { get; set; }
    public string? LabelVariant { get; set; }
}
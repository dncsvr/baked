namespace Baked.Ui;

public record InputText : IComponentSchema, IHasLabel
{
    public string? Label { get; set; }
    public string? TargetProp { get; set; }
    public string? LabelMode { get; set; }
    public string? LabelVariant { get; set; }
}
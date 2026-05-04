namespace Baked.Ui;

public interface IHasLabel : IComponentSchema
{
    string? Label { get; set; }
    string? LabelMode { get; set; }
    string? LabelVariant { get; set; }

}
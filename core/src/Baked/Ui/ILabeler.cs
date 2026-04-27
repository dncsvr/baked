namespace Baked.Ui;

// TODO: Label mode and variants must be enum 🧐
public interface ILabeler : IComponentSchema
{
    string? LabelMode { get; set; }
    string? LabelVariant { get; set; }
}
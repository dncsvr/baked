namespace Baked.Ui;

public record InputNumber(string Label)
    : IComponentSchema
{
    public string Label { get; set; } = Label;
    public bool? NoGrouping { get; set; }
}
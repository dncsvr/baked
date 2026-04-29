namespace Baked.Ui;

// TODO Datapanel aware olsun, style wise
public record DataContainer(IComponentDescriptor Content)
    : IComponentSchema
{
    public List<Input> Inputs { get; init; } = [];
    public IComponentDescriptor Content { get; set; } = Content;
}
﻿using Do.Domain.Model;

namespace Do.RestApi.Model;

public record ControllerModel(TypeModel TypeModel)
{
    public string Id { get; } = TypeModel.CSharpFriendlyFullName;
    public string ClassName { get; } = TypeModel.Name;
    public string GroupName { get; set; } = TypeModel.Name;
    public Dictionary<string, ActionModel> Action { get; init; } = [];

    public IEnumerable<ActionModel> Actions { get => Action.Values.OrderBy(a => a.Order); init => Action = value.ToDictionary(a => a.Id); }
}
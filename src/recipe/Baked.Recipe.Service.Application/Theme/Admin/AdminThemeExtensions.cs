﻿using Baked.Theme;
using Baked.Theme.Admin;

namespace Baked;

public static class AdminThemeExtensions
{
    public static AdminThemeFeature Admin(this ThemeConfigurator _,
        IEnumerable<string>? componentExports = default
    ) => new([.. componentExports ?? []]);
}
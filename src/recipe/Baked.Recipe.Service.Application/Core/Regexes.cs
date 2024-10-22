﻿using System.Text.RegularExpressions;

namespace Baked.Core;

internal static partial class Regexes
{
    [GeneratedRegex(@"[\s\S]*?(?=.Application|$)")]
    public static partial Regex AssemblyNameBeforeApplicationSuffix();
}
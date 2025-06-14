﻿using Baked.Architecture;
using Baked.Core;
using Baked.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Baked;

public static class CoreExtensions
{
    public static void AddCore(this List<IFeature> features, Func<CoreConfigurator, IFeature<CoreConfigurator>> configure) =>
        features.Add(configure(new()));

    public static Dictionary<string, string> Merge(this Dictionary<string, string>? dictionary, Dictionary<string, string>? input)
    {
        dictionary ??= [];
        input ??= [];

        foreach (var (key, value) in input)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
        }

        return dictionary;
    }

    public static int AnInteger(this Stubber _) =>
        42;

    public static string AnEmail(this Stubber _) =>
        "info@test.com";

    public static string AString(this Stubber _,
        string? value = default
    ) => value ?? "test string";

    public static Guid ToGuid(this string guidStr) =>
        Guid.Parse(guidStr);

    public static DateTime ADateTime(this Stubber _,
        int year = 2023,
        int month = 9,
        int day = 17,
        int hour = 13,
        int minute = 29,
        int second = 00
    ) => new(year, month, day, hour, minute, second);

    public static Dictionary<string, string> ADictionary(this Stubber giveMe) => giveMe.ADictionary<string, string>();
    public static Dictionary<TKey, TValue> ADictionary<TKey, TValue>(this Stubber _, params IEnumerable<(TKey, TValue)> pairs)
        where TKey : notnull
    => pairs.ToDictionary(pair => pair.Item1, pair => pair.Item2);

    public static Guid AGuid(this Stubber _,
        string? starts = default
    )
    {
        starts ??= string.Empty;

        const string template = "4d13bbe0-07a4-4b64-9d31-8fef958fbef1";

        return Guid.Parse($"{starts}{template[starts.Length..]}");
    }

    public static Uri AUrl(this Stubber giveMe,
        string? url = default
    )
    {
        url ??= $"https://www.{Regex.Replace(giveMe.AGuid().ToString(), "[0-9]", "x")}.com";

        return new(url);
    }

    public static void ShouldBe(this string? @string, object? expected, string format) =>
        @string.ShouldBe(string.Format(format, expected));

    public static void ShouldBe(this Uri? uri, string urlString) =>
        uri?.ToString().ShouldBe(urlString);

    public static void ShouldContainKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        params TKey[] keys
    ) where TKey : notnull
    {
        foreach (var key in keys)
        {
            dictionary.ShouldContainKey(key);
        }
    }

    public static void ShouldDeeplyBe(this object? payload, object? json,
        bool useSystemTextJson = false
    ) => payload
        .ToJsonString(useSystemTextJson: useSystemTextJson)
        .ShouldBe(json.ToJsonString(useSystemTextJson: useSystemTextJson));

    [return: NotNullIfNotNull("payload")]
    public static string? ToJsonString(this object? payload,
        bool useSystemTextJson = false
    ) => payload is null ? null :
        useSystemTextJson ? System.Text.Json.JsonSerializer.Serialize(payload) :
        JsonConvert.SerializeObject(payload);

    [return: NotNullIfNotNull("payload")]
    public static object? ToJsonObject(this object? payload) =>
        JsonConvert.DeserializeObject(payload.ToJsonString() ?? string.Empty);

    public static PropertyInfo? ThePropertyOf<T>(this Stubber _, string name) =>
        typeof(T).GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

    public static void ShouldBe<T>(this Type type) =>
        type.ShouldBe(typeof(T));

    public static void ShouldBeAbstract(this PropertyInfo property)
    {
        var getMethod = property.GetGetMethod(true);

        getMethod.ShouldNotBeNull();
        getMethod.ShouldBeAbstract();
    }

    public static void ShouldBeVirtual(this PropertyInfo property)
    {
        var getMethod = property.GetGetMethod(true);

        getMethod.ShouldNotBeNull();
        getMethod.ShouldBeVirtual();
    }

    public static MethodInfo? TheMethodOf<T>(this Stubber _, string name) =>
        typeof(T).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

    public static void ShouldBeAbstract(this MethodInfo method)
    {
        method.IsAbstract.ShouldBeTrue();
    }

    public static void ShouldBeVirtual(this MethodInfo method)
    {
        method.IsVirtual.ShouldBeTrue();
    }

    public static void ShouldHaveOneParameter<T>(this MethodInfo method)
    {
        method.GetParameters().Length.ShouldBe(1);
        method.GetParameters().First().ParameterType.ShouldBe<T>();
    }

    public static void ValuesShouldBe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        params TValue[] values
    ) where TKey : notnull
    {
        foreach (var value in dictionary.Values)
        {
            value.ShouldBe(values.FirstOrDefault());
            values = [.. values.Skip(1)];
        }
    }
}
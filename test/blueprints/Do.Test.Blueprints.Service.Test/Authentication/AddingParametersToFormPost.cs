﻿using Do.Architecture;
using Do.Authentication;

namespace Do.Test.Authentication;

public class AddingParametersToFormPost : TestServiceNfr
{
    protected override IEnumerable<Func<AuthenticationConfigurator, IFeature<AuthenticationConfigurator>>>? Authentications =>
        [
            c => c.FixedBearerToken(tokens =>
            {
                tokens.Add("Jane", claims: ["User"]);
            },
            formPostParameters: ["additional"])
        ];

    [Test]
    public async Task Hash_and_configured_additional_parameters_are_added_to_form_post_authenticate_requests()
    {
        var form = new Dictionary<string, string>
        {
            ["value"] = "value",
            ["additional"] = "additional",
            ["hash"] = "ph0dPl9pFZdlrAq92u8NmuOHyMVus/pJQ0zNMEuej5A=" // valueadditional11111111111111111111111111111111 -sha256-> a61d1d3e5f69159765ac0abddaef0d9ae387c8c56eb3fa49434ccd304b9e8f90
        };

        var response = await Client.PostAsync("authentication-samples/form-post-authenticate", new FormUrlEncodedContent(form));

        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("FixedBearerToken");
    }

    [Test]
    public async Task Hash_parameter_is_required()
    {
        var form = new Dictionary<string, string>
        {
            ["value"] = "value",
            ["additional"] = "additional"
        };

        var response = await Client.PostAsync("authentication-samples/form-post-authenticate", new FormUrlEncodedContent(form));

        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBeNullOrEmpty();
    }
}
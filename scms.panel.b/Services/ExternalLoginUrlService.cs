using Microsoft.AspNetCore.Components;
using scms.panel.b.Abstractions;
using scms.panel.b.Abstractions.Helper;
using scms.panel.b.Enums;
using System;
using System.ComponentModel.Design;

namespace scms.panel.b.Services;

public class ExternalLoginUrlService : IExternalLoginUrlService
{
    readonly IConfiguration _configuration;
    readonly NavigationManager _navigationManager;
    readonly IHelper _helper;

    public ExternalLoginUrlService(IConfiguration configuration, NavigationManager navigationManager, IHelper helper)
    {
        _configuration = configuration;
        _navigationManager = navigationManager;
        _helper = helper;
    }

    public string? CreateAndGetUrl(ExternalLoginType type)
    {
        string? url = $"{_configuration?.GetSection(type.ToString()).GetSection("EndPointUrl").Value}?";

        var paramList = _configuration?.GetSection(type.ToString()).GetSection("params").GetChildren().ToArray();

        foreach (var item in paramList)
        {
            if (item.Key == "redirect_uri")
            {
                url += $"&{item.Key}={_navigationManager.BaseUri.ToString()}{item.Value}";
            }
            else if (item.Key == "state")
            {
                url += $"&{item.Key}={_helper.CreateHashRandom()}";
            }
            else
            {
                url += $"&{item.Key}={item.Value}";
            }
        }
        //for (int i = 0; i < paramList.Count(); i++)
        //{
        //    var item = paramList[i];
        //    //if (i == 0)
        //    //{
        //    //    //url = $"{_configuration.GetSection(type.ToString()).GetSection("EndPointUrl").Value}?{item.Key}={item.Value}";
        //    //    url = $"{_configuration?.GetSection(type.ToString()).GetSection("EndPointUrl").Value}?";
        //    //}

        //    if (item.Key == "redirect_uri")
        //    {
        //        url += $"&{item.Key}={_navigationManager.BaseUri.ToString()}{item.Value}";
        //    } else if (item.Key == "state")
        //    {
        //        url += $"&{item.Key}={_helper.CreateHashRandom()}";
        //    }
        //    else
        //    {
        //        url += $"&{item.Key}={item.Value}";
        //    }
        //}
        return url;
    }
}

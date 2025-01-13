using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GoogleDocsImporter// : WebClient
{    
    //private const string EDIT = "edit?gid=";
    //private const string EXPORT = "export?format=tsv&gid=";

    //private readonly CookieContainer container = new CookieContainer();

    //protected override WebRequest GetWebRequest(Uri address)
    //{
    //    WebRequest r = base.GetWebRequest(address);
    //    var request = r as HttpWebRequest;
    //    if (request != null)
    //    {
    //        request.CookieContainer = container;
    //    }
    //    return r;
    //}

    //protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
    //{
    //    WebResponse response = base.GetWebResponse(request, result);
    //    ReadCookies(response);
    //    return response;
    //}

    //protected override WebResponse GetWebResponse(WebRequest request)
    //{
    //    WebResponse response = base.GetWebResponse(request);
    //    ReadCookies(response);
    //    return response;
    //}

    //private void ReadCookies(WebResponse r)
    //{
    //    var response = r as HttpWebResponse;
    //    if (response != null)
    //    {
    //        CookieCollection cookies = response.Cookies;
    //        container.Add(cookies);
    //    }
    //}

    //public string GetDataFromUrl(string url)
    //{
    //    string result = String.Empty;

    //    url = RebuildUrl(url);     
        
    //    Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
    //    Headers.Add("DNT", "1");
    //    Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
    //    Headers.Add("Accept-Encoding", "deflate");
    //    Headers.Add("Accept-Language", "en-US,en;q=0.5");

    //    byte[] dt = DownloadData(url);
    //    result = System.Text.Encoding.UTF8.GetString(dt ?? new byte[] { });
        
    //    return result;
    //}

    //private string RebuildUrl(string input)
    //{
    //    input = input.Replace(EDIT, EXPORT);       
    //    return input;
    //}
}

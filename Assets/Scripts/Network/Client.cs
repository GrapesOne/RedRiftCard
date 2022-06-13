using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class Client 
{
    private static readonly HttpClient HttpClient = new HttpClient();
    
   
    public static async Task<Jsend<byte[]>> DownloadFileBytesAsync(string url, CancellationToken token = new CancellationToken())
    {
        if (!HasInternet()) return DefaultJsend<byte[]>();
        var jsend = new Jsend<byte[]>();
        try
        {
            var response = await HttpClient.GetAsync(url, token);
            jsend.status = Status.Success;
            jsend.message = "";
            jsend.data = await response.Content.ReadAsByteArrayAsync();
        }
        catch (HttpRequestException e)
        {
            jsend.status = Status.NoInternetConnection;
            jsend.message = e.Message;
        }
        catch (TaskCanceledException e)
        {
            jsend.status = Status.TaskCanceled;
            jsend.message = e.Message;
        }

        return jsend;
    }
    
    public static bool HasInternet() => Application.internetReachability != NetworkReachability.NotReachable;

    private static Jsend<TResponse> DefaultJsend<TResponse>() => new Jsend<TResponse>
    {
        status = Status.NoInternetConnection,
        message = "NoInternetConnection"
    };
    [Serializable]
    public class Jsend<T>
    {
        public string status;
        public string message;
        public T data;
    }
    
   
}

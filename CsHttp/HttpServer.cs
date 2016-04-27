using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CsHttp
{
  public class HttpServer
  {
    private readonly HttpListener _httpListener;
    private readonly Dictionary<string, Action<HttpListenerContext>> _routes;

    public HttpServer()
    {
      _httpListener = new HttpListener();
      _routes = new Dictionary<string, Action<HttpListenerContext>>();
    }

    public void AddRoute(String path, Action<HttpListenerContext> action)
    {
       _routes.Add(path, action);
    }

    public async void Listen(int port)
    {
        _httpListener.Prefixes.Add($"http://*:{port}/");
        _httpListener.Start();
        for (;;)
        {
          var context = _httpListener.GetContext();
          await Dispatch(context);
        }
    }

    private async Task<bool> Dispatch(HttpListenerContext context)
    {
        Action<HttpListenerContext> action;
        if (_routes.TryGetValue(context.Request.Url.LocalPath, out action))
        {
          await Task.Run(() => action.Invoke(context));
        }
        else
        {
          await Task.Run(() =>
          {
            context.Response.StatusCode = 404;
            context.Response.Close();
          });
        }
      return true;
    }

  }
}
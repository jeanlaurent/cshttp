using System.Text;


namespace CsHttp
{

  class Program
  {

    static void Main(string[] args)
    {
      // named pipes on windows : \\<machine_address>\pipe\<pipename>
      var httpServer = new HttpServer();
      httpServer.AddRoute("/", (context) =>
      {
          context.Response.StatusCode = 200;
          context.Response.Close();
      });
      httpServer.AddRoute("/foobar", (context) =>
      {
        context.Response.StatusCode = 200;
        context.Response.Headers.Add("Content-Type", "application/json");
        var bytes = Encoding.UTF8.GetBytes("{\"name\":\"foo\"}");
        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        context.Response.Close();
      });
      httpServer.Listen(8080);
    }
  }
}



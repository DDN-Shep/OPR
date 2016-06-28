using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;
using System.Net;

namespace Validus.OPR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.AppSettings()
                   .CreateLogger();

            var listener = (HttpListener)app.Properties[typeof(HttpListener).FullName];

            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;

            app.MapHubs("/signalr", new HubConfiguration
            {
                EnableJavaScriptProxies = true,
                EnableCrossDomain = true
            });

            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString(""),
                FileSystem = new PhysicalFileSystem(@".\web"),
            });
        }
    }
}
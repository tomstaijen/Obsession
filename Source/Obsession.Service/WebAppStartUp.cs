using System.Net.Http.Formatting;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Nancy;
using Owin;

namespace Obsession.Service
{
    public class WebAppStartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options =>
              options.PerformPassThrough = context =>
                  context.Response.StatusCode == HttpStatusCode.NotFound);

            var config = new HttpConfiguration();
            var route = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Bootstrapper.GetContainer());
            app.UseWebApi(config);
        }
    }
}
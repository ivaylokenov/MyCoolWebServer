namespace MyCoolWebServer.Application
{
    using Application.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/", 
                req => new HomeController().Index());

            appRouteConfig.Get(
                "/testsession",
                req => new HomeController().SessionTest(req));
            
            appRouteConfig.Get(
                "/users/{(?<name>[a-z]+)}",
                req => new HomeController().Index());
        }
    }
}

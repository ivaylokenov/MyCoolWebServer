namespace MyCoolWebServer.Application.Views.Home
{
    using Server.Contracts;

    public class IndexView : IView
    {
        public string View() => "<h1>Welcome!</h1>";
    } 
}

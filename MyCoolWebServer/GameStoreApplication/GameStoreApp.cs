namespace MyCoolWebServer.GameStoreApplication
{
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using System;
    using System.Globalization;
    using ViewModels.Account;
    using ViewModels.Admin;

    public class GameStoreApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("/account/register");
            appRouteConfig.AnonymousPaths.Add("/account/login");

            appRouteConfig
                .Get("/", req => new HomeController(req).Index());

            appRouteConfig
                .Get(
                    "/account/register",
                    req => new AccountController(req).Register());

            appRouteConfig
                .Post(
                    "/account/register",
                    req => new AccountController(req).Register(
                        new RegisterViewModel
                        {
                            Email = req.FormData["email"],
                            FullName = req.FormData["full-name"],
                            Password = req.FormData["password"],
                            ConfirmPassword = req.FormData["confirm-password"]
                        }));

            appRouteConfig
                .Get(
                    "/account/login",
                    req => new AccountController(req).Login());

            appRouteConfig
                .Post(
                    "/account/login",
                    req => new AccountController(req).Login(
                        new LoginViewModel
                        {
                            Email = req.FormData["email"],
                            Password = req.FormData["password"]
                        }));

            appRouteConfig
                .Get(
                    "/account/logout",
                    req => new AccountController(req).Logout());

            appRouteConfig
                .Get(
                    "/admin/games/add",
                    req => new AdminController(req).Add());

            appRouteConfig
                .Post(
                    "/admin/games/add",
                    req => new AdminController(req).Add(
                        new AdminAddGameViewModel
                        {
                            Title = req.FormData["title"],
                            Description = req.FormData["description"],
                            Image = req.FormData["thumbnail"],
                            Price = decimal.Parse(req.FormData["price"]),
                            Size = double.Parse(req.FormData["size"]),
                            VideoId = req.FormData["video-id"],
                            ReleaseDate = DateTime.ParseExact(
                                req.FormData["release-date"],
                                "yyyy-MM-dd",
                                CultureInfo.InvariantCulture)
                        }));

            appRouteConfig
                .Get(
                    "/admin/games/list",
                    req => new AdminController(req).List());
        }
    }
}

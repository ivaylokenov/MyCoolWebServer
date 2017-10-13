namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using System;
    using ViewModels;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";

        private readonly IUserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(RegisterView);
        }

        public IHttpResponse Register(
            IHttpRequest req, 
            RegisterUserViewModel model)
        {
            this.SetDefaultViewData();
            
            if (model.Username.Length < 3
                || model.Password.Length < 3
                || model.ConfirmPassword != model.Password)
            {
                this.ShowError("Invalid user details");

                return this.FileViewResponse(RegisterView);
            }

            var success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(req, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowError("This username is taken");

                return this.FileViewResponse(RegisterView);
            }
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(LoginView);
        }

        public IHttpResponse Login(
            IHttpRequest req,
            LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username)
                || string.IsNullOrWhiteSpace(model.Password))
            {
                this.ShowError("You have empty fields");

                return this.FileViewResponse(LoginView);
            }

            var success = this.users.Find(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(req, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowError("Invalid user details");

                return this.FileViewResponse(LoginView);
            }
        }

        public IHttpResponse Profile(IHttpRequest req)
        {
            if (!req.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = this.users.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database.");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse(@"account\profile");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData() => this.ViewData["authDisplay"] = "none";

        private void LoginUser(IHttpRequest req, string username)
        {
            req.Session.Add(SessionStore.CurrentUserKey, username);
            req.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}

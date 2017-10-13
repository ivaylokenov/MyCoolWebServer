namespace MyCoolWebServer.GameStoreApplication.Controllers
{
    using Server.Http;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";

        private readonly IUserService users;

        public AccountController(IHttpRequest request)
            : base(request)
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
            => this.FileViewResponse(RegisterView);

        public IHttpResponse Register(RegisterViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            var success = this.users
                .Create(model.Email, model.FullName, model.Password);

            if (!success)
            {
                this.ShowError("E-mail is taken.");
                return this.Register();
            }
            else
            {
                this.LoginUser(model.Email);
                return this.RedirectResponse(HomePath);
            }
        }

        public IHttpResponse Login()
            => this.FileViewResponse(LoginView);

        public IHttpResponse Login(LoginViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            var success = this.users.Find(model.Email, model.Password);

            if (!success)
            {
                this.ShowError("Invalid user details.");

                return this.Login();
            }
            else
            {
                this.LoginUser(model.Email);

                return this.RedirectResponse(HomePath);
            }
        }

        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();

            return this.RedirectResponse(HomePath);
        }
        
        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
        }
    }
}

namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using System;
    using System.Linq;
    using ViewModels;

    public class ShoppingController : BaseController
    {
        private readonly IUserService users;
        private readonly IProductService products;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.users = new UserService();
            this.products = new ProductService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            var productExists = this.products.Exists(id);

            if (!productExists)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.ProductIds.Add(id);

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (req.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={req.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productsInCart = this.products
                    .FindProductsInCart(shoppingCart.ProductIds);

                var items = productsInCart
                    .Select(pr => $"<div>{pr.Name} - ${pr.Price:F2}</div><br />");

                var totalPrice = productsInCart
                    .Sum(pr => pr.Price);
                
                this.ViewData["cartItems"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalPrice:F2}";
            }

            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var userId = this.users.GetUserId(username);
            if (userId == null)
            {
                throw new InvalidOperationException($"User {username} does not exist");
            }

            var productIds = shoppingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this.shopping.CreateOrder(userId.Value, productIds);

            shoppingCart.ProductIds.Clear();

            return this.FileViewResponse(@"shopping\finish-order");
        }
    }
}

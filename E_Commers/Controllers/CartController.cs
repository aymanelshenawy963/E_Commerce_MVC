using E_Commers.Models;
using E_Commers.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commers.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartRepository cartRepository,UserManager<ApplicationUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepository.GetAll([e => e.Product]).Where(e => e.ApplicationUserId == ApplicationUserId);

            ViewBag.Total = cartProduct.Sum(e => e.Product.Price * e.count);

            return View(cartProduct.ToList());
        }

        public IActionResult AddToCart(int count,int productId)
        {
            Cart cart = new Cart()
            {
                count = count,
                ProductId = productId,
                ApplicationUserId = userManager.GetUserId(User)
            };
            cartRepository.Add(cart);
            cartRepository.Commit();
            TempData["success"] = "Add Product To Cart success";
            return RedirectToAction("Index","Home");
        }

        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = cartRepository.GetOne(expression: e =>e.ApplicationUserId == ApplicationUserId && e.ProductId==productId);

            if (product != null)
            {
                product.count++;
                cartRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage","Home");
        }

        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.count--;
                if (product.count > 0)
                    cartRepository.Commit();
                else
                    product.count = 1;

                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepository.GetOne(null, e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepository.Delete(product);
                cartRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage", "Home");
        }


        public IActionResult Pay()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepository.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}

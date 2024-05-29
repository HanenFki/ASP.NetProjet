using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projet2.Models;
using projet2.Models.Repositories;
using projet2.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace projet2.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository<Order> _orderRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IProductRepository<Produit> _produitRepository;
        private readonly ICartRepository _cartRepository;

        public OrderController(IOrderRepository<Order> orderRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IProductRepository<Produit> produitRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _produitRepository = produitRepository;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            var cart = _cartRepository.GetCartItems();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new HomeViewModel
            {
                CartItems = cart,
                CartTotal = _cartRepository.GetCartTotal(),
				UserEmail = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(HomeViewModel model)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            var cart = _cartRepository.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = _cartRepository.GetCartTotal(),
                IsDelivered = false,
                User = user.UserName,
                Products = cart.Select(c =>
                {
                    var produit = c.Produit;
                    _produitRepository.Attach(produit);
                    return produit;
                }).ToList()
            };

            _orderRepository.Add(order);
            _cartRepository.ClearCart();

            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

       
        
			public IActionResult Index(string email)
			{
				IEnumerable<Order> orders;
				if (!string.IsNullOrEmpty(email))
				{
					orders = _orderRepository.GetAll().Where(o => o.User == email);
				}
				else
				{
					orders = _orderRepository.GetAll();
				}

				foreach (var order in orders)
				{
					order.Products = _orderRepository.GetOrderProducts(order.OrderId).ToList();
				}

				return View(orders);
			}
		


        public IActionResult Details(int id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        public IActionResult Deliver(int id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }

            order.IsDelivered = true;
            _orderRepository.Update(order);

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderRepository.Delete(id);
            return RedirectToAction("Index");
        }


    }
}

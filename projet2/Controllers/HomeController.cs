using Microsoft.AspNetCore.Mvc;
using projet2.Models;
using projet2.Models.Repositories;
using projet2.ViewModels;

namespace projet2.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICategorieRepository<Categorie> _categorieRepository;
		private readonly IProductRepository<Produit> _productRepository;
		private readonly ICartRepository _cartRepository;

		public HomeController(ICategorieRepository<Categorie> categorieRepository, IProductRepository<Produit> productRepository, ICartRepository cartRepository)
		{
			_categorieRepository = categorieRepository;
			_productRepository = productRepository;
			this._cartRepository = cartRepository;
		}

		public IActionResult Index()
		{
			var categories = _categorieRepository.GetAll().ToList();
			var products = _productRepository.GetAll().ToList();
			var marques = _productRepository.GetAllMarques().ToList();

			var viewModel = new HomeViewModel
			{
				Categories = categories,
				Products = products,
				CartItems = _cartRepository.GetCartItems(),
				CartItemCount = _cartRepository.GetCartItemCount(), 
				CartTotal = _cartRepository.GetCartTotal(),
				Marques = marques
			};

			return View(viewModel);
		}

		public IActionResult Cart()
		{
			var cartItems = _cartRepository.GetCartItems() ?? new List<CartItem>();
			var cartItemCount = _cartRepository.GetCartItemCount();
			var cartTotal = _cartRepository.GetCartTotal();

			var viewModel = new HomeViewModel
			{
				CartItems = cartItems,
				CartItemCount = cartItemCount,
				CartTotal = cartTotal,
				Categories = new List<Categorie>(), 
				Products = new List<Produit>()      
			};

			return View(viewModel);
		}

		 [HttpGet]
        public IActionResult Search(string marque, string query)
        {
            var products = _productRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(marque))
            {
                products = products.Where(p => p.Marque == marque);
            }

            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Désignation.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            var categories = _categorieRepository.GetAll().ToList();
            var marques = _productRepository.GetAllMarques().ToList();

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products.ToList(),
                CartItems = _cartRepository.GetCartItems() ?? new List<CartItem>(),
                CartTotal = _cartRepository.GetCartTotal(),
                Marques = marques
            };

            return View("Index", viewModel);
        }
        [HttpPost]
		public IActionResult AddToCart(int productId)
		{
			_cartRepository.AddToCart(productId);
			return RedirectToAction("Index");
		}

	}
}

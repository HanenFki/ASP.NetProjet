using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using projet2.Controllers;

namespace projet2.Models.Repositories
{
	public class SqlCartRepository : ICartRepository
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IProductRepository<Produit> _productRepository;

		public SqlCartRepository(IHttpContextAccessor httpContextAccessor, IProductRepository<Produit> productRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_productRepository = productRepository;
		}

		public void AddToCart(int productId)
		{
			var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<CartItem>>("CartItems") ?? new List<CartItem>();
			var product = _productRepository.Get(productId);
			if (product != null)
			{
				var cartItem = cartItems.SingleOrDefault(item => item.Produit.Id == productId);
				if (cartItem == null)
				{
					cartItems.Add(new CartItem
					{
						Id = cartItems.Count + 1, // Juste pour l'exemple
						Produit = product,
						Quantite = 1
					});
				}
				else
				{
					cartItem.Quantite++;
				}
			}
			_httpContextAccessor.HttpContext.Session.Set("CartItems", cartItems);
		}

		public List<CartItem> GetCartItems()
		{
			return _httpContextAccessor.HttpContext.Session.Get<List<CartItem>>("CartItems") ?? new List<CartItem>();
		}

		public int GetCartItemCount()
		{
			var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<CartItem>>("CartItems");
			return cartItems != null ? cartItems.Sum(item => item.Quantite) : 0;
		}

		public float GetCartTotal()
		{
			var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<CartItem>>("CartItems");
			return cartItems != null ? cartItems.Sum(item => item.TotalPrice) : 0;
		}
        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove("CartItems");
        }

    }
}

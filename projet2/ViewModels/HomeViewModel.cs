using projet2.Models;

namespace projet2.ViewModels
{
	public class HomeViewModel
	{
		internal string? UserEmail;

		public IEnumerable<Categorie> Categories { get; set; }
		public IEnumerable<Produit> Products { get; set; }
		public IEnumerable<string> Marques { get; set; } // Ajoutez une propriété pour stocker les marques
		public IEnumerable<CartItem> CartItems { get; set; }
		public int CartItemCount { get; set; }
		public float CartTotal { get; set; }
	}
}

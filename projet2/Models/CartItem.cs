
namespace projet2.Models
{
    public class CartItem
    {

		public int Id { get; set; }
		public Produit Produit { get; set; }
		public int Quantite { get; set; }
		public float TotalPrice => Produit.Prix * Quantite;
	}
}

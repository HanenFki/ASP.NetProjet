namespace projet2.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public required string User { get; set; }
        // Collection de produits associés à cette commande
        public required ICollection<Produit> Products { get; set; }
        public bool IsDelivered { get; set; }
    }
}

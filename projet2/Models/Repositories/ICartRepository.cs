namespace projet2.Models.Repositories
{
    public interface ICartRepository {
        void AddToCart(int productId);
        int GetCartItemCount();
        List<CartItem> GetCartItems();
		float GetCartTotal();
        void ClearCart();
    }

}

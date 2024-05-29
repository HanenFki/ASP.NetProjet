namespace projet2.Models.Repositories
{
    public interface IOrderRepository<T>
    {
        T Get(int Id);
        IEnumerable<T> GetAll();
        T Add(T t);
        T Update(T t);
        T Delete(int Id);
        IEnumerable<Produit> GetOrderProducts(int Id);
    }
}

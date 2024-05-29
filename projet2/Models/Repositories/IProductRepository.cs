namespace projet2.Models.Repositories
{
	public interface IProductRepository<T>
	{
		T Get(int Id);
		IEnumerable<T> GetAll();
		T Add(T t);
		T Update(T t);
		T Delete(int Id);
		IEnumerable<T> GetProductsByCategoryId(int categoryId);
		IEnumerable<String> GetAllMarques();
		List<T> Search(string query);
        void Attach(T entity);


    }
}

namespace projet2.Models.Repositories
{
	public interface ICategorieRepository<T>
	{
		T Get(int Id);
		IEnumerable<T> GetAll();
		T Add(T t);
		T Update(T t);
		T Delete(int Id);
        int GetCategoryIdByName(string categoryName);
    }
}

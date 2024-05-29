using Microsoft.EntityFrameworkCore;

namespace projet2.Models.Repositories
{
	public class SqlCategorieRepository : ICategorieRepository<Categorie>
	{
		private readonly AppDbContext context;
		public SqlCategorieRepository(AppDbContext context)
		{
			this.context = context;
		}
		public Categorie Add(Categorie cat)
		{
			context.Categories.Add(cat);
			context.SaveChanges();
			return cat;
		}
		public Categorie Delete(int Id)
		{
			Categorie cat = context.Categories.Find(Id);
			if (cat != null)
			{
				context.Categories.Remove(cat);
				context.SaveChanges();
			}
			return cat;
		}
		public IEnumerable<Categorie> GetAll()
		{
			return context.Categories;
		}
		public Categorie Get(int Id)
		{
			return context.Categories.Find(Id);
		}
		public Categorie Update(Categorie cat)
		{
			var Categorie =
			context.Categories.Attach(cat);
			Categorie.State = EntityState.Modified;
			context.SaveChanges();
			return cat;
		}
        public int GetCategoryIdByName(string categoryName)
        {
            var category = context.Categories.FirstOrDefault(c => c.Name == categoryName);
            return category != null ? category.Id : 0; // Retourne l'ID de la catégorie si elle existe, sinon retourne 0
        }

    }

}


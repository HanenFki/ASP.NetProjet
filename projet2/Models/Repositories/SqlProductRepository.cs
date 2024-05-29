using Microsoft.EntityFrameworkCore;

namespace projet2.Models.Repositories
{
	public class SqlProductRepository : IProductRepository<Produit>
	{
		private readonly AppDbContext context;
		public SqlProductRepository(AppDbContext context)
		{
			this.context = context;
		}
		public Produit Add(Produit P)
		{
			context.Produits.Add(P);
			context.SaveChanges();
			return P;
		}
		public Produit Delete(int Id)
		{
			Produit P = context.Produits.Find(Id);
			if (P != null)
			{
				context.Produits.Remove(P);
				context.SaveChanges();
			}
			return P;
		}
		public IEnumerable<Produit> GetAll()
		{
			return context.Produits.Include(x=> x.Category).ToList();
		}
		public Produit Get(int Id)
		{
			return context.Produits.Find(Id);
		}
		public Produit Update(Produit P)
		{
			var Produit =
			context.Produits.Attach(P);
			Produit.State = EntityState.Modified;
			context.SaveChanges();
			return P;
		}
		public IEnumerable<Produit> GetProductsByCategoryId(int categoryId)
		{
			return context.Produits.Where(p => p.CategoryId == categoryId).ToList();
		}


		
		
			public IEnumerable<string> GetAllMarques()
			{
				// Utilisez LINQ pour extraire toutes les marques uniques des produits
				return context.Produits.Select(p => p.Marque).Distinct().ToList();
			}
		
		public List<Produit> Search(string query)
        {
            // Recherchez dans les produits où le nom ou la description contient la requête
             return context.Produits
                .Where(p => p.Désignation.Contains(query) || p.Marque.Contains(query))
                .ToList();
        }
        public void Attach(Produit produit)
        {
            context.Attach(produit);
        }

    }
}

	
	

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace projet2.Models
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Categorie> Categories { get; set; }
		public DbSet<Produit> Produits { get; set; }
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // Important: Appel à la méthode de base

			

			modelBuilder.Entity<Order>()
				.Property(o => o.TotalAmount)
				.HasColumnType("decimal(18,2)"); // Définir le type et la précision

			// Configuration additionnelle pour les entités
			modelBuilder.Entity<Produit>()
				.HasKey(p => p.Id);
			modelBuilder.Entity<Order>()
				.HasKey(o => o.OrderId);
		}
	}
}

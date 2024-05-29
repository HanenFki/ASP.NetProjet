using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projet2.Models
{
	public class Produit
	{
		public int Id { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Désignation { get; set; }
		[Required]
		[Display(Name = "Prix en dinar :")]
		public float Prix { get; set; }
		[Required]
		[Display(Name = "Quantité en unité :")]
		public int Quantite { get; set; }
		[Required]
		[Display(Name = "Image :")]
		public string Image { get; set; }
		[Display(Name = "Nombre de ventes")]

		public int NbDeVente { get; set; }
		// Clé étrangère
		[Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        // Navigation vers la catégorie
        [ForeignKey("CategoryId")]
		public virtual Categorie Category { get; set; }
        [Display(Name = "Marque")]

        public String Marque { get; set; }
       
        

    }
}

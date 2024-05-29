using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace projet2.ViewModels
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Désignation { get; set; }
        [Required]
        [StringLength(50)]
        public string Marque { get; set; }

        [Required]
        [Display(Name = "Prix en dinar :")]
        public float Prix { get; set; }

        [Required]
        [Display(Name = "Quantité en unité :")]
        public int Quantite { get; set; }

        [Required]
        [Display(Name = "Image :")]
        public IFormFile ImagePath { get; set; }

        [Display(Name = "Nombre de ventes")]
        public int NbDeVente { get; set; }

        [Required]
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }
       
    }
}

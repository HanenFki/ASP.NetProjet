using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace projet2.ViewModels
{
    public class EditProduitViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ Désignation est requis.")]
        public string Désignation { get; set; }

        [Required(ErrorMessage = "Le champ Prix est requis.")]
        public float Prix { get; set; }

        [Required(ErrorMessage = "Le champ Quantite est requis.")]
        public int Quantite { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImagePath { get; set; } // Facultatif pour l'édition

        public string ExistingImagePath { get; set; }

        [Required(ErrorMessage = "Le champ NbDeVente est requis.")]
        public int NbDeVente { get; set; }

        [Required(ErrorMessage = "Le champ CategoryId est requis.")]
        public int CategoryId { get; set; }
        public bool IsNewImageUploaded { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Marque { get; set; }




    }
}

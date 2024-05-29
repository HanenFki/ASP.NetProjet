using System.ComponentModel.DataAnnotations;

namespace projet2.Models
{
	public class Categorie
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Image :")]
		public string ImageUrl { get; set; }
	}
}

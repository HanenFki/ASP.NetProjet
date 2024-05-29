using System.ComponentModel.DataAnnotations;

namespace projet2.ViewModels
{
	public class CreateCategorieViewModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Image :")]
		public IFormFile ImagePath { get; set; }
	}
}

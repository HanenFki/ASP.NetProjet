using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projet2.Models;
using projet2.Models.Repositories;
using projet2.ViewModels;

namespace projet2.Controllers
{
	public class CategorieController : Controller
	{
		private readonly ICategorieRepository<Categorie> CategorieRepository;
		private readonly IWebHostEnvironment hostingEnvironment;

		public CategorieController(ICategorieRepository<Categorie> catRepository, IWebHostEnvironment hostingEnvironment)
		{
			CategorieRepository = catRepository;
			this.hostingEnvironment = hostingEnvironment;
		}
		// GET: CategorieController
		public ActionResult Index()
		{
			var categories = CategorieRepository.GetAll();
			return View(categories);
		}

		// GET: CategorieController/Details/5
		public ActionResult Details(int id)
		{
			var categorie = CategorieRepository.Get(id);
			if (categorie == null)
			{
				return NotFound();
			}
			return View(categorie);
		}

		// GET: CategorieController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CategorieController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public ActionResult Create(CreateCategorieViewModel model)
		{
			if (ModelState.IsValid)
			{
				string uniqueFileName = null;

				// Vérifie si l'utilisateur a sélectionné une image
				if (model.ImagePath != null)
				{
					string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
					uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);
					model.ImagePath.CopyTo(new FileStream(filePath, FileMode.Create));
				}

				// Crée une nouvelle catégorie avec les données fournies
				Categorie newCategorie = new Categorie
				{
					Name = model.Name,

					ImageUrl = uniqueFileName
				};

				// Ajoute la nouvelle catégorie à la base de données
				CategorieRepository.Add(newCategorie);

				return RedirectToAction("details", new { id = newCategorie.Id });
			}

			return View();
		}
	

		// GET: CategorieController/Edit/5
		public ActionResult Edit(int id)
		{
			var categorie = CategorieRepository.Get(id);
           
            EditCategorieViewModel categorieEditViewModel = new EditCategorieViewModel
            {
                Id = categorie.Id,
				 Name= categorie.Name,
                ExistingImagePath = categorie.ImageUrl
            };
            return View(categorieEditViewModel);
        }

		// POST: CategorieController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditCategorieViewModel model)

        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the product being edited from the database
                Categorie categorie = CategorieRepository.Get(model.Id);
                // Update the product object with the data in the model object
                categorie.Name = model.Name;
              
                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.ImagePath != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingImagePath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "img", model.ExistingImagePath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the product object which will be
                    // eventually saved in the database
                    categorie.ImageUrl = ProcessUploadedFile(model);
                }
                // Call update method on the repository service passing it the
                // product object to update the data in the database table
                Categorie updatedCategorie = CategorieRepository.Update(categorie);
                if (updatedCategorie != null)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            return View(model);
        }
        [NonAction]
        private string ProcessUploadedFile(EditCategorieViewModel model)
        {
            string uniqueFileName = null;
            if (model.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: CategorieController/Delete/5
        public ActionResult Delete(int id)
		{
			var categorie = CategorieRepository.Get(id);
			if (categorie == null)
			{
				return NotFound();
			}
			return View(categorie);
		}

		// POST: CategorieController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				CategorieRepository.Delete(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
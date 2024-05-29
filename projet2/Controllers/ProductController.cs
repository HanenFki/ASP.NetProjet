using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using projet2.Models;
using projet2.Models.Repositories;
using projet2.ViewModels;
using System;
using System.IO;

namespace projet2.Controllers
{
   
    public class ProductController : Controller
    {
        private readonly IProductRepository<Produit> _productRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICategorieRepository<Categorie> _categoryRepository;
        private readonly AppDbContext _context;
		private readonly ICartRepository _cartRepository;
		public ProductController(AppDbContext context,IProductRepository<Produit> productRepository, IWebHostEnvironment hostingEnvironment, ICategorieRepository<Categorie> categoryRepository, ICartRepository cartRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _hostingEnvironment = hostingEnvironment;
            _categoryRepository = categoryRepository;
			_cartRepository = cartRepository;

		}
        // GET: ProductController
        public ActionResult Index()
		{
            var categories = _categoryRepository.GetAll().ToList();
            var products = _productRepository.GetAll().ToList();

            // Initialisez un objet HomeViewModel
            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products,
                CartItems = _cartRepository.GetCartItems(),
                CartItemCount = _cartRepository.GetCartItemCount(),
                CartTotal = _cartRepository.GetCartTotal()
            };

            // Retournez la vue avec le modèle correct
            return View(viewModel);
        }
    

		// GET: ProductController/Details/5
		public ActionResult Details(int id)
{
            var product = _context.Produits.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.ImagePath != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ImagePath.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                // Obtenez la catégorie associée au produit à partir de son ID
                var category = _categoryRepository.Get(model.CategoryId);

                Produit newProduct = new Produit
                {
                    Désignation = model.Désignation,
                    Prix = model.Prix,
                    Quantite = model.Quantite,
                    Image = uniqueFileName,
                    NbDeVente = 0,
                    Marque=model.Marque,
                    // Affectez la catégorie au produit
                    Category = category
                };

                _productRepository.Add(newProduct);
                return RedirectToAction("Details", new { id = newProduct.Id });
            }

            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(model);
        }


        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            // Chargez les catégories avant de renvoyer la vue
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            EditProduitViewModel productEditViewModel = new EditProduitViewModel
            {
                Id = product.Id,
                Désignation = product.Désignation,
                Prix = product.Prix,
                Quantite = product.Quantite,
                CategoryId = product.CategoryId,
                ExistingImagePath = product.Image,
                NbDeVente=product.NbDeVente,
                Marque= product.Marque,
            };

            return View(productEditViewModel);
        }



        // POST: ProductController/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProduitViewModel model)
        {
            if (ModelState.IsValid)
            {
                Produit product = _productRepository.Get(model.Id);

                product.Désignation = model.Désignation;
                product.Prix = model.Prix;
                product.Quantite = model.Quantite;
                product.NbDeVente = model.NbDeVente;
                product.Marque = model.Marque;

                // Déterminez si une nouvelle image a été téléchargée
                bool isNewImageUploaded = (model.ImagePath != null && model.ImagePath.Length > 0);

                // Si une nouvelle image a été téléchargée, traitez-la
                if (isNewImageUploaded)
                {
                    // Supprimez l'ancienne image si elle existe
                    if (!string.IsNullOrEmpty(model.ExistingImagePath))
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", model.ExistingImagePath);
                        System.IO.File.Delete(filePath);
                    }
                    // Traitez la nouvelle image téléchargée
                    product.Image = ProcessUploadedFile(model);
                }

                // Mettez à jour le produit
                Produit updatedProduct = _productRepository.Update(product);
                if (updatedProduct != null)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }

            // Si le modèle n'est pas valide, rechargez la vue avec le modèle pour afficher les erreurs
            return View(model);
        }


        [NonAction]
		private string ProcessUploadedFile(EditProduitViewModel model)
		{
			string uniqueFileName = null;
			if (model.ImagePath != null)
			{
				string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.ImagePath.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _context.Produits.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de suppression, par exemple, en affichant un message d'erreur
                ModelState.AddModelError("", "Error deleting product: " + ex.Message);
                return View();
            }
        }
        public ActionResult ProductsByCategory(int categoryId)
        {
            var category = _categoryRepository.Get(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            var products = _productRepository.GetProductsByCategoryId(categoryId);
            return View(products);
        }
        public IActionResult AddToCart(int productId)
        {
            // Récupérer le panier de la session
            List<int> cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

         
            cart.Add(productId);

            
            HttpContext.Session.Set("Cart", cart);

         
            return RedirectToAction("Index");
        }
    }

   
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }

}

using NewFaceBook.Models;
using NewFaceBook.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NewFaceBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        //      public HomeController(ILogger<HomeController> logger)
        //    {
        //      _logger = logger;
        //}
        public HomeController(IUserRepository userRepository,
                  IHostingEnvironment hostingEnvironment)
        {
            _userRepository = userRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var model = _userRepository.GetAllUser();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {

                User = _userRepository.GetUser(id ?? 1),
                PageTitle = "Employee Details"

            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                User newUser = _userRepository.Add(user);
                return RedirectToAction("details", new { id = newUser.Id });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                User newEmployee = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Location = model.Location,
                    PhotoPath = uniqueFileName
                };
                _userRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

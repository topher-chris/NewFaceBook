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
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;

namespace NewFaceBook.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment;


        public HomeController(IUserRepository userRepository,
                  Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _userRepository = userRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _userRepository.GetAllUser();
            return View(model);
        }
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
          
            User user = _userRepository.GetUser(id.Value);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id.Value);
            }


            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {

                User = user,
                PageTitle = "User Details"

            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
      
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
      
        public ViewResult Edit(int id)
        {
            User user = _userRepository.GetUser(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Location = user.Location,
                ExistingPhotoPath = user.PhotoPath

            };
            return View(userEditViewModel);
        }


        [HttpPost]
        
        public IActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.GetUser(model.Id);
                user.Name = model.Name;
                user.Email = model.Email;
                user.Location = model.Location;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                      string filePath =  Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    user.PhotoPath = ProcessUploadedFile(model);
                }
  
                User updatedUser = _userRepository.Update(user);
                return RedirectToAction("index");

            }
            return View();
        }

        private string ProcessUploadedFile(UserCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        //  public IActionResult Privacy()
        //    {
        //       return View();
        // }
        [HttpPost]
       
        public IActionResult Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                User newuser = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Location = model.Location,
                    PhotoPath = uniqueFileName
                };
                _userRepository.Add(newuser);
                return RedirectToAction("details", new { id = newuser.Id });

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


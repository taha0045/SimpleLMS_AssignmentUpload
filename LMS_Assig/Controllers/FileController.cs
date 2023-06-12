using LMS_Assig_.Data;
using LMS_Assig_.Models;
using LMS_Assig_.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace LMS_Assig_.Controllers
{
    
    public class FileController : Controller
    {
        

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public FileController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            
             _context = context;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            return View(await _context.Assignment.Where(e => e.userId == currentUser.Id).ToListAsync());
        }

      
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
             var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var UserId = currentUser.Id;
            var viewModel = new AssignmentViewModel();
            var sectionIds = _context.AssignSection
                .Where(a => a.Teacher == currentUser.Id)
                .ToList();
            
            foreach (var item in sectionIds)
            {
                var dt = _context.SectionDetails.Where(a => a.SectionId == item.SectionId).ToList();


                if(viewModel.SectionSelectList==null)
                    viewModel.SectionSelectList=dt;

                else
                    viewModel.SectionSelectList.AddRange(dt);


            }
           



            return View(viewModel);
        }
  
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, AssignmentViewModel viewModel)
        {
            viewModel.Assignment.Status = Status.upload;
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var UserId = currentUser.Id;
            viewModel.Assignment.userId = UserId;
            string filename = file.FileName;
            filename = Path.GetFileName(filename);

            // Check file type


            string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".txt", ".txts" };
            string fileExtension = Path.GetExtension(filename);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                ViewBag.Message = "File type not allowed";
                return View();
            }


            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UserFile");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string uploadpath = Path.Combine(folderPath, filename);
            using (var stream = new FileStream(uploadpath, FileMode.Create))
            {
                file.CopyToAsync(stream);
                viewModel.Assignment.FilePath= uploadpath;
                _context.Add(viewModel.Assignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");


        }






        [Authorize(Roles = "Teacher")]
        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UserFile", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fileStream, "application/octet-stream", fileName);
        }

        public ActionResult ShowAssignments()
        {
            var vm = new AssignmentViewModel();
            vm.ApplicationUser = _userManager.GetUserAsync(HttpContext.User).Result;
            int sectionID = GetCurrentUserSectionID();
            vm.assignments = _context.Assignment.ToList();
           vm.FilterAssignments = vm.assignments.Where(a => a.SectionID == sectionID && a.Status==Status.upload).ToList();
            return View(vm);
        }

        private int GetCurrentUserSectionID()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (currentUser != null)
            {
                var sectionId = currentUser.SectionId;

                if (sectionId == null)
                {
                    return 0;
                }
                // Default value or error handling if section ID is not valid
                return (int)sectionId;
            }

            // Default value or error handling if current user is not found
            return 0;
        }






































        public async Task<IActionResult> AnsFiles()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            return View(await _context.Assignment.Where(e=>e.userId== currentUser.Id).ToListAsync());
        }





        [HttpGet]
        public async Task<IActionResult> UploadAnswer(int? id)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (id == null || _context.Assignment == null)
            {
                return NotFound();
            }

            var assignments = await _context.Assignment.FindAsync(id);
           
            return View(assignments);
        }


       


        [HttpPost]
        public async Task<IActionResult> UploadAnswer(IFormFile file, int id)
        {
            if (id == null || _context.Assignment == null)
            {
                return NotFound();
            }

            var assignments = await _context.Assignment.FindAsync(id);
            if (assignments == null)
            {
                return NotFound();
            }
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Assignment assignment = new Assignment{
                assgnID = assignments.Id,
                Name = assignments.Name,
                SectionName = assignments.SectionName,
                UploadDate = DateTime.Now,
                SectionID = assignments.SectionID,
                Status = Status.submit,
                userId= currentUser.Id
            };

            


            // Save the uploaded file in the "Uploads" folder
            string filename = file.FileName;
            filename = Path.GetFileName(filename);

            // Check file type


            string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".txt", ".txts" };
            string fileExtension = Path.GetExtension(filename);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                ViewBag.Message = "File type not allowed";
                return View();
            }
            // Save the uploaded file in the "Uploads" folder
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "AwnserUserFile");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string uploadpath = Path.Combine(folderPath, filename);
            using (var stream = new FileStream(uploadpath, FileMode.Create))
            {
                file.CopyToAsync(stream);
                assignment.FilePath = uploadpath;
                _context.Add(assignment);
                await _context.SaveChangesAsync();
            }




            return RedirectToAction("AnsFiles");
        }
        public async Task<IActionResult> AllAnswer(int? id)
        {
            return View(await _context.Assignment.Where(e => e.assgnID == id).ToListAsync());
        }

    }
}

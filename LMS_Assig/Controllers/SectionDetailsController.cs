using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_Assig_.Data;
using LMS_Assig_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LMS_Assig_.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LMS_Assig_.Controllers
{
    //[Authorize(Roles = "Teacher")]
  

    public class SectionDetailsController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        public SectionDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SectionDetails
        public async Task<IActionResult> Index()
        {
              return _context.SectionDetails != null ? 
                          View(await _context.SectionDetails.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SectionDetails'  is null.");
        }
        [Authorize(Roles = "Teacher")]
        // GET: SectionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails
                .FirstOrDefaultAsync(m => m.SectionId == id);
            if (sectionDetails == null)
            {
                return NotFound();
            }

            return View(sectionDetails);
        }
        //[Authorize(Roles = "Teacher")]
        // GET: SectionDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SectionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SectionId,Name,Title")] SectionDetails sectionDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectionDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sectionDetails);
        }
        //[Authorize(Roles = "Teacher")]
        //[Authorize(Roles = "Student")]
        // GET: SectionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails.FindAsync(id);
            if (sectionDetails == null)
            {
                return NotFound();
            }
            return View(sectionDetails);
        }

        // POST: SectionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SectionId,Name,Title")] SectionDetails sectionDetails)
        {
            if (id != sectionDetails.SectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sectionDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionDetailsExists(sectionDetails.SectionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sectionDetails);
        }

        // GET: SectionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails
                .FirstOrDefaultAsync(m => m.SectionId == id);
            if (sectionDetails == null)
            {
                return NotFound();
            }

            return View(sectionDetails);
        }

        // POST: SectionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SectionDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SectionDetails'  is null.");
            }
            var sectionDetails = await _context.SectionDetails.FindAsync(id);
            if (sectionDetails != null)
            {
                _context.SectionDetails.Remove(sectionDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionDetailsExists(int id)
        {
          return (_context.SectionDetails?.Any(e => e.SectionId == id)).GetValueOrDefault();
        }





        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult>  GrantSectionAccess()
        {
            var viewModel = new AssignSectionViewModel();
            viewModel.SectionSelectList = _context.SectionDetails.ToList();
            //viewModel.AssignSection = new AssignSection();

            viewModel.Teachers = await _userManager.Users.Where(o => o.Role == "Teacher").ToListAsync();


            return View(viewModel);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantSectionAccess(AssignSection assignSection)
        {
            //var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            assignSection.UserId= assignSection.Teacher;   
            if (ModelState!=null)
            {
                bool isAlreadyAssigned = await CheckIfTeacherIsAssigned(assignSection.Teacher, assignSection.SectionId);

                if (isAlreadyAssigned)
                {
                    ModelState.AddModelError("", "Teacher is already assigned to this section.");
                }
                else
                {
                    _context.Add(assignSection);
                    await _context.SaveChangesAsync();
                    return Redirect("index");
                }
            }

            // Refresh available sections
            return View(assignSection);
        }

        private async Task<bool> CheckIfTeacherIsAssigned(string teacherId, int sectionId)
        {
            var existingAssignment = await _context.AssignSection
                .FirstOrDefaultAsync(a => a.Teacher == teacherId && a.SectionId == sectionId);

            return existingAssignment != null;
        }



    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context; // Data layer for database interaction
        private readonly ClaimService _claimService;    // Service for business logic (optional)

        // Constructor for dependency injection of DbContext and optional service
        public LecturerController(ApplicationDbContext context, ClaimService claimService)
        {
            _context = context;
            _claimService = claimService;
        }

        // GET: Lecturer/SubmitClaim
        public IActionResult SubmitClaim()
        {
            return View();
        }

        // POST: Lecturer/SubmitClaim
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Validate file size (limit to 5 MB)
                if (file != null && file.Length > 5 * 1024 * 1024) // 5 MB limit
                {
                    ModelState.AddModelError("File", "File size cannot exceed 5 MB.");
                    return View(claim);
                }

                // Validate file type (only .pdf, .docx, .xlsx)
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                if (file != null && !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    ModelState.AddModelError("File", "Unsupported file type.");
                    return View(claim);
                }

                // File handling via ClaimService or directly in the controller
                if (_claimService != null && file != null)
                {
                    // Calling ClaimService to handle file saving and claim submission
                    _claimService.SubmitClaim(claim, file);
                }
                else
                {
                    // Default behavior: Save the claim to the database
                    if (file != null && file.Length > 0)
                    {
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        Directory.CreateDirectory(uploadsPath); // Ensure the directory exists

                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        claim.FilePath = $"/uploads/{fileName}"; // Save relative path
                    }

                    claim.Status = "Pending"; // Default status
                    await _context.Claims.AddAsync(claim);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("ClaimSubmitted");
            }

            // If model state is invalid, return to the form view
            return View(claim);
        }

        // Confirmation page after submitting a claim
        public IActionResult ClaimSubmitted()
        {
            return View();
        }
    }
}

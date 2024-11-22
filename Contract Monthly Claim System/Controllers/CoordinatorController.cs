using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Hubs;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ClaimHub> _hubContext;

        public CoordinatorController(ApplicationDbContext context, IHubContext<ClaimHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: Coordinator/PendingClaims
        public IActionResult PendingClaims()
        {
            // Fetch all claims with status "Pending"
            var claims = _context.Claims.Where(c => c.Status == "Pending").ToList();
            return View(claims);
        }

        // POST: Coordinator/ApproveClaim
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            // Find claim by ID
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Approved";  // Set status to Approved
                await _context.SaveChangesAsync();  // Save changes to the database

                // Notify clients of the status change
                await _hubContext.Clients.All.SendAsync("ClaimStatusUpdated", claim.ClaimID, claim.Status);
            }
            return RedirectToAction("PendingClaims");  // Redirect back to the PendingClaims view
        }

        // POST: Coordinator/RejectClaim
        [HttpPost]
        public async Task<IActionResult> RejectClaim(int id)
        {
            // Find claim by ID
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";  // Set status to Rejected
                await _context.SaveChangesAsync();  // Save changes to the database

                // Notify clients of the status change
                await _hubContext.Clients.All.SendAsync("ClaimStatusUpdated", claim.ClaimID, claim.Status);
            }
            return RedirectToAction("PendingClaims");  // Redirect back to the PendingClaims view
        }
    }
}

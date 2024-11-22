using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Mvc;

public class ManagerController : Controller
{
    private readonly ApplicationDbContext _context;

    public ManagerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Example action to manage claims
    public IActionResult Dashboard()
    {
        // Fetch some data for the manager's dashboard
        var claims = _context.Claims.ToList();
        return View(claims);
    }

    // Additional actions for the manager as needed
}



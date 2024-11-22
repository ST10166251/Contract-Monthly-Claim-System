using Contract_Monthly_Claim_System.Models;

public class ClaimService
{
    private readonly ApplicationDbContext _context;

    public ClaimService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SubmitClaim(Claim claim, IFormFile file)
    {
        // Save claim to database and handle file upload
        if (file != null)
        {
            var filePath = UploadFile(file);
            claim.FilePath = filePath;
        }

        claim.Status = "Pending";  // Default status
        _context.Claims.Add(claim);
        _context.SaveChanges();
    }

    public IEnumerable<Claim> GetPendingClaims()
    {
        return _context.Claims.Where(c => c.Status == "Pending").ToList();
    }

    public void ApproveClaim(int id)
    {
        var claim = _context.Claims.Find(id);
        if (claim != null)
        {
            claim.Status = "Approved";
            _context.SaveChanges();
        }
    }

    public void RejectClaim(int id)
    {
        var claim = _context.Claims.Find(id);
        if (claim != null)
        {
            claim.Status = "Rejected";
            _context.SaveChanges();
        }
    }

    private string UploadFile(IFormFile file)
    {
        var filePath = Path.Combine("wwwroot", "uploads", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        return filePath;
    }
}



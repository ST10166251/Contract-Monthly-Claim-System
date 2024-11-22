using System;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claim
    {
        // Primary key for the claim
        public int ClaimID { get; set; }

        // Foreign key to link to the Lecturer
        public int LecturerID { get; set; }

        // Navigation property to Lecturer model
        public Lecturer Lecturer { get; set; }

        // Hours worked for the claim
        public decimal HoursWorked { get; set; }

        // Hourly rate for the claim
        public decimal HourlyRate { get; set; }

        // Total amount (calculated from hours worked and hourly rate)
        public decimal TotalAmount => HoursWorked * HourlyRate;

        // Status of the claim (Pending, Approved, Rejected)
        public string Status { get; set; }

        // Notes for the claim
        public string Notes { get; set; }

        // Date the claim was submitted
        public DateTime SubmissionDate { get; set; }

        // Path to the uploaded file (if applicable)
        public string FilePath { get; set; }
    }
}

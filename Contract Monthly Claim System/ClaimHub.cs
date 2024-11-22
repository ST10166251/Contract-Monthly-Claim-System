using Microsoft.AspNetCore.SignalR;

namespace Contract_Monthly_Claim_System.Hubs
{
    public class ClaimHub : Hub
    {
        // You can add methods here if needed for client-server communication
        public async Task NotifyClaimStatusChange(int claimId, string status)
        {
            await Clients.All.SendAsync("ClaimStatusUpdated", claimId, status);
        }
    }
}


// This script listens to the SignalR connection for claim status updates

$(document).ready(function () {
    // Create a connection to the SignalR hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/claimHub")  // Ensure the URL matches the SignalR hub URL in your app
        .build();

    // Start the connection
    connection.start()
        .then(function () {
            console.log("SignalR connection established.");
        })
        .catch(function (err) {
            return console.error("SignalR connection failed: " + err.toString());
        });

    // Define the event listener for claim status updates
    connection.on("ClaimStatusUpdated", function (claimId, newStatus) {
        // Find the table row corresponding to the claim ID
        const row = $("tr[data-claim-id='" + claimId + "']");

        // Update the status column for that claim
        if (row.length > 0) {
            row.find(".claim-status").text(newStatus);  // Update the status text
            row.attr("data-status", newStatus);         // Optionally update the row's data-status attribute

            // Optional: Add a visual cue like changing the row's color based on status
            if (newStatus === "Approved") {
                row.addClass("table-success");  // Green color for Approved
                row.removeClass("table-danger"); // Remove red color if rejected
            } else if (newStatus === "Rejected") {
                row.addClass("table-danger");   // Red color for Rejected
                row.removeClass("table-success"); // Remove green color if approved
            } else {
                row.removeClass("table-success table-danger"); // Reset colors for pending status
            }
        }
    });
});


const connection = new signalR.HubConnectionBuilder().withUrl("/hubs/adboard-status").configureLogging(signalR.LogLevel.Information).build();

connection.on("ReceiveAdboardStatus", (adboardId, status) => {
    console.log(`Adboard ${adboardId} connected: ${status}`);
    // Update the UI with the new status
    
});

connection.start()
    .then(() => {
        console.log("SignalR connection started successfully.");
    })
    .catch((err) => {
        console.error("Error starting SignalR connection: " + err);
    });
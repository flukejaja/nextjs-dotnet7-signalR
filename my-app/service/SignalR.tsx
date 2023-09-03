import * as signalR from "@microsoft/signalr";

export const connection = new signalR.HubConnectionBuilder()
.withUrl("http://localhost:5182/timer", {
  skipNegotiation: true,
  transport: signalR.HttpTransportType.WebSockets,
})
.configureLogging(signalR.LogLevel.Information)
.build();

connection.onclose(function () {
    console.log("connecition closed");
});





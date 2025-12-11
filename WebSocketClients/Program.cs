// .NET 9 Console App
// Make sure you install: dotnet add package Microsoft.AspNetCore.SignalR.Client

using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;

int clientCount = 700; // number of clients you want
var connections = Enumerable.Range(0, clientCount)
    .Select(i =>
    {
        var connection = new HubConnectionBuilder()
            //.WithUrl("http://localhost:5073/hubs/live-trade", options =>
            .WithUrl("http://20.207.238.194:5000/hubs/live-trade")
            .Build();

        connection.On<object>("CommunityChatUpdate", payload =>
        {
            Console.WriteLine($"Client {i} received: {System.Text.Json.JsonSerializer.Serialize(payload)}");
        });
        connection.ServerTimeout = TimeSpan.FromSeconds(60);
        connection.KeepAliveInterval = TimeSpan.FromSeconds(15);


        return (connection, i);
    })
    .ToList();

// Start all clients in parallel
await Task.WhenAll(connections.Select(c => c.connection.StartAsync()));
foreach (var c in connections)
{
    Console.WriteLine($"Client {c.i} connected.");
}

//Each client sends a message
//await Task.WhenAll(connections.Select(c =>
//    c.connection.InvokeAsync("SendMessage", $"Hello from client {c.i}")
//));

Console.WriteLine("Press Enter to exit...");
Console.ReadLine();

// Stop all clients in parallel
//await Task.WhenAll(connections.Select(c => c.connection.StopAsync()));

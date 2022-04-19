// See https://aka.ms/new-console-template for more information
using CommunicationLib.CommunictionServers;
using ServerAndClient.ChatServers;

Console.WriteLine("Hello, World!");
var chatServer = new ChatServer(new CommunictionServer());
Console.ReadLine();

// See https://aka.ms/new-console-template for more information
using CommunicationLib.CommunicationClients;
using ServerAndClient.ChatClients;

Console.WriteLine("Hello, World!");
var client = new ChatClient(new CommunicationClient());
client.ReceivedMessage += OnReceivedMessage;
client.ForcedOffline += OnForcedOffline;


client.UserName = "joey";
client.Online();
client.SendMessage("hello my name is joey");
Console.ReadLine();
client.Offline();


void OnReceivedMessage(object? sender, EventArgs e)
{
    Console.WriteLine("message count: " + client.HistoryMessageList?.Count);
    foreach (var message in client.HistoryMessageList)
    {
        Console.WriteLine(message.SendTime + " " + message.UserName + " " + message.Content);
    }
}

void OnForcedOffline(object? sender, EventArgs e)
{
    Console.WriteLine("disconnected from server");
}
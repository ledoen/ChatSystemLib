namespace CommunicationLib.CommunicationClients
{
    public interface ICommunicationClient
    {
        event EventHandler ConnectionDisconnected;

        event EventHandler<string> ReceivedDataFromServer;

        bool ConnectToServer();

        void DisConnectToServer();

        void SendDataToServer(string data);
    }
}
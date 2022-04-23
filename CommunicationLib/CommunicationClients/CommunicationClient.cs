using CommunicationLib.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommunicationLib.CommunicationClients
{
    public class CommunicationClient : ICommunicationClient
    {
        private const string serverAddress = "192.168.50.227";

        private const int serverPort = 11000;

        private Socket? _socket;

        public event EventHandler ConnectionDisconnected;

        public event EventHandler<string> ReceivedDataFromServer;
        public bool ConnectToServer()
        {
            if (_socket?.Connected == true)
                throw new Exception("Alread connected");
            // 【1】初始化socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 【2】连接到服务器
            var res = _socket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverAddress), serverPort)).Wait(1000);
            if (res == false) return false;
            // 【3】开始异步接收
            BeginReceive();
            return true;
        }

        public void DisConnectToServer()
        {
            if (_socket?.Connected == true)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }

        public void SendDataToServer(string data)
        {
            if (_socket?.Connected == false) return;

            // 【1】处理数据，即添加尾部
            data = CommunictionDataHelpers.AddTail(data);

            // 【2】发送该数据
            _socket.Send(Encoding.UTF8.GetBytes(data));
        }

        private void BeginReceive()
        {
            var state = new StateObject();
            _socket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceivedCallback), state);
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            // 【1】获取接收字节数
            var state = (StateObject)ar.AsyncState;
            try
            {
                var bytesReceived = _socket.EndReceive(ar);

                if (bytesReceived > 0)
                {
                    HandleReceivedData(state, bytesReceived);
                }
                else
                {
                    HandleConnectionDisconnected();
                }
            }
            catch (Exception)
            {
                HandleConnectionDisconnected();
            }
        }

        private void HandleReceivedData(StateObject? state, int bytesReceived)
        {
            state.ReceivedSb.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesReceived));
            // 【2】判断接收是否结束
            var content = state.ReceivedSb.ToString();
            if (content.Contains("<EOF>") == true)
            {
                HandleTransferFinshed(state, content);
            }
            // 【3】继续接收
            _socket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceivedCallback, state);
        }

        private void HandleConnectionDisconnected()
        {
            // 【1】服务器主动关闭连接,客户端也关闭连接，并触发事件
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            ConnectionDisconnected?.Invoke(this, null);
        }

        private void HandleTransferFinshed(StateObject? state, string content)
        {
            // 【2.1】结束，触发接收事件并传递信息
            content = CommunictionDataHelpers.RemoveTail(content);
            ReceivedDataFromServer?.Invoke(this, content);

            // 【2.1.1】重置缓存
            state.ReceivedSb.Clear();
        }

    }
}
using CommunicationLib.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommunicationLib.CommunictionServers
{
    public class CommunictionServer : ICommunicationServer
    {
        private readonly ManualResetEvent _acceptDone = new(false);
        private readonly Dictionary<int, Socket> _clientWorkers = new();

        public event EventHandler<int> ClientConnected;

        public event EventHandler<int> ClientDisconnected;

        public event EventHandler<(int, string)> ReceivedDataFromClient;

        public void SendDataToClients(string data)
        {
            foreach (var worker in _clientWorkers.Values)
            {
                worker.SendTo(Encoding.UTF8.GetBytes(data), worker.RemoteEndPoint);
            }
        }

        public void StartListening(int port)
        {
            // 【1】新建socket并绑定到本地端点
            // 【1.1】获取本地IP
            IPAddress? localAddress = NetworkHelpers.GetLocalIPAddress();
            if (localAddress == null)
                throw new Exception("No available IP Address");
            // 【1.2】新建并绑定socket
            var listener = new Socket(localAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(new IPEndPoint(localAddress, port));
                // 【2】开始接收客户端连接
                listener.Listen(100);

                while (true)
                {
                    _acceptDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    _acceptDone.WaitOne();
                }
            }
            catch (SocketException ex)
            {
                throw;
            }
        }

        public void TailAndSendDataToClients(string data)
        {
            data = CommunictionDataHelpers.AddTail(data);
            SendDataToClients(data);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _acceptDone.Set();
            // 【1】获取listener socket
            var listener = (Socket)ar.AsyncState;
            // 【2】获取worker socket
            var worker = listener.EndAccept(ar);
            // 【3】将上线客户端加入到地址薄
            AddNewClientWorker(worker);
            // 【4】触发客户上线事件，如果上线事件由客户端主动上报，则不需要触发该事件
            ClientConnected?.Invoke(this, worker.GetHashCode());
            // 【5】开始接收数据
            BeginReceiveData(worker);
        }

        private void BeginReceiveData(Socket worker)
        {
            var stateObject = new StateObject { Worker = worker };
            worker.BeginReceive(stateObject.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceivedCallback), stateObject);
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            // 【1】获取stateObject
            var stateObject = (StateObject)ar.AsyncState;
            // 【2】获取worker
            var worker = stateObject.Worker;
            try
            {
                // 【3】获取接收字节数
                var receivedBytes = worker.EndReceive(ar);

                if (receivedBytes > 0)
                {
                    HandleReceiveBytes(stateObject, receivedBytes);
                }
                else
                    HandleDisconnectClientWorker(worker);
            }
            catch (Exception)
            {
                HandleDisconnectClientWorker(worker);
            }
        }

        private void HandleReceiveBytes(StateObject? stateObject, int receivedBytes)
        {
            // 【4】接收到数据
            stateObject.ReceivedSb.Append(Encoding.UTF8.GetString(stateObject.Buffer, 0, receivedBytes));
            // 【4.1】判断接收是否接收
            var content = stateObject.ReceivedSb.ToString();
            if (content.Contains("<EOF>"))
            {
                HandleFinshedAMessage(stateObject, content);
            }
            // 【4.2】继续接收
            stateObject.Worker.BeginReceive(stateObject.Buffer, 0, StateObject.BufferSize, 0, ReceivedCallback, stateObject);
        }

        private void HandleFinshedAMessage(StateObject? stateObject, string content)
        {
            // 【4.1.1】接收结束
            // 【4.1.1.1】重置stateObject的缓存，准备下一次数据接收
            stateObject.ReceivedSb.Clear();
            // 【4.1.1.2】 处理数据，并触发接收到数据事件
            ReceivedDataFromClient?.Invoke(this, (stateObject.Worker.GetHashCode(), content));
        }

        private void HandleDisconnectClientWorker(Socket worker)
        {
            // 【5】客户端下线
            // 【5.1】将客户端worker移除
            RemoveClientWorker(worker);
            // 【5.2】触发离线事件
            ClientDisconnected.Invoke(this, worker.GetHashCode());
            // 【5.3】关闭worker
            worker.Shutdown(SocketShutdown.Both);
            worker.Close();
        }

        #region client work socket list manage

        private void AddNewClientWorker(Socket worker)
        {
            _clientWorkers.Add(worker.GetHashCode(), worker);
        }

        private void RemoveClientWorker(Socket worker)
        {
            _clientWorkers.Remove(worker.GetHashCode());
        }

        #endregion client work socket list manage
    }
}
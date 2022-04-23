using CommunicationLib.Common;
using ServerAndClient.Models;

namespace ServerAndClient.Libraries
{
    internal static class ChatMessageHelpers
    {
        public static ChatMessage ResolveMessageFromReceivedData(string data)
        {
            // 【1】去尾
            var messageStr = CommunictionDataHelpers.RemoveTail(data);
            // 【2】提取
            return messageStr.FromJson<ChatMessage>();
        }
    }
}
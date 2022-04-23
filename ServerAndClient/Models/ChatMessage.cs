namespace ServerAndClient.Models
{
    public class ChatMessage
    {
        public ChatMessage()
        {
        }

        public ChatMessage(string user, string content)
        {
            SendTime = DateTime.Now.ToString();
            UserName = user;
            Content = content;
        }

        public string Content { get; set; }
        public string SendTime { get; set; }
        public string UserName { get; set; }
    }
}
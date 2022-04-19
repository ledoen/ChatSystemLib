namespace CommunicationLib.CommunictionServers
{
    internal static class CommunictionDataHelpers
    {

        public static string AddTail(string data)
        {
            return data + "<EOF>";
        }

        public static string RemoveTail(string content)
        {
            return content.Replace("<EOF>", "");
        }
    }
}
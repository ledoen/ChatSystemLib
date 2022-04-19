namespace CommunicationLib.Common
{
    public static class CommunictionDataHelpers
    {

        public static string AddTail(string data)
        {
            return data + "<EOF>";
        }

        public static string RemoveTail(string content)
        {
            if (content == null)
            {
                return null;
            }
            return content.Replace("<EOF>", "");
        }
    }
}
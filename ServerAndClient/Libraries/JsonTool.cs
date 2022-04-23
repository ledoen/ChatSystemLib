using Newtonsoft.Json;

namespace ServerAndClient.Libraries
{
    internal static class JsonTool
    {
        public static T FromJson<T>(this string inputStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(inputStr);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string ToJSON(this object obj)
        {
            if (obj == null) return null;

            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
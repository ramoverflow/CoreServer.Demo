using Newtonsoft.Json;

namespace CoreServer.Common.Util
{
    public static class JsonUtil
    {
        public static string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static T ToObj<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
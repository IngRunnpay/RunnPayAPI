using Newtonsoft.Json;
using System.Collections.Generic;

namespace System
{
    public static class MyExtensions
    {

        private static Dictionary<string, string> UrlErrorCharacteresReverse = new Dictionary<string, string>()
        {
            { "kw8SD9C" , "%1"},
            { "Fh8TQ2P" , "%2"},
            { "dW8Y7eX" , "%3"},
            { "5S8bLw4" , "%4"},
            { "M98Gjrp" , "%5"},
            { "qo8V2EZ" , "%6"},
            { "cx86zKt" , "%7"},
            { "3U8y7YH" , "%8"},
            { "H88gNTZ" , "%9"},
            { "Vm8K9rC" , "%0"}
        };

        private static Dictionary<string, string> UrlErrorCharacteresNormal = new Dictionary<string, string>()
        {
            {"%1", "kw8SD9C"},
            {"%2", "Fh8TQ2P"},
            {"%3", "dW8Y7eX"},
            {"%4", "5S8bLw4"},
            {"%5", "M98Gjrp"},
            {"%6", "qo8V2EZ"},
            {"%7", "cx86zKt"},
            {"%8", "3U8y7YH"},
            {"%9", "H88gNTZ"},
            {"%0", "Vm8K9rC"}
        };
        public static string ToHtmlStringList(this List<string> data)
        {
            string result = $"<ul>[List]</ul>";
            if(data != null)
            {
                if (data.Count > 0)
                {
                    foreach(var dt in data)
                    {
                        result = result.Replace("[List]", $"<li>{dt}</li> {Environment.NewLine} [List]");
                    }
                }
            }
            result = result.Replace("[List]","");

            return result;
        }
        public static string ToHtmlStringList(this List<Tuple<string, string>> data)
        {
            string result = $"<ul>[List]</ul>";
            if (data != null)
            {
                if (data.Count > 0)
                {
                    foreach (var dt in data)
                    {
                        result = result.Replace("[List]", $"<li>{dt.Item1 + "<br>" + dt.Item2}</li> {Environment.NewLine} [List]");
                    }
                }
            }
            result = result.Replace("[List]", "");

            return result;
        }
        public static T MapObject<T>(this object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static string UrlErrorCharacteresEncode(this string input, bool reverse = false)
        {
            var list = reverse ? UrlErrorCharacteresReverse : UrlErrorCharacteresNormal;
            foreach (var item in list)
            {
                input = input.Replace(item.Key, item.Value);
            }
            return input;
        }
    }
}
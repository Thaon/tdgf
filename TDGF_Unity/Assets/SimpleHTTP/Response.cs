using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;

namespace SimpleHTTP
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string fixJson(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

    }

    public class Response
    {
        private long status;
        private string body;
        private byte[] rawBody;

        public Response(long status, string body, byte[] rawBody)
        {
            this.status = status;
            this.body = body;
            this.rawBody = rawBody;
        }

        public T To<T>()
        {
            return JsonUtility.FromJson<T>(body);
        }

        public class RecordsList<T>
        {
            public List<T> records = new List<T>();
        }

        public List<T> ToArray<T>()
        {
            var records = new RecordsList<T>();
            var fixedJson = JsonHelper.fixJson(body);
            records.records = JsonHelper.FromJson<T>(fixedJson).ToList();
            return records.records;
        }


        public long Status()
        {
            return status;
        }

        public string Body()
        {
            return body;
        }

        public byte[] RawBody()
        {
            return rawBody;
        }

        public bool IsOK()
        {
            return status >= 200 && status < 300;
        }

        public new string ToString()
        {
            return "status: " + status.ToString() + " - response: " + body.ToString();
        }

        public static Response From(UnityWebRequest www)
        {
            return new Response(www.responseCode, www.downloadHandler.text, www.downloadHandler.data);
        }
    }
}

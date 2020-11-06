using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StraviaTecMovil.Helpers.Network
{
    public class Response
    {
        public HttpStatusCode Code { get; set; }
        public bool Succeeded { get; set; }
        public string ResponseBody { get; set; }

        public Response(HttpStatusCode code, bool success, string body)
        {
            Code = code;
            Succeeded = success;
            ResponseBody = body;
        }

        public T ParseBody<T>()
        {
            try
            {
                var instance = JsonConvert.DeserializeObject<T>(ResponseBody);
                return instance;
            } catch (JsonException)
            {
                return default;
            }
        }
    }
}

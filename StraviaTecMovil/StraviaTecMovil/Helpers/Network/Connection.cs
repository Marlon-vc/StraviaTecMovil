using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StraviaTecMovil.Helpers.Network
{
    public static class Connection
    {
        private enum Method
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        private static string MediaType => "application/json";

        public static async Task<Response> Get(string url, int timeout = 30)
        {
            return await MakeRequest(Method.GET, url, null, timeout)
                .ConfigureAwait(false);
        }

        public static async Task<Response> Post(string url, string bodyContent, int timeout = 30)
        {
            return await MakeRequest(Method.POST, url, bodyContent, timeout)
                .ConfigureAwait(false);
        }

        public static async Task<Response> Put(string url, string bodyContent, int timeout = 30)
        {
            return await MakeRequest(Method.PUT, url, bodyContent, timeout)
                .ConfigureAwait(false);
        }

        public static async Task<Response> Delete(string url, int timeout = 30)
        {
            return await MakeRequest(Method.DELETE, url, null, timeout)
                .ConfigureAwait(false);
        }

        private static async Task<Response> MakeRequest(Method method, string url, string bodyContent, int timeout)
        {
            using (var client = new HttpClient())
            using (var ts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout)))
            {
                try
                {
                    StringContent body = null;
                    HttpResponseMessage response = null;

                    switch (method)
                    {
                        case Method.GET:
                            response = await client.GetAsync(url, ts.Token)
                                .ConfigureAwait(false);
                            break;
                        case Method.POST:
                            body = new StringContent(bodyContent, Encoding.UTF8, MediaType);
                            response = await client.PostAsync(url, body, ts.Token)
                                .ConfigureAwait(false);
                            break;
                        case Method.PUT:
                            body = new StringContent(bodyContent, Encoding.UTF8, MediaType);
                            response = await client.PutAsync(url, body, ts.Token)
                                .ConfigureAwait(false);
                            break;
                        case Method.DELETE:
                            response = await client.DeleteAsync(url, ts.Token)
                                .ConfigureAwait(false);
                            break;
                    }

                    var content = await response.Content.ReadAsStringAsync()
                        .ConfigureAwait(false);

                    return new Response(response.StatusCode, response.IsSuccessStatusCode, content);
                }
                catch (TaskCanceledException ex)
                {
                }
                catch (HttpRequestException ex)
                {
                }
                catch (ArgumentNullException ex)
                {
                }
                catch (Exception ex)
                {
                }
                return new Response(HttpStatusCode.BadRequest, false, null);
            }
        }
    }
}

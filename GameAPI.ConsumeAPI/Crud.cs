using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameAPI.ConsumeAPI
{
    public class Crud<T>
    {
        public static T Create(string apiUrl, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }

        public static T[] Read_All(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetStringAsync(apiUrl);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T[]>(json);

                return result;
            }
        }

        public static T Read_ById(string apiUrl, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                apiUrl = apiUrl + "/" + id;
                var response = client.GetStringAsync(apiUrl);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }

        public static bool Update(string apiUrl, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                apiUrl = apiUrl + "/" + id;
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                return !response.Result.Content.ReadAsStringAsync().IsFaulted;
            }
        }

        public static bool Delete(string apiUrl, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                apiUrl = apiUrl + "/" + id;
                var response = client.DeleteAsync(apiUrl);
                response.Wait();

                return !response.Result.Content.ReadAsStringAsync().IsFaulted;
            }
        }
    }
}

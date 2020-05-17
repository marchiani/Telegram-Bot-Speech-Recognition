using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TataBot.Common.Helpers
{
    public static class ZamzarHelper
    {
        public static async Task<T> GetSimpleResponse<T>(string key, string url)
        {
            using var handler = new HttpClientHandler { Credentials = new NetworkCredential(key, "") };
            using var client = new HttpClient(handler);
            using var response = await client.GetAsync(url);
            using var content = response.Content;
            return JsonHelper.GetObject<T>(await content.ReadAsStringAsync());
        }

        public static async Task<T> Upload<T>(string key, string url, string sourceFile, string targetFormat)
        {
            using var handler = new HttpClientHandler { Credentials = new NetworkCredential(key, "") };
            using var client = new HttpClient(handler);
            var request = new MultipartFormDataContent
            {
                { new StringContent(targetFormat), "target_format" },
                { new StreamContent(File.OpenRead(sourceFile)), "source_file", new FileInfo(sourceFile).Name }
            };
            using var response = await client.PostAsync(url, request).ConfigureAwait(false);
            using var content = response.Content;
            return JsonHelper.GetObject<T>(await content.ReadAsStringAsync());
        }

        public static async Task Download(string key, string url, string file)
        {
            using var handler = new HttpClientHandler { Credentials = new NetworkCredential(key, "") };
            using var client = new HttpClient(handler);
            using var response = await client.GetAsync(url);
            using var content = response.Content;
            await using var stream = await content.ReadAsStreamAsync();
            await using var writer = File.Create(file);
            await stream.CopyToAsync(writer);
        }
    }
}
namespace EddieBlazorResume.Client.Services
{   
    public class HttpClientHelper
    {
        private readonly HttpClient _client;
        private string? responseString = null;

        public HttpClientHelper(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> HttpGetAsync(string requestUri)
        {
            {
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync(requestUri);
                    responseString = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return responseString;
        }
    }

}
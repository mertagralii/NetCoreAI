using NetCoreIA.Project03.RapidApi.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

var client = new HttpClient(); 

List<ApiSeriesViewModel> apiSeriesViewModels = new List<ApiSeriesViewModel>(); 

var request = new HttpRequestMessage // Request oluşturuyoruz. (İstek Oluşturuyoruz.)
{
    Method = HttpMethod.Get, // Get methodunu kullanıyoruz.
    RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/"), // Api adresi
    Headers =
    {
        { "x-rapidapi-key", "797184eba5msh2a415fa0f15e8a3p1f3062jsn3314467a8846" }, // Api key
        { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" }, // Api host
    },
};

using (var response = await client.SendAsync(request)) // Requesti gönderiyoruz.
{
    response.EnsureSuccessStatusCode(); // Hata kontrolü yapıyoruz.

    var body = await response.Content.ReadAsStringAsync();  // Response body'sini okuyoruz.

    apiSeriesViewModels = JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body); // Json veriyi ApiSeriesViewModel listesine çeviriyoruz.

    foreach (var series in apiSeriesViewModels) // ApiSeriesViewModel listesini dönüyoruz.
    {
        Console.WriteLine(series.rank +" - " +series.title + " Film Puanı : " + series.rating + " Yapım Yılı : " + series.year); // Verileri yazdırıyoruz.
    }
    
}
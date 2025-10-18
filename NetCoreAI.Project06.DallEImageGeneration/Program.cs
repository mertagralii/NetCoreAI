
using System.Text;
using Newtonsoft.Json;

class Program
{
    public static async Task Main(string[] args)
    {
        // Bu proje bizim girdiğimiz komutları kullanarak DALL·E API'si üzerinden resim oluşturacak.
        string apiKey = "Api Anahtarı Buraya Gelecek";  // OpenAI API anahtarınızı buraya ekleyin.
        Console.WriteLine("Example DALL·E Image Generation using .NET Core AI"); // Proje başlığı
        string promt = Console.ReadLine(); // Kullanıcıdan resim oluşturmak için bir komut alıyoruz.
        using (var client = new HttpClient()) // HttpClient nesnesi oluşturuyoruz.
        {
            client.DefaultRequestHeaders.Add("Authorization",$"Bearer {apiKey}"); // Yetkilendirme başlığı ekliyoruz.
            var requestBody = new // İstek gövdesi oluşturuyoruz.
            {
                prompt = promt, // Kullanıcının girdiği komut
                n = 1, // Oluşturulacak resim sayısı
                size = "1024x1024" // Resim boyutu
            };
            string jsonBody = JsonConvert.SerializeObject(requestBody); // İstek gövdesini JSON formatına dönüştürüyoruz.
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // İçerik türünü belirtiyoruz.
            HttpResponseMessage response = await client.PostAsync($"http://api.openai.com/v1/images/generations", content); // POST isteği gönderiyoruz.
            string responseString = await response.Content.ReadAsStringAsync(); // Yanıtı okuyoruz.
            Console.WriteLine(responseString); // Burada gelen yanıt bize bir resim URL'si verecek. Bu url üzerinden resmi görüntüleyebiliriz.
        }
    }
}
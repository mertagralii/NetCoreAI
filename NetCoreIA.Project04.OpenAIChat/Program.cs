using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // Bu proje OpenAI ChatGPT API'sini kullanarak kullanıcıdan aldığı soruya cevap verecek.
        // Döküman Linki : https://platform.openai.com/docs/overview
        // Apikey' i kullanabilmek için ödeme yapılması gerekiyor. Gerekli Linkleri aşağıda : 
        // Ana Sayfa : https://platform.openai.com/usage
        // Anahtar oluşturma yeri : https://platform.openai.com/api-keys
        // Ödeme yeri : https://platform.openai.com/settings/organization/billing/overview
        
        // Aşağıdaki OpenAI'dan aldığımız keyi giriyoruz.
        var apiKey = "BURAYA OPENAI'DAN ALDIĞIMIZ KEY GELECEK";
        // Ekrana yazı yazdırdık
        Console.WriteLine("Lütfen Sorunuzu yazınız:(Örnek : İstanbulda bugün hava kaç derece?)");
        var prompt = Console.ReadLine(); // Kullanıcının yazdığı mesaj
        using var httpclient = new HttpClient(); // Yeni bir client açtık
        httpclient.DefaultRequestHeaders.Add("Authorization",$"Bearer {apiKey}"); // Bu başlangıçta zorunlu
        var requestBody = new
        {
            model = "gpt-3.5-turbo", // hangi modeli kullanacağız
            messages = new[] // mesaj içeriği kısmı
            {
                new { role = "system", content = "You are a helpfull assistant" }, // yapay zeka mesajı
                new{ role = "user", content = prompt} // kullanıcı mesajı
            },
            max_tokens = 500 // max kaç karakterlik yazı
        };

        var json = JsonSerializer.Serialize(requestBody); // göndereceğimiz modelimizi  json'a çevirdik
        var content = new StringContent(json, Encoding.UTF8, "application/json"); //  buraya modelimizi, türkçe karakter kısmını yazıyoruz.

        try
        {
            var response = await httpclient.PostAsync("https://api.openai.com/v1/chat/completions", content); // İstek atılacak yer ve isteğe gidecek model
            var responseString = await response.Content.ReadAsStringAsync(); // Openai'in cevabını okuyoruz.
            if (response.IsSuccessStatusCode) // Eğer doğru ise aşağıdaki işlemler yapılıyor.
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseString); // OpenAİ'dan gelen json'u dönüştürüyoruz.
                var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString(); // gelen mesajdan 2 seçenek arasından ilk geleni
                                                                                                                         // ve içinden mesaj kısmını ve contentini aldık
                Console.WriteLine("OpenAI'in Cevabı : "); // Ekrana yazı
                Console.WriteLine(answer); // OpenAI'dan gelen cevabı yazdırdık.
            }
            else
            {
                Console.WriteLine($"Bir Hata Oluştu :{response.StatusCode}");
                Console.WriteLine(responseString);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir Hata Oluştu :{ex.Message}");
        }
        


    }
}
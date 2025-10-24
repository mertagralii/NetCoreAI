

using System.Text.Json.Serialization;
using Newtonsoft.Json;

class Program
{
    // Bu örnek, OpenAI'nin GPT-3.5-turbo modelini kullanarak verilen metni İngilizce'ye çevirir.
    // NOT : Api keyini almak için aşağıdaki linkten OpenAI hesabı oluşturup api key alabilirsiniz.
    // https://platform.openai.com/account/api-keys
    // Projeyi çalıştırmadan önce "YOUR_OPENAI" kısmına kendi OpenAI API anahtarınızı eklemeyi unutmayın.
    // Ayrıca Newtonsoft.Json paketinin yüklü olduğundan emin olun. Yüklemek için:
    // dotnet add package Newtonsoft.Json
    // NOT : Api keyi oluşturduktan sonra openai hesabımızda kredimizin olması gerekiyor. Aksi takdirde isteklerimiz başarısız olur.
    private async static Task Main(string[] args) 
    {
        Console.WriteLine("Çevirilmesini istediğiniz metni giriniz:");
        string inputText = Console.ReadLine();
        string apiKey = "YOUR_OPENAI"; // OpenAI API anahtarınızı buraya ekleyin.
        string translatedText = await TranslateTexttoEnglish(inputText, apiKey); // Kullanıcının yazdığı mesajı ve api anahtarını fonksiyona gönderiyoruz.
        if(!string.IsNullOrEmpty(translatedText)) // Çeviri başarılı ise sonucu ekrana yazdırıyoruz.
        {
            Console.WriteLine("Çeviri Sonucu:");
            Console.WriteLine(translatedText); 
            Console.WriteLine();
        }else // Çeviri başarısız ise hata mesajı gösteriyoruz.
        {
            Console.WriteLine("Çeviri başarısız oldu.");
        }

    }
    
    private async static Task<string> TranslateTexttoEnglish(string inputText, string apiKey) // Metni İngilizce'ye çeviren fonksiyon
    {
        using (HttpClient client = new HttpClient()) // HttpClient nesnesi oluşturuyoruz.
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}"); // API anahtarını yetkilendirme başlığına ekliyoruz.
            var requestBody = new // İstek gövdesini oluşturuyoruz.
            {
                model = "gpt-3.5-turbo", // Kullanılacak model
                messages = new[] // Mesaj dizisi
                {
                    // Sistem mesajı: Asistanın rolünü belirtiyoruz.
                    new { role = "system", content = "You are a helpful assistant that translates text to English." }, // Sistem mesajı
                    new { role = "user", content = $"Translate the following text to English: {inputText}" } // Kullanıcının mesajı
                }
            };
            string jsonRequestBody = JsonConvert.SerializeObject(requestBody); // İstek gövdesini JSON formatına dönüştürüyoruz.
            var content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json"); // İstek içeriğini oluşturuyoruz.
            try
            {
                // API'ye POST isteği gönderiyoruz.
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseString = await response.Content.ReadAsStringAsync(); // Yanıt içeriğini okuyoruz.
                dynamic responseObject = JsonConvert.DeserializeObject(responseString); // Yanıtı dinamik bir nesneye dönüştürüyoruz.
                string translatedText = responseObject.choices[0].message.content; // Çevrilmiş metni alıyoruz.
                return translatedText; // Çevrilmiş metni döndürüyoruz.
            }
            catch (Exception e) // Hata yakalama bloğu
            {
                Console.WriteLine(@"Bir Hata Oluştu : " + e.Message); // Hata mesajını ekrana yazdırıyoruz.
                return null;
            }
           
           
        }
    }
    
}
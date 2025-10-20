using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        // Bu proje OpenAI Whisper API'sini kullanarak bir ses dosyasını metne dönüştürecek (transkript).
        // Proje notu: çalıştırmadan önce `audio1.mp3` dosyasının build çıktısına kopyalandığından ve `apiKey` değerinin güvenli şekilde sağlandığından emin olun.
        string apiKey = "Api Key Buraya gelecek"; // OpenAI'dan alınan secret key (gerçek projide ortam değişkeni kullanın)
        var audioPathFile = "audio1.mp3"; // İşlenecek ses dosyasının yolu veya adı

        using (var client = new HttpClient()) // HttpClient örneği oluştur (IDisposable, using ile kapatılır) 
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); // Bearer token ile yetkilendir

            var form = new MultipartFormDataContent(); // Multipart/form-data içeriği oluştur (dosya + alanlar için)
            var audioContent = new ByteArrayContent(File.ReadAllBytes(audioPathFile)); // Dosyayı byte dizisi olarak oku
            audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg"); // Dosyanın MIME tipini ayarla (mp3 için audio/mpeg)
            form.Add(audioContent, "file", Path.GetFileName(audioPathFile)); // Dosyayı "file" alanı ile form'a ekle (alan adı küçük harf olmalı)
            form.Add(new StringContent("whisper-1"), "model"); // Kullanılacak modeli belirt (Whisper için "whisper-1")

            Console.WriteLine("Ses Dosyayı Yükleniyor. Lütfen Bekleyiniz..."); // Kullanıcıyı bilgilendir

            var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form); // POST isteği gönder (URL'de ':' eksik olmamalı)

            if (response.IsSuccessStatusCode) // Başarılı durum kontrolü
            {
                var result = await response.Content.ReadAsStringAsync(); // API cevabını oku (JSON dönebilir)
                Console.WriteLine("Transcript:"); // Başlık yazdır
                Console.WriteLine(result); // Transkript veya JSON çıktısını yazdır
            }
            else
            {
                Console.WriteLine($"Hata:{response.StatusCode}"); // Hata durumunda HTTP durum kodunu yazdır
                Console.WriteLine(await response.Content.ReadAsStringAsync()); // Hata detaylarını yazdır (loglama/inceleme için)
            }
        }

    }
}
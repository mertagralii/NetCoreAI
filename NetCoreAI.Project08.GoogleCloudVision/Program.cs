using Google.Cloud.Vision.V1;

class Program
{
    static void Main(string[] args)
    {
        // Bu proje Google Cloud Vision ile Resim Üzerindeki Yazıları Okumayı Sağlıyor.
        // Kurduğumuz NuGet Paketi : Google Cloud Vision
        // İlk önce aşağıdaki linke giriş yapıcaksın
        // https://console.cloud.google.com/welcome
        // Giriş yapıp Sol üst taraftan da projemizi oluşturduktan sonra yapman gerekenler
        // Arama kısmına APIs & Services yaz. 
        // Arından APIs & Services kısmındaki Credentials kısmına tıkla.
        // Yukardaki Create Credentials 'a tıkla ve çıkan seçeneklerden Service account'u seç
        // Çıkan ekranda Create service account' kısmında oluşturacağın servisin adını gir ve Create and Contine'e tıkla (ID oluşturucak)
        // Permissions kısmına gelince de Project içerisinden Owner'i seç ve devam et.
        // Principals with access kısmını boş geç ve Done'e bas.
        // Tüm bu işlemleri yaptıktan sonra seni tekrardan Credentials sayfasına yönlendirecek oradan en alt kısımda Service Accounts' kısmından oluşturduğun servisi göreceksin
        // Service Accounts kısmında sayfanın en sağında Manage service accounts var ona tıkla.
        // Ardından oluşturduğun servisin Actions kısmındaki üç noktaya tıkla ve manage key'i seç.
        // Gelen ekrandan  Add Key 'e tıklayıp Create new key'i seç ve çıkan ekrandan da json dosyasını seçip Create'e bas.
        // Tüm bü işlemlerin sonunda sana bir json dosyası gelicek bu dosyayı projene koy ve ardından kod kısmında yolunu göstermem gereken kısma yüklediğin dosyanın yolunu yaz.
        // Sonrasında burdan console.developers.google.com/apis/api/vision.googleapis.com/overview?project=(projeId'si) oluşturduğun api servisini aktif et ve artık Cloud vision'un hazır.
        
        // DİPNOT : Google Vision'u kullanabilmen için  Fatura Bilgilerini eklemen gerekiyor.  Kısaca ödeme yapacağın kart bilgilerini girmelisin.
        
        
        Console.Write("Resim Yolunu Giriniz:"); // Örnek : C:\Users\username\Pictures\resim.jpg
        string imagePath = Console.ReadLine(); // Resim yolunu kullanıcıdan alıyoruz
        Console.WriteLine(); // Boşluk bırakmak için
        
        string credentialPath = @"Google Cloud Vision Api dosyası JSON DOSYASININ YOLU GELİCEK BURAYA"; // Örnek : C:\Users\username\source\repos\NetCoreIA.Project08.GoogleCloudVision\google-credentials.json
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath); // Ortam değişkeni olarak kimlik bilgilerini ayarlıyoruz

        try
        {
            var client = ImageAnnotatorClient.Create(); // ImageAnnotatorClient nesnesini oluşturuyoruz
            var image = Image.FromFile(imagePath); // Resmi dosyadan yüklüyoruz
            var response = client.DetectText(image); // Resim üzerindeki metinleri algılıyoruz
            Console.WriteLine("Resimdeki Metinler:"); // Başlık yazdırıyoruz
            foreach (var annotation in response) // Algılanan metinleri döngü ile geziyoruz
            {
                if (!string.IsNullOrEmpty(annotation.Description)) // Metin boş değilse
                {
                    Console.WriteLine(annotation.Description); // Metni ekrana yazdırıyoruz
                }
            }
        }
        catch (Exception e) // Hata yakalama bloğu
        {
            
            Console.WriteLine("Bir hata oluştu: " + e.Message); // Hata mesajını ekrana yazdırıyoruz
            throw;
        }
    }
}
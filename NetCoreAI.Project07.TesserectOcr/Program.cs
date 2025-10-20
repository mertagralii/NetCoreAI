using Tesseract;

class  Program
{
    static void Main(string[] args)
    {
        //Bu Projede Tesseract OCR ile Resim üzerindeki yazıları okuma işlemi yapılıyor..
        // Bu projeye başlamadan önce Nuget Package Manager üzerinden Tesseract paketini yüklemeniz gerekiyor.
        // Ayrıca Tesseract'ın dil dosyalarını indirip projenizde kullanmanız gerekiyor. (İndirdikten sonra "tessdata" klasörünü projenize ekleyin)
        // NOT : Ben Teseract dosyasını indirip C:\tessdata klasörüne attım. Sizde istediğiniz bir yere atabilirsiniz. Sadece kodda o yolu belirtmeniz gerekiyor.
        
        Console.Write("Karakter Okuması Yapılacak Resim Yolu:"); // Giriş yazısı
        string imagePath = Console.ReadLine(); // Kullanıcı okunmasını istediği resmin yolunu girer
        Console.WriteLine(); // Boşluk bırakır
        string testDataParh = @"C:\tessdata"; // Tesseract dil dosyalarının yolu

        try
        {
            using (var engine = new TesseractEngine(testDataParh, "eng", Tesseract.EngineMode.Default)) // Tesseract motorunu başlatır
            {
                using (var image = Pix.LoadFromFile(imagePath)) // Kullanıcının belirttiği resmi yükler
                {
                    using (var page = engine.Process(image)) // Resmi işler
                    {
                        var text = page.GetText(); // Resimden metni alır
                        Console.WriteLine("Okunan Metin:"); // Başlık yazısı
                        Console.WriteLine(text); // Okunan metni ekrana yazdırır
                    }
                }
            }
        }
        catch (Exception e) // Hata yakalama bloğu
        {
           Console.WriteLine($"Bir Hata Oluştu: {e.Message}"); // Hata mesajını ekrana yazdırır
        }

        Console.ReadLine(); // Programın hemen kapanmaması için
    }
}
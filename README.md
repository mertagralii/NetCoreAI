# NetCoreAI.Project01.ApiDemo

## API Kullanımı

### Başlangıçta Yaptıklarımız

ASP.NET Core Web API projemizi oluşturduktan sonra gerekli **Entity Framework** paketlerini kurduk.

Paketleri kurduktan sonra **Code First** yaklaşımı ile veritabanımızı oluşturmak istedik.

Projemizin içine **Entities** ve **Context** adında iki klasör oluşturduk.

**Entities** klasörüne **Customer** modelini tanımladık:

```csharp
namespace NetCoreAI.Project01.ApiDemo.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; } // Müşteri ID (Primary Key)
        public string Name { get; set; } // Müşteri adı
        public string Surname { get; set; } // Müşteri soyadı
        public decimal Balance { get; set; } // Müşteri bakiyesi
    }
}
```

Ardından **Context** klasörüne **ApiContext** sınıfını oluşturduk:

```csharp
using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project01.ApiDemo.Entities;

namespace NetCoreAI.Project01.ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQL Server bağlantı dizesini belirtiyoruz (Database ismini hangi isimde açılmasını istiyorsan Database kısmına yaz CodeFirst ile oluşacak zaten hepsi)
            optionsBuilder.UseSqlServer("Server=MERT;Database=ApiAIDb;Integrated Security=true;TrustServerCertificate=True");
        }
        
        public DbSet<Customer> Customers { get; set; } // Veritabanında Customers adlı tablo oluşturulacak
    }
}
```

Daha sonra **View** sekmesinden **Other Windows -> Package Manager Console** terminaline girerek aşağıdaki komutları sırasıyla çalıştırdık:

```sh

// İlk göç dosyasını oluşturur
add-migration mig1

// Veritabanını günceller
update-database
 
```

Sonrasında **Program.cs** dosyasına **DbContext**'i ekledik:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlamını servislere ekliyoruz
builder.Services.AddDbContext<ApiContext>();
```

Son olarak **Controllers** klasörüne sağ tıklayıp **New Class** seçeneği ile bir **API Controller** oluşturduk. 
**Dependency Injection** kullanarak **ApiContext**'i enjekte ettik:

```csharp
private readonly ApiContext _context; // Veritabanı bağlamı

public CustomersController(ApiContext context)
{
    _context = context; // Bağlamı kurucu metod aracılığıyla atıyoruz
}
```

---

Aşağıda **CustomersController** içinde tanımlı olan endpoint'ler bulunmaktadır:

### 1. Tüm Müşterileri Getir
```http
GET /api/customers
```
```csharp
[HttpGet] // GET ==> Veri çekme işlemi
public IActionResult CustomersList()
{
    var customersList = _context.Customers.ToList(); // Tüm müşteri listesini alıyoruz
    return Ok(customersList); // Listeyi döndürüyoruz
}
```
Açıklama: Mevcut tüm müşterileri listeler.

### 2. Yeni Müşteri Ekle
```http
POST /api/customers/CreateCustomers
```
```csharp
[HttpPost("CreateCustomers")]
public IActionResult CreateCustomers(Customer customer)
{
    _context.Customers.Add(customer); // Yeni müşteri ekleniyor
    _context.SaveChanges(); // Değişiklikler veritabanına kaydediliyor
    return Ok("Müşteri Ekleme İşlemi Gerçekleştirildi.");
}
```
Açıklama: Yeni müşteri eklemek için kullanılır.

### 3. Müşteri Sil
```http
DELETE /api/customers/DeleteCustomers?Id={id}
```
```csharp
[HttpDelete("DeleteCustomers")]
public IActionResult DeleteCustomers(int Id)
{
    var deleteCustomer = _context.Customers.Find(Id); // Silinecek müşteri aranıyor
    if (deleteCustomer == null)
        return NotFound("Müşteri bulunamadı."); // Müşteri yoksa hata döndür

    _context.Customers.Remove(deleteCustomer); // Müşteri siliniyor
    _context.SaveChanges(); // Değişiklikler kaydediliyor
    return Ok("Müşteri Silme İşlemi Gerçekleştirildi.");
}
```
Açıklama: Belirtilen **Id** değerine sahip müşteriyi siler.

### 4. Belirli Bir Müşteriyi Getir
```http
GET /api/customers/GetCustomer?Id={id}
```
```csharp
[HttpGet("GetCustomer")]
public IActionResult GetCustomer(int Id)
{
    var getCustomer = _context.Customers.Find(Id); // ID'ye göre müşteri aranıyor
    if (getCustomer == null)
        return NotFound("Müşteri bulunamadı."); // Müşteri yoksa hata döndür

    return Ok(getCustomer); // Müşteri bilgilerini döndür
}
```
Açıklama: Verilen **Id**'ye göre müşteri bilgilerini getirir.

### 5. Müşteri Bilgilerini Güncelle
```http
PUT /api/customers/UpdateCustomers
```
```csharp
[HttpPut("UpdateCustomers")]
public IActionResult UpdateCustomer(Customer customer)
{
    _context.Customers.Update(customer); // Müşteri bilgilerini güncelliyoruz
    _context.SaveChanges(); // Değişiklikleri kaydediyoruz
    return Ok("Müşteri Güncelleme İşlemi Gerçekleştirildi.");
}
```
Açıklama: Gönderilen müşteri nesnesine göre verileri günceller.

## Önemli Notlar
- **CRUD İşlemleri İçin HTTP Metotları:**
  - Veri çekmek için `HttpGet`
  - Veri eklemek için `HttpPost`
  - Veri güncellemek için `HttpPut`
  - Veri silmek için `HttpDelete`
- Aynı **HTTP metodu** birden fazla kullanılacaksa, `[HttpGet("ÖzelAd")]` gibi bir isimlendirme yapılmalıdır.




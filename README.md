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










# NetCoreAI.Project02.ApiConsumeUI

Bu proje, bir API tüketim uygulamasıdır. ASP.NET Core MVC kullanarak, harici bir API'den müşteri verilerini çekmek, oluşturmak, güncellemek ve silmek için geliştirilmiştir.

## Kullanılan Teknolojiler
- **ASP.NET Core MVC** (Model-View-Controller mimarisi)
- **IHttpClientFactory** (API ile haberleşmek için)
- **Newtonsoft.Json** (JSON serileştirme ve ayrıştırma işlemleri için)
- **Bootstrap** (UI tasarımı için)

## Proje Yapısı
```
NetCoreAI.Project02.ApiConsumeUI
│-- Controllers
│   ├── CustomerController.cs
│-- Dtos
│   ├── CreateCustomerDto.cs
│   ├── GetByIdCustomerDto.cs
│   ├── ResultCustomerDto.cs
│   ├── UpdateCustomerDto.cs
│-- Views
│   ├── Customer
│       ├── CustomerList.cshtml
│       ├── CreateCustomer.cshtml
│       ├── UpdateCustomer.cshtml
│-- wwwroot
│-- Program.cs
│-- appsettings.json
```
 `Dtos (Data Transfer Objects)`: API'den gelen verileri eşlemek için kullanılan veri transfer nesneleri içerir.

## API Kullanımı
Uygulama, müşteri verilerini yönetmek için aşağıdaki **RESTful API** uç noktalarına istek atmaktadır:

| HTTP Metodu | URL | Açıklama |
|------------|-------------------------------------|----------------------------------|
| GET | `/api/Customers` | Tüm müşterileri listeler |
| POST | `/api/Customers/CreateCustomers` | Yeni müşteri ekler |
| DELETE | `/api/Customers/DeleteCustomers/{id}` | Belirtilen ID'ye sahip müşteriyi siler |
| GET | `/api/Customers/GetCustomer?Id={id}` | Belirtilen ID'ye sahip müşteriyi getirir |
| PUT | `/api/Customers/UpdateCustomers` | Mevcut bir müşteriyi günceller |

## Controller Açıklaması
**CustomerController.cs** içinde API ile iletişim kuran metodlar bulunmaktadır:

- `CustomersList()` → API'den müşteri listesini çeker ve **CustomerList.cshtml** View'ine gönderir.
- `CreateCustomer()` → Yeni müşteri eklemek için API'ye **POST** isteği atar.
- `DeleteCustomer(int Id)` → Belirtilen **ID'ye** sahip müşteriyi silmek için API'ye **DELETE** isteği atar.
- `UpdateCustomer(int id) [GET]` → Güncellenecek müşterinin mevcut bilgilerini API'den çeker.
- `UpdateCustomer(GetByIdCustomerDto model) [POST]` → Müşteri güncelleme işlemini API'ye **PUT** isteği ile yapar.

## View Açıklaması
- **CustomerList.cshtml** → Müşteri listesini tablo halinde gösterir. Silme ve Güncelleme butonlarını içerir.
- **CreateCustomer.cshtml** → Yeni müşteri eklemek için bir form içerir.
- **UpdateCustomer.cshtml** → Mevcut müşteri bilgilerini güncellemek için bir form içerir.


## Kurulum & Çalışma

Bir önceki projede bir API oluşturup, projenin içerisine entity paketlerini kurduktan sonra CRUD işlemleri yapacak API'leri kurmuştuk. Bu projede ise oluşturduğumuz bu API'leri ASP.NET Core MVC projesinde kullanacağız.

Projeyi açtıktan sonra paket olarak `NewtonSoft.Json` paketini yüklüyoruz.

Yükleme işlemini gerçekleştirdikten sonra projemizin içine bir `Dtos (Data Transfer Objects)` klasörü açıyoruz.

Açtıktan sonra `SwaggerUI` kısmındaki API'lerimize göre modellerimizi oluşturuyoruz.

Modellerimizi oluşturduktan sonra `Program.cs` dosyasına aşağıdaki kodu ekliyoruz:

```csharp
// HttpClientFactory servisini uygulamaya dahil ediyoruz.
builder.Services.AddHttpClient(); 
```

Sonrasında, bunu bağımlılık enjeksiyonu (Dependency Injection) ile projemize dahil ediyoruz:

```csharp
// HttpClientFactory üzerinden bir istemci oluşturup API ile haberleşeceğiz.
private readonly IHttpClientFactory _httpClientFactory;

public CustomerController(IHttpClientFactory httpClientFactory)
{
    _httpClientFactory = httpClientFactory;
}
```

Ardından oluşturduğumuz API'lere göre işlemleri gerçekleştirebiliriz.

### API Tüketimi Örnek Kodlar

#### Müşteri Listesi Getirme (GET Request)
```csharp
public async Task<IActionResult> CustomersList()
{
    var client = _httpClientFactory.CreateClient(); // HTTP istemcisi oluşturuyoruz.
    var responseMessage = await client.GetAsync("https://localhost:7180/api/Customers"); // API'ye GET isteği gönderiyoruz.
    if (responseMessage.IsSuccessStatusCode) // Başarılı cevap kontrolü
    {
        var jsonData = await responseMessage.Content.ReadAsStringAsync(); // JSON verisini okuyoruz.
        var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData); // JSON'u C# nesnesine çeviriyoruz.
        return View(values); // Veriyi View'e gönderiyoruz.
    }
    return View();
}
```

#### Yeni Müşteri Ekleme (POST Request)
```csharp
[HttpPost]
public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
{
    var client = _httpClientFactory.CreateClient(); // HTTP istemcisi oluşturuyoruz.
    var jsonData = JsonConvert.SerializeObject(createCustomerDto); // Nesneyi JSON formatına çeviriyoruz.
    var content = new StringContent(jsonData, Encoding.UTF8, "application/json"); // JSON içeriğini hazırlıyoruz.
    var responseMessage = await client.PostAsync("https://localhost:7180/api/Customers/CreateCustomers", content); // API'ye POST isteği gönderiyoruz.
    if (responseMessage.IsSuccessStatusCode) // Başarılı cevap kontrolü
    {
        return RedirectToAction("CustomersList"); // Başarılıysa listeye yönlendiriyoruz.
    }
    return View();
}
```

#### Müşteri Silme (DELETE Request)
```csharp
public async Task<IActionResult> DeleteCustomer(int Id)
{
    var client = _httpClientFactory.CreateClient(); // HTTP istemcisi oluşturuyoruz.
    var responseMessage = await client.DeleteAsync($"https://localhost:7180/api/Customers/DeleteCustomers/{Id}"); // API'ye DELETE isteği gönderiyoruz.
    if (responseMessage.IsSuccessStatusCode) // Başarılı cevap kontrolü
    {
        return RedirectToAction("CustomersList"); // Başarılıysa listeye yönlendiriyoruz.
    }
    return View();
}
```

### Görünüm (View) Dosyaları

#### Müşteri Listesi Görünümü (CustomerList.cshtml)
```html
@model List<NetCoreAI.Project02.ApiConsumeUI.Dtos.ResultCustomerDto> // Eğer ResulCustomerDto diye çıkmazsa model o zaman böyle gezinerek de modeli tanıtabilirsin.
<link href="~/lib/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
<div class="container">
    <br /> <br />
    <table class="table table-bordered">
        <tr>
            <th>#</th>
            <th>Müşteri Adı Soyadı</th>
            <th>Müşteri Bakiye</th>
            <th>Sil</th>
            <th>Güncelle</th>
        </tr>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.CustomerId</td>
                <td>@item.Name @item.Surname</td>
                <td>@item.Balance</td>
                <td><a href="/Customer/DeleteCustomer/@item.CustomerId" class="btn btn-danger">Sil</a></td>
                <td><a href="/Customer/UpdateCustomer/@item.CustomerId" class="btn btn-success">Güncelle</a></td>
            </tr>
        }
    </table>
    <a href="/Customer/CreateCustomer" class="btn btn-info">Müşteri Ekle</a>
</div>
```

Bu şekilde projemizde API'leri kullanarak müşteri CRUD işlemlerini gerçekleştirebiliriz.














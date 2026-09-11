# NetCoreAI

Nine standalone .NET projects that go from building and consuming your own REST API to wiring real AI services — OpenAI, Whisper, DALL·E, Tesseract OCR and Google Cloud Vision — into C# applications.

Each project is its own runnable solution folder, so you can open just the one you care about.

![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![OpenAI](https://img.shields.io/badge/OpenAI-412991?style=flat-square&logo=openai&logoColor=white)
![Google Cloud](https://img.shields.io/badge/Cloud_Vision-4285F4?style=flat-square&logo=googlecloud&logoColor=white)

---

## Projects

| # | Project | What it does | Key technologies |
|---|---|---|---|
| 01 | **ApiDemo** | REST API with full CRUD over a `Customer` model, built code-first | ASP.NET Core Web API · EF Core · SQL Server |
| 02 | **ApiConsumeUI** | MVC front-end that consumes the API from project 01 | ASP.NET Core MVC · `IHttpClientFactory` · DTOs · Bootstrap |
| 03 | **RapidApi** | Consuming a third-party public API and mapping its JSON into C# classes | Console app · RapidAPI · Newtonsoft.Json |
| 04 | **OpenAIChat** | Chat completions against the OpenAI API from C# | OpenAI API · HttpClient |
| 05 | **OpenWhisperAudioTranskript** | Transcribing an audio file to text | OpenAI Whisper API |
| 06 | **DallEImageGeneration** | Generating images from a text prompt | OpenAI DALL·E API |
| 07 | **TesserectOcr** | Reading text out of an image locally, with downloadable language packs | Tesseract · tessdata |
| 08 | **GoogleCloudVision** | Cloud-based image analysis and text detection | Google Cloud Vision API · service account auth |
| 09 | **OpenAITranslate** | Translating text between languages | OpenAI API |

## What this repo is for

The projects build on each other deliberately:

1. **Own the data** — project 01 shows the code-first EF Core path: entities, `DbContext`, migrations, controllers.
2. **Consume an API properly** — project 02 uses `IHttpClientFactory` instead of `new HttpClient()`, and maps responses into DTOs rather than passing entities to the view.
3. **Consume someone else's API** — project 03 covers the same pattern against a third-party service, including turning a JSON response into strongly typed classes.
4. **Add AI** — projects 04–09 each integrate a different AI capability: conversation, speech-to-text, image generation, OCR (local and cloud) and translation.

## Requirements

- .NET SDK
- SQL Server (or LocalDB) for projects 01–02
- An **OpenAI API key** for projects 04, 05, 06 and 09
- A **RapidAPI key** for project 03
- A **Google Cloud service account** JSON credential file with the Vision API enabled for project 08
- Tesseract **tessdata** language files for project 07

> Keep API keys out of source control — use `appsettings.Development.json`, user secrets or environment variables.

## Running a project

```bash
git clone https://github.com/mertagralii/NetCoreAI.git
cd NetCoreAI/NetCoreAI.Project01.ApiDemo
dotnet run
```

For projects 01–02, set your connection string and apply the migrations first:

```bash
dotnet ef database update
```

## Detailed notes

[**NOTES.md**](NOTES.md) contains the full step-by-step walkthrough for every project — NuGet packages, configuration, credential setup and code explanations (in Turkish).

## Author

**Mert Ağralı** — [GitHub](https://github.com/mertagralii) · [LinkedIn](https://www.linkedin.com/in/mertagrali/)

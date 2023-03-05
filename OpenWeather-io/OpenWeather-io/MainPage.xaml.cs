using Newtonsoft.Json;

namespace OpenWeather_io;

public partial class MainPage : ContentPage
{ 
	public MainPage()
	{
        Report report = Task.Run(() => OpenWeather(44, -73.6)).Result;
        //InitializeComponent();
        InitializeComponent();
        tempText.Text = report.Temperature;
        weatherText.Text = report.Weather;
        //Task.Run(async () =>
        //{

        //});

    }

    private static async Task<Report> OpenWeather(double lat, double lng)
    {

        // Key and URL Build
        string key = "2ccabbbdd0c082256f182826839977e9";
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={key}";


        // Build and Connect to OpenWeather API
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
        return new Report(result.main.temp,result.weather);
    }
}


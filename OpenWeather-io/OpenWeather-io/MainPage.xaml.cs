using Newtonsoft.Json;
using OpenWeather_io;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Microsoft.Maui.Networking;

namespace OpenWeather_io;

public partial class MainPage : ContentPage
{

    public MainPage()
    {

        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
        Handle();

    }
    private async void Handle() //Starts a thread async after page intialization
    {
        MakeRequest();
        Report report = await OpenWeather(44, -73.6);
        tempText.Text = report.Temp;
        weatherText.Text = report.Weather;
        weatherIcon.Source = report.Icon;
        scrollBack.BackgroundColor = Color.FromHex(report.Color);

    }
    public static async Task<Report> OpenWeather(double lat, double lng)
    {
        string color;
        // Key and URL Build
        string key = "56605f7020426e4c13970270f548908e";
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={key}&units=imperial";


        // Build and Connect to OpenWeather API
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        // Get JSON Data
        string content = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
        string temp = result.main.temp.ToString() + " F";
        string weather = result.weather[0].main.ToString();
        ImageSource icon = $"https://openweathermap.org/img/wn/{result.weather[0].icon.ToString()}.png";

        if (result.dt > result.sys.sunset)
        {
            color = "#666666";
        }
        else
        {
            color = "#48afff";
        }

        // Return Data
        return new Report(temp, weather, icon, color);


    }

    public static async Task MakeRequest()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string key = "AIzaSyAgURdrNzcJ8JTamAFq9ikwLBd2Zf1dL70";
                string ip = GetIP();
                string url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
            }   
            catch
            {

            }
        }
    }

    public static string GetIP() {

        IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress address in addresses)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                return address.ToString();
   
            }
        }
        return null;
       
    }
}




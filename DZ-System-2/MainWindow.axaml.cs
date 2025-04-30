using System;
using System.Net.Http;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Newtonsoft.Json.Linq;


namespace MyProgekt;

public partial class MainWindow : Window
{
    
    private Thread updateThread;
    private bool running = false;
    private const string ApiKey = "dbde8fa8e1904f5da8724a3188076258";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void GetWeatherButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var city = CityTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(city))
        {
            ErrorTextBlock.Text = "Введите название города.";
            return;
        }

        if (running)
        {
            running = false;
            updateThread?.Join();
        }

        running = true;
        updateThread = new Thread(() => UpdateWeatherLoop(city));
        updateThread.IsBackground = true;
        updateThread.Start();
    }

    private async void UpdateWeatherLoop(string city)
    {
        while (running)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric&lang=ru");

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(json);

                    string weather = $"Город: {data["name"]}\nТемпература: {data["main"]["temp"]}°C\nПогода: {data["weather"][0]["description"]}";

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        WeatherTextBlock.Text = weather;
                        ErrorTextBlock.Text = "";
                    });
                }
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ErrorTextBlock.Text = "Ошибка: " + ex.Message;
                    WeatherTextBlock.Text = "";
                });
            }

            Thread.Sleep(10000);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        running = false;
        updateThread?.Join();
        base.OnClosed(e);
    }
    
}

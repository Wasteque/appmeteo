using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json.Linq;
using Avalonia.Media;
using Avalonia.Threading;

namespace appmeteo
{
    public partial class MainWindow : Window
    {
        private TextBox cityTextBox;
        private Button searchButton;
        private Button toggleRgbButton;
        private TextBlock weatherResultText;
        private TextBlock forecastResultText;
        private TabControl tabs;

        private GradientStop gradientStop1;
        private GradientStop gradientStop2;
        private LinearGradientBrush backgroundGradientBrush;
        private DispatcherTimer animationTimer;
        private bool isRgbActive = true;
        private double hue1 = 0;
        private double hue2 = 100;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            cityTextBox = this.FindControl<TextBox>("CityTextBox");
            searchButton = this.FindControl<Button>("SearchButton");
            toggleRgbButton = this.FindControl<Button>("ToggleRgbButton");
            weatherResultText = this.FindControl<TextBlock>("WeatherResultText");
            forecastResultText = this.FindControl<TextBlock>("ForecastResultText");
            tabs = this.FindControl<TabControl>("Tabs");

            searchButton.Click += async (sender, e) => await SearchWeatherAsync();
            toggleRgbButton.Click += ToggleRgbEffect;

            backgroundGradientBrush = (LinearGradientBrush)this.Resources["GradientBrush"];
            gradientStop1 = backgroundGradientBrush.GradientStops[0];
            gradientStop2 = backgroundGradientBrush.GradientStops[1];

            animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(25)
            };
            animationTimer.Tick += AnimateGradient;
            animationTimer.Start();
        }

        private async Task SearchWeatherAsync()
        {
            string city = cityTextBox.Text;

            if (string.IsNullOrWhiteSpace(city))
            {
                weatherResultText.Text = "Veuillez entrer une ville valide.";
                forecastResultText.Text = "";
                return;
            }

            try
            {
                string apiKey = File.ReadAllText("api_key.txt");
                string currentWeatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=fr";
                string forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric&lang=fr";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage currentWeatherResponse = await client.GetAsync(currentWeatherUrl);
                    currentWeatherResponse.EnsureSuccessStatusCode();

                    string currentWeatherBody = await currentWeatherResponse.Content.ReadAsStringAsync();
                    JObject currentWeatherData = JObject.Parse(currentWeatherBody);

                    string description = currentWeatherData["weather"][0]["description"].ToString();
                    string temperature = currentWeatherData["main"]["temp"].ToString();
                    string humidity = currentWeatherData["main"]["humidity"].ToString();

                    weatherResultText.Text = $"Météo : {description}\nTempérature : {temperature} °C\nHumidité : {humidity}%";

                    HttpResponseMessage forecastResponse = await client.GetAsync(forecastUrl);
                    forecastResponse.EnsureSuccessStatusCode();

                    string forecastBody = await forecastResponse.Content.ReadAsStringAsync();
                    JObject forecastData = JObject.Parse(forecastBody);

                    forecastResultText.Text = "Prévisions sur 5 jours :\n";

                    for (int i = 0; i < 5; i++)
                    {
                        var forecast = forecastData["list"][i * 8]; 
                        string date = forecast["dt_txt"].ToString();
                        string temp = forecast["main"]["temp"].ToString();
                        string desc = forecast["weather"][0]["description"].ToString();

                        forecastResultText.Text += $"- {date}: {desc}, {temp} °C\n";
                    }
                }
            }
            catch (Exception ex)
            {
                weatherResultText.Text = $"Erreur : {ex.Message}";
                forecastResultText.Text = "";
            }
        }

        private void ToggleRgbEffect(object sender, EventArgs e)
        {
            isRgbActive = !isRgbActive;

            if (isRgbActive)
            {
                animationTimer.Start();
            }
            else
            {
                animationTimer.Stop();
                ResetGradientToDefault();
            }
        }

        private void ResetGradientToDefault()
        {
            gradientStop1.Color = Color.Parse("#FFFFFF");
            gradientStop2.Color = Color.Parse("#FFFFFF");
        }

        private void AnimateGradient(object sender, EventArgs e)
        {
            if (!isRgbActive) return;

            gradientStop1.Color = ColorFromHue(hue1);
            gradientStop2.Color = ColorFromHue(hue2);
            hue1 = (hue1 + 1) % 360;
            hue2 = (hue2 + 1) % 360;

            backgroundGradientBrush.GradientStops[0] = gradientStop1;
            backgroundGradientBrush.GradientStops[1] = gradientStop2;
        }

        private Color ColorFromHue(double hue)
        {
            hue = hue % 360;
            if (hue < 60)
                return Color.FromRgb((byte)(255), (byte)(hue * 255 / 60), 0);
            else if (hue < 120)
                return Color.FromRgb((byte)((120 - hue) * 255 / 60), 255, 0);
            else if (hue < 180)
                return Color.FromRgb(0, 255, (byte)((hue - 120) * 255 / 60));
            else if (hue < 240)
                return Color.FromRgb(0, (byte)((240 - hue) * 255 / 60), 255);
            else if (hue < 300)
                return Color.FromRgb((byte)((hue - 240) * 255 / 60), 0, 255);
            else
                return Color.FromRgb(255, 0, (byte)((360 - hue) * 255 / 60));
        }
    }
}

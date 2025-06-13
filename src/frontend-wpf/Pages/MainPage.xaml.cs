using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Classes;
using Laendlefinder.Collections;

namespace Laendlefinder.Pages;

public partial class MainPage : Page
{
    private EventCollection eventCollection = new();

    public MainPage()
    {
        InitializeComponent();
        LoadEventsAsync();
    }

    private async void LoadEventsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/events");
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(responseString);

                eventCollection.Clear();
                foreach (var ev in events)
                {
                    eventCollection.Add(ev);
                }

                eventCollection.Draw(EventsPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Events: " + ex.Message);
            }
        }
    }
}
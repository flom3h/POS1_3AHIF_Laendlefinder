using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Laendlefinder.Classes;


namespace Laendlefinder.Pages;

public partial class LoginPage : Page
{
    private bool passwordVisible = false;
    public static event EventHandler LoginButtonClickedNavHome;
    public LoginPage()
    {
        InitializeComponent();
        PasswordBox.Visibility = Visibility.Visible;
        PlainPasswordBox.Visibility = Visibility.Collapsed;
    }

    private void ChangePasswordVisibility_Click(object sender, RoutedEventArgs e)
    {
        passwordVisible = !passwordVisible;

        if (passwordVisible)
        {
            PlainPasswordBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
            PlainPasswordBox.Visibility = Visibility.Visible;
        }
        else
        {
            PasswordBox.Password = PlainPasswordBox.Text;
            PasswordBox.Visibility = Visibility.Visible;
            PlainPasswordBox.Visibility = Visibility.Collapsed;
        }
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        AsncMethode(sender, e);
    }

    private async void AsncMethode(object sender, RoutedEventArgs e)
    {
        string email = MailBox.Text;
        string passwort = passwordVisible ? PlainPasswordBox.Text : PasswordBox.Password;

        var loginData = new LoginRequest
        {
            email = email,
            passwort = passwort
        };

        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = JsonSerializer.Serialize(loginData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/login", content);

                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LoginButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Login fehlgeschlagen: " + responseString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Login: " + ex.Message);
            }
        }
    }

}
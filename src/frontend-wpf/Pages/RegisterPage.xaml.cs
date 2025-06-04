using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Laendlefinder.Classes;

namespace Laendlefinder.Pages;

public partial class RegisterPage : Page
{
    private bool passwordVisible = false;
    
    public static event EventHandler LoginButtonClickedNavHome;
    public static event EventHandler LoginButtonClickedNavLogin;
    public RegisterPage()
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

    private void RegButton_Click(object sender, RoutedEventArgs e)
    {
        AsncMethode(sender, e);
    }
    
    private async void AsncMethode(object sender, RoutedEventArgs e)
    {
        string sn = SnBox.Text;
        string ln = LnBox.Text;
        if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(ln) || string.IsNullOrWhiteSpace(sn) || string.IsNullOrWhiteSpace(ln) || Regex.IsMatch(sn, @"\d") || Regex.IsMatch(ln, @"\d"))
        {
            MessageBox.Show("Vorname und Nachname dürfen nicht leer sein oder Zahlen enthalten.");
            return;
        }
        string email = MailBox.Text;
        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) // Prüft auf simple E-Mail
        {
            MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            return;
        }
        string passwort = passwordVisible ? PlainPasswordBox.Text : PasswordBox.Password;
        if (string.IsNullOrEmpty(passwort) || passwort.Length < 8)
        {
            MessageBox.Show("Das Passwort muss mindestens 8 Zeichen lang sein.");
            return;
        }
        
        var regData = new RegRequest()
        {
            firstname = sn,
            lastname = ln,
            email = email,
            passwort = passwort
        };

        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = JsonSerializer.Serialize(regData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/registrieren", content);

                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LoginButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Registrierung fehlgeschlagen: " + responseString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Registrieren: " + ex.Message);
            }
        }
    }

    private void SnBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderSn.Visibility = string.IsNullOrEmpty(SnBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void LnBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderLn.Visibility = string.IsNullOrEmpty(LnBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void MailBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderMail.Visibility = string.IsNullOrEmpty(MailBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void PlainPasswordBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderPassword.Visibility = string.IsNullOrEmpty(PlainPasswordBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        PlaceholderPassword.Visibility = string.IsNullOrEmpty(PasswordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void YesLoginButton_Click(object sender, RoutedEventArgs e)
    {
        LoginButtonClickedNavLogin?.Invoke(this, EventArgs.Empty);
    }
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Classes;

namespace Laendlefinder.Pages;

public partial class ProfilePage : Page
{
    private bool passwordVisible = false;
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    public static event EventHandler SaveChangesButtonClickedNavHome;
    public static event EventHandler LogOutButtonClickedNavLogin;

    public ProfilePage()
    {
        InitializeComponent();
        PasswordBox.Visibility = Visibility.Visible;
        PlainPasswordBox.Visibility = Visibility.Collapsed;
        LoadProfileAsync();
    }

    public async void LoadProfileAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:8081/user/{uid}");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var user = System.Text.Json.JsonSerializer.Deserialize<RegRequest>(responseString);
                PlaceholderSn.Content = user.firstname;
                PlaceholderLn.Content = user.lastname;
                PlaceholderMail.Content = user.email;
                MainWindow.Logger.Information($"ProfilePage: User geladen: {user.firstname} {user.lastname}, Email: {user.email}");
            }
        }
    }

    private void ChangePasswordVisibility_Click(object sender, RoutedEventArgs e)
    {
        passwordVisible = !passwordVisible;

        if (passwordVisible)
        {
            PlainPasswordBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
            PlainPasswordBox.Visibility = Visibility.Visible;
            MainWindow.Logger.Information("Passwortfeld auf Klartext umgeschaltet.");
        }
        else
        {
            PasswordBox.Password = PlainPasswordBox.Text;
            PasswordBox.Visibility = Visibility.Visible;
            PlainPasswordBox.Visibility = Visibility.Collapsed;
            MainWindow.Logger.Information("Passwortfeld auf versteckt umgeschaltet.");
        }
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
    }

    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
    }

    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
    }

    private void MapButton_Click(object sender, RoutedEventArgs e)
    {
        MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
    }

    private void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
    }

    private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
    {
        string sn = string.IsNullOrEmpty(SnBox.Text.Trim())
            ? PlaceholderSn.Content.ToString().Trim()
            : SnBox.Text.Trim();
        string ln = string.IsNullOrEmpty(LnBox.Text.Trim())
            ? PlaceholderLn.Content.ToString().Trim()
            : LnBox.Text.Trim();
        if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(ln) || Regex.IsMatch(sn, @"\d") ||
            Regex.IsMatch(ln, @"\d"))
        {
            MessageBox.Show("Vorname und Nachname dürfen nicht leer sein oder Zahlen enthalten.");
            MainWindow.Logger.Error("Ungültige Eingabe für Vorname oder Nachname.");
            return;
        }

        string email = string.IsNullOrEmpty(MailBox.Text.Trim())
            ? PlaceholderMail.Content.ToString().Trim()
            : MailBox.Text.Trim();
        if (string.IsNullOrEmpty(email) ||
            !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) // Prüft auf simple E-Mail
        {
            MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            MainWindow.Logger.Error("Ungültige E-Mail-Adresse eingegeben.");
            return;
        }

        string passwort = passwordVisible ? PlainPasswordBox.Text.Trim() : PasswordBox.Password.Trim();
        if (string.IsNullOrEmpty(passwort) || passwort.Length < 8)
        {
            MessageBox.Show("Das Passwort muss mindestens 8 Zeichen lang sein.");
            MainWindow.Logger.Error("Ungültiges Passwort eingegeben.");
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
            client.BaseAddress = new Uri("http://127.0.0.1:8081");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonSerializer.Serialize(regData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"/update/{uid}", content);

            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Änderungen erfolgreich gespeichert.");
                MainWindow.Logger.Information("Änderungen erfolgreich gespeichert.");
                SaveChangesButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Fehler beim Speichern der Änderungen: " + responseString);
                MainWindow.Logger.Error("Fehler beim Speichern der Änderungen: " + responseString);
            }
        }

        
    }
    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
        LogOutButtonClickedNavLogin?.Invoke(this, EventArgs.Empty);
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
        PlaceholderPassword.Visibility =
            string.IsNullOrEmpty(PlainPasswordBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        PlaceholderPassword.Visibility =
            string.IsNullOrEmpty(PasswordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
    }
    
}
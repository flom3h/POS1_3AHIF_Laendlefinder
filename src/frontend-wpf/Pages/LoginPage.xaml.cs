using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Laendlefinder.Classes;


namespace Laendlefinder.Pages;

/**
 * @class LoginPage
 * @brief Repräsentiert die Login-Seite der Anwendung.
 * Ermöglicht Benutzern, sich mit E-Mail und Passwort anzumelden.
 */
public partial class LoginPage : Page
{
    public static int CurrentUserID { get; set; } = 0;
    private bool passwordVisible = false;
    public static event EventHandler LoginButtonClickedNavHome;
    public static event EventHandler LoginButtonClickedNavRegister;

    /**
    * Konstruktor für die LoginPage. Initialisiert die Komponenten und setzt die Sichtbarkeit der Passwortfelder.
    */
    public LoginPage()
    {
        InitializeComponent();
        PasswordBox.Visibility = Visibility.Visible;
        PlainPasswordBox.Visibility = Visibility.Collapsed;
        MainWindow.Logger.Information("LoginPage initialized");
    }

    /**
    * Event-Handler zum Umschalten der Passwortsichtbarkeit.
    * Zeigt oder versteckt das Passwort im Klartextfeld.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
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

    /**
    * Event-Handler für den Login-Button. Löst das LoginButtonClickedNavHome-Event aus.
    * Leitet zur HomePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        AsncMethode(sender, e);
        MainWindow.Logger.Information("LoginButton wurde aktiviert.");

    }

    /**
    * Asynchrone Methode zur Durchführung des Logins.
    * Sendet die Login-Daten an den Server und verarbeitet die Antwort.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
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
                string message = JsonDocument.Parse(responseString).RootElement.GetProperty("message").GetString();

                if (response.IsSuccessStatusCode)
                {
                    CurrentUserID = JsonDocument.Parse(responseString).RootElement.GetProperty("userID").GetInt32();
                    MainWindow.Logger.Information($"Login erfolgreich für Benutzer {CurrentUserID}.");
                    LoginButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Login fehlgeschlagen: " + message);
                    MainWindow.Logger.Error($"Login fehlgeschlagen: {message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Login: " + ex.Message);
                MainWindow.Logger.Error($"Fehler beim Login: {ex.Message}");
            }
        }
    }

    /**
    * Event-Handler für Textänderungen in der MailBox.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void MailBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderMail.Visibility = string.IsNullOrEmpty(MailBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }
    
    /**
    * Event-Handler für Textänderungen im Klartext-Passwortfeld.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void PlainPasswordBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderPassword.Visibility = string.IsNullOrEmpty(PlainPasswordBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    /**
    * Event-Handler für Änderungen im versteckten Passwortfeld.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        PlaceholderPassword.Visibility = string.IsNullOrEmpty(PasswordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
    }
    
    /**
    * Event-Handler für den Register-Button. Löst das LoginButtonClickedNavRegister-Event aus.
    * Leitet zur RegisterPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void NavToRegisterPage(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Register-Button wurde geklickt, Navigation zur RegisterPage.");
        LoginButtonClickedNavRegister?.Invoke(this, EventArgs.Empty);
    }
}
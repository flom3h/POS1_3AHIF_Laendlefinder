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

/**
 * @class RegisterPage
 * @brief Repräsentiert die Registrierungsseite der Anwendung.
 * Ermöglicht Benutzern, sich zu registrieren und ein neues Konto zu erstellen.
 */
public partial class RegisterPage : Page
{
    public static int CurrentUserID { get; set; } = 0;
    private bool passwordVisible = false;
    
    public static event EventHandler LoginButtonClickedNavHome;
    public static event EventHandler LoginButtonClickedNavLogin;
    /**
    * Konstruktor für die RegisterPage. Initialisiert die Komponenten und setzt die Passwortfelder.
    */
    public RegisterPage()
    {
        InitializeComponent();
        MainWindow.Logger.Information("RegisterPage initialized");
        PasswordBox.Visibility = Visibility.Visible;
        PlainPasswordBox.Visibility = Visibility.Collapsed;
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
    private void RegButton_Click(object sender, RoutedEventArgs e)
    {
        AsncMethode(sender, e);
    }

    /**
     *  Asynchrone Methode zur Durchführung der Registrierung.
     *  Validiert die Eingaben und sendet eine Registrierungsanfrage an den Server.
     *  @param sender Das auslösende Objekt.
     *  @param e Event-Argumente.
     */
    private async void AsncMethode(object sender, RoutedEventArgs e)
    {
        string sn = SnBox.Text.Trim();
        string ln = LnBox.Text.Trim();
        if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(ln) || Regex.IsMatch(sn, @"\d") || Regex.IsMatch(ln, @"\d"))
        {
            MessageBox.Show("Vorname und Nachname dürfen nicht leer sein oder Zahlen enthalten.");
            MainWindow.Logger.Error("Vorname oder Nachname ungültig: " + sn + " " + ln);
            return;
        }
        string email = MailBox.Text.Trim();
        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) // Prüft auf simple E-Mail
        {
            MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            MainWindow.Logger.Error("Ungültige E-Mail-Adresse eingegeben: " + email);
            return;
        }
        string passwort = passwordVisible ? PlainPasswordBox.Text.Trim() : PasswordBox.Password.Trim();
        if (string.IsNullOrEmpty(passwort) || passwort.Length < 8)
        {
            MessageBox.Show("Das Passwort muss mindestens 8 Zeichen lang sein.");
            MainWindow.Logger.Error("Ungültiges Passwort eingegeben: " + passwort);
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
                string message = JsonDocument.Parse(responseString).RootElement.GetProperty("message").GetString();

                if (response.IsSuccessStatusCode)
                {
                    CurrentUserID = JsonDocument.Parse(responseString).RootElement.GetProperty("userID").GetInt32();
                    LoginButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Registrierung fehlgeschlagen: " + message);
                    MainWindow.Logger.Error("Registrierung fehlgeschlagen: " + message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Registrieren: " + ex.Message);
                MainWindow.Logger.Error("Fehler beim Registrieren: " + ex.Message);
            }
        }
    }

    /**
    * Event-Handler für Textänderungen im Klartext-Passwortfeld.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void SnBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderSn.Visibility = string.IsNullOrEmpty(SnBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    /**
    * Event-Handler für Textänderungen im Nachnamenfeld.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void LnBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderLn.Visibility = string.IsNullOrEmpty(LnBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    /**
    * Event-Handler für Textänderungen im E-Mail-Feld.
    * Blendet den Platzhalter je nach Inhalt ein oder aus.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void MailBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderMail.Visibility = string.IsNullOrEmpty(MailBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    /**
    * Event-Handler für Änderungen im Klartext-Passwortfeld.
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
    * Event-Handler für den Zurück-zum-Login-Button. Löst das LoginButtonClickedNavLogin-Event aus.
    * Leitet zur LoginPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void YesLoginButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Zurück-zum-Login-Button wurde geklickt, Navigation zur LoginPage.");
        LoginButtonClickedNavLogin?.Invoke(this, EventArgs.Empty);
    }
}
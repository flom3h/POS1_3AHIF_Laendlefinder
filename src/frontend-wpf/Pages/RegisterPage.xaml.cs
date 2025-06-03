using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Laendlefinder.Pages;

public partial class RegisterPage : Page
{
    private bool passwordVisible = false;
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
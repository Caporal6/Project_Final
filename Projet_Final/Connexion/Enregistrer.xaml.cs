using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Singleton;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Connexion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Enregistrer : ContentDialog
    {
        public Enregistrer()
        {
            this.InitializeComponent();
        }

        public bool ReturnValue { get; set; }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private async void enregistrer_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

            // Vérifier le nom d'utilisateur
            if (NomUtilisateurTextBox.Text == "")
            {
                tbUsernameError.Text = "Le nom d'utilisateur est obligatoire";
                tbUsernameError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbUsernameError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            // Vérifier le mot de passe
            if (MotDePassePasswordBox.Password == "")
            {
                tbPasswordError.Text = "Le mot de passe est obligatoire";
                tbPasswordError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbPasswordError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (formValid)
            {
                bool login = SingletonAdministrateur.GetInstance().AjouterAdministrateur(NomUtilisateurTextBox.Text, MotDePassePasswordBox.Password);

                if (login)
                {

                    this.Hide();

                    ContentDialog dialog2 = new ContentDialog();
                    dialog2.XamlRoot = mainGrid.XamlRoot;
                    dialog2.Title = "Information";
                    dialog2.CloseButtonText = "OK";
                    dialog2.Content = "enregistrement réussi!";

                    ReturnValue = true;

                    var result = await dialog2.ShowAsync();

                }
                else
                {
                    tbPasswordError.Text = "Nom d'utilisateur ou mot de passe incorrect";
                    tbPasswordError.Visibility = Visibility.Visible;
                }
            }
        }
    }
}

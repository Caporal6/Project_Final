using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.ModuleProjet;
using Projet_Final.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Connexion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnexionProjet : ContentDialog
    {
        public ConnexionProjet()
        {
            this.InitializeComponent();
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void connexion_Click(object sender, RoutedEventArgs e)
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
                bool login = SingletonAdministrateur.GetInstance().VerifierAdministrateur(NomUtilisateurTextBox.Text, MotDePassePasswordBox.Password);

                if (login)
                {
                    this.Hide();
                    connexionFrame.Navigate(typeof(ListeProjet));
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

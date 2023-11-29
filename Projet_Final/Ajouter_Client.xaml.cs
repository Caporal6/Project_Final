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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Ajouter_Client : Page
    {
        public Ajouter_Client()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtNom.Text == "")
            {
                Nom.Text = "Erreur: Entrez un nom";
            }
            else if (txtAdresse.Text == "")
            {
                Adresse.Text = "Erreur: Entrez une adresse";
            }
            else if (txtTelephone.Text == "")
            {
                Telephone.Text = "Erreur: Entrez un numero de Telephone";
            }
            else if (txtEmail.Text == "")
            {
                Email.Text = "Erreur: Entrez un email";
            }
            else
            {
                SingletonClient.getInstance().ajouter_Client(txtNom.Text, txtAdresse.Text, txtTelephone.Text, txtEmail.Text);
            }





        }
    }
}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final
{
    public sealed partial class Modifier_Client : ContentDialog
    {
        string nom;
        string adresse;
        string num;
        string email;

        public Modifier_Client()
        {
            this.InitializeComponent();
        }

        public string Nom { get => nom;}
        public string Adresse { get => adresse;}
        public string Num { get => num;}
        public string Email { get => email;}

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {


            if (tbxNom.Text == "")
            {
                tbxNom.Text = "Erreur: Entrez un nom";
            }
            else if (tbxAdresse.Text == "")
            {
                tbxAdresse.Text = "Erreur: Entrez une adresse";
            }
            else if (tbxNum.Text == "")
            {
                tbxNum.Text = "Erreur: Entrez un numero de Telephone";
            }
            else if (tbxEmail.Text == "")
            {
                tbxEmail.Text = "Erreur: Entrez un email";
            }
            else
            {
                
                nom = tbxNom.Text;
                adresse = tbxAdresse.Text;
                num = tbxNum.Text;
                email = tbxEmail.Text;
                
            }
        }
    }
}

using Microsoft.UI;
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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

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

        internal void SetData(Client client)
        {
            tbxNom.Text = client.Nom;
            tbxAdresse.Text = client.Adresse;
            tbxNum.Text = client.Num;
            tbxEmail.Text = client.Email;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool formValid = true;

            args.Cancel = true;

            if (tbxNom.Text == "")
            {
                
                tbxNomErr.Text = "Le nom est obligatoire";
                tbxNomErr.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbxNomErr.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            if (tbxAdresse.Text == "")
            {
                tbxAdresseErr.Text = "L'adresse est obligatoire";
                tbxAdresseErr.Visibility = Visibility.Visible;
                formValid = formValid & false;

            }
            else
            {
                tbxAdresseErr.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            if (tbxNum.Text == "")
            {
               
                tbxNumErr.Text = "Le numero de telephone est obligatoire";
                tbxNumErr.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else {

                string phonePattern = @"^\d{10}$";
                Regex regex = new Regex(phonePattern);

                if (regex.IsMatch(tbxNum.Text.Trim()) == false)
                {
                    tbxNumErr.Text = "Entrer un numero de telephone valide";
                    tbxNumErr.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else {
                    tbxNumErr.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }
                


            if (tbxEmail.Text == "")
            {
                tbxEmailErr.Text = "L'email est obligatoire";
                tbxEmailErr.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);

                if (regex.IsMatch(tbxEmail.Text.Trim()) == false)
                {
                    tbxEmailErr.Text = "Entrer un email valide";
                    tbxEmailErr.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    tbxEmailErr.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }


            if(formValid) 
            {
                
                nom = tbxNom.Text;
                adresse = tbxAdresse.Text;
                num = tbxNum.Text;
                email = tbxEmail.Text;

                args.Cancel = false;

            }
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

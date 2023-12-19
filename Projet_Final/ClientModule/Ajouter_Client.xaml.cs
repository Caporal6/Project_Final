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
using Google.Protobuf.WellKnownTypes;
using Microsoft.UI;
using System.Text.RegularExpressions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Ajouter_Client : ContentDialog
    {
        public Ajouter_Client()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            bool formValid = true;


            if (txtNom.Text == "")
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


            if (txtAdresse.Text == "")
            {
                txtAdresseErr.Text = "L'adresse est obligatoire";
                txtAdresseErr.Visibility = Visibility.Visible;
                formValid = formValid & false;

            }
            else
            {
                txtAdresseErr.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            if (txtTelephone.Text == "")
            {

                txtTelephoneErr.Text = "Le numero de telephone est obligatoire";
                txtTelephoneErr.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {

                string phonePattern = @"^\d{10}$";
                Regex regex = new Regex(phonePattern);

                if (regex.IsMatch(txtTelephone.Text.Trim()) == false)
                {
                    txtTelephoneErr.Text = "Entrer un numero de telephone valide";
                    txtTelephoneErr.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    txtTelephoneErr.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }



            if (txtEmail.Text == "")
            {
                txtEmailErr.Text = "L'email est obligatoire";
                txtEmailErr.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);

                if (regex.IsMatch(txtEmail.Text.Trim()) == false)
                {
                    txtEmailErr.Text = "Entrer un email valide";
                    txtEmailErr.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    txtEmailErr.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }


            if (formValid)
            {

                SingletonClient.getInstance().ajouter_Client(txtNom.Text, txtAdresse.Text, txtTelephone.Text, txtEmail.Text);

                this.Hide();

                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = mainGrid.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Client ajouter avec success";

                var result = await dialog.ShowAsync();

            }

        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

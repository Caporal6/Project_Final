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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Information_Client : Page
    {

        public Information_Client()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {
                Client yup = (Client) e.Parameter;
                tbId.Text = yup.Id.ToString();
                tbNom.Text = yup.Nom.ToString();
                tbAdresse.Text = yup.Adresse.ToString();
                tbNum.Text = yup.Num.ToString();
                tbEmail.Text = yup.Email.ToString();
                
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Modifier_Client dialog = new Modifier_Client();
            dialog.XamlRoot = stkpnl.XamlRoot;
            dialog.Title = "Modifier un client";
            dialog.PrimaryButtonText = "Modifier";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;

            ContentDialogResult result = await dialog.ShowAsync();
            
            if(result == ContentDialogResult.Primary)
            {
              SingletonClient.getInstance().modifier_Client(tbId.Text,dialog.Nom, dialog.Adresse, dialog.Num, dialog.Email);
            }
        }





    }
}

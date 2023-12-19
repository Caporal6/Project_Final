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
using MySqlX.XDevAPI.Common;
using Projet_Final.Singleton;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.EmployeModule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuGestionClient : Page
    {
        public MenuGestionClient()
        {
            this.InitializeComponent();

            if (SingletonAdministrateur.GetInstance().online())
            {
                ajouterClientBorder.Visibility = Visibility.Visible;
            }
            else
            {
                ajouterClientBorder.Visibility = Visibility.Collapsed;
            }
        }

        private async void ajouterClient_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Ajouter_Client dialog = new Ajouter_Client();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.Title = "Ajouter un client";

            var result2 = await dialog.ShowAsync();

            if (result2 == ContentDialogResult.Primary)
            {
                this.Frame.Navigate(typeof(Afficher_Client));
            }
            else
            {
                this.Frame.Navigate(typeof(Afficher_Client));

            }
        }

        private void listClient_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Afficher_Client));
        }
    }
}

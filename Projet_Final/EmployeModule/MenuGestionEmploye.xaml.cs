using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.EmployeModule;
using Projet_Final.Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Employe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuGestionEmploye : Page
    {
        public MenuGestionEmploye()
        {
            
            this.InitializeComponent();

            if (SingletonAdministrateur.GetInstance().online())
            {
                ajouteEmploye.Visibility = Visibility.Visible;
            }
            else
            {
                ajouteEmploye.Visibility = Visibility.Collapsed;
            }
        }

        private void listEmploye_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListeEmploye));
        }

        private async void ajouterEmploye_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FourmulaireAjout dialog = new FourmulaireAjout();
            dialog.XamlRoot = mainGrid.XamlRoot;
           
            var result = await dialog.ShowAsync();
        }

        private void listEmployeBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}

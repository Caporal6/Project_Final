using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Employe;
using Projet_Final.EmployeModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.ModuleProjet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuGestionProjet : Page
    {
        public MenuGestionProjet()
        {
            this.InitializeComponent();
        }

        private void listProjet_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListeProjet));
        }

        private async void ajouterProjet_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FormulaireAjoutProjet dialog = new FormulaireAjoutProjet();
            dialog.XamlRoot = mainGrid.XamlRoot;

            var result = await dialog.ShowAsync();
        }
    }
}
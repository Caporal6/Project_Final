using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.EmployeModule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailEmploye : Page
    {

        string MatriculeEmploye = "";
        public DetailEmploye()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {

                EmployeC employe = SingletonEmploye.GetInstance().RetourneUnEmploye(e.Parameter as String);

                MatriculeEmploye = employe.Matricule;

                tbNom.Text = employe.Nom;

            }
        }

        private async void modifierEmployer_Click(object sender, RoutedEventArgs e)
        {
            EmployeC employe = SingletonEmploye.GetInstance().RetourneUnEmploye(MatriculeEmploye);
            FormulaireModifier dialog = new FormulaireModifier();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.SetData(employe);

            var result = await dialog.ShowAsync();
        }
    }
}

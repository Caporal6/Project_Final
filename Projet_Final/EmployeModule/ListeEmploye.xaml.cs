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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using Projet_Final.EmployeModule;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Employe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListeEmploye : Page
    {
        ObservableCollection<EmployeC> listeEmployes = SingletonListeBD.GetInstance().ListeEmployees();
        int index = 0;
        Boolean cliked = false;
        public ListeEmploye()
        {
            this.InitializeComponent();

            Console.WriteLine(listeEmployes.Count.ToString());
        }

        private void gvListeEmploye_ItemClick(object sender, ItemClickEventArgs e)
        {

            cliked = true;
        }

        private void gvListeEmploye_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cliked)
            {
                index = gvListeEmploye.SelectedIndex;
                this.Frame.Navigate(typeof(DetailEmploye), listeEmployes[index].Matricule);

            }
            cliked = false;
        }
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Classes;
using Projet_Final.Employe;
using Projet_Final.EmployeModule;
using Projet_Final.Singleton;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ListeProjet : Page
    {
        ObservableCollection<ProjetClient> listeProjetsClient = SingletonProjetClient.GetInstance().ListeProjetsAvecClients();
        int index = 0;
        Boolean cliked = false;

        public ListeProjet()
        {
            listeProjetsClient.Clear();
            listeProjetsClient = SingletonProjetClient.GetInstance().ListeProjetsAvecClients();
            this.InitializeComponent();
        }

        private void gvListeProjet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cliked)
            {
                index = gvListeProjet.SelectedIndex;
                this.Frame.Navigate(typeof(Afficher_Projet), listeProjetsClient[index].NumeroProjet);

            }
            cliked = false;
        }

        private void gvListeProjet_ItemClick(object sender, ItemClickEventArgs e)
        {
            cliked = true;
        }

        private void retour_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuGestionProjet));
        }
    }
}

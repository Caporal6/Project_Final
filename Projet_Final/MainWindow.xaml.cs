using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Employe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(Afficher_Projet));
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            var item = (NavigationViewItem)args.SelectedItem;

            switch (item.Name)
            {
                case "AjProjet":
                    mainFrame.Navigate(typeof(Ajouter_Projet));
                    break;
                case "AfProjet":
                    mainFrame.Navigate(typeof(Afficher_Projet));
                    break;
                case "AjClient":
                    mainFrame.Navigate(typeof(Ajouter_Client));
                    break;
                case "AfClient":
                    mainFrame.Navigate(typeof(Afficher_Client));
                    break;
                case "AjEmployer":
                    mainFrame.Navigate(typeof(Ajouter_Employer));
                    break;
                case "AfEmployer":
                    mainFrame.Navigate(typeof(Afficher_Employer));
                    break;
                case "gestionEmploye":
                    mainFrame.Navigate(typeof(MenuGestionEmploye));
                    break;
                default:
                    break;
            }




        }

        private void navView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if(mainFrame.CanGoBack)
            {
                
                mainFrame.GoBack();
            }
        }
    }
}

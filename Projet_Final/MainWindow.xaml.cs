using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Employe;
using Projet_Final.ModuleProjet;
using Projet_Final.Singleton;
using Projet_Final.Connexion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MySqlX.XDevAPI.Common;
using Projet_Final.EmployeModule;
using System.Diagnostics;

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
            if (SingletonAdministrateur.GetInstance().online())
            {
                connexionProjet.Content = "Deconnexion";
            }
            else
            {
                connexionProjet.Content = "Connexion";
            }

            mainFrame.Navigate(typeof(ListeProjet));
        }



        private async void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
                case "gestionEmploye":
                    mainFrame.Navigate(typeof(MenuGestionEmploye));
                    break;
                case "gestionProjet":
                    mainFrame.Navigate(typeof(MenuGestionProjet));
                    break;
                case "connexionProjet":
                    {

                        if(SingletonAdministrateur.GetInstance().online() == false)
                        {
                            ConnexionProjet dialog = new ConnexionProjet();
                            dialog.XamlRoot = navView.XamlRoot;
                            //dialog.PrimaryButtonText = "Connexion";

                            var resultat = await dialog.ShowAsync();

                            // Acc�dez � la valeur retourn�e apr�s la fermeture du ContentDialog
                            if (resultat == ContentDialogResult.Primary)
                            {
                                bool returnedValue = dialog.ReturnValue;
                                if (returnedValue)
                                {
                                    mainFrame.Navigate(typeof(ListeProjet));
                                }
                            }
                            else
                            {
                                bool returnedValue = dialog.ReturnValue;
                                if (returnedValue)
                                {
                                    ChangerElementSelectionne("gestionProjet");
                                    connexionProjet.Content = "Deconnexion";
                                    
                                }
                            }
                        }
                        else
                        {
                            SingletonAdministrateur.GetInstance().deconnexion();
                            ContentDialog dialog2 = new ContentDialog();
                            dialog2.XamlRoot = navView.XamlRoot;
                            dialog2.Title = "Information";
                            dialog2.CloseButtonText = "OK";
                            dialog2.Content = "Deconnexion r�ussi!";

                            var result = await dialog2.ShowAsync();

                            connexionProjet.Content = "Connexion";
                            ChangerElementSelectionne("gestionProjet");


                        }



                        //if (resultat == ContentDialogResult.Primary)
                        //{


                        //    ContentDialog dialog2 = new ContentDialog();
                        //    dialog.XamlRoot = navView.XamlRoot;
                        //    dialog.Title = "Information";
                        //    dialog.CloseButtonText = "OK";
                        //    dialog.Content = "connexion r�ussi!";

                        //    var result = await dialog2.ShowAsync();

                        //    mainFrame.Navigate(typeof(ListeProjet));
                        //}

                    }
                    break;
                default:
                    break;
            }



        }

        // Supposez que "navView" est votre NavigationView

        private void ChangerElementSelectionne(string nomElement)
        {
            foreach (var item in navView.MenuItems.OfType<NavigationViewItem>())
            {
                if (item.Name == nomElement)
                {
                    // S�lectionnez manuellement l'�l�ment de menu
                    navView.SelectedItem = item;
                    break;
                }
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

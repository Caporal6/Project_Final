using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Classes;
using Projet_Final.EmployeModule;
using Projet_Final.ModuleProjet;
using Projet_Final.Singleton;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class Afficher_Projet : Page
    {
        ObservableCollection<EmployeProjet> listeEmployeProjet;
        ObservableCollection<ProjetClient> listeProjetClient;
        public Afficher_Projet()
        {
            this.InitializeComponent();

            if (SingletonAdministrateur.GetInstance().online())
            {
                assigner.Visibility = Visibility.Visible;
            }
            else
            {
                assigner.Visibility = Visibility.Collapsed;
            }

            if (SingletonAdministrateur.GetInstance().online())
            {
                modifierProjet.Visibility = Visibility.Visible;
            }
            else
            {
                modifierProjet.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {

                Projet projet = SingletonProjet.GetInstance().RetourneProjetParNumero(e.Parameter as String);
                listeEmployeProjet = SingletonEmployeProjet.GetInstance().RetournelesEmployeLierAuProjet(e.Parameter as String);

                
                listeProjetClient = SingletonProjetClient.GetInstance().ListeProjetsAvecClients();

                ProjetClient projetClient =  listeProjetClient.FirstOrDefault(p => p.IdentifiantClient == projet.ClientIdentifiant);

                Debug.WriteLine(listeProjetClient[0].ToString());

                NumeroProjetTextBlock.Text = projet.NumeroProjet;
                TitreTextBlock.Text = projet.Titre;
                DateDebutTextBlock.Text = "Date de Début: " + projet.DateDebut.ToString("dd MMMM yyyy");
                DescriptionTextBlock.Text = projet.Description;
                BudgetTextBlock.Text = projet.Budget.ToString();
                EmployesRequisTextBlock.Text = projet.EmployesRequis.ToString();
                TotalSalairesTextBlock.Text = projet.TotalSalaires.ToString();
                ClientIdentifiantTextBlock.Text = projetClient.NomClient.ToString();
                StatutTextBlock.Text = projet.Statut.ToString();

            }
        }

        private async void assigner_Click(object sender, RoutedEventArgs e)
        {
            if(listeEmployeProjet.Count <= Convert.ToInt32(EmployesRequisTextBlock.Text)) {

                FormulaireAssignation dialog = new FormulaireAssignation();
                dialog.XamlRoot = mainStack.XamlRoot;
                dialog.SetData(NumeroProjetTextBlock.Text);

                dialog.ParentPageReference = this;

                var result = await dialog.ShowAsync();

                // Accédez à la valeur retournée après la fermeture du ContentDialog
                if (result == ContentDialogResult.Primary)
                {

                    bool returnedValue = dialog.ReturnValue;

                    if (returnedValue)
                    {

                        this.Frame.Navigate(typeof(Afficher_Projet), NumeroProjetTextBlock.Text);

                    }

                }
                else
                {
                    bool returnedValue = dialog.ReturnValue;

                    if (returnedValue)
                    {
                        this.Frame.Navigate(typeof(Afficher_Projet), NumeroProjetTextBlock.Text);

                    }

                }
            }
            else
            {
                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = mainStack.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "La limite d'employer a ete atteinte";

                var result = await dialog.ShowAsync();
            }
            
        }

        public void RefreshEmployeeList()
        {
            // Mettez à jour la liste des employés liés au projet
            listeEmployeProjet = SingletonEmployeProjet.GetInstance().RetournelesEmployeLierAuProjet(NumeroProjetTextBlock.Text);

            // Mettez à jour votre interface utilisateur, par exemple, en réassignant la source de données
            // pour la ListView ou tout autre contrôle que vous utilisez pour afficher la liste des employés.
            // Exemple hypothétique :
            gvListeEmployeProjet.ItemsSource = listeEmployeProjet;
        }

        private void retour_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListeProjet));
        }

        private async void modifierProjet_Click(object sender, RoutedEventArgs e)
        {
            Projet projet = SingletonProjet.GetInstance().RetourneProjetParNumero(NumeroProjetTextBlock.Text);
            FormulaireModificationProjet dialog = new FormulaireModificationProjet();
            dialog.XamlRoot = mainStack.XamlRoot;
            dialog.SetData(projet);

            var result = await dialog.ShowAsync();

            // Accédez à la valeur retournée après la fermeture du ContentDialog
            if (result == ContentDialogResult.Primary)
            {
                
                bool returnedValue = dialog.ReturnValue;
               
                if (returnedValue)
                {

                    this.Frame.Navigate(typeof(Afficher_Projet), NumeroProjetTextBlock.Text);

                }

            }
            else
            {
                bool returnedValue = dialog.ReturnValue;

                if (returnedValue)
                {
                    this.Frame.Navigate(typeof(Afficher_Projet), NumeroProjetTextBlock.Text);

                }

            }
        }
    }
}

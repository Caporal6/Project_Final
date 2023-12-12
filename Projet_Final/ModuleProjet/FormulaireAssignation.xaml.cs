using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
    public sealed partial class FormulaireAssignation : ContentDialog
    {
        ObservableCollection<EmployeC> listeEmployes = SingletonEmploye.GetInstance().ListeEmployees();
        string idEmploye = "";
        public FormulaireAssignation()
        {
            this.InitializeComponent();
        }

        private void Employe_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var client in listeEmployes)
                {
                    var found = splitText.All((key) =>
                    {
                        return client.Nom.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(client.Nom);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        private void Employe_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            EmployeC employe = listeEmployes.FirstOrDefault(client => client.Nom == args.SelectedItem.ToString());

            idEmploye = employe.Matricule;
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void assigner_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

            //titre

            if (idEmploye == "")
            {
                employeRequisError.Text = "L'employe est obligatoire";
                employeRequisError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                employeRequisError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            //nombre d'heures

            if (tbHeures.Text == "")
            {
                tbHeuresError.Visibility = Visibility.Visible;
                tbHeuresError.Text = "Le nombre d'heures est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                tbHeuresError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if(formValid)
            {

                SingletonProjet.GetInstance().AssignerEmployeAProjet(idEmploye, numeroProjet, tbHeures.Text);
            }
        }
    }
}

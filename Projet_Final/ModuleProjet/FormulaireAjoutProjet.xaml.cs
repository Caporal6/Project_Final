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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.ModuleProjet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormulaireAjoutProjet : ContentDialog
    {
        
        ObservableCollection<Client> listeClients = SingletonClient.getInstance().afficher_Client();
        bool dateDebutChange = false;
        int idClient = 0;
        public FormulaireAjoutProjet()
        {
            this.InitializeComponent();

            Debug.WriteLine(listeClients.Count);
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private async void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

            //titre

            if (tbTitre.Text == "")
            {
                tbTitreError.Text = "Le titre est obligatoire";
                tbTitreError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbTitreError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            //date debut

            if (!dateDebutChange)
            {
                dpDateDebutError.Visibility = Visibility.Visible;
                dpDateDebutError.Text = "La date de debut est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                dpDateDebutError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            // description

            if (tbDescription.Text == "")
            {
                tbDescriptionError.Text = "L'email est obligatoire";
                tbDescriptionError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbDescriptionError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            // budget

            if (nbBudget.Value.ToString() == "" || nbBudget.Text == "")
            {

                nbBudgetError.Text = "Le Taux budget est obligatoire";
                nbBudgetError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                if (nbBudget.Value < 0)
                {
                    nbBudgetError.Text = "Le budget ne peut etre une valeur negative";
                    nbBudgetError.Visibility = Visibility.Visible;
                    formValid = formValid & false;

                }
                else if (nbBudget.Value < 30)
                {
                    nbBudgetError.Text = "Le budget ne peut etre inferieur a 30";
                    nbBudgetError.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    nbBudgetError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }


            // nb employe requis

            if (nbEmployeRequis.Value.ToString() == "" || nbEmployeRequis.Text == "")
            {

                nbEmployeRequisError.Text = "Le nombre d'employe requis est obligatoire";
                nbEmployeRequisError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                if (nbEmployeRequis.Value < 0)
                {
                    nbEmployeRequisError.Text = "Le nombre d'employe requis ne peut etre une valeur negative";
                    nbEmployeRequisError.Visibility = Visibility.Visible;
                    formValid = formValid & false;

                }
                else if (nbEmployeRequis.Value > 5)
                {
                    nbEmployeRequisError.Text = "Le nombre d'employe requis ne peut etre superieur a 5";
                    nbEmployeRequisError.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    nbEmployeRequisError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }

            //date debut

            if (client.Text == "")
            {
                clientRequisError.Visibility = Visibility.Visible;
                clientRequisError.Text = "Le client est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                clientRequisError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }



            if (formValid == true)
            {

                Projet projet = new Projet
                {
                    Titre = tbTitre.Text,
                    DateDebut = dpDateDebut.Date.DateTime,
                    Description = tbDescription.Text,
                    Budget =  Convert.ToInt32(nbBudget.Text),
                    EmployesRequis = Convert.ToInt32(nbEmployeRequis.Text),
                    ClientIdentifiant = Convert.ToInt32(idClient),
                };


                SingletonProjet.GetInstance().AjouterProjet(projet);

                this.Hide();

                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Projet ajouter avec success";

                var result = await dialog.ShowAsync();

            }
        }

        private void dpDateDebut_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dateDebutChange = true;
        }

        private void client_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var client in listeClients)
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

        private void client_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //SuggestionOutput.Text = args.SelectedItem.ToString();

            Client clientRecherche = listeClients.FirstOrDefault(client => client.Nom == args.SelectedItem.ToString());

            idClient = Convert.ToInt32(clientRecherche.Id);

            Debug.WriteLine(idClient);

        }
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySqlX.XDevAPI;
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

namespace Projet_Final.ModuleProjet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormulaireModificationProjet : ContentDialog
    {

        String NumeroProjet = "";
        string statut = "";
        public FormulaireModificationProjet()
        {
            this.InitializeComponent();
        }

        public bool ReturnValue { get; set; }

        internal void SetData(Projet projet)
        {
            NumeroProjet = projet.NumeroProjet;
            tbTitre.Text = projet.Titre;
            tbDescription.Text = projet.Description;
            nbBudget.Text = projet.Budget.ToString();
            nbBudget.Value = projet.Budget;
            cbStatut.SelectedItem = projet.Statut;
        }

        private void cbStatut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statut = cbStatut.SelectedItem as String;
        }

        private async void btnModifier_Click(object sender, RoutedEventArgs e)
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

            if (statut == "")
            {
                cbStatutError.Visibility = Visibility.Visible;
                cbStatutError.Text = "Le statut est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                cbStatutError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (formValid == true)
            {

                Projet projet = new Projet
                {
                    NumeroProjet = NumeroProjet,
                    Titre = tbTitre.Text,
                    Description = tbDescription.Text,
                    Budget = Convert.ToInt32(nbBudget.Text),
                    Statut = statut,
                };


                String message  = SingletonProjet.GetInstance().ModifierProjet(projet);

                this.Hide();

                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = message;

                ReturnValue = true;

                var result = await dialog.ShowAsync();

            }
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}

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
using System.Text.RegularExpressions;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.EmployeModule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FourmulaireAjout : ContentDialog
    {
        Boolean dateNaissanceChange = false;
        Boolean dateEmbaucheChange = false;
        String statut = "";
        public FourmulaireAjout()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void dpDateEmbauche_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dateEmbaucheChange = true;
        }

        private void cbStatut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statut = cbStatut.SelectedItem as String;
        }

        private async void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

            Debug.WriteLine("-----------------------------------");

            Debug.WriteLine(formValid);

            //nom

            if (tbNom.Text == "")
            {
                tbNomError.Text = "Le nom est obligatoire";
                tbNomError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbNomError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            //prenom


            if (tbPrenom.Text == "")
            {
                tbPrenomError.Text = "Le prenom est obligatoire";
                tbPrenomError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbPrenomError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            //date de naissance

            if (dateNaissanceChange == false)
            {
                dpDateNaissanceError.Visibility = Visibility.Visible;
                dpDateNaissanceError.Text = "La date de naissance est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                dpDateNaissanceError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            // email

            if (tbEmail.Text == "")
            {
                tbEmailError.Text = "L'email est obligatoire";
                tbEmailError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbEmailError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            // adresse

            if (tbAdresse.Text == "")
            {
                tbAdresseError.Text = "L'adresse est obligatoire";
                tbAdresseError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbAdresseError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            // date embauche

            if (dateEmbaucheChange == false)
            {
                dpDateEmbaucheError.Visibility = Visibility.Visible;
                dpDateEmbaucheError.Text = "La date d'embauche est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                dpDateEmbaucheError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            Debug.WriteLine(formValid);

            // taux horraire

            if (nbTauxHorraire.Value.ToString() == "" || nbTauxHorraire.Text == "")
            {

                nbTauxHorraireError.Text = "Le Taux horraire est obligatoire";
                nbTauxHorraireError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                if (nbTauxHorraire.Value < 0 || nbTauxHorraire.Value < 15)
                {
                    nbTauxHorraireError.Text = "Le taux horraire doit etre compris entre 0 et 15";
                    nbTauxHorraireError.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    nbTauxHorraireError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }

            Debug.WriteLine(formValid);

            // photo identite

            Regex regex = new Regex(@"\.(png|jpg)$", RegexOptions.IgnoreCase);

            if (tbPhotoIdentite.Text == "")
            {
                tbPhotoIdentiteError.Visibility = Visibility.Visible;
                tbPhotoIdentiteError.Text = "La photo est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                if (regex.IsMatch(tbPhotoIdentite.Text) == false)
                {
                    tbPhotoIdentiteError.Visibility = Visibility.Visible;
                    tbPhotoIdentiteError.Text = "L'image doit etre dans le format png ou jpg";
                    formValid = formValid & false;
                }
                else
                {
                    tbPhotoIdentiteError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }

            Debug.WriteLine(formValid);

            // statut

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

            Debug.WriteLine(formValid);

            if (formValid == true)
            {

                Debug.WriteLine("cool");

                EmployeC employe = new EmployeC
                {
                    Nom = tbNom.Text,
                    Prenom = tbPrenom.Text,
                    DateNaissance = dpDateNaissance.Date.DateTime,
                    Email = tbEmail.Text,
                    Adresse = tbAdresse.Text,
                    DateEmbauche = dpDateEmbauche.Date.DateTime,
                    TauxHoraire = Convert.ToInt32(nbTauxHorraire.Text),
                    PhotoIdentite = tbPhotoIdentite.Text,
                    Statut = statut
                };

                SingletonListeBD.GetInstance().Ajouter(employe);

                this.Hide();

                ContentDialog dialog = new ContentDialog();
                
                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Employe ajouter avec success";

                var result = await dialog.ShowAsync();

            }
        }

        private void dpDateNaissance_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dateNaissanceChange = true;
        }
    }
}

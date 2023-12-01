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

        // Fonction pour calculer l'âge à partir de la date de naissance
        public int CalculateAge(DateTime dateOfBirth)
        {
            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - dateOfBirth.Year;

            if (currentDate < dateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }

        private async void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

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


            //date de naissance

            if (!dateNaissanceChange)
            {
                dpDateNaissanceError.Visibility = Visibility.Visible;
                dpDateNaissanceError.Text = "La date de naissance est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                dpDateNaissanceError.Visibility = Visibility.Collapsed;

                // Vérification de l'âge entre 18 et 65 ans
                int age = CalculateAge(dpDateNaissance.Date.DateTime);

                if (age < 18 || age > 65)
                {
                    dpDateNaissanceError.Visibility = Visibility.Visible;
                    dpDateNaissanceError.Text = "Veuillez entrer une date valide qui donne a l'employe entre 18 et 65 ans";
                    formValid = formValid & false;
                }
                else
                {
                    dpDateNaissanceError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }

            


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


            // date embauche

            if (!dateEmbaucheChange)
            {
                dpDateEmbaucheError.Visibility = Visibility.Visible;
                dpDateEmbaucheError.Text = "La date d'embauche est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                dpDateEmbaucheError.Visibility = Visibility.Collapsed;

                if (dpDateEmbauche.Date.DateTime > DateTime.Today)
                {
                    dpDateEmbaucheError.Visibility = Visibility.Visible;
                    dpDateEmbaucheError.Text = "La date d'embauche ne peut pas être postérieure à la date du jour";
                    formValid = formValid & false;
                }
                else
                {
                    dpDateEmbaucheError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }


            // taux horraire

            if (nbTauxHorraire.Value.ToString() == "" || nbTauxHorraire.Text == "")
            {

                nbTauxHorraireError.Text = "Le Taux horraire est obligatoire";
                nbTauxHorraireError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                if (nbTauxHorraire.Value < 0)
                {
                    nbTauxHorraireError.Text = "Le taux horraire ne peut etre une valeur negative";
                    nbTauxHorraireError.Visibility = Visibility.Visible;
                    formValid = formValid & false;

                }else if ( nbTauxHorraire.Value > 30)
                {
                    nbTauxHorraireError.Text = "Le taux horraire ne peut etre supperieur a 30";
                    nbTauxHorraireError.Visibility = Visibility.Visible;
                    formValid = formValid & false;
                }
                else
                {
                    nbTauxHorraireError.Visibility = Visibility.Collapsed;
                    formValid = formValid & true;
                }
            }


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

                SingletonEmploye.GetInstance().AjouterEmploye(employe);

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

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

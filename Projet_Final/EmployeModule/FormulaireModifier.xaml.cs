using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.EmployeModule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormulaireModifier : ContentDialog
    {
        String statut = "";
        String ancienStatut = "";
        DateTime dateEmbauche = DateTime.MinValue;
        String Matricule = "";
        
        public FormulaireModifier()
        {
            this.InitializeComponent();
        }

        public bool ReturnValue { get; set; }

        internal void SetData(EmployeC employe)
        {
            Matricule = employe.Matricule;
            tbNom.Text = employe.Nom;
            tbPrenom.Text = employe.Prenom;
            //dpDateNaissance.Date = employe.DateNaissance;
            tbEmail.Text = employe.Email;
            tbAdresse.Text = employe.Adresse;
            //dpDateEmbauche.Date = employe.DateEmbauche;
            dateEmbauche = employe.DateEmbauche;
            nbTauxHorraire.Value = employe.TauxHoraire;
            tbPhotoIdentite.Text = employe.PhotoIdentite;
            cbStatut.SelectedItem = employe.Statut;
            ancienStatut = employe.Statut.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private async void btnModifier_Click(object sender, RoutedEventArgs e)
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

                }
                else if (nbTauxHorraire.Value > 30)
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

            if (string.IsNullOrEmpty(statut))
            {
                cbStatutError.Visibility = Visibility.Visible;
                cbStatutError.Text = "Le statut est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                cbStatutError.Visibility = Visibility.Collapsed;

                // Vérification si le changement de statut est valide
                if (statut == "Journalier" && ancienStatut == "Permanent")
                {
                    // Impossible de passer de Permanent à Journalier
                    cbStatutError.Visibility = Visibility.Visible;
                    cbStatutError.Text = "Impossible de passer de Permanent à Journalier";
                    formValid = formValid & false;
                }
                else if (statut == "Permanent" && ancienStatut == "Journalier")
                {

                    // Calcul de l'ancienneté en années
                    int anciennete = (DateTime.Today - dateEmbauche).Days / 365;

                    // Vérification si l'ancienneté est inférieure à 3 ans
                    if (anciennete < 3)
                    {
                        cbStatutError.Visibility = Visibility.Visible;
                        cbStatutError.Text = "L'employé n'a pas l'ancienneté suffisante pour cette opération";
                        formValid = formValid & false;
                    }
                    else
                    {
                        cbStatutError.Visibility = Visibility.Collapsed;
                        formValid = formValid & true;
                    }
                }
                else
                {
                    // Aucune vérification spécifique pour les autres changements de statut
                    formValid = formValid & true;
                }
            }


            //if (statut == "")
            //{
            //    cbStatutError.Visibility = Visibility.Visible;
            //    cbStatutError.Text = "Le statut est obligatoire";
            //    formValid = formValid & false;
            //}
            //else
            //{
            //    cbStatutError.Visibility = Visibility.Collapsed;
            //    formValid = formValid & true;
            //}


            if (formValid == true)
            {

                Debug.WriteLine("cool");

                EmployeC employe = new EmployeC
                {
                    Matricule = Matricule,
                    Nom = tbNom.Text,
                    Prenom = tbPrenom.Text,
                    Email = tbEmail.Text,
                    Adresse = tbAdresse.Text,
                    TauxHoraire = Convert.ToInt32(nbTauxHorraire.Text),
                    PhotoIdentite = tbPhotoIdentite.Text,
                    Statut = statut
                };

                SingletonEmploye.GetInstance().ModifierInformationsEmploye(employe);

                this.Hide();

                ContentDialog dialog = new ContentDialog();

                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Modification effectuer avec success";

                ReturnValue = true;

                var result = await dialog.ShowAsync();              

                

            }
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //private void dpDateNaissance_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        //{

        //}

        //private void dpDateEmbauche_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        //{

        //}

        private void cbStatut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statut = cbStatut.SelectedItem as String;
        }
    }
}

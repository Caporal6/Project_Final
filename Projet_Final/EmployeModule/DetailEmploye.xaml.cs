using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Employe;
using Projet_Final.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.EmployeModule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailEmploye : Page
    {

        string MatriculeEmploye = "";
        public DetailEmploye()
        {
            this.InitializeComponent();

            if (SingletonAdministrateur.GetInstance().online())
            {
                modifierEmployer.Visibility = Visibility.Visible;
            }
            else
            {
                modifierEmployer.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {

                EmployeC employe = SingletonEmploye.GetInstance().RetourneUnEmploye(e.Parameter as String);

                MatriculeEmploye = employe.Matricule;
                imgPhotoIdentite.Source = new BitmapImage(new Uri(employe.PhotoIdentite));
                tbNom.Text = employe.Nom;
                tbPrenom.Text = employe.Prenom;
                tbDateNaissance.Text = employe.DateNaissance.ToString("dd MMMM yyyy");
                tbEmail.Text = employe.Email;
                tbAdresse.Text = employe.Adresse;   
                tbDateEmbauche.Text =  employe.DateEmbauche.ToString("dd MMMM yyyy");
                tbTauxHorraire.Text = employe.TauxHoraire.ToString()+"$";
                tbStatut.Text = employe.Statut;

            }
        }

        private async void modifierEmployer_Click(object sender, RoutedEventArgs e)
        {
            EmployeC employe = SingletonEmploye.GetInstance().RetourneUnEmploye(MatriculeEmploye);
            FormulaireModifier dialog = new FormulaireModifier();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.SetData(employe);

            var result = await dialog.ShowAsync();

            // Acc�dez � la valeur retourn�e apr�s la fermeture du ContentDialog
            if (result == ContentDialogResult.Primary)
            {
                Debug.WriteLine("test3");
                bool returnedValue = dialog.ReturnValue;
                Debug.WriteLine("test4");
                if (returnedValue) {
                    Debug.WriteLine("test5");
                    this.Frame.Navigate(typeof(DetailEmploye), MatriculeEmploye);
                    Debug.WriteLine("test6");
                }
                Debug.WriteLine("test7");
            }
            else
            {
                Debug.WriteLine("test8");
                bool returnedValue = dialog.ReturnValue;
                Debug.WriteLine("test9");
                if (returnedValue)
                {
                    Debug.WriteLine("test10");
                    this.Frame.Navigate(typeof(DetailEmploye), MatriculeEmploye);
                    Debug.WriteLine("test11");
                }
                Debug.WriteLine("test12");
            }
        }

        private void AssocierAProjet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void retour_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListeEmploye));
        }
    }
}
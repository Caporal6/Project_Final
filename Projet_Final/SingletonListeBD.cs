using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    internal class SingletonListeBD
    {
        ObservableCollection<EmployeC> liste;
        MySqlConnection con;
        static SingletonListeBD instance = null;

        // Constructeur de la classe
        public SingletonListeBD()
        {
            liste = new ObservableCollection<EmployeC>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420326_gr01_2204989-yousouf-esdras-manefa;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonListeBD GetInstance()
        {
            if (instance == null)
                instance = new SingletonListeBD();

            return instance;
        }

        public ObservableCollection<EmployeC> ListeEmployees()
        {
            liste.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("GetEmployeeList");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                EmployeC Employe;
                while (r.Read())
                {
                    Employe = new EmployeC
                    {
                        Matricule = r["Matricule"] as String,
                        Nom = r["Nom"] as String,
                        Prenom = r["Prenom"] as String,
                        DateNaissance = Convert.ToDateTime(r["DateNaissance"]),
                        Email = r["Email"] as String,
                        Adresse = r["Adresse"] as String,
                        DateEmbauche = Convert.ToDateTime(r["DateEmbauche"]),
                        TauxHoraire = Convert.ToDouble(r["TauxHoraire"], CultureInfo.InvariantCulture),
                        PhotoIdentite = r["PhotoIdentite"] as String,
                        Statut = r["Statut"] as String,
                        ProjetId = r["ProjetId"] as String,
                    };

                    liste.Add(Employe);
                }

                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return liste;
        }

        // Ajoute un Employe dans la BD
        public void Ajouter(EmployeC employe)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("AjouterEmploye");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("@p_Nom", employe.Nom);
                commande.Parameters.AddWithValue("@p_Prenom", employe.Prenom);
                commande.Parameters.AddWithValue("@p_DateNaissance", employe.DateNaissance);
                commande.Parameters.AddWithValue("@p_Email", employe.Email);
                commande.Parameters.AddWithValue("@p_Adresse", employe.Adresse);
                commande.Parameters.AddWithValue("@p_DateEmbauche", employe.DateNaissance);
                commande.Parameters.AddWithValue("@p_TauxHoraire", employe.TauxHoraire);
                commande.Parameters.AddWithValue("@p_PhotoIdentite", employe.PhotoIdentite);
                commande.Parameters.AddWithValue("@p_Statut", employe.Statut);

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
            }

            liste.Add(employe);
        }
    }
}

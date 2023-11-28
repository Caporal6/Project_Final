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
                //commande.CommandText = "Select * from produits";
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
                        DateNaissance = r["DateNaissance"] as String,
                        Email = r["Email"] as String,
                        Adresse = r["Adresse"] as String,
                        DateEmbauche = r["DateEmbauche"] as String,
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
    }
}

using MySql.Data.MySqlClient;
using Projet_Final.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Singleton
{
    internal class SingletonProjet
    {
        ObservableCollection<Projet> liste;
        MySqlConnection con;
        static SingletonProjet instance = null;

        // Constructeur de la classe
        public SingletonProjet()
        {
            liste = new ObservableCollection<Projet>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq23;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonProjet GetInstance()
        {
            if (instance == null)
                instance = new SingletonProjet();

            return instance;
        }

        public ObservableCollection<Projet> ListeProjets()
        {
            liste.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("GetAllProjects");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                Projet Projet;
                while (r.Read())
                {
                    Projet = new Projet
                    {
                        NumeroProjet = r["NumeroProjet"] as String,
                        Titre = r["Titre"] as String,
                        DateDebut = Convert.ToDateTime(r["DateDebut"]),
                        Description = r["Description"] as String,
                        Budget = Convert.ToDouble(r["Budget"]),
                        EmployesRequis = Convert.ToInt32(r["EmployesRequis"]),
                        TotalSalaires = Convert.ToDouble(r["TotalSalaires"]),
                        ClientIdentifiant = Convert.ToInt32(r["ClientIdentifiant"]),
                        Statut = r["Statut"] as String
                    };

                    liste.Add(Projet);
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


        // Ajoute un Projet dans la BD
        public void AjouterProjet(Projet projet)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("CreerProjet");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("@p_Titre", projet.Titre);
                commande.Parameters.AddWithValue("@p_DateDebut", projet.DateDebut);
                commande.Parameters.AddWithValue("@p_Description", projet.Description);
                commande.Parameters.AddWithValue("@p_Budget", projet.Budget);
                commande.Parameters.AddWithValue("@p_EmployesRequis", projet.EmployesRequis);
                commande.Parameters.AddWithValue("@p_ClientIdentifiant", projet.ClientIdentifiant);
                

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

            liste.Add(projet);
        }


        // Retourne un projet grace a son numero de projet
        public Projet RetourneProjetParNumero(String NumeroProjet)
        {
            Projet projet = new Projet();

            try
            {
                MySqlCommand commande = new MySqlCommand("GetProjetByNumero");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("@p_NumeroProjet", NumeroProjet);

                con.Open();
                commande.Prepare();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    projet = new Projet
                    {
                        NumeroProjet = r["NumeroProjet"] as String,
                        Titre = r["Titre"] as String,
                        DateDebut = Convert.ToDateTime(r["DateDebut"]),
                        Description = r["Description"] as String,
                        Budget = Convert.ToDouble(r["Budget"], CultureInfo.InvariantCulture),
                        EmployesRequis = Convert.ToInt32(r["EmployesRequis"]),
                        TotalSalaires = Convert.ToDouble(r["TotalSalaires"], CultureInfo.InvariantCulture),
                        ClientIdentifiant = Convert.ToInt32(r["ClientIdentifiant"]),
                        Statut = r["Statut"] as String
                    };
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

            return projet;
        }

    }
}

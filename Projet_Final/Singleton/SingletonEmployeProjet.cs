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
    internal class SingletonEmployeProjet
    {
        ObservableCollection<EmployeProjet> liste;
        MySqlConnection con;
        static SingletonEmployeProjet instance = null;

        // Constructeur de la classe
        public SingletonEmployeProjet()
        {
            liste = new ObservableCollection<EmployeProjet>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq23;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonEmployeProjet GetInstance()
        {
            if (instance == null)
                instance = new SingletonEmployeProjet();

            return instance;
        }

        public List<EmployeProjet> RetournelesEmployeLierAuProjet(String NumeroProjet)
        {
            List<EmployeProjet> listeEmployes = new List<EmployeProjet>();

            try
            {
                MySqlCommand commande = new MySqlCommand("GetEmployesProjetDetails");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("@p_NumeroProjet", NumeroProjet);

                con.Open();
                commande.Prepare();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    EmployeProjet employe = new EmployeProjet
                    {
                        Matricule = r["Matricule"] as String,
                        Nom = r["Nom"] as String,
                        Prenom = r["Prenom"] as String,
                        TauxHoraire = Convert.ToDouble(r["TauxHoraire"], CultureInfo.InvariantCulture),
                        PhotoIdentite = r["PhotoIdentite"] as String,
                        Statut = r["Statut"] as String,
                        NbreHeures = Convert.ToInt32(r["NbreHeures"]),
                        Salaire = Convert.ToDouble(r["Salaire"], CultureInfo.InvariantCulture),
                    };

                    listeEmployes.Add(employe);
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

            return listeEmployes;
        }

    }
}

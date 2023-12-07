using MySql.Data.MySqlClient;
using Projet_Final.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Singleton
{
    internal class SingletonProjetClient
    {
        ObservableCollection<ProjetClient> liste;
        MySqlConnection con;
        static SingletonProjetClient instance = null;

        // Constructeur de la classe
        public SingletonProjetClient()
        {
            liste = new ObservableCollection<ProjetClient>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq23;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonProjetClient GetInstance()
        {
            if (instance == null)
                instance = new SingletonProjetClient();

            return instance;
        }

        public ObservableCollection<ProjetClient> ListeProjetsAvecClients()
        {

            try
            {
                MySqlCommand commande = new MySqlCommand("GetProjectsAndClientsWithDetails");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    ProjetClient projetClient = new ProjetClient
                    {
                        NumeroProjet = r["NumeroProjet"] as String,
                        Titre = r["TitreProjet"] as String,
                        DateDebut = Convert.ToDateTime(r["DateDebut"]),
                        Description = r["Description"] as String,
                        Budget = Convert.ToDouble(r["Budget"]),
                        EmployesRequis = Convert.ToInt32(r["EmployesRequis"]),
                        TotalSalaires = Convert.ToDouble(r["TotalSalaires"]),
                        Statut = r["Statut"] as String,
                        IdentifiantClient = Convert.ToInt32(r["Identifiant"]),
                        NomClient = r["Nom"] as String,
                        AdresseClient = r["Adresse"] as String,
                        NumeroTelephoneClient = r["NumeroTelephone"] as String,
                        EmailClient = r["Email"] as String
                    };

                    liste.Add(projetClient);
                }

                r.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return liste;
        }

    }
}

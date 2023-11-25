using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    class SingletonClient
    {
        public ObservableCollection<Client> liste;
        static SingletonClient instance = null;

        MySqlConnection con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq23;Uid=2130649;Pwd=2130649;");

        public SingletonClient()
        {
            liste = new ObservableCollection<Client>();
        }

        public static SingletonClient getInstance()
        {
            if (instance == null)
                instance = new SingletonClient();

            return instance;
        }

        //Retourne la liste (pas super important)
        public ObservableCollection<Client> GetListeClients()
        {
            return liste;
        }

        /*------------------Ajoute les Clients-----------------------------*/
        public void ajouter_Client(string nom, string adresse, string num, string email)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                /*------------------------Procedure stockes-----------------------------------*/
                commande.CommandText = "CALL AjouterClient(0,@nom, @adresse, @num, @email)";

                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@adresse", adresse);
                commande.Parameters.AddWithValue("@num", num);
                commande.Parameters.AddWithValue("@email", email);

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                //message d'erreur
            }

        }


        /*--------------------------Retourne la liste des Clients------------------------*/
        public ObservableCollection<Client> afficher_Client()
        {

            try
            {
                ObservableCollection<Client> liste2 = new ObservableCollection<Client>();
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "CALL GetClientList()";
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                while (r.Read())
                {

                    liste2.Add(new Client { Id = int.Parse(r["Identifiant"].ToString()), Nom = r["Nom"].ToString(), Adresse = r["Adresse"].ToString(), Num = r["NumeroTelephone"].ToString(), Email = r["Email"].ToString() });
                }

                r.Close();
                con.Close();
                liste = liste2;
                
            }
            catch (MySqlException ex)
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

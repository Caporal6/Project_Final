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
        ObservableCollection<Client> liste;
        static SingletonClient instance = null;

        MySqlConnection con = new MySqlConnection("Server=adresseServeur;Database=nomdeBD;Uid=nomUtilisateur;Pwd=motdepassse;");

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

        public void ajouter_Client(string nom,string adresse, string num, string email)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "insert into clients values(@nom, @adresse, @num, @email) ";

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



        public void afficher_Client()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "Select * from clients";
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                while (r.Read())
                {
                    //lvListe.Items.Add(r["id"] + " " + r["nom"] + " " + r["prenom"]);
                }

                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }

        }













    }
}

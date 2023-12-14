﻿using MySql.Data.MySqlClient;
using Projet_Final.Classes;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Projet_Final.Singleton
{
    internal class SingletonAdministrateur
    {
        MySqlConnection con;
        static SingletonAdministrateur instance = null;
        Boolean Statut = false;

        // Constructeur de la classe
        private SingletonAdministrateur()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq23;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonAdministrateur GetInstance()
        {
            if (instance == null)
                instance = new SingletonAdministrateur();

            return instance;
        }

        // Vérifie si un administrateur existe en fonction du nom d'utilisateur et du mot de passe
        public bool VerifierAdministrateur(string nomUtilisateur, string motDePasse)
        {
            try
            {
                // Hacher le mot de passe avec SHA-256
                //string motDePasseHache = HashMotDePasse(motDePasse);

                MySqlCommand commande = new MySqlCommand("VerifierAdministrateur");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;


                commande.Parameters.AddWithValue("@p_NomUtilisateur", nomUtilisateur);
                commande.Parameters.AddWithValue("@p_MotDePasse", motDePasse);

                con.Open();
                
                MySqlDataReader r = commande.ExecuteReader();

                bool test = r.Read();

                if (test)
                {
                    Statut = true;
                }
                else
                {
                    Statut = false;
                }

                con.Close();


                // Si le résultat est supérieur à zéro, il y a une correspondance
                return test;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                // En cas d'erreur, renvoyer false
                return false;
            }
        }


        //private string HashMotDePasse(string motDePasse)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(motDePasse));
        //        StringBuilder builder = new StringBuilder();

        //        foreach (byte b in hashBytes)
        //        {
        //            builder.Append(b.ToString("x2"));
        //        }

        //        return builder.ToString();
        //    }
        //}


        public bool online()
        {
            return Statut;
        }
    }
}

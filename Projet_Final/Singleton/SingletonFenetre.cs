using Microsoft.UI.Xaml;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailDeux
{
    internal class SingletonFenetre
    {
        static SingletonFenetre instance = null;
        Window fenetre;

        // Constructeur de la classe
        public SingletonFenetre()
        {
            
            
        }

        public Window Fenetre { get => fenetre; set => fenetre = value; }

        // Retourne l'instance du singleton
        public static SingletonFenetre GetInstance()
        {
            if (instance == null)
                instance = new SingletonFenetre();

            return instance;
        }

       
    }
}

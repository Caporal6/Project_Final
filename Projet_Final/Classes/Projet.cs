using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    class Projet
    {
        private string id_Projet;
        private string titre;
        private string date_Debut;
        private string description;
        private double budget;
        private int employes;
        private double total_salaires;
        private int id_client;
        private string statut;

        protected string Id_Projet { get => id_Projet; set => id_Projet = value; }
        protected string Titre { get => titre; set => titre = value; }
        protected string Date_Debut { get => date_Debut; set => date_Debut = value; }
        protected string Description { get => description; set => description = value; }
        protected double Budget { get => budget; set => budget = value; }
        protected int Employes { get => employes; set => employes = value; }
        protected double Total_salaires { get => total_salaires; set => total_salaires = value; }
        protected int Id_client { get => id_client; set => id_client = value; }
        protected string Statut { get => statut; set => statut = value; }

        public Projet(string id_Projet, string titre, string date_Debut, string description, double budget, int employes, double total_salaires, int id_client, string statut)
        {
            this.id_Projet = id_Projet;
            this.titre = titre;
            this.date_Debut = date_Debut;
            this.description = description;
            this.budget = budget;
            this.employes = employes;
            this.total_salaires = total_salaires;
            this.id_client = id_client;
            this.statut = statut;
        }

        public override bool Equals(object obj)
        {
            return obj is Projet projet &&
                   id_Projet == projet.id_Projet &&
                   titre == projet.titre &&
                   date_Debut == projet.date_Debut &&
                   description == projet.description &&
                   budget == projet.budget &&
                   employes == projet.employes &&
                   total_salaires == projet.total_salaires &&
                   id_client == projet.id_client &&
                   statut == projet.statut;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(id_Projet);
            hash.Add(titre);
            hash.Add(date_Debut);
            hash.Add(description);
            hash.Add(budget);
            hash.Add(employes);
            hash.Add(total_salaires);
            hash.Add(id_client);
            hash.Add(statut);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return $"ID: {Id_Projet} Titre: {Titre}";
        }
    }
}

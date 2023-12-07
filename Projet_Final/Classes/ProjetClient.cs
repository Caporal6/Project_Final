using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class ProjetClient
    {
        public string NumeroProjet { get; set; }
        public string Titre { get; set; }
        public DateTime DateDebut { get; set; }
        public string Description { get; set; }
        public double Budget { get; set; }
        public int EmployesRequis { get; set; }
        public double TotalSalaires { get; set; }
        public int ClientIdentifiant { get; set; }
        public string Statut { get; set; }

        // Informations du client
        public int IdentifiantClient { get; set; }
        public string NomClient { get; set; }
        public string AdresseClient { get; set; }
        public string NumeroTelephoneClient { get; set; }
        public string EmailClient { get; set; }

        public string DateDebutString
        {
            get { return DateDebut.ToString("dd MMMM yyyy"); }
        }

        public override string ToString()
        {
            return $"NumeroProjet: {NumeroProjet} - Titre: {Titre} - DateDebut: {DateDebut} - Description: {Description} - Budget: {Budget} - EmployesRequis: {EmployesRequis} - TotalSalaires: {TotalSalaires} - Statut: {Statut}\nClient - Identifiant: {IdentifiantClient} - Nom: {NomClient} - Adresse: {AdresseClient} - NumeroTelephone: {NumeroTelephoneClient} - Email: {EmailClient}";
        }

        public string ToStringCSV()
        {
            return $"{NumeroProjet}; {Titre}; {DateDebut}; {Description}; {Budget}; {EmployesRequis}; {TotalSalaires}; {ClientIdentifiant}; {Statut}; {IdentifiantClient}; {NomClient}; {AdresseClient}; {NumeroTelephoneClient}; {EmailClient}";
        }
    }
}

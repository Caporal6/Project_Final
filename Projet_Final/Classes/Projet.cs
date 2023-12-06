using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    class Projet
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

        public override string ToString()
        {
            return $"NumeroProjet: {NumeroProjet} - Titre: {Titre} - DateDebut: {DateDebut} - Description: {Description} - Budget: {Budget} - EmployesRequis: {EmployesRequis} - TotalSalaires: {TotalSalaires} - ClientIdentifiant: {ClientIdentifiant} - Statut: {Statut}";
        }

        public string ToStringCSV()
        {
            return $"{NumeroProjet}; {Titre}; {DateDebut}; {Description}; {Budget}; {EmployesRequis}; {TotalSalaires}; {ClientIdentifiant}; {Statut}";
        }
    }
}

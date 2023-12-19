using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class EmployeProjet
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public double TauxHoraire { get; set; }
        public string PhotoIdentite { get; set; }
        public string Statut { get; set; }
        public int NbreHeures { get; set; }
        public double Salaire { get; set; }

        public string TauxHorraireString
        {
            get { return TauxHoraire.ToString()+"$"; }
        }

        public string SalaireString
        {
            get { return Salaire.ToString() + "$"; }
        }

        public string ToStringCSV()
        {
            return $"{Matricule}; {Nom}; {Prenom}; {TauxHoraire}; {PhotoIdentite}; {Statut}; {NbreHeures}; {Salaire}";
        }

        public override string ToString()
        {
            return $"Matricule: {Matricule} - Nom: {Nom} - Prenom: {Prenom} - TauxHoraire: {TauxHoraire} - PhotoIdentite: {PhotoIdentite} - Statut: {Statut} - NbreHeures: {NbreHeures} - Salaire: {Salaire}";
        }
    }
}

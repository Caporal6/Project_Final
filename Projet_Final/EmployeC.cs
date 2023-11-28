using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    internal class EmployeC
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string DateNaissance { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string DateEmbauche { get; set; }
        public Double TauxHoraire { get; set; }
        public string PhotoIdentite { get; set; }
        public string Statut { get; set; }
        public string ProjetId { get; set; }

        public override string ToString()
        {
            return $"Matricule: {Matricule} - Nom: {Nom} - Prenom: {Prenom} - DateNaissance: {DateNaissance} - Email: {Email} - Adresse: {Adresse} - DateEmbauche: {DateEmbauche} - TauxHoraire: {TauxHoraire} - PhotoIdentite: {PhotoIdentite} - Statut: {Statut} - ProjetId: {ProjetId}";
        }

        public string ToStringCSV()
        {
            return $"{Matricule}; {Nom}; {Prenom}; {DateNaissance}; {Email}; {Adresse}; {DateEmbauche}; {TauxHoraire}; {PhotoIdentite}; {Statut}; {ProjetId}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class Administrateur
    {
        public int Id { get; set; }
        public string NomUtilisateur { get; set; }
        public string MotDePasseHash { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - NomUtilisateur: {NomUtilisateur} - MotDePasseHash: {MotDePasseHash}";
        }


    }
}

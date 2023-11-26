using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    class Client
    {
         int id;
         string nom;
         string adresse;
         string num;
         string email;

        public Client() 
        { 
            id = 0;
            nom = "";
            adresse = "";
            num = "";
            email = string.Empty;
        
        }

        public Client(int id, string nom, string adresse, string num, string email)
        {
            this.id = id;
            this.nom = nom;
            this.adresse = adresse;
            this.num = num;
            this.email = email;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public string Num { get => num; set => num = value; }
        public string Email { get => email; set => email = value; }

        public override bool Equals(object obj)
        {
            return obj is Client client &&
                   id == client.id &&
                   nom == client.nom &&
                   adresse == client.adresse &&
                   num == client.num &&
                   email == client.email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nom, adresse, num, email);
        }

        public override string ToString()
        {
            return $"Id: {Id} \nNom: {Nom} \nAdresse: {Adresse} \nNumero de telephone: {Num} \nEmail {Email}";
        }
    }
}

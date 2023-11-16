drop table sceance;
drop table traitement;
drop table client;

/*Creation de la table Employe*/

CREATE TABLE Employe (
    Matricule VARCHAR(20) PRIMARY KEY,
    Nom VARCHAR(50),
    Prenom VARCHAR(50),
    DateNaissance DATE,
    Email VARCHAR(100),
    Adresse VARCHAR(200),
    DateEmbauche DATE,
    TauxHoraire DECIMAL(5, 2) CHECK (TauxHoraire >= 15),
    PhotoIdentite VARCHAR(255),
    Statut VARCHAR(20) CHECK (Statut IN ('Journalier', 'Permanent'))
);

/*Creation de la table Client*/

CREATE TABLE Client (
                         Identifiant INT PRIMARY KEY,
                         Nom VARCHAR(50),
                         Adresse VARCHAR(200),
                         NumeroTelephone VARCHAR(15),
                         Email VARCHAR(100)
);

/*Creation de la table Projet*/

CREATE TABLE Projet (
                         NumeroProjet VARCHAR(20) PRIMARY KEY,
                         Titre VARCHAR(100),
                         DateDebut DATE,
                         Description TEXT,
                         Budget DECIMAL(10, 2),
                         EmployesRequis INT CHECK (EmployesRequis <= 5),
                         TotalSalaires DECIMAL(10, 2),
                         ClientIdentifiant INT,
                         Statut VARCHAR(20) CHECK (Statut IN ('Terminé',  'En cours')) default 'En cours',
                         FOREIGN KEY (ClientIdentifiant) REFERENCES Client (Identifiant)
);

CREATE TABLE Assignations (
                              AssignationId SERIAL PRIMARY KEY,
                              EmployeId INT,
                              ProjetId VARCHAR(20),
                              FOREIGN KEY (EmployeId) REFERENCES Employe (Matricule),
                              FOREIGN KEY (ProjetId) REFERENCES Projet (NumeroProjet)
);











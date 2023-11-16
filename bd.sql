/*Creation de la table Employe*/

CREATE TABLE Employe (
    Matricule VARCHAR(20),
    Nom VARCHAR(50),
    Prenom VARCHAR(50),
    DateNaissance DATE,
    Email VARCHAR(100),
    Adresse VARCHAR(200),
    DateEmbauche DATE,
    TauxHoraire DECIMAL(5, 2) ,
    PhotoIdentite VARCHAR(255),
    Statut VARCHAR(20),
    ProjetId VARCHAR(20),
    CONSTRAINT Pk_Employe PRIMARY KEY (Matricule),
    CONSTRAINT Ck_Statut CHECK (Statut IN ('Journalier', 'Permanent')),
    CONSTRAINT Ck_Tauxhorraire CHECK (TauxHoraire >= 15)
);

/*Trigger sur la table Employe*/

DELIMITER //

CREATE TRIGGER before_insert_employe_set_matricule
BEFORE INSERT ON Employe
FOR EACH ROW
BEGIN
    SET NEW.Matricule = CONCAT(SUBSTRING(NEW.Nom, 1, 2), '-', YEAR(NEW.DateNaissance), '-', FLOOR(RAND() * (99 - 10 + 1) + 10));
END;

//

DELIMITER ;

DELIMITER //

CREATE TRIGGER before_insert_employe_tauxhoraire_verification
BEFORE INSERT ON Employe
FOR EACH ROW
BEGIN
    IF NEW.TauxHoraire < 15 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Le taux horaire doit être supérieur ou égal à 15 $';
    END IF;
END;

//

DELIMITER ;


/*Creation de la table Client*/

CREATE TABLE Client (
    Identifiant INT,
    Nom VARCHAR(50),
    Adresse VARCHAR(200),
    NumeroTelephone VARCHAR(15),
    Email VARCHAR(100),
    CONSTRAINT Pk_Client PRIMARY KEY (Identifiant)
);

/*Trigger sur la table Client */

DELIMITER //

CREATE TRIGGER before_insert_client_set_identifiant
BEFORE INSERT ON Client
FOR EACH ROW
BEGIN
    DECLARE random_id INT;

    -- Générer une valeur aléatoire entre 100 et 999
    SET random_id = FLOOR(RAND() * (999 - 100 + 1) + 100);

    -- Vérifier si la valeur aléatoire est déjà utilisée
    WHILE EXISTS (SELECT 1 FROM Client WHERE Identifiant = random_id) DO
        SET random_id = FLOOR(RAND() * (999 - 100 + 1) + 100);
    END WHILE;

    -- Affecter la valeur aléatoire à l'identifiant
    SET NEW.Identifiant = random_id;
END;

//

DELIMITER ;

/*Creation de la table Projet*/

CREATE TABLE Projet (
    NumeroProjet VARCHAR(20) ,
    Titre VARCHAR(100),
    DateDebut DATE,
    Description TEXT,
    Budget DECIMAL(10, 2),
    EmployesRequis INT,
    TotalSalaires DECIMAL(10, 2),
    ClientIdentifiant INT,
    Statut VARCHAR(20)  DEFAULT  'En cours',
    CONSTRAINT CK_NombreEmploye CHECK (EmployesRequis <= 5),
    CONSTRAINT CK_StatutProjet CHECK (Statut IN ('Terminé',  'En cours')),
    CONSTRAINT Pk_Projet PRIMARY KEY (NumeroProjet),
    CONSTRAINT Fk_Projet_Client FOREIGN KEY (ClientIdentifiant) REFERENCES Client (Identifiant)
);

/*Trigger sur la table Projet */

DELIMITER //

CREATE TRIGGER before_insert_projet_set_numeroprojet
BEFORE INSERT ON Projet
FOR EACH ROW
BEGIN

    -- Affecter le nouveau numéro de projet à la nouvelle entrée
    SET NEW.NumeroProjet = CONCAT(New.ClientIdentifiant, '-', (FLOOR(RAND() * 99) + 1), '-', YEAR(New.DateDebut));
END;

//

DELIMITER ;


/*Creation de la table Assignation qui regroupe chaque assignations d'un employe a un projet*/

CREATE TABLE Assignation (
    AssignationId INT(32) AUTO_INCREMENT,
    EmployeId VARCHAR(20),
    ProjetId VARCHAR(20),
    CONSTRAINT Pk_Assignation PRIMARY KEY (AssignationId),
    CONSTRAINT Fk_Assignation_Employe FOREIGN KEY (EmployeId) REFERENCES Employe (Matricule),
    CONSTRAINT Fk_Assignation_Projet FOREIGN KEY (ProjetId) REFERENCES Projet (NumeroProjet)
);

DELIMITER //

CREATE TRIGGER after_insert_assignation_attribute_to_employe
AFTER INSERT ON Assignation
FOR EACH ROW
BEGIN

    -- Mettre à jour le ProjetId de l'employé
    UPDATE Employe SET ProjetId = NEW.ProjetId WHERE Matricule = NEW.EmployeId;
END;

//

DELIMITER ;



/*Manage DB*/

/*Commandes SELECT*/

SELECT * FROM Employe;
SELECT * FROM Client;
SELECT * FROM Projet;
SELECT * FROM Assignation;

/*Commandes DROP*/

DROP TABLE IF EXISTS Employe;
DROP TABLE IF EXISTS Client;
DROP TABLE IF EXISTS Projet;
DROP TABLE IF EXISTS Assignation;

/*Commandes DELETE*/

DELETE FROM Employe;
DELETE FROM Client;
DELETE FROM Projet;
DELETE FROM Assignation;

/*Commandes DROP TRIGGER*/

DROP TRIGGER IF EXISTS before_insert_employe_set_matricule;
DROP TRIGGER IF EXISTS before_insert_employe_tauxhoraire_verification;
DROP TRIGGER IF EXISTS before_insert_client_set_identifiant;
DROP TRIGGER IF EXISTS before_insert_projet_set_numeroprojet;
DROP TRIGGER IF EXISTS after_insert_assignation_attribute_to_employe;

/*Commandes INSERT*/

/*Table Employe*/

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, Statut, ProjetId)
VALUES ('', 'Doe', 'John', '1978-01-01', 'john.doe@example.com', '123 Main St', '2022-01-01', 20.0, 'Permanent', '123-01-2023'),
       ('', 'Smith', 'Jane', '1985-05-15', 'jane.smith@example.com', '456 Oak St', '2022-02-01', 18.5, 'Journalier', '456-02-2023'),
       ('', 'Johnson', 'Robert', '1990-09-10', 'robert.johnson@example.com', '789 Pine St', '2022-03-01', 22.0, 'Permanent', '789-03-2023');

/*Table Client*/

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (null, 'ABC Corporation', '10 Corporate Lane', '555-1234', 'abc@example.com'),
       (null, 'XYZ Corporation', '20 Business Street', '555-5678', 'xyz@example.com'),
       (null, 'LMN Company', '30 Enterprise Blvd', '555-9101', 'lmn@example.com');

/*Table Projet*/

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES (null, 'Project A', '2023-01-15', 'Description of Project A', 50000.00, 3, 0.00, 296, null),
       (null, 'Project B', '2023-02-01', 'Description of Project B', 75000.00, 5, 0.00, 571, null),
       (null, 'Project C', '2023-03-10', 'Description of Project C', 100000.00, 4, 0.00, 852, null);

DESCRIBE Projet;

/*Table Assignation*/

INSERT INTO Assignation (EmployeId, ProjetId)
VALUES ('DO-1978-25', '101-01-2023'),
       ('SM-1985-42', '202-02-2023'),
       ('JO-1990-17', '303-03-2023');

/*TODO Faire en sorte qu'un projet a sa creation ai le statut en cours*/





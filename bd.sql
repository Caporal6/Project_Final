/**********************Creation de la table Employe******************/

DROP TABLE IF EXISTS Employe;
CREATE TABLE Employe
(
    Matricule     VARCHAR(20),
    Nom           VARCHAR(50),
    Prenom        VARCHAR(50),
    DateNaissance DATE,
    Email         VARCHAR(100),
    Adresse       VARCHAR(200),
    DateEmbauche  DATE,
    TauxHoraire   DECIMAL(5, 2),
    PhotoIdentite VARCHAR(255),
    Statut        VARCHAR(20),
    ProjetId      VARCHAR(20),
    CONSTRAINT Pk_Employe PRIMARY KEY (Matricule),
    CONSTRAINT Ck_Statut CHECK (Statut IN ('Journalier', 'Permanent')),
    CONSTRAINT Ck_Tauxhorraire CHECK (TauxHoraire >= 15)
);

/*******************Trigger sur la table Employe********************/

/*------------------Trigger qui change le matricule----------------*/

DELIMITER //

CREATE TRIGGER before_insert_employe_set_matricule
    BEFORE INSERT
    ON Employe
    FOR EACH ROW
BEGIN
    SET NEW.Matricule =
            CONCAT(SUBSTRING(NEW.Nom, 1, 2), '-', YEAR(NEW.DateNaissance), '-', FLOOR(RAND() * (99 - 10 + 1) + 10));
END;

//

DELIMITER ;

/*-----------------Trigger qui verifie le taux horraire------------------*/

DELIMITER //

CREATE TRIGGER before_insert_employe_tauxhoraire_verification
    BEFORE INSERT
    ON Employe
    FOR EACH ROW
BEGIN
    IF NEW.TauxHoraire < 15 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Le taux horaire doit être supérieur ou égal à 15 $';
    END IF;
END;

//

DELIMITER ;

/*----------------Trigger changement de statut-------------------------*/

DELIMITER //

CREATE TRIGGER before_update_statut_check_anciennete
    BEFORE UPDATE
    ON Employe
    FOR EACH ROW
BEGIN
    DECLARE anciennete INT;
    IF NEW.Statut = 'Permanent' AND OLD.Statut = 'Journalier' THEN
        -- Calculer la différence entre la date actuelle et la date d'embauche en années

        SET anciennete = DATEDIFF(OLD.DateEmbauche, CURRENT_DATE());

        -- Vérifier si l'ancienneté est inférieure à 3 ans jour pour jour
        IF anciennete < 3 THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Un employé doit avoir au moins 3 ans d''ancienneté pour devenir Permanent';
        ELSEIF NEW.Statut = 'Journalier' AND OLD.Statut = 'Permanent' THEN
            -- Empêcher le passage de Permanent à Journalier
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Impossible de passer de Permanent à Journalier';
        END IF;
    END IF;
END;

//

DELIMITER ;

/*************Procedure stocker sur la table employe**************/

/*------------Retourne la liste d'employees----------------------*/

CREATE PROCEDURE GetEmployeeList()
BEGIN
    SELECT * FROM Employe;
END;

/*------------Creer un nouvel employe----------------------*/
DROP PROCEDURE IF EXISTS AjouterEmploye;
CREATE PROCEDURE AjouterEmploye(
    IN p_Nom VARCHAR(50),
    IN p_Prenom VARCHAR(50),
    IN p_DateNaissance DATE,
    IN p_Email VARCHAR(100),
    IN p_Adresse VARCHAR(200),
    IN p_DateEmbauche DATE,
    IN p_TauxHoraire DECIMAL(5, 2),
    IN p_PhotoIdentite VARCHAR(255),
    IN p_Statut VARCHAR(20)
)
BEGIN
    INSERT INTO Employe (

        Nom,
        Prenom,
        DateNaissance,
        Email,
        Adresse,
        DateEmbauche,
        TauxHoraire,
        PhotoIdentite,
        Statut,
        ProjetId
    ) VALUES (

                 p_Nom,
                 p_Prenom,
                 p_DateNaissance,
                 p_Email,
                 p_Adresse,
                 p_DateEmbauche,
                 p_TauxHoraire,
                 p_PhotoIdentite,
                 p_Statut,
                 NULL
             );
END;

/*------------Modifier les informations d'un employe----------------------*/


CREATE PROCEDURE ModifierInformationsEmploye(
    IN p_Matricule VARCHAR(20),
    IN p_Nom VARCHAR(50),
    IN p_Prenom VARCHAR(50),
    IN p_Email VARCHAR(100),
    IN p_Adresse VARCHAR(200),
    IN p_TauxHoraire DECIMAL(5, 2),
    IN p_PhotoIdentite VARCHAR(255),
    IN p_Statut VARCHAR(20)
)
BEGIN
    UPDATE Employe
    SET
        Nom = p_Nom,
        Prenom = p_Prenom,
        Email = p_Email,
        Adresse = p_Adresse,
        TauxHoraire = p_TauxHoraire,
        PhotoIdentite = p_PhotoIdentite,
        Statut = p_Statut
    WHERE Matricule = p_Matricule;
END;

/*------------Recherche un employe par son matricule----------------------*/


DELIMITER //
DROP PROCEDURE IF EXISTS ObtenirEmployeParMatricule;
//
CREATE PROCEDURE ObtenirEmployeParMatricule(IN MatriculeParam VARCHAR(20))
BEGIN
    SELECT *
    FROM Employe
    WHERE Matricule = MatriculeParam;
END //
DELIMITER ;





/***************Creation de la table Client*******************/
DROP TABLE IF EXISTS Client;
CREATE TABLE Client
(
    Identifiant     INT,
    Nom             VARCHAR(50),
    Adresse         VARCHAR(200),
    NumeroTelephone VARCHAR(15),
    Email           VARCHAR(100),
    CONSTRAINT Pk_Client PRIMARY KEY (Identifiant)
);

/*Trigger sur la table Client */

DELIMITER //

CREATE TRIGGER before_insert_client_set_identifiant
    BEFORE INSERT
    ON Client
    FOR EACH ROW
BEGIN
    DECLARE random_id INT;

    -- Générer une valeur aléatoire entre 100 et 999
    SET random_id = FLOOR(RAND() * (999 - 100 + 1) + 100);

    -- Vérifier si la valeur aléatoire est déjà utilisée
    WHILE EXISTS (SELECT 1 FROM Client WHERE Identifiant = random_id)
        DO
            SET random_id = FLOOR(RAND() * (999 - 100 + 1) + 100);
        END WHILE;

    -- Affecter la valeur aléatoire à l'identifiant
    SET NEW.Identifiant = random_id;
END;

//

DELIMITER ;


/*************Procedure stocker sur la table Client**************/

/*------------Retourne la liste de Client----------------------*/

CREATE PROCEDURE GetClientList()
BEGIN
    SELECT * FROM Client;
END;

/*------------Creer un nouveau Client----------------------*/

CREATE PROCEDURE AjouterClient(
    IN c_Identifiant     INT,
    IN c_Nom             VARCHAR(50),
    IN c_Adresse         VARCHAR(200),
    IN c_NumeroTelephone VARCHAR(15),
    IN c_Email           VARCHAR(100)
)
BEGIN
    INSERT INTO Client (
        Identifiant,
        Nom,
        Adresse,
        NumeroTelephone,
        Email
    ) VALUES (
                 c_Identifiant,
                 c_Nom,
                 c_Adresse,
                 c_NumeroTelephone,
                 c_Email
             );
END;



/*------------Modifier les informations d'un Client----------------------*/


CREATE PROCEDURE ModifierInformationsClient(
    IN c_Identifiant     INT,
    IN c_Nom             VARCHAR(50),
    IN c_Adresse         VARCHAR(200),
    IN c_NumeroTelephone VARCHAR(15),
    IN c_Email           VARCHAR(100)
)
BEGIN
    UPDATE Client
    SET
        Identifiant = c_Identifiant,
        Nom = c_Nom,
        Adresse = c_Adresse,
        NumeroTelephone = c_NumeroTelephone,
        Email = c_Email
    WHERE Identifiant = c_Identifiant;
END;



/*Creation de la table Projet*/
DROP TABLE IF EXISTS Projet;
CREATE TABLE Projet
(
    NumeroProjet      VARCHAR(20),
    Titre             VARCHAR(100),
    DateDebut         DATE,
    Description       TEXT,
    Budget            DECIMAL(10, 2),
    EmployesRequis    INT,
    TotalSalaires     DECIMAL(10, 2),
    ClientIdentifiant INT,
    test              INT(32)     default 20,
    Statut            VARCHAR(20) DEFAULT 'En cours',
    CONSTRAINT CK_NombreEmploye CHECK (EmployesRequis <= 5),
    CONSTRAINT Pk_Projet PRIMARY KEY (NumeroProjet),
    CONSTRAINT Fk_Projet_Client FOREIGN KEY (ClientIdentifiant) REFERENCES Client (Identifiant)
);

/*Trigger sur la table Projet */

DELIMITER //

CREATE TRIGGER before_insert_projet_set_numeroprojet
    BEFORE INSERT
    ON Projet
    FOR EACH ROW
BEGIN

    -- Affecter le nouveau numéro de projet à la nouvelle entrée
    SET NEW.NumeroProjet = CONCAT(New.ClientIdentifiant, '-', (FLOOR(RAND() * 99) + 1), '-', YEAR(New.DateDebut));
END;

//

DELIMITER ;


/*Creation de la table Assignation qui regroupe chaque assignations d'un employe a un projet*/

CREATE TABLE Assignation
(
    AssignationId INT(32) AUTO_INCREMENT,
    EmployeId     VARCHAR(20),
    ProjetId      VARCHAR(20),
    NbreHeures    INT(32),
    CONSTRAINT Pk_Assignation PRIMARY KEY (AssignationId),
    CONSTRAINT Fk_Assignation_Employe FOREIGN KEY (EmployeId) REFERENCES Employe (Matricule),
    CONSTRAINT Fk_Assignation_Projet FOREIGN KEY (ProjetId) REFERENCES Projet (NumeroProjet)
);

DELIMITER //

CREATE TRIGGER after_insert_assignation_attribute_to_employe
    AFTER INSERT
    ON Assignation
    FOR EACH ROW
BEGIN

    -- Mettre à jour le ProjetId de l'employé
    UPDATE Employe SET ProjetId = NEW.ProjetId WHERE Matricule = NEW.EmployeId;
END;

//

DELIMITER ;


/*Manage DB*/

/*Commandes SELECT*/

SELECT *
FROM Employe;
SELECT *
FROM Client;
SELECT *
FROM Projet;
SELECT *
FROM Assignation;

/*Commandes DROP*/

DROP TABLE IF EXISTS Employe;
DROP TABLE IF EXISTS Client;
DROP TABLE IF EXISTS Assignation;
DROP TABLE IF EXISTS Projet;


/*Commandes DELETE*/

DELETE
FROM Employe;
DELETE
FROM Client;
DELETE
FROM Projet;
DELETE
FROM Assignation;

/*Commandes DROP TRIGGER*/

DROP TRIGGER IF EXISTS before_insert_employe_set_matricule;
DROP TRIGGER IF EXISTS before_insert_employe_tauxhoraire_verification;
DROP TRIGGER IF EXISTS before_insert_client_set_identifiant;
DROP TRIGGER IF EXISTS before_insert_projet_set_numeroprojet;
DROP TRIGGER IF EXISTS after_insert_assignation_attribute_to_employe;

/*Commandes INSERT*/

drop table if exists lol;
create table lol
(
    lol int(32) default 398
);

select *
from lol;

INSERT INTO lol
VALUES ();

insert into lol
values (3),
       ();

/*Table Employe*/

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, Statut, ProjetId)
VALUES ('', 'Doe', 'John', '1978-01-01', 'john.doe@example.com', '123 Main St', '2022-01-01', 20.0, 'Permanent',
        '123-01-2023'),
       ('', 'Smith', 'Jane', '1985-05-15', 'jane.smith@example.com', '456 Oak St', '2022-02-01', 18.5, 'Journalier',
        '456-02-2023'),
       ('', 'Johnson', 'Robert', '1990-09-10', 'robert.johnson@example.com', '789 Pine St', '2022-03-01', 22.0,
        'Permanent', '789-03-2023');

/*Table Client*/

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (null, 'ABC Corporation', '10 Corporate Lane', '555-1234', 'abc@example.com'),
       (null, 'XYZ Corporation', '20 Business Street', '555-5678', 'xyz@example.com'),
       (null, 'LMN Company', '30 Enterprise Blvd', '555-9101', 'lmn@example.com');

/*Table Projet*/

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires,
                    ClientIdentifiant)
VALUES (null, 'Project A', '2023-01-15', 'Description of Project A', 50000.00, 3, 0.00, 256),
       (null, 'Project B', '2023-02-01', 'Description of Project B', 75000.00, 5, 0.00, 786),
       (null, 'Project C', '2023-03-10', 'Description of Project C', 100000.00, 4, 0.00, 903);

DESCRIBE Projet;

/*Table Assignation*/

INSERT INTO Assignation (EmployeId, ProjetId)
VALUES ('DO-1978-25', '101-01-2023'),
       ('SM-1985-42', '202-02-2023'),
       ('JO-1990-17', '303-03-2023');

/*TODO Faire en sorte qu'un projet a sa creation ai le statut en cours*/

update employe set Statut = 'Permanent' where Nom =  'Doe';

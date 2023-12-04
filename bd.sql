-- DROP DES ELEMENTS EXISTANTS

-- DROP DES PROCEDURES
DROP PROCEDURE IF EXISTS AjouterEmploye;
DROP PROCEDURE IF EXISTS ModifierInformationsEmploye;
DROP PROCEDURE IF EXISTS ObtenirEmployeParMatricule;
DROP PROCEDURE IF EXISTS GetEmployeeList;
DROP PROCEDURE IF EXISTS AjouterClient;
DROP PROCEDURE IF EXISTS ModifierInformationsClient;
DROP PROCEDURE IF EXISTS GetClientList;

-- DROP DES FONCTIONS
DROP FUNCTION IF EXISTS CalculerCoutTotalProjet;
DROP FUNCTION IF EXISTS ObtenirBudgetTotalProjetsClient;
DROP FUNCTION IF EXISTS ObtenirNombreAssignationsEmploye;
DROP FUNCTION IF EXISTS CalculerTotalSalaireEmploye;
DROP FUNCTION IF EXISTS ObtenirInformationsClient;

-- DROP DES TRIGGERS
DROP TRIGGER IF EXISTS before_insert_employe_set_matricule;
DROP TRIGGER IF EXISTS before_insert_employe_tauxhoraire_verification;
DROP TRIGGER IF EXISTS before_update_statut_check_anciennete;
DROP TRIGGER IF EXISTS before_insert_client_set_identifiant;
DROP TRIGGER IF EXISTS before_insert_projet_set_numeroprojet;
DROP TRIGGER IF EXISTS after_insert_assignation_attribute_to_employe;

-- DROP DES VUES
DROP VIEW IF EXISTS VueEmployeDetails;
DROP VIEW IF EXISTS VueClientInformations;
DROP VIEW IF EXISTS VueProjetDetails;
DROP VIEW IF EXISTS VueAssignations;
DROP VIEW IF EXISTS VueEmployesProjets;

-- DROP DES TABLES
DROP TABLE IF EXISTS Assignation;
DROP TABLE IF EXISTS Employe;
DROP TABLE IF EXISTS Projet;
DROP TABLE IF EXISTS Client;

-- CREATION DES TABLES

-- Création de la table Employe
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
    CONSTRAINT Ck_Tauxhoraire CHECK (TauxHoraire >= 15)
);

-- Création de la table Client

CREATE TABLE Client
(
    Identifiant INT,
    Nom VARCHAR(50),
    Adresse VARCHAR(200),
    NumeroTelephone VARCHAR(15),
    Email VARCHAR(100),
    CONSTRAINT Pk_Client PRIMARY KEY (Identifiant)
);

-- Création de la table Projet

CREATE TABLE Projet
(
    NumeroProjet VARCHAR(20),
    Titre VARCHAR(100),
    DateDebut DATE,
    Description TEXT,
    Budget DECIMAL(10, 2),
    EmployesRequis INT,
    TotalSalaires DECIMAL(10, 2),
    ClientIdentifiant INT,
    test INT(32) DEFAULT 20,
    Statut VARCHAR(20) DEFAULT 'En cours',
    CONSTRAINT CK_NombreEmploye CHECK (EmployesRequis <= 5),
    CONSTRAINT Pk_Projet PRIMARY KEY (NumeroProjet),
    CONSTRAINT Fk_Projet_Client FOREIGN KEY (ClientIdentifiant) REFERENCES Client (Identifiant)
);

-- Création de la table Assignation
DROP TABLE IF EXISTS Assignation;

CREATE TABLE Assignation
(
    AssignationId INT(32) AUTO_INCREMENT,
    EmployeId VARCHAR(20),
    ProjetId VARCHAR(20),
    NbreHeures INT(32),
    CONSTRAINT Pk_Assignation PRIMARY KEY (AssignationId),
    CONSTRAINT Fk_Assignation_Employe FOREIGN KEY (EmployeId) REFERENCES Employe (Matricule),
    CONSTRAINT Fk_Assignation_Projet FOREIGN KEY (ProjetId) REFERENCES Projet (NumeroProjet)
);

-- CREATION DES TRIGGERS

-- Trigger pour générer le Matricule
DELIMITER //

CREATE TRIGGER before_insert_employe_set_matricule
    BEFORE INSERT
    ON Employe
    FOR EACH ROW
BEGIN
    -- Générer le Matricule basé sur le nom, la date de naissance et une valeur aléatoire
    SET NEW.Matricule = CONCAT(SUBSTRING(NEW.Nom, 1, 2), '-', YEAR(NEW.DateNaissance), '-', FLOOR(RAND() * (99 - 10 + 1) + 10));
END;

//

DELIMITER ;

-- Trigger pour vérifier le Taux Horaire
DELIMITER //

CREATE TRIGGER before_insert_employe_tauxhoraire_verification
    BEFORE INSERT
    ON Employe
    FOR EACH ROW
BEGIN
    -- Vérifier que le Taux Horaire est supérieur ou égal à 15
    IF NEW.TauxHoraire < 15 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Le taux horaire doit être supérieur ou égal à 15 $';
    END IF;
END;

//

DELIMITER ;

-- Trigger pour changer le Statut
DELIMITER //

CREATE TRIGGER before_update_statut_check_anciennete
    BEFORE UPDATE
    ON Employe
    FOR EACH ROW
BEGIN
    DECLARE anciennete INT;

    -- Vérifier les changements de statut et l'ancienneté nécessaire
    IF NEW.Statut = 'Permanent' AND OLD.Statut = 'Journalier' THEN
        SET anciennete = DATEDIFF(OLD.DateEmbauche, CURRENT_DATE());

        -- Vérifier si l'ancienneté est inférieure à 3 ans
        IF anciennete < 3 THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Un employé doit avoir au moins 3 ans d''ancienneté pour devenir Permanent';
        ELSEIF NEW.Statut = 'Journalier' AND OLD.Statut = 'Permanent' THEN
            -- Empêcher la transition de Permanent à Journalier
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Impossible de passer de Permanent à Journalier';
        END IF;
    END IF;
END;

//

DELIMITER ;

-- Trigger sur la table Client
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

    -- Affecter la valeur aléatoire à l'Identifiant
    SET NEW.Identifiant = random_id;
END;

//
DELIMITER ;

-- Trigger sur la table Projet
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

-- Trigger après insertion sur la table Assignation
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


-- CREATION PROCEDURES STOCKEES

-- Procédure pour retourner la liste des employés
CREATE PROCEDURE GetEmployeeList()
BEGIN
    SELECT * FROM Employe;
END;

-- Procédure pour ajouter un nouvel employé
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

-- Procédure pour modifier les informations d'un employé
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

-- Procédure pour obtenir un employé par Matricule
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

-- Création de la table Client
DROP TABLE IF EXISTS Client;

CREATE TABLE Client
(
    Identifiant INT,
    Nom VARCHAR(50),
    Adresse VARCHAR(200),
    NumeroTelephone VARCHAR(15),
    Email VARCHAR(100),
    CONSTRAINT Pk_Client PRIMARY KEY (Identifiant)
);


-- Procédure pour retourner la liste des clients
CREATE PROCEDURE GetClientList()
BEGIN
    SELECT * FROM Client;
END;

-- Procédure pour ajouter un nouveau client
CREATE PROCEDURE AjouterClient(
    IN c_Identifiant INT,
    IN c_Nom VARCHAR(50),
    IN c_Adresse VARCHAR(200),
    IN c_NumeroTelephone VARCHAR(15),
    IN c_Email VARCHAR(100)
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

-- Procédure pour modifier les informations d'un client
CREATE PROCEDURE ModifierInformationsClient(
    IN c_Identifiant INT,
    IN c_Nom VARCHAR(50),
    IN c_Adresse VARCHAR(200),
    IN c_NumeroTelephone VARCHAR(15),
    IN c_Email VARCHAR(100)
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


-- CREATION DES FONCTIONS STOCKEES

-- Fonction pour calculer le coût total d'un projet
DELIMITER //
CREATE FUNCTION CalculerCoutTotalProjet(numeroProjet VARCHAR(20)) RETURNS DECIMAL(10, 2)
BEGIN
    DECLARE coutTotal DECIMAL(10, 2);

    SELECT SUM(E.TauxHoraire * A.NbreHeures)
    INTO coutTotal
    FROM Assignation A
         JOIN Employe E ON A.EmployeId = E.Matricule
    WHERE A.ProjetId = numeroProjet;

    RETURN coutTotal;
END //

DELIMITER //

-- Fonction pour obtenir le budget total des projets d'un client
DELIMITER //
CREATE FUNCTION ObtenirBudgetTotalProjetsClient(clientId INT) RETURNS DECIMAL(10, 2)
BEGIN
    DECLARE budgetTotal DECIMAL(10, 2);

    SELECT SUM(Budget)
    INTO budgetTotal
    FROM Projet
    WHERE ClientIdentifiant = clientId;

    RETURN budgetTotal;
END //

DELIMITER //

-- Fonction pour obtenir le nombre d'assignations d'un employé
DELIMITER //
CREATE FUNCTION ObtenirNombreAssignationsEmploye(matricule VARCHAR(20)) RETURNS INT
BEGIN
    DECLARE nombreAssignations INT;

    SELECT COUNT(*) INTO nombreAssignations
    FROM Assignation
    WHERE EmployeId = matricule;

    RETURN nombreAssignations;
END //

DELIMITER //

-- Fonction pour calculer le salaire total d'un employé
DELIMITER //
CREATE FUNCTION CalculerTotalSalaireEmploye(matricule VARCHAR(20)) RETURNS DECIMAL(10, 2)
BEGIN
    DECLARE totalSalaire DECIMAL(10, 2);

    SELECT SUM(E.TauxHoraire * A.NbreHeures) INTO totalSalaire
    FROM Assignation A
         JOIN Employe E ON A.EmployeId = E.Matricule
    WHERE A.EmployeId = matricule;

    RETURN totalSalaire;
END //

DELIMITER ;

-- Fonction pour obtenir les informations d'un client
DELIMITER //
CREATE FUNCTION ObtenirInformationsClient(clientId INT) RETURNS VARCHAR(500)
BEGIN
    DECLARE clientInfo VARCHAR(500);

    SELECT CONCAT('Nom: ', Nom, ', Adresse: ', Adresse, ', NumeroTelephone: ', NumeroTelephone, ', Email: ', Email)
    INTO clientInfo
    FROM Client
    WHERE Identifiant = clientId;

    RETURN clientInfo;
END //

DELIMITER ;


-- CREATION DES VUES

-- Vue pour afficher les détails des employés
CREATE VIEW VueEmployeDetails AS
SELECT Matricule, Nom, Prenom, Email, Statut
FROM Employe;

-- Vue pour afficher les informations des clients
CREATE VIEW VueClientInformations AS
SELECT Identifiant, Nom, Adresse, NumeroTelephone, Email
FROM Client;

-- Vue pour afficher les détails des projets
CREATE VIEW VueProjetDetails AS
SELECT NumeroProjet, Titre, DateDebut, Budget, Statut
FROM Projet;

-- Vue pour afficher les assignations
CREATE VIEW VueAssignations AS
SELECT AssignationId, EmployeId, ProjetId, NbreHeures
FROM Assignation;

-- Vue pour afficher les employés et les projets auxquels ils sont assignés
CREATE VIEW VueEmployesProjets AS
SELECT E.Matricule, E.Nom, E.Prenom, E.ProjetId, P.Titre AS TitreProjet
FROM Employe E
LEFT JOIN Assignation A ON E.Matricule = A.EmployeId
LEFT JOIN Projet P ON A.ProjetId = P.NumeroProjet;


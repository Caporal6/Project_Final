-- DROP DES ELEMENTS EXISTANTS

-- DROP DES PROCEDURES
DROP PROCEDURE IF EXISTS AjouterEmploye;
DROP PROCEDURE IF EXISTS ModifierInformationsEmploye;
DROP PROCEDURE IF EXISTS ObtenirEmployeParMatricule;
DROP PROCEDURE IF EXISTS GetEmployeeList;
DROP PROCEDURE IF EXISTS AjouterClient;
DROP PROCEDURE IF EXISTS ModifierInformationsClient;
DROP PROCEDURE IF EXISTS GetClientList;
DROP PROCEDURE IF EXISTS GetAllProjects;
DROP PROCEDURE IF EXISTS CreerProjet;
DROP PROCEDURE IF EXISTS ObtenirIdClientParNom;
DROP PROCEDURE IF EXISTS GetProjectsAndClientsWithDetails;
DROP PROCEDURE IF EXISTS GetEmployesProjetDetails;
DROP PROCEDURE IF EXISTS GetProjetByNumero;
DROP PROCEDURE IF EXISTS AssignerEmployeAProjet;
DROP PROCEDURE IF EXISTS VerifierAdministrateur;
DROP PROCEDURE IF EXISTS ModifierProjet;
DROP PROCEDURE IF EXISTS AjouterAdministrateur;
DROP PROCEDURE IF EXISTS ObtenirAdministrateurs;

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
DROP TRIGGER IF EXISTS after_insert_assignation_update_totalsalaires;

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
drop TABLE IF EXISTS Administrateur;

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
    TotalSalaires DECIMAL(10, 2) default 0,
    ClientIdentifiant INT,
    Statut VARCHAR(20) DEFAULT 'En cours',
    CONSTRAINT CK_NombreEmploye CHECK (EmployesRequis <= 5),
    CONSTRAINT Pk_Projet PRIMARY KEY (NumeroProjet),
    CONSTRAINT Fk_Projet_Client FOREIGN KEY (ClientIdentifiant) REFERENCES Client (Identifiant)
);


-- Création de la table Administrateur
CREATE TABLE IF NOT EXISTS Administrateur (
                                              Id INT AUTO_INCREMENT PRIMARY KEY,
                                              NomUtilisateur VARCHAR(50) NOT NULL UNIQUE,
                                              MotDePasseHash VARCHAR(64) NOT NULL -- Stocker le hachage du mot de passe (SHA-256)
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

-- Trigger qui va mettre a jour la colone totalsalaire de la table projet apres l'assignation d'un employer a un projet

DELIMITER //

CREATE TRIGGER after_insert_assignation_update_totalsalaires
    AFTER INSERT
    ON Assignation
    FOR EACH ROW
BEGIN
    DECLARE salaire DECIMAL(10, 2);

    -- Calculer le salaire pour la nouvelle assignation
    SET salaire = NEW.NbreHeures * (SELECT TauxHoraire FROM Employe WHERE Matricule = NEW.EmployeId);

    -- Mettre à jour le TotalSalaires dans la table Projet
    UPDATE Projet
    SET TotalSalaires = TotalSalaires + salaire
    WHERE NumeroProjet = NEW.ProjetId;
END //

DELIMITER ;



-- CREATION PROCEDURES STOCKEES


-- Procédure stockée pour vérifier le nom d'utilisateur et le mot de passe
DELIMITER //
CREATE PROCEDURE VerifierAdministrateur(
    IN p_NomUtilisateur VARCHAR(50),
    IN p_MotDePasse VARCHAR(255)
)
BEGIN
    -- Hacher le mot de passe avec SHA-256
    SET p_MotDePasse = SHA2(p_MotDePasse, 256);

    -- Sélectionner la ligne correspondant aux identifiants fournis
    SELECT *
    FROM Administrateur
    WHERE NomUtilisateur = p_NomUtilisateur AND MotDePasseHash = p_MotDePasse;
END //
DELIMITER ;


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

-- Procédure qui retourne la liste de tous les projets

CREATE PROCEDURE GetAllProjects()
BEGIN
    SELECT * FROM Projet;
END;

-- Procédure qui ajoute un projet

DELIMITER //

CREATE PROCEDURE CreerProjet(
    IN p_Titre VARCHAR(100),
    IN p_DateDebut DATE,
    IN p_Description TEXT,
    IN p_Budget DECIMAL(10, 2),
    IN p_EmployesRequis INT,
    IN p_ClientIdentifiant INT
)
BEGIN

    -- Insérer le nouveau projet
    INSERT INTO Projet (
        Titre,
        DateDebut,
        Description,
        Budget,
        EmployesRequis,
        ClientIdentifiant
    ) VALUES (
                 p_Titre,
                 p_DateDebut,
                 p_Description,
                 p_Budget,
                 p_EmployesRequis,
                 p_ClientIdentifiant
             );
END //

DELIMITER ;

-- Procedure qui retoure l'id d'un client grace a son nom

DELIMITER //

CREATE PROCEDURE ObtenirIdClientParNom(
    IN p_NomClient VARCHAR(50),
    OUT p_IdentifiantClient INT
)
BEGIN
    -- Initialiser l'identifiant à null
    SET p_IdentifiantClient = NULL;

    -- Rechercher l'identifiant du client par son nom
    SELECT Identifiant INTO p_IdentifiantClient
    FROM Client
    WHERE Nom = p_NomClient;

    -- Vérifier si le client a été trouvé
    IF p_IdentifiantClient IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Aucun client trouvé avec ce nom.';
    END IF;
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS ObtenirClientParNom;
//
CREATE PROCEDURE ObtenirClientParNom(IN NomParam VARCHAR(20))
BEGIN
    SELECT *
    FROM Client
    WHERE Nom = NomParam;
END //
DELIMITER ;

-- Procedure stocker qui retourne les projets et le nom du client qui est associer au projet

DELIMITER //

CREATE PROCEDURE GetProjectsAndClientsWithDetails()
BEGIN
    SELECT
        P.NumeroProjet,
        P.Titre AS TitreProjet,
        P.DateDebut,
        P.Description,
        P.Budget,
        P.EmployesRequis,
        P.TotalSalaires,
        P.Statut,
        C.*
    FROM
        Projet P
            LEFT JOIN
        Client C ON P.ClientIdentifiant = C.Identifiant;
END //

DELIMITER ;


-- Procedure stocker qui retourne tous les employes lier a un projet

DELIMITER //

CREATE PROCEDURE GetEmployesProjetDetails(
    IN p_NumeroProjet VARCHAR(20)
)
BEGIN
    SELECT
        E.Matricule,
        E.Nom,
        E.Prenom,
        E.TauxHoraire,
        E.PhotoIdentite,
        E.Statut,
        A.NbreHeures,
        E.TauxHoraire * A.NbreHeures AS Salaire
    FROM
        Employe E
            JOIN
        Assignation A ON E.Matricule = A.EmployeId
    WHERE
            A.ProjetId = p_NumeroProjet;
END //

DELIMITER ;

-- Procedure stocker qui retourne un projet en le cherchant par le numero de projet

DELIMITER //

CREATE PROCEDURE GetProjetByNumero(
    IN p_NumeroProjet VARCHAR(20)
)
BEGIN
    SELECT
        NumeroProjet,
        Titre,
        DateDebut,
        Description,
        Budget,
        EmployesRequis,
        TotalSalaires,
        ClientIdentifiant,
        Statut
    FROM
        Projet
    WHERE
            NumeroProjet = p_NumeroProjet;
END //

DELIMITER ;

-- Procédure pour assigner un employé à un projet
DELIMITER //

CREATE PROCEDURE AssignerEmployeAProjet(
    IN p_MatriculeEmploye VARCHAR(20),
    IN p_NumeroProjet VARCHAR(20),
    IN p_NbreHeures INT
)
BEGIN
    -- Vérifier si l'employé existe
    IF NOT EXISTS (SELECT 1 FROM Employe WHERE Matricule = p_MatriculeEmploye) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'L''employé spécifié n''existe pas.';
    END IF;

    -- Vérifier si le projet existe
    IF NOT EXISTS (SELECT 1 FROM Projet WHERE NumeroProjet = p_NumeroProjet) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Le projet spécifié n''existe pas.';
    END IF;

    -- Vérifier si l'employé a déjà été assigné à un projet
    IF EXISTS (SELECT 1 FROM Assignation WHERE EmployeId = p_MatriculeEmploye AND ProjetId = p_NumeroProjet) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'L''employé est déjà assigné à ce projet.';
    END IF;

    -- Vérifier si l'employé a déjà été assigné à un projet
    IF (SELECT ProjetId FROM Employe WHERE Matricule = p_MatriculeEmploye) IS NOT NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'L''employé est déjà assigné à un projet.';
    END IF;

    -- Insérer l'assignation
    INSERT INTO Assignation (EmployeId, ProjetId, NbreHeures)
    VALUES (p_MatriculeEmploye, p_NumeroProjet, p_NbreHeures);
END //

DELIMITER ;



DELIMITER //

CREATE PROCEDURE ModifierProjet(
    IN p_NumeroProjet VARCHAR(20),
    IN p_Titre VARCHAR(100),
    IN p_Description TEXT,
    IN p_Budget DECIMAL(10, 2),
    IN p_Statut VARCHAR(20)
)
BEGIN
    UPDATE Projet
    SET
        Titre = p_Titre,
        Description = p_Description,
        Budget = p_Budget,
        Statut = p_Statut
    WHERE NumeroProjet = p_NumeroProjet;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE AjouterAdministrateur(
    IN p_NomUtilisateur VARCHAR(50),
    IN p_MotDePasse VARCHAR(255)
)
BEGIN
    -- Hachage du mot de passe avec SHA-256
    DECLARE p_MotDePasseHash VARCHAR(64);
    SET p_MotDePasseHash = SHA2(p_MotDePasse, 256);

    -- Insertion dans la table
    INSERT INTO Administrateur (NomUtilisateur, MotDePasseHash)
    VALUES (p_NomUtilisateur, p_MotDePasseHash);
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE ObtenirAdministrateurs()
BEGIN
    SELECT * FROM Administrateur;
END //

DELIMITER ;






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

-- Insertion dans la table client

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (1, 'Client1', 'Adresse1', '1234567890', 'client1@email.com');

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (2, 'Client2', 'Adresse2', '9876543210', 'client2@email.com');

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (3, 'Client3', 'Adresse3', '1112233445', 'client3@email.com');

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (4, 'Client4', 'Adresse4', '5544332211', 'client4@email.com');

INSERT INTO Client (Identifiant, Nom, Adresse, NumeroTelephone, Email)
VALUES (5, 'Client5', 'Adresse5', '9876543210', 'client5@email.com');


-- Insertion dans la table Projet

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES ('P1', 'Projet1', '2023-01-01', 'Description1', 5000.00, 3, 0.00, 221, 'En cours');

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES ('P2', 'Projet2', '2023-02-01', 'Description2', 8000.00, 5, 0.00, 285, 'En cours');

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES ('P3', 'Projet3', '2023-03-01', 'Description3', 10000.00, 4, 0.00, 392, 'En cours');

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES ('P4', 'Projet4', '2023-04-01', 'Description4', 12000.00, 2, 0.00, 665, 'En cours');

INSERT INTO Projet (NumeroProjet, Titre, DateDebut, Description, Budget, EmployesRequis, TotalSalaires, ClientIdentifiant, Statut)
VALUES ('P5', 'Projet5', '2023-05-01', 'Description5', 15000.00, 5, 0.00, 839, 'En cours');

-- Insertion dans la table employe

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E1', 'Nom1', 'Prenom1', '1990-01-01', 'email1@email.com', 'Adresse1', '2022-01-01', 20.00, 'https://this-person-does-not-exist.com/img/avatar-gen1127327022fdaf218d25ad38bf43ceb9.jpg', 'Permanent', null);

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E2', 'Nom2', 'Prenom2', '1995-02-01', 'email2@email.com', 'Adresse2', '2021-02-01', 18.50, 'https://this-person-does-not-exist.com/img/avatar-gendbd5b847626d8e5d7a87c1d9dbe31f8f.jpg', 'Journalier', NULL);

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E3', 'Nom3', 'Prenom3', '1988-03-15', 'email3@email.com', 'Adresse3', '2020-03-15', 22.00, 'https://this-person-does-not-exist.com/img/avatar-gen11941bb5663cdff6cb47ca1a14afcf77.jpg', 'Permanent', null);

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E4', 'Nom4', 'Prenom4', '1992-04-20', 'email4@email.com', 'Adresse4', '2019-04-20', 19.75, 'https://this-person-does-not-exist.com/img/avatar-gen6fd598bdedea19322695ecbe422429e6.jpg', 'Journalier', NULL);

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E5', 'Nom5', 'Prenom5', '1997-05-10', 'email5@email.com', 'Adresse5', '2018-05-10', 21.50, 'https://this-person-does-not-exist.com/img/avatar-genc761d7f546238d5d5728e951bb10820f.jpg', 'Permanent', NULL);

INSERT INTO Employe (Matricule, Nom, Prenom, DateNaissance, Email, Adresse, DateEmbauche, TauxHoraire, PhotoIdentite, Statut, ProjetId)
VALUES ('E5', 'Nom6', 'Prenom6', '1997-05-10', 'email5@email.com', 'Adresse5', '2018-05-10', 21.50, 'https://this-person-does-not-exist.com/img/avatar-genc761d7f546238d5d5728e951bb10820f.jpg', 'Permanent', NULL);

-- Manage DB

select  * from client;
select  * from employe;
select  * from projet;
select  * from assignation;
select * from  administrateur;

delete  from employe;

delete  from administrateur;

-- insertion d'un administrateur
SET @nomUtilisateur = 'admin';
SET @motDePasse = 'motDePasseSecret';

SET @motDePasseHash = SHA2(@motDePasse, 256);

INSERT INTO Administrateur (NomUtilisateur, MotDePasseHash) VALUES (@nomUtilisateur, @motDePasseHash);















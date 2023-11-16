CREATE TABLE Client(

    id INT CHECK (id BETWEEN 100 AND 999) PRIMARY KEY,
    nom VARCHAR(255),
    adresse VARCHAR(255),
    numero_telephone VARCHAR(15),
    email VARCHAR(255) UNIQUE
);
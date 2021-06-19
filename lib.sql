CREATE DATABASE library;
USE library;

CREATE TABLE users(
    id int NOT NULL AUTO_INCREMENT,
    name varchar(255) NOT NULL,
    password varchar(64) NOT NULL,
    admin tinyint(1) NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE books(
    id int NOT NULL AUTO_INCREMENT,
    PRIMARY KEY (id),
    isbn int NOT NULL,
    title varchar(255) NOT NULL,
    author varchar(255) NOT NULL,
    publisher varchar(255) NOT NULL,
    genre varchar(255) NOT NULL,
    published int NOT NULL,
    pages int NOT NULL,
    fk_user_id INT,
    FOREIGN KEY(fk_user_id) REFERENCES users(id) ON DELETE CASCADE
);

INSERT INTO users(name,password,admin)
VALUES
(
    'Bob Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    1
),
(
    'Hans Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Dorte Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Lotte Bobsen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Gertrud Petersen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Peder Sorensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
);

INSERT INTO books(isbn, title, author, publisher, genre, published, pages)
VALUES
(
    123,
    'Pride and Prejudice',
    'Jane Austen',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    122,
    '1984',
    'George Orwell',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    125,
    'Crime and Punishment',
    'Fyodor Dostoyevsky',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    183,
    'Hamlet',
    'William Shakespeare',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    223,
    'One Hundres Years of Solitude',
    'Gabriel Garcia Marquez',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    1238,
    'Anna Karenina',
    'Leo Tolstoy',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    13,
    'The Stranger',
    'Albert Camus',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    23,
    'The Brothers Karamzov',
    'Fyodor Dostoyevsky',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    129,
    'The Odyssey',
    'Homer',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    12399,
    'The Arabian Nights',
    'Anonymous',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    243,
    'War and Peace',
    'Leo Tolstoy',
    'The pubhliser',
    'The genre',
    1992,
    300
);

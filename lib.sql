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
    FOREIGN KEY(fk_user_id) REFERENCES users(id)
);


INSERT INTO users(name,password,admin)
VALUES
(
    'Bob Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    1
),
(
    'hans Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'dfsa Jensen',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Lotte haf',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Has',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
),
(
    'Peder',
    '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',
    0
);

INSERT INTO books(isbn, title, author, publisher, genre, published, pages)
VALUES
(
    123,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    122,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    125,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    183,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    223,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    1238,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    13,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    23,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    129,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    12399,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
),
(
    243,
    'The book',
    'The author',
    'The pubhliser',
    'The genre',
    1992,
    300
);

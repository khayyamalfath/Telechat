-- Added Email field to Users table
-- Added RegisteredAt field to Users table

SHOW DATABASES;
CREATE DATABASE TelechatDevelopment;
USE TelechatDevelopment;

CREATE TABLE Users (
	Id INT AUTO_INCREMENT PRIMARY KEY,
	Name VARCHAR(255) UNIQUE NOT NULL,
	PasswordHash VARCHAR(512) NOT NULL,
	Email VARCHAR(255) UNIQUE,
	RegisteredAt DATETIME NOT NULL
);
 
CREATE TABLE Messages (
	Id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	MessageText TEXT NOT NULL,
	SentAt DATETIME NOT NULL,
	UserId INT NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
 
SHOW TABLES;
 
SELECT * FROM Messages;
SELECT * FROM Users;
 
DROP DATABASE TelechatDevelopment;
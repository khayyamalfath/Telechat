-- Added Users table
-- Deleted Username field from Messages table
-- Added UserId field to Messages table
-- Added foreign key in Messages table (Users table)

SHOW DATABASES;
CREATE DATABASE TelechatDevelopment;
USE TelechatDevelopment;

CREATE TABLE Users (
	Id INT AUTO_INCREMENT PRIMARY KEY,
	Name VARCHAR(255) UNIQUE NOT NULL,
	PasswordHash VARCHAR(255) NOT NULL,
	Email VARCHAR(255) UNIQUE,
	PhoneNumber VARCHAR(255) UNIQUE
);
 
CREATE TABLE Messages (
	Id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	MessageText TEXT NOT NULL,
	SentAt DATETIME NOT NULL,
	UserId INT NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
);
 
SHOW TABLES;
 
SELECT * FROM Messages;
 
DROP DATABASE TelechatDevelopment;

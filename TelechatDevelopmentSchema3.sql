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
	PasswordHash VARCHAR(512) NOT NULL
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

-- Renamed column name in Messages table from Message to MessageText

SHOW DATABASES;
CREATE DATABASE TelechatDevelopment;
USE TelechatDevelopment;
 
CREATE TABLE Messages (
	Id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	MessageText TEXT NOT NULL,
	Username VARCHAR(255) NOT NULL,
	SentAt DATETIME NOT NULL
);
 
SHOW TABLES;
 
SELECT * FROM Messages;
 
DROP DATABASE TelechatDevelopment;

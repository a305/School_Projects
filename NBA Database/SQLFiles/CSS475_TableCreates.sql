-- Creates Division table
CREATE TABLE DIVISION
(
divId		TINYINT			NOT NULL,
divName		VARCHAR(30)		NOT NULL,
confeName	VARCHAR(30)		NOT NULL,
PRIMARY KEY (divID),
CHECK (divID > 0)
);

-- Creates City table
CREATE TABLE CITY(
City_Id tinyint NOT NULL IDENTITY(1,1) PRIMARY KEY,
City_Name VARCHAR(20) NOT NULL,
CState VARCHAR(20) NOT NULL,
);

-- Creates Stadium table
CREATE TABLE STADIUM(
Stadium_Id tinyint NOT NULL IDENTITY(1,1) PRIMARY KEY,
Stadium_Name VARCHAR(20) NOT NULL,
Seats INT NOT NULL,
Year_Built smallint NOT NULL,
S_CityId tinyint NOT NULL FOREIGN KEY REFERENCES CITY(City_Id));

-- Creates Team table
CREATE TABLE TEAM(
champWins tinyint NOT NULL,
team_Name varchar(30) NOT NULL,
t_City tinyint NOT NULL,
d_Id tinyint NOT NULL,
away_Percent tinyint NOT NULL CHECK(away_Percent >= 0 AND away_Percent <= 100),
home_Percent tinyint NOT NULL CHECK(home_Percent >= 0 AND home_Percent <= 100),
PRIMARY KEY(team_Name),
FOREIGN KEY(t_City) REFERENCES CITY(city_Id),
FOREIGN KEY(d_Id) REFERENCES DIVISION(divId));

-- Creates Coach table
CREATE TABLE COACH(
coach_Name varchar(30) NOT NULL,
t_Name varchar(30) NOT NULL,
c_champWins tinyint NOT NULL,
PRIMARY KEY(coach_Name, t_Name),
FOREIGN KEY(t_Name) REFERENCES TEAM(team_Name));

-- Creates Owner table
CREATE TABLE TEAM_OWNER(
id_Number tinyint NOT NULL,
owner_Name varchar(30) NOT NULL,
t_Purch_Value int NOT NULL,
tm_Name varchar(30) NOT NULL,
PRIMARY KEY(id_Number),
FOREIGN KEY(tm_Name) REFERENCES TEAM(team_Name));

-- Creates Team Worth table
CREATE TABLE TEAM_WORTH(
t_Name varchar(30) NOT NULL,
t_Value int NOT NULL,
PRIMARY KEY(t_Name),
FOREIGN KEY(t_Name) REFERENCES TEAM(team_Name));

-- Creates Player Profile table
CREATE TABLE PLAYER_PROFILE(
PLAYER_ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
PNAME VARCHAR(25) NOT NULL,
DOB DATE NOT NULL,
COLLEGE VARCHAR(20) NULL,
HEIGHT TINYINT NOT NULL,
WEIGHT TINYINT NOT NULL,
POSITION VARCHAR(2));

-- Creates Player Stats table
CREATE TABLE PLAYER_STATS (
PLAYER_ID INT NOT NULL FOREIGN KEY REFERENCES PLAYER_PROFILE(PLAYER_ID) PRIMARY KEY,
FG DECIMAL(4,2) NOT NULL CHECK(FG >= 0 AND FG <= 1),
THREEFG DECIMAL(4,2) NOT NULL CHECK(THREEFG>=0 AND THREEFG<=1),
PPG TINYINT NOT NULL,
RPG TINYINT NOT NULL,
APG TINYINT NOT NULL,
TOPG TINYINT NOT NULL);

-- Creates Player Contract table
CREATE TABLE PLAYER_CONTRACT (
PLAYER_ID INT NOT NULL FOREIGN KEY REFERENCES PLAYER_PROFILE(PLAYER_ID) PRIMARY KEY,
TEAM VARCHAR(30) NOT NULL FOREIGN KEY REFERENCES TEAM(team_Name),
SALARY INT NOT NULL CHECK(SALARY>=0),
YEAR_START INT NOT NULL,
YEAR_END INT NOT NULL, 
CONSTRAINT CHK_ContractYear 
CHECK(YEAR_END>=YEAR_START));

-- Creates Sponsor table
CREATE TABLE SPONSOR
(
sponsorId				INT				NOT NULL,
sponsorName				VARCHAR(50)		NOT NULL,
sponsorBusiness			VARCHAR(50),
PRIMARY KEY (sponsorId),
CHECK (sponsorId > 0)
);

-- Creates Sponsor Contract table
CREATE TABLE SPONSOR_CONTRACT
(
spId				INT		NOT NULL,
plId				INT		NOT NULL,
amount				INT		NOT NULL,
startDate			DATE,
endDate				DATE,
PRIMARY KEY (spId, plId),
FOREIGN KEY (spId) REFERENCES SPONSOR(sponsorId),
FOREIGN KEY (plId) REFERENCES PLAYER_PROFILE(player_Id),
CHECK (spId > 0 AND plId > 0)
);


-- Hand created inserts to be used in testing of the database.
SET IDENTITY_INSERT dbo.CITY ON;

INSERT INTO CITY(City_Id, City_Name, CState) VALUES (1, 'Seattle', 'WA');
INSERT INTO CITY(City_Id, City_Name, CState) VALUES (2, 'Los Angles', 'CA');

INSERT INTO DIVISION VALUES(1, 'Pacific', 'Western Conference');

INSERT INTO TEAM VALUES (100, 'Seattle Sonics', 1, 1, 60, 80);
INSERT INTO TEAM VALUES (99, 'Lakers', 2, 1, 55, 70);

INSERT INTO COACH VALUES ('Jon Doe', 'Seattle Sonics', 100);
INSERT INTO COACH VALUES ('Jane Doe', 'Lakers', 99);

INSERT INTO TEAM_OWNER VALUES (1, 'Bill Gates', 2000000, 'Seattle Sonics');
INSERT INTO TEAM_OWNER VALUES (2, 'Paul Allen', 1000000, 'Lakers');

INSERT INTO TEAM_WORTH VALUES ('Seattle Sonics', 5000000);
INSERT INTO TEAM_WORTH VALUES ('Lakers', 4000000);

INSERT INTO STADIUM (Stadium_Name, Seats, Year_Built, S_CityId) VALUES ('Key Arena', 30000, 1987, 1);
INSERT INTO STADIUM (Stadium_Name, Seats, Year_Built, S_CityId) VALUES ('ABC', 28000, 2017, 2);

INSERT INTO PLAYER_PROFILE
           (PNAME
           ,DOB
           ,COLLEGE
           ,HEIGHT
           ,WEIGHT
           ,POSITION)
  VALUES ('Arthur James'
           ,convert(date,'19850817')
,'St. Vincent'
           ,207
           ,250
           ,'SF')

INSERT INTO PLAYER_STATS VALUES (1, 0.3, 0.1, 1, 2, 3, 4);

INSERT INTO PLAYER_CONTRACT VALUES (1, 'Seattle Sonics', 100000, 2016, 2018);

INSERT INTO SPONSOR VALUES (12, 'Microsoft', 'Technology');
INSERT INTO SPONSOR VALUES (29, 'UPS', 'Shipping');

INSERT INTO SPONSOR_CONTRACT VALUES (12, 1, 1000000, '2015-01-12', '2017-01-12');
INSERT INTO SPONSOR_CONTRACT VALUES (29, 1, 200000, '2017-11-02', '2018-11-02');




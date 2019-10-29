create database myIotDb

go

use myIotDb

create table Administrators
(
  AdName nvarchar(50) primary key,
  AdPassword nvarchar(50) not null,
  AdLevel nvarchar(50) not null
)

create table Users
(
  UsID nvarchar(50) primary key,
  UsName nvarchar(50) not null,
  UsCreateTime datetime not null
)

create table UserSignIn
(
  UsID nvarchar(50) foreign key references Users(UsID),
  UsSignInTime datetime not null
)

go

insert into Administrators(AdName,AdPassword,AdLevel) values('admin','admin','超级管理员')


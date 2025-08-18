DROP SCHEMA IF EXISTS `usuarios` ;

create database usuarios;

use usuarios;

create table __efmigrationshistory
(
    MigrationId    varchar(150) not null
        primary key,
    ProductVersion varchar(32)  not null
);

create table aspnetroles
(
    Id               varchar(255) not null
        primary key,
    Name             varchar(256) null,
    NormalizedName   varchar(256) null,
    ConcurrencyStamp longtext     null,
    constraint RoleNameIndex
        unique (NormalizedName)
);

create table aspnetroleclaims
(
    Id         int auto_increment
        primary key,
    RoleId     varchar(255) not null,
    ClaimType  longtext     null,
    ClaimValue longtext     null,
    constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
        foreign key (RoleId) references aspnetroles (Id)
            on delete cascade
);

create index IX_AspNetRoleClaims_RoleId
    on aspnetroleclaims (RoleId);

create table aspnetusers
(
    Id                   varchar(255)                                     not null
primary key,
    UserName             varchar(256)                                     null,
    NormalizedUserName   varchar(256)                                     null,
    Email                varchar(256)                                     null,
    NormalizedEmail      varchar(256)                                     null,
    EmailConfirmed       tinyint(1)                                       not null,
    PasswordHash         longtext                                         null,
    SecurityStamp        longtext                                         null,
    ConcurrencyStamp     longtext                                         null,
    PhoneNumber          longtext                                         null,
    PhoneNumberConfirmed tinyint(1)                                       not null,
    TwoFactorEnabled     tinyint(1)                                       not null,
    LockoutEnd           datetime                                         null,
    LockoutEnabled       tinyint(1)                                       not null,
    AccessFailedCount    int                                              not null,
    Cpf                  varchar(14) default ''                           not null,
    BirthDate            datetime(6) default '0001-01-01 00:00:00.000000' not null,
    constraint UserNameIndex
        unique (NormalizedUserName)
);

create table aspnetuserclaims
(
    Id         int auto_increment
        primary key,
    UserId     varchar(255) not null,
    ClaimType  longtext     null,
    ClaimValue longtext     null,
    constraint FK_AspNetUserClaims_AspNetUsers_UserId
        foreign key (UserId) references aspnetusers (Id)
            on delete cascade
);

create index IX_AspNetUserClaims_UserId
    on aspnetuserclaims (UserId);

create table aspnetuserlogins
(
    LoginProvider       varchar(255) not null,
    ProviderKey         varchar(255) not null,
    ProviderDisplayName longtext     null,
    UserId              varchar(255) not null,
    primary key (LoginProvider, ProviderKey),
    constraint FK_AspNetUserLogins_AspNetUsers_UserId
        foreign key (UserId) references aspnetusers (Id)
            on delete cascade
);

create index IX_AspNetUserLogins_UserId
    on aspnetuserlogins (UserId);

create table aspnetuserroles
(
    UserId varchar(255) not null,
    RoleId varchar(255) not null,
    primary key (UserId, RoleId),
    constraint FK_AspNetUserRoles_AspNetRoles_RoleId
        foreign key (RoleId) references aspnetroles (Id)
            on delete cascade,
    constraint FK_AspNetUserRoles_AspNetUsers_UserId
        foreign key (UserId) references aspnetusers (Id)
            on delete cascade
);

create index IX_AspNetUserRoles_RoleId
    on aspnetuserroles (RoleId);

create index EmailIndex
    on aspnetusers (NormalizedEmail);

create table aspnetusertokens
(
    UserId        varchar(255) not null,
    LoginProvider varchar(255) not null,
    Name          varchar(255) not null,
    Value         longtext     null,
    primary key (UserId, LoginProvider, Name),
    constraint FK_AspNetUserTokens_AspNetUsers_UserId
        foreign key (UserId) references aspnetusers (Id)
            on delete cascade
);


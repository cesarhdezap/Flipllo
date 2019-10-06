
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/06/2019 10:27:06
-- Generated from EDMX file: C:\Users\CesarHA\source\repos\Flipllo\FliplloServidor\Flipllo\AccesoABaseDeDatos\ModelFlipllo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BDFlipllo];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsuarioSet'
CREATE TABLE [dbo].[UsuarioSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombreDeUsuario] nvarchar(max)  NOT NULL,
    [Contraseña] nvarchar(max)  NOT NULL,
    [CorreoElectronico] nvarchar(max)  NOT NULL,
    [Estado] int  NOT NULL,
    [Puntuacion_Id] int  NOT NULL
);
GO

-- Creating table 'PuntuacionSet'
CREATE TABLE [dbo].[PuntuacionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PartidasJugadas] int  NOT NULL,
    [Victorias] int  NOT NULL,
    [ExperienciaTotal] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UsuarioSet'
ALTER TABLE [dbo].[UsuarioSet]
ADD CONSTRAINT [PK_UsuarioSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PuntuacionSet'
ALTER TABLE [dbo].[PuntuacionSet]
ADD CONSTRAINT [PK_PuntuacionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Puntuacion_Id] in table 'UsuarioSet'
ALTER TABLE [dbo].[UsuarioSet]
ADD CONSTRAINT [FK_UsuarioPuntuacion]
    FOREIGN KEY ([Puntuacion_Id])
    REFERENCES [dbo].[PuntuacionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioPuntuacion'
CREATE INDEX [IX_FK_UsuarioPuntuacion]
ON [dbo].[UsuarioSet]
    ([Puntuacion_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/16/2019 01:30:19
-- Generated from EDMX file: C:\Users\CesarHA\source\repos\cesarhdezap\Flipllo\FliplloServidor\Flipllo\AccesoABaseDeDatos\ModelFlipllo.edmx
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

IF OBJECT_ID(N'[dbo].[FK_UsuarioObjetoEnInventario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjetoEnInventarioSet] DROP CONSTRAINT [FK_UsuarioObjetoEnInventario];
GO
IF OBJECT_ID(N'[dbo].[FK_ObjetoEnCofreObjeto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjetoEnCofreSet] DROP CONSTRAINT [FK_ObjetoEnCofreObjeto];
GO
IF OBJECT_ID(N'[dbo].[FK_ObjetoEnInventarioObjeto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjetoEnInventarioSet] DROP CONSTRAINT [FK_ObjetoEnInventarioObjeto];
GO
IF OBJECT_ID(N'[dbo].[FK_CofreObjetoEnCofre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjetoEnCofreSet] DROP CONSTRAINT [FK_CofreObjetoEnCofre];
GO
IF OBJECT_ID(N'[dbo].[FK_CofreUsuario_Cofre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CofreUsuario] DROP CONSTRAINT [FK_CofreUsuario_Cofre];
GO
IF OBJECT_ID(N'[dbo].[FK_CofreUsuario_Usuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CofreUsuario] DROP CONSTRAINT [FK_CofreUsuario_Usuario];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UsuarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[ObjetoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObjetoSet];
GO
IF OBJECT_ID(N'[dbo].[ObjetoEnInventarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObjetoEnInventarioSet];
GO
IF OBJECT_ID(N'[dbo].[ObjetoEnCofreSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObjetoEnCofreSet];
GO
IF OBJECT_ID(N'[dbo].[CofreSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CofreSet];
GO
IF OBJECT_ID(N'[dbo].[CofreUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CofreUsuario];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsuarioSet'
CREATE TABLE [dbo].[UsuarioSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombreDeUsuario] nvarchar(max)  NOT NULL,
    [Contraseña] nvarchar(max)  NOT NULL,
    [CorreoElectronico] nvarchar(max)  NOT NULL,
    [Estado] smallint  NOT NULL,
    [PartidasJugadas] smallint  NOT NULL,
    [ExperienciaTotal] float  NOT NULL,
    [CodigoDeVerificacion] nvarchar(max)  NOT NULL,
    [Victorias] smallint  NOT NULL
);
GO

-- Creating table 'ObjetoSet'
CREATE TABLE [dbo].[ObjetoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] smallint  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Imagen] tinyint  NOT NULL
);
GO

-- Creating table 'ObjetoEnInventarioSet'
CREATE TABLE [dbo].[ObjetoEnInventarioSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaDeObtencion] datetime  NOT NULL,
    [Usuario_Id] int  NOT NULL,
    [Objeto_Id] int  NOT NULL
);
GO

-- Creating table 'ObjetoEnCofreSet'
CREATE TABLE [dbo].[ObjetoEnCofreSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProbabilidadDeObtener] decimal(18,0)  NOT NULL,
    [Objeto_Id] int  NOT NULL,
    [Cofre_Id] int  NOT NULL
);
GO

-- Creating table 'CofreSet'
CREATE TABLE [dbo].[CofreSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Imagen] nvarchar(max)  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CofreUsuario'
CREATE TABLE [dbo].[CofreUsuario] (
    [Cofre_Id] int  NOT NULL,
    [Usuario_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'ObjetoSet'
ALTER TABLE [dbo].[ObjetoSet]
ADD CONSTRAINT [PK_ObjetoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ObjetoEnInventarioSet'
ALTER TABLE [dbo].[ObjetoEnInventarioSet]
ADD CONSTRAINT [PK_ObjetoEnInventarioSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ObjetoEnCofreSet'
ALTER TABLE [dbo].[ObjetoEnCofreSet]
ADD CONSTRAINT [PK_ObjetoEnCofreSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CofreSet'
ALTER TABLE [dbo].[CofreSet]
ADD CONSTRAINT [PK_CofreSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Cofre_Id], [Usuario_Id] in table 'CofreUsuario'
ALTER TABLE [dbo].[CofreUsuario]
ADD CONSTRAINT [PK_CofreUsuario]
    PRIMARY KEY CLUSTERED ([Cofre_Id], [Usuario_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Usuario_Id] in table 'ObjetoEnInventarioSet'
ALTER TABLE [dbo].[ObjetoEnInventarioSet]
ADD CONSTRAINT [FK_UsuarioObjetoEnInventario]
    FOREIGN KEY ([Usuario_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioObjetoEnInventario'
CREATE INDEX [IX_FK_UsuarioObjetoEnInventario]
ON [dbo].[ObjetoEnInventarioSet]
    ([Usuario_Id]);
GO

-- Creating foreign key on [Objeto_Id] in table 'ObjetoEnCofreSet'
ALTER TABLE [dbo].[ObjetoEnCofreSet]
ADD CONSTRAINT [FK_ObjetoEnCofreObjeto]
    FOREIGN KEY ([Objeto_Id])
    REFERENCES [dbo].[ObjetoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjetoEnCofreObjeto'
CREATE INDEX [IX_FK_ObjetoEnCofreObjeto]
ON [dbo].[ObjetoEnCofreSet]
    ([Objeto_Id]);
GO

-- Creating foreign key on [Objeto_Id] in table 'ObjetoEnInventarioSet'
ALTER TABLE [dbo].[ObjetoEnInventarioSet]
ADD CONSTRAINT [FK_ObjetoEnInventarioObjeto]
    FOREIGN KEY ([Objeto_Id])
    REFERENCES [dbo].[ObjetoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjetoEnInventarioObjeto'
CREATE INDEX [IX_FK_ObjetoEnInventarioObjeto]
ON [dbo].[ObjetoEnInventarioSet]
    ([Objeto_Id]);
GO

-- Creating foreign key on [Cofre_Id] in table 'ObjetoEnCofreSet'
ALTER TABLE [dbo].[ObjetoEnCofreSet]
ADD CONSTRAINT [FK_CofreObjetoEnCofre]
    FOREIGN KEY ([Cofre_Id])
    REFERENCES [dbo].[CofreSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CofreObjetoEnCofre'
CREATE INDEX [IX_FK_CofreObjetoEnCofre]
ON [dbo].[ObjetoEnCofreSet]
    ([Cofre_Id]);
GO

-- Creating foreign key on [Cofre_Id] in table 'CofreUsuario'
ALTER TABLE [dbo].[CofreUsuario]
ADD CONSTRAINT [FK_CofreUsuario_Cofre]
    FOREIGN KEY ([Cofre_Id])
    REFERENCES [dbo].[CofreSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Usuario_Id] in table 'CofreUsuario'
ALTER TABLE [dbo].[CofreUsuario]
ADD CONSTRAINT [FK_CofreUsuario_Usuario]
    FOREIGN KEY ([Usuario_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CofreUsuario_Usuario'
CREATE INDEX [IX_FK_CofreUsuario_Usuario]
ON [dbo].[CofreUsuario]
    ([Usuario_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
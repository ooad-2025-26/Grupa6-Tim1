IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [IdentityRoles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [NormalizedName] nvarchar(450) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_IdentityRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [IdentityUserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_IdentityUserRoles] PRIMARY KEY ([UserId], [RoleId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [Korisnici] (
        [Id] int NOT NULL IDENTITY,
        [Ime] nvarchar(50) NOT NULL,
        [Prezime] nvarchar(50) NOT NULL,
        [BrojIndeksa] nvarchar(20) NULL,
        [Uloga] int NOT NULL,
        [Telefon] nvarchar(30) NULL,
        [Adresa] nvarchar(150) NULL,
        [UserName] nvarchar(max) NULL,
        [NormalizedUserName] nvarchar(450) NULL,
        [Email] nvarchar(max) NULL,
        [NormalizedEmail] nvarchar(450) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Korisnici] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [Obroci] (
        [Id] int NOT NULL IDENTITY,
        [Naziv] nvarchar(80) NOT NULL,
        [Cijena] decimal(8,2) NOT NULL,
        [Opis] nvarchar(300) NOT NULL,
        [Sastojci] nvarchar(300) NOT NULL,
        [Dostupan] bit NOT NULL,
        [Kolicina] int NOT NULL,
        CONSTRAINT [PK_Obroci] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [Meniji] (
        [Id] int NOT NULL IDENTITY,
        [Datum] datetime2 NOT NULL,
        [ObrokId] int NOT NULL,
        CONSTRAINT [PK_Meniji] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Meniji_Obroci_ObrokId] FOREIGN KEY ([ObrokId]) REFERENCES [Obroci] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [Rezervacije] (
        [Id] int NOT NULL IDENTITY,
        [Datum] datetime2 NOT NULL,
        [TerminPreuzimanja] time NOT NULL,
        [Status] int NOT NULL,
        [NacinPreuzimanja] int NOT NULL,
        [KorisnikId] int NOT NULL,
        [ObrokId] int NOT NULL,
        CONSTRAINT [PK_Rezervacije] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rezervacije_Korisnici_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnici] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Rezervacije_Obroci_ObrokId] FOREIGN KEY ([ObrokId]) REFERENCES [Obroci] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE TABLE [QRKodovi] (
        [Id] int NOT NULL IDENTITY,
        [Validan] bit NOT NULL,
        [VrijemeGenerisanja] datetime2 NOT NULL,
        [Kod] nvarchar(80) NOT NULL,
        [RezervacijaId] int NOT NULL,
        CONSTRAINT [PK_QRKodovi] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QRKodovi_Rezervacije_RezervacijaId] FOREIGN KEY ([RezervacijaId]) REFERENCES [Rezervacije] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cijena', N'Dostupan', N'Kolicina', N'Naziv', N'Opis', N'Sastojci') AND [object_id] = OBJECT_ID(N'[Obroci]'))
        SET IDENTITY_INSERT [Obroci] ON;
    EXEC(N'INSERT INTO [Obroci] ([Id], [Cijena], [Dostupan], [Kolicina], [Naziv], [Opis], [Sastojci])
    VALUES (1, 5.5, CAST(1 AS bit), 30, N''Pileci file sa rizom'', N''Topli studentski obrok sa prilogom i salatom.'', N''Piletina, riza, salata''),
    (2, 4.8, CAST(1 AS bit), 18, N''Vegetarijanska pasta'', N''Pasta sa povrcem i paradajz sosom.'', N''Pasta, tikvice, paprika, paradajz''),
    (3, 3.2, CAST(1 AS bit), 24, N''Corba i domace pecivo'', N''Lagani dnevni obrok za pauzu izmedju predavanja.'', N''Povrce, zacini, pecivo'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cijena', N'Dostupan', N'Kolicina', N'Naziv', N'Opis', N'Sastojci') AND [object_id] = OBJECT_ID(N'[Obroci]'))
        SET IDENTITY_INSERT [Obroci] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Datum', N'ObrokId') AND [object_id] = OBJECT_ID(N'[Meniji]'))
        SET IDENTITY_INSERT [Meniji] ON;
    EXEC(N'INSERT INTO [Meniji] ([Id], [Datum], [ObrokId])
    VALUES (1, ''2026-06-01T00:00:00.0000000'', 1),
    (2, ''2026-06-01T00:00:00.0000000'', 2),
    (3, ''2026-06-02T00:00:00.0000000'', 3)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Datum', N'ObrokId') AND [object_id] = OBJECT_ID(N'[Meniji]'))
        SET IDENTITY_INSERT [Meniji] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [IdentityRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Korisnici] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Korisnici] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE INDEX [IX_Meniji_ObrokId] ON [Meniji] ([ObrokId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE UNIQUE INDEX [IX_QRKodovi_RezervacijaId] ON [QRKodovi] ([RezervacijaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE INDEX [IX_Rezervacije_KorisnikId] ON [Rezervacije] ([KorisnikId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    CREATE INDEX [IX_Rezervacije_ObrokId] ON [Rezervacije] ([ObrokId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260531203332_InitialCampusEatsIdentity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260531203332_InitialCampusEatsIdentity', N'10.0.8');
END;

COMMIT;
GO


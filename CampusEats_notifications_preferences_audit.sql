BEGIN TRANSACTION;
ALTER TABLE [Rezervacije] DROP CONSTRAINT [FK_Rezervacije_Korisnici_KorisnikId];

ALTER TABLE [Rezervacije] ADD [KurirId] int NULL;

ALTER TABLE [Korisnici] ADD [Alergije] nvarchar(300) NULL;

ALTER TABLE [Korisnici] ADD [OmiljenaHrana] nvarchar(300) NULL;

ALTER TABLE [Korisnici] ADD [Vegetarijanac] bit NOT NULL DEFAULT CAST(0 AS bit);

CREATE TABLE [Aktivnosti] (
    [Id] int NOT NULL IDENTITY,
    [KorisnikEmail] nvarchar(120) NULL,
    [Akcija] nvarchar(80) NOT NULL,
    [Entitet] nvarchar(80) NULL,
    [EntitetId] int NULL,
    [Opis] nvarchar(700) NOT NULL,
    [Vrijeme] datetime2 NOT NULL,
    CONSTRAINT [PK_Aktivnosti] PRIMARY KEY ([Id])
);

CREATE TABLE [HistorijaIzmjena] (
    [Id] int NOT NULL IDENTITY,
    [Entitet] nvarchar(80) NOT NULL,
    [EntitetId] int NOT NULL,
    [TipIzmjene] nvarchar(80) NOT NULL,
    [KorisnikEmail] nvarchar(120) NULL,
    [Opis] nvarchar(700) NOT NULL,
    [Vrijeme] datetime2 NOT NULL,
    CONSTRAINT [PK_HistorijaIzmjena] PRIMARY KEY ([Id])
);

CREATE TABLE [Obavijesti] (
    [Id] int NOT NULL IDENTITY,
    [KorisnikId] int NULL,
    [RezervacijaId] int NULL,
    [Naslov] nvarchar(120) NOT NULL,
    [Poruka] nvarchar(600) NOT NULL,
    [DatumSlanja] datetime2 NOT NULL,
    [Procitana] bit NOT NULL,
    CONSTRAINT [PK_Obavijesti] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Obavijesti_Korisnici_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnici] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Obavijesti_Rezervacije_RezervacijaId] FOREIGN KEY ([RezervacijaId]) REFERENCES [Rezervacije] ([Id]) ON DELETE SET NULL
);

CREATE INDEX [IX_Rezervacije_KurirId] ON [Rezervacije] ([KurirId]);

CREATE INDEX [IX_Obavijesti_KorisnikId] ON [Obavijesti] ([KorisnikId]);

CREATE INDEX [IX_Obavijesti_RezervacijaId] ON [Obavijesti] ([RezervacijaId]);

ALTER TABLE [Rezervacije] ADD CONSTRAINT [FK_Rezervacije_Korisnici_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnici] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Rezervacije] ADD CONSTRAINT [FK_Rezervacije_Korisnici_KurirId] FOREIGN KEY ([KurirId]) REFERENCES [Korisnici] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260609215715_CampusEatsNotificationsPreferencesAudit', N'10.0.8');

COMMIT;
GO


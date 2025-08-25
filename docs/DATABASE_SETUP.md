# Guide de Configuration de la Base de Données AdeauMao

Ce guide détaillé vous accompagne dans la configuration complète de la base de données SQL Server pour le système AdeauMao. Nous couvrirons l'installation, la configuration, les migrations, et les meilleures pratiques pour une base de données robuste et performante.

## Table des Matières

1. [Vue d'ensemble de l'architecture de données](#vue-densemble-de-larchitecture-de-données)
2. [Installation de SQL Server](#installation-de-sql-server)
3. [Configuration de l'instance SQL Server](#configuration-de-linstance-sql-server)
4. [Création et configuration de la base de données](#création-et-configuration-de-la-base-de-données)
5. [Exécution des migrations Entity Framework](#exécution-des-migrations-entity-framework)
6. [Données de seed et initialisation](#données-de-seed-et-initialisation)
7. [Configuration des performances](#configuration-des-performances)
8. [Sécurité et permissions](#sécurité-et-permissions)
9. [Sauvegarde et restauration](#sauvegarde-et-restauration)
10. [Monitoring et maintenance](#monitoring-et-maintenance)

## Vue d'ensemble de l'architecture de données

AdeauMao utilise une architecture de base de données relationnelle optimisée pour les systèmes de GMAO (Gestion de Maintenance Assistée par Ordinateur). La structure de données est conçue pour supporter efficacement les opérations de maintenance industrielle tout en maintenant l'intégrité référentielle et les performances.

### Modèle de données principal

Le modèle de données AdeauMao s'articule autour de plusieurs entités centrales qui reflètent les processus métier de la maintenance industrielle.

**Équipements et Infrastructure** : Au cœur du système se trouvent les équipements industriels, organisés hiérarchiquement par sites et lignes de production. Chaque équipement peut contenir plusieurs organes (composants) qui peuvent faire l'objet de maintenance spécifique. Cette structure permet une gestion granulaire des actifs industriels.

**Ressources Humaines** : Le système gère les employés avec leurs compétences spécifiques, leur organisation en équipes, et leurs rôles dans les processus de maintenance. Cette organisation permet une assignation optimale des tâches selon les compétences disponibles.

**Processus de Maintenance** : Les demandes d'intervention initient le processus de maintenance, qui se concrétise par des ordres de travail. Ces ordres suivent des workflows configurables et peuvent être liés à des plans de maintenance préventive.

**Gestion des Stocks** : Le système intègre la gestion des pièces de rechange et consommables nécessaires aux interventions, avec suivi des stocks et gestion des fournisseurs.

### Relations et contraintes

Les relations entre entités sont soigneusement définies pour maintenir la cohérence des données :

- **Intégrité référentielle** : Toutes les clés étrangères sont protégées par des contraintes pour éviter les références orphelines
- **Contraintes métier** : Des contraintes CHECK valident les règles métier (ex: dates cohérentes, statuts valides)
- **Index optimisés** : Les index sont positionnés sur les colonnes fréquemment utilisées dans les requêtes et jointures

### Stratégie de nommage

La base de données suit une convention de nommage cohérente :
- **Tables** : Noms au singulier en français (ex: Equipement, Employe)
- **Colonnes** : PascalCase en français (ex: NomEquipement, DateCreation)
- **Clés primaires** : Toujours nommées "Id"
- **Clés étrangères** : Nom de l'entité + "Id" (ex: EquipementId)
- **Index** : Préfixe IX_ suivi du nom de table et colonnes

## Installation de SQL Server

L'installation de SQL Server doit être effectuée avec soin pour assurer une base solide pour AdeauMao.

### Choix de l'édition SQL Server

**SQL Server Express** : Gratuit et suffisant pour les petites installations (jusqu'à 10 GB de données). Idéal pour le développement et les petites entreprises. Limitations : 1 GB de RAM maximum, 1 CPU, pas d'Agent SQL Server.

**SQL Server Standard** : Recommandé pour la plupart des installations de production. Supporte jusqu'à 128 GB de RAM et 24 cœurs. Inclut toutes les fonctionnalités nécessaires pour AdeauMao.

**SQL Server Enterprise** : Pour les grandes installations nécessitant des fonctionnalités avancées comme la compression de données, le partitionnement, ou la haute disponibilité.

### Prérequis système

Avant l'installation, vérifiez que votre système répond aux exigences :

**Système d'exploitation** : Windows Server 2016 ou supérieur, Windows 10/11 pour le développement.

**Mémoire** : Minimum 4 GB RAM, 8 GB recommandés pour SQL Server Standard, 16 GB pour les environnements de production.

**Stockage** : SSD recommandé pour les fichiers de données et logs. Prévoyez au moins 50 GB d'espace libre pour l'installation et la croissance initiale.

**Réseau** : Configurez le pare-feu pour autoriser le port 1433 (ou le port personnalisé choisi).

### Processus d'installation

**Téléchargement** : Téléchargez SQL Server depuis le site officiel Microsoft. Choisissez l'édition appropriée selon vos besoins.

**Lancement de l'installation** : Exécutez le programme d'installation en tant qu'administrateur. Choisissez "Installation personnalisée" pour avoir un contrôle complet sur la configuration.

**Sélection des fonctionnalités** : Pour AdeauMao, sélectionnez au minimum :
- Services de moteur de base de données
- Outils de gestion - Complets (inclut SSMS)
- Connectivité des outils clients
- Integration Services (si vous prévoyez des imports/exports de données)

**Configuration de l'instance** : Choisissez "Instance par défaut" sauf si vous avez des besoins spécifiques. Le nom de l'instance sera alors "MSSQLSERVER".

**Configuration du service** : Configurez le service SQL Server pour démarrer automatiquement. Utilisez un compte de service dédié plutôt que le compte système local pour une meilleure sécurité.

**Configuration de l'authentification** : Choisissez "Mode mixte" pour permettre l'authentification SQL Server et Windows. Définissez un mot de passe fort pour le compte "sa".

**Configuration des répertoires** : Séparez les fichiers de données et de logs sur des disques différents si possible pour optimiser les performances.

### Post-installation

**Installation de SQL Server Management Studio (SSMS)** : Si ce n'est pas déjà fait, téléchargez et installez SSMS séparément. Cet outil est essentiel pour la gestion de la base de données.

**Configuration du pare-feu** : Ajoutez une exception pour SQL Server dans le pare-feu Windows :
```cmd
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433
```

**Test de connectivité** : Testez la connexion avec SSMS en utilisant l'authentification Windows et SQL Server.

## Configuration de l'instance SQL Server

Une configuration appropriée de l'instance SQL Server est cruciale pour les performances et la sécurité d'AdeauMao.

### Configuration de la mémoire

**Mémoire maximale du serveur** : Configurez la mémoire maximale pour laisser suffisamment de RAM au système d'exploitation. Règle générale : RAM totale - 2-4 GB pour l'OS.

```sql
-- Exemple pour un serveur avec 16 GB de RAM
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'max server memory', 12288; -- 12 GB en MB
RECONFIGURE;
```

**Mémoire minimale du serveur** : Définissez une mémoire minimale pour éviter que SQL Server libère trop de mémoire sous pression.

```sql
EXEC sp_configure 'min server memory', 2048; -- 2 GB en MB
RECONFIGURE;
```

### Configuration des fichiers de base de données

**Croissance automatique** : Configurez la croissance automatique en taille fixe plutôt qu'en pourcentage pour éviter les fragmentations.

**Fichiers de données multiples** : Pour les grandes bases de données, utilisez plusieurs fichiers de données pour améliorer les performances.

**Séparation des logs** : Placez les fichiers de logs sur un disque séparé des fichiers de données.

### Configuration de tempdb

Tempdb est crucial pour les performances. Configurez-le correctement :

**Nombre de fichiers** : Créez autant de fichiers tempdb que de cœurs CPU (maximum 8 pour la plupart des cas).

**Taille initiale** : Définissez une taille initiale appropriée pour éviter les croissances fréquentes.

```sql
-- Exemple de configuration tempdb
ALTER DATABASE tempdb MODIFY FILE (NAME = 'tempdev', SIZE = 1024MB, FILEGROWTH = 256MB);
ALTER DATABASE tempdb ADD FILE (NAME = 'tempdev2', FILENAME = 'C:\TempDB\tempdev2.mdf', SIZE = 1024MB, FILEGROWTH = 256MB);
```

### Configuration des options de base de données

**Niveau de compatibilité** : Utilisez le niveau de compatibilité le plus récent pour bénéficier des optimisations.

**Options de récupération** : Pour la production, utilisez le mode de récupération FULL pour permettre les sauvegardes de logs.

**Auto-statistiques** : Activez la mise à jour automatique des statistiques pour maintenir les performances des requêtes.

## Création et configuration de la base de données

La création de la base de données AdeauMao doit suivre les meilleures pratiques pour assurer performances et maintenabilité.

### Création manuelle de la base de données

Si vous préférez créer la base de données manuellement avant d'exécuter les migrations :

```sql
-- Création de la base de données AdeauMao
CREATE DATABASE AdeauMaoDB
ON 
( NAME = 'AdeauMaoDB_Data',
  FILENAME = 'C:\Database\AdeauMaoDB_Data.mdf',
  SIZE = 500MB,
  MAXSIZE = 10GB,
  FILEGROWTH = 100MB )
LOG ON 
( NAME = 'AdeauMaoDB_Log',
  FILENAME = 'C:\Database\AdeauMaoDB_Log.ldf',
  SIZE = 100MB,
  MAXSIZE = 2GB,
  FILEGROWTH = 50MB );

-- Configuration des options de base de données
ALTER DATABASE AdeauMaoDB SET COMPATIBILITY_LEVEL = 160; -- SQL Server 2022
ALTER DATABASE AdeauMaoDB SET RECOVERY FULL;
ALTER DATABASE AdeauMaoDB SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE AdeauMaoDB SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE AdeauMaoDB SET AUTO_UPDATE_STATISTICS_ASYNC ON;
```

### Configuration des collations

AdeauMao utilise la collation French_CI_AS pour supporter correctement les caractères français :

```sql
ALTER DATABASE AdeauMaoDB COLLATE French_CI_AS;
```

### Configuration des paramètres de performance

```sql
-- Optimisations pour AdeauMao
ALTER DATABASE AdeauMaoDB SET READ_COMMITTED_SNAPSHOT ON;
ALTER DATABASE AdeauMaoDB SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE AdeauMaoDB SET PARAMETERIZATION FORCED;
```

### Création d'un utilisateur dédié

Créez un utilisateur SQL Server dédié pour AdeauMao plutôt qu'utiliser "sa" :

```sql
-- Création du login
CREATE LOGIN AdeauMaoUser WITH PASSWORD = 'VotreMotDePasseSecurise123!';

-- Utilisation de la base de données
USE AdeauMaoDB;

-- Création de l'utilisateur
CREATE USER AdeauMaoUser FOR LOGIN AdeauMaoUser;

-- Attribution des permissions
ALTER ROLE db_owner ADD MEMBER AdeauMaoUser;
```

## Exécution des migrations Entity Framework

Entity Framework Core gère automatiquement la structure de la base de données via les migrations.

### Configuration de la chaîne de connexion

Modifiez le fichier `appsettings.json` pour pointer vers votre instance SQL Server :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AdeauMaoDB;User Id=AdeauMaoUser;Password=VotreMotDePasseSecurise123!;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

Pour l'authentification Windows :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AdeauMaoDB;Integrated Security=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Vérification des migrations

Avant d'exécuter les migrations, vérifiez qu'elles sont présentes :

```bash
# Depuis le répertoire racine du projet
dotnet ef migrations list --project AdeauMao.Infrastructure --startup-project AdeauMao.API
```

### Exécution des migrations

Exécutez les migrations pour créer la structure de base de données :

```bash
# Création de la base de données et application des migrations
dotnet ef database update --project AdeauMao.Infrastructure --startup-project AdeauMao.API
```

### Vérification de la structure

Après l'exécution des migrations, vérifiez que toutes les tables ont été créées :

```sql
-- Lister toutes les tables
SELECT TABLE_SCHEMA, TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Vérifier les contraintes de clés étrangères
SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM sys.foreign_keys fk
INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
ORDER BY tp.name, cp.name;
```

### Gestion des erreurs de migration

**Erreur de connexion** : Vérifiez la chaîne de connexion et que SQL Server est démarré.

**Erreur de permissions** : Assurez-vous que l'utilisateur a les permissions db_owner ou au minimum db_ddladmin.

**Erreur de migration existante** : Si une migration a échoué partiellement, vous devrez peut-être nettoyer manuellement :

```sql
-- Supprimer la table de migrations si nécessaire
DROP TABLE IF EXISTS __EFMigrationsHistory;
```

Puis relancer la migration.

## Données de seed et initialisation

AdeauMao inclut un système de données de seed pour initialiser la base de données avec les données essentielles.

### Données de seed automatiques

Le système initialise automatiquement :

**Rôles utilisateurs** :
- Administrator : Accès complet au système
- Manager : Gestion des équipements et validation des OT
- Technician : Exécution des ordres de travail
- Operator : Consultation et demandes d'intervention

**Utilisateur administrateur par défaut** :
- Nom d'utilisateur : admin
- Mot de passe : Admin123!
- Rôle : Administrator

**Catégories de base** :
- Types d'équipements (Compresseur, Pompe, Moteur, etc.)
- Causes de pannes courantes
- Priorités standard

### Personnalisation des données de seed

Vous pouvez personnaliser les données de seed en modifiant le fichier `SeedData.cs` :

```csharp
// Exemple d'ajout de données personnalisées
public static async Task SeedCustomData(ApplicationDbContext context)
{
    // Ajouter des sites spécifiques à votre organisation
    if (!context.Sites.Any())
    {
        var sites = new List<Site>
        {
            new Site { NomSite = "Usine Principale", Adresse = "Zone Industrielle, Casablanca" },
            new Site { NomSite = "Atelier Maintenance", Adresse = "Rue de l'Industrie, Rabat" }
        };
        
        context.Sites.AddRange(sites);
        await context.SaveChangesAsync();
    }
    
    // Ajouter des lignes de production
    if (!context.LignesProduction.Any())
    {
        var site = context.Sites.First();
        var lignes = new List<LigneProduction>
        {
            new LigneProduction { NomLigne = "Ligne A", SiteId = site.Id },
            new LigneProduction { NomLigne = "Ligne B", SiteId = site.Id }
        };
        
        context.LignesProduction.AddRange(lignes);
        await context.SaveChangesAsync();
    }
}
```

### Vérification des données de seed

Après l'initialisation, vérifiez que les données de seed ont été correctement insérées :

```sql
-- Vérifier les rôles
SELECT * FROM AspNetRoles;

-- Vérifier l'utilisateur admin
SELECT u.UserName, u.Email, r.Name as Role
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName = 'admin';

-- Vérifier les catégories
SELECT * FROM Categories;

-- Vérifier les sites et lignes de production
SELECT s.NomSite, s.Adresse, COUNT(lp.Id) as NombreLignes
FROM Sites s
LEFT JOIN LignesProduction lp ON s.Id = lp.SiteId
GROUP BY s.Id, s.NomSite, s.Adresse;
```

## Configuration des performances

Une configuration optimale des performances est essentielle pour une utilisation fluide d'AdeauMao.

### Index recommandés

En plus des index créés automatiquement par Entity Framework, ajoutez ces index pour optimiser les requêtes courantes :

```sql
-- Index pour les recherches d'équipements
CREATE NONCLUSTERED INDEX IX_Equipements_Search 
ON Equipements (Nom, Reference, TypeEquipement)
INCLUDE (Localisation, EtatOperationnel);

-- Index pour les ordres de travail par statut et date
CREATE NONCLUSTERED INDEX IX_OrdresDeTravail_Statut_Date 
ON OrdresDeTravail (Statut, DateCreation)
INCLUDE (EquipementId, TechnicienAssigneeId, Priorite);

-- Index pour les demandes d'intervention par équipement
CREATE NONCLUSTERED INDEX IX_DemandesIntervention_Equipement 
ON DemandesIntervention (EquipementId, Statut)
INCLUDE (DateDemande, Priorite);

-- Index pour les employés par équipe
CREATE NONCLUSTERED INDEX IX_EquipeMembres_Equipe 
ON EquipeMembres (EquipeId)
INCLUDE (EmployeId);
```

### Configuration des statistiques

Configurez la mise à jour automatique des statistiques :

```sql
-- Mise à jour des statistiques pour toutes les tables
EXEC sp_updatestats;

-- Configuration de la mise à jour automatique
ALTER DATABASE AdeauMaoDB SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE AdeauMaoDB SET AUTO_UPDATE_STATISTICS_ASYNC ON;
```

### Optimisation des requêtes

**Plan de requête** : Utilisez le plan d'exécution dans SSMS pour identifier les requêtes coûteuses.

**Index manquants** : Surveillez les recommandations d'index manquants dans SSMS.

**Statistiques d'attente** : Analysez les statistiques d'attente pour identifier les goulots d'étranglement.

### Configuration de la maintenance

Créez des tâches de maintenance automatiques :

```sql
-- Réorganisation des index
ALTER INDEX ALL ON Equipements REORGANIZE;

-- Mise à jour des statistiques
UPDATE STATISTICS Equipements;

-- Nettoyage des logs
BACKUP LOG AdeauMaoDB TO DISK = 'NUL';
```

## Sécurité et permissions

La sécurité de la base de données est cruciale pour protéger les données sensibles d'AdeauMao.

### Principe du moindre privilège

Créez des utilisateurs avec les permissions minimales nécessaires :

```sql
-- Utilisateur pour l'application (lecture/écriture)
CREATE LOGIN AdeauMaoApp WITH PASSWORD = 'MotDePasseApplication123!';
USE AdeauMaoDB;
CREATE USER AdeauMaoApp FOR LOGIN AdeauMaoApp;
ALTER ROLE db_datareader ADD MEMBER AdeauMaoApp;
ALTER ROLE db_datawriter ADD MEMBER AdeauMaoApp;

-- Utilisateur pour les rapports (lecture seule)
CREATE LOGIN AdeauMaoReports WITH PASSWORD = 'MotDePasseRapports123!';
CREATE USER AdeauMaoReports FOR LOGIN AdeauMaoReports;
ALTER ROLE db_datareader ADD MEMBER AdeauMaoReports;

-- Utilisateur pour la maintenance (admin)
CREATE LOGIN AdeauMaoMaintenance WITH PASSWORD = 'MotDePasseMaintenance123!';
CREATE USER AdeauMaoMaintenance FOR LOGIN AdeauMaoMaintenance;
ALTER ROLE db_owner ADD MEMBER AdeauMaoMaintenance;
```

### Chiffrement des données

**Chiffrement en transit** : Activez SSL/TLS pour les connexions :

```sql
-- Forcer le chiffrement SSL
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'force encryption', 1;
RECONFIGURE;
```

**Chiffrement au repos** : Pour SQL Server Standard/Enterprise, activez Transparent Data Encryption (TDE) :

```sql
-- Créer la clé maître de base de données
USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'CléMaîtreSecurisée123!';

-- Créer le certificat
CREATE CERTIFICATE AdeauMaoTDECert WITH SUBJECT = 'AdeauMao TDE Certificate';

-- Utiliser la base de données
USE AdeauMaoDB;

-- Créer la clé de chiffrement
CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE AdeauMaoTDECert;

-- Activer le chiffrement
ALTER DATABASE AdeauMaoDB SET ENCRYPTION ON;
```

### Audit et surveillance

Configurez l'audit pour tracer les accès sensibles :

```sql
-- Créer un audit
USE master;
CREATE SERVER AUDIT AdeauMaoAudit
TO FILE (FILEPATH = 'C:\Audit\AdeauMaoAudit.sqlaudit');

-- Activer l'audit
ALTER SERVER AUDIT AdeauMaoAudit WITH (STATE = ON);

-- Créer une spécification d'audit pour la base de données
USE AdeauMaoDB;
CREATE DATABASE AUDIT SPECIFICATION AdeauMaoDBSpec
FOR SERVER AUDIT AdeauMaoAudit
ADD (SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo BY public);

-- Activer la spécification
ALTER DATABASE AUDIT SPECIFICATION AdeauMaoDBSpec WITH (STATE = ON);
```

## Sauvegarde et restauration

Une stratégie de sauvegarde robuste est essentielle pour protéger les données d'AdeauMao.

### Stratégie de sauvegarde

**Sauvegarde complète** : Quotidienne pendant les heures creuses.

**Sauvegarde différentielle** : Toutes les 6 heures.

**Sauvegarde des logs** : Toutes les 15 minutes pour minimiser la perte de données.

### Scripts de sauvegarde

```sql
-- Sauvegarde complète
BACKUP DATABASE AdeauMaoDB 
TO DISK = 'C:\Backup\AdeauMaoDB_Full_' + CONVERT(VARCHAR, GETDATE(), 112) + '.bak'
WITH COMPRESSION, CHECKSUM, INIT;

-- Sauvegarde différentielle
BACKUP DATABASE AdeauMaoDB 
TO DISK = 'C:\Backup\AdeauMaoDB_Diff_' + CONVERT(VARCHAR, GETDATE(), 112) + '_' + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + '.bak'
WITH DIFFERENTIAL, COMPRESSION, CHECKSUM, INIT;

-- Sauvegarde des logs
BACKUP LOG AdeauMaoDB 
TO DISK = 'C:\Backup\AdeauMaoDB_Log_' + CONVERT(VARCHAR, GETDATE(), 112) + '_' + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), ':', '') + '.trn'
WITH COMPRESSION, CHECKSUM, INIT;
```

### Automatisation avec SQL Server Agent

Créez des tâches SQL Server Agent pour automatiser les sauvegardes :

```sql
-- Tâche de sauvegarde complète (quotidienne à 2h00)
EXEC msdb.dbo.sp_add_job
    @job_name = 'AdeauMao - Sauvegarde Complète';

EXEC msdb.dbo.sp_add_jobstep
    @job_name = 'AdeauMao - Sauvegarde Complète',
    @step_name = 'Sauvegarde',
    @command = 'BACKUP DATABASE AdeauMaoDB TO DISK = ''C:\Backup\AdeauMaoDB_Full_'' + CONVERT(VARCHAR, GETDATE(), 112) + ''.bak'' WITH COMPRESSION, CHECKSUM, INIT;';

EXEC msdb.dbo.sp_add_schedule
    @schedule_name = 'Quotidien 2h00',
    @freq_type = 4,
    @freq_interval = 1,
    @active_start_time = 020000;

EXEC msdb.dbo.sp_attach_schedule
    @job_name = 'AdeauMao - Sauvegarde Complète',
    @schedule_name = 'Quotidien 2h00';
```

### Procédure de restauration

**Restauration complète** :
```sql
-- Arrêter les connexions actives
ALTER DATABASE AdeauMaoDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

-- Restaurer la sauvegarde
RESTORE DATABASE AdeauMaoDB 
FROM DISK = 'C:\Backup\AdeauMaoDB_Full_20240115.bak'
WITH REPLACE, CHECKSUM;

-- Remettre en mode multi-utilisateur
ALTER DATABASE AdeauMaoDB SET MULTI_USER;
```

**Restauration point-in-time** :
```sql
-- Restaurer la sauvegarde complète
RESTORE DATABASE AdeauMaoDB 
FROM DISK = 'C:\Backup\AdeauMaoDB_Full_20240115.bak'
WITH REPLACE, NORECOVERY, CHECKSUM;

-- Restaurer la sauvegarde différentielle
RESTORE DATABASE AdeauMaoDB 
FROM DISK = 'C:\Backup\AdeauMaoDB_Diff_20240115_120000.bak'
WITH NORECOVERY, CHECKSUM;

-- Restaurer les logs jusqu'au point souhaité
RESTORE LOG AdeauMaoDB 
FROM DISK = 'C:\Backup\AdeauMaoDB_Log_20240115_143000.trn'
WITH STOPAT = '2024-01-15 14:25:00', CHECKSUM;
```

## Monitoring et maintenance

Un monitoring proactif permet de maintenir les performances optimales de la base de données.

### Surveillance des performances

**Requêtes lentes** : Identifiez les requêtes consommant le plus de ressources :

```sql
-- Top 10 des requêtes les plus coûteuses
SELECT TOP 10
    qs.total_elapsed_time / qs.execution_count AS avg_elapsed_time,
    qs.total_logical_reads / qs.execution_count AS avg_logical_reads,
    qs.execution_count,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2)+1) AS statement_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
ORDER BY avg_elapsed_time DESC;
```

**Utilisation de l'espace** : Surveillez la croissance de la base de données :

```sql
-- Taille des tables
SELECT 
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB,
    SUM(a.used_pages) * 8 AS UsedSpaceKB,
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
FROM sys.tables t
INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE t.NAME NOT LIKE 'dt%' AND t.is_ms_shipped = 0 AND i.OBJECT_ID > 255
GROUP BY t.Name, s.Name, p.Rows
ORDER BY TotalSpaceKB DESC;
```

### Maintenance automatique

**Réorganisation des index** : Planifiez la maintenance des index :

```sql
-- Script de maintenance des index
DECLARE @TableName NVARCHAR(255);
DECLARE @IndexName NVARCHAR(255);
DECLARE @Fragmentation FLOAT;

DECLARE index_cursor CURSOR FOR
SELECT 
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 10
AND i.name IS NOT NULL;

OPEN index_cursor;
FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @Fragmentation > 30
        EXEC('ALTER INDEX ' + @IndexName + ' ON ' + @TableName + ' REBUILD');
    ELSE
        EXEC('ALTER INDEX ' + @IndexName + ' ON ' + @TableName + ' REORGANIZE');
    
    FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;
END;

CLOSE index_cursor;
DEALLOCATE index_cursor;
```

### Alertes et notifications

Configurez des alertes pour les événements critiques :

```sql
-- Alerte pour l'espace disque faible
EXEC msdb.dbo.sp_add_alert
    @name = 'AdeauMao - Espace disque faible',
    @message_id = 3201,
    @severity = 0,
    @notification_message = 'Espace disque insuffisant pour la base de données AdeauMao';

-- Alerte pour les échecs de sauvegarde
EXEC msdb.dbo.sp_add_alert
    @name = 'AdeauMao - Échec sauvegarde',
    @message_id = 3201,
    @severity = 0,
    @notification_message = 'Échec de la sauvegarde de la base de données AdeauMao';
```

Cette configuration complète de la base de données vous permettra de déployer AdeauMao avec une base solide, sécurisée et performante. N'hésitez pas à adapter ces configurations selon vos besoins spécifiques et votre environnement de production.


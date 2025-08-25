# Guide d'Installation Complet - AdeauMao

Ce guide vous accompagne pas à pas dans l'installation complète du système AdeauMao, depuis la préparation de l'environnement jusqu'au déploiement en production. Suivez attentivement chaque étape pour garantir une installation réussie.

## Table des Matières

1. [Vue d'ensemble de l'installation](#vue-densemble-de-linstallation)
2. [Prérequis système](#prérequis-système)
3. [Installation des composants](#installation-des-composants)
4. [Configuration de l'environnement](#configuration-de-lenvironnement)
5. [Déploiement de l'application](#déploiement-de-lapplication)
6. [Configuration post-installation](#configuration-post-installation)
7. [Tests et validation](#tests-et-validation)
8. [Dépannage](#dépannage)

## Vue d'ensemble de l'installation

AdeauMao est une application web moderne basée sur ASP.NET Core 8 qui nécessite plusieurs composants pour fonctionner correctement. L'installation comprend la configuration du serveur web, de la base de données, et du système d'authentification.

### Architecture de déploiement

**Environnement de développement** : Installation locale sur un poste de développeur avec SQL Server Express et IIS Express.

**Environnement de test** : Serveur dédié avec SQL Server Standard et IIS, répliquant l'environnement de production.

**Environnement de production** : Infrastructure robuste avec haute disponibilité, sauvegarde automatique, et monitoring.

### Composants principaux

- **ASP.NET Core 8 Runtime** : Moteur d'exécution de l'application
- **SQL Server** : Base de données relationnelle
- **IIS** : Serveur web (production) ou IIS Express (développement)
- **Entity Framework Core** : ORM pour l'accès aux données
- **JWT Authentication** : Système d'authentification sécurisé

## Prérequis système

Avant de commencer l'installation, vérifiez que votre environnement répond aux exigences minimales.

### Configuration matérielle

**Environnement de développement** :
- Processeur : Intel Core i5 ou AMD Ryzen 5 (4 cœurs minimum)
- Mémoire : 8 GB RAM minimum, 16 GB recommandés
- Stockage : 20 GB d'espace libre sur SSD
- Réseau : Connexion Internet pour les téléchargements

**Environnement de production** :
- Processeur : Intel Xeon ou AMD EPYC (8 cœurs minimum)
- Mémoire : 16 GB RAM minimum, 32 GB recommandés
- Stockage : 100 GB d'espace libre sur SSD, RAID recommandé
- Réseau : Connexion réseau stable et sécurisée

### Système d'exploitation

**Développement** :
- Windows 10 version 1909 ou supérieur
- Windows 11 (recommandé)
- macOS 10.15 ou supérieur (avec Docker)
- Ubuntu 18.04 LTS ou supérieur (avec Docker)

**Production** :
- Windows Server 2019 ou supérieur (recommandé)
- Windows Server 2016 (minimum)
- Ubuntu Server 20.04 LTS ou supérieur
- Red Hat Enterprise Linux 8 ou supérieur

### Logiciels prérequis

Téléchargez et préparez les installateurs suivants :

**.NET 8 SDK** : Version la plus récente depuis [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0)

**SQL Server** : 
- SQL Server 2019 Express (développement)
- SQL Server 2019 Standard/Enterprise (production)
- SQL Server Management Studio (SSMS)

**Visual Studio** (développement) :
- Visual Studio 2022 Community/Professional/Enterprise
- Visual Studio Code (alternative légère)

**Git** : Pour la gestion de version et le déploiement

**IIS** (production Windows) : Activé via les fonctionnalités Windows

## Installation des composants

Procédez à l'installation des composants dans l'ordre recommandé pour éviter les conflits de dépendances.

### Installation de .NET 8 SDK

**.NET 8 SDK** est le composant fondamental pour exécuter AdeauMao.

**Windows** :
1. Téléchargez l'installateur depuis le site officiel Microsoft
2. Exécutez l'installateur en tant qu'administrateur
3. Suivez l'assistant d'installation avec les options par défaut
4. Redémarrez votre ordinateur si demandé

**Vérification de l'installation** :
```bash
dotnet --version
dotnet --list-sdks
dotnet --list-runtimes
```

Vous devriez voir .NET 8.0.x dans la liste des SDK et runtimes installés.

**Linux (Ubuntu)** :
```bash
# Ajouter le repository Microsoft
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# Installer .NET 8 SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# Vérifier l'installation
dotnet --version
```

### Installation de SQL Server

**SQL Server Express (Développement)** :

1. Téléchargez SQL Server Express depuis le site Microsoft
2. Lancez l'installateur et choisissez "Installation personnalisée"
3. Sélectionnez les fonctionnalités :
   - Services de moteur de base de données
   - Outils de gestion - Complets
   - Connectivité des outils clients

4. Configuration de l'instance :
   - Instance par défaut (MSSQLSERVER)
   - Mode d'authentification mixte
   - Mot de passe fort pour le compte 'sa'

5. Configuration des services :
   - Démarrage automatique pour SQL Server
   - Compte de service par défaut

**SQL Server Standard/Enterprise (Production)** :

Suivez la même procédure mais avec des considérations supplémentaires :

- Planifiez l'emplacement des fichiers de données et de logs
- Configurez la mémoire maximale du serveur
- Activez les fonctionnalités de haute disponibilité si nécessaire
- Configurez les sauvegardes automatiques

**Installation de SQL Server Management Studio** :

SSMS est maintenant un téléchargement séparé :
1. Téléchargez SSMS depuis le site Microsoft
2. Installez avec les options par défaut
3. Testez la connexion à votre instance SQL Server

### Installation d'IIS (Windows Production)

Pour les déploiements de production sur Windows Server :

**Activation d'IIS** :
1. Ouvrez "Gestionnaire de serveur"
2. Cliquez sur "Ajouter des rôles et fonctionnalités"
3. Sélectionnez "Services de rôle Web Server (IIS)"
4. Ajoutez les fonctionnalités :
   - Fonctionnalités HTTP communes
   - Développement d'applications (ASP.NET Core)
   - Sécurité (Authentification Windows, Filtrage des demandes)
   - Outils de gestion (Console de gestion IIS)

**Installation du module ASP.NET Core** :
1. Téléchargez le "ASP.NET Core Runtime" avec "Hosting Bundle"
2. Installez le bundle qui inclut le module IIS
3. Redémarrez IIS : `iisreset`

**Vérification** :
```cmd
# Vérifier que le module ASP.NET Core est installé
%windir%\system32\inetsrv\appcmd.exe list modules | findstr AspNetCore
```

### Installation de Git

**Windows** :
1. Téléchargez Git for Windows depuis git-scm.com
2. Installez avec les options recommandées
3. Configurez votre identité :
```bash
git config --global user.name "Votre Nom"
git config --global user.email "votre.email@exemple.com"
```

**Linux** :
```bash
# Ubuntu/Debian
sudo apt-get install git

# CentOS/RHEL
sudo yum install git

# Configuration
git config --global user.name "Votre Nom"
git config --global user.email "votre.email@exemple.com"
```

## Configuration de l'environnement

Une fois tous les composants installés, configurez l'environnement pour AdeauMao.

### Clonage du repository

Clonez le code source d'AdeauMao depuis votre repository :

```bash
# Cloner le repository
git clone https://github.com/votre-organisation/adeaumao.git
cd adeaumao

# Vérifier la structure du projet
ls -la
```

Vous devriez voir la structure suivante :
```
adeaumao/
├── AdeauMao.sln
├── AdeauMao.API/
├── AdeauMao.Application/
├── AdeauMao.Infrastructure/
├── AdeauMao.Core/
├── docs/
└── README.md
```

### Configuration de la base de données

**Création de la base de données** :

Connectez-vous à SQL Server avec SSMS et exécutez :

```sql
-- Créer la base de données
CREATE DATABASE AdeauMaoDB
ON 
( NAME = 'AdeauMaoDB_Data',
  FILENAME = 'C:\Database\AdeauMaoDB_Data.mdf',
  SIZE = 500MB,
  FILEGROWTH = 100MB )
LOG ON 
( NAME = 'AdeauMaoDB_Log',
  FILENAME = 'C:\Database\AdeauMaoDB_Log.ldf',
  SIZE = 100MB,
  FILEGROWTH = 50MB );

-- Créer un utilisateur dédié
CREATE LOGIN AdeauMaoUser WITH PASSWORD = 'VotreMotDePasseSecurise123!';
USE AdeauMaoDB;
CREATE USER AdeauMaoUser FOR LOGIN AdeauMaoUser;
ALTER ROLE db_owner ADD MEMBER AdeauMaoUser;
```

**Configuration de la chaîne de connexion** :

Modifiez le fichier `appsettings.json` dans le projet AdeauMao.API :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AdeauMaoDB;User Id=AdeauMaoUser;Password=VotreMotDePasseSecurise123!;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "VotreCléSecrèteJWTTrèsLongueEtSécurisée123456789!",
    "Issuer": "AdeauMaoAPI",
    "Audience": "AdeauMaoClients",
    "TokenExpirationInHours": 24,
    "RefreshTokenExpirationInDays": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Restauration des packages NuGet

Restaurez tous les packages NuGet nécessaires :

```bash
# Depuis le répertoire racine du projet
dotnet restore

# Vérifier que tous les packages sont restaurés
dotnet list package
```

### Exécution des migrations

Créez et appliquez les migrations Entity Framework :

```bash
# Vérifier les migrations existantes
dotnet ef migrations list --project AdeauMao.Infrastructure --startup-project AdeauMao.API

# Appliquer les migrations
dotnet ef database update --project AdeauMao.Infrastructure --startup-project AdeauMao.API
```

Si les migrations n'existent pas encore, créez-les :

```bash
# Créer la migration initiale
dotnet ef migrations add InitialCreate --project AdeauMao.Infrastructure --startup-project AdeauMao.API

# Appliquer la migration
dotnet ef database update --project AdeauMao.Infrastructure --startup-project AdeauMao.API
```

### Configuration des variables d'environnement

**Développement** :

Créez un fichier `appsettings.Development.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AdeauMaoDB_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "JwtSettings": {
    "Secret": "CléDeTestPourLeDéveloppement123456789!"
  }
}
```

**Production** :

Configurez les variables d'environnement système :

```bash
# Windows
setx ASPNETCORE_ENVIRONMENT "Production"
setx ConnectionStrings__DefaultConnection "Server=prod-server;Database=AdeauMaoDB;User Id=AdeauMaoUser;Password=MotDePasseProduction;"

# Linux
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=AdeauMaoDB;User Id=AdeauMaoUser;Password=MotDePasseProduction;"
```

## Déploiement de l'application

Le processus de déploiement varie selon l'environnement cible.

### Déploiement en développement

**Test local avec IIS Express** :

```bash
# Depuis le répertoire AdeauMao.API
dotnet run

# Ou avec un profil spécifique
dotnet run --launch-profile "AdeauMao.API"
```

L'application sera accessible à :
- HTTPS : https://localhost:5001
- HTTP : http://localhost:5000
- Swagger : https://localhost:5001/api-docs

**Test avec IIS local** :

1. Publiez l'application :
```bash
dotnet publish -c Release -o ./publish
```

2. Créez un site IIS :
   - Ouvrez IIS Manager
   - Clic droit sur "Sites" > "Add Website"
   - Nom : AdeauMao
   - Chemin physique : vers le dossier publish
   - Port : 8080

3. Configurez le pool d'applications :
   - Version .NET CLR : "No Managed Code"
   - Mode pipeline : Intégré

### Déploiement en production

**Préparation de la publication** :

```bash
# Publication optimisée pour la production
dotnet publish -c Release -o ./publish --self-contained false --runtime win-x64

# Ou pour Linux
dotnet publish -c Release -o ./publish --self-contained false --runtime linux-x64
```

**Déploiement sur Windows Server avec IIS** :

1. Copiez les fichiers publiés vers le serveur :
```cmd
xcopy /E /I .\publish \\serveur-prod\c$\inetpub\adeaumao\
```

2. Créez le site IIS :
```cmd
# Via PowerShell sur le serveur
Import-Module WebAdministration

# Créer le pool d'applications
New-WebAppPool -Name "AdeauMaoPool"
Set-ItemProperty -Path "IIS:\AppPools\AdeauMaoPool" -Name "processModel.identityType" -Value "ApplicationPoolIdentity"
Set-ItemProperty -Path "IIS:\AppPools\AdeauMaoPool" -Name "managedRuntimeVersion" -Value ""

# Créer le site web
New-Website -Name "AdeauMao" -Port 80 -PhysicalPath "C:\inetpub\adeaumao" -ApplicationPool "AdeauMaoPool"
```

3. Configurez HTTPS :
```cmd
# Lier un certificat SSL
New-WebBinding -Name "AdeauMao" -Protocol "https" -Port 443 -SslFlags 1
```

**Déploiement sur Linux avec Nginx** :

1. Installez les prérequis :
```bash
# Ubuntu
sudo apt-get update
sudo apt-get install nginx

# Installer .NET 8 Runtime
sudo apt-get install aspnetcore-runtime-8.0
```

2. Copiez les fichiers :
```bash
sudo mkdir -p /var/www/adeaumao
sudo cp -r ./publish/* /var/www/adeaumao/
sudo chown -R www-data:www-data /var/www/adeaumao
```

3. Configurez Nginx :
```nginx
# /etc/nginx/sites-available/adeaumao
server {
    listen 80;
    server_name votre-domaine.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

4. Activez le site :
```bash
sudo ln -s /etc/nginx/sites-available/adeaumao /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

5. Créez un service systemd :
```ini
# /etc/systemd/system/adeaumao.service
[Unit]
Description=AdeauMao GMAO Application
After=network.target

[Service]
Type=notify
ExecStart=/usr/bin/dotnet /var/www/adeaumao/AdeauMao.API.dll
Restart=always
RestartSec=5
KillSignal=SIGINT
SyslogIdentifier=adeaumao
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
WorkingDirectory=/var/www/adeaumao

[Install]
WantedBy=multi-user.target
```

6. Démarrez le service :
```bash
sudo systemctl enable adeaumao.service
sudo systemctl start adeaumao.service
sudo systemctl status adeaumao.service
```

## Configuration post-installation

Après le déploiement, effectuez ces configurations essentielles.

### Configuration de la sécurité

**Certificats SSL** :

Pour la production, configurez un certificat SSL valide :

```bash
# Avec Let's Encrypt (Linux)
sudo apt-get install certbot python3-certbot-nginx
sudo certbot --nginx -d votre-domaine.com
```

**Pare-feu** :

Configurez le pare-feu pour autoriser uniquement les ports nécessaires :

```bash
# Windows
netsh advfirewall firewall add rule name="AdeauMao HTTP" dir=in action=allow protocol=TCP localport=80
netsh advfirewall firewall add rule name="AdeauMao HTTPS" dir=in action=allow protocol=TCP localport=443

# Linux (UFW)
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw enable
```

### Configuration des logs

**Répertoire des logs** :

Créez un répertoire dédié pour les logs :

```bash
# Windows
mkdir C:\Logs\AdeauMao
icacls C:\Logs\AdeauMao /grant "IIS_IUSRS:(OI)(CI)F"

# Linux
sudo mkdir -p /var/log/adeaumao
sudo chown www-data:www-data /var/log/adeaumao
```

**Configuration Serilog** :

Modifiez `appsettings.Production.json` :

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "/var/log/adeaumao/adeaumao-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "formatter": "Serilog.Formatting.Json.JsonFormatter"
        }
      }
    ]
  }
}
```

### Configuration de la surveillance

**Health Checks** :

Configurez un monitoring externe pour vérifier la santé de l'application :

```bash
# Test du endpoint de santé
curl -f https://votre-domaine.com/health || echo "Application down"
```

**Monitoring des performances** :

Installez des outils de monitoring comme :
- Application Insights (Azure)
- Prometheus + Grafana
- ELK Stack (Elasticsearch, Logstash, Kibana)

### Sauvegarde automatique

Configurez des sauvegardes automatiques :

**Base de données** :

```sql
-- Script de sauvegarde automatique
BACKUP DATABASE AdeauMaoDB 
TO DISK = 'C:\Backup\AdeauMaoDB_' + CONVERT(VARCHAR, GETDATE(), 112) + '.bak'
WITH COMPRESSION, CHECKSUM;
```

**Fichiers application** :

```bash
# Script de sauvegarde des fichiers (Linux)
#!/bin/bash
tar -czf /backup/adeaumao-$(date +%Y%m%d).tar.gz /var/www/adeaumao
find /backup -name "adeaumao-*.tar.gz" -mtime +30 -delete
```

## Tests et validation

Validez votre installation avec ces tests essentiels.

### Tests fonctionnels

**Test de connectivité** :

```bash
# Test de base
curl -k https://localhost:5001/health

# Test avec authentification
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"admin","password":"Admin123!"}'
```

**Test des API** :

Utilisez Postman ou curl pour tester les endpoints principaux :

```bash
# Obtenir un token
TOKEN=$(curl -s -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"admin","password":"Admin123!"}' | \
  jq -r '.data.token')

# Tester l'API des équipements
curl -H "Authorization: Bearer $TOKEN" \
  https://localhost:5001/api/equipements
```

### Tests de performance

**Test de charge** :

Utilisez des outils comme Apache Bench ou k6 :

```bash
# Test simple avec Apache Bench
ab -n 1000 -c 10 https://localhost:5001/api/equipements

# Test avec k6
k6 run --vus 10 --duration 30s test-script.js
```

**Monitoring des ressources** :

Surveillez l'utilisation des ressources pendant les tests :

```bash
# Windows
perfmon

# Linux
htop
iostat -x 1
```

### Tests de sécurité

**Test des certificats SSL** :

```bash
# Vérifier le certificat SSL
openssl s_client -connect votre-domaine.com:443 -servername votre-domaine.com
```

**Test des headers de sécurité** :

```bash
# Vérifier les headers de sécurité
curl -I https://votre-domaine.com
```

## Dépannage

Solutions aux problèmes courants rencontrés lors de l'installation.

### Problèmes de base de données

**Erreur de connexion SQL Server** :

1. Vérifiez que SQL Server est démarré :
```cmd
net start MSSQLSERVER
```

2. Testez la connectivité :
```cmd
sqlcmd -S localhost -U AdeauMaoUser -P VotreMotDePasse
```

3. Vérifiez la chaîne de connexion dans `appsettings.json`

**Erreur de migration** :

```bash
# Réinitialiser les migrations
dotnet ef database drop --project AdeauMao.Infrastructure --startup-project AdeauMao.API
dotnet ef migrations remove --project AdeauMao.Infrastructure --startup-project AdeauMao.API
dotnet ef migrations add InitialCreate --project AdeauMao.Infrastructure --startup-project AdeauMao.API
dotnet ef database update --project AdeauMao.Infrastructure --startup-project AdeauMao.API
```

### Problèmes de déploiement

**Erreur 500.30 - ASP.NET Core app failed to start** :

1. Vérifiez les logs dans l'Event Viewer
2. Activez les logs détaillés :
```xml
<!-- web.config -->
<aspNetCore processPath="dotnet" arguments=".\AdeauMao.API.dll" stdoutLogEnabled="true" stdoutLogFile=".\logs\stdout" />
```

3. Vérifiez que le runtime ASP.NET Core est installé

**Problème de permissions** :

```cmd
# Windows - Donner les permissions IIS
icacls "C:\inetpub\adeaumao" /grant "IIS_IUSRS:(OI)(CI)F"

# Linux - Corriger les permissions
sudo chown -R www-data:www-data /var/www/adeaumao
sudo chmod -R 755 /var/www/adeaumao
```

### Problèmes de performance

**Application lente** :

1. Vérifiez les logs pour identifier les requêtes lentes
2. Optimisez la base de données :
```sql
-- Mettre à jour les statistiques
UPDATE STATISTICS;

-- Réorganiser les index
ALTER INDEX ALL ON Equipements REORGANIZE;
```

3. Augmentez la mémoire allouée à l'application

**Erreurs de timeout** :

Augmentez les timeouts dans `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AdeauMaoDB;User Id=AdeauMaoUser;Password=VotreMotDePasse;Connection Timeout=60;"
  }
}
```

Cette installation complète vous permettra de déployer AdeauMao dans différents environnements avec succès. N'hésitez pas à adapter les configurations selon vos besoins spécifiques et votre infrastructure.


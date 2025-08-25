# AdeauMao - Système de Gestion de Maintenance Assistée par Ordinateur (GMAO)

![AdeauMao Logo](https://via.placeholder.com/200x100/2c3e50/ffffff?text=AdeauMao)

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red.svg)](https://www.microsoft.com/en-us/sql-server/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📋 Table des Matières

- [Vue d'ensemble](#vue-densemble)
- [Fonctionnalités](#fonctionnalités)
- [Architecture](#architecture)
- [Technologies utilisées](#technologies-utilisées)
- [Installation rapide](#installation-rapide)
- [Documentation](#documentation)
- [API Endpoints](#api-endpoints)
- [Contribution](#contribution)
- [Support](#support)
- [Licence](#licence)

## 🎯 Vue d'ensemble

AdeauMao est un système complet de Gestion de Maintenance Assistée par Ordinateur (GMAO) développé avec ASP.NET Core 8 et Entity Framework Core. Il offre une solution robuste pour la gestion des équipements industriels, la planification de la maintenance préventive et corrective, ainsi que le suivi des interventions techniques.

Le système est conçu pour répondre aux besoins des entreprises industrielles souhaitant optimiser leurs processus de maintenance, réduire les temps d'arrêt et améliorer la productivité de leurs équipements.

## ✨ Fonctionnalités

### 🔧 Gestion des Équipements
- **Inventaire complet** : Référencement détaillé de tous les équipements avec leurs caractéristiques techniques
- **Hiérarchie d'équipements** : Organisation par sites, lignes de production et organes
- **Historique de maintenance** : Traçabilité complète des interventions
- **États opérationnels** : Suivi en temps réel du statut des équipements

### 👥 Gestion des Ressources Humaines
- **Gestion des employés** : Profils complets avec compétences et certifications
- **Équipes de travail** : Organisation en équipes spécialisées
- **Planification des interventions** : Assignation automatique basée sur les compétences
- **Suivi des performances** : Indicateurs de productivité par technicien

### 📋 Ordres de Travail
- **Création automatique** : Génération d'OT depuis les demandes d'intervention
- **Planification intelligente** : Optimisation des ressources et des délais
- **Suivi en temps réel** : Progression des travaux avec notifications
- **Validation hiérarchique** : Workflow d'approbation configurable

### 🔄 Maintenance Préventive
- **Calendriers de maintenance** : Planification basée sur le temps ou l'usage
- **Gammes opératoires** : Procédures standardisées
- **Alertes automatiques** : Notifications avant échéance
- **Optimisation des coûts** : Analyse ROI des actions préventives

### 📊 Rapports et Analyses
- **Tableaux de bord** : Indicateurs clés de performance (KPI)
- **Analyses prédictives** : Tendances et recommandations
- **Rapports personnalisés** : Génération automatique de documents
- **Exports de données** : Intégration avec outils externes

### 🔐 Sécurité et Authentification
- **Authentification JWT** : Sécurisation des API
- **Gestion des rôles** : Contrôle d'accès granulaire
- **Audit trail** : Traçabilité des actions utilisateurs
- **Chiffrement des données** : Protection des informations sensibles

## 🏗️ Architecture

Le projet suit une architecture en couches (Clean Architecture) pour assurer la maintenabilité et la testabilité :

```
AdeauMao/
├── AdeauMao.API/              # Couche de présentation (Controllers, Middleware)
├── AdeauMao.Application/      # Couche application (Services, DTOs, Interfaces)
├── AdeauMao.Infrastructure/   # Couche infrastructure (Data Access, Services externes)
└── AdeauMao.Core/            # Couche domaine (Entités, Interfaces métier)
```

### Principes architecturaux
- **Séparation des responsabilités** : Chaque couche a un rôle bien défini
- **Inversion de dépendance** : Les couches hautes ne dépendent pas des couches basses
- **Testabilité** : Architecture facilitant les tests unitaires et d'intégration
- **Extensibilité** : Facilité d'ajout de nouvelles fonctionnalités

## 🛠️ Technologies utilisées

### Backend
- **ASP.NET Core 8** : Framework web moderne et performant
- **Entity Framework Core 8** : ORM pour l'accès aux données
- **SQL Server** : Base de données relationnelle
- **AutoMapper** : Mapping automatique entre objets
- **FluentValidation** : Validation des données d'entrée
- **Serilog** : Logging structuré et performant

### Authentification et Sécurité
- **ASP.NET Core Identity** : Gestion des utilisateurs et rôles
- **JWT (JSON Web Tokens)** : Authentification stateless
- **HTTPS** : Chiffrement des communications
- **CORS** : Gestion des requêtes cross-origin

### Documentation et Tests
- **Swagger/OpenAPI** : Documentation interactive des API
- **XML Documentation** : Documentation du code
- **Health Checks** : Monitoring de l'état de l'application

### Outils de développement
- **Visual Studio 2022** : IDE recommandé
- **SQL Server Management Studio** : Gestion de base de données
- **Postman** : Tests d'API
- **Git** : Contrôle de version

## 🚀 Installation rapide

### Prérequis
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) ou [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (recommandé) ou [Visual Studio Code](https://code.visualstudio.com/)

### Installation en 5 minutes

1. **Cloner le repository**
   ```bash
   git clone https://github.com/votre-organisation/adeaumao.git
   cd adeaumao
   ```

2. **Configurer la base de données**
   ```bash
   # Modifier la chaîne de connexion dans appsettings.json
   # Puis exécuter les migrations
   dotnet ef database update --project AdeauMao.Infrastructure --startup-project AdeauMao.API
   ```

3. **Lancer l'application**
   ```bash
   cd AdeauMao.API
   dotnet run
   ```

4. **Accéder à l'API**
   - API : `https://localhost:5001`
   - Documentation Swagger : `https://localhost:5001/api-docs`
   - Health Check : `https://localhost:5001/health`

## 📚 Documentation

### Guides détaillés
- [📖 Guide d'installation complet](docs/INSTALLATION.md)
- [🔧 Configuration de Visual Studio](docs/VISUAL_STUDIO_SETUP.md)
- [🗄️ Configuration de la base de données](docs/DATABASE_SETUP.md)
- [🔐 Configuration de l'authentification](docs/AUTHENTICATION.md)
- [📊 Guide d'utilisation des API](docs/API_USAGE.md)

### Documentation technique
- [🏗️ Architecture détaillée](docs/ARCHITECTURE.md)
- [📋 Modèle de données](docs/DATA_MODEL.md)
- [🔄 Workflows métier](docs/BUSINESS_WORKFLOWS.md)
- [🧪 Guide de tests](docs/TESTING.md)
- [🚀 Déploiement](docs/DEPLOYMENT.md)

## 🌐 API Endpoints

### Authentification
```http
POST /api/auth/login          # Connexion utilisateur
POST /api/auth/register       # Inscription utilisateur
POST /api/auth/refresh-token  # Rafraîchissement du token
GET  /api/auth/me            # Informations utilisateur actuel
```

### Équipements
```http
GET    /api/equipements                    # Liste des équipements
GET    /api/equipements/{id}              # Détails d'un équipement
POST   /api/equipements                   # Créer un équipement
PUT    /api/equipements/{id}              # Modifier un équipement
DELETE /api/equipements/{id}              # Supprimer un équipement
GET    /api/equipements/{id}/organes      # Organes d'un équipement
```

### Ordres de Travail
```http
GET    /api/ordresdetravail               # Liste des ordres de travail
GET    /api/ordresdetravail/{id}          # Détails d'un ordre de travail
POST   /api/ordresdetravail              # Créer un ordre de travail
PUT    /api/ordresdetravail/{id}          # Modifier un ordre de travail
PATCH  /api/ordresdetravail/{id}/progression # Mettre à jour la progression
```

### Employés et Équipes
```http
GET    /api/employes                     # Liste des employés
GET    /api/employes/{id}                # Détails d'un employé
POST   /api/employes                     # Créer un employé
GET    /api/equipes                      # Liste des équipes
POST   /api/equipes                      # Créer une équipe
```

Pour une documentation complète des API, consultez la [documentation Swagger](https://localhost:5001/api-docs) une fois l'application lancée.

## 🤝 Contribution

Nous accueillons les contributions de la communauté ! Voici comment participer :

### Processus de contribution
1. **Fork** le repository
2. **Créer** une branche pour votre fonctionnalité (`git checkout -b feature/nouvelle-fonctionnalite`)
3. **Commiter** vos changements (`git commit -am 'Ajout d'une nouvelle fonctionnalité'`)
4. **Pousser** vers la branche (`git push origin feature/nouvelle-fonctionnalite`)
5. **Créer** une Pull Request

### Standards de code
- Suivre les conventions de nommage C#
- Ajouter des tests unitaires pour les nouvelles fonctionnalités
- Documenter les API avec des commentaires XML
- Respecter les principes SOLID

### Signalement de bugs
Utilisez les [GitHub Issues](https://github.com/votre-organisation/adeaumao/issues) pour signaler des bugs en incluant :
- Description détaillée du problème
- Étapes pour reproduire
- Environnement (OS, version .NET, etc.)
- Logs d'erreur si disponibles

## 📞 Support

### Communauté
- [📧 Email](mailto:support@adeaumao.com) : support@adeaumao.com
- [💬 Discord](https://discord.gg/adeaumao) : Rejoignez notre serveur Discord
- [📖 Wiki](https://github.com/votre-organisation/adeaumao/wiki) : Documentation communautaire

### Support commercial
Pour un support professionnel, contactez-nous à [commercial@adeaumao.com](mailto:commercial@adeaumao.com)

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

**Développé avec ❤️ par l'équipe AdeauMao**

*AdeauMao - Optimisez votre maintenance industrielle*


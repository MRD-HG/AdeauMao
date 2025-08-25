# Guide de Configuration de Visual Studio pour AdeauMao

Ce guide détaillé vous accompagne dans la configuration complète de Visual Studio 2022 pour développer efficacement avec le projet AdeauMao. Nous couvrirons l'installation des outils nécessaires, la configuration de l'environnement de développement, et les meilleures pratiques pour une productivité optimale.

## Table des Matières

1. [Prérequis système](#prérequis-système)
2. [Installation de Visual Studio 2022](#installation-de-visual-studio-2022)
3. [Configuration des workloads](#configuration-des-workloads)
4. [Extensions recommandées](#extensions-recommandées)
5. [Configuration du projet](#configuration-du-projet)
6. [Configuration de la base de données](#configuration-de-la-base-de-données)
7. [Configuration du debugging](#configuration-du-debugging)
8. [Outils de productivité](#outils-de-productivité)
9. [Résolution des problèmes courants](#résolution-des-problèmes-courants)

## Prérequis système

Avant de commencer l'installation, assurez-vous que votre système répond aux exigences minimales pour un développement optimal avec AdeauMao.

### Configuration matérielle recommandée

Pour une expérience de développement fluide, nous recommandons la configuration suivante :

**Processeur** : Intel Core i5-8400 / AMD Ryzen 5 2600 ou supérieur. Les processeurs multi-cœurs sont particulièrement bénéfiques pour la compilation parallèle et l'exécution simultanée de plusieurs services (API, base de données, outils de développement).

**Mémoire RAM** : 16 GB minimum, 32 GB recommandés. Visual Studio 2022, SQL Server, et les outils de développement peuvent consommer une quantité significative de mémoire, surtout lors du debugging de solutions complexes comme AdeauMao.

**Stockage** : SSD avec au moins 50 GB d'espace libre. Un SSD améliore considérablement les temps de compilation et de chargement des projets. Prévoyez de l'espace supplémentaire pour les bases de données de développement et les logs.

**Système d'exploitation** : Windows 10 version 1909 ou supérieur, Windows 11 recommandé. Les versions récentes offrent de meilleures performances et une compatibilité optimale avec .NET 8.

### Logiciels prérequis

Avant d'installer Visual Studio, assurez-vous d'avoir les éléments suivants :

**.NET 8 SDK** : Téléchargez et installez la dernière version du SDK .NET 8 depuis le site officiel Microsoft. Cette version est essentielle pour compiler et exécuter les projets AdeauMao.

**Git pour Windows** : Indispensable pour la gestion de version. Installez Git avec les options par défaut, en vous assurant que l'intégration avec Visual Studio est activée.

**SQL Server** : Installez SQL Server 2019 Express ou une version supérieure. Pour le développement, SQL Server Express est suffisant et gratuit. Assurez-vous d'installer également SQL Server Management Studio (SSMS) pour la gestion de base de données.

## Installation de Visual Studio 2022

L'installation de Visual Studio 2022 doit être effectuée avec soin pour inclure tous les composants nécessaires au développement AdeauMao.

### Téléchargement et lancement

Rendez-vous sur le site officiel de Microsoft Visual Studio et téléchargez Visual Studio 2022. Nous recommandons l'édition Professional ou Enterprise pour les fonctionnalités avancées de debugging et de profilage, mais l'édition Community est suffisante pour la plupart des développeurs.

Lancez l'installateur en tant qu'administrateur pour éviter les problèmes de permissions. L'installateur Visual Studio vous permettra de sélectionner précisément les composants nécessaires.

### Sélection de l'édition

**Visual Studio Community 2022** : Gratuite et suffisante pour la plupart des développeurs individuels et des petites équipes. Inclut toutes les fonctionnalités de base nécessaires pour AdeauMao.

**Visual Studio Professional 2022** : Recommandée pour les équipes de développement. Offre des fonctionnalités avancées de collaboration, de debugging et de profilage.

**Visual Studio Enterprise 2022** : Pour les grandes organisations nécessitant des outils avancés de test, d'analyse de code et de déploiement.

## Configuration des workloads

La sélection appropriée des workloads est cruciale pour disposer de tous les outils nécessaires au développement AdeauMao.

### Workloads essentiels

**ASP.NET and web development** : Ce workload est absolument indispensable pour AdeauMao. Il inclut les templates de projet ASP.NET Core, les outils de debugging web, et l'intégration avec IIS Express. Assurez-vous que les composants suivants sont sélectionnés :
- .NET 8 Runtime
- ASP.NET Core Runtime
- Web Deploy
- IIS Express
- JavaScript and TypeScript language support

**Data storage and processing** : Nécessaire pour travailler avec SQL Server et Entity Framework Core. Ce workload inclut :
- SQL Server Data Tools (SSDT)
- SQL Server Express LocalDB
- Entity Framework Tools
- Azure Data Lake and Stream Analytics Tools

**.NET desktop development** : Bien qu'AdeauMao soit principalement une application web, ce workload peut être utile pour développer des outils auxiliaires ou des clients de test.

### Composants individuels recommandés

En plus des workloads, sélectionnez ces composants individuels dans l'onglet "Individual components" :

**Debugging and testing** :
- .NET profiling tools
- IntelliTrace
- Live Unit Testing (Enterprise uniquement)

**Development activities** :
- Git for Windows
- GitHub extension for Visual Studio
- NuGet package manager

**SDKs, libraries, and frameworks** :
- .NET 8 SDK
- .NET Framework 4.8 targeting pack
- Windows 10/11 SDK (dernière version)

## Extensions recommandées

Les extensions Visual Studio améliorent significativement l'expérience de développement avec AdeauMao. Voici les extensions essentielles à installer.

### Extensions de productivité

**ReSharper** (JetBrains) : Bien que payante, cette extension transforme l'expérience de développement C#. Elle offre :
- Refactoring avancé
- Analyse de code en temps réel
- Navigation intelligente dans le code
- Génération automatique de code
- Tests unitaires intégrés

**CodeMaid** : Extension gratuite qui maintient la propreté du code :
- Nettoyage automatique du code
- Réorganisation des using statements
- Formatage cohérent
- Visualisation de la complexité du code

**Productivity Power Tools** : Collection d'outils Microsoft pour améliorer la productivité :
- Solution Explorer amélioré
- Tabs colorés
- Recherche rapide dans les fichiers
- Outils de navigation avancés

### Extensions pour le développement web

**Web Essentials** : Indispensable pour le développement web moderne :
- Support amélioré pour HTML, CSS, JavaScript
- Minification automatique
- Compilation SASS/LESS
- Validation en temps réel

**REST Client** : Pour tester les API AdeauMao directement depuis Visual Studio :
- Envoi de requêtes HTTP
- Gestion des headers et authentification
- Historique des requêtes
- Export vers Postman

### Extensions pour Entity Framework

**EF Core Power Tools** : Outils avancés pour Entity Framework Core :
- Reverse engineering de base de données
- Génération de diagrammes ER
- Optimisation des requêtes
- Migration avancée

**Entity Framework Visual Editor** : Éditeur visuel pour les modèles EF :
- Création visuelle des entités
- Gestion des relations
- Génération automatique de code
- Validation des modèles

## Configuration du projet

Une fois Visual Studio installé et configuré, il est temps de configurer spécifiquement le projet AdeauMao.

### Clonage du repository

Ouvrez Visual Studio et utilisez l'option "Clone a repository" depuis l'écran de démarrage. Entrez l'URL du repository AdeauMao :

```
https://github.com/votre-organisation/adeaumao.git
```

Choisissez un répertoire local approprié, de préférence sur un SSD pour de meilleures performances. Évitez les chemins trop longs ou contenant des caractères spéciaux.

### Configuration de la solution

Une fois le projet cloné, Visual Studio devrait automatiquement détecter la solution AdeauMao.sln. Si ce n'est pas le cas, ouvrez manuellement le fichier solution.

**Vérification des projets** : Assurez-vous que tous les projets se chargent correctement :
- AdeauMao.API (projet de démarrage)
- AdeauMao.Application
- AdeauMao.Infrastructure
- AdeauMao.Core

**Configuration du projet de démarrage** : Clic droit sur AdeauMao.API dans l'Explorateur de solutions et sélectionnez "Set as Startup Project".

### Restauration des packages NuGet

Visual Studio devrait automatiquement restaurer les packages NuGet au premier chargement. Si ce n'est pas le cas, clic droit sur la solution et sélectionnez "Restore NuGet Packages".

Vérifiez que tous les packages sont correctement installés en consultant la fenêtre "Package Manager Console" :

```powershell
Update-Package -Reinstall
```

### Configuration des paramètres de build

Configurez les paramètres de build pour optimiser le développement :

**Configuration Debug** : Utilisez cette configuration pour le développement quotidien. Elle inclut les symboles de debugging et désactive les optimisations.

**Configuration Release** : Utilisez cette configuration pour les tests de performance et les déploiements. Elle active les optimisations et exclut les informations de debugging.

**Platform target** : Configurez sur "Any CPU" pour une compatibilité maximale.

## Configuration de la base de données

La configuration de la base de données est cruciale pour le bon fonctionnement d'AdeauMao.

### Configuration de SQL Server

**Installation de SQL Server Express** : Si vous n'avez pas encore installé SQL Server, téléchargez SQL Server Express depuis le site Microsoft. Choisissez l'installation "Basic" pour une configuration simple.

**Configuration de l'instance** : Assurez-vous que l'instance SQL Server est configurée pour accepter les connexions locales. L'instance par défaut "(localdb)\MSSQLLocalDB" convient parfaitement pour le développement.

**SQL Server Management Studio** : Installez SSMS pour gérer visuellement vos bases de données. Cet outil est indispensable pour examiner les données, optimiser les requêtes et déboguer les problèmes de base de données.

### Configuration des chaînes de connexion

Modifiez le fichier `appsettings.json` dans le projet AdeauMao.API pour configurer la chaîne de connexion :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AdeauMaoDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Pour un environnement de développement partagé, vous pourriez préférer une instance SQL Server complète :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AdeauMaoDB;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

### Exécution des migrations

Ouvrez la "Package Manager Console" dans Visual Studio (Tools > NuGet Package Manager > Package Manager Console) et exécutez les commandes suivantes :

```powershell
# Définir le projet par défaut
Default-Project AdeauMao.Infrastructure

# Créer la base de données et appliquer les migrations
Update-Database
```

Si vous rencontrez des erreurs, vérifiez que :
- SQL Server est démarré
- La chaîne de connexion est correcte
- Vous avez les permissions nécessaires sur la base de données

### Vérification de la base de données

Utilisez SQL Server Management Studio pour vous connecter à votre instance et vérifier que :
- La base de données AdeauMaoDB a été créée
- Toutes les tables sont présentes
- Les données de seed ont été insérées

## Configuration du debugging

Une configuration appropriée du debugging améliore considérablement l'efficacité du développement.

### Configuration des points d'arrêt

**Points d'arrêt conditionnels** : Utilisez des conditions pour arrêter l'exécution seulement dans des cas spécifiques. Clic droit sur un point d'arrêt et sélectionnez "Conditions".

**Points d'arrêt avec actions** : Configurez des actions à exécuter lors du passage sur un point d'arrêt, comme l'écriture de logs ou l'évaluation d'expressions.

**Groupes de points d'arrêt** : Organisez vos points d'arrêt en groupes pour les activer/désactiver facilement selon le contexte de debugging.

### Configuration des fenêtres de debugging

**Fenêtre Autos** : Affiche automatiquement les variables pertinentes dans le contexte actuel.

**Fenêtre Locals** : Montre toutes les variables locales dans la portée actuelle.

**Fenêtre Watch** : Permet de surveiller des expressions spécifiques.

**Fenêtre Call Stack** : Indispensable pour comprendre le flux d'exécution.

**Fenêtre Output** : Configurez pour afficher les logs de l'application et du serveur.

### Configuration du debugging multi-projets

Pour déboguer efficacement AdeauMao, vous pourriez vouloir démarrer plusieurs projets simultanément :

1. Clic droit sur la solution
2. Sélectionnez "Properties"
3. Choisissez "Multiple startup projects"
4. Configurez AdeauMao.API en "Start"

### Debugging des API

**Postman Integration** : Configurez Postman pour pointer vers votre instance de développement (généralement https://localhost:5001).

**Swagger UI** : Utilisez l'interface Swagger intégrée pour tester les endpoints directement depuis le navigateur.

**Fiddler ou similaire** : Utilisez un proxy HTTP pour intercepter et analyser les requêtes/réponses.

## Outils de productivité

Maximisez votre productivité avec ces configurations et outils avancés.

### Configuration de l'éditeur

**Thème et couleurs** : Choisissez un thème qui réduit la fatigue oculaire. Le thème sombre est populaire pour les longues sessions de développement.

**Police et taille** : Utilisez une police monospace lisible comme Consolas, Fira Code, ou Cascadia Code. Une taille de 12-14pt est généralement optimale.

**Indentation et formatage** : Configurez l'indentation automatique et le formatage du code selon les standards de l'équipe :
- Indentation : 4 espaces
- Accolades : nouvelle ligne
- Espaces autour des opérateurs

### Raccourcis clavier essentiels

Maîtrisez ces raccourcis pour une productivité maximale :

**Navigation** :
- `Ctrl+,` : Recherche rapide (Go to All)
- `Ctrl+T` : Recherche de types
- `F12` : Aller à la définition
- `Shift+F12` : Rechercher toutes les références
- `Ctrl+F12` : Aller à l'implémentation

**Édition** :
- `Ctrl+K, Ctrl+D` : Formater le document
- `Ctrl+K, Ctrl+C` : Commenter la sélection
- `Ctrl+K, Ctrl+U` : Décommenter la sélection
- `Alt+↑/↓` : Déplacer les lignes
- `Ctrl+D` : Dupliquer la ligne

**Debugging** :
- `F5` : Démarrer le debugging
- `F9` : Basculer le point d'arrêt
- `F10` : Pas à pas principal
- `F11` : Pas à pas détaillé
- `Shift+F5` : Arrêter le debugging

### Configuration de Git

Configurez l'intégration Git dans Visual Studio :

**Team Explorer** : Utilisez Team Explorer pour gérer les branches, commits, et synchronisation.

**Git Changes** : La nouvelle fenêtre Git Changes offre une interface moderne pour les opérations Git courantes.

**Configuration des branches** : Configurez des stratégies de branche appropriées :
- `main` : branche de production
- `develop` : branche de développement
- `feature/*` : branches de fonctionnalités
- `hotfix/*` : corrections urgentes

### Snippets de code

Créez des snippets personnalisés pour les patterns courants d'AdeauMao :

**Controller snippet** : Template pour créer rapidement des contrôleurs API.

**Service snippet** : Template pour les services avec injection de dépendance.

**DTO snippet** : Template pour les objets de transfert de données.

**Test snippet** : Template pour les tests unitaires.

## Résolution des problèmes courants

Cette section couvre les problèmes les plus fréquemment rencontrés lors de la configuration.

### Problèmes de compilation

**Erreur de package NuGet** : Si des packages ne se restaurent pas correctement :
1. Supprimez le dossier `packages` et `bin/obj`
2. Redémarrez Visual Studio
3. Exécutez `Update-Package -Reinstall`

**Erreur de version .NET** : Vérifiez que le SDK .NET 8 est installé :
```bash
dotnet --list-sdks
```

**Erreur de référence de projet** : Vérifiez que tous les projets de la solution sont correctement référencés.

### Problèmes de base de données

**Erreur de connexion SQL Server** :
1. Vérifiez que SQL Server est démarré
2. Testez la chaîne de connexion avec SSMS
3. Vérifiez les permissions utilisateur

**Erreur de migration** :
1. Supprimez la base de données existante
2. Supprimez le dossier Migrations
3. Recréez les migrations : `Add-Migration InitialCreate`
4. Appliquez : `Update-Database`

### Problèmes de performance

**Visual Studio lent** :
1. Désactivez les extensions non essentielles
2. Augmentez la mémoire virtuelle
3. Excluez les dossiers de build de l'antivirus
4. Utilisez un SSD pour les projets

**Compilation lente** :
1. Activez la compilation parallèle
2. Utilisez des builds incrémentales
3. Configurez des exclusions d'antivirus appropriées

### Problèmes de debugging

**Points d'arrêt ignorés** :
1. Vérifiez que vous êtes en mode Debug
2. Assurez-vous que les symboles sont chargés
3. Vérifiez que le code correspond à la version exécutée

**Debugging impossible** :
1. Vérifiez que le projet est défini comme projet de démarrage
2. Assurez-vous que l'application se lance sans erreur
3. Vérifiez les paramètres de debugging du projet

Cette configuration complète de Visual Studio vous permettra de développer efficacement avec AdeauMao. N'hésitez pas à personnaliser ces paramètres selon vos préférences et les besoins spécifiques de votre équipe de développement.


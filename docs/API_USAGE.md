# Guide d'Utilisation des API AdeauMao

Ce guide complet vous accompagne dans l'utilisation des API REST d'AdeauMao. Vous apprendrez à authentifier vos requêtes, à manipuler les données, et à intégrer efficacement AdeauMao dans vos applications ou workflows existants.

## Table des Matières

1. [Introduction aux API AdeauMao](#introduction-aux-api-adeaumao)
2. [Authentification et sécurité](#authentification-et-sécurité)
3. [Structure des réponses](#structure-des-réponses)
4. [Gestion des équipements](#gestion-des-équipements)
5. [Gestion des employés et équipes](#gestion-des-employés-et-équipes)
6. [Ordres de travail](#ordres-de-travail)
7. [Demandes d'intervention](#demandes-dintervention)
8. [Pagination et filtrage](#pagination-et-filtrage)
9. [Gestion des erreurs](#gestion-des-erreurs)
10. [Exemples d'intégration](#exemples-dintégration)

## Introduction aux API AdeauMao

Les API AdeauMao suivent les principes REST et utilisent le format JSON pour l'échange de données. Toutes les API sont documentées avec OpenAPI/Swagger et accessibles via l'interface interactive à l'adresse `/api-docs` de votre instance AdeauMao.

### URL de base

L'URL de base pour toutes les API est :
```
https://votre-domaine.com/api
```

Pour un environnement de développement local :
```
https://localhost:5001/api
```

### Formats de données

**Requêtes** : Les données doivent être envoyées au format JSON avec l'en-tête `Content-Type: application/json`.

**Réponses** : Toutes les réponses sont au format JSON avec l'en-tête `Content-Type: application/json; charset=utf-8`.

**Dates** : Les dates suivent le format ISO 8601 (ex: `2024-01-15T10:30:00Z`).

**Encodage** : UTF-8 pour tous les textes.

### Codes de statut HTTP

Les API AdeauMao utilisent les codes de statut HTTP standard :

- `200 OK` : Requête réussie
- `201 Created` : Ressource créée avec succès
- `400 Bad Request` : Erreur dans la requête (validation, format)
- `401 Unauthorized` : Authentification requise ou invalide
- `403 Forbidden` : Accès refusé (permissions insuffisantes)
- `404 Not Found` : Ressource non trouvée
- `409 Conflict` : Conflit (ex: référence déjà existante)
- `422 Unprocessable Entity` : Erreur de validation métier
- `500 Internal Server Error` : Erreur serveur

## Authentification et sécurité

AdeauMao utilise l'authentification JWT (JSON Web Token) pour sécuriser l'accès aux API.

### Obtention d'un token

Pour obtenir un token d'authentification, utilisez l'endpoint de connexion :

```http
POST /api/auth/login
Content-Type: application/json

{
  "userName": "votre-nom-utilisateur",
  "password": "votre-mot-de-passe",
  "rememberMe": false
}
```

**Réponse réussie** :
```json
{
  "success": true,
  "message": "Connexion réussie",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "550e8400-e29b-41d4-a716-446655440000",
    "tokenExpiration": "2024-01-15T18:30:00Z",
    "user": {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userName": "technicien1",
      "email": "technicien1@adeaumao.com",
      "firstName": "Ahmed",
      "lastName": "Alami",
      "roles": ["Technician"]
    }
  },
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Utilisation du token

Incluez le token dans l'en-tête `Authorization` de toutes vos requêtes :

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Rafraîchissement du token

Les tokens ont une durée de vie limitée (24 heures par défaut). Utilisez le refresh token pour obtenir un nouveau token :

```http
POST /api/auth/refresh-token
Content-Type: application/json
Authorization: Bearer votre-token-actuel

{
  "token": "votre-token-actuel",
  "refreshToken": "votre-refresh-token"
}
```

### Gestion des rôles

AdeauMao implémente un système de rôles pour contrôler l'accès aux fonctionnalités :

- **Administrator** : Accès complet à toutes les fonctionnalités
- **Manager** : Gestion des équipements, employés, et validation des OT
- **Technician** : Création et mise à jour des OT, consultation des équipements
- **Operator** : Consultation et création de demandes d'intervention

Certains endpoints nécessitent des rôles spécifiques. Ces restrictions sont documentées pour chaque endpoint.

## Structure des réponses

Toutes les réponses API suivent une structure cohérente pour faciliter l'intégration.

### Réponse standard

```json
{
  "success": true,
  "message": "Description de l'opération",
  "data": {
    // Données spécifiques à l'endpoint
  },
  "errors": null,
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Réponse d'erreur

```json
{
  "success": false,
  "message": "Description de l'erreur",
  "data": null,
  "errors": [
    "Détail de l'erreur 1",
    "Détail de l'erreur 2"
  ],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Réponse paginée

```json
{
  "success": true,
  "message": "Données récupérées avec succès",
  "data": {
    "items": [
      // Liste des éléments
    ],
    "totalCount": 150,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 15,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Gestion des équipements

Les équipements sont au cœur du système AdeauMao. Ces API permettent de gérer l'inventaire complet des équipements industriels.

### Lister les équipements

```http
GET /api/equipements?pageNumber=1&pageSize=10&searchTerm=compresseur
Authorization: Bearer votre-token
```

**Paramètres de requête** :
- `pageNumber` (optionnel) : Numéro de page (défaut: 1)
- `pageSize` (optionnel) : Taille de page (défaut: 10, max: 100)
- `searchTerm` (optionnel) : Terme de recherche (nom, référence, type)
- `sortBy` (optionnel) : Champ de tri (nom, reference, type, datecreation)
- `sortDescending` (optionnel) : Tri descendant (défaut: false)
- `dateFrom` (optionnel) : Date de création minimum
- `dateTo` (optionnel) : Date de création maximum

**Exemple de réponse** :
```json
{
  "success": true,
  "message": "Équipements récupérés avec succès",
  "data": {
    "items": [
      {
        "id": 1,
        "reference": "EQ-001",
        "nom": "Compresseur Principal",
        "typeEquipement": "Compresseur",
        "fabricant": "Atlas Copco",
        "modele": "GA-75",
        "dateMiseEnService": "2023-01-15T00:00:00Z",
        "localisation": "Atelier Principal - Zone A",
        "ligneProductionId": 1,
        "ligneProductionNom": "Ligne de Production 1",
        "description": "Compresseur d'air principal",
        "etatOperationnel": "En service",
        "dateCreation": "2023-01-10T08:00:00Z",
        "dateModification": "2024-01-15T10:30:00Z",
        "organes": [
          {
            "id": 1,
            "nomOrgane": "Moteur électrique",
            "equipementId": 1,
            "description": "Moteur principal 75kW",
            "dateCreation": "2023-01-10T08:00:00Z"
          }
        ]
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
}
```

### Obtenir un équipement spécifique

```http
GET /api/equipements/1
Authorization: Bearer votre-token
```

### Créer un équipement

```http
POST /api/equipements
Content-Type: application/json
Authorization: Bearer votre-token

{
  "reference": "EQ-002",
  "nom": "Pompe Centrifuge",
  "typeEquipement": "Pompe",
  "fabricant": "Grundfos",
  "modele": "CR-64",
  "dateMiseEnService": "2024-01-15",
  "localisation": "Station de pompage",
  "ligneProductionId": 1,
  "description": "Pompe centrifuge pour circuit de refroidissement",
  "etatOperationnel": "En service"
}
```

**Validation** :
- `reference` : Obligatoire, unique, 50 caractères max, format alphanuméririque avec tirets
- `nom` : Obligatoire, 255 caractères max
- `typeEquipement` : Optionnel, 100 caractères max
- `fabricant` : Optionnel, 100 caractères max
- `modele` : Optionnel, 100 caractères max
- `dateMiseEnService` : Optionnel, ne peut pas être dans le futur
- `localisation` : Optionnel, 255 caractères max
- `ligneProductionId` : Optionnel, doit référencer une ligne de production existante

### Mettre à jour un équipement

```http
PUT /api/equipements/1
Content-Type: application/json
Authorization: Bearer votre-token

{
  "id": 1,
  "reference": "EQ-001",
  "nom": "Compresseur Principal Modifié",
  "typeEquipement": "Compresseur",
  "fabricant": "Atlas Copco",
  "modele": "GA-75",
  "dateMiseEnService": "2023-01-15",
  "localisation": "Atelier Principal - Zone B",
  "ligneProductionId": 1,
  "description": "Compresseur d'air principal - Déplacé en zone B",
  "etatOperationnel": "En maintenance"
}
```

### Supprimer un équipement

```http
DELETE /api/equipements/1
Authorization: Bearer votre-token
```

**Note** : La suppression n'est possible que si l'équipement n'est référencé dans aucun ordre de travail ou demande d'intervention.

### Gestion des organes

Les organes sont des composants d'équipements qui peuvent faire l'objet de maintenance spécifique.

**Lister les organes d'un équipement** :
```http
GET /api/equipements/1/organes
Authorization: Bearer votre-token
```

**Créer un organe** :
```http
POST /api/equipements/organes
Content-Type: application/json
Authorization: Bearer votre-token

{
  "nomOrgane": "Filtre à air",
  "equipementId": 1,
  "description": "Filtre à air d'admission"
}
```

**Supprimer un organe** :
```http
DELETE /api/equipements/organes/1
Authorization: Bearer votre-token
```

## Gestion des employés et équipes

La gestion des ressources humaines est essentielle pour l'organisation du travail de maintenance.

### Lister les employés

```http
GET /api/employes?pageNumber=1&pageSize=10&searchTerm=ahmed
Authorization: Bearer votre-token
```

**Exemple de réponse** :
```json
{
  "success": true,
  "message": "Employés récupérés avec succès",
  "data": {
    "items": [
      {
        "id": 1,
        "nom": "Alami",
        "prenom": "Ahmed",
        "contact": "0661234567",
        "roleInterne": "Technicien Électricien",
        "utilisateurId": 1,
        "utilisateurNom": "ahmed.alami",
        "dateCreation": "2023-01-10T08:00:00Z",
        "dateModification": "2024-01-15T10:30:00Z",
        "competences": [
          {
            "id": 1,
            "nomCompetence": "Électricité industrielle",
            "description": "Maintenance électrique haute tension",
            "dateCreation": "2023-01-10T08:00:00Z"
          }
        ],
        "equipes": [
          {
            "id": 1,
            "nomEquipe": "Équipe Électricité",
            "responsableId": 2,
            "responsableNom": "Hassan Bennani",
            "dateCreation": "2023-01-10T08:00:00Z"
          }
        ]
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
  }
}
```

### Créer un employé

```http
POST /api/employes
Content-Type: application/json
Authorization: Bearer votre-token

{
  "nom": "Bennani",
  "prenom": "Hassan",
  "contact": "0662345678",
  "roleInterne": "Chef d'équipe",
  "utilisateurId": 2
}
```

### Gestion des équipes

**Lister les équipes** :
```http
GET /api/equipes
Authorization: Bearer votre-token
```

**Créer une équipe** :
```http
POST /api/equipes
Content-Type: application/json
Authorization: Bearer votre-token

{
  "nomEquipe": "Équipe Mécanique",
  "responsableId": 3
}
```

**Assigner un employé à une équipe** :
```http
POST /api/employes/assign-equipe
Content-Type: application/json
Authorization: Bearer votre-token

{
  "equipeId": 1,
  "employeId": 4
}
```

### Gestion des compétences

**Lister les compétences** :
```http
GET /api/competences
Authorization: Bearer votre-token
```

**Créer une compétence** :
```http
POST /api/competences
Content-Type: application/json
Authorization: Bearer votre-token

{
  "nomCompetence": "Soudure TIG",
  "description": "Soudage TIG acier inoxydable"
}
```

**Assigner une compétence à un employé** :
```http
POST /api/employes/assign-competence
Content-Type: application/json
Authorization: Bearer votre-token

{
  "employeId": 1,
  "competenceId": 2
}
```

## Ordres de travail

Les ordres de travail (OT) sont le cœur opérationnel du système de maintenance.

### Lister les ordres de travail

```http
GET /api/ordresdetravail?pageNumber=1&pageSize=10&statut=EnCours
Authorization: Bearer votre-token
```

**Paramètres de filtrage spécifiques** :
- `statut` : AFaire, EnCours, Termine, Annule
- `priorite` : Basse, Moyenne, Haute, Urgente
- `typeMaintenance` : Preventive, Corrective, Ameliorative
- `equipementId` : ID de l'équipement
- `technicienId` : ID du technicien assigné

**Exemple de réponse** :
```json
{
  "success": true,
  "message": "Ordres de travail récupérés avec succès",
  "data": {
    "items": [
      {
        "id": 1,
        "numeroOT": "OT-2024-001",
        "equipementId": 1,
        "equipementNom": "Compresseur Principal",
        "equipementReference": "EQ-001",
        "descriptionTache": "Maintenance préventive trimestrielle",
        "dateDebutPrevue": "2024-01-20T08:00:00Z",
        "dateFinPrevue": "2024-01-20T12:00:00Z",
        "technicienAssigneeId": 1,
        "technicienNom": "Ahmed Alami",
        "statut": "EnCours",
        "priorite": "Moyenne",
        "dateDebutReelle": "2024-01-20T08:15:00Z",
        "dateFinReelle": null,
        "tempsPasse": 2.5,
        "causePanneId": null,
        "causePanneNom": null,
        "solutionApportee": null,
        "remarques": "Démarrage avec 15 minutes de retard",
        "demandeInterventionId": null,
        "typeMaintenance": "Preventive",
        "organeId": 1,
        "organeNom": "Moteur électrique",
        "coutReel": 150.00,
        "pourcentageProgression": 60,
        "sousTraitantId": null,
        "sousTraitantNom": null,
        "dateValidation": null,
        "validateurId": null,
        "validateurNom": null,
        "workflowId": 1,
        "workflowNom": "Workflow Maintenance Standard",
        "dateCreation": "2024-01-15T10:00:00Z",
        "dateModification": "2024-01-20T10:30:00Z"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
  }
}
```

### Créer un ordre de travail

```http
POST /api/ordresdetravail
Content-Type: application/json
Authorization: Bearer votre-token

{
  "numeroOT": "OT-2024-002",
  "equipementId": 1,
  "descriptionTache": "Remplacement du filtre à air",
  "dateDebutPrevue": "2024-01-25T09:00:00Z",
  "dateFinPrevue": "2024-01-25T11:00:00Z",
  "technicienAssigneeId": 1,
  "statut": "AFaire",
  "priorite": "Moyenne",
  "typeMaintenance": "Preventive",
  "organeId": 1,
  "workflowId": 1
}
```

### Mettre à jour la progression

```http
PATCH /api/ordresdetravail/1/progression
Content-Type: application/json
Authorization: Bearer votre-token

{
  "id": 1,
  "pourcentageProgression": 80,
  "statut": "EnCours",
  "dateDebutReelle": "2024-01-20T08:15:00Z",
  "tempsPasse": 3.5,
  "solutionApportee": "Filtre remplacé, système testé",
  "remarques": "Intervention conforme aux procédures"
}
```

### Valider un ordre de travail

```http
POST /api/ordresdetravail/1/validate
Content-Type: application/json
Authorization: Bearer votre-token

{
  "id": 1,
  "validateurId": 2,
  "commentaires": "Travail conforme, équipement remis en service"
}
```

### Générer un numéro d'OT

```http
GET /api/ordresdetravail/generate-numero
Authorization: Bearer votre-token
```

**Réponse** :
```json
{
  "success": true,
  "message": "Numéro généré avec succès",
  "data": "OT-2024-003"
}
```

## Demandes d'intervention

Les demandes d'intervention permettent aux opérateurs de signaler des problèmes nécessitant une intervention technique.

### Lister les demandes d'intervention

```http
GET /api/demandesintervention?pageNumber=1&pageSize=10&statut=Nouvelle
Authorization: Bearer votre-token
```

**Paramètres de filtrage** :
- `statut` : Nouvelle, EnCours, Terminee, Annulee
- `priorite` : Basse, Moyenne, Haute, Urgente
- `equipementId` : ID de l'équipement
- `demandeurId` : ID du demandeur

### Créer une demande d'intervention

```http
POST /api/demandesintervention
Content-Type: application/json
Authorization: Bearer votre-token

{
  "equipementId": 1,
  "descriptionProbleme": "Bruit anormal au niveau du compresseur, vibrations importantes",
  "demandeurId": 3,
  "priorite": "Haute"
}
```

### Mettre à jour le statut

```http
PATCH /api/demandesintervention/1/statut
Content-Type: application/json
Authorization: Bearer votre-token

{
  "id": 1,
  "statut": "EnCours",
  "commentaires": "Prise en charge par l'équipe technique"
}
```

### Créer un OT depuis une demande

```http
POST /api/demandesintervention/1/create-ot
Content-Type: application/json
Authorization: Bearer votre-token

{
  "numeroOT": "OT-2024-004",
  "equipementId": 1,
  "descriptionTache": "Diagnostic et réparation - Bruit anormal compresseur",
  "dateDebutPrevue": "2024-01-22T08:00:00Z",
  "dateFinPrevue": "2024-01-22T16:00:00Z",
  "technicienAssigneeId": 1,
  "statut": "AFaire",
  "priorite": "Haute",
  "typeMaintenance": "Corrective"
}
```

## Pagination et filtrage

Toutes les API de liste supportent la pagination et le filtrage pour optimiser les performances et faciliter la navigation.

### Paramètres de pagination

- `pageNumber` : Numéro de page (commence à 1)
- `pageSize` : Nombre d'éléments par page (max 100)

### Paramètres de tri

- `sortBy` : Champ de tri (varie selon l'endpoint)
- `sortDescending` : true pour tri descendant, false pour ascendant

### Paramètres de recherche

- `searchTerm` : Terme de recherche global
- `dateFrom` / `dateTo` : Filtrage par plage de dates

### Exemple complet

```http
GET /api/equipements?pageNumber=2&pageSize=20&searchTerm=pompe&sortBy=nom&sortDescending=false&dateFrom=2024-01-01&dateTo=2024-01-31
Authorization: Bearer votre-token
```

## Gestion des erreurs

Une gestion appropriée des erreurs est cruciale pour une intégration robuste.

### Types d'erreurs courantes

**Erreur de validation (400)** :
```json
{
  "success": false,
  "message": "Erreur de validation",
  "data": null,
  "errors": [
    "Le champ 'nom' est requis",
    "La référence doit être unique"
  ],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

**Erreur d'authentification (401)** :
```json
{
  "success": false,
  "message": "Token d'authentification invalide ou expiré",
  "data": null,
  "errors": ["Unauthorized"],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

**Erreur de permissions (403)** :
```json
{
  "success": false,
  "message": "Accès refusé",
  "data": null,
  "errors": ["Permissions insuffisantes pour cette opération"],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

**Ressource non trouvée (404)** :
```json
{
  "success": false,
  "message": "Équipement non trouvé",
  "data": null,
  "errors": ["L'équipement avec l'ID 999 n'existe pas"],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Stratégies de gestion d'erreurs

**Retry automatique** : Implémentez une logique de retry pour les erreurs temporaires (5xx).

**Gestion des tokens expirés** : Détectez les erreurs 401 et rafraîchissez automatiquement le token.

**Validation côté client** : Validez les données avant envoi pour réduire les erreurs 400.

**Logging des erreurs** : Loggez toutes les erreurs pour faciliter le debugging.

## Exemples d'intégration

Cette section présente des exemples concrets d'intégration dans différents langages et frameworks.

### JavaScript/Node.js

```javascript
class AdeauMaoClient {
  constructor(baseUrl, credentials) {
    this.baseUrl = baseUrl;
    this.credentials = credentials;
    this.token = null;
    this.refreshToken = null;
  }

  async authenticate() {
    const response = await fetch(`${this.baseUrl}/api/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(this.credentials)
    });

    if (!response.ok) {
      throw new Error('Authentication failed');
    }

    const data = await response.json();
    this.token = data.data.token;
    this.refreshToken = data.data.refreshToken;
    
    return data.data;
  }

  async makeRequest(endpoint, options = {}) {
    if (!this.token) {
      await this.authenticate();
    }

    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      ...options,
      headers: {
        'Authorization': `Bearer ${this.token}`,
        'Content-Type': 'application/json',
        ...options.headers
      }
    });

    if (response.status === 401) {
      await this.refreshTokens();
      return this.makeRequest(endpoint, options);
    }

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message);
    }

    return response.json();
  }

  async getEquipements(filters = {}) {
    const params = new URLSearchParams(filters);
    return this.makeRequest(`/api/equipements?${params}`);
  }

  async createEquipement(equipement) {
    return this.makeRequest('/api/equipements', {
      method: 'POST',
      body: JSON.stringify(equipement)
    });
  }

  async refreshTokens() {
    const response = await fetch(`${this.baseUrl}/api/auth/refresh-token`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.token}`
      },
      body: JSON.stringify({
        token: this.token,
        refreshToken: this.refreshToken
      })
    });

    if (response.ok) {
      const data = await response.json();
      this.token = data.data.token;
      this.refreshToken = data.data.refreshToken;
    } else {
      await this.authenticate();
    }
  }
}

// Utilisation
const client = new AdeauMaoClient('https://localhost:5001', {
  userName: 'admin',
  password: 'password123'
});

// Récupérer les équipements
const equipements = await client.getEquipements({
  pageNumber: 1,
  pageSize: 10,
  searchTerm: 'compresseur'
});

// Créer un équipement
const nouvelEquipement = await client.createEquipement({
  reference: 'EQ-003',
  nom: 'Nouvelle Pompe',
  typeEquipement: 'Pompe',
  fabricant: 'Grundfos'
});
```

### Python

```python
import requests
import json
from datetime import datetime, timedelta

class AdeauMaoClient:
    def __init__(self, base_url, credentials):
        self.base_url = base_url
        self.credentials = credentials
        self.token = None
        self.refresh_token = None
        self.token_expiry = None
        self.session = requests.Session()

    def authenticate(self):
        response = self.session.post(
            f"{self.base_url}/api/auth/login",
            json=self.credentials
        )
        response.raise_for_status()
        
        data = response.json()['data']
        self.token = data['token']
        self.refresh_token = data['refreshToken']
        self.token_expiry = datetime.fromisoformat(
            data['tokenExpiration'].replace('Z', '+00:00')
        )
        
        self.session.headers.update({
            'Authorization': f"Bearer {self.token}"
        })
        
        return data

    def _ensure_authenticated(self):
        if not self.token or datetime.now() >= self.token_expiry:
            if self.refresh_token:
                self._refresh_token()
            else:
                self.authenticate()

    def _refresh_token(self):
        response = self.session.post(
            f"{self.base_url}/api/auth/refresh-token",
            json={
                'token': self.token,
                'refreshToken': self.refresh_token
            }
        )
        
        if response.status_code == 200:
            data = response.json()['data']
            self.token = data['token']
            self.refresh_token = data['refreshToken']
            self.token_expiry = datetime.fromisoformat(
                data['tokenExpiration'].replace('Z', '+00:00')
            )
            
            self.session.headers.update({
                'Authorization': f"Bearer {self.token}"
            })
        else:
            self.authenticate()

    def make_request(self, method, endpoint, **kwargs):
        self._ensure_authenticated()
        
        response = self.session.request(
            method, 
            f"{self.base_url}{endpoint}", 
            **kwargs
        )
        
        if response.status_code == 401:
            self._refresh_token()
            response = self.session.request(
                method, 
                f"{self.base_url}{endpoint}", 
                **kwargs
            )
        
        response.raise_for_status()
        return response.json()

    def get_equipements(self, **filters):
        return self.make_request('GET', '/api/equipements', params=filters)

    def create_equipement(self, equipement_data):
        return self.make_request('POST', '/api/equipements', json=equipement_data)

    def get_ordres_travail(self, **filters):
        return self.make_request('GET', '/api/ordresdetravail', params=filters)

    def update_ot_progression(self, ot_id, progression_data):
        return self.make_request(
            'PATCH', 
            f'/api/ordresdetravail/{ot_id}/progression',
            json=progression_data
        )

# Utilisation
client = AdeauMaoClient('https://localhost:5001', {
    'userName': 'admin',
    'password': 'password123'
})

# Récupérer les équipements avec filtrage
equipements = client.get_equipements(
    pageNumber=1,
    pageSize=20,
    searchTerm='pompe',
    sortBy='nom'
)

print(f"Trouvé {equipements['data']['totalCount']} équipements")

# Créer un nouvel équipement
nouvel_equipement = client.create_equipement({
    'reference': 'EQ-004',
    'nom': 'Pompe de Circulation',
    'typeEquipement': 'Pompe',
    'fabricant': 'Wilo',
    'localisation': 'Salle des machines'
})

print(f"Équipement créé avec l'ID: {nouvel_equipement['data']['id']}")
```

### C# (.NET)

```csharp
using System.Text.Json;
using System.Text;

public class AdeauMaoClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly AuthCredentials _credentials;
    private string? _token;
    private string? _refreshToken;
    private DateTime _tokenExpiry;

    public AdeauMaoClient(string baseUrl, AuthCredentials credentials)
    {
        _baseUrl = baseUrl;
        _credentials = credentials;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<AuthResponse> AuthenticateAsync()
    {
        var json = JsonSerializer.Serialize(_credentials);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<ApiResponse<AuthData>>(responseContent);
        
        _token = authResponse.Data.Token;
        _refreshToken = authResponse.Data.RefreshToken;
        _tokenExpiry = authResponse.Data.TokenExpiration;
        
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        
        return authResponse.Data;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiry)
        {
            if (!string.IsNullOrEmpty(_refreshToken))
            {
                await RefreshTokenAsync();
            }
            else
            {
                await AuthenticateAsync();
            }
        }
    }

    private async Task RefreshTokenAsync()
    {
        var refreshRequest = new
        {
            Token = _token,
            RefreshToken = _refreshToken
        };
        
        var json = JsonSerializer.Serialize(refreshRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/auth/refresh-token", content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<ApiResponse<AuthData>>(responseContent);
            
            _token = authResponse.Data.Token;
            _refreshToken = authResponse.Data.RefreshToken;
            _tokenExpiry = authResponse.Data.TokenExpiration;
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }
        else
        {
            await AuthenticateAsync();
        }
    }

    public async Task<PagedResult<Equipement>> GetEquipementsAsync(EquipementFilter filter = null)
    {
        await EnsureAuthenticatedAsync();
        
        var queryString = filter?.ToQueryString() ?? "";
        var response = await _httpClient.GetAsync($"/api/equipements{queryString}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<PagedResult<Equipement>>>(content);
        
        return result.Data;
    }

    public async Task<Equipement> CreateEquipementAsync(CreateEquipementDto equipement)
    {
        await EnsureAuthenticatedAsync();
        
        var json = JsonSerializer.Serialize(equipement);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/equipements", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<Equipement>>(responseContent);
        
        return result.Data;
    }
}

// Classes de modèle
public class AuthCredentials
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}

public class EquipementFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
    
    public string ToQueryString()
    {
        var parameters = new List<string>();
        
        parameters.Add($"pageNumber={PageNumber}");
        parameters.Add($"pageSize={PageSize}");
        
        if (!string.IsNullOrEmpty(SearchTerm))
            parameters.Add($"searchTerm={Uri.EscapeDataString(SearchTerm)}");
            
        if (!string.IsNullOrEmpty(SortBy))
            parameters.Add($"sortBy={SortBy}");
            
        parameters.Add($"sortDescending={SortDescending.ToString().ToLower()}");
        
        return parameters.Count > 0 ? "?" + string.Join("&", parameters) : "";
    }
}

// Utilisation
var client = new AdeauMaoClient("https://localhost:5001", new AuthCredentials
{
    UserName = "admin",
    Password = "password123"
});

// Authentification
await client.AuthenticateAsync();

// Récupérer les équipements
var equipements = await client.GetEquipementsAsync(new EquipementFilter
{
    PageNumber = 1,
    PageSize = 10,
    SearchTerm = "compresseur",
    SortBy = "nom"
});

Console.WriteLine($"Trouvé {equipements.TotalCount} équipements");

// Créer un équipement
var nouvelEquipement = await client.CreateEquipementAsync(new CreateEquipementDto
{
    Reference = "EQ-005",
    Nom = "Compresseur Auxiliaire",
    TypeEquipement = "Compresseur",
    Fabricant = "Kaeser"
});

Console.WriteLine($"Équipement créé avec l'ID: {nouvelEquipement.Id}");
```

Ces exemples d'intégration montrent comment utiliser efficacement les API AdeauMao dans différents environnements de développement. Ils incluent la gestion de l'authentification, le rafraîchissement automatique des tokens, et la gestion des erreurs, constituant une base solide pour vos intégrations.


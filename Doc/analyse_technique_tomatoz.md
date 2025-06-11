# Analyse Technique – Wiki Collaboratif Tomatoz

**Nom du projet :** Tomatoz  
**Technologies principales :** Blazor (.NET 9), MSSQL, .NET Aspire, TestRigor, Shouldly, MSTest  
**Date :** [À compléter]

---

## 🏗️ 1. Architecture générale

### 1.1 Structure des projets

- `Tomatoz.Client` : Frontend en Blazor (WebAssembly ou Server)
- `Tomatoz.Server` : API REST (BFF) exposant les services vers le client
- `Tomatoz.Shared` : Bibliothèque partagée pour DTOs, enums, constantes, modèles simples
- `Tomatoz.Application` : Couche métier, services, interfaces, logique applicative
- `Tomatoz.Infrastructure` : Implémentation de l'accès aux données (EF Core, Repositories)
- `Tomatoz.Tests.Unit` : Tests unitaires avec MSTest + Shouldly
- `Tomatoz.Tests.WebTests` : Tests web automatisés avec TestRigor
- Orchestration via **.NET Aspire**

---

## 🔐 2. Authentification & Sécurité

- Authentification classique (email/mot de passe) avec ASP.NET Identity
- Intégration des providers standards :
  - Google
  - Facebook
  - Microsoft
- Configuration via `appsettings.json`
- Côté client : Blazor auth avec OIDC
- Validation des accès exclusivement côté serveur via `[Authorize]`
- Utilisation de **politiques d’autorisations** (roles, claims, etc.)

---

## ⚙️ 3. Architecture technique détaillée

### 3.1 Couche API (Tomatoz.Server)

- Sert de **Backend For Frontend (BFF)** pour Tomatoz.Client
- Communique avec `Tomatoz.Application` via interfaces injectées
- API RESTful exposée en JSON uniquement
- Versionnement des routes (`/api/v1/...`)
- Configuration de CORS pour autoriser les appels du client
- Mise en place de **limites d’usage** via un système d’API key pour la documentation Scalar

### 3.2 Documentation API

- **Scalar** au lieu de Swagger
- Exposée même en production, mais protégée via API key
- Possibilité de basculer vers une licence payante en cas de dépassement de quotas

---

## 🧩 4. Pratiques recommandées

### 4.1 Injection de dépendances

- `Scoped` : services métier, contextes liés à la requête
- `Singleton` : services utilitaires
- `Transient` : services légers, stateless
- Utilisation de `IServiceCollection` pour une configuration centralisée

### 4.2 Logging & Télémétrie

- Standard `ILogger<T>`
- Intégration avec OpenTelemetry
- Exporters vers Console ou Jaeger
- Logs par niveau : Information, Warning, Error

### 4.3 Configuration

- Fichier `appsettings.json` + `appsettings.Development.json`
- Utilisation de `IOptions<T>` pour les paramètres métiers
- Secrets protégés via User Secrets / Azure Key Vault

---

## 📦 5. Données et EF Core

- Utilisation d’**Entity Framework Core**
- Migrations via `dotnet ef migrations`
- Séparation entités / DTOs
- Mapping via AutoMapper (entre Domain & DTO)

---

## 🧪 6. Tests

### 6.1 Unit Tests

- Framework : **MSTest**
- Assertions : **Shouldly**
- Portée :
  - Services métiers
  - Validations métiers
  - Méthodes utilitaires

### 6.2 Component Tests

- Utilisation de **bUnit** pour tester les composants Blazor
- Couplé à MSTest pour uniformité

### 6.3 Web Tests

- Utilisation de **TestRigor** pour les tests E2E :
  - Tests en langage naturel
  - Génération automatique et maintenance facilitée
  - Compatible avec GitHub Actions / pipelines CI

---

## 🌐 7. Frontend Blazor

- UI 100% en composants Razor réutilisables
- Appels via `HttpClient` à l’API JSON
- Stockage local (localStorage / IndexedDB) pour les préférences et les tokens
- Compatibilité mobile (responsive design)
- Accessibilité : respect des normes WCAG 2.0 AA
- Composants : fiche de tomate, formulaire d’encodage, galerie photo, votes & commentaires

---

## 🌍 8. Aspire et Orchestration

- **.NET Aspire** utilisé pour :
  - Configuration multi-services
  - Observabilité (logs, traces, métriques)
  - Déploiement et orchestration locale ou cloud
- Configuration centralisée de tous les projets
- Point d’entrée unique pour `Tomatoz.Server`

---

## ✅ Résumé

| Domaine                | Solution retenue                    |
|------------------------|-------------------------------------|
| Authentification       | ASP.NET Identity + providers OIDC   |
| UI                     | Blazor WebAssembly/Server           |
| API                    | ASP.NET Core + JSON + Scalar        |
| Données                | MSSQL + EF Core                     |
| Couche métier          | Tomatoz.Application                 |
| Tests unitaires        | MSTest + Shouldly                   |
| Tests UI               | TestRigor                           |
| Logging                | OpenTelemetry + ILogger             |
| Orchestration          | .NET Aspire                         |
| Documentation API      | Scalar avec protection API Key      |

---
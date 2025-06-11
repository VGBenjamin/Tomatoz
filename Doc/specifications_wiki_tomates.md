# Spécifications Fonctionnelles – Wiki Collaboratif sur les Tomates

**Version : 1.0**  
**Phase : 1 – Wiki**  
**Auteur : Vangansewinkel Benjamin
**Date : 11 juin 2025

---

## 1. Objectifs du projet

Créer une plateforme collaborative permettant la création, la consultation et la modération de fiches détaillées sur les variétés de tomates. À terme, cette plateforme sera évolutive pour intégrer d'autres plantes potagères, mais cette première phase se concentre exclusivement sur les tomates.

---

## 2. Fonctionnalités principales

### 2.1 Consultation et navigation

- Affichage public des fiches de tomates.
- Moteur de recherche libre (nom, description, mot-clé).
- Filtres dynamiques :
  - Type 
  - Couleur
  - Forme
  - Résistance aux maladies
  - Période de récolte
- Affichage en liste ou grille.
- Vue détaillée de la fiche avec :
  - Données botaniques
  - Photos
  - Avis (notation multiple : goût, facilité de culture, etc.)
  - Historique des modifications (versions validées + refusées de l’auteur)
- Système de vote (utilité de la fiche, qualité des données)

### 2.2 Fiche de variété

#### Champs principaux
- Nom commun
- Nom scientifique
- Type de croissance
- Hauteur moyenne
- Forme, couleur
- Poids du fruit (fourchette)
- Taille du fruit (fourchette)
- Productivité
- Précocité
- Rendement estimé (kg/plante)
- Type de chair
- Texture
- Résistance aux maladies
- Période de culture (semis, repiquage, récolte)
- Conseils de culture
- Histoire, anecdotes, origine
- Tags généraux

#### Média
- Photos par fiche, typées (plant, fruit, tranches, etc.)
- Tags et descriptions facultatives

---

## 3. Contribution collaborative

### 3.1 Édition collaborative

- Interface en mode formulaire multi-étapes pour :
  - Création
  - Proposition de modification
- Indicateur de complétude de la fiche
- Attribution de tous les auteurs ayant contribué
- Versioning strict :
  - Chaque proposition crée une nouvelle version avec statut
  - Affichage uniquement des versions validées par défaut

### 3.2 Workflow d’approbation

- État "Proposé" → "Validé" ou "Refusé"
- Validation par pair ou administrateur
- Historique visible selon le contexte :
  - En lecture : versions validées + versions refusées de l’utilisateur courant
  - En validation : toutes les propositions
- Raison de refus obligatoire
- Souscription possible aux changements sur une fiche

---

## 4. Modération

- Rôles :
  - Utilisateur : lecture, proposition
  - Contributeur vérifié : vote, approbation
  - Modérateur : validation, édition
  - Administrateur : gestion globale
- Signalement d’erreur simplifié pour débutants
- Back-office pour les statuts et les propositions
- Possibilité de commenter les erreurs sans modifier

---

## 5. Gamification et communauté

- Points attribués pour :
  - Création de fiche : +10
  - Ajout de photo : +5
  - Validation : +8
  - Vote utile : +1
- Badges de contribution (ex. : Botaniste, Jardinier du mois…)
- Tableau de classement
- Notifications pour :
  - Changement de statut des propositions
  - Fiches suivies

---

## 6. Responsivité et accessibilité

- Design responsive (mobile-first)
- Accessibilité conforme WCAG 2.0 AA

---

## 7. Multilinguisme

- Multilingue prévu dès l’architecture
- Langue de base : FR
- EN prévu rapidement

---

## 8. Licence de contenu

- Licence ouverte de type Creative Commons BY-NC-SA
- Protection contre réutilisation commerciale sans autorisation

---

## 9. Architecture technique – Points d’attention

- Architecture modulaire et extensible à d’autres types de plantes
- Versioning en base de données par ligne/version/statut
- API REST/JSON prévue dès le début
- Stockage optimisé des médias
- Sécurité contre abus et spams
- Futur module IA pour agrégation et reformulation de contenu

---
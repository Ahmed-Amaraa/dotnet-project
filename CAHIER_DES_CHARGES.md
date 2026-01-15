# Cahier des Charges — InventoryManagement.Web

## 1. Contexte
La société souhaite disposer d'un portail web unique pour piloter l'inventaire (produits, fournisseurs, stocks) et fournir aux équipes entrepôt & achats une vision consolidée des niveaux de stock, alertes et actions à engager.

## 2. Objectifs généraux
- Centraliser toutes les opérations de gestion d'inventaire dans une application ASP.NET Core.
- Offrir un tableau de bord synthétique avec indicateurs clés (ruptures, tendance de consommation, tâches de réapprovisionnement).
- Garantir un suivi fiable des fournisseurs et des commandes associées.
- Assurer une base de tests automatisés couvrant les services critiques.

## 3. Périmètre fonctionnel
1. **Gestion des produits**
   - CRUD produits avec catégorisation, prix, seuil de réapprovisionnement.
   - Import/export CSV pour synchronisation ponctuelle.
2. **Gestion des stocks**
   - Ajustements manuels (entrées/sorties) avec justification.
   - Calcul automatique du statut (normal, à surveiller, critique) selon seuils.
3. **Gestion des fournisseurs**
   - Fiche fournisseur (coordonnées, délais, conditions de paiement).
   - Historique des commandes et du respect des SLA.
4. **Tableau de bord**
   - KPI cartes : total articles, ruptures imminentes, commandes en attente.
   - Liste actionnable des produits sous seuil + liens rapides vers création de commande.
5. **Authentification & rôles**
   - Basé sur ASP.NET Core Identity.
   - Rôles prévus : Admin, Gestionnaire Stocks, Visiteur lecture seule.

## 4. Périmètre technique
- Framework : .NET 8 / ASP.NET Core MVC + Entity Framework Core.
- Base de données : SQL Server (hébergement Azure SQL cible).
- UI : Razor Pages + Bootstrap (kit approuvé) + scripts Chart.js pour les KPI.
- CI/CD : Azure DevOps (build dotnet + déploiement Web App).
- Tests : xUnit + FluentAssertions sur `InventoryManagement.Web.Tests`.

## 5. Exigences fonctionnelles détaillées
| ID | Description | Priorité | Critère d'acceptation |
|----|-------------|----------|-----------------------|
| F1 | Créer/éditer un produit avec validations (nom unique, seuil positif). | Haute | Formulaire bloque la sauvegarde si validations KO et message explicite. |
| F2 | Visualiser le stock en temps réel sur la page produit. | Haute | Après un ajustement, la valeur affichée est mise à jour sans rechargement complet (AJAX). |
| F3 | Générer une liste des articles sous seuil classée par fournisseur. | Haute | Tableau exportable en CSV, filtrable par catégorie. |
| F4 | Enregistrer une commande fournisseur associée à plusieurs produits. | Moyenne | Création guidée, enregistre la date prévue de livraison. |
| F5 | Tableau de bord avec KPI + graphiques hebdomadaires. | Moyenne | Les données reflètent les mouvements des 7 derniers jours. |
| F6 | Historiser toute modification d'un stock avec horodatage et utilisateur. | Haute | Audit trail consultable dans l'onglet "Historique". |

## 6. Exigences non fonctionnelles
- **Performance** : temps de réponse < 1s pour 95% des requêtes internes; test de charge ciblé sur 5 000 produits.
- **Sécurité** : chiffrement TLS, gestion des secrets via Azure Key Vault, journalisation des connexions.
- **Disponibilité** : objectif 99,5% grâce aux slots de déploiement.
- **Accessibilité** : conformité WCAG 2.1 AA sur les écrans principaux.
- **Observabilité** : instrumentation Application Insights (traces, métriques stock critique).

## 7. Contraintes
- Appuyé exclusivement sur technologies Microsoft supportées LTS (pas de dépendances exotiques).
- Reprise partielle des données actuelles via fichiers CSV fournis par l'équipe métier.
- Déploiement sur abonnement Azure mutualisé → fenêtre de déploiement coordonnée (slot hebdo).
- Budget temps limité à 9 sprints hebdomadaires (cf. planification).

## 8. Organisation & responsabilités
| Rôle | Responsabilités principales |
|------|---------------------------|
| Product Owner | Arbitrer les priorités, valider les US & livrables. |
| Dev A (Backend) | Modèles EF, services, API sécurisées. |
| Dev B (Front) | Razor views, composants UI, accessibilité. |
| Dev C (Qualité/Tooling) | Stratégie de tests, pipelines CI, observabilité. |
| QA | Campagnes de tests exploratoires & régression. |

## 9. Planning de référence
Le projet suit 9 semaines séquencées :
1. Cadrage & préparation.
2. Couche données prête.
3. Services métiers.
4. API publiques.
5. Première passe UI.
6. Intégration & durcissement.
7. Sprint QA.
8. Buffer / préparation release.
9. UAT + mise en production.

## 10. Livrables
- Code source complet (frontend + backend) sur référentiel Git.
- Documentation technique (architecture, schémas DB, runbooks).
- Jeux de données de référence + scripts de seed.
- Rapport de tests automatisés + plan de tests QA.
- Script/pipeline de déploiement Azure DevOps.

## 11. Critères de réception
- Toutes les exigences F1–F6 validées par le Product Owner.
- Tous les tests automatisés passent en CI; couverture minimale de 75% sur les services.
- Aucun bug bloquant ou majeur ouvert avant mise en production.
- Documentation mise à jour et approuvée (README, cahier de recette, runbook).
- Preuve de déploiement sur environnement de préproduction avec monitoring actif.

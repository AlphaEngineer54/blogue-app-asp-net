# Projet RedSocial App

RedSocial App est une application web développée en **ASP.NET Core**, conçue pour offrir une plateforme sociale avec gestion de blogs, commentaires, votes, et un système robuste d'authentification utilisateur. Le projet s'appuie sur **Entity Framework Core** pour la gestion des données et intègre plusieurs services clés pour assurer la sécurité, la performance et la gestion des sessions.

---

## Fonctionnalités principales

- **Authentification sécurisée**  
  Implémentation via `IPasswordHasher` assurant un hachage robuste des mots de passe.

- **Gestion des contenus**  
  Services modulaires pour blogs, commentaires et votes.

- **Protection CSRF**  
  Intégration d’un système Antiforgery configuré.

- **Limitation du débit**  
  Mise en place d’un système de **Rate Limiting**.

- **Sessions sécurisées**  
  Gestion des sessions via cookies sécurisés, avec support des environnements Docker.

- **Sérialisation JSON avancée**  
  Utilisation de `Newtonsoft.Json` pour une meilleure flexibilité des échanges JSON.

---

## Prérequis techniques

- **.NET 6.0 ou supérieur**
- **SQL Server**
- **Visual Studio 2022** ou équivalent

---

## Configuration

### 1. Variables d’environnement

- Définissez la variable `BLOGUE_DB_CONNECTION_STRING` qui doit contenir la chaîne de connexion complète à votre base de données SQL Server (cloud ou local). Exemple :

```bash
export BLOGUE_DB_CONNECTION_STRING="Server=localhost;Database=RedSocialDB;User Id=sa;Password=VotreMotDePasse;"
```

- Copier-là dans un fichier .env

### 2. Lancement de l'application avec Docker
  - Compiler l'image
     ```bash
        docker build -t red-social-app . 
   - Créer le conteneur
      ```bash
         docker run -d -p 5000:5000 --name red-social-app \
            --env-file .env \
            red-social-app 

## Utilisation

- Accédez à l’application via https://localhost:5000 (ou le port configuré).
- Créez un compte utilisateur, publiez des blogs, commentez et votez.
- Les protections CSRF, la limitation de requêtes et la gestion des sessions sont activées par défaut.


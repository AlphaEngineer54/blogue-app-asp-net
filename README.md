# 📝 RedSocial – Application de blogue sociale  
![Status](https://img.shields.io/badge/status-Terminé-brightgreen)  
![Framework](https://img.shields.io/badge/framework-ASP.NET%20Core-blue)  
![Database](https://img.shields.io/badge/database-SQLServer-purple)  
![License](https://img.shields.io/badge/license-MIT-lightgrey)

---

## 🚀 À propos du projet

**RedSocial App** est une application web moderne développée en **ASP.NET Core**, pensée pour offrir une plateforme sociale intuitive et sécurisée. Elle permet aux utilisateurs de publier des blogs, commenter, voter et interagir dans un environnement protégé et performant.

🔧 Le projet repose sur **Entity Framework Core** pour la gestion des données, et intègre plusieurs services clés pour garantir :

- 🔐 Sécurité renforcée  
- ⚡ Performance optimisée  
- 🧠 Gestion intelligente des sessions  
---

## ✨ Fonctionnalités principales

### 🔐 Authentification sécurisée
- Hachage robuste des mots de passe via `IPasswordHasher`

### 📝 Gestion des contenus
- Services modulaires pour **blogs**, **commentaires** et **votes**

### 🛡️ Protection CSRF
- Intégration d’un système **Antiforgery** configuré

### 🚦 Limitation du débit
- Système de **Rate Limiting** pour éviter les abus

### 🍪 Sessions sécurisées
- Gestion via **cookies sécurisés**, compatible avec Docker

### 🔄 Sérialisation JSON avancée
- Utilisation de `Newtonsoft.Json` pour une flexibilité maximale

---

## 🧰 Prérequis techniques

- 💻 **.NET 6.0 ou supérieur**  
- 🗄️ **SQL Server**  
- 🧪 **Visual Studio 2022** ou équivalent

---
## ⚙️ Configuration

### 1️⃣ Variables d’environnement

Définissez la variable `BLOGUE_DB_CONNECTION_STRING` contenant la chaîne de connexion SQL Server. Exemple :

```bash
export BLOGUE_DB_CONNECTION_STRING="Server=localhost;Database=RedSocialDB;User Id=sa;Password=VotreMotDePasse;"
```

➡️ Copier-la dans un fichier `.env`

---

### 2️⃣ Lancement de l'application avec Docker

#### 🛠️ Compiler l'image
```bash
docker build -t red-social-app .
```

#### 🚀 Créer le conteneur
```bash
docker run -d -p 5000:5000 --name red-social-app \
  --env-file .env \
  red-social-app
```

---

## 🌐 Utilisation

- Accédez à l’application via [https://localhost:5000](https://localhost:5000) (ou le port configuré)  
- Créez un compte utilisateur, publiez des blogs, commentez et votez  
- ✅ Les protections **CSRF**, la **limitation de requêtes** et la **gestion des sessions** sont activées par défaut

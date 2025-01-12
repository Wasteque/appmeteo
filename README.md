# AppMeteo ⛅

## Description
Cette application météo, développée en C# avec .NET et Avalonia, utilise l'API d'OpenWeatherMap pour fournir des informations météorologiques en temps réel. Elle offre une interface graphique intuitive et coloré permettant aux utilisateurs de :
1. Rechercher la météo d'une ville.
2. Visualiser la météo avec un super gradient RGB.

---

## Fonctionnalités

### Recherche d'une ville
- Nom de la ville.
- Température en °C.
- Description succincte du temps (ex. : ciel dégagé).
- Humidité.
- Prevision meteo.
---

## Prérequis
Avant d'exécuter l'application :

1. Obtenir une clé API d'OpenWeatherMap :
   - [Site officiel](https://openweathermap.org/api).
   - La clé doit être stockée dans un fichier de configuration exclu du répertoire Git.

2. Installer le package NuGet **Newtonsoft.Json** :
   ```bash
   dotnet add package Newtonsoft.Json
   ```

3. Installer Avalonia pour la création de l'interface graphique :
   - [Documentation Avalonia](https://docs.avaloniaui.net).

---

## Installation et Exécution

1. Clonez le dépôt Git :
   ```bash
   git clone [https://github.com/Wasteque/appmeteo.git]
   cd appmeteo
   ```
2. Clé API :
    * Créer un fichier `api_key.txt` Avec votre clé API d'OpenWeatherMap.

3. Exécutez le projet :
   ```bash
   dotnet run
   ```

---

## Structure du Projet

### Fichiers principaux
- `Program.cs` : Point d'entrée de l'application.
- `api_key.txt` : Fichier de configuration API (non inclus dans le dépôt).
- `MainWindow.axaml.cs` : Service de communication avec l'API.
- `MainWindow.axaml.cs` : Composants de l'interface utilisateur.

---

## Précautions
- La clé API est unique et confidentielle : **ne pas la partager ni la commiter dans le dépôt.**


---

## Liens Utiles

- OpenWeatherMap :
  - [API Documentation](https://openweathermap.org/api)
  - [Guide OpenWeather](https://openweathermap.org/guide)

- Newtonsoft.Json :
  - [Site officiel](https://www.newtonsoft.com/json)

- Avalonia :
  - [Documentation officielle](https://docs.avaloniaui.net/)

## Auteurs

* Ethan/Wasteque - Développeur Principal - [Wasteque](https://github.com/Wasteque)

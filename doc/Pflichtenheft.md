# Pflichtenheft (Rohat, Joudi, Florian)

## Softwarevoraussetzungen 
- .Net (8.0)
- Serilog (4.3.0)
- Serilog.Sinks.Console (6.0.0)
- Serilog.Sinks.File (7.0.0)
- ExtendedWPFToolkit (4.7.25)
- Mapsui3.UI.Wpf (3.0.0)
- MicrosoftWeb.WebView2 (1.0.33)
- PresentationFramework (4.6.0)
- PresentationFramework (1.0.1)
- SharpVectors.Reloaded (1.8.4.2)
- SharpVectors.Wpf (1.8.4.2)
- SkiaSharp.Views.WPF (3.119.0)

## Funktionsblockdiagramm (von ChatGPT erklärt)
![Diagramm](diagramm.svg)

## Detaillierte Beschreibung
  Wenn man das Programm startet, sieht man zuerst den Login Screen, solange man einen Account hat, kann man direkt loslegen. Wenn nicht, kann man sich Registrieren, die Registration benötigt den Vor- und Nachnamen, die email und ein Passwort.
  </br>

  Nun kommt man auf den Homescreen in welchem, die Events präsentiert werden. Mit der Suchzeilekönnen die Events nach Kategorie, Name und anderen Attributen gefiltert werden. Das Programm im generellen wird durch die Sidebar gesteuert, welche sich an der linken Seite des Bildschirms befindet.
  </br>

  Auf den Event Objekten befindet sich der Titel, ein Bild, ein Knopf zum Favorisieren, das Datum, die Uhrzeit, und der Button zur Detailansicht.
  </br>

  In der Detailansicht befindet sich, neben den schon genannten Dingen auch eine Karte, um genau zu wissen, wo sich das Event befindet, und noch eine Beschreibung, welche einem mehr über die Attraktion erzählt.
  </br>

  Auf der Entdecken Seite findet man Events, welche in den nächsten Tagen stattfinden werden. Die Entdecken Seite, ist gleich aufgebaut, wie die HomePage um eine vereinheitlichte Erfahrung zu schaffen.
  </br>

  Unter Favoriten, findet man seine, userspezifische, Liste an Events. Um genau die zu finden, welche man besuchen möchte.
  </br>

  Bei Map sieht man eine Karte, welche die genauen Standorte der Events beinhaltet um so zu sehen, was in der Umgebung so vor sich geht.
  </br>

  Mit der Profil Seite kann man seine Nutzerdaten, nach belieben, anpassen und sich auch wieder Abmelden.

## Umsetzung
  Wir haben begonnen mit der Ideenfindung, nachdem wir unsere idee (Events in Vorarlberg finden) bekommen haben, fingen wir an das Design in Figma, sowie die Technologie, welche wir nutzen werden, zu bestimmen. Wir haben WPF als Framework genommen, da wir schon einige Erfahrung in WPF gesammelt haben und das den Prozess des Entwickelns, sowie der Fehlersuche erheblich vereinfachen würde. Für Supabase haben wir uns entschieden 1. da die Dokumentation gut war 2. die API verbindung zum Projekt nicht kompliziert war und 3. der Service, bis zu einem gewissen Zeitpunkt, gratis ist. Nachdem alle Technolgien ausgewählt worden sind, haben wir uns and das Backend gesetzt, hier haben wir uns für Swagger mit einem Python-Flask Webserver entschieden, da wir dank dem DBI Unterricht schon wussten wie das ganze funktioniert. Nachdem das Design bestimmt war, begann die Arbeit am Backend, da es den Zugang zur API noch nicht gab, lag der Fokus eher auf die Verbindung zwischen der Datenbank und dem Swagger Server sowie den Endpoints, welche benötigt werden. Gleichzeitig startete auch die Arbeit am Frontend Projekt, so gut wie es geht haben wir das Design, sowie den Grundaufbau der Pages gemacht, gleichzeitig implimentierten wir die Map, da diese nicht mit der Event API zusammenhing. Nachdem wir Zugang zur API bekommen haben. Haben wir die Daten aus der API gezogen und nun konnte die Entwicklung richtig beginnen. Nachdem wir alle Event daten bekamen setzten wir diese ein. Zum Schluss haben wir noch einige Verfeinerungen am Code, sowie dem Design gemacht und UnitTests und Logging eingebaut um den Code zu testen und zu debuggen.

## Probleme und Ihre Lösung
1. **Event-API**
    Problem:
        Wir haben keinen Zugang bekommen zu API und als wir den Zugang bekommen haben, war der durchblick nicht wirklich gegeben.
    Fix:
        Dank Herrn Nesensohn bekamen wir den Zugang zur API und Dank Herrn Bechtold konnten wir die JSON-Dumps analysieren und verarbeiten.
2. **NuGet Package WatermarkTextBox**
    Problem:
        Das Package war grundsätzlich gut, aber es hatte einige Designschwierigkeiten, welche uns nicht gefallen haben.
    Fix:
        Eigene Placeholder Logik eingebaut, um mehr Kontrolle über das Design zu haben.
3. **Map-Bilder**
    Problem:
        Die Map GPS Bilder konnten nicht geladen werden, da es Probleme mit dem Pfad gab.
    Fix:
        Wir haben eigene GPS Bilder improvisiert im Code.
4. **Zeit**
    Problem:
        Wir hatten nicht genug Zeit für unser Projekt, da wir einige Prüfungen hatten und auch hatten wir einen späteren Start, als letztes Jahr.
    Fix:
        Wir haben das beste aus der Zeit gemacht, welche wir hatten und haben uns auf die wichtigsten Features konzentriert, um ein funktionierendes Projekt zu haben.
        
## Wie wurde die Software getestet?
  Die Software wurde zum Teil manuell getestet, indem wir das Programm gestartet haben und die Funktionen ausprobiert haben. Wir haben auch Unit-Tests geschrieben, um sicherzustellen, dass die wichtigsten Funktionen funktionieren. Außerdem haben wir Logging eingebaut, um Fehler zu protokollieren und zu debuggen.

## Bedienungsanleitung

### 1. Start und Login
Nach dem Start der Anwendung erscheint der Login-Screen. Hier gibst du deine E-Mail und dein Passwort ein. Falls du noch keinen Account hast, klicke auf „Registrieren“ und fülle das Formular aus.

![Login](ProgrammScreenshots/LoginScreenshot.png)
![Registrieren](ProgrammScreenshots/RegistrierenScreenshot.png)

### 2. Home & Events entdecken
Nach dem Login landest du auf der Startseite (Home). Hier werden dir aktuelle Events angezeigt. Über die Suchleiste kannst du nach Namen, Kategorie oder anderen Attributen filtern. Die Navigation erfolgt über die Sidebar links. Hier kannst einzelne Events favorisieren, indem du auf das Stern-Symbol klickst. Favorisierte Events findest du später unter "Favoriten".

![Home](ProgrammScreenshots/HomeScreenshot.png)

### 3. Event-Details
Klicke bei einem Event auf den "Mehr erfahren"-Button, um die Detailansicht zu öffnen. Dort findest du weitere Informationen, ein Bild, das Datum, die Uhrzeit, eine Karte mit dem Standort und eine ausführliche Beschreibung. Bei einem Event kannst du es favorisieren, indem du auf das Stern-Symbol klickst. So kannst du es später bei deinen Favoriten leichter wiederfinden. Wenn der Stern 

![Mehr erfahren](ProgrammScreenshots/MehrErfahrenScreenshot.png)

### 4. Entdecken
Auf der Entdecken-Seite findest du Events, die in den nächsten Tagen stattfinden mit Zeit-Filter Möglichkeiten. Die Seite ist ähnlich wie die Startseite aufgebaut. Du kannst auch hier natürlich Events favorisieren und Details ansehen.

![Entdecken](ProgrammScreenshots/EntdeckenScreenshot.png)

### 5. Favoriten
Unter Favoriten findest du deine gespeicherten Events. So kannst du schnell auf deine persönlichen Highlights zugreifen.

![Favoriten](ProgrammScreenshots/FavoritenScreenshot.png)

### 6. Karte
Die Map-Seite zeigt dir alle Events auf einer Karte an. So siehst du sofort, was in deiner Umgebung los ist. Man kann die Karte zoomen und verschieben, um verschiedene Bereiche zu erkunden. Die Event-Standorte sind durch Marker gekennzeichnet. Wenn du auf einen Marker klickst, öffnet wirst du zu den Details des Events weitergeleitet.

![Map](ProgrammScreenshots/MapScreenshot.png)

### 7. Profil
Im Profil kannst du deine Benutzerdaten ändern und dich abmelden.

![Profil](ProgrammScreenshots/ProfilScreenshot.png)

---

Mit diesen Schritten kannst du die Anwendung einfach bedienen und alle Funktionen nutzen. Die Screenshots geben dir einen schnellen Überblick über die wichtigsten Seiten.

## Quellen
- Bilder der Events wurden von der Event-API von Vorarlberg geladen.
  - Durch folgenden API Link: [EventAPILink](https://v-cloud.vorarlberg.travel/api/v4/endpoints/b24513ef-acbb-4d9b-8cdc-eda44787baee?token=aed0e815dc2374d59cfc2e9f397a8653)
- Die Icons wurden von [Flaticon](https://www.flaticon.com/) geladen.
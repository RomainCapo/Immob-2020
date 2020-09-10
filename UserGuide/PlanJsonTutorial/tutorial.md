# Explication du plan JSON
* *Immob-2020*
* *Romain Capocasale, Jonas Freiburghaus et Vincent Moulin*
* *Cours d'infographie*
* *He-Arc, INF3dlm-a*
* *2019-2020*

Le plan de l'appartement est représenté à l'aide du format JSON. Ce document décris la structure à respecter pour que la génération de l'appartement se déroule de la bonne manière. 

## Structure générale
![Global](Global.png)

* **wallHeight :** la hauteur des murs de l'appartement *(en métres)*
* **wallWidth :** l'épaisseur des murs de l'appartement *(en métres)*
* **windowH1 :** la hauteur des murs en dessous des fenêtres *(en métres)*
* **windowH2 :** la hauteur des murs en dessus des fenêtres *(en métres)*
* **entryPoint :** point de départ de la caméra *(tableau composé de 2 coordonnées [x et z])*
* **segments :** tableau contenant la liste des segments qui compose l'appartement *(détaillé en dessous)*
* **areas :** tableau contenant la liste des zones de l'appartement *(détaillé en dessous)*
* **doors :** tableau contenant la liste des zones des portes *(détaillé en dessous)*

## Segments de l'appartement
Le tableau de segments est composé d'une liste d'objet JSON.

![Segments](Segments.png)

* **name :** nom du segment
* **start :** point de départ du segment *(tableau composé de 2 coordonnées [x et z])*
* **stop :** point de d'arrivé du segment *(tableau composé de 2 coordonnées [x et z])*
* **window :** vide si le segment ne contient pas de fenêtre, si le segment contient une fenêtre il faut préciser via un objet JSON.
  * **start :** point de départ de la fenêtre *(tableau composé de 2 coordonnées [x et z])*
  * **stop :** point d'arrivé de la fenêtre *(tableau composé de 2 coordonnées [x et z])*

## Zones de l'appartement
Le tableau de zone est composé d'une liste d'objet JSON.

![Zones](Zones.png)

* **type :** type de la zone *(par exemple : "Room", "LivingRoom", "Bathroom", "Kitchen", ...)*
* **points :** tableau contenant une liste de 4 points définissant le rectangle que la zone va couvrir.
  * Chaque sous-tableau correspond à un point de la zone *(composé de 2 coordonnées [x et z])*
* **isLightOn :** indique si la lumière est allumé ou non dans la pièce *(valeur booléen)*

## Portes de l'appartement
Le tableau des portes de l'appartement est une liste d'objet JSON.

![Portes](Portes.PNG)

* **name :** nom de la porte
* **isFrontDoor :** indique si il s'agit de la porte d'entrée ou non *(valeur booléen)*
* **start :** point de départ de la porte *(tableau composé de 2 coordonnées [x et z])*
* **stop :** point d'arrivé de la porte *(tableau composé de 2 coordonnées [x et z])*

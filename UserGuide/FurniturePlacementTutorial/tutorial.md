# Guide pour le placement des meubles
* *Immob-2020*
* *Romain Capocasale, Jonas Freiburghaus et Vincent Moulin*
* *Cours d'infographie*
* *He-Arc, INF3dlm-a*
* *2019-2020*

Pour que le placement des meubles fonctionne correctement, il faut respecter certaines régles. Ce document décrit les différents aspects à respecter pour que le placement des meubles s'effectue de la bonne manière.

Tout d'abord, pour ajouter un nouveau meuble il faut que chaque meubles sois un prefab. Le meuble doit avoir le script ``FurnitureProperties.cs``. Dans ce script, il faut préciser les deux paramètres en entrée suivant :
* ``Furniture Scale Pack``:  Nom du pack auxquel appartient l'objet. Ceci permettera d'ajouter le bon facteur d'échelle au meuble. (S'il s'agit d'un nouveau pack de meubles il faudra ajouter le nouveau facteur d'échelle dans le script ``FurnitureProperties.cs``)
* ``Placement Type``: ``EveryWhere`` indique que le meuble peut se retrouver n'importe ou dans la chambre. ``AgainstTheWall`` indique que le meuble doit être contre un mur.

![FurnitureProperties](FurnitureProperties.PNG)

Ensuite, il faut que que la zone d'intêret du meuble (porte du four, du frigo, place du canapé, ...) soit orienté contre l'axe Z comme on peut le voir sur la capture :

![AxisXZ](AxisXZ.PNG)

Ceci permettera d'assurer que la zone d'intêret du meuble ne se place pas contre un mur.

Il faut également que le centre de gravité sois au centre de l'objet comme le montre la capture ci-dessous :

![Center](Center.PNG)

## Hiérarchie des dossiers
Le meuble doit être placé dans le dossier ``Resources`` de Unity. Dans ce dossier, il y un sous dossier par style de mobilier. Si le meuble provient d'un nouveau style de meuble il faudra alors créer un dossier avec le nom du style dans le répertoire ``Resources``. Dans le dossier correpondant au style de meuble se trouve plusieurs dossier pour chaque type de meubles. Voici la liste des meubles disponible pour le projet :  
  * DiningTable
  * Sofa
  * Cabinet
  * Desktop
  * Chair
  * Decoration
  * Bed
  * Oven
  * KitchenWashbin
  * Fridge
  * Toilet
  * BathroomWashbinWaterSource

Veuillez ajouter le meuble dans le dossier correspondant à sa catégorie. Si le dossier n'existe pas dans le style de meuble correpondant il est possible de le créer et d'ajouter le meuble à l'intérieur. Si vous voulez ajouter un nouveau style de meuble vous pouvez créer les dossier pour chaque type de meubles dont vous disposez.

Le placemement des meubles ce passe comme ci :
* Par exemple imaginons que l'on veuille placer dans la pièce courante une chaise du style ``Retro``.
* Le script va vérifier que le dossier ``Resources/Retro/Chair`` existe. Si c'est le cas, il va alors sélectionner un des prefabs de chaises dans ce dossier.
* Si le dossier ``Resources/Retro/Chair`` n'existe pas, le script va alors aller regarder dans le dossier ``Resources/Default`` qui contient des prefabs par défaut pour chaque type de meuble. Le script selectionnera alors une chaise dans ``Resources/Default/Chair``.
* Cette méthologie permet de définir un style de meuble sans forcement avoir tout les types de meubles à disposition.

## Fichier de Règles
Le fichier de régles est défini au format JSON et se trouve dans le dossier : ``JSON/furnitureRules.json``. Ce fichier permet d'indiquer quelle type de meubles dois se trouver dans quelle type de pièce et la quantité de type de meubles par pièces.

La structure du fichier est la suivante :

![Global](Global.PNG)

* **areaRules** : contient la liste des types de meubles pour chaque pièce.
* **furnitureConstraints** : contient la liste du nombre de type de meubles par pièce.

### Règles des pièces

La liste de type de meubles par pièce est composé d'un tableau d'objet JSON.

![AreaRules](AreaRules.PNG)

* **type** : Nom du type de la pièce (LivingRoom, Room, Kitchen, Bathroom et Corridor)
* **furnitureType** : Liste des types de meubles autorisé dans la pièce. La liste des meubles autorisé est la même qu'au chapitre : ``Hiérarchie des dossiers``. Les meubles en premier dans la liste auront une plus grande priorité de placement par rapport aux meubles suivants.

## Contraintes des meubles

La liste des contraintes des meubles est composé d'un tableau d'objets JSON.

![FurnitureType](FurnitureType.PNG)

* **type** : Nom du type de meubles. La liste des meubles autorisé est la même qu'au chapitre : ``Hiérarchie des dossiers``.
* **quantityConstraints** : indique le quantité du type de meuble spécifié dans la pièce. Les valeurs autorisés sont :
  * *OneToOne* : Le meuble dois se trouver une seule fois dans la pièce.
  * *ZeroToOne* : Le meuble dois se trouver zero ou une fois dans la pièce.
  * *OneToMany* : Le meuble dois se trouver une ou plusieurs dans la pièce.
  * *ZeroToMany* : Le meuble dois se trouver zero ou plusieurs fois dans la pièce.

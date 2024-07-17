https://github.com/AIChubar/GPTJRPG/tree/master

# Unity C# Scripts

Here, you can find a description of the scripts for this Unity project. All listed scripts can be found in this folder https://github.com/AIChubar/GPTJRPG/tree/master/Assets/Scripts.

There are 3 scenes in the GPT JRPG project:
- **Menu** - Main Menu scene with the [Game World](https://github.com/AIChubar/GPTJRPG?tab=readme-ov-file#gpt-jrpg-game) generation functionality.
- **Map** - This is the scene for all levels in the game.
- **Battle Scene** - Scene players are taken to when the battle begins. This scene is additive and is loaded on top of the **Map** scene and unloaded after the battle ends.

The scripts are distributed across several folders.

## [Tools](https://github.com/AIChubar/GPTJRPG/tree/master/Assets/Scripts/Tools)

This folder contains supplementary `MonoBehaviour` scripts that are used across several scenes. 

### AudioManager

This script contains and manages all sounds used in the game and can be accessed via a static instance in this class.

### GameEvents

This script contains and manages events used inside the **Map** and **Battle** scenes. It is accessed through the static instance of this class.

### GameEvents

This script contains various methods and fields that multiple other classes should access. It contains sprite atlases, information about the game's state, instances of various classes, sounds, and some universally accessed methods. It is accessed through the static instance of this class and exists in the **Map** and **Battle** scenes.

### ItemDrag

This class implements the `IEndDragHandler`, `IBeginDragHandler`, and `IDragHandler` interfaces, overriding their methods to define the desired behaviour for drag-and-drop actions inside.

### SceneController

This class manages scene-loading fading animation and makes multiple calls to `GameManager` instance to invoke several methods depending on which scene is loaded or unloaded. It can be accessed through the static instance and exists in the **Map** and **Battle** scenes.

### SwappingInterface

It is the parent class for the `GroupInfoHUD` and `RecruitSystem`. Provide the interface for swapping the `ItemDrag` game objects.

## UI

This folder contains scripts that manage the User Interface and store some data that is being displayed.

### Pause

`Pause` is the UI element that manages and contains instances of several other UI elements: `DialogueHUD`, `GameMessageController`, `GroupInfoHUD`, `QuestMenu`, and `RecruitSystem`. The instance of this class can be accessed through the `GameManager`.

### DialogueHUD

This script stores the dialogue information and manages the displayed dialogue. The UI element contains information about the enemy you have a dialogue with and the phrases that have already occurred. After the enemy phrase is displayed, the script waits until the player chooses one of the available phrase options (an object containing `DialogueButton`) and clicks on one of them. After the dialogue ends `OutcomeButtons` are displayed.

### DialogueButton

This UI element is initialized inside the `DialogueHUD`. It contains and displays information about the dialogue option to which it is referred.

### OutcomeButtons

A UI element that is displayed inside the `DialogueHUD`. Its methods, which are attached to buttons, invoke several different events from `GameEvents` and call `Pause` methods to open other UI elements.

### GameMessageController

This script manages the window, and different messages can appear when the game is started, completed, or lost.

### GroupInfoHUD

This class is inherited from the `SwappingInterface`. It swaps the positions of the `Hero` units and displays information about the items.

### RecruitSystem 

This class is inherited from the `SwappingInterface`. It swaps the positions of the `Hero` units and `WorldEnemy` units. Called from one of the `OutcomeButtons` methods.

### InfoHUD 

It is a universally used UI element that displays information about game objects containing the `ObjectForInfoHUD` script.

### KnowledgeBase

The script that controls the game Knowledge Base. It also controls the `QuestMenu` in-game HUD.

### ObjectFOrInfoHUD

Supplementary class that contains public fields which can be accessed from the `InfoHUD`.

### QuestMenu

This manages the quest system UI element, contains all `QuestButton`, and displays information about the quests they contain.

### QuestButton

It is the label of the quest it represents. The information about this quest is displayed upon clicking.

### UnitGroupInfo

Auxiliary script for `KnowledgeBase`. 

### UnitHUD

UI script that contains and displays information about the `JSONReader.UnitJSON`.

## MainMenuScripts

Scripts that are used by objects inside the **Menu** scene.

### MainMenu

The class that manages **Menu** scene UI elements. 

### ScriptRunner

This script calls the Powershell script, which leads to the creation of the new Game World that is represented by the `WordButton`, which is added inside the `MainMenu` UI element.

### WorldManager

Stores and manages existing in the game Game Worlds (`WorldButtons`). 

### WorldButton

It is the label of the Game World it represents. The corresponding Game World is chosen to be played upon clicking. 

## LevelScripts

Scripts that are used by objects inside the **Map** scene.

### JSONReader

The class that parses the chosen in **Menu** GameWorld from JSON to various structs is implemented inside this class. It also tweaks some values in the parsed objects using the data from others.

### Hero

The script manages the playable Protagonist Hero game object controls and logic. 

### LevelGenerator

Initialize all `WordEnemy` inside the **Map** scene. 

### WorldEnemy

Script for a game object that can interact with the `Hero` upon colliding, which makes a call to `Pause`, which in turn calls the `DialogueHUD`.

### ProceduralFloor

The script randomly changes random tiles with the given pattern.

### PaletteSelection

The script that swaps the palette assigned to a grid of this level is based on the Game World data.

### GameData

The script contains the data from `Hero` and `WorldEnemy`, which is required for the **Battle** scene setup.

## BattleScripts

Scripts that are used by objects inside the **Battle** scene.

### BattleSystem

The main **Battle** scene script contains and manages information about the state of the battle, `Units`, methods corresponding to `Unit` actions, etc.

### Unit

The script controls and contains the data about the unit participating in a battle. Has methods responsible for change of its parameters, like hp or armour, death animations, etc. 

### Battle Journal

The script manages a UI element that shows the log of the battle.

### AbilityIndication

The script that is responsible for animation that displays the `Unit` action.

### HealthBarUnit

Manages the health bar shown above `Unit`.

### FloatingText

Text UI element animation that is called when the `Unit` changes its health parameter.

### GridSystem

Class that contains `GridObjects` and assign `Unit` to them

### GridObject

Script for the battle grid tile that represents the position inside the **Battle** scene and holds the `Unit` assigned to this tile.

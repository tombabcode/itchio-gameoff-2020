# itchio-gameoff-2020

Project for [Game Off 2020 by Github](https://itch.io/jam/game-off-2020) on itch.io

## Core features

-   **Dimension**: 2D
-   **View**: Top
-   **Genre**: Action / Roguelike
-   **Gameplay**: Singleplayer
-   **Style**: Drawing

## Game features

-   Multiple levels (Moons)
-   Boss fights
-   Combat system (Melee and ranged)
-   Procedurally generated world and mazes
-   Items and weapons

## How to install (WIP)

1. Install .NET Core SDK and .NET Core Runtime from [Microsoft .NET Core](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. Download and unzip [application code](https://github.com/tombabcode/itchio-gameoff-2020/archive/main.zip)
3. Run Shell Script, depending on which platform you work on, `compile-win-x64.sh` or `compile-osx-x64.sh`
4. After succesfull compilation, the bin-<platform> folder should appear in the same directory
5. Go into bin-<platform> directory and run GameJam

## TODO List

### Development

Progress: 14.81% (4 of 27 done)

-   [ ] Map
	- [ ] World interactions
	- [ ] Procedurally world generation
		- [ ] Level (maze) generation
		- [ ] Level's objects generation
-	[ ] NPC/AI
	- [ ] Generation in the world
	- [ ] Interactions with the player
	- [ ] Combat logic
-   [ ] Player
	- [ ] Collision detection
	- [ ] Inventory
	- [ ] Statistics
-   [ ] Combat system
	- [ ] Melee
	- [ ] Ranged
	- [ ] Collision detection
-   [ ] Menus
	- [ ] Main menu
	- [ ] Settings submenu
	- [ ] Intro
	- [ ] Pause menu
	- [ ] Credits menu
	- [ ] Save/Load game menu
-   [x] Configuration
-   [x] Translations system
-   [x] Logging
	- [x] Console

### Art

Progress: 0.00% (- of - done)

-   [ ] Concept art

### Sound design

Progress: 50.0% (1 of 2 done)

-   [x] Button hover sound
-   [ ] Button click sound

## In-game commands

To turn on/off the console in-game you have to press tilde key (~)

`hide` - Hide the console

`exit` - Force quit. Closes the application immediately

`newgame [username]` - Starts a new game. You can pass `[username]` parameter which set player's name. If parameter is not set, the `Unknown` username will be set

## Credits

Software Developer: Tomasz Babiak [GitHub](https://github.com/tombabcode)

Sound Designer: ASwan [Website](https://aswan0400.wixsite.com/website-2)

Art: Oliver F.

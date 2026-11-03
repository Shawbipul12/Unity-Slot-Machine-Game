# Royal Jackpot – Unity Slot Machine Game

## Game Overview

Royal Jackpot is a classic three-reel slot machine game developed using Unity and C#.

The objective is to spin three reels and match all three symbols to win coins.

## Features

- Three spinning reels.
- Randomized symbol generation using RNG.
- Winning condition: all three reels display the same symbol.
- Smooth reel animations.
- Coin balance and payout system.
- Spin and restart controls.
- Browser-compatible WebGL build.
- Uses the graphical assets provided in the assignment.

## Game Rules

- Starting balance: 1,000 coins.
- Each spin costs 10 coins.
- Matching all three symbols awards 100 coins.
- If the symbols do not match, the player loses the spin cost.

## Technologies Used

- Unity 2022.3 LTS
- C#
- Unity UI
- Unity Coroutines
- WebGL
- Git and GitHub

## Project Structure

```text
Assets/
  Scripts/
  Scenes/
  Resources/

Build/
  WebGL/
    index.html
    Build/
    TemplateData/

Packages/
ProjectSettings/
README.md
```

## How to Play

1. Open the game.
2. Check your coin balance.
3. Click the Spin button.
4. Wait for all three reels to stop.
5. Match all three symbols to win 100 coins.
6. Use Restart Game to reset your balance.

## How to Run the Unity Project

1. Install Unity Hub.
2. Install Unity 2022.3 LTS.
3. Open the project through Unity Hub.
4. Open `Assets/Scenes/SlotMachine.unity`.
5. Press Play.

## How to Run the WebGL Build

The WebGL build is available in:

`Build/WebGL/`

Serve the folder using a local web server, then open its `index.html` through your browser.

For example:

```bash
py -m http.server 8000
```

## Development Approach

The game was developed using an object-oriented approach with separate responsibilities for game management, reel control, symbol selection, payouts and UI.

Randomized outcomes are generated independently of the reel presentation. After the reels stop, the game checks whether all three symbols match and updates the player's balance accordingly.

The project was tested in the Unity Editor and its WebGL build was tested in a browser.

## Author

Developed as part of a Unity Game Developer assignment.
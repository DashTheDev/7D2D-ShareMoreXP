# Share More XP
[![Latest Release](https://img.shields.io/github/v/release/DashTheDev/7D2D-ShareMoreXP)](https://github.com/DashTheDev/7D2D-ShareMoreXP/releases/latest)
<br>
![7D2D v2.6](https://img.shields.io/badge/7D2D-v2.6-brightgreen)
<br>
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A [7 Days to Die](https://store.steampowered.com/app/251570/7_Days_to_Die/) mod that enhances XP sharing.

- Gain XP from zombies killed by wood spikes, iron spikes and barbed wires! (All players in configurable radius get XP, split evenly by default)
- While in a party and within a configurable radius, gain shared XP from your teammate's Harvesting, Upgrading, Crafting, Selling, Looting and Repairing!
- Adjust shared XP percentages, turn off sharing of certain XP sources and many other configurable settings!

## Installation
Download the [latest release](https://github.com/DashTheDev/7D2D-ShareMoreXP/releases/latest), unzip and drop the `ShareMoreXP` folder into your game's `Mods` directory.

## Development Setup
1. Ensure you have [Visual Studio](https://visualstudio.microsoft.com/) and [7D2D](https://store.steampowered.com/app/251570/7_Days_to_Die/) installed
2. Clone the repository
3. Create a `Local.props` file in the root of the project with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <!-- Insert game path here (i.e Z:\SteamLibrary\steamapps\common\7 Days To Die) -->
        <GamePath>YOUR_GAME_PATH_GOES_HERE</GamePath>
    </PropertyGroup>
</Project>
```

4. Open the solution in [Visual Studio](https://visualstudio.microsoft.com/) and build.
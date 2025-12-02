# ElectraCITY
### A digital version of the ElectraCITY activity by the [Science and Engineering Challenge](https://www.newcastle.edu.au/college/engineering-science-environment/education/science-and-engineering-challenge/our-activities) 
---
## Overview
**Authors:** James Davies, Axel Ello <br>
**Game Engine:** Unity <br>
**Description:**
- Draw electricity from the generators to power the city! 
- Connect wires between buildings to extend the powerline and score points based on the powered building
- Challenge yourself with 3 levels of difficulty and options to remove different kinds of wires
## Project Structure of Main Files
**/Assets/Audio** - Audio files  <br>
**/Assets/Colorblind Effect** - Colorblind filter files  <br>
**/Assets/Images** - Miscellaneous sprites including UI elements and power generators  <br>
**/Assets/Images/BG Objects** - Background object sprites  <br>
**/Assets/Images/Consumers** - Consumer object sprites (buildings)  <br>
**/Assets/Prefabs** - Prefabricated game objects <br>
**/Assets/Resources** - Miscellaneous files, currently contains JSON for tutorial dialogue <br>
**/Assets/Scenes** - .Unity files for each game scene <br>
**/Assets/Scripts** - C# scripts <br>
## Prefabs
**BGObject** - Background objects for the main game scene. Composed of a blank square sprite. Upon starting a game session, a number of these objects are generated via Global.cs with a random sprite.
**Connector**
**Consumer**
**Generator**
**Global**
**OptionsMenu**
**UI**
## Scenes
## Known Bugs
- Colorblind filter does not apply correctly when using a URP asset in the render pipeline (URP currently removed from pipeline)
- 2D Light does not render correctly for generated consumer objects (URP removal means this is currently disabled by default)

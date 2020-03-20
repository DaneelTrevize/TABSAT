# TABSAT - They Are Billions Save Automation Tool

This tool is intended to help players backup, restore, and modify their save games.

Common use-cases would be:  
* Reverting to a game save prior to some irksome mistake, or extreme RNG;  
* Removing the relatively new Mutants from otherwise-Classic 800% difficulty games.  
* Enabling playing with the more Giants, Mutants, Uber-VOD and resource distribution of Caustic Lands while having the terrain theme of Frozen Highlands, Desert Wastelands, etc.

TABSAT 0.8 [usage demonstration animation](https://imgur.com/T5EU6IJ).

Simply download a Release and launch TABSAT.exe to be presented with the Graphical User Interface.

![UI 1](https://raw.githubusercontent.com/DaneelTrevize/TABSAT/master/screenshots/UI%201.png)

----
## Furthur development

For more information about the goals of this project, please see [TABSAT.txt](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT.txt).

For reporting bugs or providing suggestions (Feature Requests), please use the GitHub Issues system.  
There is also a simple [TODO.txt](https://github.com/DaneelTrevize/TABSAT/blob/master/TODO.txt) list of some considered changes.

----

## Modifying save files - How it works

This tool is formed of 2 parts, the main application: TABSAT.exe and a sister binary TABReflector.exe that is stored compressed within the main assembly. We also use the 3rd party Ionic.Zip.dll as found in the TAB install.

TAB saves are encrypted with user-unknown passwords, yet TAB itself evidently can access these files. This tool therefore ironically creates a *zombie* process instance of TAB to then coerce into helping extract and repack save files for our needs, before terminating it.

In the action of subverting TAB, the Reflector component is automatically temporarily copied into your TAB install directory and launched in another process, augmented with TAB components. The main application interacts with this process, and handles tidying the Reflector away once it is no longer needed or the app is closed.  
The name 'Reflector' comes from the introspection mechanism used by the augmented process to locate and invoke the TAB components.

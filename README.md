# TABSAT - They Are Billions Save Automation Tool

This tool is intended to help players backup, restore, and modify their save games.

Common use-cases would be:  
* Enabling playing with the Caustic Lands map generation (more Giants, Mutants, Uber-VOD and resources distribution) while having the terrain tiles & sound theme of Frozen Highlands, or Desert Wastelands, etc.  
* Removing the relatively new Mutants from the otherwise-Classic 800% difficulty Survival mode option.  
* Restarting a game save to day 0, to replay a rare layout, or to try a different strategy.  
* Reverting to a game save prior to some irksome mistake, or extreme RNG.  

[Guide to the Save Modification Features](https://github.com/DaneelTrevize/TABSAT/blob/master/Features.md) that make the game easier/different/harder, as well as the Auto-Backup functionality.

Usage Demonstration animations:  
[TABSAT 1.0](https://github.com/DaneelTrevize/TABSAT/blob/master/screenshots/Demo%203.mkv?raw=true).  

[Download a Release](https://github.com/DaneelTrevize/TABSAT/releases) and launch TABSAT.exe to be presented with the GUI (Graphical User Interface):

![UI 2 1](https://github.com/DaneelTrevize/TABSAT/blob/master/screenshots/UI%202%201.png)

----

## Non-Steam-based game owners

For those who purchased TAB outside of Steam, e.g. via Origin, you will need to pass TABSAT the path to your TAB install as a command line argument. There is a .bat file [here](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT/TABSAT/bin/x64/Release/TABSAT.bat) that can be modified to do so (and is also for seeing console output should you be experiencing other issues), and serves as an example for creating your own shortcut or integrating with a launcher.  
A second path argument can be passed to provide a non-default TAB saves location.

----

## Furthur development

For more information about the goals of this project, please see [TABSAT.txt](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT.txt).

For reporting bugs or providing suggestions (Feature Requests), please use the GitHub Issues system.  
There is also a simple [ideas list](https://github.com/DaneelTrevize/TABSAT/blob/master/TODO.txt) of some considered changes.

----

## Modifying save files - How it works

This tool is formed of 2 C# projects, the main application: TABSAT.exe and a sister binary TABReflector.exe that is stored compressed within the main assembly. We also use the 3rd party Ionic.Zip.dll as found in the TAB install.

TAB saves are Zipped and encrypted with user-unknown passwords, yet TAB itself evidently can access these files. TABSAT therefore ironically creates a *zombie* instance of TAB to then coerce into helping extract and repack save files for our needs, before terminating it.

In the action of subverting TAB, the Reflector component is automatically temporarily copied into your TAB install directory and launched in a second process which is augmented with TAB components. The main TABSAT application communicates with this process, and handles tidying the Reflector away once it is no longer needed or TABSAT is closed.  
The name 'Reflector' comes from [the introspection mechanism](https://docs.microsoft.com/en-us/dotnet/api/system.reflection?view=netframework-4.0) used by the augmented process to locate and invoke the TAB components within itself.

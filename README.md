# TABSAT - They Are Billions Save Automation Tool

This tool is intended to help players backup, restore, and modify their game save files.

Common use-cases would be:  
* Playing Survival mode with the Caustic Lands map generation (more Giants, Mutants, single-massive-VOD, e.t.c.) while having the terrain tiles & sound theme of Frozen Highlands, or Desert Wastelands.  
* Reverting to a game save prior to some player mistake, or extreme RNG with respect to noise activation of Mutants or Harpies, or swarm wave direction and pathing.  
* Restarting a game save to day 0 (if you save & exit then first so that a backup can be automatically made), to replay an interesting layout generated by yourself or other players.  
* Removing the relatively new Mutant zombies to experience the Classic 800% hardest difficulty Survival mode.  

Tool usage short animation:  
[TABSAT 1.0 in action](https://github.com/DaneelTrevize/TABSAT/blob/master/screenshots/Demo%203.mkv?raw=true).  

Here is a [Guide to the Save Modification Features](https://github.com/DaneelTrevize/TABSAT/blob/master/Features.md) that make the game easier/different/harder, as well as the Auto-Backup functionality.

[Download a Release](https://github.com/DaneelTrevize/TABSAT/releases) and launch TABSAT.exe to be presented with the Graphical User Interface:

![UI 2 1](https://github.com/DaneelTrevize/TABSAT/blob/master/screenshots/UI%202%201.png)

----

## Non-Steam-based game owners

For those who purchased TAB outside of Steam, e.g. via Origin, you will need to pass TABSAT the path to your TAB install as a command line argument. There is a .bat file [here](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT/TABSAT/bin/x64/Release/TABSAT.bat) that can be modified to do so (and is also for seeing console output should you be experiencing other issues), and serves as an example for creating your own shortcut or integrating with a launcher.  
A second path argument can be passed to provide a non-default TAB saves location.

----

## Furthur development

For more information about the goals of this project, please see [TABSAT.txt](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT.txt).

For reporting bugs or providing suggestions (Feature Requests), please use the [GitHub Issues](https://github.com/DaneelTrevize/TABSAT/issues?q=is%3Aissue+updated%3A%3E2023-01-01) system.  

There is also a simple [ideas list](https://github.com/DaneelTrevize/TABSAT/blob/master/TODO.txt) of some considered changes.

----

## Modifying save files - How it works

TAB saves are Zipped folders of XML files, and encrypted with user-unknown passwords, yet TAB itself evidently can regenerate these passwords and access these files.

TABSAT ironically launches a *zombie* incomplete instance of TAB to then coerce into helping extract and repack save files for our needs, before terminating it.  
The incomplete nature of this TAB instance is why it mistakenly presents a warning pop-up about corrupted game data files, rather than the main game window, but the data files are unchanged.

![Data Files Corrupted pop-up]](https://github.com/DaneelTrevize/TABSAT/blob/master/TABSAT/TABSAT/Resources/TAB_ignore_popup.png)

This tool is formed of 2 C# projects, the main application: TABSAT.exe and a sister binary TABReflector.exe that is stored compressed within the main assembly. We also use the 3rd party Ionic.Zip.dll as found in the TAB install.  
In the action of subverting TAB to modify save files, the Reflector component is automatically temporarily copied into your TAB install directory and launched in a second process which is augmented with TAB exe components.  
The main TABSAT application communicates with this process, and handles tidying the Reflector away once it is no longer needed or TABSAT is closed.

The name 'Reflector' comes from [the introspection mechanism](https://docs.microsoft.com/en-us/dotnet/api/system.reflection?view=netframework-4.0) used by the augmented process to locate and invoke the TAB components within itself.

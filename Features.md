# Save Modification Features

The following Save Modification options have been implemented, and are categorised below by how they affect the game difficulty:

## Make the game easier

* Removing all Mutants (i.e. as 800% was before June 2019).

* Relocating all Mutants, or Giants, to the position of the furthest Mutant, or Giant, either considering the whole map at once or on a per compass direction quadrant basis (i.e. moving all Northern Mutants to the Northern Giant most distant from the Command Center).

* Removing the 'Villages of Doom'.

* Removing all 'Fog of War', enabling players to see the terrain, VODs, reclaimable buildings (e.g. Radar Towers) and pickable items (e.g. Food Trucks).

* Removing the Fog in a circle centered on the Command Center, of variable tile/cell radius.

* Granting constant full map vision, as though you have 'The Beholder', or the final wave has begun.

* Adding to the Extra Resource Supplies levels of the Command Center.

* Granting free/bonus units and buildings, of variable number (e.g. 3 Rangers, or 1 Titan).

* Filling the Command Center and Warehouse storage with resources.  
N.b. This does not yet current account for Mayors that increase storage.

## Situational difficulty adjustments

* Replacing all Mutants with Giants, and vice versa.

* Resizing all VOD buildings to Dwellings (the smallest), Taverns, or City Halls (the largest), which should also affect resource drops.  
N.b. City Halls can spawn thousands more zombies and cause pathing issues, which leads to lag.

* Changing the map Theme. This not only changes the visuals and background sound, but also inherits the rules modifiers of the standard themes, such as Frozen Highlands slowing zombies & increasing power consumption, or Desert Wasteland increasing zombie speed and range of noise.  
See [the spreadsheets generated in the Modded Mayors repo](https://github.com/DaneelTrevize/Modded-Mayors/tree/master/Source) for the specific data from the MapThemes table.

## Make the game harder

* Enable Swarms (Waves) to come from several directions. Earlier swarms can come from any 2 cardinal directions at once, later swarms from any 3.  
N.b. The on-screen text and icons will represent all chosen directions, but the audio announcement will only name the first direction per swarm.

* Disabling Mayors. No new mayors will be granted for reaching colony population thresholds.

* Stack multiple copies of VOD buildings at their existing positions.

----

# Automatical Backup Features

'Active' game save file sets, those in the usual game saves location (`DRIVE:\Users\USERNAME\Documents\My Games\They Are Billions\Saves\`), can be monitored for changes by TABSAT, once you navigate to the 'Automatically Backup Save Files' User Interface tab.  
Save changes will be reported in the Log text area at the bottom of the UI.

Active saves for which there is not a matching Backup set found in the Backups folder (default `DRIVE:\Users\USERNAME\Documents\TABSAT\saves\`) can be selected and manually backed up in the UI, copying both the `.zxsav` and `.zxcheck` files of a given save set.  
The matching criteria is based upon a full file checksum, not just name and creation/modification timestamps. Checksumming may take in the order of 1 second per 100 save files, be those active or backup files, and the results are cached per TABSAT usage.

Active saves can also automatically be backed up, upon change detection, by toggling the top right 'Enable AutoBackup' button. The Log area will also notify when Automatic Backup is enabled or disabled.

Backups are versioned under their save name (for which there can be many corresponding saves per game, as well as name reuse between games), and then under the timestamp of when that save was last modified.

Backups can be selected and manually restored in the UI, again copying both paired files of a given save set.

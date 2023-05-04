# Save Modification Features

The following Save Modification options have been implemented, and are categorised below by how they affect the game difficulty:

## Make the game easier

* Relocating all Mutants, or Giants, to the position of the furthest Mutant, or Giant, either considering the whole map at once or on a per compass direction quadrant basis (i.e. moving all Northern Mutants to the Northern Giant most distant from the Command Center).

* Removing all 'Fog of War', enabling players to see the terrain, VODs, reclaimable buildings (e.g. Radar Towers) and pickable items (e.g. Food Trucks).

* Removing the Fog in a circle centered on the Command Center, of variable tile/cell radius.

* Granting constant full map vision, as though you have 'The Beholder', or the final wave has begun.

* Adding to the Extra Resource Supplies levels of the Command Center.

* Granting free/bonus units and buildings, of variable number (e.g. 3 Rangers, or 1 Titan).

* Filling the Command Center and Warehouse storage with resources.  
N.b. This does not account for Mayors that increase storage.

## Situational difficulty adjustments

* Replacing all Mutants with Giants, and vice versa.

* Resizing all VOD buildings to Dwellings (the smallest), Taverns, or City Halls (the largest), which should also affect resource drops.  
N.b. City Halls can spawn thousands more zombies and cause pathing issues, which leads to lag.

* Changing the map Theme. This not only changes the visuals and background sound, but also inherits the rules modifiers of the standard themes, such as Frozen Highlands slowing zombies & increasing power consumption, or Desert Wasteland increasing zombie speed and range of noise.  
See [the spreadsheets generated in the Modded Mayors repo](https://github.com/DaneelTrevize/Modded-Mayors/tree/master/Source) for the specific data from the MapThemes table.

* Scaling the zombie population by a decimal factor, on a per-type basis. This can be applied to the initially spawned map zombies that remain idle, and/or the later spawned zombies from swarms, VODs, and those aggrod by the player.  
E.g. Harpy x0.5 is a 50% chance to keep/remove each Harpy.  
Spitter x2.1 would duplicate each Spitter plus a 10% chance to triplicate them.

* Stack multiple copies of VOD buildings at their existing positions by a decimal factor, on a per-type basis.  
E.g. Dwellings x1.5 is a 50% chance to duplicate each Dwelling.

## Make the game harder

* Set the Swarms (Waves) to come according to the 50 Days Challenge timings.  
N.b. This does not add the 'Neighbouring Colony gifts' or decrease the zombie density to that of the official 50 Challenge custom game.

* Set the number of directions for Swarms to come from at once, from the pre-final swarm's default of 1, up to the final swarm's default of 4 (which is not reduced by these modifications).  
Swarms are defined in 2 sets, Earlier swarms which consist of Walkers and Runners, and Later swarms which also have Executives and Chubbies (Fatties).  
N.b. The on-screen text and icons will represent all current directions of a swarm, but the audio announcement will only name 1 direction per swarm.

* Disabling Mayors. No new mayors will be granted for reaching colony population thresholds.

* Removing neutral buildings and loot piles.
These are the reclaimable towers that start on the map, as well as the pickable items from that initial start and any subsequently destroyed VOD buildings.

----

# Automatical Backup Features

'Active' game save file sets, those in the usual game saves location (`DRIVE:\Users\USERNAME\Documents\My Games\They Are Billions\Saves\`), can be monitored for changes by TABSAT, once you navigate to the 'Automatically Backup Save Files' User Interface tab.  
Save changes will be reported in the Log text area at the bottom of the UI.

Active saves for which there is not a matching Backup set found in the Backups folder (default `DRIVE:\Users\USERNAME\Documents\TABSAT\saves\`) can be selected and manually backed up in the UI, copying both the `.zxsav` and `.zxcheck` files of a given save set.  
The matching criteria is based upon a full file checksum, not just name and creation/modification timestamps. Checksumming may take in the order of 1 second per 100 save files, be those active or backup files, and the results are cached per TABSAT usage.

Active saves can also automatically be backed up, upon change detection, by toggling the central 'Automatically Backup Save Files' checkbox. The Log area will also notify when Automatic Backup is enabled or disabled.

Backups are versioned under their save name (for which there can be many corresponding saves per game, as well as name reuse between games), and then under the timestamp of when that save was last modified.

Backups can be selected and manually restored in the UI, again copying both paired files of a given save set.

----

# Map Viewer

When a Save File is extracted as a step in the Modification process, the contents is unpacked into a new folder (default under `DRIVE:\Users\USERNAME\Documents\TABSAT\edits\`).  
These XML files contain enough of the state data layers from the game that a visual representation can be generated similar to the in-game minimap, with options to have the various layers shown, as well as derived values such as terrain navigability, and additional entity properties.

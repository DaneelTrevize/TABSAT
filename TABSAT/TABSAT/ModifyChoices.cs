
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TABSAT
{
    internal class ModifyChoices
    {
        public enum AreaChoices : byte
        {
            None,
            Everywhere,
            Sections,
            WithinRadius,
            BeyondRadius
        }

        public enum MutantChoices : byte
        {
            ReplaceWithGiants,
            ReplaceWithMutants,
            MoveToGiants,
            MoveToMutants,
            MoveToGiantsPerQuadrant,
            MoveToMutantsPerQuadrant
        }

        internal readonly AreaChoices PopulationArea;
        internal readonly byte PopulationSections;
        internal readonly byte PopulationRadius;
        internal readonly bool ScaleIdle;
        internal readonly bool ScaleActive;
        internal readonly byte PopulationScale;
        internal readonly SortedDictionary<LevelEntities.ScalableZombieGroups, byte> ScalableZombieGroupFactors;
        internal readonly byte GiantScale;
        internal readonly byte MutantScale;
        internal readonly AreaChoices VODArea;
        internal readonly byte VODSections;
        internal readonly byte VODRadius;
        internal readonly byte VODSmallScale;
        internal readonly byte VODMediumScale;
        internal readonly byte VODLargeScale;
        internal readonly ushort GiftFood;
        internal readonly ushort GiftEnergy;
        internal readonly ushort GiftWorkers;
        internal readonly byte GiftCount;
        internal readonly LevelEntities.GiftableTypes Gift;
        internal readonly AreaChoices MutantsArea;
        internal readonly byte MutantsSections;
        internal readonly byte MutantsRadius;
        internal readonly MutantChoices Mutants;
        internal readonly AreaChoices FogArea;
        internal readonly byte FogSections;
        internal readonly byte FogRadius;
        internal readonly bool FogShowFullVision;
        internal readonly bool FillGold;
        internal readonly bool FillWood;
        internal readonly bool FillStone;
        internal readonly bool FillIron;
        internal readonly bool FillOil;
        internal readonly bool FasterSwarms;
        internal readonly bool ChangeEasy;
        internal readonly SaveReader.SwarmDirections EasySwarms;
        internal readonly bool ChangeHard;
        internal readonly SaveReader.SwarmDirections HardSwarms;
        internal readonly bool ChangeTheme;
        internal readonly SaveReader.ThemeType Theme;
        internal readonly bool DisableMayors;
        internal readonly bool RemoveReclaimables;

        internal ModifyChoices(
            in AreaChoices PopulationArea,
            in byte PopulationSections,
            in byte PopulationRadius,
            in bool ScaleIdle,
            in bool ScaleActive,
            byte PopulationScale,
            in SortedDictionary<LevelEntities.ScalableZombieGroups, byte> ScalableZombieGroupFactors,
            in byte GiantScale,
            in byte MutantScale,
            in AreaChoices VODArea,
            in byte VODSections,
            in byte VODRadius,
            byte VODSmallScale,
            byte VODMediumScale,
            byte VODLargeScale,
            in ushort GiftFood,
            in ushort GiftEnergy,
            in ushort GiftWorkers,
            in byte GiftCount,
            in LevelEntities.GiftableTypes Gift,
            in AreaChoices MutantsArea,
            in byte MutantsSections,
            in byte MutantsRadius,
            in MutantChoices Mutants,
            in AreaChoices FogArea,
            in byte FogSections,
            in byte FogRadius,
            in bool FogShowFullVision,
            in bool FillGold,
            in bool FillWood,
            in bool FillStone,
            in bool FillIron,
            in bool FillOil,
            in bool FasterSwarms,
            in bool ChangeEasy,
            in SaveReader.SwarmDirections EasySwarms,
            in bool ChangeHard,
            in SaveReader.SwarmDirections HardSwarms,
            in bool ChangeTheme,
            in SaveReader.ThemeType Theme,
            in bool DisableMayors,
            in bool RemoveReclaimables
            )
        {
            this.PopulationArea = PopulationArea;
            this.PopulationSections = PopulationSections;
            this.PopulationRadius = PopulationRadius;
            this.ScaleIdle = ScaleIdle;
            this.ScaleActive = ScaleActive;
            this.PopulationScale = PopulationScale;
            this.ScalableZombieGroupFactors = ScalableZombieGroupFactors;
            this.GiantScale = GiantScale;
            this.MutantScale = MutantScale;
            this.VODArea = VODArea;
            this.VODSections = VODSections;
            this.VODRadius = VODRadius;
            this.VODSmallScale = VODSmallScale;
            this.VODMediumScale = VODMediumScale;
            this.VODLargeScale = VODLargeScale;
            this.GiftFood = GiftFood;
            this.GiftEnergy = GiftEnergy;
            this.GiftWorkers = GiftWorkers;
            this.GiftCount = GiftCount;
            this.Gift = Gift;
            this.MutantsArea = MutantsArea;
            this.MutantsSections = MutantsSections;
            this.MutantsRadius = MutantsRadius;
            this.Mutants = Mutants;
            this.FogArea = FogArea;
            this.FogSections = FogSections;
            this.FogRadius = FogRadius;
            this.FogShowFullVision = FogShowFullVision;
            this.FillGold = FillGold;
            this.FillWood = FillWood;
            this.FillStone = FillStone;
            this.FillIron = FillIron;
            this.FillOil = FillOil;
            this.FasterSwarms = FasterSwarms;
            this.ChangeEasy = ChangeEasy;
            this.EasySwarms = EasySwarms;
            this.ChangeHard = ChangeHard;
            this.HardSwarms = HardSwarms;
            this.ChangeTheme = ChangeTheme;
            this.Theme = Theme;
            this.DisableMayors = DisableMayors;
            this.RemoveReclaimables = RemoveReclaimables;
        }

        internal static bool modifySave( ModifyChoices choices, SaveEditor dataEditor, MainWindow.StatusWriterDelegate statusWriter )
        {
            void log( string text, AreaChoices area, Byte sections, Byte radius )
            {
                String areaText;
                switch( area )
                {
                    case AreaChoices.Everywhere:
                        areaText = "";
                        break;
                    case AreaChoices.Sections:
                        areaText = " within sections: " + formatSections( sections );
                        break;
                    case AreaChoices.WithinRadius:
                        areaText = " within cell range: " + radius;
                        break;
                    case AreaChoices.BeyondRadius:
                        areaText = " beyond cell range: " + radius;
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + area );
                }
                statusWriter( String.Format( text, areaText ) );
            }

            void logScale( String name, AreaChoices area, Byte sections, Byte radius, Byte scale )
            {
                log( "Scaling " + name + ( scale == 100 ? " per type" : " {0}" + scale + "% {0}" ), area, sections, radius );
            }

            try
            {
                // Zombie Population Scaling
                SaveReader.ItemInArea popArea = dataEditor.getItemInArea( choices.PopulationArea, choices.PopulationSections, choices.PopulationRadius );
                if( popArea != null )
                {
                    byte popScale = choices.PopulationScale;
                    // Log Idle/Active/Both using choices.ScaleIdle, choices.ScaleActive..?
                    if( popScale != 100U )
                    {
                        logScale( "Zombie population", choices.PopulationArea, choices.PopulationSections, choices.PopulationRadius, popScale );
                        dataEditor.scalePopulation( popScale, choices.ScaleIdle, choices.ScaleActive, popArea );
                    }
                    else if( choices.ScalableZombieGroupFactors.Any() )
                    {
                        logScale( "Zombie population", choices.PopulationArea, choices.PopulationSections, choices.PopulationRadius, popScale );
                        dataEditor.scalePopulation( choices.ScalableZombieGroupFactors, choices.ScaleIdle, choices.ScaleActive, popArea );
                    }
                    logScale( "Giant population", choices.PopulationArea, choices.PopulationSections, choices.PopulationRadius, choices.GiantScale );
                    dataEditor.scaleEntities( (UInt64) LevelEntities.HugeTypes.Giant, choices.GiantScale, popArea, true );
                    logScale( "Mutant population", choices.PopulationArea, choices.PopulationSections, choices.PopulationRadius, choices.MutantScale );
                    dataEditor.scaleEntities( (UInt64) LevelEntities.HugeTypes.Mutant, choices.MutantScale, popArea, true );
                }

                // VODs
                /*if( choices.ResizeVODs )
                {
                    statusWriter( "Replacing all VOD buildings with " + LevelEntities.vodSizesNames[choices.VodSize] + '.' );
                    dataEditor.resizeVODs( choices.VodSize );
                }
                else*/
                SaveReader.ItemInArea vodArea = dataEditor.getItemInArea( choices.VODArea, choices.VODSections, choices.VODRadius );
                if( vodArea != null )
                {
                    logScale( "Dwellings count", choices.VODArea, choices.VODSections, choices.VODRadius, choices.VODSmallScale );
                    dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingSmall, choices.VODSmallScale, vodArea );
                    logScale( "Taverns count", choices.VODArea, choices.VODSections, choices.VODRadius, choices.VODMediumScale );
                    dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingMedium, choices.VODMediumScale, vodArea );
                    logScale( "City Halls count", choices.VODArea, choices.VODSections, choices.VODRadius, choices.VODLargeScale );
                    dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingLarge, choices.VODLargeScale, vodArea );
                }

                // Command Center Extras
                if( choices.GiftFood != 0 || choices.GiftEnergy != 0 || choices.GiftWorkers != 0 )
                {
                    statusWriter( "Adding Command Center extra supplies,"
                        + ( choices.GiftFood > 0 ? " Food: +" + choices.GiftFood : "" )
                        + ( choices.GiftEnergy > 0 ? " Energy: +" + choices.GiftEnergy : "" )
                        + ( choices.GiftWorkers > 0 ? " Workers: +" + choices.GiftWorkers : "" )
                        + '.' );
                    dataEditor.addExtraSupplies( choices.GiftFood, choices.GiftEnergy, choices.GiftWorkers );
                }

                if( choices.GiftCount != 0 )
                {
                    statusWriter( "Gifting " + choices.GiftCount + "x " + LevelEntities.giftableTypeNames[choices.Gift] + "." );
                    dataEditor.giftEntities( choices.Gift, choices.GiftCount );
                }

                // Mutants
                SaveReader.InArea mutantsInArea = dataEditor.getInArea( choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                if( mutantsInArea != null )
                {
                    switch( choices.Mutants )
                    {
                        case MutantChoices.ReplaceWithGiants:
                            log( "Replacing all Mutants{0} with Giants.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.replaceHugeZombies( true, mutantsInArea );
                            break;
                        case MutantChoices.ReplaceWithMutants:
                            log( "Replacing all Giants{0} with Mutants.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.replaceHugeZombies( false, mutantsInArea );
                            break;
                        case MutantChoices.MoveToGiants:
                            log( "Relocating Mutants{0} to farthest Giant on the map.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.relocateMutants( true, false, mutantsInArea );
                            break;
                        case MutantChoices.MoveToMutants:
                            log( "Relocating Mutants{0} to farthest Mutant on the map.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.relocateMutants( false, false, mutantsInArea );
                            break;
                        case MutantChoices.MoveToGiantsPerQuadrant:
                            log( "Relocating Mutants{0} to farthest Giant per Compass quadrant if possible.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.relocateMutants( true, true, mutantsInArea );
                            break;
                        case MutantChoices.MoveToMutantsPerQuadrant:
                            log( "Relocating Mutants{0} to farthest Mutant per Compass quadrant.", choices.MutantsArea, choices.MutantsSections, choices.MutantsRadius );
                            dataEditor.relocateMutants( false, true, mutantsInArea );
                            break;
                        default:
                            throw new NotImplementedException( "Unimplemented choice: " + choices.Mutants );
                    }
                }

                // Fill Resource Storage
                if( choices.FillGold || choices.FillWood || choices.FillStone || choices.FillIron || choices.FillOil )
                {
                    statusWriter( "Filling storage for specified resources:"
                        + ( choices.FillGold ? " Gold;" : "" )
                        + ( choices.FillWood ? " Wood;" : "" )
                        + ( choices.FillStone ? " Stone;" : "" )
                        + ( choices.FillIron ? " Iron;" : "" )
                        + ( choices.FillOil ? " Oil;" : "" ) );
                    dataEditor.fillStorage( choices.FillGold, choices.FillWood, choices.FillStone, choices.FillIron, choices.FillOil );
                }

                // Fog of War
                switch( choices.FogArea )
                {
                    case AreaChoices.None:
                        break;
                    case AreaChoices.Everywhere:
                        if( choices.FogShowFullVision )
                        {
                            statusWriter( "Revealing the map." );
                            dataEditor.showFullMap();
                        }
                        else
                        {
                            statusWriter( "Removing all the fog." );
                            dataEditor.removeFogSections( MapNavigation.ALL_DIRECTIONS );
                        }
                        break;
                    case AreaChoices.Sections:
                        statusWriter( "Removing the fog within sections:" + formatSections( choices.FogSections ) );
                        dataEditor.removeFogSections( choices.FogSections );
                        break;
                    case AreaChoices.WithinRadius:
                        statusWriter( "Removing the fog within cell range: " + choices.FogRadius );
                        dataEditor.removeFog( choices.FogRadius, true );
                        break;
                    case AreaChoices.BeyondRadius:
                        statusWriter( "Removing the fog beyond cell range: " + choices.FogRadius );
                        dataEditor.removeFog( choices.FogRadius );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.FogArea );
                }

                // Swarms
                if( choices.FasterSwarms )
                {
                    statusWriter( "Setting swarms to 50 Days Challenge timings." );
                    dataEditor.fasterSwarms();
                }
                if( choices.ChangeEasy )
                {
                    statusWriter( "Setting earlier swarm directions to " + SaveReader.SwarmDirectionsNames[choices.EasySwarms] + '.' );
                    dataEditor.setSwarms( true, choices.EasySwarms );
                }
                if( choices.ChangeHard )
                {
                    statusWriter( "Setting later swarm directions to " + SaveReader.SwarmDirectionsNames[choices.HardSwarms] + '.' );
                    dataEditor.setSwarms( false, choices.HardSwarms );
                }

                // General Rules
                if( choices.ChangeTheme )
                {
                    statusWriter( "Changing Theme to " + SaveReader.themeTypeNames[choices.Theme] + '.' );
                    dataEditor.changeTheme( choices.Theme );
                }
                if( choices.DisableMayors )
                {
                    statusWriter( "Disabling Mayors." );
                    dataEditor.disableMayors();
                }
                if( choices.RemoveReclaimables )
                {
                    statusWriter( "Removing neutral buildings and loot piles." );
                    dataEditor.removeReclaimables();
                }

                dataEditor.save();
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( "Problem modifying save file: " + e.Message + Environment.NewLine + e.StackTrace );
                statusWriter( "Problem modifying save file: " + e.Message );
                return false;
            }
            return true;
        }

        private static String formatSections( byte sections )
        {
            StringBuilder sb = new StringBuilder();

            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.NORTHWEST ) )
            {
                sb.Append( " NW" );
            }
            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.NORTH ) )
            {
                sb.Append( " N" );
            }
            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.NORTHEAST ) )
            {
                sb.Append( " NE" );
            }

            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.WEST ) )
            {
                sb.Append( " W" );
            }

            sb.Append( " M" );

            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.EAST ) )
            {
                sb.Append( " E" );
            }

            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.SOUTHWEST ) )
            {
                sb.Append( " SW" );
            }
            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.SOUTH ) )
            {
                sb.Append( " S" );
            }
            if( MapNavigation.containsDirection( sections, MapNavigation.Direction.SOUTHEAST ) )
            {
                sb.Append( " SE" );
            }

            sb.Append( "." );

            return sb.ToString();
        }
    }
}


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
            string formatArea( in byte radius, in bool beyondNotWithin )    // Refactor into AreaSelectorControl..?
            {
                return ( radius == 0 ? "" : ( beyondNotWithin ? " beyond" : " within" ) + " cell range: " + radius );
            }

            void logAndScale( string name, byte scale, string areaText, Action<byte> modify, in bool overrideScale = false )
            {
                if( scale == 100 && !overrideScale )
                {
                    return;
                }
                statusWriter( "Scaling " + name + ( scale == 100 ? " per type" : " " + scale + "%" ) + areaText );
                modify.Invoke( scale );
            }

            void logAndModify( string text, string areaText, Action modify )
            {
                statusWriter( String.Format( text, areaText ) );
                modify.Invoke();
            }

            try
            {
                // Zombie Population Scaling
                byte popRadius = 0;
                bool popBNW = true;
                SaveReader.ItemInArea popArea = null;
                byte popScale = choices.PopulationScale;
                switch( choices.PopulationArea )
                {
                    case AreaChoices.None:
                        break;
                    case AreaChoices.Everywhere:
                        popArea = SaveReader.ItemEverywhere;
                        break;
                    case AreaChoices.WithinRadius:
                        popRadius = choices.PopulationRadius;
                        popBNW = false;
                        popArea = dataEditor.ItemInRadiusArea( popRadius, popBNW );
                        break;
                    case AreaChoices.BeyondRadius:
                        popRadius = choices.PopulationRadius;
                        popArea = dataEditor.ItemInRadiusArea( popRadius, popBNW );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.PopulationArea );
                }
                if( popArea != null )
                {
                    // Log Idle/Active/Both using choices.ScaleIdle, choices.ScaleActive..?
                    if( popScale != 100U )
                    {
                        logAndScale( "Zombie population", popScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scalePopulation( s, choices.ScaleIdle, choices.ScaleActive, popArea ); } );
                    }
                    else if( choices.ScalableZombieGroupFactors.Any() )
                    {
                        logAndScale( "Zombie population", popScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scalePopulation( choices.ScalableZombieGroupFactors, choices.ScaleIdle, choices.ScaleActive, popArea ); }, true );
                    }
                    logAndScale( "Giant population", choices.GiantScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scaleEntities( (UInt64) LevelEntities.HugeTypes.Giant, s, popArea, true ); } );
                    logAndScale( "Mutant population", choices.MutantScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scaleEntities( (UInt64) LevelEntities.HugeTypes.Mutant, s, popArea, true ); } );
                }

                // VODs
                /*if( choices.ResizeVODs )
                {
                    statusWriter( "Replacing all VOD buildings with " + LevelEntities.vodSizesNames[choices.VodSize] + '.' );
                    dataEditor.resizeVODs( choices.VodSize );
                }
                else*/
                byte vodRadius = 0;
                bool vodBNW = true;
                SaveReader.ItemInArea vodArea = null;
                switch( choices.VODArea )
                {
                    case AreaChoices.None:
                        break;
                    case AreaChoices.Everywhere:
                        vodArea = SaveReader.ItemEverywhere;
                        break;
                    case AreaChoices.WithinRadius:
                        vodRadius = choices.VODRadius;
                        vodBNW = false;
                        vodArea = dataEditor.ItemInRadiusArea( vodRadius, vodBNW );
                        break;
                    case AreaChoices.BeyondRadius:
                        vodRadius = choices.VODRadius;
                        vodArea = dataEditor.ItemInRadiusArea( vodRadius, vodBNW );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.VODArea );
                }
                if( vodArea != null )
                {
                    logAndScale( "Dwellings count", choices.VODSmallScale, formatArea( vodRadius, vodBNW ), ( s ) => { dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingSmall, s, vodArea ); } );
                    logAndScale( "Taverns count", choices.VODMediumScale, formatArea( vodRadius, vodBNW ), ( s ) => { dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingMedium, s, vodArea ); } );
                    logAndScale( "City Halls count", choices.VODLargeScale, formatArea( vodRadius, vodBNW ), ( s ) => { dataEditor.scaleEntities( (UInt64) LevelEntities.VODTypes.DoomBuildingLarge, s, vodArea ); } );
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
                byte mutantsRadius = 0;
                bool mutantsBNW = true;
                SaveReader.InArea mutantsInArea = null;
                switch( choices.MutantsArea )
                {
                    case AreaChoices.None:
                        break;
                    case AreaChoices.Everywhere:
                        mutantsInArea = SaveReader.Everywhere;
                        break;
                    case AreaChoices.WithinRadius:
                        mutantsRadius = choices.MutantsRadius;
                        mutantsBNW = false;
                        mutantsInArea = dataEditor.InRadiusArea( mutantsRadius, mutantsBNW );
                        break;
                    case AreaChoices.BeyondRadius:
                        mutantsRadius = choices.MutantsRadius;
                        mutantsInArea = dataEditor.InRadiusArea( mutantsRadius, mutantsBNW );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.MutantsArea );
                }
                if( mutantsInArea != null )
                {
                    switch( choices.Mutants )
                    {
                        case MutantChoices.ReplaceWithGiants:
                            logAndModify( "Replacing all Mutants{0} with Giants.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.replaceHugeZombies( true, mutantsInArea ); } );
                            break;
                        case MutantChoices.ReplaceWithMutants:
                            logAndModify( "Replacing all Giants{0} with Mutants.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.replaceHugeZombies( false, mutantsInArea ); } );
                            break;
                        case MutantChoices.MoveToGiants:
                            logAndModify( "Relocating Mutants{0} to farthest Giant on the map.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( true, false, mutantsInArea ); } );
                            break;
                        case MutantChoices.MoveToMutants:
                            logAndModify( "Relocating Mutants{0} to farthest Mutant on the map.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( false, false, mutantsInArea ); } );
                            break;
                        case MutantChoices.MoveToGiantsPerQuadrant:
                            logAndModify( "Relocating Mutants{0} to farthest Giant per Compass quadrant if possible.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( true, true, mutantsInArea ); } );
                            break;
                        case MutantChoices.MoveToMutantsPerQuadrant:
                            logAndModify( "Relocating Mutants{0} to farthest Mutant per Compass quadrant if possible.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( false, true, mutantsInArea ); } );
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
                            dataEditor.removeFog();
                        }
                        break;
                    case AreaChoices.Sections:
                        statusWriter( "Removing the fog within sections:" + AreaSelectorControl.formatSections( choices.FogSections ) );
                        dataEditor.removeFog( dataEditor.InSectionsArea( choices.FogSections ) );
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
    }
}

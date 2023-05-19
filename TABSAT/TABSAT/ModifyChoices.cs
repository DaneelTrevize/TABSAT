
using System.Collections.Generic;

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
        internal readonly byte PopulationRadius;
        internal readonly bool ScaleIdle;
        internal readonly bool ScaleActive;
        internal readonly byte PopulationScale;
        internal readonly SortedDictionary<LevelEntities.ScalableZombieGroups, byte> ScalableZombieGroupFactors;
        internal readonly byte GiantScale;
        internal readonly byte MutantScale;
        internal readonly AreaChoices VODArea;
        internal readonly byte VODRadius;
        internal readonly byte VODSmallScale;
        internal readonly byte VODMediumScale;
        internal readonly byte VODLargeScale;
        internal readonly ushort GiftFood;
        internal readonly ushort GiftEnergy;
        internal readonly ushort GiftWorkers;
        internal readonly ushort GiftCount;
        internal readonly LevelEntities.GiftableTypes Gift;
        internal readonly AreaChoices MutantsArea;
        internal readonly byte MutantsRadius;
        internal readonly MutantChoices Mutants;
        internal readonly AreaChoices FogArea;
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
            in byte PopulationRadius,
            in bool ScaleIdle,
            in bool ScaleActive,
            byte PopulationScale,
            in SortedDictionary<LevelEntities.ScalableZombieGroups, byte> ScalableZombieGroupFactors,
            in byte GiantScale,
            in byte MutantScale,
            in AreaChoices VODArea,
            in byte VODRadius,
            byte VODSmallScale,
            byte VODMediumScale,
            byte VODLargeScale,
            in ushort GiftFood,
            in ushort GiftEnergy,
            in ushort GiftWorkers,
            in ushort GiftCount,
            in LevelEntities.GiftableTypes Gift,
            in AreaChoices MutantsArea,
            in byte MutantsRadius,
            in MutantChoices Mutants,
            in AreaChoices FogArea,
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
            this.PopulationRadius = PopulationRadius;
            this.ScaleIdle = ScaleIdle;
            this.ScaleActive = ScaleActive;
            this.PopulationScale = PopulationScale;
            this.ScalableZombieGroupFactors = ScalableZombieGroupFactors;
            this.GiantScale = GiantScale;
            this.MutantScale = MutantScale;
            this.VODArea = VODArea;
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
            this.MutantsRadius = MutantsRadius;
            this.Mutants = Mutants;
            this.FogArea = FogArea;
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
    }
}

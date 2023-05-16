
using System.Collections.Generic;

namespace TABSAT
{
    internal class ModifyChoices
    {
        public enum AreaChoices
        {
            None,
            Everywhere,
            Sections,
            WithinRadius,
            BeyondRadius
        }

        public enum MutantChoices
        {
            ReplaceWithGiants,
            ReplaceWithMutants,
            MoveToGiants,
            MoveToMutants,
            MoveToGiantsPerQuadrant,
            MoveToMutantsPerQuadrant
        }

        internal readonly AreaChoices PopulationArea;
        internal readonly uint PopulationRadius;
        internal readonly bool ScaleIdle;
        internal readonly bool ScaleActive;
        internal readonly decimal PopulationScale;
        internal readonly SortedDictionary<LevelEntities.ScalableZombieGroups, decimal> ScalableZombieGroupFactors;
        internal readonly decimal GiantScale;
        internal readonly decimal MutantScale;
        internal readonly AreaChoices VODArea;
        internal readonly uint VODRadius;
        internal readonly decimal VODSmallScale;
        internal readonly decimal VODMediumScale;
        internal readonly decimal VODLargeScale;
        internal readonly uint Food;
        internal readonly uint Energy;
        internal readonly uint Workers;
        internal readonly uint GiftCount;
        internal readonly LevelEntities.GiftableTypes Gift;
        internal readonly AreaChoices MutantsArea;
        internal readonly uint MutantsRadius;
        internal readonly MutantChoices Mutants;
        internal readonly AreaChoices FogArea;
        internal readonly uint FogRadius;
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
            in AreaChoices pA,
            in uint pR,
            in bool sI,
            in bool sA,
            in decimal pS,
            in SortedDictionary<LevelEntities.ScalableZombieGroups, decimal> szgF,
            in decimal gS,
            in decimal mS,
            in AreaChoices vA,
            in uint vR,
            in decimal vS,
            in decimal vM,
            in decimal vL,
            in uint f,
            in uint e,
            in uint w,
            in uint gC,
            in LevelEntities.GiftableTypes g,
            in AreaChoices mA,
            in uint mR,
            in MutantChoices m,
            in AreaChoices fA,
            in uint fR,
            in bool fF,
            in bool fG,
            in bool fW,
            in bool fS,
            in bool fI,
            in bool fO,
            in bool fast,
            in bool cE,
            in SaveReader.SwarmDirections eS,
            in bool cH,
            in SaveReader.SwarmDirections hS,
            in bool cT,
            in SaveReader.ThemeType t,
            in bool dM,
            in bool rR
            )
        {
            PopulationArea = pA;
            PopulationRadius = pR;
            ScaleIdle = sI;
            ScaleActive = sA;
            PopulationScale = pS;
            ScalableZombieGroupFactors = szgF;
            GiantScale = gS;
            MutantScale = mS;
            VODArea = vA;
            VODSmallScale = vS;
            VODMediumScale = vM;
            VODLargeScale = vL;
            VODRadius = vR;
            Food = f;
            Energy = e;
            Workers = w;
            GiftCount = gC;
            Gift = g;
            MutantsArea = mA;
            MutantsRadius = mR;
            Mutants = m;
            FogArea = fA;
            FogRadius = fR;
            FogShowFullVision = fF;
            FillGold = fG;
            FillWood = fW;
            FillStone = fS;
            FillIron = fI;
            FillOil = fO;
            FasterSwarms = fast;
            ChangeEasy = cE;
            EasySwarms = eS;
            ChangeHard = cH;
            HardSwarms = hS;
            ChangeTheme = cT;
            Theme = t;
            DisableMayors = dM;
            RemoveReclaimables = rR;
        }
    }
}

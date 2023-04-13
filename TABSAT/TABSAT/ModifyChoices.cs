﻿
using System.Collections.Generic;
using static TABSAT.SaveReader;

namespace TABSAT
{
    internal class ModifyChoices
    {
        public enum MutantChoices
        {
            None,
            ReplaceWithGiants,
            ReplaceWithMutants,
            MoveToGiants,
            MoveToMutants,
            MoveToGiantsPerQuadrant,
            MoveToMutantsPerQuadrant
        }

        public enum FogChoices
        {
            None,
            All,
            Radius,
            Full
        }

        internal readonly decimal PopulationScale;
        internal readonly SortedDictionary<ScalableZombieGroups, decimal> ScalableZombieGroupFactors;
        internal readonly decimal GiantScale;
        internal readonly decimal MutantScale;
        internal readonly MutantChoices Mutants;
        internal readonly bool ResizeVODs;
        internal readonly VodSizes VodSize;
        internal readonly decimal SmallScale;
        internal readonly decimal MediumScale;
        internal readonly decimal LargeScale;
        internal readonly FogChoices Fog;
        internal readonly uint FogRadius;
        internal readonly uint Food;
        internal readonly uint Energy;
        internal readonly uint Workers;
        internal readonly uint GiftCount;
        internal readonly GiftableTypes Gift;
        internal readonly bool FillGold;
        internal readonly bool FillWood;
        internal readonly bool FillStone;
        internal readonly bool FillIron;
        internal readonly bool FillOil;
        internal readonly bool FasterSwarms;
        internal readonly bool ChangeEasy;
        internal readonly SwarmDirections EasySwarms;
        internal readonly bool ChangeHard;
        internal readonly SwarmDirections HardSwarms;
        internal readonly bool ChangeTheme;
        internal readonly ThemeType Theme;
        internal readonly bool DisableMayors;

        internal ModifyChoices(
            in decimal pS,
            in SortedDictionary<ScalableZombieGroups, decimal> szgF,
            in decimal gS,
            in decimal mS,
            in MutantChoices m,
            in bool rV,
            in VodSizes vS,
            in decimal sV,
            in decimal mV,
            in decimal lV,
            in FogChoices fog,
            in uint fR,
            in uint f,
            in uint e,
            in uint w,
            in uint gC,
            in GiftableTypes g,
            in bool fG,
            in bool fW,
            in bool fS,
            in bool fI,
            in bool fO,
            in bool fast,
            in bool cE,
            in SwarmDirections eS,
            in bool cH,
            in SwarmDirections hS,
            in bool cT,
            in ThemeType t,
            in bool dM
            )
        {
            PopulationScale = pS;
            ScalableZombieGroupFactors = szgF;
            GiantScale = gS;
            MutantScale = mS;
            Mutants = m;
            ResizeVODs = rV;
            VodSize = vS;
            SmallScale = sV;
            MediumScale = mV;
            LargeScale = lV;
            Fog = fog;
            FogRadius = fR;
            Food = f;
            Energy = e;
            Workers = w;
            GiftCount = gC;
            Gift = g;
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
        }
    }
}
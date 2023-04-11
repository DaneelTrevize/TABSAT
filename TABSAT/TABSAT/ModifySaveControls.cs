using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static TABSAT.SaveReader;

namespace TABSAT
{
    public partial class ModifySaveControls : UserControl
    {
        private bool automatedStateSetting;
        private readonly List<CheckBox> zombieScalingCheckBoxes;
        private readonly List<CheckBox> vodCheckBoxes;
        private readonly List<CheckBox> ccExtrasCheckBoxes;
        private readonly List<CheckBox> warehousesFillCheckBoxes;
        private readonly List<CheckBox> swarmsCheckBoxes;
        private readonly List<CheckBox> generalCheckBoxes;

        private static bool anyChecked( IList<CheckBox> boxes )
        {
            foreach( var c in boxes )
            {
                if( c.Checked )
                {
                    return true;
                }
            }
            return false;
        }

        public ModifySaveControls()
        {
            InitializeComponent();

            automatedStateSetting = false;

            zombieScalingCheckBoxes = new List<CheckBox>( 9 )
            {
                zombieScaleWeakCheckBox,
                zombieScaleMediumCheckBox,
                zombieScaleDressedCheckBox,
                zombieScaleStrongCheckBox,
                zombieScaleVenomCheckBox,
                zombieScaleHarpyCheckBox,
                zombieScaleCheckBox,
                zombieScaleGiantCheckBox,
                zombieScaleMutantCheckBox
            };
            vodCheckBoxes = new List<CheckBox>( 4 )
            {
                vodReplaceCheckBox,
                vodStackDwellingCheckBox,
                vodStackTavernsCheckBox,
                vodStackCityHallsCheckBox
            };
            ccExtrasCheckBoxes = new List<CheckBox>( 3 )
            {
                ccExtraFoodCheckBox,
                ccExtraEnergyCheckBox,
                ccExtraWorkersCheckBox
            };
            warehousesFillCheckBoxes = new List<CheckBox>( 5 )
            {
                warehousesFillWoodCheckBox,
                warehousesFillStoneCheckBox,
                warehousesFillIronCheckBox,
                warehousesFillOilCheckBox,
                warehousesFillGoldCheckBox
            };
            swarmsCheckBoxes = new List<CheckBox>( 3 )
            {
                swarmsFasterCheckBox,
                swarmsEasyCheckBox,
                swarmsHardCheckBox
            };
            generalCheckBoxes = new List<CheckBox>( 2 )
            {
                themeCheckBox,
                disableMayorsCheckBox
            };

            vodReplaceComboBox.DataSource = new BindingSource( vodSizesNames, null );
            vodReplaceComboBox.DisplayMember = "Value";
            vodReplaceComboBox.ValueMember = "Key";

            giftComboBox.DataSource = new BindingSource( giftableTypeNames, null );
            giftComboBox.DisplayMember = "Value";
            giftComboBox.ValueMember = "Key";

            swarmEasyComboBox.DataSource = new BindingSource( SwarmDirectionsNames, null );
            swarmEasyComboBox.DisplayMember = "Value";
            swarmEasyComboBox.ValueMember = "Key";
            swarmHardComboBox.DataSource = new BindingSource( SwarmDirectionsNames, null );
            swarmHardComboBox.DisplayMember = "Value";
            swarmHardComboBox.ValueMember = "Key";

            themeComboBox.DataSource = new BindingSource( themeTypeNames, null );
            themeComboBox.DisplayMember = "Value";
            themeComboBox.ValueMember = "Key";

            mutantReplaceAllComboBox.SelectedIndex = 0;
            mutantMoveWhatComboBox.SelectedIndex = 0;
            mutantMoveGlobalComboBox.SelectedIndex = 1;
            vodReplaceComboBox.SelectedIndex = 0;
            giftComboBox.SelectedIndex = 1;
            swarmEasyComboBox.SelectedIndex = 1;
            swarmHardComboBox.SelectedIndex = 2;
            themeComboBox.SelectedIndex = 3;

            // Register these following event handlers after the above 'default' index choices, to avoid handling non-user events.

            var comboHandler = new EventHandler( comboBox_SelectedIndexChanged );
            var numericHandler = new EventHandler( numericUpDown_ValueChanged );

            zombieScaleWeakCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleMediumCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleDressedCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleStrongCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleVenomCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleHarpyCheckBox.CheckedChanged += notZombieScaleCheckBox_CheckedChanged;
            zombieScaleCheckBox.CheckedChanged += zombieScaleCheckBox_CheckedChanged;
            zombieScaleGiantCheckBox.CheckedChanged += zombieScaleCheckBox_CheckedChanged;
            zombieScaleMutantCheckBox.CheckedChanged += zombieScaleCheckBox_CheckedChanged;

            zombieScaleWeakNumericUpDown.ValueChanged += numericHandler;
            zombieScaleMediumNumericUpDown.ValueChanged += numericHandler;
            zombieScaleDressedNumericUpDown.ValueChanged += numericHandler;
            zombieScaleStrongNumericUpDown.ValueChanged += numericHandler;
            zombieScaleVenomNumericUpDown.ValueChanged += numericHandler;
            zombieScaleHarpyNumericUpDown.ValueChanged += numericHandler;
            zombieScaleNumericUpDown.ValueChanged += numericHandler;
            zombieScaleGiantNumericUpDown.ValueChanged += numericHandler;
            zombieScaleMutantNumericUpDown.ValueChanged += numericHandler;


            mutantReplaceAllComboBox.SelectedIndexChanged += comboHandler;
            mutantMoveGlobalComboBox.SelectedIndexChanged += comboHandler;
            mutantMoveWhatComboBox.SelectedIndexChanged += comboHandler;


            vodReplaceCheckBox.CheckedChanged += vodReplaceCheckBox_CheckedChanged;
            vodStackDwellingCheckBox.CheckedChanged += notVodReplaceCheckBox_CheckedChanged;
            vodStackTavernsCheckBox.CheckedChanged += notVodReplaceCheckBox_CheckedChanged;
            vodStackCityHallsCheckBox.CheckedChanged += notVodReplaceCheckBox_CheckedChanged;

            vodReplaceComboBox.SelectedIndexChanged += comboHandler;

            vodStackDwellingsNumericUpDown.ValueChanged += numericHandler;
            vodStackTavernsNumericUpDown.ValueChanged += numericHandler;
            vodStackCityHallsNumericUpDown.ValueChanged += numericHandler;


            fogNumericUpDown.ValueChanged += numericHandler;


            ccFoodNumericUpDown.ValueChanged += numericHandler;
            ccEnergyNumericUpDown.ValueChanged += numericHandler;
            ccWorkersNumericUpDown.ValueChanged += numericHandler;

            giftComboBox.SelectedIndexChanged += comboHandler;
            giftNumericUpDown.ValueChanged += numericHandler;


            swarmEasyComboBox.SelectedIndexChanged += comboHandler;
            swarmHardComboBox.SelectedIndexChanged += comboHandler;


            themeComboBox.SelectedIndexChanged += comboHandler;
        }

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            var box = (ComboBox) sender;

            if( box == mutantReplaceAllComboBox )
            {
                mutantReplaceAllRadio.Checked = true;
            }
            else if( box == mutantMoveWhatComboBox || box == mutantMoveGlobalComboBox )
            {
                mutantsMoveRadio.Checked = true;
            }
            else if( box == vodReplaceComboBox )
            {
                vodReplaceCheckBox.Checked = true;
            }
            else if( box == giftComboBox )
            {
                ccGiftCheckBox.Checked = true;
            }
            else if( box == swarmEasyComboBox )
            {
                swarmsEasyCheckBox.Checked = true;
            }
            else if( box == swarmHardComboBox )
            {
                swarmsHardCheckBox.Checked = true;
            }
            else if( box == themeComboBox )
            {
                themeCheckBox.Checked = true;
            }
        }

        private void numericUpDown_ValueChanged( object sender, EventArgs e )
        {
            var num = (NumericUpDown) sender;

            if( num == zombieScaleNumericUpDown )
            {
                zombieScaleCheckBox.Checked = true;
                automatedStateSetting = true;
                zombieScaleWeakNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                zombieScaleMediumNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                zombieScaleDressedNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                zombieScaleStrongNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                zombieScaleVenomNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                zombieScaleHarpyNumericUpDown.Value = zombieScaleNumericUpDown.Value;
                automatedStateSetting = false;
            }
            else if( num == zombieScaleWeakNumericUpDown && !automatedStateSetting )
            {
                zombieScaleWeakCheckBox.Checked = true;
            }
            else if( num == zombieScaleMediumNumericUpDown && !automatedStateSetting )
            {
                zombieScaleMediumCheckBox.Checked = true;
            }
            else if( num == zombieScaleDressedNumericUpDown && !automatedStateSetting )
            {
                zombieScaleDressedCheckBox.Checked = true;
            }
            else if( num == zombieScaleStrongNumericUpDown && !automatedStateSetting )
            {
                zombieScaleStrongCheckBox.Checked = true;
            }
            else if( num == zombieScaleVenomNumericUpDown && !automatedStateSetting )
            {
                zombieScaleVenomCheckBox.Checked = true;
            }
            else if( num == zombieScaleHarpyNumericUpDown && !automatedStateSetting )
            {
                zombieScaleHarpyCheckBox.Checked = true;
            }
            else if( num == zombieScaleGiantNumericUpDown )
            {
                zombieScaleGiantCheckBox.Checked = true;
            }
            else if( num == zombieScaleMutantNumericUpDown )
            {
                zombieScaleMutantCheckBox.Checked = true;
            }
            else if( num == vodStackDwellingsNumericUpDown )
            {
                vodStackDwellingCheckBox.Checked = true;
            }
            else if( num == vodStackTavernsNumericUpDown )
            {
                vodStackTavernsCheckBox.Checked = true;
            }
            else if( num == vodStackCityHallsNumericUpDown )
            {
                vodStackCityHallsCheckBox.Checked = true;
            }
            else if( num == fogNumericUpDown )
            {
                fogClearRadioButton.Checked = true;
            }
            else if( num == ccFoodNumericUpDown )
            {
                ccExtraFoodCheckBox.Checked = true;
            }
            else if( num == ccEnergyNumericUpDown )
            {
                ccExtraEnergyCheckBox.Checked = true;
            }
            else if( num == ccWorkersNumericUpDown )
            {
                ccExtraWorkersCheckBox.Checked = true;
            }
            else if( num == giftNumericUpDown )
            {
                ccGiftCheckBox.Checked = true;
            }
        }

        private void zombieScaleCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                zombieScaleWeakCheckBox.Checked = false;
                zombieScaleMediumCheckBox.Checked = false;
                zombieScaleDressedCheckBox.Checked = false;
                zombieScaleStrongCheckBox.Checked = false;
                zombieScaleVenomCheckBox.Checked = false;
                zombieScaleHarpyCheckBox.Checked = false;
            }
        }

        private void notZombieScaleCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                zombieScaleCheckBox.Checked = false;
            }
        }

        private void vodReplaceCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                vodStackDwellingCheckBox.Checked = false;
                vodStackTavernsCheckBox.Checked = false;
                vodStackCityHallsCheckBox.Checked = false;
            }
        }

        private void notVodReplaceCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                vodReplaceCheckBox.Checked = false;
            }
        }

        internal bool anyModificationChosen()
        {
            return anyChecked( zombieScalingCheckBoxes ) || !mutantsNothingRadio.Checked || anyChecked( vodCheckBoxes ) || !fogLeaveRadioButton.Checked
                || anyChecked( ccExtrasCheckBoxes ) || ccGiftCheckBox.Checked || anyChecked( warehousesFillCheckBoxes ) || anyChecked( swarmsCheckBoxes ) || anyChecked( generalCheckBoxes );
        }

        internal ModifyChoices getChoices()
        {
            // Zombie Population Scaling
            SortedDictionary<ScalableZombieGroups, decimal> scalableZombieGroupFactors = new SortedDictionary<ScalableZombieGroups, decimal>();
            if( zombieScaleWeakCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.WEAK, zombieScaleWeakNumericUpDown.Value );
            }
            if( zombieScaleMediumCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.MEDIUM, zombieScaleMediumNumericUpDown.Value );
            }
            if( zombieScaleDressedCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.DRESSED, zombieScaleDressedNumericUpDown.Value );
            }
            if( zombieScaleStrongCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.STRONG, zombieScaleStrongNumericUpDown.Value );
            }
            if( zombieScaleVenomCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.VENOM, zombieScaleVenomNumericUpDown.Value );
            }
            if( zombieScaleHarpyCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( ScalableZombieGroups.HARPY, zombieScaleHarpyNumericUpDown.Value );
            }

            // Mutants
            ModifyChoices.MutantChoices mutants = ModifyChoices.MutantChoices.None;
            if( mutantReplaceAllRadio.Checked )
            {
                bool toGiantNotMutant = mutantReplaceAllComboBox.SelectedIndex == 0;
                mutants = toGiantNotMutant ? ModifyChoices.MutantChoices.ReplaceWithGiants : ModifyChoices.MutantChoices.ReplaceWithMutants;
            }
            else if( mutantsMoveRadio.Checked )
            {
                bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                bool perDirection = mutantMoveGlobalComboBox.SelectedIndex == 1;
                if( perDirection )
                {
                    mutants = toGiantNotMutant ? ModifyChoices.MutantChoices.MoveToGiantsPerQuadrant : ModifyChoices.MutantChoices.MoveToMutantsPerQuadrant;
                }
                else
                {
                    mutants = toGiantNotMutant ? ModifyChoices.MutantChoices.MoveToGiants : ModifyChoices.MutantChoices.MoveToMutants;
                }
            }

            // VODs
            VodSizes vodSize = VodSizes.SMALL;
            if( vodReplaceCheckBox.Checked )
            {
                KeyValuePair<VodSizes, string> kv = (KeyValuePair<VodSizes, string>) vodReplaceComboBox.SelectedItem;
                vodSize = kv.Key;
            }

            // Fog of War
            ModifyChoices.FogChoices fog = ModifyChoices.FogChoices.None;
            uint fogRadius = 0;
            if( fogRemoveRadioButton.Checked )
            {
                fog = ModifyChoices.FogChoices.All;
            }
            else if( fogClearRadioButton.Checked )
            {
                fogRadius = Convert.ToUInt32( fogNumericUpDown.Value );
                fog = ModifyChoices.FogChoices.Radius;
            }
            else if( fogShowFullRadioButton.Checked )
            {
                fog = ModifyChoices.FogChoices.Full;
            }

            // Command Center Extras
            uint giftCount = 0;
            GiftableTypes gift = GiftableTypes.SoldierRegular;
            if( ccGiftCheckBox.Checked )
            {
                giftCount = Convert.ToUInt32( giftNumericUpDown.Value );
                KeyValuePair<GiftableTypes, string> kv = (KeyValuePair<GiftableTypes, string>) giftComboBox.SelectedItem;
                gift = kv.Key;
            }

            // Fill Resource Storage

            // Swarms
            SwarmDirections easy = SwarmDirections.ONE;
            SwarmDirections hard = SwarmDirections.ONE;
            if( swarmsEasyCheckBox.Checked )
            {
                KeyValuePair<SwarmDirections, string> kv = (KeyValuePair<SwarmDirections, string>) swarmEasyComboBox.SelectedItem;
                easy = kv.Key;
            }
            if( swarmsHardCheckBox.Checked )
            {
                KeyValuePair<SwarmDirections, string> kv = (KeyValuePair<SwarmDirections, string>) swarmHardComboBox.SelectedItem;
                hard = kv.Key;
            }

            // General Rules
            ThemeType theme = ThemeType.FA;
            if( themeCheckBox.Checked )
            {
                KeyValuePair<ThemeType, string> kv = (KeyValuePair<ThemeType, string>) themeComboBox.SelectedItem;
                theme = kv.Key;
            }

            return new ModifyChoices(
                zombieScaleCheckBox.Checked ? zombieScaleNumericUpDown.Value : 1,
                scalableZombieGroupFactors,
                zombieScaleGiantCheckBox.Checked ? zombieScaleGiantNumericUpDown.Value : 1,
                zombieScaleMutantCheckBox.Checked ? zombieScaleMutantNumericUpDown.Value : 1,
                mutants,
                vodReplaceCheckBox.Checked,
                vodSize,
                vodStackDwellingCheckBox.Checked ? vodStackDwellingsNumericUpDown.Value : 1,
                vodStackTavernsCheckBox.Checked ? vodStackTavernsNumericUpDown.Value : 1,
                vodStackCityHallsCheckBox.Checked ? vodStackCityHallsNumericUpDown.Value : 1,
                fog,
                fogRadius,
                ccExtraFoodCheckBox.Checked ? Convert.ToUInt32( ccFoodNumericUpDown.Value ) : 0,
                ccExtraEnergyCheckBox.Checked ? Convert.ToUInt32( ccEnergyNumericUpDown.Value ) : 0,
                ccExtraWorkersCheckBox.Checked ? Convert.ToUInt32( ccWorkersNumericUpDown.Value ) : 0,
                giftCount,
                gift,
                warehousesFillGoldCheckBox.Checked,
                warehousesFillWoodCheckBox.Checked,
                warehousesFillStoneCheckBox.Checked,
                warehousesFillIronCheckBox.Checked,
                warehousesFillOilCheckBox.Checked,
                swarmsFasterCheckBox.Checked,
                swarmsEasyCheckBox.Checked,
                easy,
                swarmsHardCheckBox.Checked,
                hard,
                themeCheckBox.Checked,
                theme,
                disableMayorsCheckBox.Checked
            );
        }

        internal void resetChoices()
        {
            List<List<CheckBox>> checkboxGroups = new List<List<CheckBox>>() { zombieScalingCheckBoxes, vodCheckBoxes, ccExtrasCheckBoxes, warehousesFillCheckBoxes, swarmsCheckBoxes, generalCheckBoxes };
            foreach( var g in checkboxGroups )
            {
                foreach( var c in g )
                {
                    c.Checked = false;
                }
            }
            mutantsNothingRadio.Checked = true;
            fogLeaveRadioButton.Checked = true;
            ccGiftCheckBox.Checked = false;
        }
    }
}

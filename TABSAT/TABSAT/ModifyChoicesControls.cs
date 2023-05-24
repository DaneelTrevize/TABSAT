using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static TABSAT.SaveReader;

namespace TABSAT
{
    public partial class ModifyChoicesControls : UserControl
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

        public ModifyChoicesControls()
        {
            InitializeComponent();

            automatedStateSetting = false;

            zombieScalingCheckBoxes = new List<CheckBox>( 9 )
            {
                zombieScaleAllCheckBox,
                zombieScaleGiantCheckBox,
                zombieScaleMutantCheckBox,
                zombieScaleWeakCheckBox,
                zombieScaleMediumCheckBox,
                zombieScaleDressedCheckBox,
                zombieScaleStrongCheckBox,
                zombieScaleVenomCheckBox,
                zombieScaleHarpyCheckBox
            };
            vodCheckBoxes = new List<CheckBox>( 4 )
            {
                vodDwellingCheckBox,
                vodTavernsCheckBox,
                vodCityHallsCheckBox
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
            generalCheckBoxes = new List<CheckBox>( 3 )
            {
                themeCheckBox,
                disableMayorsCheckBox,
                removeReclaimablesCheckBox
            };

            giftComboBox.DataSource = new BindingSource( LevelEntities.giftableTypeNames, null );
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

            mutantsReplaceAllComboBox.SelectedIndex = 0;
            mutantsMoveWhatComboBox.SelectedIndex = 0;
            mutantsMoveGlobalComboBox.SelectedIndex = 1;
            giftComboBox.SelectedIndex = 1;
            swarmEasyComboBox.SelectedIndex = 1;
            swarmHardComboBox.SelectedIndex = 2;
            themeComboBox.SelectedIndex = 3;

            // Register these following event handlers after the above 'default' index choices, to avoid handling non-user events.

            var comboHandler = new EventHandler( comboBox_SelectedIndexChanged );
            var numericHandler = new EventHandler( numericUpDown_ValueChanged );

            zombieScaleAllCheckBox.CheckedChanged += zombieScaleAllCheckBox_CheckedChanged;
            zombieScaleGiantCheckBox.CheckedChanged += zombieScaleHugeCheckBox_CheckedChanged;
            zombieScaleMutantCheckBox.CheckedChanged += zombieScaleHugeCheckBox_CheckedChanged;
            zombieScaleWeakCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;
            zombieScaleMediumCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;
            zombieScaleDressedCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;
            zombieScaleStrongCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;
            zombieScaleVenomCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;
            zombieScaleHarpyCheckBox.CheckedChanged += zombieScaleNotAllCheckBox_CheckedChanged;

            activeRadioButton.CheckedChanged += zombieScaleRadioButton_CheckedChanged;
            idleRadioButton.CheckedChanged += zombieScaleRadioButton_CheckedChanged;
            bothRadioButton.CheckedChanged += zombieScaleRadioButton_CheckedChanged;

            zombieScaleAllNumericUpDown.ValueChanged += numericHandler;
            zombieScaleGiantNumericUpDown.ValueChanged += numericHandler;
            zombieScaleMutantNumericUpDown.ValueChanged += numericHandler;
            zombieScaleWeakNumericUpDown.ValueChanged += numericHandler;
            zombieScaleMediumNumericUpDown.ValueChanged += numericHandler;
            zombieScaleDressedNumericUpDown.ValueChanged += numericHandler;
            zombieScaleStrongNumericUpDown.ValueChanged += numericHandler;
            zombieScaleVenomNumericUpDown.ValueChanged += numericHandler;
            zombieScaleHarpyNumericUpDown.ValueChanged += numericHandler;

            vodDwellingCheckBox.CheckedChanged += vod_CheckedChanged;
            vodTavernsCheckBox.CheckedChanged += vod_CheckedChanged;
            vodCityHallsCheckBox.CheckedChanged += vod_CheckedChanged;
            vodStackDwellingsNumericUpDown.ValueChanged += numericHandler;
            vodStackTavernsNumericUpDown.ValueChanged += numericHandler;
            vodStackCityHallsNumericUpDown.ValueChanged += numericHandler;

            mutantsReplaceAllRadio.CheckedChanged += mutants_CheckedChanged;
            mutantsMoveRadio.CheckedChanged += mutants_CheckedChanged;
            mutantsReplaceAllComboBox.SelectedIndexChanged += comboHandler;
            mutantsMoveGlobalComboBox.SelectedIndexChanged += comboHandler;
            mutantsMoveWhatComboBox.SelectedIndexChanged += comboHandler;

            fogRemoveRadioButton.CheckedChanged += fog_CheckedChanged;
            fogShowFullRadioButton.CheckedChanged += fog_CheckedChanged;

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

            if( box == mutantsReplaceAllComboBox )
            {
                mutantsReplaceAllRadio.Checked = true;
                mutantsAreaSelectorControl.ensureSomeArea();
            }
            else if( box == mutantsMoveWhatComboBox || box == mutantsMoveGlobalComboBox )
            {
                mutantsMoveRadio.Checked = true;
                mutantsAreaSelectorControl.ensureSomeArea();
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

            if( num == zombieScaleAllNumericUpDown )
            {
                zombieScaleAllCheckBox.Checked = true;
                automatedStateSetting = true;
                zombieScaleWeakNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
                zombieScaleMediumNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
                zombieScaleDressedNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
                zombieScaleStrongNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
                zombieScaleVenomNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
                zombieScaleHarpyNumericUpDown.Value = zombieScaleAllNumericUpDown.Value;
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
                zombieAreaSelectorControl.ensureSomeArea();
            }
            else if( num == zombieScaleMutantNumericUpDown )
            {
                zombieScaleMutantCheckBox.Checked = true;
                zombieAreaSelectorControl.ensureSomeArea();
            }
            else if( num == vodStackDwellingsNumericUpDown )
            {
                vodDwellingCheckBox.Checked = true;
                vodAreaSelectorControl.ensureSomeArea();
            }
            else if( num == vodStackTavernsNumericUpDown )
            {
                vodTavernsCheckBox.Checked = true;
                vodAreaSelectorControl.ensureSomeArea();
            }
            else if( num == vodStackCityHallsNumericUpDown )
            {
                vodCityHallsCheckBox.Checked = true;
                vodAreaSelectorControl.ensureSomeArea();
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

        private void zombieScaleAllCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                zombieScaleWeakCheckBox.Checked = false;
                zombieScaleMediumCheckBox.Checked = false;
                zombieScaleDressedCheckBox.Checked = false;
                zombieScaleStrongCheckBox.Checked = false;
                zombieScaleVenomCheckBox.Checked = false;
                zombieScaleHarpyCheckBox.Checked = false;
                zombieAreaSelectorControl.ensureSomeArea();
            }
        }

        private void zombieScaleNotAllCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                zombieScaleAllCheckBox.Checked = false;
                zombieAreaSelectorControl.ensureSomeArea();
            }
        }

        private void zombieScaleRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                zombieAreaSelectorControl.ensureSomeArea();
            }
        }

        private void zombieScaleHugeCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                zombieAreaSelectorControl.ensureSomeArea();
            }
        }

        private void vod_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (CheckBox) sender ).Checked )
            {
                vodAreaSelectorControl.ensureSomeArea();
            }
        }

        private void mutants_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mutantsAreaSelectorControl.ensureSomeArea();
            }
        }

        private void fog_CheckedChanged( object sender, EventArgs e )
        {
            if( fogRemoveRadioButton.Checked )
            {
                fogAreaSelectorControl.ensureSomeArea();
                fogAreaSelectorControl.enableRadiusChoice();
            }
            else
            {
                // fogShowFullRadioButton.Checked
                fogAreaSelectorControl.SetAreaEverywhere();
                fogAreaSelectorControl.disableRadiusChoice();
            }
        }

        internal bool anyModificationChosen()
        {
            return zombieAreaSelectorControl.AreaChoice() != ModifyChoices.AreaChoices.None
                || vodAreaSelectorControl.AreaChoice() != ModifyChoices.AreaChoices.None || anyChecked( ccExtrasCheckBoxes ) || ccGiftCheckBox.Checked
                || mutantsAreaSelectorControl.AreaChoice() != ModifyChoices.AreaChoices.None || anyChecked( warehousesFillCheckBoxes )
                || fogAreaSelectorControl.AreaChoice() != ModifyChoices.AreaChoices.None || anyChecked( swarmsCheckBoxes )
                || anyChecked( generalCheckBoxes );
        }

        internal ModifyChoices getChoices()
        {
            // Zombie Population Scaling
            SortedDictionary<LevelEntities.ScalableZombieGroups, byte> scalableZombieGroupFactors = new SortedDictionary<LevelEntities.ScalableZombieGroups, byte>();
            if( zombieScaleWeakCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.WEAK, Convert.ToByte( zombieScaleWeakNumericUpDown.Value ) );
            }
            if( zombieScaleMediumCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.MEDIUM, Convert.ToByte( zombieScaleMediumNumericUpDown.Value ) );
            }
            if( zombieScaleDressedCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.DRESSED, Convert.ToByte( zombieScaleDressedNumericUpDown.Value ) );
            }
            if( zombieScaleStrongCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.STRONG, Convert.ToByte( zombieScaleStrongNumericUpDown.Value ) );
            }
            if( zombieScaleVenomCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.VENOM, Convert.ToByte( zombieScaleVenomNumericUpDown.Value ) );
            }
            if( zombieScaleHarpyCheckBox.Checked )
            {
                scalableZombieGroupFactors.Add( LevelEntities.ScalableZombieGroups.HARPY, Convert.ToByte( zombieScaleHarpyNumericUpDown.Value ) );
            }

            // Mutants
            ModifyChoices.MutantChoices mutants;
            if( mutantsReplaceAllRadio.Checked )
            {
                bool toGiantNotMutant = mutantsReplaceAllComboBox.SelectedIndex == 0;
                mutants = toGiantNotMutant ? ModifyChoices.MutantChoices.ReplaceWithGiants : ModifyChoices.MutantChoices.ReplaceWithMutants;
            }
            else// if( mutantsMoveRadio.Checked )
            {
                bool toGiantNotMutant = mutantsMoveWhatComboBox.SelectedIndex == 0;
                bool perDirection = mutantsMoveGlobalComboBox.SelectedIndex == 1;
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

            // Fog of War

            // Command Center Extras
            byte giftCount = 0;
            LevelEntities.GiftableTypes gift = LevelEntities.GiftableTypes.SoldierRegular;
            if( ccGiftCheckBox.Checked )
            {
                giftCount = Convert.ToByte( giftNumericUpDown.Value );
                KeyValuePair<LevelEntities.GiftableTypes, string> kv = (KeyValuePair<LevelEntities.GiftableTypes, string>) giftComboBox.SelectedItem;
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
                zombieAreaSelectorControl.AreaChoice(),
                zombieAreaSelectorControl.Sections(),
                zombieAreaSelectorControl.Radius(),
                idleRadioButton.Checked || bothRadioButton.Checked,
                activeRadioButton.Checked || bothRadioButton.Checked,
                zombieScaleAllCheckBox.Checked ? Convert.ToByte( zombieScaleAllNumericUpDown.Value ) : (byte) (100U),
                scalableZombieGroupFactors,
                zombieScaleGiantCheckBox.Checked ? Convert.ToByte( zombieScaleGiantNumericUpDown.Value ) : (byte) (100U),
                zombieScaleMutantCheckBox.Checked ? Convert.ToByte( zombieScaleMutantNumericUpDown.Value ) : (byte) (100U),
                vodAreaSelectorControl.AreaChoice(),
                vodAreaSelectorControl.Sections(),
                vodAreaSelectorControl.Radius(),
                vodDwellingCheckBox.Checked ? Convert.ToByte( vodStackDwellingsNumericUpDown.Value ) : (byte) (100U),
                vodTavernsCheckBox.Checked ? Convert.ToByte( vodStackTavernsNumericUpDown.Value ) : (byte) (100U),
                vodCityHallsCheckBox.Checked ? Convert.ToByte( vodStackCityHallsNumericUpDown.Value ) : (byte) (100U),
                ccExtraFoodCheckBox.Checked ? Convert.ToUInt16( ccFoodNumericUpDown.Value ) : (ushort) (0U),
                ccExtraEnergyCheckBox.Checked ? Convert.ToUInt16( ccEnergyNumericUpDown.Value ) : (ushort) (0U),
                ccExtraWorkersCheckBox.Checked ? Convert.ToUInt16( ccWorkersNumericUpDown.Value ) : (ushort) (0U),
                giftCount,
                gift,
                mutantsAreaSelectorControl.AreaChoice(),
                mutantsAreaSelectorControl.Sections(),
                mutantsAreaSelectorControl.Radius(),
                mutants,
                fogAreaSelectorControl.AreaChoice(),
                fogAreaSelectorControl.Sections(),
                fogAreaSelectorControl.Radius(),
                fogShowFullRadioButton.Checked,
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
                disableMayorsCheckBox.Checked,
                removeReclaimablesCheckBox.Checked
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
            zombieScaleAllCheckBox.Checked = true;
            bothRadioButton.Checked = true;       // Should we also reset the numerous ComboBox.SelectedIndex and NumericUpDown.Value..?
            zombieAreaSelectorControl.reset();
            vodAreaSelectorControl.reset();
            mutantsAreaSelectorControl.reset();
            fogAreaSelectorControl.reset();
            ccGiftCheckBox.Checked = false;
        }
    }
}

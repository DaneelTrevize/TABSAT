using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    public partial class ModifySaveControls : UserControl
    {
        private readonly StatusWriterDelegate statusWriter;
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

        public ModifySaveControls( StatusWriterDelegate sW )
        {
            InitializeComponent();

            statusWriter = sW;

            automatedStateSetting = false;

            zombieScalingCheckBoxes = new List<CheckBox>( 9 );
            zombieScalingCheckBoxes.Add( zombieScaleWeakCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleMediumCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleDressedCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleStrongCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleVenomCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleHarpyCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleGiantCheckBox );
            zombieScalingCheckBoxes.Add( zombieScaleMutantCheckBox );
            vodCheckBoxes = new List<CheckBox>( 4 );
            vodCheckBoxes.Add( vodReplaceCheckBox );
            vodCheckBoxes.Add( vodStackDwellingCheckBox );
            vodCheckBoxes.Add( vodStackTavernsCheckBox );
            vodCheckBoxes.Add( vodStackCityHallsCheckBox );
            ccExtrasCheckBoxes = new List<CheckBox>( 3 );
            ccExtrasCheckBoxes.Add( ccExtraFoodCheckBox );
            ccExtrasCheckBoxes.Add( ccExtraEnergyCheckBox );
            ccExtrasCheckBoxes.Add( ccExtraWorkersCheckBox );
            warehousesFillCheckBoxes = new List<CheckBox>( 5 );
            warehousesFillCheckBoxes.Add( warehousesFillWoodCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillStoneCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillIronCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillOilCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillGoldCheckBox );
            swarmsCheckBoxes = new List<CheckBox>( 3 );
            swarmsCheckBoxes.Add( swarmsFasterCheckBox );
            swarmsCheckBoxes.Add( swarmsEasyCheckBox );
            swarmsCheckBoxes.Add( swarmsHardCheckBox );
            generalCheckBoxes = new List<CheckBox>( 2 );
            generalCheckBoxes.Add( themeCheckBox );
            generalCheckBoxes.Add( disableMayorsCheckBox );

            vodReplaceComboBox.DataSource = new BindingSource( SaveEditor.vodSizesNames, null );
            vodReplaceComboBox.DisplayMember = "Value";
            vodReplaceComboBox.ValueMember = "Key";

            giftComboBox.DataSource = new BindingSource( SaveEditor.giftableTypeNames, null );
            giftComboBox.DisplayMember = "Value";
            giftComboBox.ValueMember = "Key";

            swarmEasyComboBox.DataSource = new BindingSource( SaveEditor.SwarmDirectionsNames, null );
            swarmEasyComboBox.DisplayMember = "Value";
            swarmEasyComboBox.ValueMember = "Key";
            swarmHardComboBox.DataSource = new BindingSource( SaveEditor.SwarmDirectionsNames, null );
            swarmHardComboBox.DisplayMember = "Value";
            swarmHardComboBox.ValueMember = "Key";

            themeComboBox.DataSource = new BindingSource( SaveEditor.themeTypeNames, null );
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

        internal bool modifySave( SaveEditor dataEditor )
        {
            try
            {
                // Zombie Population Scaling
                if( zombieScaleCheckBox.Checked )
                {
                    decimal scale = zombieScaleNumericUpDown.Value;
                    statusWriter( "Scaling Zombie population x" + scale + '.' );
                    dataEditor.scalePopulation( scale );
                }
                else
                {
                    SortedDictionary<SaveEditor.ScalableZombieGroups, decimal> scalableZombieGroupFactors = new SortedDictionary<SaveEditor.ScalableZombieGroups, decimal>();
                    if( zombieScaleWeakCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.WEAK, zombieScaleWeakNumericUpDown.Value );
                    }
                    if( zombieScaleMediumCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.MEDIUM, zombieScaleMediumNumericUpDown.Value );
                    }
                    if( zombieScaleDressedCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.DRESSED, zombieScaleDressedNumericUpDown.Value );
                    }
                    if( zombieScaleStrongCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.STRONG, zombieScaleStrongNumericUpDown.Value );
                    }
                    if( zombieScaleVenomCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.VENOM, zombieScaleVenomNumericUpDown.Value );
                    }
                    if( zombieScaleHarpyCheckBox.Checked )
                    {
                        scalableZombieGroupFactors.Add( SaveEditor.ScalableZombieGroups.HARPY, zombieScaleHarpyNumericUpDown.Value );
                    }

                    if( scalableZombieGroupFactors.Any() )
                    {
                        statusWriter( "Scaling Zombie population per type." );
                        dataEditor.scalePopulation( scalableZombieGroupFactors );
                    }
                }
                if( zombieScaleGiantCheckBox.Checked )
                {
                    decimal scale = zombieScaleGiantNumericUpDown.Value;
                    statusWriter( "Scaling Giant population x" + scale + '.' );
                    dataEditor.scaleHugePopulation( true, scale );
                }
                if( zombieScaleMutantCheckBox.Checked )
                {
                    decimal scale = zombieScaleMutantNumericUpDown.Value;
                    statusWriter( "Scaling Mutant population x" + scale + '.' );
                    dataEditor.scaleHugePopulation( false, scale );
                }

                // Mutants
                if( mutantReplaceAllRadio.Checked )
                {
                    bool toGiantNotMutant = mutantReplaceAllComboBox.SelectedIndex == 0;
                    statusWriter( "Replacing all " + ( toGiantNotMutant ? "Mutants with Giants." : "Giants with Mutants." ) );
                    dataEditor.replaceHugeZombies( toGiantNotMutant );
                }
                else if( mutantsMoveRadio.Checked )
                {
                    bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                    bool perDirection = mutantMoveGlobalComboBox.SelectedIndex == 1;
                    statusWriter( "Relocating Mutants to farthest " + ( toGiantNotMutant ? "Giant" : "Mutant" ) + ( perDirection ? " per Compass quadrant if possible." : " on the map." ) );
                    dataEditor.relocateMutants( toGiantNotMutant, perDirection );
                }

                // VODs
                if( vodReplaceCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.VodSizes, string> kv = (KeyValuePair<SaveEditor.VodSizes, string>) vodReplaceComboBox.SelectedItem;
                    statusWriter( "Replacing all VOD buildings with " + kv.Value + '.' );
                    dataEditor.resizeVODs( kv.Key );
                }
                else
                {
                    if( vodStackDwellingCheckBox.Checked )
                    {
                        decimal scale = vodStackDwellingsNumericUpDown.Value;
                        statusWriter( "Scaling Dwellings count x" + scale + '.' );
                        dataEditor.stackVODbuildings( SaveEditor.VodSizes.SMALL, scale );
                    }
                    if( vodStackTavernsCheckBox.Checked )
                    {
                        decimal scale = vodStackTavernsNumericUpDown.Value;
                        statusWriter( "Scaling Taverns count x" + scale + '.' );
                        dataEditor.stackVODbuildings( SaveEditor.VodSizes.MEDIUM, scale );
                    }
                    if( vodStackCityHallsCheckBox.Checked )
                    {
                        decimal scale = vodStackCityHallsNumericUpDown.Value;
                        statusWriter( "Scaling City Halls count x" + scale + '.' );
                        dataEditor.stackVODbuildings( SaveEditor.VodSizes.LARGE, scale );
                    }
                }

                // Fog of War
                if( fogRemoveRadioButton.Checked )
                {
                    statusWriter( "Removing all the fog." );
                    dataEditor.removeFog();
                }
                else if( fogClearRadioButton.Checked )
                {
                    uint radius = Convert.ToUInt32( fogNumericUpDown.Value );
                    statusWriter( "Removing the fog with cell range: " + radius );
                    dataEditor.removeFog( radius );
                }
                else if( fogShowFullRadioButton.Checked )
                {
                    statusWriter( "Revealing the map." );
                    dataEditor.showFullMap();
                }

                // Command Center Extras
                if( anyChecked( ccExtrasCheckBoxes ) )
                {
                    uint food = ccExtraFoodCheckBox.Checked ? Convert.ToUInt32( ccFoodNumericUpDown.Value ) : 0;
                    uint energy = ccExtraEnergyCheckBox.Checked ? Convert.ToUInt32( ccEnergyNumericUpDown.Value ) : 0;
                    uint workers = ccExtraWorkersCheckBox.Checked ? Convert.ToUInt32( ccWorkersNumericUpDown.Value ) : 0;
                    statusWriter( "Adding Command Center extra supplies,"
                        + ( food > 0 ? " Food: +" + food : "" )
                        + ( energy > 0 ? " Energy: +" + energy : "" )
                        + ( workers > 0 ? " Workers: +" + workers : "" )
                        + '.' );
                    dataEditor.addExtraSupplies( food, energy, workers );
                }

                if( ccGiftCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.GiftableTypes, string> kv = (KeyValuePair<SaveEditor.GiftableTypes, string>) giftComboBox.SelectedItem;
                    uint typeID = Convert.ToUInt32( giftNumericUpDown.Value );
                    statusWriter( "Gifting " + typeID + "x " + kv.Value + "." );
                    dataEditor.giftEntities( kv.Key, typeID );
                }

                // Fill Resource Storage
                if( anyChecked( warehousesFillCheckBoxes ) )
                {
                    statusWriter( "Filling storage for specified resources:"
                        + ( warehousesFillGoldCheckBox.Checked ? " Gold;" : "" )
                        + ( warehousesFillWoodCheckBox.Checked ? " Wood;" : "" )
                        + ( warehousesFillStoneCheckBox.Checked ? " Stone;" : "" )
                        + ( warehousesFillIronCheckBox.Checked ? " Iron;" : "" )
                        + ( warehousesFillOilCheckBox.Checked ? " Oil;" : "" ) );
                    dataEditor.fillStorage( warehousesFillGoldCheckBox.Checked, warehousesFillWoodCheckBox.Checked, warehousesFillStoneCheckBox.Checked, warehousesFillIronCheckBox.Checked, warehousesFillOilCheckBox.Checked );
                }

                // Swarms
                if( swarmsFasterCheckBox.Checked )
                {
                    statusWriter( "Setting swarms to 50 Days Challenge timings." );
                    dataEditor.fasterSwarms();
                }
                if( swarmsEasyCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.SwarmDirections, string> kv = (KeyValuePair<SaveEditor.SwarmDirections, string>) swarmEasyComboBox.SelectedItem;
                    statusWriter( "Setting earlier swarm directions to " + kv.Value + '.' );
                    dataEditor.setSwarms( true, kv.Key );
                }
                if( swarmsHardCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.SwarmDirections, string> kv = (KeyValuePair<SaveEditor.SwarmDirections, string>) swarmHardComboBox.SelectedItem;
                    statusWriter( "Setting later swarm directions to " + kv.Value + '.' );
                    dataEditor.setSwarms( false, kv.Key );
                }

                // General Rules
                if( themeCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.ThemeType, string> kv = (KeyValuePair<SaveEditor.ThemeType, string>) themeComboBox.SelectedItem;
                    statusWriter( "Changing Theme to " + kv.Value + '.' );
                    dataEditor.changeTheme( kv.Key );
                }
                if( disableMayorsCheckBox.Checked )
                {
                    statusWriter( "Disabling Mayors." );
                    dataEditor.disableMayors();
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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifyManagerControls : UserControl
    {
        private enum EditingState : byte
        {
            CHOOSING_SAVE,
            SAVE_CHOSEN,
            QUICK_EXTRACTING,
            PROBLEM_ENCOUNTED,
            QUICK_EXTRACTED,
            QUICK_REPACKING,
            SAVE_REPACKED,
            MANUAL_EXTRACTING,
            MANUAL_EXTRACTED,
            MANUAL_REPACKING
        };

        private readonly ModifyManager modifyManager;
        private readonly StatusWriterDelegate statusWriter;
        private readonly ModifyChoicesControls modifyChoicesControls;
        private EditingState editingState;

        public ModifyManagerControls( ModifyManager m, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            modifyManager = m;
            statusWriter = sW;

            modifyChoicesControls = new ModifyChoicesControls
            {
                Location = new System.Drawing.Point( 0, 0 ),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Name = "modifyChoicesControls"
            };
            optionsSplitContainer.Panel1.Controls.Add( modifyChoicesControls );

            editingState = EditingState.CHOOSING_SAVE;

            saveOpenFileDialog.Filter = "TAB Save Files|" + TAB.SAVES_FILTER;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;
        }

        internal void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            if( !String.IsNullOrEmpty( outLine.Data ) )
            {
                if( reflectorTextBox.InvokeRequired )
                {
                    reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
                }
                else
                {
                    reflectorTextBox.AppendText( Environment.NewLine + outLine.Data );
                }
            }
        }

        private void setEditingState( EditingState newState )
        {
            if( this.InvokeRequired )
            {
                this.Invoke( new Action( () => setEditingState( newState ) ) );
            }
            else
            {
                switch( newState )
                {
                    // Use a mapping of valid state transitions..?

                    case EditingState.CHOOSING_SAVE:
                        if( editingState != EditingState.SAVE_REPACKED &&
                            editingState != EditingState.MANUAL_EXTRACTED &&
                            editingState != EditingState.PROBLEM_ENCOUNTED )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        if( editingState == EditingState.MANUAL_EXTRACTED )
                        {
                            // Skipping repacking
                            updateButtonsAfterManualRepacking();
                            considerStoppingReflector( reflectorStopRepackCheckBox.Checked );
                        }
                        editingState = newState;

                        saveFileTextBox.Text = "";
                        modifyManager.setSaveFile( null );

                        enableControls();
                        break;
                    case EditingState.SAVE_CHOSEN:
                        if( editingState != EditingState.CHOOSING_SAVE &&
                            editingState != EditingState.SAVE_CHOSEN )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        editingState = newState;

                        enableControls();
                        break;
                    case EditingState.QUICK_EXTRACTING:
                    case EditingState.MANUAL_EXTRACTING:
                    case EditingState.MANUAL_REPACKING:
                        editingState = newState;

                        disableControls();
                        break;
                    case EditingState.PROBLEM_ENCOUNTED:
                        if( editingState != EditingState.QUICK_EXTRACTING &&
                            editingState != EditingState.QUICK_EXTRACTED &&
                            editingState != EditingState.QUICK_REPACKING &&
                            editingState != EditingState.MANUAL_EXTRACTING &&
                            editingState != EditingState.MANUAL_EXTRACTED &&
                            editingState != EditingState.MANUAL_REPACKING )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        // Did we fail during a work sequence that would end after extraction or after repacking?
                        bool wasJustExtracting = editingState == EditingState.MANUAL_EXTRACTING || editingState == EditingState.MANUAL_EXTRACTED;
                        considerStoppingReflector( wasJustExtracting ? reflectorStopExtractCheckBox.Checked : reflectorStopRepackCheckBox.Checked );
                        editingState = newState;

                        break;
                    case EditingState.QUICK_EXTRACTED:
                    // We don't actually consider stopping and restarting the reflector during quick modifications.
                    case EditingState.QUICK_REPACKING:
                        editingState = newState;

                        break;
                    case EditingState.SAVE_REPACKED:
                        if( editingState != EditingState.QUICK_REPACKING &&
                            editingState != EditingState.MANUAL_REPACKING )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        if( editingState == EditingState.MANUAL_REPACKING )
                        {
                            updateButtonsAfterManualRepacking();
                        }
                        editingState = newState;

                        considerStoppingReflector( reflectorStopRepackCheckBox.Checked );
                        break;
                    case EditingState.MANUAL_EXTRACTED:
                        editingState = newState;

                        updateButtonsBeforeManualRepacking();

                        considerStoppingReflector( reflectorStopExtractCheckBox.Checked );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented EditingState: " + newState );
                }
            }
        }

        private void disableControls()
        {
            // Don't let different options be chosen during file operations
            resetButton.Enabled = false;
            modifyChoicesControls.Enabled = false;
            saveFileGroupBox.Enabled = false;
            extractLeaveCheckBox.Enabled = false;
            reflectorStopRepackCheckBox.Enabled = false;
            reflectorStopExtractCheckBox.Enabled = false;

            extractRepackSaveButton.Enabled = false;
            quickSkipSaveButton.Enabled = false;

            reflectorStopButton.Enabled = false;        // Don't allow stopping attempts during operation
        }

        private void enableControls()
        {
            if( editingState == EditingState.CHOOSING_SAVE )
            {
                resetButton.Enabled = true;
                modifyChoicesControls.Enabled = true;
                saveFileGroupBox.Enabled = true;
                extractLeaveCheckBox.Enabled = true;
                reflectorStopRepackCheckBox.Enabled = true;
                reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
            }
            extractRepackSaveButton.Enabled = editingState == EditingState.SAVE_CHOSEN;
            quickSkipSaveButton.Enabled = editingState == EditingState.SAVE_CHOSEN;
        }

        private void considerStoppingReflector( bool autoStop )
        {
            if( autoStop )
            {
                stopReflector();
            }
            else
            {
                reflectorStopButton.Enabled = modifyManager.reflectorReadyToStop();
            }
        }

        private void updateButtonsAfterManualRepacking()
        {
            extractRepackSaveButton.Text = "Manual Modify";
            quickSkipSaveButton.Text = "Quick Modify";
        }

        private void updateButtonsBeforeManualRepacking()
        {
            extractRepackSaveButton.Text = "Repack the Save File";
            quickSkipSaveButton.Text = "Skip repacking";
            extractRepackSaveButton.Enabled = true;
            quickSkipSaveButton.Enabled = true;

            extractLeaveCheckBox.Enabled = true;
            reflectorStopRepackCheckBox.Enabled = true;
        }

        internal void refreshSaveFileChoice()
        {
            if( editingState == EditingState.CHOOSING_SAVE || editingState == EditingState.SAVE_CHOSEN )
            {
                string saveFile = TAB.GetMostRecentSave( saveOpenFileDialog.InitialDirectory );
                if( saveFile != null )
                {
                    saveOpenFileDialog.FileName = saveFile; // No need to Path.GetFileName( saveFile ), doesn't help FileDialog only displaying the last ~8.3 chars
                    setSaveFile( saveFile );
                }
            }
        }

        private void resetButton_Click( object sender, EventArgs e )
        {
            modifyChoicesControls.resetChoices();
        }

        private void saveFileChooseButton_Click( object sender, EventArgs e )
        {
            if( saveOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                string saveFile = saveOpenFileDialog.FileName;
                if( TAB.IsFileWithinDirectory( saveFile, BackupsManager.DEFAULT_BACKUP_DIRECTORY ) )    // Maybe could use a dynamic value for the current backups directory, from the other tab's BackupManager...
                {
                    // Editing a backup will not trigger a checksum update once the modified file is repacked, confuses AutoBackup UI
                    statusWriter( "Please do not modify files within the backups directory: " + saveFile );
                    return;
                }

                setSaveFile( saveFile );
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = saveFile;
            setEditingState( EditingState.SAVE_CHOSEN );
            modifyManager.setSaveFile( saveFile );
        }

        private void quickSkipSaveButton_Click( object sender, EventArgs e )
        {
            switch( editingState )
            {
                case EditingState.SAVE_CHOSEN:
                    // Quick modifying

                    setEditingState( EditingState.QUICK_EXTRACTING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( quickModify_DoWork );
                    modifySaveBackgroundWorker.RunWorkerCompleted += repackingWork_RunWorkerCompleted;
                    bool anyMods = modifyChoicesControls.anyModificationChosen();
                    modifySaveBackgroundWorker.RunWorkerAsync( ( !extractLeaveCheckBox.Checked, backupCheckBox.Checked, anyMods, anyMods ? modifyChoicesControls.getChoices() : null ) );

                    break;
                case EditingState.MANUAL_EXTRACTED:
                    // Skipping repacking

                    setEditingState( EditingState.CHOOSING_SAVE );

                    break;
                default:
                    Console.Error.WriteLine( "quickSkipSaveButton_Click() should not have been invoked, invalid editing state." );
                    return;
            }
        }

        private void quickModify_DoWork( object sender, DoWorkEventArgs e )
        {
            (bool useTempDir, bool backupSave, bool anyMods, ModifyChoices choices) = ((bool, bool, bool, ModifyChoices)) e.Argument;

            // Firstly extract the save
            bool result = extractSave( useTempDir );
            if( result )
            {
                setEditingState( EditingState.QUICK_EXTRACTED );

                // Make modifications
                result = modifyExtractedSave( anyMods, choices );
                if( result )
                {
                    // No modified state to set, moving on...

                    // Backup & Repack the save
                    setEditingState( EditingState.QUICK_REPACKING );
                    result = backupAndRepackSave( backupSave );
                }

                if( useTempDir )
                {
                    // Remove the extracted files
                    modifyManager.removeDecryptedDir();
                    statusWriter( "Removed extracted files." );
                }
            }

            e.Result = result;
        }

        private void repackingWork_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( "Error while operating on the save: " + e.Error.ToString() );
            }
            // We don't care to use the worker reference to report progress or make the task cancellable.
            else
            {
                if( (bool) e.Result )
                {
                    setEditingState( EditingState.SAVE_REPACKED );
                }
                else
                {
                    setEditingState( EditingState.PROBLEM_ENCOUNTED );
                }
            }

            setEditingState( EditingState.CHOOSING_SAVE );
        }

        private void extractRepackSaveButton_Click( object sender, EventArgs e )
        {
            switch( editingState )
            {
                case EditingState.SAVE_CHOSEN:
                    // Extracting

                    setEditingState( EditingState.MANUAL_EXTRACTING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( manualExtractSave_DoWork );
                    modifySaveBackgroundWorker.RunWorkerCompleted += manualExtractSave_RunWorkerCompleted;
                    bool anyMods = modifyChoicesControls.anyModificationChosen();
                    modifySaveBackgroundWorker.RunWorkerAsync( ( !extractLeaveCheckBox.Checked, anyMods, anyMods ? modifyChoicesControls.getChoices() : null ) );

                    break;
                case EditingState.MANUAL_EXTRACTED:
                    // Repacking

                    setEditingState( EditingState.MANUAL_REPACKING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += (s,a) => a.Result = backupAndRepackSave( backupCheckBox.Checked );
                    modifySaveBackgroundWorker.RunWorkerCompleted += repackingWork_RunWorkerCompleted;
                    modifySaveBackgroundWorker.RunWorkerAsync();

                    break;
                default:
                    Console.Error.WriteLine( "extractRepackSaveButton_Click() should not have been invoked, invalid editing state." );
                    return;
            }
        }

        private void manualExtractSave_DoWork( object sender, DoWorkEventArgs e )
        {
            (bool useTempDir, bool anyMods, ModifyChoices choices) = ((bool, bool, ModifyChoices)) e.Argument;
            bool result = extractSave( useTempDir );
            if( result )
            {
                setEditingState( EditingState.MANUAL_EXTRACTED );
                result = modifyExtractedSave( anyMods, choices );
                // No modified state to set, moving on...
            }

            e.Result = result;
        }

        private void manualExtractSave_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( "Error while extracting or modifying the save: " + e.Error.ToString() );
            }
            // We don't care to use the worker reference to report progress or make the task cancellable.
            else
            {
                // State is already MANUAL_EXTRACTED if successful extraction or modification.
                if( !(bool) e.Result )
                {
                    setEditingState( EditingState.PROBLEM_ENCOUNTED );

                    setEditingState( EditingState.CHOOSING_SAVE );
                }
            }
        }

        private bool extractSave( bool useTempDir )
        {
            string saveFile = modifyManager.extractSave( useTempDir );
            if( saveFile == null )
            {
                statusWriter( "Unable to extract save file." );
                return false;
            }
            else
            {
                statusWriter( "Extracted save file:\t\t" + saveFile );
                return true;
            }
        }

        private bool modifyExtractedSave( in bool anyMods, in ModifyChoices choices )
        {
            if( !anyMods )
            {
                statusWriter( "No modifications chosen." );
                return true;
            }

            SaveEditor dataEditor = modifyManager.getSaveEditor();
            if( dataEditor == null )
            {
                statusWriter( "Unable to read extracted save file." );
                return false;
            }
            if( choices != null && !modifySave( choices, dataEditor ) )
            {
                statusWriter( "Unable to modify extracted save file." );
                return false;
            }
            return true;
        }

        private bool modifySave( ModifyChoices choices, SaveEditor dataEditor )
        {
            string formatArea( in byte radius, in bool beyondNotWithin )
            {
                return ( radius == 0 ? "" : ( beyondNotWithin ? " beyond" : " within" ) + " cell range: " + radius );
            }

            void logAndScale( string name, byte scale, string areaText, Action<byte> modify )
            {
                if( scale == 100 )
                {
                    return;
                }
                statusWriter( "Scaling " + name + ( scale == 100 ? " per type" : " x" + scale ) + areaText );
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
                SaveReader.InArea popArea = null;
                byte popScale = choices.PopulationScale;
                switch( choices.PopulationArea )
                {
                    case ModifyChoices.AreaChoices.None:
                        break;
                    case ModifyChoices.AreaChoices.Everywhere:
                        popArea = SaveReader.Everywhere;
                        break;
                    case ModifyChoices.AreaChoices.WithinRadius:
                        popRadius = choices.PopulationRadius;
                        popBNW = false;
                        popArea = dataEditor.InRadiusArea( popRadius, popBNW );
                        break;
                    case ModifyChoices.AreaChoices.BeyondRadius:
                        popRadius = choices.PopulationRadius;
                        popArea = dataEditor.InRadiusArea( popRadius, popBNW );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.PopulationArea );
                }
                if( popArea != null )
                {
                    if( popScale != 100 )
                    {
                        logAndScale( "Zombie population", popScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scalePopulation( s, choices.ScaleIdle, choices.ScaleActive, popArea ); } );
                    }
                    else if( choices.ScalableZombieGroupFactors.Any() )
                    {
                        logAndScale( "Zombie population", popScale, formatArea( popRadius, popBNW ), ( s ) => { dataEditor.scalePopulation( choices.ScalableZombieGroupFactors, choices.ScaleIdle, choices.ScaleActive, popArea ); } );
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
                SaveReader.InArea vodArea = null;
                switch( choices.VODArea )
                {
                    case ModifyChoices.AreaChoices.None:
                        break;
                    case ModifyChoices.AreaChoices.Everywhere:
                        vodArea = SaveReader.Everywhere;
                        break;
                    case ModifyChoices.AreaChoices.WithinRadius:
                        vodRadius = choices.VODRadius;
                        vodBNW = false;
                        vodArea = dataEditor.InRadiusArea( vodRadius, vodBNW );
                        break;
                    case ModifyChoices.AreaChoices.BeyondRadius:
                        vodRadius = choices.VODRadius;
                        vodArea = dataEditor.InRadiusArea( vodRadius, vodBNW );
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
                SaveReader.InArea mutantsArea = null;
                switch( choices.MutantsArea )
                {
                    case ModifyChoices.AreaChoices.None:
                        break;
                    case ModifyChoices.AreaChoices.Everywhere:
                        mutantsArea = SaveReader.Everywhere;
                        break;
                    case ModifyChoices.AreaChoices.WithinRadius:
                        mutantsRadius = choices.MutantsRadius;
                        mutantsBNW = false;
                        mutantsArea = dataEditor.InRadiusArea( mutantsRadius, mutantsBNW );
                        break;
                    case ModifyChoices.AreaChoices.BeyondRadius:
                        mutantsRadius = choices.MutantsRadius;
                        mutantsArea = dataEditor.InRadiusArea( mutantsRadius, mutantsBNW );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented choice: " + choices.MutantsArea );
                }
                if( mutantsArea != null )
                {
                    switch( choices.Mutants )
                    {
                        case ModifyChoices.MutantChoices.ReplaceWithGiants:
                            logAndModify( "Replacing all Mutants{0} with Giants.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.replaceHugeZombies( true, mutantsArea ); } );
                            break;
                        case ModifyChoices.MutantChoices.ReplaceWithMutants:
                            logAndModify( "Replacing all Giants{0} with Mutants.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.replaceHugeZombies( false, mutantsArea ); } );
                            break;
                        case ModifyChoices.MutantChoices.MoveToGiants:
                            logAndModify( "Relocating Mutants{0} to farthest Giant on the map.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( true, false, mutantsArea ); } );
                            break;
                        case ModifyChoices.MutantChoices.MoveToMutants:
                            logAndModify( "Relocating Mutants{0} to farthest Mutant on the map.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( false, false, mutantsArea ); } );
                            break;
                        case ModifyChoices.MutantChoices.MoveToGiantsPerQuadrant:
                            logAndModify( "Relocating Mutants{0} to farthest Giant per Compass quadrant if possible.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( true, true, mutantsArea ); } );
                            break;
                        case ModifyChoices.MutantChoices.MoveToMutantsPerQuadrant:
                            logAndModify( "Relocating Mutants{0} to farthest Mutant per Compass quadrant if possible.", formatArea( mutantsRadius, mutantsBNW ), () => { dataEditor.relocateMutants( false, true, mutantsArea ); } );
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
                if( choices.FogShowFullVision )
                {
                    statusWriter( "Revealing the map." );
                    dataEditor.showFullMap();
                }
                else
                {
                    switch( choices.FogArea )
                    {
                        case ModifyChoices.AreaChoices.None:
                            break;
                        case ModifyChoices.AreaChoices.Everywhere:
                            statusWriter( "Removing all the fog." );
                            dataEditor.removeFog();
                            break;
                        case ModifyChoices.AreaChoices.WithinRadius:
                            statusWriter( "Removing the fog within cell range: " + choices.FogRadius );
                            dataEditor.removeFog( choices.FogRadius, true );
                            break;
                        case ModifyChoices.AreaChoices.BeyondRadius:
                            statusWriter( "Removing the fog beyond cell range: " + choices.FogRadius );
                            dataEditor.removeFog( choices.FogRadius );
                            break;
                        case ModifyChoices.AreaChoices.Sections:
                            // To implement...
                        default:
                            throw new NotImplementedException( "Unimplemented choice: " + choices.FogArea );
                    }
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

        private bool backupAndRepackSave( bool backupSave )
        {
            if( backupSave )
            {
                string backupFile = modifyManager.backupSave();
                if( backupFile == null )
                {
                    statusWriter( "Unable to backup save file." );
                    return false; // To not overwrite an existing copy of the save with the repacked files
                }
                else
                {
                    statusWriter( "Save File backed up to:\t" + backupFile );
                }
            }

            string newSaveFile = modifyManager.repackDirAsSave();   // Assumed to always work...
            statusWriter( "New Save File created:\t" + newSaveFile );
            return newSaveFile != null;
        }

        private void reflectorStopRepackCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
            reflectorStopExtractCheckBox.Checked = false;   // Never leave it checked when disabled. Elsewhere can't check enabled status before using checked status as it's disabled during operations.
        }

        private void reflectorStopButton_Click( object sender, EventArgs e )
        {
            stopReflector();
        }

        private void stopReflector()
        {
            reflectorStopButton.Enabled = false;
            modifyManager.stopReflector();
        }

        internal void removeReflector()
        {
            // We don't check we're in a state to do any of this, or if it succeeds...
            stopReflector();
            modifyManager.removeReflector();
        }
    }
}

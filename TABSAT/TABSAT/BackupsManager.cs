using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    class BackupsManager
    {
        /*
         *  Each active save file can be checksummed, and that compared to a sorted set of checksums from the backups directory tree.
         *  
         *  Assumes no user manual modification inside backup directory. Could instead watch it too (including subdirs)?
         *  
         *  Each save file has a base name, which also determines the paired check file, and related pair of _Backup save and check files.
         *  Strip _Backup when it exists, for the purposes of top level folder game naming?
         *  
         *  Ideally, but not always (because of manual interference?), a backup file pair will be found (indirectly) under a directory with the save file base name.
         *  Ideally, but not always, the intermediary directory will have the LastWriteTimeUtc (from their existence in the active saves directory) formatted into the name.
         *  
         *  When checksumming backups, if any duplicates are discovered, determine the oldest, and remove the newer, along with their paired check file, and potentially parent directory if now empty?
         */

        /*
        internal enum State
        {
            IDLE,
            CHECKSUMMING_BACKUPS,
            CHECKSUMMING_ACTIVES,
            BACKING_UP,
            RESTORING
        }
        */
        private const string LAST_WRITE_UTC_FORMAT = "yyyyMMdd HHmmss fff";

        internal static readonly string DEFAULT_BACKUP_DIRECTORY = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\TABSAT\saves";

        public class ActiveSavesChangedEventArgs : EventArgs
        {
            public ICollection<KeyValuePair<string,CheckState>> ActiveSaves { get; }
            public ActiveSavesChangedEventArgs ()
            {
                ActiveSaves = new LinkedList<KeyValuePair<string, CheckState>>();
            }
        }

        public class BackupSavesChangedEventArgs : EventArgs
        {
            public TreeNode[] Backups { get; set; }
            public int Count { get; set; }
        }

        private readonly static HashAlgorithm algo = HMACMD5.Create();   // Don't need SHA256 or better for this purpose, MD5 is faster

        private readonly StatusWriterDelegate statusWriter;
        private readonly DirectoryInfo activeSavesDirInfo;
        private DirectoryInfo backupsDirectory;
        private readonly ConcurrentDictionary<string,SaveFile> activeSaves;
        private readonly SortedDictionary<string,SortedSet<string>> backupsTree;
        private readonly SortedSet<string> checksummedBackups;
        private bool autoBackup;
        private FileSystemWatcher savesFolderWatcher;
        private string ignoreRestoredSave;
        public event EventHandler<ActiveSavesChangedEventArgs> ActiveSavesChanged;
        public event EventHandler<BackupSavesChangedEventArgs> BackupSavesChanged;


        private class SaveFile
        {
            /*
            internal enum BackedUp
            {
                UNKNOWN,
                YES,
                NO
            }
            */
            internal readonly FileInfo file;
            internal readonly string baseName;
            internal readonly DateTime lastWriteUtc;
            private string checksum;

            internal SaveFile( FileInfo f )
            {
                file = f;
                baseName = Path.GetFileNameWithoutExtension( file.Name );
                file.Refresh();
                lastWriteUtc = file.LastWriteTimeUtc;
                checksum = null;
            }

            public override string ToString()
            {
                return baseName;
            }

            internal bool calculateChecksum( HashAlgorithm algo )
            {
                if( checksum == null )
                {
                    try
                    {
                        // Create a fileStream for the file.
                        FileStream fileStream = file.Open( FileMode.Open );
                        // Be sure it's positioned to the beginning of the stream.
                        fileStream.Position = 0;
                        // Compute the hash of the fileStream.
                        byte[] hashValue = algo.ComputeHash( fileStream );

                        checksum = Encoding.UTF8.GetString( hashValue, 0, hashValue.Length );

                        // Close the file.
                        fileStream.Close();
                    }
                    catch( IOException e )
                    {
                        Console.Error.WriteLine( $"I/O Exception: {e.Message}" );
                        return false;
                    }
                    catch( UnauthorizedAccessException e )
                    {
                        Console.Error.WriteLine( $"Access Exception: {e.Message}" );
                        return false;
                    }
                }
                return true;
            }

            internal string getChecksum()
            {
                return checksum;
            }
        }

        private void addBackup( FileInfo file )
        {
            SaveFile backup = new SaveFile( file );

            backup.calculateChecksum( algo );
            checksummedBackups.Add( backup.getChecksum() );   // Need to check for dupes and remove newer ones here..?

            // Ensure tree has top level baseName key, and that the value set contains file's parent timestamped directory
            string baseNameDirectory = ( file.Directory ).Parent.Name;
            string timeDirectory = file.Directory.Name;

            SortedSet<string> timesDirectories;
            if( !backupsTree.ContainsKey( baseNameDirectory ) )
            {
                backupsTree[baseNameDirectory] = new SortedSet<string>();
            }
            timesDirectories = backupsTree[baseNameDirectory];

            if( !timesDirectories.Contains( timeDirectory ) )
            {
                timesDirectories.Add( timeDirectory );
            }
        }

        private static void copyFileTimes( string to, FileInfo from )
        {
            File.SetCreationTimeUtc( to, from.CreationTimeUtc );
            File.SetLastWriteTimeUtc( to, from.LastWriteTimeUtc );
            File.SetLastAccessTimeUtc( to, from.LastAccessTimeUtc );
        }


        internal BackupsManager( string savesDir, StatusWriterDelegate s )
        {
            statusWriter = s;

            if( !Directory.Exists( savesDir ) )
            {
                throw new ArgumentException( "The provided saves directory does not exist." );
            }

            activeSavesDirInfo = new DirectoryInfo( savesDir );
            activeSaves = new ConcurrentDictionary<string,SaveFile>();
            backupsTree = new SortedDictionary<string,SortedSet<string>>();
            checksummedBackups = new SortedSet<string>();
            autoBackup = false;
            ignoreRestoredSave = null;

            setupWatcher();
        }

        private void setupWatcher()
        {
            savesFolderWatcher = new FileSystemWatcher
            {
                InternalBufferSize = 64 * 1024,             // Max, 64KB
                Filter = TAB.CHECKS_FILTER,
                Path = activeSavesDirInfo.FullName,
                NotifyFilter = NotifyFilters.FileName       // For Created/Renamed/Deleted events
                             | NotifyFilters.LastWrite      // For Changed events
            };
            savesFolderWatcher.Created += savesFolderWatcher_OnChanged;
            savesFolderWatcher.Changed += savesFolderWatcher_OnChanged;
            savesFolderWatcher.Deleted += savesFolderWatcher_OnChanged;
            //savesFolderWatcher.Renamed += savesFolderWatcher_OnRenamed;
        }

        private void savesFolderWatcher_OnChanged( object sender, FileSystemEventArgs e )
        {
            //statusWriter( "Saves change (" + e.ChangeType + ") detected:\t" + e.FullPath );
            string baseName = Path.GetFileNameWithoutExtension( e.FullPath );

            //FileInfo checkInfo = new FileInfo( e.FullPath );
            FileInfo newSaveInfo = new FileInfo( Path.ChangeExtension( e.FullPath, TAB.SAVE_EXTENSION ) );
            SaveFile newSave;
            SaveFile oldSave;
            switch( e.ChangeType )
            {
                case WatcherChangeTypes.Created:
                    statusWriter( "New Save detected: " + baseName );
                    newSave = new SaveFile( newSaveInfo );
                    newSave.calculateChecksum( algo );
                    if( !activeSaves.TryAdd( baseName, newSave ) )
                    {
                        statusWriter( "Conflict attempting to add a new Active Save record for: " + baseName );
                    }
                    else
                    {
                        tryAutoBackup( newSave );
                    }
                    break;
                case WatcherChangeTypes.Deleted:
                    statusWriter( "Save deletion detected: " + baseName );
                    if( !activeSaves.TryRemove( baseName, out oldSave ) )
                    {
                        statusWriter( "Conflict attempting to remove the Active Save record for: " + baseName );
                    }
                    else
                    {
                        OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );
                    }
                    break;
                case WatcherChangeTypes.Changed:
                    if( activeSaves.TryGetValue( baseName, out oldSave ) )
                    {
                        // Check for actual changes first, to preserve potentially calculated checksums
                        newSaveInfo.Refresh();
                        if( oldSave.lastWriteUtc != newSaveInfo.LastWriteTimeUtc )
                        {
                            newSave = new SaveFile( newSaveInfo );
                            newSave.calculateChecksum( algo );
                            if( !activeSaves.TryUpdate( baseName, newSave, oldSave ) )
                            {
                                statusWriter( "Conflict attempting to replace an existing Active Save record for: " + baseName );
                            }
                            else
                            {
                                tryAutoBackup( newSave );
                            }
                        }
                        else
                        {
                            Console.WriteLine( "Save change detected but ignored for: " + baseName );
                        }
                    }
                    break;
                default:
                    statusWriter( "Unhandled Save change (" + e.ChangeType + ") detected:\t" + baseName );
                    break;
            }
        }

        private void tryAutoBackup( SaveFile save )
        {

            if( ignoreRestoredSave == save.baseName )
            {
                Console.WriteLine( "Save restoration detected, skipping autobackup for: " + save.baseName );
                ignoreRestoredSave = null;
            }
            else if( autoBackup )
            {
                if( tryBackup( save ) )
                {
                    statusWriter( "Automatically backed up:\t" + save.baseName );
                }
                else
                {
                    statusWriter( "Failed to automatically back up:\t" + save.baseName );
                }
            }

            // We assume the active save is new and still needs to be signalled to interested parties anyway
            OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );
        }

        protected virtual void OnActiveSavesChanged( ActiveSavesChangedEventArgs e )
        {
            IOrderedEnumerable<SaveFile> currentSaves = activeSaves.Values.OrderByDescending( s => s.lastWriteUtc );
            foreach( var save in currentSaves )
            {
                e.ActiveSaves.Add( new KeyValuePair<string,CheckState>( save.ToString(), isBackedUp( save ) ) );
            }
            ActiveSavesChanged?.Invoke( this, e );
        }

        protected virtual void OnBackupSavesChanged( BackupSavesChangedEventArgs e )
        {
            e.Backups = new TreeNode[backupsTree.Count];

            List<KeyValuePair<string,string>> backupsNamesToNewest = new List<KeyValuePair<string,string>>( backupsTree.Count );
            foreach( var saveSet in backupsTree )
            {
                backupsNamesToNewest.Add( new KeyValuePair<string,string>( saveSet.Key, saveSet.Value.Last() ) );   // Pair baseNames and newest timeDirs
            }
            backupsNamesToNewest.Sort( ( pair1, pair2 ) => pair2.Value.CompareTo( pair1.Value ) );                  // Sort by timeDirs

            int i = 0;
            foreach( var pair in backupsNamesToNewest )
            {
                string baseName = pair.Key;
                TreeNode baseNode = new TreeNode( baseName ) { Name = baseName };
                e.Backups[i++] = baseNode;

                backupsTree.TryGetValue( baseName, out SortedSet<string> timeDirs );  // Not checking concurrency issues, but then again the nested SortedSets aren't thread-safe anyway...
                foreach( string time in timeDirs.Reverse() )
                {
                    baseNode.Nodes.Add( time );
                }
                baseNode.Expand();
            }

            e.Count = checksummedBackups.Count();
            BackupSavesChanged?.Invoke( this, e );
        }

        internal void setAutoBackup( bool enabled )
        {
            autoBackup = enabled;
            statusWriter( "Automatic Backup: " + ( enabled ? "Enabled." : "Disabled." ) );
        }

        internal void startWatcher()
        {
            if( !savesFolderWatcher.EnableRaisingEvents )
            {
                statusWriter( "Started monitoring TAB saves directory."/*: " + savesFolderWatcher.Path*/ );
                savesFolderWatcher.EnableRaisingEvents = true;
            }
        }

        internal void stopWatcher()
        {
            if( savesFolderWatcher.EnableRaisingEvents )
            {
                savesFolderWatcher.EnableRaisingEvents = false;
                statusWriter( "Stopped monitoring TAB saves directory." );
            }
        }
            
        public void Dispose()
        {
            savesFolderWatcher.Dispose();
        }

        internal bool setBackupsDirectory( BackgroundWorker worker, DoWorkEventArgs e )
        {
            string backupsDir = (string) e.Argument;

            if( !Directory.Exists( backupsDir ) )
            {
                try
                {
                    Directory.CreateDirectory( backupsDir );
                    statusWriter( "Backups directory created." );
                }
                catch( Exception ex )
                {
                    throw new ArgumentException( "The provided backup directory does not exist amd could not be created.", ex );
                }
            }
            backupsDirectory = new DirectoryInfo( backupsDir );

            int backupsCount = backupsDirectory.GetFiles( TAB.SAVES_FILTER, SearchOption.AllDirectories ).Length;
            statusWriter( "Calculating " + backupsCount + " Backups checksums." );
            checksummedBackups.Clear();
            int i = 0;

            var baseNameDirectories = backupsDirectory.GetDirectories();
            foreach( var baseNameDirectory in baseNameDirectories )
            {
                foreach( var timeDirectory in baseNameDirectory.GetDirectories()/*.OrderByDescending( d => d.LastWriteTimeUtc )*/ )
                {
                    var saveFiles = timeDirectory.GetFiles( TAB.SAVES_FILTER );
                    if( saveFiles.Length != 1 )
                    {
                        statusWriter( "Unexpected number of Backup Saves in: " + timeDirectory.FullName );
                    }
                    else
                    {
                        addBackup( saveFiles[0] );
                        i += 100;
                        worker.ReportProgress( i / backupsCount );
                    }
                }
            }

            OnBackupSavesChanged( new BackupSavesChangedEventArgs() );

            return true;
        }

        internal string getBackupsDirectory()
        {
            if( backupsDirectory == null )
            {
                throw new InvalidOperationException( "No Backups directory has been set." );
            }
            return backupsDirectory.FullName;
        }

        internal void loadActiveSaves()
        {
            FileInfo[] newActiveSavesInfo = activeSavesDirInfo.GetFiles( TAB.SAVES_FILTER );
            if( !newActiveSavesInfo.Any() )
            {
                statusWriter( "No Active Save Files found." );
                activeSaves.Clear();
                OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );
                return;
            }

            // Make a new dictionary, carry over existing ActiveSaveFiles that haven't been modified because they might have checksums, add new ones without (and remove references to saves no longer active?)

            IDictionary<string,SaveFile> newActiveSaves = new SortedDictionary<string,SaveFile>();
            foreach( FileInfo newSaveInfo in newActiveSavesInfo )
            {
                string baseName = Path.GetFileNameWithoutExtension( newSaveInfo.Name );
                if( activeSaves.ContainsKey( baseName ) )
                {
                    if( !activeSaves.TryGetValue( baseName, out SaveFile oldSave ) )
                    {
                        statusWriter( "Conflict attempting to compare an existing Active Save record for: " + baseName );
                        // It already just got deleted? So don't try add it now?
                    }
                    else
                    {
                        // Check for actual changes first, to preserve potentially calculated checksums
                        newSaveInfo.Refresh();
                        if( oldSave.lastWriteUtc == newSaveInfo.LastWriteTimeUtc )
                        {
                            newActiveSaves.Add( baseName, oldSave );
                        }
                        else
                        {
                            newActiveSaves.Add( baseName, new SaveFile( newSaveInfo ) );
                        }
                    }
                }
                else
                {
                    newActiveSaves.Add( baseName, new SaveFile( newSaveInfo ) );
                }
            }

            activeSaves.Clear();
            foreach( var entry in newActiveSaves )
            {
                if( !activeSaves.TryAdd( entry.Key, entry.Value ) )
                {
                    statusWriter( "Conflict attempting to add a new Active Save record for: " + entry.Key );
                    // Already just been added? So don't try to add it now?
                }
            }
            OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );
        }

        internal bool calculateActiveChecksums( BackgroundWorker worker )
        {
            int savesCount = activeSaves.Count;
            statusWriter( "Calculating " + savesCount + " Active Save File checksums." );

            bool anyIssues = false;

            int i = 0;
            foreach( SaveFile save in activeSaves.Values )
            {
                anyIssues &= !save.calculateChecksum( algo );
                //OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );    // Crushingly inefficient

                i += 100;
                worker.ReportProgress( Math.Min( i / savesCount, 100 ) );       // In case extra active saves were added, we mustn't report over 100%
            }
            OnActiveSavesChanged( new ActiveSavesChangedEventArgs() );

            return !anyIssues;
        }

        internal CheckState backupActiveSave( string baseName )
        {
            if( !activeSaves.TryGetValue( baseName, out SaveFile save ) )
            {
                throw new ArgumentException( "Invalid active save file name." );
            }

            if( save.getChecksum() == null )
            {
                throw new InvalidOperationException( "This save file has not been checksummed to compare against backed up files." );
            }

            if( tryBackup( save ) )
            {
                statusWriter( "Backed up:\t\t\t" + save.baseName );
            }
            else
            {
                statusWriter( "Failed to back up:\t\t" + save.baseName );
            }

            return isBackedUp( save );
        }

        private bool tryBackup( SaveFile save )
        {
            try
            {
                string backupNameDir = Path.Combine( backupsDirectory.FullName, save.baseName );
                string backupTimeDir = Path.Combine( backupNameDir, save.lastWriteUtc.ToString( LAST_WRITE_UTC_FORMAT ) );     // Not Refresh()'d file.lastWriteTimeUtc?

                string saveFileBackupPath = Path.Combine( backupTimeDir, save.baseName + TAB.SAVE_EXTENSION );
                string checkFileBackupPath = Path.Combine( backupTimeDir, save.baseName + TAB.CHECK_EXTENSION );
                if( File.Exists( saveFileBackupPath ) || File.Exists( checkFileBackupPath ) )
                {
                    statusWriter( "The save file backup already exists for the same name and last write time: " + save.file.FullName );
                    return false;
                }

                if( !Directory.Exists( backupTimeDir ) )
                {
                    try
                    {
                        Directory.CreateDirectory( backupTimeDir );
                    }
                    catch( Exception e )
                    {
                        Console.Error.WriteLine( e.Message );
                        statusWriter( "There was a problem creating the directory for the backup file: " + backupTimeDir );
                        return false;
                    }
                }

                //statusWriter( "Copying: " + saveFileBackupPath );
                File.Copy( save.file.FullName, saveFileBackupPath );
                // Also copy over timestamps
                copyFileTimes( saveFileBackupPath, save.file );

                addBackup( new FileInfo( saveFileBackupPath ) );

                string checkFileActivePath = Path.Combine( save.file.DirectoryName, save.baseName + TAB.CHECK_EXTENSION );
                if( !File.Exists( checkFileActivePath ) )
                {
                    statusWriter( "The check file does not exist: " + checkFileActivePath );
                    return false;
                }
                //statusWriter( "Copying: " + checkFileBackupPath );
                File.Copy( checkFileActivePath, checkFileBackupPath );
                // Also copy over timestamps
                copyFileTimes( checkFileBackupPath, new FileInfo( checkFileActivePath ) );
                // No tree node for check files

                OnBackupSavesChanged( new BackupSavesChangedEventArgs() );

                return true;
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                return false;
            }
        }

        private CheckState isBackedUp( SaveFile save )
        {
            CheckState checkState;

            string checksum = save.getChecksum();
            if( checksum == null )
            {
                checkState = CheckState.Indeterminate;
            }
            else if( checksummedBackups.Contains( checksum ) )
            {
                checkState = CheckState.Checked;
            }
            else
            {
                checkState = CheckState.Unchecked;
            }

            return checkState;
        }


        internal bool restoreBackup( string baseName, string timeDir )
        {
            try
            {
                string backupPathWithoutExtension = Path.Combine( Path.Combine( Path.Combine( backupsDirectory.FullName, baseName ), timeDir ), baseName );

                string backupSavePath = Path.ChangeExtension( backupPathWithoutExtension, TAB.SAVE_EXTENSION );
                if( !File.Exists( backupSavePath ) )
                {
                    //throw new ArgumentException
                    statusWriter( "No Backup save file found: " + backupSavePath );
                    return false;
                }

                string backupCheckPath = Path.ChangeExtension( backupPathWithoutExtension, TAB.CHECK_EXTENSION );
                if( !File.Exists( backupCheckPath ) )
                {
                    //throw new ArgumentException
                    statusWriter( "No Backup check file found: " + backupCheckPath );
                    return false;
                }

                //statusWriter( "Would now restore Backup File: " + Path.GetFileName( backupSavePath ) );
                string restoreSavePath = Path.Combine( activeSavesDirInfo.FullName, Path.GetFileName( backupSavePath ) );
                //bool overwroteExisting = false;
                if( File.Exists( restoreSavePath ) )
                {
                    Console.WriteLine( "Overwriting existing Save File: " + restoreSavePath );
                    //overwroteExisting = true;
                }
                File.Copy( backupSavePath, restoreSavePath, true );
                // Also copy over timestamps
                copyFileTimes( restoreSavePath, new FileInfo( backupSavePath ) );

                // Flag for autobackup to ignore this change we're about to make, so as to not try to back it up to the existing path
                if( autoBackup )
                {
                    ignoreRestoredSave = baseName;
                }

                string restoreCheckPath = Path.Combine( activeSavesDirInfo.FullName, Path.GetFileName( backupCheckPath ) );
                if( File.Exists( restoreCheckPath ) )
                {
                    Console.WriteLine( "Overwriting existing Check File: " + restoreCheckPath );
                }
                File.Copy( backupCheckPath, restoreCheckPath, true );
                // Also copy over timestamps
                copyFileTimes( restoreCheckPath, new FileInfo( backupSavePath ) );
                /*
                // Updating activeSaves. Or should the watcher see this?
                if( !overwroteExisting )
                {
                    ActiveSaveFile newSave = new ActiveSaveFile( new FileInfo( restoreSavePath ) );
                    newSave.calculateChecksum( algo );
                    //activeSaves[newSave.baseName] = newSave;
                    if( !activeSaves.TryAdd( newSave.baseName, newSave ) )
                    {
                        statusWriter( "Conflict attempting to add a new Active Save record for: " + newSave.baseName );
                        // Already just been added? So don't try to add it now?
                    }
                }
                */
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                return false;
            }

            return true;
        }

    }
}

using System;
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
         *  Each active save file can be checksummed, and that compared to a sorted map of files in the backups directory tree.
         *  
         *  Rebuild backup tree structures only on backup directory change?
         *  Except backing up saves might create new folders and certainly new files, so those need to be added to the tree to mirror the file system.
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

        private const string defaultBackupDirectoryName = @"TABSAT\saves";
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
        private static HashAlgorithm algo = HMACMD5.Create();   // Don't need SHA256 or better for this purpose, MD5 is faster

        private StatusWriterDelegate statusWriter;
        private DirectoryInfo activeSavesDirInfo;
        private DirectoryInfo backupsDirectory;
        private SortedDictionary<string,ActiveSaveFile> activeSaves;
        private TreeNode backupsTree;
        private SortedDictionary<string,BackupSaveFile> checksummedBackups;
        private FileSystemWatcher savesFolderWatcher;

        private abstract class SaveFile
        {
            /*
            internal enum BackedUp
            {
                UNKNOWN,
                YES,
                NO
            }
            */
            protected readonly FileInfo file;
            internal readonly string baseName;
            private string checksum;

            internal SaveFile( FileInfo f )
            {
                file = f;
                baseName = Path.GetFileNameWithoutExtension( file.Name );
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
                        Console.WriteLine( $"I/O Exception: {e.Message}" );
                        return false;
                    }
                    catch( UnauthorizedAccessException e )
                    {
                        Console.WriteLine( $"Access Exception: {e.Message}" );
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

        private class ActiveSaveFile : SaveFile
        {
            internal readonly DateTime lastWriteUtc;

            internal ActiveSaveFile( FileInfo f ) : base ( f )
            {
                file.Refresh();
                lastWriteUtc = file.LastWriteTimeUtc;
            }

            internal BackupSaveFile backup( DirectoryInfo backupDir, TreeNode backupsTree )
            {
                try
                {
                    string backupNameDir = Path.Combine( backupDir.FullName, baseName );
                    TreeNode backupNameDirNode;
                    if( !Directory.Exists( backupNameDir ) )
                    {
                        //Console.WriteLine( "Creating: " + backupNameDir );
                        DirectoryInfo backupNameDirInfo = Directory.CreateDirectory( backupNameDir );
                        backupNameDirNode = new TreeNode( backupNameDirInfo.Name );
                        backupsTree.Nodes.Add( backupNameDirNode );
                    }
                    else
                    {
                        backupNameDirNode = backupsTree.Nodes.Find( backupNameDir, false ).First();
                        // Tree node might not exist even if empty folder does..?
                    }
                    backupsTree.Expand();

                    string backupTimeDir = Path.Combine( backupNameDir, file.LastWriteTimeUtc.ToString( "yyyyMMdd HHmmss fff" ) );
                    TreeNode backupTimeDirNode;
                    if( !Directory.Exists( backupTimeDir ) )
                    {
                        //Console.WriteLine( "Creating: " + backupTimeDir );
                        DirectoryInfo backupTimeDirInfo = Directory.CreateDirectory( backupTimeDir );

                        backupTimeDirNode = new TreeNode( backupTimeDirInfo.Name );
                        backupNameDirNode.Nodes.Add( backupTimeDirNode );
                    }
                    else
                    {
                        backupTimeDirNode = backupNameDirNode.Nodes.Find( backupTimeDir, false ).First();
                        // Tree node might not exist even if empty folder does..?
                    }
                    backupNameDirNode.Expand();

                    string saveFileBackupPath = Path.Combine( backupTimeDir, baseName + TABReflector.TABReflector.saveExtension );
                    string checkFileBackupPath = Path.Combine( backupTimeDir, baseName + TABReflector.TABReflector.checkExtension );
                    if( File.Exists( saveFileBackupPath ) || File.Exists( checkFileBackupPath ) )
                    {
                        Console.Error.WriteLine( "The save file backup already exists for the same name and last write time: " + file.FullName );
                        return null;
                    }

                    //Console.WriteLine( "Copying: " + saveFileBackupPath );
                    File.Copy( file.FullName, saveFileBackupPath );
                    // Also copy over timestamps
                    copyFileTimes( saveFileBackupPath, file );

                    FileInfo backupFileInfo = new FileInfo( saveFileBackupPath );
                    BackupSaveFile backup = BackupSaveFile.create( backupFileInfo, backupTimeDirNode.Nodes );

                    string checkFileActivePath = Path.Combine( file.DirectoryName, baseName + TABReflector.TABReflector.checkExtension );
                    if( !File.Exists( checkFileActivePath ) )
                    {
                        Console.Error.WriteLine( "The check file does not exist: " + checkFileActivePath );
                        return null;
                    }
                    //Console.WriteLine( "Copying: " + checkFileBackupPath );
                    File.Copy( checkFileActivePath, checkFileBackupPath );
                    // Also copy over timestamps
                    copyFileTimes( checkFileBackupPath, new FileInfo( checkFileActivePath ) );
                    // No tree node for check files

                    return backup;
                }
                catch ( Exception e )
                {
                    Console.Error.WriteLine( e.Message );
                    return null;
                }
            }
        }

        private class BackupSaveFile : SaveFile
        {
            private TreeNode node;

            internal static BackupSaveFile create( FileInfo file, TreeNodeCollection nodes )
            {
                BackupSaveFile save = new BackupSaveFile( file );

                TreeNode node = new TreeNode( save.ToString() );
                save.setNode( node );
                nodes.Add( node );
                
                return save;
            }
            
            private BackupSaveFile( FileInfo f ) : base( f )
            {
                node = null;
            }
            
            internal void setNode( TreeNode n )
            {
                node = n;
            }

            internal void displayNode()
            {
                (node.Parent).Parent.EnsureVisible();
            }
        }

        private static void copyFileTimes( string to, FileInfo from )
        {
            File.SetCreationTimeUtc( to, from.CreationTimeUtc );
            File.SetLastWriteTimeUtc( to, from.LastWriteTimeUtc );
            File.SetLastAccessTimeUtc( to, from.LastAccessTimeUtc );
        }


        internal static string getDefaultBackupDirectory()
        {
            return Path.Combine( Environment.ExpandEnvironmentVariables( @"%USERPROFILE%\Documents\" ), defaultBackupDirectoryName );
        }


        internal BackupsManager( string savesDir, StatusWriterDelegate s )
        {
            statusWriter = s;

            if( !Directory.Exists( savesDir ) )
            {
                throw new ArgumentException( "The provided saves directory does not exist." );
            }

            activeSavesDirInfo = new DirectoryInfo( savesDir );
            activeSaves = new SortedDictionary<string,ActiveSaveFile>();
            backupsTree = new TreeNode();
            checksummedBackups = new SortedDictionary<string,BackupSaveFile>();

            setupWatcher();
        }

        private void setupWatcher()
        {
            savesFolderWatcher = new FileSystemWatcher();
            //( (ISupportInitialize) ( this.savesFolderWatcher ) ).BeginInit();
            //savesFolderWatcher.EnableRaisingEvents = false;

            savesFolderWatcher.Filter = TABSAT.checkFilesFilter;
            //savesFolderWatcher.SynchronizingObject = this;   // No longer syncing on form component

            savesFolderWatcher.Path = activeSavesDirInfo.FullName;
            savesFolderWatcher.NotifyFilter = NotifyFilters.FileName;       // For Created/Renamed/Deleted events
            //                                | NotifyFilters.LastWrite;    // For Changed events (so as to pick up file modified in our case)

            savesFolderWatcher.Created += savesFolderWatcher_OnChanged;
            //savesFolderWatcher.Changed += savesFolderWatcher_OnChanged;   // We can actually ignore Changed events, because all save modifications are done by TAB via renames to ._old and from .zxsav.tmp?
            savesFolderWatcher.Deleted += savesFolderWatcher_OnChanged;
            savesFolderWatcher.Renamed += savesFolderWatcher_OnRenamed;
            
            //( (ISupportInitialize) ( savesFolderWatcher ) ).EndInit();
        }

        private void savesFolderWatcher_OnChanged( object sender, FileSystemEventArgs e )
        {
            statusWriter( "Saves folder change (" + e.ChangeType + ") detected:\t" + e.FullPath );
        }

        private void savesFolderWatcher_OnRenamed( object sender, RenamedEventArgs e )
        {
            statusWriter( "Saves folder rename detected, from:\t" + e.OldFullPath + " to:\t" + e.FullPath );
        }


        internal bool isWatching()
        {
            return savesFolderWatcher.EnableRaisingEvents;
        }

        internal void startWatcher()
        {
            if( !isWatching() )
            {
                statusWriter( "Started monitoring TAB saves directory."/*: " + savesFolderWatcher.Path*/ );
                savesFolderWatcher.EnableRaisingEvents = true;
            }
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

            // Is it more efficient to filter through all subdirs first to know the count, and maybe prefetch, or cache bump?
            //var allBackupSaveFiles = backupsDirectory.GetFiles( TABSAT.saveFilesFilter, SearchOption.AllDirectories );
            IList<BackupSaveFile> backups = new List<BackupSaveFile>( /*allBackupSaveFiles.Length*/ );

            backupsTree = new TreeNode( backupsDirectory.Name ) { Tag = backupsDirectory };
            backupsTree.Expand();

            var stack = new Stack<TreeNode>();
            stack.Push( backupsTree );

            do
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo) currentNode.Tag;
                currentNode.Tag = null;     // Don't keep DirectoryInfo handles around, only use Tag when building the tree
                var subDirectories = directoryInfo.GetDirectories();
                //IOrderedEnumerable<DirectoryInfo> subDirectories = subDirectories.OrderByDescending( d => d.LastWriteTimeUtc );    // We can just use the timestamped folder names to sort after populating the tree
                foreach( var directory in subDirectories )
                {
                    var childDirectoryNode = new TreeNode( directory.Name ) { Tag = directory };
                    currentNode.Nodes.Add( childDirectoryNode );
                    stack.Push( childDirectoryNode );
                }
                var saveFiles = directoryInfo.GetFiles( TABSAT.saveFilesFilter );
                foreach( var file in saveFiles )
                {
                    BackupSaveFile backup = BackupSaveFile.create( file, currentNode.Nodes );
                    backups.Add( backup );
                }

                if( currentNode.Parent == backupsTree )    // || saveFiles.Length > 0
                {
                    currentNode.Expand();
                }

                if( !subDirectories.Any() && !saveFiles.Any() )
                {
                    currentNode.Remove();
                }

            } while( stack.Any() );

            return calculateBackupsChecksums( backups, worker );
        }

        private bool calculateBackupsChecksums( IList<BackupSaveFile> backups, BackgroundWorker worker )
        {
            statusWriter( "Started recalculating " + backups.Count + " Backups checksums." );

            bool anyIssues = false;

            checksummedBackups.Clear();

            int i = 0;
            foreach( BackupSaveFile backup in backups )
            {
                anyIssues &= !backup.calculateChecksum( algo );

                // Need to check for dupes and remove newer ones here?
                checksummedBackups.Add( backup.getChecksum(), backup );

                i += 100;
                worker.ReportProgress( i / backups.Count );
            }

            return !anyIssues;
        }


        internal int getBackupsCount()
        {
            return checksummedBackups.Count();
        }

        internal string getBackupsDirectory()
        {
            if( backupsDirectory == null )
            {
                throw new InvalidOperationException( "No Backups directory has been set." );
            }
            return backupsDirectory.FullName;
        }

        internal void displayBackups( TreeView backupsTreeView )
        {
            backupsTreeView.BeginUpdate();
            backupsTreeView.Nodes.Clear();
            backupsTreeView.Nodes.Add( backupsTree );
            // Sort the tree top 2 levels by descending 2nd level folder name (the timestamped folders)...
            backupsTreeView.Sort();

            backupsTreeView.EndUpdate();
        }

        internal void reloadActiveSaves()
        {
            FileInfo[] newActiveSavesInfo = activeSavesDirInfo.GetFiles( TABSAT.saveFilesFilter );
            if( !newActiveSavesInfo.Any() )
            {
                statusWriter( "No Active Save Files found." );
                activeSaves.Clear();
                return;
            }

            // Make a new dictionary, carry over existing ActiveSaveFiles that haven't been modified because they might have checksums, add new ones without (and remove references to saves no longer active?)

            SortedDictionary<string,ActiveSaveFile> newActiveSaves = new SortedDictionary<string,ActiveSaveFile>();
            foreach( FileInfo newSaveInfo in newActiveSavesInfo )
            {
                string baseName = Path.GetFileNameWithoutExtension( newSaveInfo.Name );
                ActiveSaveFile newSave = null;
                if( activeSaves.ContainsKey( baseName ) )
                {
                    ActiveSaveFile oldSave = activeSaves[baseName];
                    newSaveInfo.Refresh();
                    if( oldSave.lastWriteUtc == newSaveInfo.LastWriteTimeUtc )
                    {
                        newSave = oldSave;      // To preserve potentially calculated checksums
                    }
                }
                if( newSave == null )
                {
                    newSave = new ActiveSaveFile( newSaveInfo );
                }
                newActiveSaves.Add( newSave.baseName, newSave );
            }

            activeSaves = newActiveSaves;
        }

        internal void displayActiveSaves( CheckedListBox checkedListBox )
        {
            checkedListBox.Items.Clear();
            //checkedListBox.SuspendLayout();
            foreach( ActiveSaveFile save in activeSaves.Values.OrderByDescending( s => s.lastWriteUtc ) )
            {
                checkedListBox.Items.Add( save, getCheckState( save ) );
            }
            //checkedListBox.ResumeLayout();
        }

        internal bool calculateActiveChecksums( BackgroundWorker worker )
        {
            statusWriter( "Started recalculating " + activeSaves.Count + " Active Save File checksums." );

            bool anyIssues = false;

            int i = 0;
            foreach( ActiveSaveFile save in activeSaves.Values )
            {
                anyIssues &= !save.calculateChecksum( algo );
                i += 100;
                worker.ReportProgress( i / activeSaves.Count );
            }

            return !anyIssues;
        }

        internal void tryDisplayBackupNode( string baseName )
        {
            if( !activeSaves.ContainsKey( baseName ) )
            {
                throw new ArgumentException( "Invalid active save file name." );
            }
            ActiveSaveFile save = activeSaves[baseName];

            string checksum = save.getChecksum();
            if( checksum == null )
            {
                //throw new InvalidOperationException( "This save file has not been checksummed to compare against backed up files." );
                return;
            }

            if( !checksummedBackups.ContainsKey( checksum ) )
            {
                return;
            }
            BackupSaveFile backup = checksummedBackups[checksum];
            backup.displayNode();
        }

        internal CheckState backupActiveSave( string baseName )
        {
            if( !activeSaves.ContainsKey( baseName ) )
            {
                throw new ArgumentException( "Invalid active save file name." );
            }
            ActiveSaveFile save = activeSaves[baseName];

            if( save.getChecksum() == null )
            {
                throw new InvalidOperationException( "This save file has not been checksummed to compare against backed up files." );
            }

            BackupSaveFile backup = save.backup( backupsDirectory, backupsTree );
            if( backup != null )
            {
                statusWriter( "Successfully backed up: " + save );

                backup.calculateChecksum( algo );
                checksummedBackups.Add( backup.getChecksum(), backup );
            }
            else
            {
                statusWriter( "Failed to back up: " + save );
            }
            return getCheckState( save );
        }

        private CheckState getCheckState( ActiveSaveFile save )
        {
            CheckState checkState;

            string checksum = save.getChecksum();
            if( checksum == null )
            {
                checkState = CheckState.Indeterminate;
            }
            else if( checksummedBackups.ContainsKey( checksum ) )
            {
                checkState = CheckState.Checked;
                //Console.WriteLine( "Display check for active save file: " + save );
            }
            else
            {
                checkState = CheckState.Unchecked;
            }

            return checkState;
        }

        internal bool restoreBackup( string backupSubPath )
        {
            try
            {
                string backupPathWithoutExtension = Path.Combine( backupsDirectory.Parent.FullName, backupSubPath );

                string backupSavePath = Path.ChangeExtension( backupPathWithoutExtension, TABReflector.TABReflector.saveExtension );
                if( !File.Exists( backupSavePath ) )
                {
                    //throw new ArgumentException
                    statusWriter( "No Backup save file found: " + backupSavePath );
                    return false;
                }

                string backupCheckPath = Path.ChangeExtension( backupPathWithoutExtension, TABReflector.TABReflector.checkExtension );
                if( !File.Exists( backupCheckPath ) )
                {
                    //throw new ArgumentException
                    statusWriter( "No Backup check file found: " + backupCheckPath );
                    return false;
                }

                //statusWriter( "Would now restore Backup File: " + Path.GetFileName( backupSavePath ) );
                string restoreSavePath = Path.Combine( activeSavesDirInfo.FullName, Path.GetFileName( backupSavePath ) );
                bool overwroteExisting = false;
                if( File.Exists( restoreSavePath ) )
                {
                    Console.WriteLine( "Overwriting existing Save File: " + restoreSavePath );
                    overwroteExisting = true;
                }
                File.Copy( backupSavePath, restoreSavePath, true );
                // Also copy over timestamps
                copyFileTimes( restoreSavePath, new FileInfo( backupSavePath ) );

                string restoreCheckPath = Path.Combine( activeSavesDirInfo.FullName, Path.GetFileName( backupCheckPath ) );
                if( File.Exists( restoreCheckPath ) )
                {
                    Console.WriteLine( "Overwriting existing Check File: " + restoreCheckPath );
                }
                File.Copy( backupCheckPath, restoreCheckPath, true );
                // Also copy over timestamps
                copyFileTimes( restoreCheckPath, new FileInfo( backupSavePath ) );
                
                // Updating activeSaves. Or should the watcher see this?
                if( !overwroteExisting )
                {
                    ActiveSaveFile newSave = new ActiveSaveFile( new FileInfo( restoreSavePath ) );
                    newSave.calculateChecksum( algo );
                    activeSaves.Add( newSave.baseName, newSave );
                }
                
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

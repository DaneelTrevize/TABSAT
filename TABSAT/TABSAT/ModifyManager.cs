using Ionic.Zip;
using System;
using System.IO;
using System.Reflection;

namespace TABSAT
{
    class ModifyManager
    {
        /*
         *  To decrypt/encrypt save files:
         *  copy the reflector binary to the TAB installation;
         *  invoke the reflector and open a pipe to it;
         *  pipe the paths to specific save files to manipulate;
         *  
         *  The reflector binary will:
         *  open a pipe to receive instructions and file paths;
         *  start a thread to launch the TAB assembly;
         *  wait for the TAB assembly to have launched, then use reflection to find the required methods to sign or generate passwords for saves;
         *  repeatedly, invoke signing or password generation for each file path supplied
         *  Exiting will have to terminate the reflector's environment, in order to end the stalled TAB assembly thread.
         */

        internal enum SaveState
        {
            UNSET,
            SET,
            EXTRACTED
        };

        internal static readonly string DEFAULT_EDITS_DIRECTORY = Environment.ExpandEnvironmentVariables( @"%USERPROFILE%\Documents\TABSAT\edits\" );

        private readonly ReflectorManager reflectorManager;
        private readonly string editsDir;
        private string currentSaveFile;
        private string currentDecryptDir;
        private SaveState state;    // Should lock() access?


        private static string backupSave( string saveFile, string backupDir, bool tryCheckFile )     // Refactor this into new BackupsManager features?
        {
            if( !File.Exists( saveFile ) )
            {
                Console.Error.WriteLine( "Save file does not exist: " + saveFile );
                return null;
            }

            // Figure out a filename that isn't taken
            string saveFileName = Path.GetFileName( saveFile );
            FileInfo[] backupsInfo = new DirectoryInfo( backupDir ).GetFiles( saveFileName + ".*" );
            int suffix = backupsInfo.Length + 1;
            string backupFile = Path.Combine( backupDir, saveFileName ) + '.' + suffix;

            if( File.Exists( backupFile ) )
            {
                Console.Error.WriteLine( "Backup already exists: " + backupFile );
                return null;
            }

            // Move the old zip
            try
            {
                File.Move( saveFile, backupFile );
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( "Unable to backup: " + saveFile + " to: " + backupFile );
                Console.Error.WriteLine( e.Message );
                return null;
            }

            if( tryCheckFile )
            {
                // Try to backup the check file too, but don't fail the overall method if this can't be done
                string checkFile = TAB.GetCheckFile( saveFile );
                if( !File.Exists( checkFile ) )
                {
                    Console.Error.WriteLine( "Check file does not exist: " + checkFile );
                }
                else
                {
                    string backupCheckFile = Path.Combine( backupDir, Path.GetFileName( checkFile ) ) + '.' + suffix;
                    if( File.Exists( backupCheckFile ) )
                    {
                        Console.Error.WriteLine( "Check backup already exists: " + backupCheckFile );
                    }
                    else
                    {
                        try
                        {
                            File.Move( checkFile, backupCheckFile );
                        }
                        catch( Exception e )
                        {
                            Console.Error.WriteLine( "Unable to backup: " + checkFile + " to: " + backupCheckFile );
                            Console.Error.WriteLine( e.Message );
                        }
                    }
                }
            }

            return backupFile;
        }

        private static void unpackSave( string saveFile, string decryptDir, string password = null )
        {
            using( ZipFile zip = ZipFile.Read( saveFile ) )
            {
                if( password != null )
                {
                    zip.Password = password;
                }

                try
                {
                    zip.ExtractAll( decryptDir );
                }
                catch( Exception e )
                {
                    Console.Error.WriteLine( "Failed to extract :" + saveFile );
                    Console.Error.WriteLine( e.Message );
                    return;
                }
                //Console.WriteLine( "Extracted save file: " + saveFile );
            }
        }

        private static void repackExtracted( string saveFile, string decryptDir, string password = null )
        {
            using( ZipFile newZip = new ZipFile() )
            {
                if( password != null )
                {
                    newZip.Password = password;
                }
                newZip.AddDirectory( decryptDir );
                newZip.Name = saveFile;

                newZip.Save();
                //Console.WriteLine( "Save repacked: " + saveFile );
            }
        }


        public ModifyManager( string TABdirectory, ReflectorManager refM, string edits )
        {
            Assembly.LoadFile( Path.Combine( TABdirectory, @"Ionic.Zip.dll" ) );

            if( refM == null )
            {
                throw new ArgumentNullException( "ReflectorManager should not be null." );
            }

            reflectorManager = refM;
            editsDir = edits;

            if( !Directory.Exists( editsDir ) )
            {
                try
                {
                    Directory.CreateDirectory( editsDir );
                }
                catch( Exception e )
                {
                    throw new ArgumentException( "The edits backup directory does not exist and could not be created.", e );
                }
            }

            setSaveFile( null );
        }


        internal void setSaveFile( string file )
        {
            if( file == null )
            {
                currentSaveFile = null;
                state = SaveState.UNSET;
            }
            else
            {
                currentSaveFile = file;
                state = SaveState.SET;
            }
            currentDecryptDir = null;
        }
        /*
        internal SaveState getState()  // Refactor into event SaveFileSet?
        {
            return state;
        }

        internal bool hasSaveFile()
        {
            return state == ModifyManager.SaveState.SET;
        }
        */
        internal string extractSave( bool useTempDir )
        {
            if( state != SaveState.SET )
            {
                throw new InvalidOperationException( "Save File has not been set." );
            }

            try
            {
                if( useTempDir )
                {
                    currentDecryptDir = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );  // A random "file"/folder name under the user's temp directory
                }
                else
                {
                    currentDecryptDir = Path.Combine( editsDir, Path.GetFileNameWithoutExtension( currentSaveFile ) );
                }
                if( Directory.Exists( currentDecryptDir ) )
                {
                    // Dynamically generate decrypted file folders for leaving files after modification or for manual edits?

                    // Nuke existing files
                    Console.WriteLine( "Deleting the contents of: " + currentDecryptDir );
                    DirectoryInfo decDir = new DirectoryInfo( currentDecryptDir );
                    foreach( FileInfo file in decDir.GetFiles() )
                    {
                        file.Delete();
                    }
                    foreach( DirectoryInfo dir in decDir.GetDirectories() )
                    {
                        dir.Delete( true );
                    }
                }
                else
                {
                    Directory.CreateDirectory( currentDecryptDir );
                }

                string password = signAndGeneratePassword( currentSaveFile );

                unpackSave( currentSaveFile, currentDecryptDir, password );

                state = SaveState.EXTRACTED;

                return currentSaveFile;
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                return null;
            }
        }

        internal SaveEditor getSaveEditor()
        {
            if( state != SaveState.EXTRACTED )
            {
                throw new InvalidOperationException( "Save File has not been extracted." );
            }
            try
            {
                return new SaveEditor( currentDecryptDir );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine( "Problem opening extracted save file: " + e.Message + Environment.NewLine + e.StackTrace );
                return null;
            }
        }

        internal string backupSave()
        {
            if( state != SaveState.EXTRACTED )
            {
                throw new InvalidOperationException( "Save File has not been extracted." );
            }
            return backupSave( currentSaveFile, editsDir, true );
        }

        internal string repackDirAsSave()
        {
            if( state != SaveState.EXTRACTED )
            {
                throw new InvalidOperationException( "Save File has not been extracted." );
            }
            // Don't create unencrypted saves where TAB or auto-backup watchers might see them
            string unencryptedSaveFile = Path.Combine( Path.GetTempPath(), Path.GetFileName( currentSaveFile ) );

            repackExtracted( unencryptedSaveFile, currentDecryptDir );

            string password = signAndGeneratePassword( unencryptedSaveFile );

            // Purge the temporary unencrypted versions of this new save file
            File.Delete( unencryptedSaveFile );
            File.Delete( TAB.GetCheckFile( unencryptedSaveFile ) );

            repackExtracted( currentSaveFile, currentDecryptDir, password );

            signAndGeneratePassword( currentSaveFile, false );

            return currentSaveFile;
        }

        internal void removeDecryptedDir()
        {
            if( state != SaveState.EXTRACTED )
            {
                throw new InvalidOperationException( "Save File has not been extracted." );
            }

            try
            {
                Directory.Delete( currentDecryptDir, true );
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
            }

            setSaveFile( currentSaveFile ); // To reset currentDecryptDir and state
        }

        internal bool reflectorReadyToStop()
        {
            return reflectorManager.getState() == ReflectorManager.ReflectorState.STARTED;
        }

        internal void stopReflector()
        {
            if( reflectorReadyToStop() )
            {
                reflectorManager.stopReflector();
            }
        }

        internal void removeReflector()
        {
            if( reflectorManager.getState() == ReflectorManager.ReflectorState.DEPLOYED )
            {
                reflectorManager.removeReflector();
            }
        }
        
        private string signAndGeneratePassword( string saveFile, bool andGeneratePassword = true )
        {
            if( reflectorManager.getState() == ReflectorManager.ReflectorState.UNDEPLOYED )
            {
                reflectorManager.deployReflector();
            }

            if( reflectorManager.getState() == ReflectorManager.ReflectorState.DEPLOYED )
            {
                reflectorManager.startReflector();
            }

            string signature = reflectorManager.generateChecksum( saveFile );
            //Console.WriteLine( "Signature from reflector: " + signature );

            if( andGeneratePassword )
            {
                string password = reflectorManager.generatePassword( saveFile );
                //Console.WriteLine( "Password from reflector: " + password );
                return password;
            }

            return signature;
        }
    }
}

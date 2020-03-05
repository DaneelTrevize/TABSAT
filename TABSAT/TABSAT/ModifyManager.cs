using Ionic.Zip;
using System;
using System.IO;

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

        /*
        enum SaveState
        {
            WAITING_FOR_FILE,
            HAVE_FILE,
            EXTRACTING,
            HAVE_EXTRACTED,
            //BACKING_UP,
            REPACKING
        };
        */

        internal static readonly string DEFAULT_EDITS_DIRECTORY = Environment.ExpandEnvironmentVariables( @"%USERPROFILE%\Documents\TABSAT\edits\" );

        private ReflectorManager reflectorManager;
        private readonly string editsDir;
        private string currentSaveFile;
        private string currentDecryptDir;


        internal static string getReflectorDirectory()
        {
            return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
        }


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
            catch
            {
                Console.Error.WriteLine( "Unable to backup: " + saveFile + " to: " + backupFile );
                return null;
            }

            if( tryCheckFile )
            {
                // Try to backup the check file too, but don't fail the overall method if this can't be done
                string checkFile = Path.ChangeExtension( saveFile, TAB.CHECK_EXTENSION );
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
                        catch
                        {
                            Console.Error.WriteLine( "Unable to backup: " + checkFile + " to: " + backupCheckFile );
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


        public ModifyManager( ReflectorManager r, string e )
        {
            if( r == null )
            {
                throw new ArgumentNullException( "ReflectorManager should not be null." );
            }

            reflectorManager = r;
            editsDir = e;

            if( !Directory.Exists( editsDir ) )
            {
                try
                {
                    Directory.CreateDirectory( editsDir );
                }
                catch( Exception ex )
                {
                    throw new ArgumentException( "The edits backup directory does not exist amd could not be created.", ex );
                }
            }

            setSaveFile( null );
        }


        internal void setSaveFile( string file )
        {
            if( file == null )
            {
                currentSaveFile = null;
            }
            else
            {
                currentSaveFile = file;
            }
            currentDecryptDir = null;
        }

        internal bool hasSaveFile()
        {
            return currentSaveFile != null;
        }

        internal string extractSave( bool useTempDir )
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

            sign( currentSaveFile );

            string password = generatePassword( currentSaveFile );

            unpackSave( currentSaveFile, currentDecryptDir, password );

            return currentSaveFile;
        }

        internal SaveEditor getSaveEditor()
        {
            return new SaveEditor( currentDecryptDir );
        }

        internal string backupSave()
        {
            return backupSave( currentSaveFile, editsDir, true );
        }

        internal string repackDirAsSave()
        {
            // Don't create unencrypted saves where TAB or auto-backup watchers might see them
            string unencryptedSaveFile = Path.Combine( Path.GetTempPath(), Path.GetFileName( currentSaveFile ) );

            repackExtracted( unencryptedSaveFile, currentDecryptDir );

            sign( unencryptedSaveFile );

            string password = generatePassword( unencryptedSaveFile );

            // Purge the temporary unencrypted versions of this new save file
            File.Delete( unencryptedSaveFile );
            File.Delete( Path.ChangeExtension( unencryptedSaveFile, TAB.CHECK_EXTENSION ) );

            repackExtracted( currentSaveFile, currentDecryptDir, password );

            sign( currentSaveFile );

            return currentSaveFile;
        }

        internal void removeDecryptedDir()
        {
            // Completely unguarded w.r.t. state, or IO exceptions...
            Directory.Delete( currentDecryptDir, true );
        }

        internal void stopReflector()
        {
            if( reflectorManager.getState() == ReflectorManager.ReflectorState.STARTED )
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


        private string sign( string saveFile )
        {
            if( reflectorManager.getState() == ReflectorManager.ReflectorState.UNDEPLOYED )
            {
                reflectorManager.deployReflector();
            }

            if( reflectorManager.getState() == ReflectorManager.ReflectorState.DEPLOYED )
            {
                reflectorManager.startReflector();
            }
            
            string signature = reflectorManager.checksum( saveFile );
            //Console.WriteLine( "Signature from reflector: " + signature );
            return signature;
        }
        
        private string generatePassword( string saveFile )
        {
            if( reflectorManager.getState() == ReflectorManager.ReflectorState.UNDEPLOYED )
            {
                reflectorManager.deployReflector();
            }

            if( reflectorManager.getState() == ReflectorManager.ReflectorState.DEPLOYED )
            {
                reflectorManager.startReflector();
            }

            string password = reflectorManager.generatePassword( saveFile );
            //Console.WriteLine( "Password from reflector: " + password );
            return password;
        }
    }
}

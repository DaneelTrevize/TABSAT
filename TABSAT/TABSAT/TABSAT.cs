using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TABSAT
{
    class TABSAT
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
         *  wait for the TAB assembly to have launched, then use reflection to find the required methods to checksum and decrypt/encrypt saves;
         *  repeatedly invoke signing or decryption/encryption for each file path supplied
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

        private const string steamVDFsubPath = @"config\config.vdf";
        //example string libraryLine =        @"    ""BaseInstallFolder_1""		          ""K:\\SteamLibrary""";
        private const string libraryPattern = @"^\s+""BaseInstallFolder_(?<count>\d+)""\s+""(?<path>.+)""$";
        private const string steamTABsubDirectory = @"\steamapps\common\They Are Billions\";
        internal static readonly string defaultSavesDirectory = Environment.ExpandEnvironmentVariables( @"%USERPROFILE%\Documents\My Games\They Are Billions\Saves\" );
        internal static readonly string saveFilesFilter = "*" + TABReflector.TABReflector.saveExtension;
        internal static readonly string checkFilesFilter = "*" + TABReflector.TABReflector.checkExtension;
        private const string decryptionSuffix = @"_decrypted";
        private ReflectorManager reflectorManager;

        private string currentSaveFile;
        private string decryptDir;
        private string dataFile;


        internal static string findTABdirectory()
        {
            string steamConfigPath = findSteamConfig();
            if( steamConfigPath != null )
            {
                //Console.WriteLine( "Steam Libraries listed within: " + steamConfigPath );

                LinkedList<string> steamLibraries = findSteamLibraries( steamConfigPath );

                foreach( string library in steamLibraries )
                {
                    string TABdirectory = library + steamTABsubDirectory;
                    if( Directory.Exists( TABdirectory ) )
                    {
                        //Console.WriteLine( "Located TAB: " + TABdirectory );
                        return TABdirectory;
                    }
                }
            }
            return null;
        }

        internal static string findSteamConfig()
        {
            using( RegistryKey steamKey = Registry.CurrentUser.OpenSubKey( @"Software\Valve\Steam" ) )
            {
                if( steamKey != null )
                {
                    object SteamPathValue = steamKey.GetValue( "SteamPath" );
                    //Console.WriteLine( "Located Steam: " + SteamPathValue );
                    if( steamKey != null )
                    {
                        string steamConfigPath = Path.Combine( (string) SteamPathValue, steamVDFsubPath );
                        if( File.Exists( steamConfigPath ) )
                        {
                            return steamConfigPath;
                        }
                    }
                }
            }
            return null;
        }

        private static LinkedList<string> findSteamLibraries( string steamConfigPath )
        {
            LinkedList<string> steamLibraries = new LinkedList<string>();

            Regex libraryRegex = new Regex( libraryPattern, RegexOptions.Compiled );

            using( StreamReader config = new StreamReader( steamConfigPath ) )
            {
                string line;
                while( ( line = config.ReadLine() ) != null )
                {
                    Match match = libraryRegex.Match( line );
                    if( match.Success )
                    {
                        string libraryPath = match.Groups["path"].Value;
                        //Console.WriteLine( "SteamLibrary #" + match.Groups["count"].Value + ": " + libraryPath );
                        steamLibraries.AddFirst( libraryPath );
                    }
                }
            }

            return steamLibraries;
        }

        internal static string getReflectorDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        internal static string findMostRecentSave( string savesDir )
        {
            if( !Directory.Exists( savesDir ) )
            {
                throw new ArgumentException( "The provided saves directory does not exist." );
            }
            DirectoryInfo savesDirInfo = new DirectoryInfo( savesDir );
            FileInfo[] savesInfo = savesDirInfo.GetFiles( saveFilesFilter );
            if( savesInfo.Length > 0 )
            {
                IOrderedEnumerable<FileInfo> sortedSavesInfo = savesInfo.OrderByDescending( s => s.CreationTimeUtc );
                FileInfo newestSave = sortedSavesInfo.First();
                return newestSave.FullName;
            }
            return null;
        }


        private static string backupSave( string saveFile )     // Refactor this into new BackupsManager features?
        {
            if( !File.Exists( saveFile ) )
            {
                Console.Error.WriteLine( "Save file does not exist: " + saveFile );
                return null;
            }

            // Figure out a filename that isn't taken
            DirectoryInfo savesDirInfo = new DirectoryInfo( Path.GetDirectoryName( saveFile ) );
            string backupFilePrefix = Path.ChangeExtension( saveFile, TABReflector.TABReflector.saveExtension + ".bak" );
            FileInfo[] savesInfo = savesDirInfo.GetFiles( Path.GetFileName( saveFile ) + ".bak*" );	// This doesn't find all?
            string backupFile = backupFilePrefix + ( savesInfo.Length + 1 );

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
        /*
        private static void setFileTimes( string file, DateTime dt )
        {
            File.SetCreationTime( file, dt );
            File.SetLastWriteTime( file, dt );
            File.SetLastAccessTime( file, dt );
        }
        */


        public TABSAT( ReflectorManager r )
        {
            if( r == null )
            {
                throw new ArgumentNullException( "ReflectorManager should not be null." );
            }

            reflectorManager = r;

            setSaveFile( null );

            //reflectorManager.deployReflector();   // Don't automatically do this at initialisation, we might not need it
        }


        internal void setSaveFile( string file )
        {
            if( file == null )
            {
                currentSaveFile = null;
                decryptDir = null;
                dataFile = null;
            }
            else
            {
                currentSaveFile = file;
                decryptDir = Path.ChangeExtension( currentSaveFile, null ) + decryptionSuffix;
                dataFile = Path.Combine( decryptDir, "Data" );
            }
        }

        internal bool hasSaveFile()
        {
            return decryptDir != null;
        }

        internal string extractSave()
        {
            if( Directory.Exists( decryptDir ) )
            {
                return null;
            }
            else
            {
                Directory.CreateDirectory( decryptDir );

                decryptSaveToDir();
                return currentSaveFile;
            }
        }

        internal SaveEditor getSaveEditor()
        {
            return new SaveEditor( dataFile );
        }

        internal string backupSave()
        {
            return backupSave( currentSaveFile );
        }

        internal string repackDirAsSave()
        {
            string saveFile = currentSaveFile;  // decryptDir + TABReflector.TABReflector.saveExtension;

            repackExtracted( saveFile, decryptDir );

            sign( saveFile );

            string password = generatePassword( saveFile );

            /*
            DateTime now = DateTime.Now;

            // Let's modify the times on the dir
            Directory.SetCreationTime( decryptDir, now );
            Directory.SetLastWriteTime( decryptDir, now );
            Directory.SetLastAccessTime( decryptDir, now );

            foreach( string fileName in Directory.GetFiles( decryptDir ) )
            {
                setFileTimes( Path.Combine( decryptDir, fileName ), now );
            }

            // Update edit time. Must it be done before and after the passwording & signing?
            setFileTimes( saveFile, now );
            */

            repackExtracted( saveFile, decryptDir, password );

            sign( saveFile );
            /*
            // Update edit time. Must it be done before and after the passwording & signing?
            setFileTimes( saveFile, now );
            */
            
            removeDecryptedSaveAndCheck();

            return saveFile;
        }

        private void removeDecryptedSaveAndCheck()
        {
            // Purge the temporary decrypted versions of this new save file
            string decryptedSave = decryptDir + TABReflector.TABReflector.saveExtension;
            string decryptedCheck = decryptDir + TABReflector.TABReflector.checkExtension;
            File.Delete( decryptedSave );
            File.Delete( decryptedCheck );
        }

        internal void removeDecryptedDir()
        {
            Directory.Delete( decryptDir, true );
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


        private void decryptSaveToDir()
        {
            sign( currentSaveFile );

            string password = generatePassword( currentSaveFile );

            unpackSave( currentSaveFile, decryptDir, password );
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

using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TABSAT
{
    class TABSAT
    {
        /*
         *  This program will:
         *  Take a path to the TAB installation, or attempt to determine it;
         *  Take a path to the TAB save files to manipulate, or attempt to determine them;
         *  Present a GUI to determine what operation to perform on which save files;
         *  
         *  To decrypt/encrypt save files:
         *  copy the reflector binary to the TAB installation;
         *  invoke the reflector and open a pipe to it;
         *  pipe the paths to specific save files to manipulate;
         *  
         *  The reflector binary will:
         *  open a pipe to recieve the save file paths;
         *  start a thread to launch the TAB assembly;
         *  wait for the TAB assembly to have launched, then use reflection to find the required methods to checksum and decrypt/encrypt saves;
         *  repeatedly invoke decryption/encryption for each save file path supplied
         *  Exit? Or wait for a piped signal to do so, as subsequent paths may be determined? If so, would also need to know whether to decrypt or encrypt.
         *  Exiting will have to terminate the reflector's environment, in order to end the stalled TAB assembly thread.
         */

        private const string steamVDFsubPath = @"config\config.vdf";
        //example string libraryLine =        @"    ""BaseInstallFolder_1""		          ""K:\\SteamLibrary""";
        private const string libraryPattern = @"^\s+""BaseInstallFolder_(?<count>\d+)""\s+""(?<path>.+)""$";
        private const string steamTABsubDirectory = @"\steamapps\common\They Are Billions\";

        private static string defaultSavesDirectory = Environment.ExpandEnvironmentVariables( @"%USERPROFILE%\Documents\My Games\They Are Billions\Saves\" );

        private ReflectorManager reflectorManager;
        private string savesDirectory;


        public static string findTABdirectory()
        {
            string steamConfigPath = findSteamConfig();
            if( steamConfigPath != null )
            {
                Console.WriteLine( "Steam Libraries listed within: " + steamConfigPath );

                LinkedList<string> steamLibraries = findSteamLibraries( steamConfigPath );

                foreach( string library in steamLibraries )
                {
                    string TABdirectory = library + steamTABsubDirectory;
                    if( Directory.Exists( TABdirectory ) )
                    {
                        Console.WriteLine( "Located TAB: " + TABdirectory );
                        return TABdirectory;
                    }
                }
            }
            return null;
        }

        public static string findSteamConfig()
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

        public static string backupSave( string saveFile )
        {
            if( !File.Exists( saveFile ) )
            {
                Console.WriteLine( "Save file does not exist: " + saveFile );
                return null;
            }

            // Figure out a filename that isn't taken
            DirectoryInfo savesDirInfo = new DirectoryInfo( Path.GetDirectoryName( saveFile ) );
            string backupFilePrefix = Path.ChangeExtension( saveFile, TABReflector.TABReflector.saveExtension + ".bak" );
            FileInfo[] savesInfo = savesDirInfo.GetFiles( Path.GetFileName( saveFile ) + ".bak*" );	// This doesn't find all?
            string backupFile = backupFilePrefix + ( savesInfo.Length + 1 );

            if( File.Exists( backupFile ) )
            {
                Console.WriteLine( "Backup already exists: " + backupFile );
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


        public TABSAT( string reflectorDir, string TABdir, string savesDir, DataReceivedEventHandler outputHandler )
        {
            if( outputHandler == null )
            {
                throw new ArgumentNullException( "outputHandler should not be null." );
            }

            reflectorManager = new ReflectorManager( reflectorDir, TABdir );

            savesDirectory = savesDir;
            if( !Directory.Exists( savesDirectory ) )
            {
                throw new ArgumentException( "The provided saves directory does not exist." );
            }

            reflectorManager.deployAndStart( outputHandler );
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
                Console.WriteLine( "Extracted save file: " + saveFile );
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
                Console.WriteLine( "Save repacked: " + saveFile );
            }
        }
        
        private static void setFileTimes( string file, DateTime dt )
        {
            File.SetCreationTime( file, dt );
            File.SetLastWriteTime( file, dt );
            File.SetLastAccessTime( file, dt );
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainWindow( defaultSavesDirectory  ) );
            /*
            // Get the most recent save file, pipe it to the reflector
            DirectoryInfo savesDirInfo = new DirectoryInfo( tabSAT.savesDirectory );
            FileInfo[] savesInfo = savesDirInfo.GetFiles( "*" + TABReflector.TABReflector.saveExtension );
            IOrderedEnumerable<FileInfo> sortedSavesInfo = savesInfo.OrderByDescending( s => s.CreationTimeUtc );
            FileInfo newestSave = sortedSavesInfo.First();
            string saveFile = newestSave.FullName;
            //Console.WriteLine( "Found save file: " + saveFile );
            */
        }

        public void decryptSaveToDir( string saveFile, string decryptDir )
        {
            sign( saveFile );

            string password = generatePassword( saveFile );

            Directory.CreateDirectory( decryptDir );
            unpackSave( saveFile, decryptDir, password );
        }

        public string repackDirAsSave( string decryptDir, string saveFile = null )
        {
            if( saveFile == null )
            {
                saveFile = decryptDir + TABReflector.TABReflector.saveExtension;
            }

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
            return saveFile;
        }

        private void sign( string saveFile )
        {
            string signature = reflectorManager.checksum( saveFile );
            Console.WriteLine( "Signature from reflector: " + signature );
        }
        
        private string generatePassword( string saveFile )
        {
            string password = reflectorManager.generatePassword( saveFile );
            Console.WriteLine( "Password from reflector: " + password );
            return password;
        }

        public void tidyUp()
        {
            reflectorManager.stopAndRemove();
        }
    }
}

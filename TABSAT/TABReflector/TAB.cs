using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TABSAT
{
    public class TAB
    {
        public const string EXE_NAME = "TheyAreBillions.exe";
        public const string SAVE_EXTENSION = ".zxsav";
        public const string CHECK_EXTENSION = ".zxcheck";
        public const string SAVES_FILTER = "*" + SAVE_EXTENSION;
        public const string CHECKS_FILTER = "*" + CHECK_EXTENSION;

        private static readonly string STEAM_DEFAULT_32BIT_PATH = Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86 ) + @"\Steam";   // Default config and library path all in one
        private static readonly string STEAM_DEFAULT_64BIT_PATH = Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ) + @"\Steam";      // Fully 64bit Steam isn't currently a thing, but we're possibly futureproofing by having this
        private const string STEAM_LIBRARYFOLDERS_VDF_SUBPATH = @"config\libraryfolders.vdf";   // Ignore the copy in \steamapps\libraryfolders.vdf?
        private const string STEAM_CONFIG_VDF_SUBPATH = @"config\config.vdf";                   // Has been depreciated as a listing of libraries?
        //example string                                    		"path"		"E:\\SteamLibrary"
        private const string STEAM_LIBRARYFOLDERS_LIBRARY_PATTERN = @"^\s+""path""\s+""(?<path>.+)""$";
        //example string libraryLine =               @"    ""BaseInstallFolder_1""		         ""K:\\SteamLibrary""";
        private const string STEAM_CONFIG_LIBRARY_PATTERN = @"^\s+""BaseInstallFolder_(?<count>\d+)""\s+""(?<path>.+)""$";
        private const string STEAM_TAB_SUBPATH = @"\steamapps\common\They Are Billions\";

        public static readonly string DEFAULT_SAVES_DIRECTORY = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\My Games\They Are Billions\Saves";

        public static string GetExeDirectory()
        {
            string steamPath = GetSteamPath();
            if( steamPath != null )
            {
                LinkedList<string> steamLibraries = new LinkedList<string>();

                string steamLibraryFoldersPath = Path.Combine(steamPath, STEAM_LIBRARYFOLDERS_VDF_SUBPATH);
                if( File.Exists(steamLibraryFoldersPath) )
                {
                    //Console.WriteLine( "Steam Libraries listed within: " + steamLibraryFoldersPath );
                    Regex libraryRegex = new Regex( STEAM_LIBRARYFOLDERS_LIBRARY_PATTERN, RegexOptions.Compiled );
                    GetSteamLibraries( steamLibraryFoldersPath, steamLibraries, libraryRegex );
                }

                if( steamLibraries.Count == 0 )
                {
                    string steamConfigPath = Path.Combine(steamPath, STEAM_CONFIG_VDF_SUBPATH);
                    if( File.Exists(steamConfigPath) )
                    {
                        //Console.WriteLine( "Steam Libraries listed within: " + steamConfigPath );
                        Regex libraryRegex = new Regex( STEAM_CONFIG_LIBRARY_PATTERN, RegexOptions.Compiled );
                        GetSteamLibraries( steamConfigPath, steamLibraries, libraryRegex );
                    }
                }

                if( steamLibraries.Count == 0 )
                {
                    steamLibraries.AddLast(steamPath);
                }

                foreach( string library in steamLibraries )
                {
                    string TABdirectory = library + STEAM_TAB_SUBPATH;
                    if( Directory.Exists( TABdirectory ) )
                    {
                        //Console.WriteLine( "Located TAB: " + TABdirectory );
                        return TABdirectory;
                    }
                }
            }

            Console.Error.WriteLine( "They Are Billions was not found." );
            return null;
        }

        private static string GetSteamPath()
        {
            using( RegistryKey steamKey = Registry.CurrentUser.OpenSubKey( @"Software\Valve\Steam" ) )
            {
                if( steamKey != null )
                {
                    object SteamPathValue = steamKey.GetValue( "SteamPath" );
                    //Console.WriteLine( "Located Steam: " + SteamPathValue );
                    if( steamKey != null )
                    {
                        string steamPath = (string) SteamPathValue;
                        if( Directory.Exists( steamPath ) )
                        {
                            return steamPath;
                        }
                    }
                }
            }

            Console.Error.WriteLine( "Steam registry value was not found." );
            if( File.Exists( STEAM_DEFAULT_32BIT_PATH ) )
            {
                return STEAM_DEFAULT_32BIT_PATH;
            }
            if( File.Exists( STEAM_DEFAULT_64BIT_PATH ) )
            {
                return STEAM_DEFAULT_64BIT_PATH;
            }

            return null;
        }

        private static void GetSteamLibraries( string steamConfigPath, LinkedList<string> steamLibraries, Regex libraryRegex )
        {
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
        }

        public static string GetMostRecentSave( string savesDir )
        {
            if( !Directory.Exists( savesDir ) )
            {
                //throw new ArgumentException( "The provided saves directory does not exist." );
                return null;
            }
            DirectoryInfo savesDirInfo = new DirectoryInfo( savesDir );
            FileInfo[] savesInfo = savesDirInfo.GetFiles( SAVES_FILTER );
            if( savesInfo.Length > 0 )
            {
                IOrderedEnumerable<FileInfo> sortedSavesInfo = savesInfo.OrderByDescending( s => s.LastWriteTimeUtc );
                FileInfo newestSave = sortedSavesInfo.First();
                return newestSave.FullName;
            }
            return null;
        }

        public static string GetCheckFile( string saveFile )
        {
            return Path.ChangeExtension( saveFile, CHECK_EXTENSION );
        }

        public static bool IsFileWithinDirectory( string file, string directory )
        {
            return Path.GetFullPath( file ).StartsWith( Path.GetFullPath( directory ) );
        }
    }
}

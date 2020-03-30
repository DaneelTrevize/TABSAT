using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace TABSAT
{
    public enum PipeFlowControl
    {
        GenerateChecksum = 'C',
        GeneratePassword = 'P',
        Quit = 'Q'/*,
        UnrecognisedCommand = 'U',
        Error = 'E',
        Result = 'R'*/
    }

    public class TABReflector
    {
        private static Assembly tabAssembly;
        private static Type ZXProgram;
        private static MethodInfo billionsMain;

        private static Type signingClass;
        private static MethodInfo getGameAccountMethod;
        private static MethodInfo signingMethod;

        private static Type ZipSerializer;
        private static PropertyInfo propCurrentZip;
        private static PropertyInfo propPassword;
        private static MethodInfo flagMethod;
        private static MethodInfo generatorMethod;

        private NamedPipeClientStream pipe;
        private StreamReader pipeReader;
        private StreamWriter pipeWriter;

        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );
        private const int SW_MINIMIZE = 6;

        public TABReflector( string handle )
        {
            pipe = new NamedPipeClientStream( ".", handle, PipeDirection.InOut );
            //Console.WriteLine( "Reflector TransmissionMode: {0}.", pipe.TransmissionMode );

            pipeReader = new StreamReader( pipe );
            pipeWriter = new StreamWriter( pipe );
        }

        private void run()
        {
            pipe.Connect();
            Console.WriteLine( "Reflector listening." );

            bool keepRunning = true;
            string command;
            string value;
            do
            {
                command = readPipe();
                switch( command[0] )
                {
                    case (char) PipeFlowControl.GenerateChecksum:
                        //writePipe( "Awaiting file path." );
                        value = readPipe();
                        string signature = generateZXCheck( value );
                        if( signature != null )
                        {
                            Console.WriteLine( "Signature: " + signature );
                            writePipe( signature );
                        }
                        else
                        {
                            writePipe( "Failed to generated zxcheck." );
                        }
                        break;
                    case (char) PipeFlowControl.GeneratePassword:
                        //writePipe( "Awaiting file path." );
                        value = readPipe();
                        // Checksum file is required or passwork generator invocation will fail. Assume the manager checked this.
                        string password = generatePassword( value );
                        Console.WriteLine( "Password: " + password );
                        writePipe( password );
                        break;
                    case (char) PipeFlowControl.Quit:
                        //writePipe( "Quitting." );
                        Console.WriteLine( "Reflector stopping." );
                        pipe.Close();
                        /*try
                        {
                            pipeReader.Dispose();
                        }
                        catch( ObjectDisposedException ode ) { }
                        try
                        {
                            pipeWriter.Dispose();
                        }
                        catch( ObjectDisposedException ode ) { }*/
                        keepRunning = false;
                        break;
                    default:
                        Console.Error.WriteLine( "Unrecognised command: " + command );
                        writePipe( "Unrecognised command: " + command );
                        break;
                }
            } while( keepRunning );
        }

        private string readPipe()
        {
            if( pipeReader == null )
            {
                throw new InvalidOperationException( "Pipe has not been initialised." );
            }

            String temp = null;
            try
            {
                while( temp == null )
                {
                    temp = pipeReader.ReadLine();
                }
            }
            catch( IOException e )
            {
                Console.Error.WriteLine( "Reading pipe error: {0}", e.Message );
            }
            return temp;
        }

        private void writePipe( string value )
        {
            if( pipeWriter == null )
            {
                throw new InvalidOperationException( "Reflector pipe has not been initialised." );
            }
            try
            {
                pipeWriter.WriteLine( value );
                pipeWriter.Flush();
            }
            catch( IOException e )
            {
                Console.Error.WriteLine( "Writing pipe error: {0}", e.Message );
            }
        }

        private static Assembly getTABAssembly()
        {
            if( tabAssembly == null )
            {
                // Attempt to load the assembly
                try
                {
                    tabAssembly = Assembly.Load( Path.GetFileNameWithoutExtension( TAB.EXE_NAME ) );

                    if( tabAssembly == null )
                    {
                        throw new Exception( "TAB assembly didn't load." );
                    }
                    else
                    {
                        Console.WriteLine( "Loaded TAB assembly." );
                    }
                }
                catch( Exception e )
                {
                    Console.Error.WriteLine( "Failed to load TAB assembly." );
                    Console.Error.WriteLine( e.Message );
                    Environment.Exit( -1 );
                }
            }
            return tabAssembly;
        }
        
        private static Type getZXProgram()
        {
            if( ZXProgram == null )
            {
                // Grab ZXProgram
                ZXProgram = getTABAssembly().GetType( "ZX.Program" );
                if( ZXProgram == null )
                {
                    Console.Error.WriteLine( "Failed to load main program class." );
                    Environment.Exit( -1 );
                }
            }
            return ZXProgram;
        }
        
        private static MethodInfo getBillionsMain()
        {
            if( billionsMain == null )
            {
                // Get a reference to the Main method
                billionsMain = getZXProgram().GetMethod( "Main", BindingFlags.Static | BindingFlags.NonPublic );
                if( billionsMain == null )
                {
                    Console.Error.WriteLine( "Failed to find the main method." );
                    Environment.Exit( -1 );
                }
            }
            return billionsMain;
        }
        
        private static void initialiseBillionsAndStall( string[] args )
        {
            try
            {
                getBillionsMain().Invoke( getZXProgram(), new object[] { args } );
                /*foreach( var a in getTABAssembly().GetReferencedAssemblies() )
                {
                    Console.WriteLine( a );
                }*/
                //getTABAssembly().GetTypes();
            }/*
            catch( ReflectionTypeLoadException rtle )
            {
                Console.Error.WriteLine( rtle.Message );
                Console.Error.WriteLine( rtle.Types.Length + ", " + rtle.LoaderExceptions.Length );
                for( int t = 0, e = 0; t < rtle.Types.Length && e < rtle.LoaderExceptions.Length; t++ )
                {
                    if( rtle.Types[t] == null )
                    {
                        Console.Error.WriteLine( rtle.LoaderExceptions[e++] );
                    }
                }
            }*/
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                if( e.InnerException != null )
                {
                    Console.Error.WriteLine( e.InnerException.Message );
                    //Console.Error.WriteLine( e.InnerException.StackTrace );
                }
            }
        }

        private static Type getSigningClass()
        {
            if( signingClass == null )
            {
                foreach( Type type in getTABAssembly().GetTypes() )
                {
                    try
                    {
                        getGameAccountMethod = type.GetMethod( "get_GameAccount" );
                        if( getGameAccountMethod != null )
                        {
                            Console.WriteLine( "Found signing class:\t" + type );
                            signingClass = type;
                            break;
                        }
                    }
                    catch( Exception e )
                    {
                        Console.Error.WriteLine( e.Message );
                    }
                }

                if( signingClass == null )
                {
                    Console.Error.WriteLine( "Failed to find the signing class." );
                }
            }

            return signingClass;
        }

        private static MethodInfo getSigningMethod()
        {
            if( signingMethod == null )
            {
                Type type = getSigningClass();
                if( type == null )
                {
                    return null;
                }

                Console.WriteLine( "Sig-scanning for signing method..." );

                // Loop over the class again, we need to locate the checksum method
                foreach( MethodInfo info in type.GetMethods( BindingFlags.Static | BindingFlags.NonPublic ) )
                {

                    // Grab the params, our target takes 2
                    ParameterInfo[] parInfo = info.GetParameters();
                    if( parInfo.Length == 2 )
                    {
                        // We are looking for String + Int32
                        if( parInfo[0].ParameterType.Name == "String" && parInfo[1].ParameterType.Name == "Int32" )
                        {
                            // Located our method
                            Console.WriteLine( "Found signing method:\t" + info.Name );
                            signingMethod = info;

                            break;
                        }
                    }
                }

                if( signingMethod == null )
                {
                    Console.Error.WriteLine( "Failed to find the signing method." );
                }
            }
            return signingMethod;
        }

        private static string generateZXCheck( string saveFile )
        {
            MethodInfo signingMethod = getSigningMethod();
            if( signingMethod == null || getGameAccountMethod == null )
            {
                Console.Error.WriteLine( "Failed to generate .zxcheck" );
                return null;
            }

            //Console.WriteLine( "Signing file: " + saveFile );
            if( !File.Exists( saveFile ) )
            {
                Console.Error.WriteLine( "The save file: " + saveFile + " does not exist." );
                return null;
            }

            try
            {
                object sigObj = signingMethod.Invoke( getGameAccountMethod, new object[] { saveFile, 2 } );
                string signature = (string) sigObj;

                return signature;
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                return null;
            }
        }

        private static Type getZipSerializer()
        {
            if( ZipSerializer == null )
            {
                Assembly ass = Assembly.Load( "DXVision" );
                //Console.WriteLine( ass.GetName() + " located in: " + ass.CodeBase );
                ZipSerializer = ass.GetType( "DXVision.Serialization.ZipSerializer" );
            }
            return ZipSerializer;
        }

        private static PropertyInfo getCurrentZip()
        {
            if( propCurrentZip == null )
            {
                Type z = getZipSerializer();
                if( z == null )
                {
                    Console.Error.WriteLine( "Unable to get Type ZipSerializer." );
                    return null;
                }
                propCurrentZip = z.GetProperty( "Current", BindingFlags.Static | BindingFlags.Public );
            }
            return propCurrentZip;
        }

        private static PropertyInfo getZipPassword()
        {
            if( propPassword == null )
            {

                Type z = getZipSerializer();
                if( z == null )
                {
                    Console.Error.WriteLine( "Unable to get Type ZipSerializer." );
                    return null;
                }
                propPassword = z.GetProperty( "Password", BindingFlags.Instance | BindingFlags.Public );
            }
            return propPassword;
        }

        private static string tryGetPassword( string saveFile, MethodInfo candidateFlagMethod, MethodInfo candidateGeneratorMethod )
        {
            try
            {
                int flag = (int) candidateFlagMethod.Invoke( null, new object[] { saveFile } );
                //Console.WriteLine( "Flag value: " + flag );
                candidateGeneratorMethod.Invoke( null, new object[] { saveFile, flag, true } );

                object activeZip = getCurrentZip().GetValue( null, null );
                //Console.WriteLine( "activeZip: " + activeZip.ToString() );
                return (string) getZipPassword().GetValue( activeZip, null );
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( e.Message );
                return null;
            }
        }

        private static string generatePassword( string saveFile )
        {
            if( flagMethod == null || generatorMethod == null )
            {
                Console.WriteLine( "Locating password generator..." );

                LinkedList<Type> candidateTypes = new LinkedList<Type>();
                // Get encryption helper Type candidates
                foreach( Type possibleType in getTABAssembly().GetTypes() )
                {
                    MethodInfo sigMatch = possibleType.GetMethod( "ProcessSpecialKeys_KeyUp", BindingFlags.Instance | BindingFlags.NonPublic );
                    if( sigMatch != null )
                    {
                        candidateTypes.AddLast( possibleType );
                    }
                }
                //Console.WriteLine( "candidateTypes count: " + candidateTypes.Count );

                // For Type's methods, there will be 1 to generate a flag, and one to generate the password, identified by return Type and Parameters
                foreach( Type candidateType in candidateTypes )
                {
                    // First hunt for a Flag method
                    LinkedList<MethodInfo> candidateFlagMethods = new LinkedList<MethodInfo>();
                    foreach( MethodInfo candidateFlagMethod in candidateType.GetMethods( BindingFlags.NonPublic | BindingFlags.Static ) )
                    {
                        if( candidateFlagMethod.ReturnType.FullName != "System.Int32" ) continue;

                        ParameterInfo[] flagParams = candidateFlagMethod.GetParameters();
                        if( flagParams.Length != 1 ) continue;
                        if( flagParams[0].ParameterType.FullName != "System.String" ) continue;

                        candidateFlagMethods.AddLast( candidateFlagMethod );
                    }
                    //Console.WriteLine( "candidateFlagMethods count: " + candidateFlagMethods.Count );

                    // Next hunt for a password generator method
                    LinkedList<MethodInfo> candidateGeneratorMethods = new LinkedList<MethodInfo>();
                    foreach( MethodInfo candidateGeneratorMethod in candidateType.GetMethods( BindingFlags.NonPublic | BindingFlags.Static ) )
                    {
                        if( candidateGeneratorMethod.ReturnType.FullName != "System.Void" ) continue;

                        ParameterInfo[] helperParams = candidateGeneratorMethod.GetParameters();
                        if( helperParams.Length != 3 ) continue;
                        if( helperParams[0].ParameterType.FullName != "System.String" ) continue;
                        if( helperParams[1].ParameterType.FullName != "System.Int32" ) continue;
                        if( helperParams[2].ParameterType.FullName != "System.Boolean" ) continue;

                        candidateGeneratorMethods.AddLast( candidateGeneratorMethod );
                    }
                    //Console.WriteLine( "candidateGeneratorMethods count: " + candidateGeneratorMethods.Count );

                    // To know we have the right methods, we must invoke them both using a save file, and check if a reasonable password value is generated
                    foreach( MethodInfo candidateFlagMethod in candidateFlagMethods )
                    {
                        foreach( MethodInfo candidateGeneratorMethod in candidateGeneratorMethods )
                        {
                            string password = tryGetPassword( saveFile, candidateFlagMethod, candidateGeneratorMethod );

                            // Did we find the password?
                            if( password == null || password.Length <= 0 ) continue;

                            Console.WriteLine( "Found password generator, used:" );
                            Console.WriteLine( "Type:\t\t\t" + candidateType.Name );
                            Console.WriteLine( "Flag method:\t\t" + candidateFlagMethod.Name );
                            Console.WriteLine( "Generator method:\t\t" + candidateGeneratorMethod.Name );
                            flagMethod = candidateFlagMethod;
                            generatorMethod = candidateGeneratorMethod;

                            return password;
                        }
                    }
                }
                Console.Error.WriteLine( "No password generator found." );
                return null;
            }
            else
            {
                return tryGetPassword( saveFile, flagMethod, generatorMethod );
            }
        }

        static void Main( string[] args )
        {
            if( args.Length != 1 )
            {
                Console.Error.WriteLine( "Pipe handle must be supplied." );
                Environment.Exit( -1 );
            }

            if( !File.Exists( AppDomain.CurrentDomain.BaseDirectory + TAB.EXE_NAME ) )
            {
                Console.Error.WriteLine( "TAB executable not found." );
                Environment.Exit( -1 );
            }
            
            new Thread( () => {
                initialiseBillionsAndStall( new string[] { "" } );
            } ).Start();
            // Now give the TAB thread time to do what we need
            //Thread.Sleep( 2000 );
            using( Process self = Process.GetCurrentProcess() )
            {
                for( int i = 0; i < 10; i++ )
                {
                    Thread.Sleep( 200 );
                    try
                    {
                        IntPtr popup = self.MainWindowHandle;
                        if( popup != IntPtr.Zero )
                        {
                            Console.WriteLine( "Popup detected, cycles: " + i );

                            try
                            {
                                ShowWindow( popup, SW_MINIMIZE );
                                Console.WriteLine( "Popup minimised." );
                            }
                            catch( InvalidOperationException ioe )
                            {
                                Console.Error.WriteLine( "Unable to obtain the Reflector's main window handle. " + ioe.Message );
                            }

                            break;
                        }
                    }
                    catch( InvalidOperationException ioe )
                    {
                        Console.WriteLine( "Waiting for popup." );
                        Thread.Sleep( 1800 );
                        break;
                    }
                }
            }
            
            TABReflector reflector = new TABReflector( args[0] );

            reflector.run();

            Environment.Exit( 0 );
        }
    }
}

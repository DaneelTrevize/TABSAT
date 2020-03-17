using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TABSAT
{
    class ReflectorManager
    {
        internal enum ReflectorState
        {
            UNDEPLOYED,
            DEPLOYED,
            STARTED,
            PROCESSING
        }

        private const string REFLECTOR_EXE = "TABReflector.exe";
        private const string REFLECTOR_RESOURCE_NAME = @"costura.tabreflector.exe.compressed";
        private const string pipeName = "reflectorpipe";

        private string reflectorDeploymentPath;

        private Process reflector;
        private NamedPipeServerStream reflectorPipe;
        private StreamWriter reflectorWriter;
        private StreamReader reflectorReader;

        private DataReceivedEventHandler outputHandler;
        private ReflectorState state;   // Should lock() access?


        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );
        private const int SW_MINIMIZE = 6;

        public ReflectorManager( string TABdirectory )
        {
            if( !Directory.Exists( TABdirectory ) )
            {
                throw new ArgumentException( "The provided TAB directory does not exist." );
            }

            reflectorDeploymentPath = Path.Combine( TABdirectory, REFLECTOR_EXE );

            resetReflector();

            outputHandler = defaultOutputHandler;

            state = ReflectorState.UNDEPLOYED;  // An assumption?
        }

        private void defaultOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            /*if( reflectorTextBox.InvokeRequired )     // Waiting for the right thread stalls the realtime displaying of Reflector process output
            {
                reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
            }
            else
            {*/
            if( !String.IsNullOrEmpty( outLine.Data ) )
            {
                //Console.WriteLine( outLine.Data );
            }
            //}
        }

        internal void setOutputHandler( DataReceivedEventHandler outHandler )
        {
            outputHandler = outHandler;
        }


        private void resetReflector()
        {
            reflector = null;
            reflectorPipe = null;
            reflectorWriter = null;
            reflectorReader = null;
        }

        private void writePipe( string value )
        {
            if( reflectorWriter == null )
            {
                throw new InvalidOperationException( "Reflector pipe has not been initialised." );
            }
            try
            {
                reflectorWriter.WriteLine( value );
                reflectorWriter.Flush();
            }
            catch( IOException e )
            {
                Console.Error.WriteLine( "Writing pipe error: {0}", e.Message );
            }
        }

        private string readPipe()
        {
            if( reflectorReader == null )
            {
                throw new InvalidOperationException( "Reflector pipe has not been initialised." );
            }

            string temp = null;
            try
            {
                while( temp == null )
                {
                    temp = reflectorReader.ReadLine();
                }
            }
            catch( IOException e )
            {
                Console.Error.WriteLine( "Reading pipe error: {0}", e.Message );
            }
            return temp;
        }


        internal ReflectorState getState()
        {
            return state;
        }

        internal void deployReflector( bool overwrite = true )
        {
            if( state != ReflectorState.UNDEPLOYED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting deployment." );
            }

            if( File.Exists( reflectorDeploymentPath ) && !overwrite )
            {
                Console.WriteLine( "Reflector file: " + REFLECTOR_EXE + " was already deployed, skipping." );
            }
            else
            {
                using( var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( REFLECTOR_RESOURCE_NAME ) )
                using( var compressStream = new DeflateStream( stream, CompressionMode.Decompress ) )
                using( var file = File.Create( reflectorDeploymentPath ) )
                {
                    compressStream.CopyTo( file );
                }
            }
            // Should checksum, whether overwriting or skipping?

            state = ReflectorState.DEPLOYED;
            //Console.WriteLine( "Reflector deployed." );
        }

        internal void removeReflector()
        {
            if( state != ReflectorState.DEPLOYED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting removal." );
            }

            if( !File.Exists( reflectorDeploymentPath ) )
            {
                Console.WriteLine( "Reflector file: " + REFLECTOR_EXE + " was not already deployed." );
            }
            else
            {
                File.Delete( reflectorDeploymentPath );
            }

            state = ReflectorState.UNDEPLOYED;
            //Console.WriteLine( "Reflector removed." );
        }

        internal void startReflector()
        {
            if( state != ReflectorState.DEPLOYED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting starting." );
            }

            reflector = new Process();
            reflector.StartInfo.FileName = reflectorDeploymentPath;

            reflectorPipe = new NamedPipeServerStream( pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message );
            //Console.WriteLine( "Pipe TransmissionMode: {0}.", reflectorPipe.TransmissionMode );

            reflector.StartInfo.Arguments = pipeName;
            reflector.StartInfo.UseShellExecute = false;
            // Redirect all console writes to the 1 handler
            reflector.StartInfo.CreateNoWindow = true;
            reflector.StartInfo.RedirectStandardOutput = true;
            reflector.OutputDataReceived += outputHandler;
            reflector.StartInfo.RedirectStandardError = true;
            reflector.ErrorDataReceived += outputHandler;

            reflector.Start();
            reflector.BeginOutputReadLine();
            reflector.BeginErrorReadLine();
            //Console.WriteLine( "Reflector started." );

            reflectorWriter = new StreamWriter( reflectorPipe );
            //reflectorWriter.AutoFlush = true;
            reflectorReader = new StreamReader( reflectorPipe );

            reflectorPipe.WaitForConnection();
            try
            {
                //reflector.WaitForInputIdle( 1000 );
                //Console.WriteLine( "Reflector window title: " + reflector.MainWindowTitle );
                IntPtr popup = reflector.MainWindowHandle;
                ShowWindow( popup, SW_MINIMIZE );
                //Console.WriteLine( "Reflector window minimised." );
                popup = IntPtr.Zero;    // Needless as it goes out of scope now anyway, and doesn't implement IDisposable/can't be wrapped in using()?
            }
            catch( InvalidOperationException ioe )
            {
                Console.Error.WriteLine( "Unable to obtain the Reflector's main window handle. " + ioe.Message );
            }
            state = ReflectorState.STARTED;
        }

        internal void stopReflector()
        {
            if( state != ReflectorState.STARTED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting stopping." );
            }

            state = ReflectorState.PROCESSING;

            writePipe( Char.ToString( (char) PipeFlowControl.Quit ) );

            reflectorPipe.Close();
            //Console.WriteLine( "Pipe closed." );

            reflector.WaitForExit();
            reflector.Close();

            reflector.Dispose();
            resetReflector();

            state = ReflectorState.DEPLOYED;
            //Console.WriteLine( "Reflector stopped." );
        }

        internal string generatePassword( string saveFile )
        {
            if( state != ReflectorState.STARTED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting processing." );
            }

            state = ReflectorState.PROCESSING;

            writePipe( Char.ToString( (char) PipeFlowControl.Generate ) );
            writePipe( saveFile );
            string password = readPipe();

            state = ReflectorState.STARTED;
            return password;
        }

        internal string checksum( string saveFile )
        {
            if( state != ReflectorState.STARTED )
            {
                throw new InvalidOperationException( "Reflector is not awaiting processing." );
            }

            state = ReflectorState.PROCESSING;

            writePipe( Char.ToString( (char) PipeFlowControl.Checksum ) );
            writePipe( saveFile );
            string signature = readPipe();

            state = ReflectorState.STARTED;
            return signature;
        }
    }
}

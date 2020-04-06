using Octokit;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static TABSAT.MainWindow;

namespace TABSAT
{
    class UpdatesManager
    {
        private const string USER_AGENT = "TABSAT";
        private const string GITHUB_USER = "DaneelTrevize";
        private const string GITHUB_REPO = "TABSAT";

        private readonly StatusWriterDelegate statusWriter;
        private readonly string lastKnownMasterTag = Properties.Resources.masterTag;      // Value captured at build time, thus always lags behind latest tagged build

        internal UpdatesManager( StatusWriterDelegate s )
        {
            statusWriter = s;

            checkUpdates();
        }

        private async void checkUpdates()
        {
            var releases = await Task.Run( () => {
                var client = new GitHubClient( new ProductHeaderValue( USER_AGENT ) );
                return client.Repository.Release.GetAll( GITHUB_USER, GITHUB_REPO );
            } );

            if( releases.Count < 2 )
            {
                statusWriter( "Unexpectedly few Releases were found." );
                return;
            }
            
            var latest = releases[0];
            if( latest.TagName.Equals( lastKnownMasterTag ) )
            {
                statusWriter( "You appear to be running an unversioned development build of TABSAT > v" + lastKnownMasterTag );
                return;
            }

            var previous = releases[1];
            if( previous.TagName.Equals( lastKnownMasterTag ) )
            {
                statusWriter( "You are running the latest released version, TABSAT v" + latest.TagName );
            }
            else
            {
                statusWriter( "An updated version is available, TABSAT v" + latest.TagName );
            }
        }
    }
}

using Jint;
using Jint.Runtime;
using ScriptedDL.InfoParser;
using ScriptedDL.ScriptInterfaces;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptedDL.ScriptLoader
{
    /// <summary>
    /// Defines the MessageEventHandler format
    /// </summary>
    /// <param name="sender">The event emmiter</param>
    /// <param name="msg">The message sent</param>
    public delegate void MessageEventHanlder ( object sender, String msg );

    /// <summary>
    /// Defines the ErrorEventHandler format
    /// </summary>
    /// <param name="sender">The event emmiter</param>
    /// <param name="err">The error message</param>
    public delegate void ErrorEventHandler ( object sender, String err );

    /// <summary>
    /// Defines the ScriptDoneHandler
    /// </summary>
    /// <param name="sender">The event emmiter</param>
    public delegate void ScriptDoneHandler ( object sender );

    public class ScriptRunner
    {
        private String FN;
        private ScriptInfo Info;

        #region Event Handling

        /// <summary>
        /// Indicates when the log() function was used
        /// </summary>
        public event MessageEventHanlder Messaged;

        /// <summary>
        /// Indicates when the error() function was used or an exception was thrown
        /// </summary>
        public event ErrorEventHandler Errored;

        /// <summary>
        /// Indicates when the script has finished running
        /// </summary>
        public event ScriptDoneHandler Finished;

        /// <summary>
        /// Called when log() function is used
        /// </summary>
        /// <param name="e">The message</param>
        protected virtual void OnMessage ( String e )
        {
            Messaged?.Invoke ( this, e );
        }

        /// <summary>
        /// Called when the error() function is used or an exception was thrown
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnError ( String e )
        {
            Errored?.Invoke ( this, e );
        }

        /// <summary>
        /// Called when the script is done running
        /// </summary>
        protected virtual void OnFinish ( )
        {
            Finished?.Invoke ( this );
        }

        #endregion Event Handling

        public ScriptRunner ( String FileName )
        {
            Info = InfoParser.InfoParser.GetInfo ( FileName );
        }

        /// <summary>
        /// Runs the script asynchronously
        /// </summary>
        /// <param name="URL">The URL to pass to the script</param>
        /// <returns></returns>
        public Thread RunScript ( String URL )
        {
            var eng = GetJSEngine ( );

            using ( var reader = new StreamReader ( Info.Path, Encoding.UTF8 ) )
            {
                var code = reader.ReadToEnd ( );

                var t = new Thread ( ( ) =>
                {
                    try
                    {
                        eng.SetValue ( "url", URLMatch ( URL ) )
                           .Execute ( code );
                    }
                    catch ( JavaScriptException ex )
                    {
                        OnError ( ex.ToString ( ) );
                    }
                    catch(Exception ex)
                    {
                        OnError ( ex.ToString ( ) );
                    }

                    OnFinish ( );
                } );

                t.SetApartmentState ( ApartmentState.STA );
                t.Start ( );

                return t;
            }
        }

        /// <summary>
        /// Returns the prepared Jint Engine
        /// </summary>
        /// <returns></returns>
        private Engine GetJSEngine ( )
        {
            var modname = Path.GetFileNameWithoutExtension ( Info.Path );

            var eng = new Engine ( cfg =>
            {
                cfg.DebugMode ( )
                    .Strict ( )
                    .LimitRecursion ( 20 );
            } );

            eng.SetValue ( "http", new HTTP ( modname, eng ) )
                .SetValue ( "io", new IO ( modname, eng ) )
                .SetValue ( "log", ( Action<String> ) Log )
                .SetValue ( "error", ( Action<String> ) Err );

            return eng;
        }

        /// <summary>
        /// Returns the URL Groups as a String array to be passed to the Js
        /// </summary>
        /// <param name="URL">The URL to perform the match on</param>
        /// <returns>The match groups</returns>
        private String[] URLMatch ( String URL )
        {
            var match = Info.URLFormat.Match ( URL );
            var ret = new String[match.Groups.Count];

            for ( var i = 0 ; i < match.Groups.Count ; i++ )
                ret[i] = match.Groups[i].Value;

            return ret;
        }

        // log(string s)
        private void Log ( String s )
        {
            OnMessage ( s );
        }

        // error(string s)
        private void Err ( String s )
        {
            OnError ( s );
        }
    }
}
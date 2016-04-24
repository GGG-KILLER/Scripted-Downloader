using Jint;
using Jint.Native;
using System;
using System.Net;
using System.Windows.Forms;

namespace ScriptedDL.ScriptInterfaces
{
    internal class HTTP
    {
        /// <summary>
        /// You can change the User Agent if you want
        /// </summary>
        public String UA = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";

        private String modname;
        private Engine eng;

        public HTTP ( String modname, Engine eng )
        {
            this.modname = modname;
            this.eng = eng;
        }

        public JsValue getString ( String URL )
        {
            using ( var client = new WebClient ( ) )
            {
                client.Headers.Set ( "user-agent", UA );

                return JsValue.FromObject ( eng, client.DownloadString ( URL ) );
            }
        }

        /// <summary>
        /// Downloads binary data from a URL
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <returns></returns>
        public byte[] getData ( String URL )
        {
            // We can't convert to JsValue here since the io.writeData and io.appendData depend on byte[]
            using ( var client = new WebClient ( ) )
            {
                client.Headers.Set ( "user-agent", UA );

                return client.DownloadData ( URL );
            }
        }

        /// <summary>
        /// Returns a document element from a URL
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <returns></returns>
        public JsValue getDoc ( String URL )
        {
            using ( var browser = new WebBrowser ( ) )
            {
                browser.ScriptErrorsSuppressed = true;
                browser.Navigate ( URL );

                while ( browser.ReadyState != WebBrowserReadyState.Complete )
                    Application.DoEvents ( );

                return JsValue.FromObject ( eng, ( dynamic ) browser.Document.DomDocument );
            }
        }

        public JsValue getJson(String URL)
        {
            using ( var client = new WebClient ( ) )
            {
                client.Headers.Set ( "user-agent", UA );

                var str = client.DownloadString ( URL );
                return eng.Json.Parse ( eng.Json, new JsValue[] { JsValue.FromObject ( eng, str ) } );
            }
        }

        /// <summary>
        /// Downloads a file from a URL to a provided path
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="Path">The path to download to</param>
        public void dlFile ( String URL, String Path )
        {
            using ( var client = new WebClient ( ) )
            {
                var io = new IO ( modname, eng );
                io.mkdir ( System.IO.Path.GetDirectoryName ( Path ) );

                client.Headers.Set ( "user-agent", UA );

                client.DownloadFile ( URL, io.formatPath ( Path ) );
            }
        }
    }
}
using Jint;
using Jint.Native;
using System;
using System.IO;

namespace ScriptedDL.ScriptInterfaces
{
    internal class IO
    {
        private String modname;
        private Engine eng;

        public IO ( String modname, Engine eng )
        {
            this.modname = modname;
            this.eng = eng;
        }

        public String formatPath ( String path )
        {
            return Path.GetFullPath ( Path.Combine ( Settings.Save_Path, modname, "./" + path.Replace ( "..", "." ) ) );
        }

        public void writeString ( String path, String contents )
        {
            mkdir ( Path.GetDirectoryName ( path ) );

            using ( var stream = new StreamWriter ( formatPath ( path ) ) )
            {
                stream.Write ( contents );
            }
        }

        public void appendString ( String path, String contents )
        {
            mkdir ( Path.GetDirectoryName ( path ) );

            using ( var stream = new StreamWriter ( formatPath ( path ), true ) )
            {
                stream.Write ( contents );
            }
        }

        public void writeData ( String path, byte[] bytes )
        {
            mkdir ( Path.GetDirectoryName ( path ) );

            using ( Stream stream = File.OpenWrite ( formatPath ( path ) ) )
            {
                stream.Write ( bytes, 0, bytes.Length );
            }
        }

        public void appendData ( String path, byte[] bytes )
        {
            mkdir ( Path.GetDirectoryName ( path ) );

            using ( Stream stream = File.OpenWrite ( formatPath ( path ) ) )
            {
                stream.Write ( bytes, ( int ) stream.Length, bytes.Length );
            }
        }

        public void mkdir ( String path )
        {
            //if ( path != "" && path != "/" )
            Directory.CreateDirectory ( formatPath ( path ) );
        }

        public void writeJson ( String path, JsValue val )
        {
            writeString ( path, eng.Json.Stringify ( eng.Json, new JsValue[] { val } ).AsString ( ) );
        }

        public JsValue readJson ( String path )
        {
            using ( var reader = new StreamReader ( formatPath ( path ) ) )
            {
                return eng.Json.Parse ( eng.Json, new JsValue[] {
                    JsValue.FromObject ( eng, reader.ReadToEnd ( ) )
                } );
            }
        }
    }
}
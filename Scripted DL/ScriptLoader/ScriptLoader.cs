using ScriptedDL.InfoParser;
using System;
using System.IO;

namespace ScriptedDL.ScriptLoader
{
    public class ScriptLoader
    {
        public static String[] GetScripts ( String Dir )
        {
            try
            {
                return Directory.GetFiles ( Dir, "*.js", SearchOption.TopDirectoryOnly );
            }
            catch ( Exception )
            {
                return null;
            }
        }

        public static ScriptInfo LoadScriptInfo ( String FileName )
        {
            return InfoParser.InfoParser.GetInfo ( $"Scripts/{FileName}" );
        }

        public static ScriptRunner LoadScript ( String FileName )
        {
            return new ScriptRunner ( FileName );
        }
    }
}
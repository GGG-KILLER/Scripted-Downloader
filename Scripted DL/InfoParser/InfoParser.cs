using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ScriptedDL.Utilities;

namespace ScriptedDL.InfoParser
{
    internal class InfoParser
    {
        public static ScriptInfo GetInfo ( String Name )
        {
            var info = new ScriptInfo
            {
                Path = Name
            };
            var shouldParse = false;
            var shouldStopParsing = false;

            using ( var reader = new StreamReader ( Name, Encoding.UTF8 ) )
            {
                String line;
                while ( !reader.EndOfStream && !shouldStopParsing && ( line = reader.ReadLine ( ) ) != null )
                {
                    ContainsSwitch.Do (
                        line,
                        ContainsSwitch.Case ( "==DLScript==", ( ) => { shouldParse = true; } ),
                        ContainsSwitch.Case ( "==/DLScript==", ( ) =>
                        {
                            shouldParse = false;
                            shouldStopParsing = true;
                        } ),
                        ContainsSwitch.Case ( "@name", ( ) =>
                        {
                            if ( shouldParse )
                                info.Name = line.GetDLScriptInfoParam ( "@name" );
                        } ),
                        ContainsSwitch.Case ( "@url", ( ) =>
                        {
                            if ( shouldParse )
                                info.URLFormat = new Regex ( line.GetDLScriptInfoParam ( "@url" ), RegexOptions.ECMAScript | RegexOptions.IgnoreCase );
                        } ),
                        ContainsSwitch.Case ( "@version", ( ) =>
                        {
                            if ( shouldParse )
                                info.Version = line.GetDLScriptInfoParam ( "@version" );
                        } ),
                        ContainsSwitch.Case ( "@author", ( ) =>
                        {
                            if ( shouldParse )
                                info.Author = line.GetDLScriptInfoParam ( "@author" );
                        } ),
                        ContainsSwitch.Case ( "@homepage", ( ) =>
                        {
                            if ( shouldParse )
                                info.Homepage = line.GetDLScriptInfoParam ( "@homepage" );
                        } )
                    );
                }
            }

            if ( info.Name == null || info.Name == "" || info.URLFormat == null || info.URLFormat == default ( Regex ) )
                throw new Exception ( "No Name or URL Format were informed." );

            return info;
        }
    }

    internal static class ParserStringExtensions
    {
        public static String GetDLScriptInfoParam ( this String Line, String Name )
        {
            return Line.Substring ( Line.IndexOf ( Name ) + Name.Length ).Trim ( );
        }
    }
}
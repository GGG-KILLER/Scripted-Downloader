using System;
using System.Text.RegularExpressions;

namespace ScriptedDL.InfoParser
{
    /// <summary>
    /// Represents a script's information
    /// </summary>
    public class ScriptInfo
    {
        /// <summary>
        /// The name of the script
        /// </summary>
        public String Name;

        /// <summary>
        /// The URL format that should be adopted
        /// </summary>
        public Regex URLFormat;

        /// <summary>
        /// The author of the script (optional)
        /// </summary>
        public String Author;

        /// <summary>
        /// The homepage of the script (optional)
        /// </summary>
        public String Homepage;

        /// <summary>
        /// The version of the script (optional, but recommended)
        /// </summary>
        public String Version;

        /// <summary>
        /// Path to the script
        /// </summary>
        public String Path;
    }
}
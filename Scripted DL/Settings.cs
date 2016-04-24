using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ScriptedDL
{
    [Serializable]
    internal class SettingsSerialize
    {
        public String Save_Path;
    }

    /// <summary>
    /// Settings class
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// Defines if settings were already loaded
        /// </summary>
        private static bool Loaded = false;

        /// <summary>
        /// The path the downloads will be saved to
        /// </summary>
        public static String Save_Path;

        /// <summary>
        /// The config file path
        /// </summary>
        private const String FN = "settings.conf";

        /// <summary>
        /// Saves the settings to the config file
        /// </summary>
        public static void Save ( )
        {
            // Creates a dummy instance with the data we want to save
            var obj = new SettingsSerialize
            {
                Save_Path = Save_Path
            };

            // Fires up the serializer and serializes the object to the memory
            var serializer = new BinaryFormatter ( );
            using ( Stream stream = File.OpenWrite ( FN ) )
            {
                serializer.Serialize ( stream, obj );
            }
        }

        /// <summary>
        /// Loads the settings from the file
        /// </summary>
        public static void Load ( )
        {
            SettingsSerialize obj;
            var serializer = new BinaryFormatter ( );
            // Creates a stream and then deserializes the settings
            using ( Stream stream = File.OpenRead ( FN ) )
                obj = ( SettingsSerialize ) serializer.Deserialize ( stream );

            // Actually loads the settings
            Save_Path = obj.Save_Path;
        }

        /// <summary>
        /// Static constructor that loads configs when needed
        /// </summary>
        static Settings ( )
        {
            if ( !Loaded )
            {
                if ( File.Exists ( FN ) )
                {
                    Load ( );
                }
                else
                {
                    Save_Path = "";
                }
                Loaded = true;
            }
        }
    }
}
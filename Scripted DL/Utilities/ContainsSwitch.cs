using System;

namespace ScriptedDL.Utilities
{
    internal class ContainsSwitch
    {
        public static void Do ( String str, params Switch.SCase<String>[] cases )
        {
            Switch.Do ( str, cases );
        }

        public static Switch.SCase<String> Case ( String Contains, Action Act )
        {
            return new Switch.SCase<String>
            {
                Cond = str => str.Contains ( Contains ),
                Act = s => Act?.Invoke ( )
            };
        }
    }
}
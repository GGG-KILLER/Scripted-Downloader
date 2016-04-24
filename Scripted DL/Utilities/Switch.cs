using System;

namespace ScriptedDL.Utilities
{
    /// <summary>
    /// Used for complex switch cases which rely on different actions
    /// rather than equality checking over a variable
    /// </summary>
    internal class Switch
    {
        /// <summary>
        /// A class that represents a switch case
        /// </summary>
        /// <typeparam name="T">The type of parameter that is accepted</typeparam>
        public class SCase<T>
        {
            public Func<T, Boolean> Cond;
            public Action<T> Act;
        }

        /// <summary>
        /// The switch case itself
        /// </summary>
        /// <typeparam name="T">The type of the variable to be checked</typeparam>
        /// <param name="param">The variable to be checked</param>
        /// <param name="cases">The switch cases</param>
        public static void Do<T> ( T param, params SCase<T>[] cases )
        {
            foreach ( var @case in cases )
                if ( @case.Cond ( param ) )
                    @case.Act ( param );
        }

        /// <summary>
        /// Creates a switch case
        /// </summary>
        /// <typeparam name="T">The type of variable to be accepted</typeparam>
        /// <param name="Cond">The condition to check</param>
        /// <param name="Act">The action to run upon the condition is met</param>
        /// <returns>The case</returns>
        public static SCase<T> Case<T> ( Func<T, Boolean> Cond, Action<T> Act )
        {
            return new SCase<T>
            {
                Cond = Cond,
                Act = Act
            };
        }
    }
}
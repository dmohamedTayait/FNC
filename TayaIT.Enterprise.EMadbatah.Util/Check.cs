using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    /// <summary>
    /// This is a Design By Contract class that you can use in any C# class.
    /// Reference : http://www.codeproject.com/csharp/designbycontract.asp
    /// </summary>
    public sealed class Check
    {
        #region Interface

        /// <summary>
        /// Precondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Throw NotImplementException() if called.
        /// </summary>
        public static void DoNotCall()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Precondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Require(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Precondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Require(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PreconditionException("Precondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Precondition failed.");
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION")]
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION")]
        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION")]
        public static void Ensure(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new PostconditionException("Postcondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Postcondition failed.");
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT")]
        public static void Invariant(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException(message);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT")]
        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT")]
        public static void Invariant(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new InvariantException("Invariant failed.");
            }
            else
            {
                Trace.Assert(assertion, "Invariant failed.");
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Assert(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
                //Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void Assert(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion) throw new AssertionException("Assertion failed.");
            }
            else
            {
                Trace.Assert(assertion, "Assertion failed.");
            }
        }

        /// <summary>
        /// Assert that the object is not null
        /// </summary>
        /// <param name="o"></param>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void AssertNotNull(object o)
        {
            Assert(o != null);
        }

        /// <summary>
        /// Check if any of the given string is Null or Empty
        /// </summary>
        /// <param name="strs"></param>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void AssertValidString(params string[] strs)
        {
            foreach (string s in strs)
            {
                if (String.IsNullOrEmpty(s))
                {
                    if (UseExceptions)
                    {
                        throw new ArgumentException(" string cannot be Null or Empty");
                    }
                    else
                    {
                        Trace.Assert(false, " string cannot be Null or Empty");
                    }
                }
            }
        }

        /// <summary>
        /// Check if any of the given object is Null
        /// </summary>
        /// <param name="objs"></param>
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void AssertNotNull(params object[] objs)
        {
            foreach (object o in objs)
            {
                AssertNotNull(o);
            }
        }

        /// <summary>
        /// Set this if you wish to use Trace Assert statements
        /// instead of exception handling.
        /// (The Check class uses exception handling by default.)
        /// </summary>
        public static bool UseAssertions
        {
            get
            {
                return _useAssertions;
            }
            set
            {
                _useAssertions = value;
            }
        }

        #endregion // Interface

        #region Implementation

        // No creation
        private Check() { }

        /// <summary>
        /// Is exception handling being used?
        /// </summary>
        private static bool UseExceptions
        {
            get
            {
                return !_useAssertions;
            }
        }

        // Are trace assertion statements being used?
        // Default is to use exception handling.
        private static bool _useAssertions = false;

        #endregion // Implementation

        #region Obsolete

        /// <summary>
        /// Precondition check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Require")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void RequireTrace(bool assertion, string message)
        {
            Trace.Assert(assertion, "Precondition: " + message);
        }


        /// <summary>
        /// Precondition check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Require")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION"),
        Conditional("DBC_CHECK_PRECONDITION")]
        public static void RequireTrace(bool assertion)
        {
            Trace.Assert(assertion, "Precondition failed.");
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Ensure")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION")]
        public static void EnsureTrace(bool assertion, string message)
        {
            Trace.Assert(assertion, "Postcondition: " + message);
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Ensure")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT"),
        Conditional("DBC_CHECK_POSTCONDITION")]
        public static void EnsureTrace(bool assertion)
        {
            Trace.Assert(assertion, "Postcondition failed.");
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Invariant")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT")]
        public static void InvariantTrace(bool assertion, string message)
        {
            Trace.Assert(assertion, "Invariant: " + message);
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Invariant")]
        [Conditional("DBC_CHECK_ALL"),
        Conditional("DBC_CHECK_INVARIANT")]
        public static void InvariantTrace(bool assertion)
        {
            Trace.Assert(assertion, "Invariant failed.");
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Assert")]
        [Conditional("DBC_CHECK_ALL")]
        public static void AssertTrace(bool assertion, string message)
        {
            Trace.Assert(assertion, "Assertion: " + message);
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        [Obsolete("Set Check.UseAssertions = true and then call Check.Assert")]
        [Conditional("DBC_CHECK_ALL")]
        public static void AssertTrace(bool assertion)
        {
            Trace.Assert(assertion, "Assertion failed.");
        }
        #endregion // Obsolete

    } // End Check

    #region Exceptions

    /// <summary>
    /// Exception raised when a contract is broken.
    /// Catch this exception type if you wish to differentiate between
    /// any DesignByContract exception and other runtime exceptions.
    ///
    /// </summary>
    public class DesignByContractException : ApplicationException
    {
        protected DesignByContractException() { }
        protected DesignByContractException(string message) : base(message) { }
        protected DesignByContractException(string message, Exception inner)
            :
                                                       base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a precondition fails.
    /// </summary>
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner)
            :
                                                base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException() { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message) : base(message) { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner)
            :
                                                  base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an invariant fails.
    /// </summary>
    public class InvariantException : DesignByContractException
    {
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException() { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message) : base(message) { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message, Exception inner)
            :
                                               base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an assertion fails.
    /// </summary>
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException() { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message) : base(message) { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner)
            :
                                                base(message, inner) { }
    }

    #endregion // Exception classes

    // End Design By Contract
}

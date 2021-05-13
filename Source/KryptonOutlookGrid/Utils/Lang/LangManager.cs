//--------------------------------------------------------------------------------
// Copyright (C) 2013-2021 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://github.com/Cocotteseb/Krypton-OutlookGrid/blob/master/LICENSE.md
//
// Visit https://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace JDHSoftware.Krypton.Toolkit.Utils.Lang
{
    /// <summary>
    /// Handle localization (singleton)
    /// </summary>
    public class LangManager
    {
        // Variable locale pour stocker une référence vers l'instance
        private static LangManager mInstance = null;

        private static readonly object mylock = new object();
        private ResourceManager rm;

        private CultureInfo ci;
        //Used for blocking critical sections on updates
        private object locker = new object();

        // Le constructeur est Private
        private LangManager()
        {
            rm = new ResourceManager("JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Utils.Lang.Strings", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture; //CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets or sets the P locker.
        /// </summary>
        /// <value>The P locker.</value>
        public object PLocker
        {
            get { return this.locker; }
            set { this.locker = value; }
        }

        /// <summary>
        /// Gets the instance of the singleton.
        /// </summary>
        public static LangManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mylock)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new LangManager();
                        }
                    }
                }

                return mInstance;
            }
        }

        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetString(string name)
        {
            return rm.GetString(name, ci);
        }
    }
}

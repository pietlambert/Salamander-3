﻿using FreeBuild.Base;
using FreeBuild.Model;
using Newt.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Newt
{
    /// <summary>
    /// The core manager class.
    /// Deals with general file and data handline and overall top-level
    /// applicaton management.
    /// Implemented as a singleton - call Core.Instance to access the current
    /// </summary>
    public class Core : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private internal singleton instance
        /// </summary>
        private static Core _Instance = null;

        /// <summary>
        /// Get the singleton instance of the core object.
        /// If the core has not yet been initialised this will return null.
        /// Call EnsureInitialisation() on the application host object before
        /// accessing this if you are unsure whether initialisation has already
        /// taken place.
        /// </summary>
        public static Core Instance { get { return _Instance; } }

        /// <summary>
        /// Get the host application interface
        /// </summary>
        public IHost Host { get; }

        /// <summary>
        /// The action manager class.
        /// Use this to load and call plug-in actions.
        /// </summary>
        public ActionManager Actions { get; }

        /// <summary>
        /// The currently active open document
        /// </summary>
        public ModelDocument ActiveDocument { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="host"></param>
        private Core(IHost host)
        {
            Host = host;
            Actions = new ActionManager();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialise the core object to the specified host.
        /// </summary>
        /// <param name="host">An object which implements the IHost interface
        /// to allow the Core to interact with the hosting application.</param>
        /// <returns></returns>
        public static void Initialise(IHost host)
        {
            if (_Instance == null) _Instance = new Core(host);
            _Instance.LoadPlugins();
        }

        /// <summary>
        /// Is the core initialised?
        /// </summary>
        /// <returns>True if the core instance is initialised and available to access.
        /// Else, false.</returns>
        public static bool IsInitialised()
        {
            return Instance != null;
        }

        /// <summary>
        /// Load all plugin assemblies
        /// </summary>
        private void LoadPlugins()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileInfo executing = new FileInfo(location);
            DirectoryInfo directory = executing.Directory;
            DirectoryInfo[] subDirectories = directory.GetDirectories("Plugins");
            foreach (DirectoryInfo pluginFolder in subDirectories)
            {
                FileInfo[] files = pluginFolder.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Extension == ".dll")
                    {
                        string filePath = file.FullName;
                        Assembly pluginAss = Assembly.LoadFrom(filePath);
                        if (pluginAss != null)
                        {
                            PrintLine("Loading Newt plugin '" + filePath + "'...");
                            //Load Actions:
                            Actions.LoadPlugin(pluginAss);
                            //Load Layers:
                            //TODO - something like:
                            //Layers.LoadPlugin(pluginAss);
                        }
                        else
                        {
                            PrintLine("Warning: Could not load assembly '" + filePath + "'!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Print a message output.
        /// This is a shortcut method that reaches through to the Instance.Host.Print method.
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public static bool Print(string message)
        {
            return Instance.Host.Print(message);
        }

        /// <summary>
        /// Print a message output followed by a newline character.
        /// This is a shortcut method that reaches through to the Instance.Host.Print method.
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public static bool PrintLine(string message)
        {
            return Instance.Host.Print(message + Environment.NewLine);
        }

        #endregion
    }
}
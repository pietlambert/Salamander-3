﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newt
{
    /// <summary>
    /// Interface for application hosts.
    /// Application hosts are responsible for initialising the core
    /// instance and for linking to key interaction logic within
    /// the target application - for example selection of objects,
    /// 3D rendering, user interface creation and so on.
    /// </summary>
    public interface IHost
    {
        #region Properties

        /// <summary>
        /// Get the GUI controller that this host provides.
        /// </summary>
        GUIController GUI { get; }

        /// <summary>
        /// Get the input controller class that this host provides.
        /// </summary>
        InputController Input { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 'Print' a message - displaying it in some form within the host
        /// application. 
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>True if the message was printed successfully.</returns>
        bool Print(string message);

        /// <summary>
        /// Cause the host application to refresh any display elements which
        /// must be manually triggered.
        /// </summary>
        void Refresh();

        #endregion
    }
}

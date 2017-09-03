﻿using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salamander.Display
{
    /// <summary>
    /// Abstract base class for display layers
    /// </summary>
    public abstract class DisplayLayerBase : NotifyPropertyChangedBase, IRenderable
    {
        /// <summary>
        /// Internal backing member for Visible
        /// </summary>
        private bool _Visible = false;

        /// <summary>
        /// Layer visibility.
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                NotifyPropertyChanged("Visible");
            }
        }

        /// <summary>
        /// Can this layer be toggled on and off?
        /// </summary>
        public virtual bool Toggleable { get { return true; } }

        /// <summary>
        /// Is this display layer a dynamic preview layer that should be updated during position selection events
        /// </summary>
        public bool IsDynamic { get; protected set; }

        public abstract bool Draw(RenderingParameters parameters);

        /// <summary>
        /// Shortcut method to create a new mesh-type avatar suitable for use in the host application.
        /// The mesh avatar will be generated by the current host application's AvatarFactory.
        /// </summary>
        /// <returns></returns>
        public IMeshAvatar CreateMeshAvatar()
        {
            return Core.Instance.Host.AvatarFactory.CreateMeshAvatar();
        }

        /// <summary>
        /// Shortcut method to create a new line type avatar suitable for use in the host application.
        /// The line avatar will be generated by the current host application's AvatarFactory.
        /// </summary>
        /// <returns></returns>
        public ICurveAvatar CreateCurveAvatar()
        {
            return Core.Instance.Host.AvatarFactory.CreateCurveAvatar();
        }

        /// <summary>
        /// Shortcut method to create a new line type avatar suitable for use in the host application.
        /// The line avatar will be generated by the current host application's AvatarFactory.
        /// </summary>
        /// <returns></returns>
        public ICurveAvatar CreateCurveAvatar(Curve curve)
        {
            var result = CreateCurveAvatar();
            result.Curve = curve;
            return result;
        }

        /// <summary>
        /// Shortcut method to create a new coordinate system type avatar suitable for use in the host
        /// application.  The coordinate system avatar will be generated by the current host application's
        /// AvatarFactory.
        /// </summary>
        /// <returns></returns>
        public ICoordinateSystemAvatar CreateCoordinateSystemAvatar()
        {
            return Core.Instance.Host.AvatarFactory.CreateCoordinateSystemAvatar();
        }

        /// <summary>
        /// Shortcut method to create a new coordinate system type avatar suitable for use in the host
        /// application.  The coordinate system avatar will be generated by the current host application's
        /// AvatarFactory.
        /// </summary>
        /// <returns></returns>
        public ICoordinateSystemAvatar CreateCoordinateSystemAvatar(ICoordinateSystem cSystem)
        {
            var result = CreateCoordinateSystemAvatar();
            result.CoordinateSystem = cSystem;
            return result;
        }

        /// <summary>
        /// Shortcut method to create a new Label type avatar suitable for use in the host application.
        /// </summary>
        /// <returns></returns>
        public ILabelAvatar CreateLabelAvatar()
        {
            return Core.Instance.Host.AvatarFactory.CreateLabelAvatar();
        }


    }
}

﻿using FreeBuild.Rendering;
using FreeBuild.Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC = Rhino.Geometry;


namespace Salamander.Rhino
{
    public class RhinoRenderingParameters : RenderingParameters
    {
        /// <summary>
        /// The Rhino DrawEventArgs containing the current pipeline
        /// </summary>
        public DrawEventArgs Args { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args"></param>
        public RhinoRenderingParameters(DrawEventArgs args)
        {
            Args = args;
        }

        /// <summary>
        /// Get point converter
        /// </summary>
        /// <param name="args"></param>
        public RhinoRenderingParameters(GetPointDrawEventArgs args) : this((DrawEventArgs)args)
        {
            CursorPosition = RCtoFB.Convert(args.CurrentPoint);
        }

        /// <summary>
        /// Draw the specified Rhino Object in the current viewport
        /// </summary>
        /// <param name="rObj"></param>
        /// <returns></returns>
        public void Draw(RhinoObject rObj)
        {
            Args.Display.DrawObject(rObj);
        }

        /// <summary>
        /// Draw the specified mesh with the specified material in the current viewport
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="material"></param>
        public void Draw(RC.Mesh mesh, DisplayMaterial material)
        {
            Args.Display.DrawMeshShaded(mesh, material);
        }

        /// <summary>
        /// Draw the specified extrusion as a surface wireframe
        /// </summary>
        /// <param name="extrusion"></param>
        /// <remarks>TODO: Replace with shaded drawing method</remarks>
        public void Draw(RC.Extrusion extrusion)
        {
            Args.Display.DrawSurface(extrusion, Color.Black, 1);
        }

        /// <summary>
        /// Draw a shaded BRep
        /// </summary>
        /// <param name="brep"></param>
        /// <param name="material"></param>
        public void Draw(RC.Brep brep, DisplayMaterial material)
        {
            Args.Display.DrawBrepShaded(brep, material);
        }

        /// <summary>
        /// Draw a coloured polyline
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="colour"></param>
        /// <param name="thickness"></param>
        public void Draw(IEnumerable<RC.Point3d> polyline, Color colour, int thickness)
        {
            Args.Display.DrawPolyline(polyline, colour, thickness);
        }

        /// <summary>
        /// Draw a coloured point cloud
        /// </summary>
        /// <param name="points"></param>
        /// <param name="colour"></param>
        /// <param name="size"></param>
        public void Draw(RC.PointCloud points, Color colour, double size)
        {
            Args.Display.DrawPointCloud(points, (int)size, colour);
        }
    }
}

﻿using Nucleus.Model;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Nucleus.Rhino;
using RC = Rhino.Geometry;
using FB = Nucleus.Geometry;
using Grasshopper.Kernel;
using Nucleus.Actions;
using Rhino;
using Rhino.DocObjects;

namespace Salamander.Grasshopper
{
    /// <summary>
    /// Geometric Goo for Linear Elements
    /// </summary>
    public class LinearElementGoo : GH_Goo<LinearElement>, ISalamander_Goo, IGH_PreviewData, IGH_BakeAwareData
    {
        #region Properties

        public override string TypeDescription
        {
            get
            {
                return "Salamander Linear Element";
            }
        }

        public override string TypeName
        {
            get
            {
                return "Linear Element";
            }
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return ToRC.Convert(Value.Geometry.BoundingBox);
            }
        }

        public override bool IsValid
        {
            get
            {
                return Value != null;
            }
        }

        /// <summary>
        /// Private backing field for ExInfo property
        /// </summary>
        private ExecutionInfo _ExInfo;

        /// <summary>
        /// The execution information for the operation which created this object
        /// </summary>
        public ExecutionInfo ExInfo
        {
            get { return _ExInfo; }
        }

        private Mesh _SectionMesh = null;

        public Mesh SectionMesh
        {
            get
            {
                if (_SectionMesh == null)
                {
                    RhinoMeshBuilder mB = new RhinoMeshBuilder();
                    mB.AddSectionPreview(Value);
                    mB.Finalize();
                    _SectionMesh = mB.Mesh;
                }
                return _SectionMesh;
            }
        }

        #endregion

        #region Constructors

        public LinearElementGoo() : base() { }

        public LinearElementGoo(LinearElement element, ExecutionInfo exInfo = null) : base(element)
        {
            _ExInfo = exInfo;
        }

        #endregion

        #region Methods

        //public override IGH_GeometricGoo DuplicateGeometry()
        //{
        //    return new LinearElementGoo(Value);
        //}

        //public override BoundingBox GetBoundingBox(Transform xform)
        //{
        //    RC.Curve rCrv = FBtoRC.Convert(Value.Geometry);
        //    if (rCrv != null)
        //    {
        //        rCrv.Transform(xform);
        //        return rCrv.GetBoundingBox(false);
        //    }
        //    else return BoundingBox.Unset;
        //}

        //public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        //{
        //    throw new NotImplementedException();
        //}

        public override string ToString()
        {
            return "Linear Element " + Value.NumericID;
        }

        //public override IGH_GeometricGoo Transform(Transform xform)
        //{
        //    throw new NotImplementedException();
        //}

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if (Value != null)
            {
                RhinoMeshBuilder builder = new RhinoMeshBuilder();
                builder.AddSectionPreview(Value);
                builder.Finalize();
                args.Display.DrawMeshShaded(builder.Mesh, args.ShadeMaterial);
            }
        }

        public static List<LinearElementGoo> Convert(LinearElementCollection collection, ExecutionInfo exInfo = null)
        {
            var result = new List<LinearElementGoo>();
            if (collection != null)
                foreach (LinearElement obj in collection) result.Add(new LinearElementGoo(obj, exInfo));
            return result;
        }

        object ISalamander_Goo.GetValue(Type type)
        {
            if (type == typeof(LinearElementCollection))
                return new LinearElementCollection(Value);
            else if (type == typeof(ElementCollection))
                return new ElementCollection(Value);
            else return Value;
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value?.Geometry != null)
            {
                if (Value.Geometry is FB.Line)
                    args.Pipeline.DrawLine(
                        new Line(ToRC.Convert(Value.Geometry.StartPoint), ToRC.Convert(Value.Geometry.EndPoint)), args.Color);
                else if (Value.Geometry is FB.Arc)
                    args.Pipeline.DrawArc(
                        new Arc(ToRC.Convert(Value.Geometry.StartPoint), ToRC.Convert(Value.Geometry.PointAt(0.5)), ToRC.Convert(Value.Geometry.EndPoint)),
                        args.Color);

                args.Pipeline.DrawMeshWires(SectionMesh, args.Color);
            }
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            //TODO
            if (Value?.Geometry != null)
            {
                args.Pipeline.DrawMeshShaded(SectionMesh, args.Material);
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new LinearElementGoo(Value, ExInfo);
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(ElementGoo)))
            {
                target = (Q)(object)new ElementGoo(Value);
                return true;
            }
            else if (typeof(Q).IsAssignableFrom(typeof(GH_Curve)))
            {
                target = (Q)((object)new GH_Curve(ToRC.Convert(Value.Geometry)));
                return true;
            }
            else if (typeof(Q).IsAssignableFrom(typeof(GH_Line)))
            {
                if (Value.Geometry is FB.Line)
                {
                    target = (Q)((object)new GH_Line(ToRC.ConvertToLine((FB.Line)Value.Geometry)));
                    return true;
                } 
            }
            else if (typeof(Q).IsAssignableFrom(typeof(GH_Mesh)))
            {
                if (SectionMesh != null)
                {
                    target = (Q)((object)new GH_Mesh(SectionMesh));
                    return true;
                }
            }
            else if (typeof(Q).IsAssignableFrom(typeof(GH_Surface)))
            {
                var surface = ToRC.ConvertToExtrusion(Value);
                target = (Q)(object)new GH_Surface(surface);
                return surface != null;
            }
            else if (typeof(Q).IsAssignableFrom(typeof(GH_Brep)))
            {
                target = (Q)(object)new GH_Brep(ToRC.ConvertToBrep(Value));
                return true;
            }
            return base.CastTo<Q>(ref target);
        }

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = Guid.Empty;
            if (GrasshopperManager.Instance.AutoBake)
                return false;
            else
            {
                var result = Core.Instance.ActiveDocument.Model.Create.CopyOf(Value, null, null);
                obj_guid = Guid.Empty;
                return true;
                //TODO: Bake section
            }

        }

        #endregion
    }
}

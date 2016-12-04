﻿using FreeBuild.Actions;
using FreeBuild.Geometry;
using FreeBuild.Model;
using FreeBuild.UI;
using Salamander.Actions;
using Salamander.Display;

namespace Salamander.TestPlugin
{
    [Action("DrawLinearElement",
            "Create a new linear element between two points.")]
    public class DrawLinearElementAction : ModelActionBase
    {
        [ActionInput(1, "the set-out geometry of the new element")]
        public Line Line { get; set; }

        [AutoUIComboBox("AvailableSections")]
        [ActionInput(Manual = false, Persistant = true)]
        public SectionProperty Section { get; set; }

        public SectionPropertyCollection AvailableSections { get { return Model.Properties.Sections; } }

        [ActionOutput(1, "the created element")]
        public LinearElement Element { get; set; }

        public override bool Execute(ExecutionInfo exInfo = null)
        {
            if (Line.Length > 0)
            {
                Element = Model.Create.LinearElement(Line, exInfo);
                Element.Property = Section;
                return true;
            }
            return false;
        }

        public override DisplayLayer PreviewLayer(PreviewParameters parameters)
        {
            if (parameters.IsDynamic && parameters.CursorPoint.IsValid() && parameters.BasePoint.IsValid() && Section != null)
            {
                ManualDisplayLayer layer = new ManualDisplayLayer();
                IMeshAvatar mesh = layer.CreateMeshAvatar();
                mesh.Builder.AddSectionPreview(new Line(parameters.BasePoint, parameters.CursorPoint), Section, 0);
                mesh.FinalizeMesh();
                layer.Add(mesh);
                return layer;
            }
            return null;
        }
    }
}

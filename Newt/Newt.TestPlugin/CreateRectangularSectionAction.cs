﻿using Salamander.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Actions;
using Nucleus.Model;
using Nucleus.Geometry;

namespace Salamander.BasicTools
{
    [Action(
        "CreateRectangularSection",
        "Create a new section property with a rectangular profile",
        IconBackground = Resources.URIs.RectangularSection,
        IconForeground = Resources.URIs.AddIcon)]
    public class CreateRectangularSectionAction : ModelActionBase
    {

        [ActionInput(1, "the name of the section")]
        public string Name { get; set; } = "Rectangular Section";

        [ActionInput(2, "the depth of the section", Manual = false)]
        public double Depth { get; set; } = 0.3;

        [ActionInput(3, "the width of the section", Manual = false)]
        public double Width { get; set; } = 0.3;

        [ActionInput(7, "the material of the section", Required = false, Manual = false)]
        public Material Material { get; set; }

        [ActionOutput(1, "the output section property")]
        public SectionFamily Section { get; set; }

        [ActionOutput(2, "the output section perimeter")]
        public Curve Perimeter
        {
            get { return Section?.Profile?.Perimeter; }
        }

        public override bool Execute(ExecutionInfo exInfo = null)
        {
            RectangularProfile rProfile = new RectangularProfile(Depth, Width);
            rProfile.Material = Material;
            Section = Model.Create.SectionFamily(Name, exInfo);
            Section.Profile = rProfile;
            return true;
        }

        public override bool PostExecutionOperations(ExecutionInfo exInfo = null)
        {
            if (exInfo == null && Section != null)
            {
                // Select the new section
                Core.Instance.Selected.Select(Section);
            }
            return base.PostExecutionOperations(exInfo);
        }
    }
}

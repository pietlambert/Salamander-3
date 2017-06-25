﻿using Salamander.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Actions;
using Nucleus.Model;

namespace Salamander.BasicTools
{
    [Action("CreateBuildUp",
        "Create a new single-layer panel build-up family",
        IconBackground = Resources.URIs.BuildUpFamily,
        IconForeground = Resources.URIs.AddIcon)]
    public class CreateBuildUp : ModelDocumentActionBase
    {
        [ActionInput(1, "the name of the section")]
        public string Name { get; set; } = "Build-Up";

        [ActionInput(2, "the thickness of the build-up")]
        public double Thickness { get; set; } = 0.1;

        [ActionOutput(2, "The output panel build-up")]
        public BuildUpFamily BuildUp { get; set; }

        public override bool Execute(ExecutionInfo exInfo = null)
        {
            BuildUp = Model.Create.BuildUpFamily(Name, exInfo);
            BuildUp.Layers.Clear();
            BuildUp.Layers.Add(new BuildUpLayer(Thickness, null));
            return true;
        }
    }
}

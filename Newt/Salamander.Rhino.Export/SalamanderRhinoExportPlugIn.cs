﻿using Salamander.RhinoCommon;
using System;
using R = Rhino;

namespace Salamander.Rhino.Export
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class SalamanderRhinoExportPlugIn : R.PlugIns.FileExportPlugIn

    {
        public SalamanderRhinoExportPlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the SalamanderRhinoExportPlugIn plug-in.</summary>
        public static SalamanderRhinoExportPlugIn Instance
        {
            get; private set;
        }

        /// <summary>Defines file extensions that this export plug-in is designed to write.</summary>
        /// <param name="options">Options that specify how to write files.</param>
        /// <returns>A list of file types that can be exported.</returns>
        protected override R.PlugIns.FileTypeList AddFileTypes(R.FileIO.FileWriteOptions options)
        {
            var result = new R.PlugIns.FileTypeList();
            result.AddFileType("Salamander File (*.s3b)", "s3b");
            result.AddFileType("Salamander: Robot File (*.rtd)", "rtd");
            result.AddFileType("Salamander: GSA Text File (*.gwa)", "gwa");
            return result;
        }

        /// <summary>
        /// Is called when a user requests to export a ."s3b file.
        /// It is actually up to this method to write the file itself.
        /// </summary>
        /// <param name="filename">The complete path to the new file.</param>
        /// <param name="index">The index of the file type as it had been specified by the AddFileTypes method.</param>
        /// <param name="doc">The document to be written.</param>
        /// <param name="options">Options that specify how to write file.</param>
        /// <returns>A value that defines success or a specific failure.</returns>
        protected override R.PlugIns.WriteFileResult WriteFile(string filename, int index, R.RhinoDoc doc, R.FileIO.FileWriteOptions options)
        {
            Host.EnsureInitialisation(true);
            if (Core.Instance.SaveDocument(filename)) return R.PlugIns.WriteFileResult.Success;
            else return R.PlugIns.WriteFileResult.Failure;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and mantain plug-in wide options in a document.
    }
}
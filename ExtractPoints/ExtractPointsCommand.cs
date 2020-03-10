using System;
using Rhino;

namespace ExtractPoints
{
    [System.Runtime.InteropServices.Guid("4ba39b52-6b10-440f-ae2c-9ff7c86bd692")]
    public class ExtractPointsCommand : Rhino.Commands.Command
    {
        static ExtractPointsCommand m_thecommand;
        public ExtractPointsCommand()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static ExtractPointsCommand TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "ExtractPoints"; }
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command is under construction", EnglishName);
            return Rhino.Commands.Result.Success;
        }
    }
}


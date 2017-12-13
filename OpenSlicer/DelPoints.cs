using System;
using Rhino;

namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("fbf7ade6-5fd5-41c6-ac31-bb097e12019e")]
    public class DelPoints : Rhino.Commands.Command
    {
        static DelPoints m_thecommand;

        public DelPoints()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static DelPoints TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "DelPoints"; }
        }

        public Rhino.Commands.Result delete(Rhino.RhinoDoc doc)
        {
            // Input incomplate
            double _incomplate = 0.1;

            var input = new Rhino.Input.Custom.GetNumber();
            input.SetCommandPrompt("Input incomplate(0.0 ~ 1.0)<0.1>");
            Rhino.Input.GetResult res = input.Get();
            if (res == Rhino.Input.GetResult.Number)
                _incomplate = input.Number();
            if (_incomplate == 0.0)
                _incomplate = 0.1;

            // Select surface
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select elements");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Point;
            res = go.GetMultiple(1, 1024 * 10);
            if (res == Rhino.Input.GetResult.Nothing)
                return Rhino.Commands.Result.Failure;

            System.Collections.Generic.List<Guid> list = new System.Collections.Generic.List<Guid>();
            for (int i = 0; i < go.ObjectCount; i++)
            {
                Guid guid = go.Object(i).ObjectId;
                list.Add(guid);                
            }
            
            int deleteCount = (int)((double)(list.Count) * _incomplate);
            Rhino.DocObjects.Tables.ObjectTable ot = Rhino.RhinoDoc.ActiveDoc.Objects;
            for(int i = 0; i < deleteCount; i++)
            {
                Guid guid = list[i];
                ot.Delete(guid, true);
            }

            return Rhino.Commands.Result.Success;
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            // RhinoApp.WriteLine("The {0} command is under construction", EnglishName);
            delete(doc);
            return Rhino.Commands.Result.Success;
        }
    }
}

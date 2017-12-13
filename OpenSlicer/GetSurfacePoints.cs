using System;
using Rhino;

namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("6a9b92b0-300a-42d1-90f8-faaf23aff4e6")]
    public class GetSurfacePoints : Rhino.Commands.Command
    {
        static GetSurfacePoints m_thecommand;
        public GetSurfacePoints()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static GetSurfacePoints TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "GetSurfacePoints"; }
        }

        public Rhino.Commands.Result GetPoints(Rhino.RhinoDoc doc)
        {
            // Input interval
            var input = new Rhino.Input.Custom.GetNumber();
            // Select surface
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select point");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Point;
            go.GetMultiple(1, 1);

            Rhino.Geometry.Point pt = go.Object(0).Point();
            Rhino.Geometry.Point3d pointA = pt.Location;

            go.SetCommandPrompt("Select surface");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Surface;
            go.GetMultiple(1, 1);

            Rhino.Geometry.Surface surfaceB = go.Object(0).Surface();
            double u, v;
            if (surfaceB.ClosestPoint(pointA, out u, out v))
            {
                Rhino.Geometry.Point3d pointC = surfaceB.PointAt(u, v);

                Rhino.Geometry.Vector3d vector = pointA - pointC;
                // write list pointD
            }

            return Rhino.Commands.Result.Success;
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command is under construction", EnglishName);
            GetPoints(doc);
            return Rhino.Commands.Result.Success;
        }
    }
}


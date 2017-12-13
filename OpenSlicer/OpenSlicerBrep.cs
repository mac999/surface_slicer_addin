using System;
using Rhino;
using System.Collections.Generic;

namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("97588db4-f30a-4fc7-9c44-2b5e4fe0f8d5")]
    public class OpenSlicerBrep : Rhino.Commands.Command
    {
        static OpenSlicerBrep m_thecommand;
        public OpenSlicerBrep()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static OpenSlicerBrep TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "OpenSlicerBrep"; }
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            IntersectBrep(doc);
            return Rhino.Commands.Result.Success;
        }

        public Rhino.Commands.Result IntersectBrep(Rhino.RhinoDoc doc)
        {
            // Input interval
            var input = new Rhino.Input.Custom.GetNumber();
            input.SetCommandPrompt("Input slicing interval");
            input.Get();
            if (input.CommandResult() != Rhino.Commands.Result.Success)
            {
                RhinoApp.WriteLine("Can't obtain interval number");
                return input.CommandResult();
            }
            double SlicingInterval = input.Number();

            // Select two curves to intersect
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select Brep");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            go.GetMultiple(1, 1);

            Rhino.Geometry.Brep brepA = null;
            Rhino.Geometry.Surface surfaceB = null;
            if (go.CommandResult() != Rhino.Commands.Result.Success)
            {
                RhinoApp.WriteLine("Can't obtain objects");
                return input.CommandResult();
            }
            brepA = go.Object(0).Brep();

            var go2 = new Rhino.Input.Custom.GetObject();
            go2.SetCommandPrompt("Select surface");
            go2.GeometryFilter = Rhino.DocObjects.ObjectType.Surface;
            go2.GetMultiple(1, 1);
            if (go2.CommandResult() != Rhino.Commands.Result.Success)
            {
                RhinoApp.WriteLine("Can't obtain objects");
                return input.CommandResult();
            }
            surfaceB = go2.Object(0).Surface();

            // Calculate the intersection
            RhinoApp.WriteLine("Executing the intersection between surfaces");
            const double intersection_tolerance = 0.001;
            Rhino.Geometry.Curve[] intersectionCurves = null;
            Rhino.Geometry.Point3d[] intersectionPoints = null;
            bool ret = Rhino.Geometry.Intersect.Intersection.BrepSurface(brepA, surfaceB, intersection_tolerance, out intersectionCurves, out intersectionPoints);
            if (ret)
            {
                List<PlanePoint> PlanePoints = new List<PlanePoint>();
                RhinoApp.WriteLine("Success - {0} curves", intersectionCurves.Length);
                for (int i = 0; i < intersectionCurves.Length; i++)
                {
                    Rhino.Geometry.Curve curve = intersectionCurves[i];
                    doc.Objects.AddCurve(curve);
                    RhinoApp.WriteLine("Curve is added");

                    Utility.CreateSections(doc, brepA, surfaceB, curve, SlicingInterval, ref PlanePoints);
                }

                RhinoApp.WriteLine("Success - {0} points", intersectionPoints.Length);
                for (int i = 0; i < intersectionPoints.Length; i++)
                {
                    Rhino.Geometry.Point3d point = intersectionPoints[i];
                    doc.Objects.AddPoint(point);
                    RhinoApp.WriteLine("Point is added");
                }

                if (Utility.SavePlanePoints(@"c:\SlicerPlanePoints.csv", PlanePoints))
                    RhinoApp.WriteLine("Saved SlicerPlanePoints.csv");
            }

            return Rhino.Commands.Result.Success;
        }
    }
}


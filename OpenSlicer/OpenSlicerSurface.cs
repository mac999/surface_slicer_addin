// Maker: Tae Wook, Kang
// Email: laputa99999@gmail.com
// Date: 2015.4.23
using System;
using Rhino;
using System.Collections.Generic;

namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("19e1b777-d5d0-4d00-b16c-29a896988157")]
    public class OpenSlicerSurface : Rhino.Commands.Command
    {
        static OpenSlicerSurface m_thecommand;
        public OpenSlicerSurface()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static OpenSlicerSurface TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "OpenSlicer"; }
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command is under construction", EnglishName);
            IntersectSurfaces(doc);
            return Rhino.Commands.Result.Success;
        }

        public Rhino.Commands.Result IntersectSurfaces(Rhino.RhinoDoc doc)
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
            go.SetCommandPrompt("Select surface");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Surface;
            go.GetMultiple(2, 2);

            Rhino.Geometry.Surface surfaceA = go.Object(0).Surface();
            Rhino.Geometry.Surface surfaceB = go.Object(1).Surface();

            // Calculate the intersection
            RhinoApp.WriteLine("Executing the intersection between surfaces");
            const double intersection_tolerance = 0.001;
            Rhino.Geometry.Curve[] intersectionCurves = null;
            Rhino.Geometry.Point3d[] intersectionPoints = null;
            bool ret = Rhino.Geometry.Intersect.Intersection.SurfaceSurface(surfaceA, surfaceB, intersection_tolerance, out intersectionCurves, out intersectionPoints);
            if (ret)
            {
                List<PlanePoint> PlanePoints = new List<PlanePoint>();
                RhinoApp.WriteLine("Success - {0} curves", intersectionCurves.Length);
                for (int i = 0; i < intersectionCurves.Length; i++)
                {
                    Rhino.Geometry.Curve curve = intersectionCurves[i];
                    doc.Objects.AddCurve(curve);
                    RhinoApp.WriteLine("Curve is added");

                    Utility.CreateSections(doc, surfaceA, surfaceB, curve, SlicingInterval, ref PlanePoints);
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

        public static Rhino.Commands.Result IntersectCurves(Rhino.RhinoDoc doc)
        {
            // Select two curves to intersect
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select two curves");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            go.GetMultiple(2, 2);
            if (go.CommandResult() != Rhino.Commands.Result.Success)
                return go.CommandResult();

            // Validate input
            var curveA = go.Object(0).Curve();
            var curveB = go.Object(1).Curve();
            if (curveA == null || curveB == null)
                return Rhino.Commands.Result.Failure;

            // Calculate the intersection
            const double intersection_tolerance = 0.001;
            const double overlap_tolerance = 0.0;
            var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(curveA, curveB, intersection_tolerance, overlap_tolerance);

            // Process the results
            if (events != null)
            {
                for (int i = 0; i < events.Count; i++)
                {
                    var ccx_event = events[i];
                    doc.Objects.AddPoint(ccx_event.PointA);
                    if (ccx_event.PointA.DistanceTo(ccx_event.PointB) > double.Epsilon)
                    {
                        doc.Objects.AddPoint(ccx_event.PointB);
                        doc.Objects.AddLine(ccx_event.PointA, ccx_event.PointB);
                    }
                }
                doc.Views.Redraw();
            }
            return Rhino.Commands.Result.Success;
        }
    }
}


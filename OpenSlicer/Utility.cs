using System;
using System.Collections.Generic;
using System.Text;
using Rhino;
using System.IO;

namespace OpenSlicer
{
    class PlanePoint
    {
        public Rhino.Geometry.Point3d pt;
        public double A, B, C, D;
        public Rhino.Geometry.Vector3d curvature;
    };

    class Utility
    {
        static public int _OutputCount;
        static public int _CurrentOutput = 0;
        static public bool GetNormalVector(Rhino.Geometry.Surface surface, Rhino.Geometry.Point3d pt, ref Rhino.Geometry.Vector3d normal, double tolerance = 0.001)
        {
            double u, v;
            if (surface.ClosestPoint(pt, out u, out v) == false)
                return false;
            Rhino.Geometry.Point3d pt2 = surface.PointAt(u, v);
            double distance = pt.DistanceTo(pt2);
            if (distance > tolerance)
                return false;
            RhinoApp.WriteLine("Found the closest point on surface within {0}", distance);
            Rhino.Geometry.Vector3d n = surface.NormalAt(u, v);
            normal = n;
            return true;
        }

        static public bool GetNormalVector(Rhino.Geometry.Brep brepA, Rhino.Geometry.Point3d pt, ref Rhino.Geometry.Vector3d normal, double tolerance = 0.001)
        {
            bool ret = false;
            foreach (Rhino.Geometry.Surface surface in brepA.Surfaces)
            {
                if (Utility.GetNormalVector(surface, pt, ref normal, tolerance) == false)
                    continue;
                ret = true;
                break;
            }
            return ret;
        }

        static public Rhino.Geometry.Vector3d CrossProduct(Rhino.Geometry.Vector3d u, Rhino.Geometry.Vector3d v)
        {
            Rhino.Geometry.Vector3d z = new Rhino.Geometry.Vector3d();
            z.X = u.Y * v.Z - u.Z * v.Y;
            z.Y = -u.X * v.Z + u.Z * v.X;
            z.Z = u.X * v.Y - u.Y * v.X;
            return z;
        }

        static public bool CreateSections(Rhino.RhinoDoc doc, Rhino.Geometry.GeometryBase geo, Rhino.Geometry.Surface surfaceB, Rhino.Geometry.Curve curve, double interval, ref List<PlanePoint> points)
        {
            Rhino.Geometry.Interval domain = curve.Domain;  // fixed issue
            for (double t = domain.T0; t < domain.T1; t += interval)
            {
                Rhino.Geometry.Point3d pt = curve.PointAt(t);
                Rhino.Geometry.Vector3d tangent = curve.TangentAt(t);
                Rhino.Geometry.Vector3d curvature = curve.CurvatureAt(t);
                Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane();
                curve.FrameAt(t, out plane);

                doc.Objects.AddPoint(pt);
                curvature = curvature * 10.0;
                Rhino.Geometry.Line line = new Rhino.Geometry.Line(pt, curvature);
                doc.Objects.AddLine(line);
                RhinoApp.WriteLine("Curve at {0}", t);

                Rhino.Geometry.Vector3d normal = new Rhino.Geometry.Vector3d();

                bool ret = false;
                if(geo is Rhino.Geometry.Brep)
                {
                    Rhino.Geometry.Brep brepA = (Rhino.Geometry.Brep)geo;
                    ret = GetNormalVector(brepA, pt, ref normal);
                    RhinoApp.WriteLine("   Added Brep point at ({0}, {0}, {0})", pt.X, pt.Y, pt.Z);
                }
                else if (geo is Rhino.Geometry.Surface)
                {
                    Rhino.Geometry.Surface surfaceA = (Rhino.Geometry.Surface)geo;
                    ret = GetNormalVector(surfaceA, pt, ref normal);
                    RhinoApp.WriteLine("   Added surface point at ({0}, {0}, {0})", pt.X, pt.Y, pt.Z);
                }

                if (ret)
                {
                    Rhino.Geometry.Vector3d ucoord = CrossProduct(tangent, normal);
                    Rhino.Geometry.Plane plane2 = new Rhino.Geometry.Plane(pt, ucoord, tangent); // normal);
                    double[] parameters = plane2.GetPlaneEquation();

                    PlanePoint PlanePoint = new PlanePoint();
                    PlanePoint.pt = pt;
                    PlanePoint.A = parameters[0];
                    PlanePoint.B = parameters[1];
                    PlanePoint.C = parameters[2];
                    PlanePoint.D = parameters[3];
                    PlanePoint.curvature = curvature;
                    points.Add(PlanePoint);

                    Rhino.Geometry.Interval Interval1 = new Rhino.Geometry.Interval(-0.1, -0.1);
                    Rhino.Geometry.Interval Interval2 = new Rhino.Geometry.Interval(0.1, 0.1);
                    Rhino.Geometry.PlaneSurface PlaneSurface = new Rhino.Geometry.PlaneSurface(plane2, Interval1, Interval2);
                    doc.Objects.AddSurface(PlaneSurface);
                }
            }
            return true;
        }

        static public void SetOutputCount(int count)
        {
            _OutputCount = count;
            _CurrentOutput = 0;
        }

        static public bool OutputData(string tag, Rhino.Geometry.Point3d v)
        {
            if (_CurrentOutput >= _OutputCount)
                return false;
            _CurrentOutput++;
            try
            {
                string text = string.Format("{0:0.00}, {1:0.00}, {2:0.00}", v.X, v.Y, v.Z);
                RhinoApp.WriteLine(tag + text);
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        static public bool OutputData(string tag, Rhino.Geometry.Vector3d v)
        {
            if (_CurrentOutput >= _OutputCount)
                return false;
            _CurrentOutput++;
            try
            {
                string text = string.Format("{0:0.00}, {1:0.00}, {2:0.00}", v.X, v.Y, v.Z);
                RhinoApp.WriteLine(tag + text);
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        static public bool SaveVector(string name, string tag, Rhino.Geometry.Point3d v)
        {
            try
            {
                StreamWriter file = new StreamWriter(name, true);

                string text = string.Format("{0:0.00}, {1:0.00}, {2:0.00}", v.X, v.Y, v.Z);
                file.WriteLine(tag + text);

                file.Close();
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        static public bool SaveVector(string name, string tag, Rhino.Geometry.Vector3d v)
        {
            try
            {
                StreamWriter file = new StreamWriter(name, true);
                
                string text = string.Format("{0:0.00}, {1:0.00}, {2:0.00}", v.X, v.Y, v.Z);
                file.WriteLine(tag + text);

                file.Close();
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        static public bool SavePlanePoints(string name, List<PlanePoint> points)
        {
            try
            {
                StreamWriter file = new StreamWriter(name, true);

                foreach (PlanePoint pt in points)
                {
                    string text = string.Format("{0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}, {4:0.00}, {5:0.00}, {6:0.00}, {7:0.00}, {8:0.00}, {9:0.00}", pt.pt.X, pt.pt.Y, pt.pt.Z, pt.A, pt.B, pt.C, pt.D, pt.curvature.X, pt.curvature.Y, pt.curvature.Z);
                    file.WriteLine(text);
                }
                file.Close();
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}

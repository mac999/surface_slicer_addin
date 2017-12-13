using System;
using Rhino;

 
namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("fbf7ade6-5fd5-41c6-ac31-bb097e11017f")]
    public class AddSurfacePoints : Rhino.Commands.Command
    {
        static AddSurfacePoints m_thecommand;
        Random _random = new Random();

        public AddSurfacePoints()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static AddSurfacePoints TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "AddSurfPoint"; }
        }

        /*
        public Rhino.Geometry.Point3d randomPoint(Rhino.Geometry.Point3d pt, Rhino.Geometry.Vector3d n, double interval, double randomValue)
        {
            const int MAX_RANDOM_VALUE = 1000;
            int maxValue = (int)(MAX_RANDOM_VALUE * interval);
            int maxRandom = (int)((double)maxValue * (double)randomValue / 100.0);

            int n1 = _random.Next(0, maxRandom);
            int n2 = _random.Next(0, maxRandom);
            int n3 = _random.Next(0, maxRandom);

            // uv 간격에 따른 랜덤값 획득
            double v1 = (double)n1 / maxValue;
            double v2 = (double)n2 / maxValue;
            double v3 = (double)n3 / maxValue;

            Rhino.Geometry.Point3d pt2 = new Rhino.Geometry.Point3d();
            pt2.X = pt.X * (1.0 + v1);   // 2.0을 나누어 uv 지점에서 노이즈 편차가 되도록 함.
            pt2.Y = pt.Y * (1.0 + v2);
            pt2.Z = pt.Z * (1.0 + v3); 
            return pt2;
        }
        function randomSpherePoint(x0,y0,z0,radius){
           var u = Math.random();
           var v = Math.random();
           var theta = 2 * Math.PI * u;
           var phi = Math.acos(2 * v - 1);
           var x = x0 + (radius * Math.sin(phi) * Math.cos(theta));
           var y = y0 + (radius * Math.sin(phi) * Math.sin(theta));
           var z = z0 + (radius * Math.cos(phi));
           return [x,y,z];
        }   
        */
        /*
        public Rhino.Geometry.Point3d randomPoint(Rhino.Geometry.Surface surface, Rhino.Geometry.Point3d pt, Rhino.Geometry.Vector3d n, double interval, double randomValue)
        {
            double radius = interval;
            double u = _random.NextDouble();
            double v = _random.NextDouble();
            double theta = 2.0 * Math.PI * u;
            double phi = Math.Acos(2.0 * v - 1.0);
            double x = pt.X + (radius * Math.Sin(phi) * Math.Cos(theta));
            double y = pt.Y + (radius * Math.Sin(phi) * Math.Sin(theta));
            double z = pt.Z + (radius * Math.Cos(phi));

            Rhino.Geometry.Point3d pt2 = new Rhino.Geometry.Point3d(x, y, z);
            return pt2;
        }
        */
        public Rhino.Geometry.Point3d randomPoint(Rhino.Geometry.Surface surface, double u, double v, double interval, double noise)
        {
            noise = noise / 2.0 / 100;
            double randomU = _random.NextDouble();  // random number from 0 to 1.0
            double randomV = _random.NextDouble();
            randomU = randomU * interval * noise;
            randomV = randomV * interval * noise;
            Rhino.Geometry.Point3d pt = surface.PointAt(u + randomU, v + randomV);
            Rhino.Geometry.Vector3d n = surface.NormalAt(u + randomU, v + randomV);

            double randomZ = _random.NextDouble();
            // randomZ = (randomZ - 0.5) * interval * noise;
            randomZ = randomZ * interval * noise;
            n = n * randomZ;

            Rhino.Geometry.Point3d pt2 = pt + n;
            return pt2;
        }

        public Rhino.Commands.Result AddPoints(Rhino.RhinoDoc doc)
        {
            // Input interval
            double _interval = 0.1;
            double _noise = 1;    // 1% noise

            var input = new Rhino.Input.Custom.GetNumber();
            input.SetCommandPrompt("Input interval(0.0 ~ 1.0)<0.1>");
            Rhino.Input.GetResult res = input.Get();
            if (res == Rhino.Input.GetResult.Number)
                _interval = input.Number();
            if (_interval == 0.0)
                _interval = 0.1;

            input.SetCommandPrompt("Noise factor(0.0 ~ 100.0)");
            res = input.Get();
            if (res == Rhino.Input.GetResult.Number)
                _noise = input.Number();

            // Select surface
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select surface");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Surface;
            res = go.GetMultiple(1, 1024);
            if (res == Rhino.Input.GetResult.Nothing)
                return Rhino.Commands.Result.Failure;

            Utility.SetOutputCount(10);
            for (int i = 0; i < go.ObjectCount; i++)
            {
                Rhino.Geometry.Surface surfaceA = go.Object(i).Surface();
                if (surfaceA == null)
                    return Rhino.Commands.Result.Failure;

                Rhino.Geometry.Interval domU = surfaceA.Domain(0);
                Rhino.Geometry.Interval domV = surfaceA.Domain(1);

                double u, v;
                u = v = 0.0;
                for (u = domU.Min; u <= domU.Max; u += _interval)
                {
                    for (v = domV.Min; v <= domV.Max; v += _interval)
                    {
                        Rhino.Geometry.Point3d pt = surfaceA.PointAt(u, v);
                        Rhino.Geometry.Vector3d n = surfaceA.NormalAt(u, v);
                        Rhino.Geometry.Point3d pt2 = randomPoint(surfaceA, u, v, _interval, _noise);
                        doc.Objects.AddPoint(pt2);
                    }
                }
            }
            return Rhino.Commands.Result.Success;
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            // RhinoApp.WriteLine("The {0} command is under construction", EnglishName);
            AddPoints(doc);
            return Rhino.Commands.Result.Success;
        }
    }
}


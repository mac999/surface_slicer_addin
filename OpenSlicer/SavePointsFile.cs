using System;
using Rhino;
using System.IO;
using System.Windows.Forms;

namespace OpenSlicer
{
    [System.Runtime.InteropServices.Guid("507ee43b-1577-4011-a4e6-042397293866")]
    public class SavePointsFile : Rhino.Commands.Command
    {
        static SavePointsFile m_thecommand;
        public SavePointsFile()
        {
            // Rhino only creates one instance of each command class defined in a plug-in, so it is
            // safe to hold on to a static reference.
            m_thecommand = this;
        }

        ///<summary>The one and only instance of this command</summary>
        public static SavePointsFile TheCommand
        {
            get { return m_thecommand; }
        }

        ///<returns>The command name as it appears on the Rhino command line</returns>
        public override string EnglishName
        {
            get { return "SavePointsFile"; }
        }

        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
        {
            // Input interval
            var input = new Rhino.Input.Custom.GetNumber();
            // Select surface
            var go = new Rhino.Input.Custom.GetObject();
            go.SetCommandPrompt("Select points");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Point;
            Rhino.Input.GetResult res = go.GetMultiple(1, 0);
            if (res == Rhino.Input.GetResult.Nothing)
                return Rhino.Commands.Result.Failure;

            RhinoApp.WriteLine("Points {0} were selected", go.ObjectCount);

            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "PointFile.xyz";
                dialog.Filter = "XYZ file|*.xyz|text file|*.txt";
                dialog.Title = "Save point cloud file";
                dialog.ShowDialog();

                if (dialog.FileName == "")
                    return Rhino.Commands.Result.Failure;

                FileStream fileStream = (System.IO.FileStream)dialog.OpenFile(); //  new FileStream(@"C:\Users\KTW\Documents\PointFile.xyz", FileMode.Create, FileAccess.Write);            
                StreamWriter writer = new StreamWriter(fileStream);

                for (int i = 0; i < go.ObjectCount; i++)
                {
                    Rhino.Geometry.Point pt = go.Object(i).Point();
                    Rhino.Geometry.Point3d pt3d = pt.Location;

                    string text = pt3d.X + " " + pt3d.Y + " " + pt3d.Z;
                    writer.WriteLine(text);
                }

                RhinoApp.WriteLine("File was saved");
                fileStream.Close();
            }
            catch(Exception e)
            {
                RhinoApp.WriteLine("{0}", e.Message);
            }

            return Rhino.Commands.Result.Success;
        }
    }
}


using System;
using Rhino;

namespace OpenSlicer
{
    ///<summary>
    /// Every RhinoCommon Plug-In must have one and only one PlugIn derived
    /// class. DO NOT create an instance of this class. It is the responsibility
    /// of Rhino to create an instance of this class.
    ///</summary>
    public class OpenSlicerPlugIn : Rhino.PlugIns.PlugIn
    {
        static OpenSlicerPlugIn m_theplugin;

        public OpenSlicerPlugIn()
        {
            m_theplugin = this;
        }

        public static OpenSlicerPlugIn ThePlugIn
        {
            get { return m_theplugin; }
        }

    }
}

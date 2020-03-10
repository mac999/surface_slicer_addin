// 2016.4
using System;
using Rhino;

namespace ExtractPoints
{
    ///<summary>
    /// Every RhinoCommon Plug-In must have one and only one PlugIn derived
    /// class. DO NOT create an instance of this class. It is the responsibility
    /// of Rhino to create an instance of this class.
    ///</summary>
    public class ExtractPointsPlugIn : Rhino.PlugIns.PlugIn
    {
        static ExtractPointsPlugIn m_theplugin;

        public ExtractPointsPlugIn()
        {
            m_theplugin = this;
        }

        public static ExtractPointsPlugIn ThePlugIn
        {
            get { return m_theplugin; }
        }

    }
}

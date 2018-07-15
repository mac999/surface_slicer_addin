''
'' (C) Copyright 2010 by Autodesk, Inc.
''
'' Permission to use, copy, modify, and distribute this software in
'' object code form for any purpose and without fee is hereby granted,
'' provided that the above copyright notice appears in all copies and
'' that both that copyright notice and the limited warranty and
'' restricted rights notice below appear in all supporting
'' documentation.
''
'' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'' UNINTERRUPTED OR ERROR FREE.
''
'' Use, duplication, or disclosure by the U.S. Government is subject to
'' restrictions set forth in FAR 52.227-19 (Commercial Computer
'' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'' (Rights in Technical Data and Computer Software), as applicable.
''


This sample demonstrates the creation of alignments and the setting
of alignment features such as station equations, design speeds,
styles, label styles, superelevations, and entities.


There are two subroutines:

AlignmentFromPolyline - draws an AutoCAD polyline and creates an
alignment based on it.  It demonstrates the creation of a style
and a label style, determining what entities make up the alignment,
and a simple station computation.

AlignmentFromLayout - Uses layout function to create an alignment.
This also demonstrates the creation of a style and a label style,
setting design speeds, station equations, station computations, and
creation of offset alignments. Note superelevation is deprecated.



----------------------------------------------------------------------
NOTE: These samples are designed to be run using the the "_Autodesk
 Civil 3D (Imperial) NCS Base.dwt" template.  The sample will work in
 other templates, but certain style elements (such as shading) may not
 be visible and label style elements (text, tick marks, data bands)
 may have an incorrect size.
----------------------------------------------------------------------
----------------------------------------------------------------------
NOTE: You are now required to specify the version number when you
 create COM objects using the GetInterfaceObject method.  For example,
 to create a new AeccApplication object, you would use the following
 code:

 Dim g_oCivilApp As AeccApplication
 Set oApp = ThisDrawing.Application
 Const sAppName = "AeccXUiLand.AeccApplication.8.0"
 Set g_oCivilApp = oApp.GetInterfaceObject(sAppName)
----------------------------------------------------------------------
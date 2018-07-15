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


This sample demonstrates the corridor API functions, including
using corridor base objects, baselines, and assemblies.


There are two subroutines:

CreateCorridorExample - Demonstrates the new corridor creation
method using a simple alignment and profile.  This sample requires
a drawing with at least one assembly in it.

GetCorridorInformationExample - Gets information about existing
corridors in the current document.  Before running this sample be
sure that the current document has a corridor, and surfaces named 
"Corridor - (1) Datum"  and "Corridor - (1) Top".
(The sample document "..\Help\Civil 3D Tutorials\Drawings\Corridor-5b.dwg"
is recommended.)  This sample will extract all information about
the corridor (including information about the baselines, assemblies,
and subassemblies) and display it in a report sent to an instance
of Microsoft Word.  Be sure that Microsoft Word is running before
calling this subroutine.  Finally, the cut and fill required for this
corridor is computed using volume surfaces.



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
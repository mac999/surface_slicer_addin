using System;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;

namespace Parameters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    public class Parameters : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
        ref string message, ElementSet elements)
        {
            Document activeDoc = revit.Application.ActiveUIDocument.Document;
            Selection selection = revit.Application.ActiveUIDocument.Selection;  // 활성화된 창 문서객체의 선택 객체 획득
            ElementSet collection = selection.Elements;	// 선택객체의 선택 요소들 획득

            foreach (Element element in collection)
            {
                GetElementParameterInformation(activeDoc, element);
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        void GetElementParameterInformation(Document document, Element element)
        {
            String prompt = "Show parameters in selected Element:";
            StringBuilder st = new StringBuilder();
            foreach (Parameter para in element.Parameters)
            {
                st.AppendLine(GetParameterInformation(para, document));
            }
            MessageBox.Show(st.ToString(), prompt, MessageBoxButtons.OK);
        }

        String GetParameterInformation(Parameter para, Document document)
        {
            string defName = para.Definition.Name + @"\t";
            switch (para.StorageType)
            {
                case StorageType.Double:
                    defName += " : " + para.AsValueString();
                    break;
                case StorageType.String:
                    defName += " : " + para.AsString();
                    break;
                default:
                    defName = "Unexposed parameter.";
                    break;
            }
            return defName;
        }
    }
}

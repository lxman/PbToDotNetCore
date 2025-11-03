namespace PbToDotNetCore.Gui.WinForms;

/// <summary>
/// Converts PowerBASIC GUI code to Windows Forms
/// </summary>
public class WinFormsGuiConverter : IGuiConverter
{
    public string ConvertDialogNew(string parent, string title, int x, int y, int width, int height, string style, string exStyle, string handleVar)
    {
        throw new NotImplementedException("WinForms DIALOG NEW conversion not yet implemented");
    }

    public string ConvertDialogShowModal(string dialogHandle, string callbackFunctionName)
    {
        throw new NotImplementedException("WinForms DIALOG SHOW MODAL conversion not yet implemented");
    }

    public string ConvertDialogEnd(string dialogHandle, string returnCode)
    {
        throw new NotImplementedException("WinForms DIALOG END conversion not yet implemented");
    }

    public string ConvertControlAdd(string controlType, string dialogHandle, string controlId, string text, int x, int y, int width, int height)
    {
        throw new NotImplementedException("WinForms CONTROL ADD conversion not yet implemented");
    }

    public string ConvertControlGetText(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WinForms CONTROL GET TEXT conversion not yet implemented");
    }

    public string ConvertControlSetText(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WinForms CONTROL SET TEXT conversion not yet implemented");
    }

    public string ConvertCallbackFunction(string functionName, string body)
    {
        throw new NotImplementedException("WinForms CALLBACK FUNCTION conversion not yet implemented");
    }

    public string ConvertListBoxAdd(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WinForms LISTBOX ADD conversion not yet implemented");
    }

    public string ConvertListBoxDelete(string dialogHandle, string controlId, string index)
    {
        throw new NotImplementedException("WinForms LISTBOX DELETE conversion not yet implemented");
    }

    public string ConvertListBoxGetCount(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WinForms LISTBOX GET COUNT conversion not yet implemented");
    }

    public string ConvertListBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WinForms LISTBOX GET SELECT conversion not yet implemented");
    }

    public string ConvertComboBoxAdd(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WinForms COMBOBOX ADD conversion not yet implemented");
    }

    public string ConvertComboBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WinForms COMBOBOX GET SELECT conversion not yet implemented");
    }

    public string ConvertComboBoxGetText(string dialogHandle, string controlId, string index, string targetVariable)
    {
        throw new NotImplementedException("WinForms COMBOBOX GET TEXT conversion not yet implemented");
    }

    public string ConvertMenuNewBar(string dialogHandle)
    {
        throw new NotImplementedException("WinForms MENU NEW BAR conversion not yet implemented");
    }

    public string ConvertMenuNewPopup(string dialogHandle)
    {
        throw new NotImplementedException("WinForms MENU NEW POPUP conversion not yet implemented");
    }

    public string ConvertMenuAddString(string text, string id, string flags)
    {
        throw new NotImplementedException("WinForms MENU ADD STRING conversion not yet implemented");
    }

    public string ConvertMenuEndPopup()
    {
        throw new NotImplementedException("WinForms MENU END POPUP conversion not yet implemented");
    }

    public string ConvertMenuAttachPopup(string dialogHandle, string popupText)
    {
        throw new NotImplementedException("WinForms MENU ATTACH POPUP conversion not yet implemented");
    }

    public string ConvertGraphicWindow(string title, int x, int y, int width, int height, string handleVar)
    {
        throw new NotImplementedException("WinForms GRAPHIC WINDOW conversion not yet implemented");
    }

    public string ConvertGraphicWindowEnd(string windowHandle)
    {
        throw new NotImplementedException("WinForms GRAPHIC WINDOW END conversion not yet implemented");
    }

    public string ConvertGraphicBitmapNew(int width, int height, string handleVar)
    {
        throw new NotImplementedException("WinForms GRAPHIC BITMAP NEW conversion not yet implemented");
    }

    public string ConvertGraphicBitmapEnd(string bitmapHandle)
    {
        throw new NotImplementedException("WinForms GRAPHIC BITMAP END conversion not yet implemented");
    }

    public string ConvertGraphicAttach(string dialogHandle, string controlId)
    {
        throw new NotImplementedException("WinForms GRAPHIC ATTACH conversion not yet implemented");
    }

    public string ConvertGraphicOperation(string operation, params string[] parameters)
    {
        throw new NotImplementedException("WinForms GRAPHIC operations conversion not yet implemented");
    }

    public List<string> GetRequiredUsings()
    {
        return new List<string>
        {
            "System.Windows.Forms",
            "System.Drawing"
        };
    }

    public string GetFrameworkName()
    {
        return "Windows Forms";
    }
}

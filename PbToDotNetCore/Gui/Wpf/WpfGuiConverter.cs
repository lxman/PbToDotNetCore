namespace PbToDotNetCore.Gui.Wpf;

/// <summary>
/// Converts PowerBASIC GUI code to Windows Presentation Foundation (WPF)
/// </summary>
public class WpfGuiConverter : IGuiConverter
{
    public string ConvertDialogNew(string parent, string title, int x, int y, int width, int height, string style, string exStyle, string handleVar)
    {
        throw new NotImplementedException("WPF DIALOG NEW conversion not yet implemented");
    }

    public string ConvertDialogShowModal(string dialogHandle, string callbackFunctionName)
    {
        throw new NotImplementedException("WPF DIALOG SHOW MODAL conversion not yet implemented");
    }

    public string ConvertDialogEnd(string dialogHandle, string returnCode)
    {
        throw new NotImplementedException("WPF DIALOG END conversion not yet implemented");
    }

    public string ConvertControlAdd(string controlType, string dialogHandle, string controlId, string text, int x, int y, int width, int height)
    {
        throw new NotImplementedException("WPF CONTROL ADD conversion not yet implemented");
    }

    public string ConvertControlGetText(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WPF CONTROL GET TEXT conversion not yet implemented");
    }

    public string ConvertControlSetText(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WPF CONTROL SET TEXT conversion not yet implemented");
    }

    public string ConvertCallbackFunction(string functionName, string body)
    {
        throw new NotImplementedException("WPF CALLBACK FUNCTION conversion not yet implemented");
    }

    public string ConvertListBoxAdd(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WPF LISTBOX ADD conversion not yet implemented");
    }

    public string ConvertListBoxDelete(string dialogHandle, string controlId, string index)
    {
        throw new NotImplementedException("WPF LISTBOX DELETE conversion not yet implemented");
    }

    public string ConvertListBoxGetCount(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WPF LISTBOX GET COUNT conversion not yet implemented");
    }

    public string ConvertListBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WPF LISTBOX GET SELECT conversion not yet implemented");
    }

    public string ConvertComboBoxAdd(string dialogHandle, string controlId, string text)
    {
        throw new NotImplementedException("WPF COMBOBOX ADD conversion not yet implemented");
    }

    public string ConvertComboBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        throw new NotImplementedException("WPF COMBOBOX GET SELECT conversion not yet implemented");
    }

    public string ConvertComboBoxGetText(string dialogHandle, string controlId, string index, string targetVariable)
    {
        throw new NotImplementedException("WPF COMBOBOX GET TEXT conversion not yet implemented");
    }

    public string ConvertMenuNewBar(string dialogHandle)
    {
        throw new NotImplementedException("WPF MENU NEW BAR conversion not yet implemented");
    }

    public string ConvertMenuNewPopup(string dialogHandle)
    {
        throw new NotImplementedException("WPF MENU NEW POPUP conversion not yet implemented");
    }

    public string ConvertMenuAddString(string text, string id, string flags)
    {
        throw new NotImplementedException("WPF MENU ADD STRING conversion not yet implemented");
    }

    public string ConvertMenuEndPopup()
    {
        throw new NotImplementedException("WPF MENU END POPUP conversion not yet implemented");
    }

    public string ConvertMenuAttachPopup(string dialogHandle, string popupText)
    {
        throw new NotImplementedException("WPF MENU ATTACH POPUP conversion not yet implemented");
    }

    public string ConvertGraphicWindow(string title, int x, int y, int width, int height, string handleVar)
    {
        throw new NotImplementedException("WPF GRAPHIC WINDOW conversion not yet implemented");
    }

    public string ConvertGraphicWindowEnd(string windowHandle)
    {
        throw new NotImplementedException("WPF GRAPHIC WINDOW END conversion not yet implemented");
    }

    public string ConvertGraphicBitmapNew(int width, int height, string handleVar)
    {
        throw new NotImplementedException("WPF GRAPHIC BITMAP NEW conversion not yet implemented");
    }

    public string ConvertGraphicBitmapEnd(string bitmapHandle)
    {
        throw new NotImplementedException("WPF GRAPHIC BITMAP END conversion not yet implemented");
    }

    public string ConvertGraphicAttach(string dialogHandle, string controlId)
    {
        throw new NotImplementedException("WPF GRAPHIC ATTACH conversion not yet implemented");
    }

    public string ConvertGraphicOperation(string operation, params string[] parameters)
    {
        throw new NotImplementedException("WPF GRAPHIC operations conversion not yet implemented");
    }

    public List<string> GetRequiredUsings()
    {
        return new List<string>
        {
            "System.Windows",
            "System.Windows.Controls",
            "System.Windows.Media"
        };
    }

    public string GetFrameworkName()
    {
        return "Windows Presentation Foundation (WPF)";
    }
}

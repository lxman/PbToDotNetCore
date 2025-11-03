namespace PbToDotNetCore.Gui;

/// <summary>
/// Interface for converting PowerBASIC GUI code to different C# GUI frameworks
/// </summary>
public interface IGuiConverter
{
    /// <summary>
    /// Converts a DIALOG NEW statement
    /// </summary>
    string ConvertDialogNew(string parent, string title, int x, int y, int width, int height, string style, string exStyle, string handleVar);

    /// <summary>
    /// Converts a DIALOG SHOW MODAL statement
    /// </summary>
    string ConvertDialogShowModal(string dialogHandle, string callbackFunctionName);

    /// <summary>
    /// Converts a DIALOG END statement
    /// </summary>
    string ConvertDialogEnd(string dialogHandle, string returnCode);

    /// <summary>
    /// Converts a CONTROL ADD statement
    /// </summary>
    string ConvertControlAdd(string controlType, string dialogHandle, string controlId, string text, int x, int y, int width, int height);

    /// <summary>
    /// Converts a CONTROL GET TEXT statement
    /// </summary>
    string ConvertControlGetText(string dialogHandle, string controlId, string targetVariable);

    /// <summary>
    /// Converts a CONTROL SET TEXT statement
    /// </summary>
    string ConvertControlSetText(string dialogHandle, string controlId, string text);

    /// <summary>
    /// Converts a CALLBACK FUNCTION declaration
    /// </summary>
    string ConvertCallbackFunction(string functionName, string body);

    /// <summary>
    /// Converts a LISTBOX ADD statement
    /// </summary>
    string ConvertListBoxAdd(string dialogHandle, string controlId, string text);

    /// <summary>
    /// Converts a LISTBOX DELETE statement
    /// </summary>
    string ConvertListBoxDelete(string dialogHandle, string controlId, string index);

    /// <summary>
    /// Converts a LISTBOX GET COUNT statement
    /// </summary>
    string ConvertListBoxGetCount(string dialogHandle, string controlId, string targetVariable);

    /// <summary>
    /// Converts a LISTBOX GET SELECT statement
    /// </summary>
    string ConvertListBoxGetSelect(string dialogHandle, string controlId, string targetVariable);

    /// <summary>
    /// Converts a COMBOBOX ADD statement
    /// </summary>
    string ConvertComboBoxAdd(string dialogHandle, string controlId, string text);

    /// <summary>
    /// Converts a COMBOBOX GET SELECT statement
    /// </summary>
    string ConvertComboBoxGetSelect(string dialogHandle, string controlId, string targetVariable);

    /// <summary>
    /// Converts a COMBOBOX GET TEXT statement
    /// </summary>
    string ConvertComboBoxGetText(string dialogHandle, string controlId, string index, string targetVariable);

    /// <summary>
    /// Converts a MENU NEW BAR statement
    /// </summary>
    string ConvertMenuNewBar(string dialogHandle);

    /// <summary>
    /// Converts a MENU NEW POPUP statement
    /// </summary>
    string ConvertMenuNewPopup(string dialogHandle);

    /// <summary>
    /// Converts a MENU ADD STRING statement
    /// </summary>
    string ConvertMenuAddString(string text, string id, string flags);

    /// <summary>
    /// Converts a MENU END POPUP statement
    /// </summary>
    string ConvertMenuEndPopup();

    /// <summary>
    /// Converts a MENU ATTACH POPUP statement
    /// </summary>
    string ConvertMenuAttachPopup(string dialogHandle, string popupText);

    /// <summary>
    /// Converts a GRAPHIC WINDOW statement
    /// </summary>
    string ConvertGraphicWindow(string title, int x, int y, int width, int height, string handleVar);

    /// <summary>
    /// Converts a GRAPHIC WINDOW END statement
    /// </summary>
    string ConvertGraphicWindowEnd(string windowHandle);

    /// <summary>
    /// Converts a GRAPHIC BITMAP NEW statement (memory bitmap)
    /// </summary>
    string ConvertGraphicBitmapNew(int width, int height, string handleVar);

    /// <summary>
    /// Converts a GRAPHIC BITMAP END statement
    /// </summary>
    string ConvertGraphicBitmapEnd(string bitmapHandle);

    /// <summary>
    /// Converts a GRAPHIC ATTACH statement (attaches to a graphic control)
    /// </summary>
    string ConvertGraphicAttach(string dialogHandle, string controlId);

    /// <summary>
    /// Converts GRAPHIC drawing operations (COLOR, CLEAR, ELLIPSE, LINE, etc.)
    /// </summary>
    string ConvertGraphicOperation(string operation, params string[] parameters);

    /// <summary>
    /// Gets additional using directives required for this GUI framework
    /// </summary>
    List<string> GetRequiredUsings();

    /// <summary>
    /// Gets the framework name for display in comments
    /// </summary>
    string GetFrameworkName();
}

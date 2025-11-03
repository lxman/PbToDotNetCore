namespace PbToDotNetCore.Gui;

/// <summary>
/// GUI converter that generates helpful comments instead of actual GUI code.
/// Used when GuiFramework.None is selected.
/// </summary>
public class NoGuiConverter : IGuiConverter
{
    public string ConvertDialogNew(string parent, string title, int x, int y, int width, int height, string style, string exStyle, string handleVar)
    {
        return $"/* TODO: DIALOG NEW - Create dialog '{title}' at ({x}, {y}) size {width}x{height} - Manual conversion required */\n";
    }

    public string ConvertDialogShowModal(string dialogHandle, string callbackFunctionName)
    {
        return $"/* TODO: DIALOG SHOW MODAL - Show dialog {dialogHandle} with callback {callbackFunctionName} - Manual conversion required */\n";
    }

    public string ConvertDialogEnd(string dialogHandle, string returnCode)
    {
        return $"/* TODO: DIALOG END - Close dialog {dialogHandle} with code {returnCode} - Manual conversion required */\n";
    }

    public string ConvertControlAdd(string controlType, string dialogHandle, string controlId, string text, int x, int y, int width, int height)
    {
        return $"/* TODO: CONTROL ADD {controlType} - ID {controlId}, text '{text}' at ({x}, {y}) size {width}x{height} - Manual conversion required */\n";
    }

    public string ConvertControlGetText(string dialogHandle, string controlId, string targetVariable)
    {
        return $"/* TODO: CONTROL GET TEXT - Get text from control {controlId} into {targetVariable} - Manual conversion required */\n";
    }

    public string ConvertControlSetText(string dialogHandle, string controlId, string text)
    {
        return $"/* TODO: CONTROL SET TEXT - Set control {controlId} text to '{text}' - Manual conversion required */\n";
    }

    public string ConvertCallbackFunction(string functionName, string body)
    {
        return $"/* TODO: CALLBACK FUNCTION {functionName} - Event handler - Manual conversion required */\n{body}";
    }

    public string ConvertListBoxAdd(string dialogHandle, string controlId, string text)
    {
        return $"/* TODO: LISTBOX ADD - Add '{text}' to listbox {controlId} - Manual conversion required */\n";
    }

    public string ConvertListBoxDelete(string dialogHandle, string controlId, string index)
    {
        return $"/* TODO: LISTBOX DELETE - Delete item {index} from listbox {controlId} - Manual conversion required */\n";
    }

    public string ConvertListBoxGetCount(string dialogHandle, string controlId, string targetVariable)
    {
        return $"/* TODO: LISTBOX GET COUNT - Get count from listbox {controlId} into {targetVariable} - Manual conversion required */\n";
    }

    public string ConvertListBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        return $"/* TODO: LISTBOX GET SELECT - Get selected index from listbox {controlId} into {targetVariable} - Manual conversion required */\n";
    }

    public string ConvertComboBoxAdd(string dialogHandle, string controlId, string text)
    {
        return $"/* TODO: COMBOBOX ADD - Add '{text}' to combobox {controlId} - Manual conversion required */\n";
    }

    public string ConvertComboBoxGetSelect(string dialogHandle, string controlId, string targetVariable)
    {
        return $"/* TODO: COMBOBOX GET SELECT - Get selected index from combobox {controlId} into {targetVariable} - Manual conversion required */\n";
    }

    public string ConvertComboBoxGetText(string dialogHandle, string controlId, string index, string targetVariable)
    {
        return $"/* TODO: COMBOBOX GET TEXT - Get text at index {index} from combobox {controlId} into {targetVariable} - Manual conversion required */\n";
    }

    public string ConvertMenuNewBar(string dialogHandle)
    {
        return $"/* TODO: MENU NEW BAR - Create menu bar for dialog {dialogHandle} - Manual conversion required */\n";
    }

    public string ConvertMenuNewPopup(string dialogHandle)
    {
        return $"/* TODO: MENU NEW POPUP - Create popup menu for dialog {dialogHandle} - Manual conversion required */\n";
    }

    public string ConvertMenuAddString(string text, string id, string flags)
    {
        return $"/* TODO: MENU ADD STRING - Add menu item '{text}' with ID {id} - Manual conversion required */\n";
    }

    public string ConvertMenuEndPopup()
    {
        return "/* TODO: MENU END POPUP - End popup menu definition - Manual conversion required */\n";
    }

    public string ConvertMenuAttachPopup(string dialogHandle, string popupText)
    {
        return $"/* TODO: MENU ATTACH POPUP - Attach popup menu '{popupText}' to dialog {dialogHandle} - Manual conversion required */\n";
    }

    public string ConvertGraphicWindow(string title, int x, int y, int width, int height, string handleVar)
    {
        return $"/* TODO: GRAPHIC WINDOW - Create graphic window '{title}' at ({x}, {y}) size {width}x{height} to {handleVar} - Manual conversion required */\n";
    }

    public string ConvertGraphicWindowEnd(string windowHandle)
    {
        return $"/* TODO: GRAPHIC WINDOW END - Close graphic window {windowHandle} - Manual conversion required */\n";
    }

    public string ConvertGraphicBitmapNew(int width, int height, string handleVar)
    {
        return $"/* TODO: GRAPHIC BITMAP NEW - Create memory bitmap size {width}x{height} to {handleVar} - Manual conversion required */\n";
    }

    public string ConvertGraphicBitmapEnd(string bitmapHandle)
    {
        return $"/* TODO: GRAPHIC BITMAP END - Close memory bitmap {bitmapHandle} - Manual conversion required */\n";
    }

    public string ConvertGraphicAttach(string dialogHandle, string controlId)
    {
        return $"/* TODO: GRAPHIC ATTACH - Attach to graphic control {controlId} in dialog {dialogHandle} - Manual conversion required */\n";
    }

    public string ConvertGraphicOperation(string operation, params string[] parameters)
    {
        string paramStr = parameters.Length > 0 ? string.Join(", ", parameters) : "";
        return $"/* TODO: GRAPHIC {operation} - Parameters: {paramStr} - Manual conversion required */\n";
    }

    public List<string> GetRequiredUsings()
    {
        return new List<string>();
    }

    public string GetFrameworkName()
    {
        return "None (Manual Conversion Required)";
    }
}

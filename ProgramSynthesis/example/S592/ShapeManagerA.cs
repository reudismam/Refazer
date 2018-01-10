

namespace ShareX.ScreenCaptureLib
{


    private void CreateContextMenu()
    {
        tsddbShapeOptions.DropDownItems.Add(tslnudBorderSize);
        tsddbShapeOptions.DropDownItems.Add(tsmiFillColor);
        cmsContextMenu.Items.Add(tssShapeOptions);
        cmsContextMenu.Items.Add(tslnudBlurRadius);

    }
}

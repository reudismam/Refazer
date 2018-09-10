#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2016 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShareX
{
    public partial class MainForm : HotkeyForm
    {
        public bool IsReady { get; private set; }

        private bool forceClose, trayMenuSaveSettings = true;
        private UploadInfoManager uim;
        private ToolStripDropDownItem tsmiImageFileUploaders, tsmiTrayImageFileUploaders, tsmiTextFileUploaders, tsmiTrayTextFileUploaders;
        private UpdateManager updateManager;

        public MainForm()
        {
            InitializeControls();
        }

        private void MainForm_HandleCreated(object sender, EventArgs e)
        {
            RunPuushTasks();

            UpdateControls();

            DebugHelper.WriteLine("Startup time: {0} ms", Program.StartTimer.ElapsedMilliseconds);

            UseCommandLineArgs(Program.CLI.Commands);
        }

        

        private void tsmiRectangle_Click(object sender, EventArgs e)
        {
            CaptureScreenshot(CaptureType.Region);
        }

        private void tsmiRectangleLight_Click(object sender, EventArgs e)
        {
            CaptureRectangleLight();
        }

        private void tsmiRectangleTransparent_Click(object sender, EventArgs e)
        {
            CaptureRectangleTransparent();
        }

        private void tsmiLastRegion_Click(object sender, EventArgs e)
        {
            CaptureScreenshot(CaptureType.LastRegion);
        }

        #endregion Menu events

        #region Tray events

        private void cmsTray_Opened(object sender, EventArgs e)
        {
            if (Program.Settings.TrayAutoExpandCaptureMenu)
            {
                tsmiTrayCapture.Select();
                tsmiTrayCapture.ShowDropDown();
            }
        }

        private void tsmiTrayFullscreen_Click(object sender, EventArgs e)
        {
            CaptureScreenshot(CaptureType.Fullscreen, null, false);
        }

        private void tsmiCapture_DropDownOpening(object sender, EventArgs e)
        {
            PrepareCaptureMenuAsync(tsmiTrayWindow, tsmiTrayWindowItems_Click, tsmiTrayMonitor, tsmiTrayMonitorItems_Click);
        }

        private void tsmiTrayWindowItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            WindowInfo wi = tsi.Tag as WindowInfo;
            if (wi != null)
            {
                CaptureWindow(wi.Handle, null, false);
            }
        }

        private void tsmiTrayMonitorItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            Rectangle rectangle = (Rectangle)tsi.Tag;
            if (!rectangle.IsEmpty)
            {
                DoCapture(() => TaskHelpers.GetScreenshot().CaptureRectangle(rectangle), CaptureType.Monitor, null, false);
            }
        }

        private void tsmiTrayRectangle_Click(object sender, EventArgs e)
        {
            CaptureScreenshot(CaptureType.Region, null, false);
        }

        private void tsmiTrayRectangleLight_Click(object sender, EventArgs e)
        {
            CaptureRectangleLight(null, false);
        }

        private void lvUploads_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            lvUploads.Invalidate(pbTips.Region);
        }

        private void pbPatreonOpen_Click(object sender, EventArgs e)
        {
            URLHelpers.OpenURL(Links.URL_PATREON);
        }

        private void pbPatreonHide_Click(object sender, EventArgs e)
        {
            flpPatreon.Visible = false;
            Program.Settings.ShowPatreonButton = false;
        }

        private void tsmiTrayRectangleTransparent_Click(object sender, EventArgs e)
        {
            CaptureRectangleTransparent(null, false);
        }

        private void tsmiTrayLastRegion_Click(object sender, EventArgs e)
        {
            CaptureScreenshot(CaptureType.LastRegion, null, false);
        }

        private void tsmiTrayTextCapture_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenOCR();
        }

        #endregion Tray events

        #endregion Hotkey/Capture codes and form events
    }
}
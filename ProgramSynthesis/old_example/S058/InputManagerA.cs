﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2017 ShareX Team

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

using System.Drawing;
using System.Windows.Forms;

namespace ShareX.ScreenCaptureLib
{
    public static class InputManager
    {
        public Point MousePosition => mouseState.Position;

        public Point PreviousMousePosition => oldMouseState.Position;

        public static Point MousePosition0Based => mouseState.ZeroBasedPosition;

        public static Point PreviousMousePosition0Based => oldMouseState.ZeroBasedPosition;

        public static Point MouseVelocity => new Point(MousePosition0Based.X - PreviousMousePosition0Based.X, MousePosition0Based.Y - PreviousMousePosition0Based.Y);

        public static bool IsMouseMoved => MouseVelocity.X != 0 || MouseVelocity.Y != 0;

        private static MouseState mouseState = new MouseState();
        private static MouseState oldMouseState;

        public static void Update(Control control)
        {
            oldMouseState = mouseState;
            mouseState.Update(control);
        }

        public static bool IsMouseDown(MouseButtons button)
        {
            return mouseState.Buttons.HasFlag(button);
        }

        public static bool IsBeforeMouseDown(MouseButtons button)
        {
            return oldMouseState.Buttons.HasFlag(button);
        }

        public static bool IsMousePressed(MouseButtons button)
        {
            return IsMouseDown(button) && !IsBeforeMouseDown(button);
        }

        public static bool IsMouseReleased(MouseButtons button)
        {
            return !IsMouseDown(button) && IsBeforeMouseDown(button);
        }
    }
}
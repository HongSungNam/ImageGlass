﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using igtasks;
using ImageGlass.Library;
using ImageGlass.Library.Image;

namespace adtasks
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string[] args;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            args = argv;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Lay param dau tien
            string topcmd = args[0].ToLower().Trim();

            //Them menu 'Open with ImageGlass'
            #region addext <string igPath> [string ext]
            if (topcmd == "addext")
            {
                //Lay duong dan exe
                string exePath = args[1];

                if (args.Length > 2)
                {
                    //Lay extension
                    string ext = args[2];

                    //Ap dung
                    Functions.AddImageGlassToContextMenu(exePath, ext);
                }
                else
                {
                    //Ap dung
                    Functions.AddImageGlassToContextMenu(exePath);
                }

                Application.Exit();
            }
            #endregion
            
            //Xoa menu 'Open with ImageGlass'
            #region removeext
            else if (topcmd == "removeext")
            {
                //Ap dung
                Functions.RemoveImageGlassToContextMenu();

                Application.Exit();
            }
            #endregion

            //Cap nhat extension cho Menu
            #region updateext <string exePath> <string exts>
            else if (topcmd == "updateext")
            {
                //Xoa tat ca
                Functions.RemoveImageGlassToContextMenu();

                //Them moi
                if (args.Length > 2)
                {
                    //Lay duong dan ImageGlass.exe
                    string exePath = args[1];

                    //Lay extension
                    string ext = args[2];

                    //Ap dung
                    Functions.AddImageGlassToContextMenu(exePath, ext);
                }

                Application.Exit();
            }
            #endregion

            //Thiet dat hinh nen wallpaper
            #region setwallpaper <string imgPath> [int style]
            else if (topcmd == "setwallpaper")
            {
                //Lay duong dan img
                string imgPath = args[1];

                if (args.Length > 2)
                {
                    //Lay style
                    string style = args[2];

                    //Ap dung
                    if (style == "1")
                    {
                        DesktopWallapaper.Set(new Uri(imgPath), DesktopWallapaper.Style.Stretched);
                    }
                    else if (style == "2")
                    {
                        DesktopWallapaper.Set(new Uri(imgPath), DesktopWallapaper.Style.Tiled);
                    }
                    else //style == "0"
                    {
                        DesktopWallapaper.Set(new Uri(imgPath), DesktopWallapaper.Style.Centered);
                    }
                }
                else
                {
                    //Ap dung
                    DesktopWallapaper.Set(new Uri(imgPath), DesktopWallapaper.Style.Centered);
                }

                Application.Exit();
            }
            #endregion






        }



    }
}

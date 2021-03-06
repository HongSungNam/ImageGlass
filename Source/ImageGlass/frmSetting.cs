﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2013 DUONG DIEU PHAP
Project homepage: http://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ImageGlass.Services.Configuration;

namespace ImageGlass
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private Color M_COLOR_MENU_ACTIVE = Color.FromArgb(255, 0, 123, 176);
        private Color M_COLOR_MENU_HOVER = Color.FromArgb(255, 0, 160, 220);
        private Color M_COLOR_MENU_NORMAL = Color.Silver;
        private List<Library.Language> dsLanguages = new List<Library.Language>();

        #region MOUSE ENTER - HOVER - DOWN MENU
        private void lblMenu_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_ACTIVE;
        }

        private void lblMenu_MouseUp(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;

            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = Color.FromArgb(255, M_COLOR_MENU_ACTIVE.R + 20,
                                                M_COLOR_MENU_ACTIVE.G + 20,
                                                M_COLOR_MENU_ACTIVE.B + 20);
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_HOVER;
            }
        }

        private void lblMenu_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = Color.FromArgb(255, M_COLOR_MENU_ACTIVE.R + 20,
                                                M_COLOR_MENU_ACTIVE.G + 20,
                                                M_COLOR_MENU_ACTIVE.B + 20);
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_HOVER;
            }

        }

        private void lblMenu_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = M_COLOR_MENU_ACTIVE;
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_NORMAL;
            }
        }
        #endregion

        #region MOUSE ENTER - HOVER - DOWN BUTTON
        private void lblButton_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_ACTIVE;            
        }

        private void lblButton_MouseUp(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_HOVER;
        }

        private void lblButton_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_HOVER;
        }

        private void lblButton_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender; 
            lbl.BackColor = M_COLOR_MENU_NORMAL;            
        }
        #endregion


        private void frmSetting_Load(object sender, EventArgs e)
        {
            //Load config
            //Windows Bound (Position + Size)--------------------------------------------
            Rectangle rc = GlobalSetting.StringToRect(GlobalSetting.GetConfig(this.Name + ".WindowsBound",
                                                "280,125,610, 570"));
            this.Bounds = rc;

            //windows state--------------------------------------------------------------
            string s = GlobalSetting.GetConfig(this.Name + ".WindowsState", "Normal");
            if (s == "Normal")
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (s == "Maximized")
            {
                this.WindowState = FormWindowState.Maximized;
            }

            LoadTabGeneralConfig();
            InitLanguagePack();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSetting_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
        
        private void frmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save config---------------------------------
            if (this.WindowState == FormWindowState.Normal)
            {
                //Windows Bound-------------------------------------------------------------------
                GlobalSetting.SetConfig(this.Name + ".WindowsBound", GlobalSetting.RectToString(this.Bounds));
            }

            //Windows State-------------------------------------------------------------------
            GlobalSetting.SetConfig(this.Name + ".WindowsState", this.WindowState.ToString());

            //Ép thực thi các thiết lập
            GlobalSetting.IsForcedActive = true;
        }

        /// <summary>
        /// Load language pack
        /// </summary>
        private void InitLanguagePack()
        {
            this.Text = GlobalSetting.LangPack.Items["frmSetting._Text"];
            lblGeneral.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral"];
            lblContextMenu.Text = GlobalSetting.LangPack.Items["frmSetting.lblContextMenu"];
            lblLanguage.Text = GlobalSetting.LangPack.Items["frmSetting.lblLanguage"];

            //General tab
            chkLockWorkspace.Text = GlobalSetting.LangPack.Items["frmSetting.chkLockWorkspace"];
            chkAutoUpdate.Text = GlobalSetting.LangPack.Items["frmSetting.chkAutoUpdate"];
            chkFindChildFolder.Text = GlobalSetting.LangPack.Items["frmSetting.chkFindChildFolder"];
            chkWelcomePicture.Text = GlobalSetting.LangPack.Items["frmSetting.chkWelcomePicture"];
            chkHideToolBar.Text = GlobalSetting.LangPack.Items["frmSetting.chkHideToolBar"];
            lblGeneral_ZoomOptimization.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral_ZoomOptimization"];
            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"], barInterval.Value);
            lblGeneral_MaxFileSize.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral_MaxFileSize"];
            lblImageLoadingOrder.Text = GlobalSetting.LangPack.Items["frmSetting.lblImageLoadingOrder"];
            lblBackGroundColor.Text = GlobalSetting.LangPack.Items["frmSetting.lblBackGroundColor"];
            btnClose.Text = GlobalSetting.LangPack.Items["frmSetting.btnClose"];
            lbl_ContextMenu_Description.Text = GlobalSetting.LangPack.Items["frmSetting.lbl_ContextMenu_Description"];
            lblExtensions.Text = GlobalSetting.LangPack.Items["frmSetting.lblExtensions"];
            lblAddDefaultContextMenu.Text = GlobalSetting.LangPack.Items["frmSetting.lblAddDefaultContextMenu"];
            lblUpdateContextMenu.Text = GlobalSetting.LangPack.Items["frmSetting.lblUpdateContextMenu"];
            lblRemoveAllContextMenu.Text = GlobalSetting.LangPack.Items["frmSetting.lblRemoveAllContextMenu"];
            lblLanguageText.Text = GlobalSetting.LangPack.Items["frmSetting.lblLanguageText"];
            lnkRefresh.Text = GlobalSetting.LangPack.Items["frmSetting.lnkRefresh"];
            lnkCreateNew.Text = GlobalSetting.LangPack.Items["frmSetting.lnkCreateNew"];
            lnkEdit.Text = GlobalSetting.LangPack.Items["frmSetting.lnkEdit"];
            lnkGetMoreLanguage.Text = GlobalSetting.LangPack.Items["frmSetting.lnkGetMoreLanguage"];
        }

        /// <summary>
        /// TAB LABEL CLICK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMenu_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Name == "lblGeneral")
            {
                tab1.SelectedTab = tabGeneral;
            }
            else if (lbl.Name == "lblContextMenu")
            {
                tab1.SelectedTab = tabContextMenu;
            }
            else if (lbl.Name == "lblLanguage")
            {
                tab1.SelectedTab = tabLanguage;
            }
        }

        private void tab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGeneral.Tag = 0;
            lblContextMenu.Tag = 0;
            lblLanguage.Tag = 0;

            lblGeneral.BackColor = M_COLOR_MENU_NORMAL;
            lblContextMenu.BackColor = M_COLOR_MENU_NORMAL;
            lblLanguage.BackColor = M_COLOR_MENU_NORMAL;

            if (tab1.SelectedTab == tabGeneral)
            {
                lblGeneral.Tag = 1;
                lblGeneral.BackColor = M_COLOR_MENU_ACTIVE;

                LoadTabGeneralConfig();
            }
            else if (tab1.SelectedTab == tabContextMenu)
            {
                lblContextMenu.Tag = 1;
                lblContextMenu.BackColor = M_COLOR_MENU_ACTIVE;

                txtExtensions.Text = GlobalSetting.ContextMenuExtensions;
            }
            else if (tab1.SelectedTab == tabLanguage)
            {
                lblLanguage.Tag = 1;
                lblLanguage.BackColor = M_COLOR_MENU_ACTIVE;

                lnkRefresh_LinkClicked(null, null);
            }
        }


        #region TAB GENERAL

        /// <summary>
        /// Get and load value of General tab
        /// </summary>
        private void LoadTabGeneralConfig()
        {
            //Get value of chkLockWorkspace
            chkLockWorkspace.Checked = bool.Parse(GlobalSetting.GetConfig("LockToEdge", "true"));

            //Get value of chkFindChildFolder
            chkFindChildFolder.Checked = bool.Parse(GlobalSetting.GetConfig("Recursive", "false"));

            //Get value of cmbAutoUpdate
            string s = GlobalSetting.GetConfig("AutoUpdate", "true");
            if (s != "0")
            {
                chkAutoUpdate.Checked = true;
            }
            else
            {
                chkAutoUpdate.Checked = false;
            }

            //Get value of chkWelcomePicture
            chkWelcomePicture.Checked = bool.Parse(GlobalSetting.GetConfig("Welcome", "true"));

            //Get value of chkHideToolBar
            chkHideToolBar.Checked = bool.Parse(GlobalSetting.GetConfig("IsHideToolbar", "false"));

            //Load items of cmbZoomOptimization
            cmbZoomOptimization.Items.Clear();
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._Auto"]);
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._SmoothPixels"]);
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._ClearPixels"]);

            //Get value of cmbZoomOptimization
            s = GlobalSetting.GetConfig("ZoomOptimize", "0");
            int i = 0;
            if (int.TryParse(s, out i))
            {
                if (-1 < i && i < cmbZoomOptimization.Items.Count)
                {}
                else
                {
                    i = 0;
                }
            }
            cmbZoomOptimization.SelectedIndex = i;

            //Get value of barInterval
            i = int.Parse(GlobalSetting.GetConfig("Interval", "5"));
            if (0 < i && i < 61)
            {
                barInterval.Value = i;
            }
            else
            {
                barInterval.Value = 5;
            }

            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"], 
                                        barInterval.Value);

            //Get value of numMaxThumbSize
            s = GlobalSetting.GetConfig("MaxThumbnailFileSize", "1");
            i = 1;
            if (int.TryParse(s, out i))
            {}
            numMaxThumbSize.Value = i;

            //Load items of cmbImageOrder
            cmbImageOrder.Items.Clear();
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Name"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Length"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._CreationTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._LastAccessTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._LastWriteTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Extension"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Random"]);

            //Get value of cmbImageOrder
            s = GlobalSetting.GetConfig("ImageLoadingOrder", "0");
            i = 0;
            if (int.TryParse(s, out i))
            {
                if (-1 < i && i < cmbImageOrder.Items.Count)
                { }
                else
                {
                    i = 0;
                }
            }
            cmbImageOrder.SelectedIndex = i;

            //Get background color
            picBackgroundColor.BackColor = GlobalSetting.BackgroundColor;
        }

        private void chkLockWorkspace_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsLockWorkspaceEdges = chkLockWorkspace.Checked;
        }

        private void chkAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoUpdate.Checked)
            {
                GlobalSetting.SetConfig("AutoUpdate", chkAutoUpdate.Checked.ToString());
            }
            else
            {
                GlobalSetting.SetConfig("AutoUpdate", "0");
            }
        }

        private void chkFindChildFolder_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("Recursive", chkFindChildFolder.Checked.ToString());
        }

        private void chkHideToolBar_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsHideToolBar = chkHideToolBar.Checked;
            GlobalSetting.SetConfig("IsHideToolbar", GlobalSetting.IsHideToolBar.ToString());
        }

        private void cmbZoomOptimization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZoomOptimization.SelectedIndex == 1)
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.SmoothPixels;
            }
            else if (cmbZoomOptimization.SelectedIndex == 2)
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.ClearPixels;
            }
            else
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.Auto;
            }
        }

        private void chkWelcomePicture_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsWelcomePicture = chkWelcomePicture.Checked;
        }

        private void barInterval_Scroll(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("Interval", barInterval.Value.ToString());
            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"],
                                        barInterval.Value);
        }

        private void numMaxThumbSize_ValueChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("MaxThumbnailFileSize", numMaxThumbSize.Value.ToString());
        }

        private void cmbImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("ImageLoadingOrder", cmbImageOrder.SelectedIndex.ToString());
            GlobalSetting.LoadImageOrderConfig();
        }

        private void picBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            c.AllowFullOpen = true;

            if (c.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                picBackgroundColor.BackColor = c.Color;
                GlobalSetting.BackgroundColor = c.Color;

                //Luu background color
                GlobalSetting.SetConfig("BackgroundColor", GlobalSetting.BackgroundColor.ToArgb().ToString());
            }
        }
        #endregion


        #region TAB CONTEXT MENU
        private void lblAddDefaultContextMenu_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "addext " + //name of param
                                    "\"" + Application.ExecutablePath + "\" " + //arg 1
                                    "\"" + GlobalSetting.SupportedExtensions + "\" "; //arg 2
            p.EnableRaisingEvents = true;
            p.Exited += p_Exited;

            try
            {
                p.Start();
            }
            catch { }

        }

        private void lblUpdateContextMenu_Click(object sender, EventArgs e)
        {
            //Update context menu
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "updateext " + //name of param
                                    "\"" + Application.ExecutablePath + "\" " + //arg 1
                                    "\"" + txtExtensions.Text.Trim() + "\" "; //arg 2
            p.EnableRaisingEvents = true;
            p.Exited += p_Exited;

            try
            {
                p.Start();
            }
            catch { }
        }

        private void lblRemoveAllContextMenu_Click(object sender, EventArgs e)
        {
            //Remove all context menu
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "removeext ";
            p.EnableRaisingEvents = true;
            p.Exited += p_Exited;

            try
            {
                p.Start();
            }
            catch { }

            txtExtensions.Text = GlobalSetting.ContextMenuExtensions;
        }

        void p_Exited(object sender, EventArgs e)
        {
            txtExtensions.Text = GlobalSetting.ContextMenuExtensions;
        }

        #endregion


        #region TAB LANGUAGE
        private void lnkGetMoreLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.imageglass.org/download/languagepacks");
            }
            catch { }
        }

        private void lnkCreateNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = (Application.StartupPath + "\\").Replace("\\\\", "\\") + "igcmd.exe";
            p.StartInfo.Arguments = "ignewlang";
            p.Start();
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = (Application.StartupPath + "\\").Replace("\\\\", "\\") + "igcmd.exe";
            p.StartInfo.Arguments = "igeditlang \"" + GlobalSetting.LangPack.FileName + "\"";
            p.Start();
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add("English");
            dsLanguages = new List<Library.Language>();
            dsLanguages.Add(new Library.Language());

            if (!Directory.Exists(GlobalSetting.StartUpDir + "Languages\\"))
            {
                Directory.CreateDirectory(GlobalSetting.StartUpDir + "Languages\\");
            }
            else
            {
                foreach (string f in Directory.GetFiles(GlobalSetting.StartUpDir + "Languages\\"))
                {
                    if (Path.GetExtension(f).ToLower() == ".iglang")
                    {
                        Library.Language l = new Library.Language(f);
                        dsLanguages.Add(l);

                        int iLang = cmbLanguage.Items.Add(l.LangName);
                        string curLang = GlobalSetting.LangPack.FileName;

                        //Nếu là ngôn ngữ đang dùng
                        if (f.CompareTo(curLang) == 0)
                        {
                            cmbLanguage.SelectedIndex = iLang;
                        }
                    }
                }
            }

            if (cmbLanguage.SelectedIndex == -1)
            {
                cmbLanguage.SelectedIndex = 0;
            }
        }
        
        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalSetting.LangPack = dsLanguages[cmbLanguage.SelectedIndex];
        }

        #endregion

        

        



    }
}

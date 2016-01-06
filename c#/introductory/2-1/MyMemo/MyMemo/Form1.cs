﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMemo
{
    public partial class Form1 : Form
    {
        const string ApplicationName = "MyMemo";　// アプリ名
        const string RegistryKey =
            @"Software\NikkeiSoftware" +
            ApplicationName;
        private string FilePath;

        private string FileNameValue = "";
        private string FileName
        {
            get { return FileNameValue; }
            set
            {
                FileNameValue = value;
                if(value != "")
                {
                    FilePath =
                        System.IO.Path.GetDirectoryName(value);
                }
                Edited = false;
            }
        }

        private bool EditedValue;
        private bool Edited
        {
            get { return EditedValue; }
            set
            {
                EditedValue = value;
                UpdateStatus();
            }
        }
        private void UpdateStatus()
        {
            string s = ApplicationName;
            if( FileName != "")
            {
                s += " - " + FileName;
            }
            if (Edited) s += "(変更あり) ";
            this.Text = s;

            if (FileName == "" || !Edited || textBoxMain.TextLength == 0)
                MenuItemFileSave.Enabled = false;
            else
                MenuItemFileSave.Enabled = true;

            if (!Edited || textBoxMain.TextLength == 0)
                MenuItemFileSaveAs.Enabled = false;
            else
                MenuItemFileSaveAs.Enabled = true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileName = "";
            textBoxMain.Multiline = true;
            textBoxMain.ScrollBars = ScrollBars.Vertical;
            textBoxMain.Dock = DockStyle.Fill;
            saveFileDialog1.Filter = "テキスト文章|*.txt|すべてのファイル|*.*";

            Microsoft.Win32.RegistryKey regKey =
                Microsoft.Win32.Registry.CurrentUser.
                CreateSubKey(RegistryKey);
            FilePath = regKey.GetValue("FilePath",
                System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.MyDocuments)).
                ToString();
            if(1 < Environment.GetCommandLineArgs().Length)
            {
                string[] args = Environment.GetCommandLineArgs();
                LoadFile(args[1]);
            }
        }

        private void MenuItemFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuItemFileOpen_Click(object sender, EventArgs e)
        {
            if (!AskGiveUpText()) return;

            openFileDialog1.InitialDirectory = FilePath;
            openFileDialog1.FileName = "";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                LoadFile(openFileDialog1.FileName);
            }
        }
           
        private void LoadFile(string value)
        {
            if (System.IO.File.Exists(value))
            {
                textBoxMain.Text = System.IO.File.ReadAllText(
                     value, Encoding.GetEncoding(
                         "Shift_JIS"));
                textBoxMain.Select(0, 0);
                FileName = value;

            }
            else
            {
                MessageBox.Show(value + "が見つかりません",
                    ApplicationName);
            }
        }

        private void MenuItemFileSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = FilePath;
            saveFileDialog1.FileName = System.IO.Path.GetFileName(FileName);
            if(DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                SaveFile(saveFileDialog1.FileName);
            }
        }

        private void SaveFile(string value)
        {
            System.IO.File.WriteAllText(value, textBoxMain.Text,
                System.Text.Encoding.GetEncoding("Shift_JIS"));
            FileName = value;
        }

        private void MenuItemFileSave_Click(object sender, EventArgs e)
        {
            SaveFile(FileName);
        }

        private void textBoxMain_TextChanged(object sender, EventArgs e)
        {
            Edited = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AskGiveUpText()) e.Cancel = true;
        }

        private bool AskGiveUpText()
        {
            if (!Edited || textBoxMain.TextLength == 0)
                return true;
            if (DialogResult.Yes ==
                MessageBox.Show("編集内容を破棄しますか?",
                ApplicationName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)) return true;
            else
                return false;

        }

        private void MenuItemFileNew_Click(object sender, EventArgs e)
        {
            if (!AskGiveUpText()) return;
            textBoxMain.Clear();
            FileName = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey =
                Microsoft.Win32.Registry.CurrentUser.
                CreateSubKey(RegistryKey);
            regKey.SetValue("FilePath", FilePath);
        }
    }
}

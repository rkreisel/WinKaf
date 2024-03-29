﻿using System.Windows.Forms;

namespace WinKaf
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
        }
        public void LoadText(string text)
        {
            rtbContent.Lines = new string[] { text };
            MoveToBottom();
        }

        public void LoadText(string[] lines)
        {
            rtbContent.Lines = lines;
            MoveToBottom();
        }

        private void MoveToBottom()
        {
            rtbContent.SelectionStart = rtbContent.Text.Length;
            rtbContent.ScrollToCaret();
        }
    }
}

using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace SchemaBuilder
{
    [System.Runtime.InteropServices.Guid("A687BDD9-F74C-4BB2-88E0-E2AEC95A9FCE")]
    public partial class SchemaBuilderPanelControl : UserControl
    {

        /// <summary>
        /// Returns the ID of this panel.
        /// </summary>
        public static Guid PanelId
        {
            get
            {
                return typeof(SchemaBuilderPanelControl).GUID;
            }
        }

        public SchemaBuilderPanelControl()
        {
            InitializeComponent();
            this.Controls.Add(SchemaBuilderPlugIn.m_browser);
            SchemaBuilderPlugIn.Instance.UserControl = this;
        }



    }
}

using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SchemaBuilder
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class SchemaBuilderPlugIn : Rhino.PlugIns.PlugIn
    {
        public static ChromiumWebBrowser m_browser;

        public SchemaBuilderPlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the SampleCsChromiumPlugIn plug-in.</summary>
        public static SchemaBuilderPlugIn Instance
        {
            get;
            private set;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and mantain plug-in wide options in a document.

        /// <summary>
        /// Called when the plug-in is being loaded.
        /// </summary>
        protected override Rhino.PlugIns.LoadReturnCode OnLoad(ref string errorMessage)
        {
            Debug.WriteLine("Cef Loaded: " + Cef.IsInitialized, "SchemaBuilder");
            var panel_type = typeof(SchemaBuilderPanelControl);
            Rhino.UI.Panels.RegisterPanel(this, panel_type, "SchemaBuilder", SchemaBuilder.Properties.Resources.SchemaBuilder);
            if (!Cef.IsInitialized)
                InitializeCef();

            InitializeBrowser();

            return Rhino.PlugIns.LoadReturnCode.Success;
        }

        void InitializeCef()
        {

            Cef.EnableHighDPISupport();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Path.GetDirectoryName(assemblyLocation);
            string pathSubprocess = Path.Combine(assemblyPath, "CefSharp.BrowserSubprocess.exe");

            CefSettings settings = new CefSettings
            {
                LogSeverity = LogSeverity.Verbose,
                LogFile = "ceflog.txt",
                BrowserSubprocessPath = pathSubprocess,
            };

            //Not needed in Rhino 6
            settings.CefCommandLineArgs.Add("disable-gpu", "1");

            // Initialize cef with the provided settings

            Cef.Initialize(settings);

        }

        private void InitializeBrowser()
        {

            if (Cef.IsInitialized)
            {
#if DEBUG
                var index = "http://localhost:9090/";
#else
                var path = Directory.GetParent(Assembly.GetExecutingAssembly().Location);

                var index = string.Format(@"{0}\app\index.html", path);

                if (!File.Exists(index))
                    Debug.WriteLine("Error. The html file doesn't exists : {0}", "SchemaBuilder");

                index = index.Replace("\\", "/");
#endif
                m_browser = new ChromiumWebBrowser(index)
                {
                    BrowserSettings = new BrowserSettings
                    {
                        FileAccessFromFileUrls = CefState.Enabled,
                        UniversalAccessFromFileUrls = CefState.Enabled
                    },

                    Dock = DockStyle.Fill
                };


                m_browser.RegisterAsyncJsObject("Interop", new Interop(m_browser));
            }
            else
            {
                Debug.WriteLine("For some reason, Cef didn't initialize", "SchemaBuilder");
            }

        }

        protected override void OnShutdown()
        {
            m_browser.Dispose();
            Cef.Shutdown();

            //Store.Dispose();
            SchemaBuilderPlugIn.Instance.UserControl?.Dispose();
            base.OnShutdown();
        }


        /// <summary>
        /// Gets tabbed dockbar user control
        /// </summary>
        public SchemaBuilderPanelControl UserControl
        {
            get;
            set;
        }
    }
}
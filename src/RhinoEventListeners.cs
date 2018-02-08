using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.DocObjects.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using CefSharp.WinForms;
using CefSharp;

namespace SchemaBuilder
{
    internal class RhinoEventListeners
    {
        
        public static ChromiumWebBrowser Browser { get; set; }
        /// <summary>
        /// Private constructor
        /// </summary>
        internal RhinoEventListeners()
        {
            IsEnabled = false;
        }

        public ObjectsInTheTree displayObjInTheTree { get; set; }
        public bool? displayObjInTheTree_bool = null;

        /// <summary>
        /// The one and only SampleCsEventHandlers object
        /// </summary>
        private static RhinoEventListeners g_instance;

        /// <summary>
        /// Returns the one and only instance of this object
        /// </summary>
        public static RhinoEventListeners Instance
        {
            get { return g_instance ?? (g_instance = new RhinoEventListeners()); }
            //SetInteropInstance(Interop interop);
    }

        /// <summary>
        /// Returns the enabled status
        /// </summary>
        public bool IsEnabled { get; private set; }
        public Interop myInterop { get; set; }
        public bool transformTrigger { get; set; }
        public bool _keyPressed { get; set; }
        //public bool transformTrigger = false;
        

        /// <summary>
        /// Enables or disables the event handlers
        /// </summary>
        public void Enable(bool enable)
        {
            if (enable != IsEnabled)
            {
                if (enable)
                {


                    //RhinoApp.AppSettingsChanged += OnAppSettingsChanged;
                    //RhinoApp.Closing += OnClosing;
                    RhinoApp.EscapeKeyPressed += OnEscapeKeyPressed;
                    //Rhino.RhinoApp.KeyboardEvent += new Rhino.RhinoApp.KeyboardHookEvent(RhinoApp_KeyboardEvent);
                    //Rhino.RhinoApp.KeyboardEvent += RhinoApp_KeyboardEvent1;
                    //public event EventHandler ThresholdReached;

                    //RhinoApp.Initialized += OnInitialized;
                    //RhinoApp.RendererChanged += OnRendererChanged;

                    //RhinoDoc.NewDocument += OnNewDocument;
                    //RhinoDoc.BeginOpenDocument += OnBeginOpenDocument;
                    RhinoDoc.EndOpenDocument += OnEndOpenDocument;
                    //RhinoDoc.BeginSaveDocument += OnBeginSaveDocument;
                    //RhinoDoc.EndSaveDocument += OnEndSaveDocument;
                    //RhinoDoc.CloseDocument += OnCloseDocument;

                    //RhinoDoc.AddRhinoObject += OnAddRhinoObject;
                    //RhinoDoc.ReplaceRhinoObject += OnReplaceRhinoObject;
                    //RhinoDoc.DeleteRhinoObject += OnDeleteRhinoObject;
                    //RhinoDoc.UndeleteRhinoObject += OnUndeleteRhinoObject;
                    //RhinoDoc.PurgeRhinoObject += OnPurgeRhinoObject;
                    RhinoDoc.ModifyObjectAttributes += OnModifyObjectAttributes;
                    RhinoDoc.BeforeTransformObjects += OnBeforeTransformObjects;

                    RhinoDoc.SelectObjects += OnSelectObjects;
                    RhinoDoc.DeselectObjects += OnDeselectObjects;
                    RhinoDoc.DeselectAllObjects += OnDeselectAllObjects;

                    //RhinoDoc.GroupTableEvent += OnGroupTableEvent;
                    //RhinoDoc.InstanceDefinitionTableEvent += OnInstanceDefinitionTableEvent;
                    RhinoDoc.LayerTableEvent += OnLayerTableEvent;
                    //RhinoDoc.LightTableEvent += OnLightTableEvent;
                    //RhinoDoc.MaterialTableEvent += OnMaterialTableEvent;
                    //RhinoDoc.DocumentPropertiesChanged += OnDocumentPropertiesChanged;

                    //RhinoDoc.RenderEnvironmentTableEvent += OnRenderEnvironmentTableEvent;
                    //RhinoDoc.RenderTextureTableEvent += OnRenderTextureTableEvent;
                    //RhinoDoc.TextureMappingEvent += OnTextureMappingEvent;

                    //RhinoView.Create += OnCreateViewEventHandler;
                    //RhinoView.Rename += OnRenameViewEventHandler;
                    //RhinoView.SetActive += OnSetActiveViewEventHandler;
                    //RhinoView.Destroy += OnDestroyViewEventHandler;

                    //Command.BeginCommand += OnBeginCommand;
                    //Command.EndCommand += OnEndCommand;
                    //Command.UndoRedo += OnUndoRedo;
                }
                else
                {
                    //RhinoApp.AppSettingsChanged -= OnAppSettingsChanged;
                    //RhinoApp.Closing -= OnClosing;
                    RhinoApp.EscapeKeyPressed -= OnEscapeKeyPressed;

                    //RhinoApp.Idle -= OnIdle;
                    
                    //RhinoApp.Initialized -= OnInitialized;
                    //RhinoApp.RendererChanged -= OnRendererChanged;

                    //RhinoDoc.NewDocument -= OnNewDocument;
                    //RhinoDoc.BeginOpenDocument -= OnBeginOpenDocument;
                    RhinoDoc.EndOpenDocument -= OnEndOpenDocument;
                    //RhinoDoc.BeginSaveDocument -= OnBeginSaveDocument;
                    RhinoDoc.EndSaveDocument -= OnEndSaveDocument;
                    //RhinoDoc.CloseDocument -= OnCloseDocument;

                    //RhinoDoc.AddRhinoObject -= OnAddRhinoObject;
                    //RhinoDoc.ReplaceRhinoObject -= OnReplaceRhinoObject;
                    //RhinoDoc.DeleteRhinoObject -= OnDeleteRhinoObject;
                    //RhinoDoc.UndeleteRhinoObject -= OnUndeleteRhinoObject;
                    //RhinoDoc.PurgeRhinoObject -= OnPurgeRhinoObject;
                    RhinoDoc.ModifyObjectAttributes -= OnModifyObjectAttributes;
                    RhinoDoc.BeforeTransformObjects -= OnBeforeTransformObjects;

                    RhinoDoc.SelectObjects -= OnSelectObjects;
                    RhinoDoc.DeselectObjects -= OnDeselectObjects;
                    RhinoDoc.DeselectAllObjects -= OnDeselectAllObjects;

                    //RhinoDoc.GroupTableEvent -= OnGroupTableEvent;
                    //RhinoDoc.InstanceDefinitionTableEvent -= OnInstanceDefinitionTableEvent;
                    RhinoDoc.LayerTableEvent -= OnLayerTableEvent;
                    //RhinoDoc.LightTableEvent -= OnLightTableEvent;
                    //RhinoDoc.MaterialTableEvent -= OnMaterialTableEvent;
                    //RhinoDoc.DocumentPropertiesChanged -= OnDocumentPropertiesChanged;

                    //RhinoDoc.RenderEnvironmentTableEvent -= OnRenderEnvironmentTableEvent;
                    //RhinoDoc.RenderTextureTableEvent -= OnRenderTextureTableEvent;
                    //RhinoDoc.TextureMappingEvent -= OnTextureMappingEvent;

                    //RhinoView.Create -= OnCreateViewEventHandler;
                    //RhinoView.Rename -= OnRenameViewEventHandler;
                    //RhinoView.SetActive -= OnSetActiveViewEventHandler;
                    //RhinoView.Destroy -= OnDestroyViewEventHandler;

                    //Command.BeginCommand -= OnBeginCommand;
                    //Command.EndCommand -= OnEndCommand;
                    //Command.UndoRedo -= OnUndoRedo;
                }
            }
            IsEnabled = enable;
        }


        void RefreshSelected()
        {
            RhinoDoc.SelectObjects += OnSelectObjects;
        }

        private void RhinoApp_KeyboardEvent1(int key)
        {
            Debug.WriteLine(key);
            //throw new NotImplementedException();
        }


        #region Application Events

        /// <summary>
        /// Called when the Rhino application is initialized
        /// </summary>
        public static void OnInitialized(object sender, EventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when the Rhino application is closing
        /// </summary>
        public static void OnClosing(object sender, EventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when an Rhino application setting has changed
        /// </summary>
        public static void OnAppSettingsChanged(object sender, EventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when the active rendering plug-in has changed
        /// </summary>
        public static void OnRendererChanged(object sender, EventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino enters an idle state
        /// </summary>
        /// 
        public void OnIdle(object sender, EventArgs e)
        {
            DebugWriteMethod();
            myInterop.SpreadObjects(0, true);
            RhinoApp.Idle -= OnIdle;
        }
        public void OnIdleHover(object sender, EventArgs e)
        {
            DebugWriteMethod();
            Rhino.RhinoApp.WriteLine("after hover");
            //myInterop.SpreadObjects(0, true);
            //System.Threading.Thread.Sleep(1000);
            RhinoApp.Idle -= OnIdleHover;
        }
        /// <summary>
        /// Called when the escape key has been pressed
        /// </summary>
        public void OnEscapeKeyPressed(object sender, EventArgs e)
        {
            DebugWriteMethod();
            Rhino.RhinoApp.WriteLine("biaaatch!");
        }

        void RhinoApp_KeyboardEvent(int key)
        {
            //Debug.WriteLine(key);

            //myKeyPressed = key == 16;

            //return;
            if (key == 17) // ctrl key pressed
            {


                Rhino.RhinoApp.WriteLine("Fire");

                System.Drawing.Point myloc = System.Windows.Forms.Cursor.Position;
                Rhino.UI.MouseCursor.SetToolTip("heywassup");
                //Rhino.RhinoApp.WriteLine(myloc.X.ToString()+","+myloc.Y.ToString());
                Rhino.Display.RhinoView myViewport = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
                System.Drawing.Rectangle view_screen_rect = myViewport.ScreenRectangle;

                double XCoor = myloc.X - view_screen_rect.Left;
                double YCoor = myloc.Y - view_screen_rect.Top;
                //Rhino.RhinoApp.WriteLine(XCoor + "," + YCoor);


                Rhino.Display.RhinoViewport viewport = myViewport.ActiveViewport;
                System.Drawing.Point view_client_point = new System.Drawing.Point();
                view_client_point.X = (int)XCoor;
                view_client_point.Y = (int)YCoor;

                Rhino.Geometry.Line myLine = new Rhino.Geometry.Line();
                bool gotline = viewport.GetFrustumLine(view_client_point.X, view_client_point.Y, out myLine);
                if (gotline == true)
                {
                    List<Guid> myGuids = myInterop.allGuids;
                    List<Guid> myHoveredGuids = new List<Guid>();

                    foreach (Guid guid in myGuids)
                    {
                        Rhino.DocObjects.RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(guid);
                        Rhino.Geometry.Curve[] myCrvs;
                        Rhino.Geometry.Point3d[] myPts;
                        bool cbx = Rhino.Geometry.Intersect.Intersection.CurveBrep(myLine.ToNurbsCurve(), foundObject.Geometry.GetBoundingBox(true).ToBrep(), 0.01, out myCrvs, out myPts);
                        if (myPts.Length > 0)
                        {
                            myHoveredGuids.Add(foundObject.Id);
                        }
                    }

                    if (myHoveredGuids.Count > 0)
                    {
                        List<double> allDists = new List<double>();
                        foreach (Guid guid in myHoveredGuids)
                        {
                            Rhino.DocObjects.RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(guid);
                            Rhino.Geometry.Point3d myCe = foundObject.Geometry.GetBoundingBox(true).Center;
                            double dist = myLine.To.DistanceTo(myCe);
                            allDists.Add(dist);
                        }
                        var sorted = allDists
                            .Select((x, i) => new KeyValuePair<double, int>(x, i))
                            .OrderBy(x => x.Key)
                            .ToList();

                        List<double> B = sorted.Select(x => x.Key).ToList();
                        List<int> idx = sorted.Select(x => x.Value).ToList();
                        Rhino.RhinoApp.WriteLine(myHoveredGuids[idx[0]].ToString());
                        RhinoApp.Idle += OnIdleHover;
                    }
                    else
                    {
                        Rhino.RhinoApp.WriteLine("C'est la dech");
                    }
                    
                }
                else
                {
                    Rhino.RhinoApp.WriteLine("no keys");
                }


            }



        }

        #endregion

        #region File Events

        /// <summary>
        /// Called when Rhino creates a new document
        /// </summary>
        public static void OnNewDocument(object sender, DocumentEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino begins to open a new document
        /// </summary>
        public static void OnBeginOpenDocument(object sender, DocumentOpenEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino finishes opening a new document
        /// </summary>
        public static void OnEndOpenDocument(object sender, DocumentOpenEventArgs e)
        {
            DebugWriteMethod();
            //update layer table info
            //RhinoEventListeners.g_instance.WriteLayers();
        }

        /// <summary>
        /// Called when Rhino begins to save the active document
        /// </summary>
        public static void OnBeginSaveDocument(object sender, DocumentSaveEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino finishes saving a document
        /// </summary>
        public static void OnEndSaveDocument(object sender, DocumentSaveEventArgs e)
        {
            DebugWriteMethod();
            //RhinoEventListeners.g_instance.WriteLayers();
        }

        /// <summary>
        /// Called when Rhino closes the active document
        /// </summary>
        public static void OnCloseDocument(object sender, DocumentEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        #region Object Events

        /// <summary>
        /// Called if a new object is added to the document.
        /// </summary>
        void OnAddRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            RhinoApp.WriteLine("add");
            DebugWriteMethod();
        }

        void OnReplaceRhinoObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
        {
            RhinoApp.WriteLine("replace");
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when an object is deleted
        /// </summary>
        void OnDeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            RhinoApp.WriteLine("delete");
            DebugWriteMethod();
        }

        void OnUndeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            myInterop.SpreadObjects(0, true);
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino permanently deletes an object
        /// </summary>
        void OnPurgeRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            RhinoApp.WriteLine("purge");
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when an object's attributes have been modified
        /// </summary>
        void OnModifyObjectAttributes(object sender, Rhino.DocObjects.RhinoModifyObjectAttributesEventArgs e)
        {
            RhinoApp.WriteLine("atts");
            DebugWriteMethod();
        }

        /// <summary>
        /// Called before objects are being transformed
        /// </summary>
        public void SetInteropInstance(Interop interop) => this.myInterop = interop;

        void OnBeforeTransformObjects(object sender, Rhino.DocObjects.RhinoTransformObjectsEventArgs e)
        {
            RhinoApp.Idle += OnIdle;
        }

        #endregion

        #region Object Select Events

        /// <summary>
        /// Called when objects are selected
        /// </summary>
        public void OnSelectObjects(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e)
        {
            DebugWriteMethod();
            if (displayObjInTheTree_bool == null)
            {
            }
            else
            {
                displayObjInTheTree.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
            List<Rhino.DocObjects.RhinoObject> mySelectedObjects = Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(true, true).ToList();
            mySelectedObjects.Reverse(); // orders selection from older to younger
            RhinoApp.WriteLine(mySelectedObjects.Count.ToString());

            List<Guid> selIds = new List<Guid>();
            foreach (Rhino.DocObjects.RhinoObject obj in mySelectedObjects)
            {
                if (myInterop.allGuids != null)
                {
                    if (myInterop.allGuids.Exists(x => x == obj.Id))
                    {
                        RhinoApp.WriteLine("It's in the tree.");
                        selIds.Add(obj.Id);

                    }
                    else
                    {
                        RhinoApp.WriteLine("It's NOT in the tree.");
                    }
                }
                else
                {
                    RhinoApp.WriteLine("NOTHING is in the tree.");
                }
            }

            if (selIds.Count > 0)
            {
                displayObjInTheTree_bool = true;
                displayObjInTheTree = new ObjectsInTheTree();
                displayObjInTheTree.Ids = selIds;
                displayObjInTheTree.Enabled = true;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                var script = string.Format("window.bus.$emit('{0}', '{1}')", "my-selected-ids", JsonConvert.SerializeObject(selIds));
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
        }

        /// <summary>
        /// Called when objects are deselected
        /// </summary>
        public void OnDeselectObjects(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e)
        {
            
            DebugWriteMethod();
            RhinoApp.WriteLine("Deselected");
            //RhinoDoc.SelectObjects += OnSelectObjects;
            if (displayObjInTheTree_bool == null)
            {
            }
            else
            {
                displayObjInTheTree.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
            List<Rhino.DocObjects.RhinoObject> mySelectedObjects = Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(true, true).ToList();
            mySelectedObjects.Reverse(); // orders selection from older to younger
            RhinoApp.WriteLine(mySelectedObjects.Count.ToString());

            List<Guid> selIds = new List<Guid>();
            foreach (Rhino.DocObjects.RhinoObject obj in mySelectedObjects)
            {
                if (myInterop.allGuids != null)
                {
                    if (myInterop.allGuids.Exists(x => x == obj.Id))
                    {
                        RhinoApp.WriteLine("It's in the tree.");
                        selIds.Add(obj.Id);
                    }
                    else
                        RhinoApp.WriteLine("It's NOT in the tree.");
                }
                else
                    RhinoApp.WriteLine("NOTHING is in the tree.");
            }

            if (selIds.Count > 0)
            {
                displayObjInTheTree_bool = true;
                displayObjInTheTree = new ObjectsInTheTree();
                displayObjInTheTree.Ids = selIds;
                displayObjInTheTree.Enabled = true;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                var script = string.Format("window.bus.$emit('{0}', '{1}')", "my-selected-ids", JsonConvert.SerializeObject(selIds));
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
        }

        /// <summary>
        /// Called when all objects are deselected
        /// </summary>
        public void OnDeselectAllObjects(object sender, Rhino.DocObjects.RhinoDeselectAllObjectsEventArgs e)
        {
            DebugWriteMethod();
            RhinoApp.WriteLine("DeselectedAll");
            if (displayObjInTheTree_bool == null)
            {
            }
            else
            {
                displayObjInTheTree.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                var script = string.Format("window.bus.$emit('{0}', '{1}')", "deselection-all", "false");
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
        }

        #endregion

        #region Document and Table Events

        /// <summary>
        /// A group table event has occurred
        /// </summary>
        void OnGroupTableEvent(object sender, Rhino.DocObjects.Tables.GroupTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// An instance definition table event has occurred
        /// </summary>
        void OnInstanceDefinitionTableEvent(object sender, Rhino.DocObjects.Tables.InstanceDefinitionTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A layer table event has occurred
        /// </summary>
        void OnLayerTableEvent(object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
        {
            //DebugWriteMethod();
            //Debug.WriteLine("Layer Index: " + e.LayerIndex.ToString(),"Layer Watcher");
            //Debug.WriteLine("New State: " + e.NewState.ToString(), "Layer Watcher");
            //Debug.WriteLine("Old State: " + e.OldState.ToString(), "Layer Watcher");
            Debug.WriteLine("Event Type: " + e.EventType.ToString(), "Layer Stalker");
            switch (e.EventType)
            {
                case LayerTableEventType.Added:
                case LayerTableEventType.Current:
                case LayerTableEventType.Deleted:
                case LayerTableEventType.Modified:
                case LayerTableEventType.Sorted:
                case LayerTableEventType.Undeleted:
                    //RhinoEventListeners.g_instance.WriteLayers();
                    break;
            }

        }

        /// <summary>
        /// A light table event has occurred
        /// </summary>
        void OnLightTableEvent(object sender, Rhino.DocObjects.Tables.LightTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A material table event has occurred
        /// </summary>
        void OnMaterialTableEvent(object sender, Rhino.DocObjects.Tables.MaterialTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when the properties of the active document are changed
        /// </summary>
        void OnDocumentPropertiesChanged(object sender, DocumentEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        #region Render Events

        /// <summary>
        /// A render environemnt table event has occurred
        /// </summary>
        public static void OnRenderEnvironmentTableEvent(object sender, RhinoDoc.RenderContentTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A render texture table event has occurred
        /// </summary>
        public static void OnRenderTextureTableEvent(object sender, RhinoDoc.RenderContentTableEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A texture mapping event has occurred
        /// </summary>
        public static void OnTextureMappingEvent(object sender, RhinoDoc.TextureMappingEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        #region View Events

        /// <summary>
        /// A view was created
        /// </summary>
        public static void OnCreateViewEventHandler(object sender, ViewEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A view was renamed
        /// </summary>
        public static void OnRenameViewEventHandler(object sender, ViewEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A view was make active
        /// </summary>
        public static void OnSetActiveViewEventHandler(object sender, ViewEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// A view was destroyed
        /// </summary>
        public static void OnDestroyViewEventHandler(object sender, ViewEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        #region Command Events

        public static void OnBeginCommand(object sender, CommandEventArgs e)
        {
            DebugWriteMethod();
        }

        public static void OnEndCommand(object sender, CommandEventArgs e)
        {
            DebugWriteMethod();
        }

        public static void OnUndoRedo(object sender, UndoRedoEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        private static void DebugWriteMethod()
        {
#if DEBUG
            var class_name = MethodBase.GetCurrentMethod().DeclaringType.Name;
            var method_name = new StackTrace().GetFrame(1).GetMethod().Name;
            //RhinoApp.WriteLine("** EVENT: {0}.{1} **", class_name, method_name);
#endif
        }
    }


}

﻿using Rhino;
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

namespace SchemaBuilder
{
    internal class RhinoEventListeners
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private RhinoEventListeners()
        {
            IsEnabled = false;
        }


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
        }

        /// <summary>
        /// Returns the enabled status
        /// </summary>
        public bool IsEnabled { get; private set; }

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
                    //RhinoApp.EscapeKeyPressed += OnEscapeKeyPressed;
                    //RhinoApp.Idle += OnIdle;
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
                    //RhinoDoc.ModifyObjectAttributes += OnModifyObjectAttributes;
                    //RhinoDoc.BeforeTransformObjects += OnBeforeTransformObjects;

                    //RhinoDoc.SelectObjects += OnSelectObjects;
                    //RhinoDoc.DeselectObjects += OnDeselectObjects;
                    //RhinoDoc.DeselectAllObjects += OnDeselectAllObjects;

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
                    //RhinoApp.EscapeKeyPressed -= OnEscapeKeyPressed;
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
                    //RhinoDoc.ModifyObjectAttributes -= OnModifyObjectAttributes;
                    //RhinoDoc.BeforeTransformObjects -= OnBeforeTransformObjects;

                    //RhinoDoc.SelectObjects -= OnSelectObjects;
                    //RhinoDoc.DeselectObjects -= OnDeselectObjects;
                    //RhinoDoc.DeselectAllObjects -= OnDeselectAllObjects;

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



        public static List<string> getAllChildren(Rhino.DocObjects.Layer layer) {
            string fullPath = layer.FullPath;
            List<string> predecessors = new List<string>();
            if (fullPath.Contains("::")){
                string[] preds = fullPath.Split(new[] { "::" }, StringSplitOptions.None);
                foreach(var p in preds)
                {
                    predecessors.Add(p);
                }
            }
            else
            {
                predecessors.Add(fullPath);
            }
            return predecessors;
        }

        public static string ToHexValue(Color color)
        {
            return "#" + color.R.ToString("X2") +
                            color.G.ToString("X2") +
                            color.B.ToString("X2");
        }







        public void refresh()
        {

            Rhino.DocObjects.Tables.ObjectTable docObjects = RhinoDoc.ActiveDoc.Objects;

            foreach (var obj in docObjects)
            {
                bool isHidden = obj.IsHidden;
                if (isHidden == true)
                {
                }
                else
                {
                    obj.Attributes.Visible = false;
                    obj.CommitChanges();
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }

        public void addToDisplay(string layer)
        {
            RhinoDoc.ActiveDoc.Objects.UnselectAll();
            var objs = RhinoDoc.ActiveDoc.Objects.FindByLayer(layer);
            if (null != objs && objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    var obj = objs[i];
                    obj.Attributes.Visible = true;
                    obj.CommitChanges();
                    //obj.Select(true);

                }
                RhinoDoc.ActiveDoc.Views.Redraw();
            }

            Rhino.DocObjects.Tables.ObjectTable docObjects = RhinoDoc.ActiveDoc.Objects;

        }

        public void SetCurrentLayer(string layer)
        {
            Rhino.DocObjects.Tables.LayerTable layers = RhinoDoc.ActiveDoc.Layers;
            int layerIndex = layers.Find(layer, true);
            layers.SetCurrentLayerIndex(layerIndex, true);



        }

        public void HideUnselectedObjects(string layer)
        {
            Rhino.DocObjects.Tables.ObjectTable docObjects = RhinoDoc.ActiveDoc.Objects;

            foreach (var obj in docObjects)
            {
                int isSelected = obj.IsSelected(false);
                if (isSelected > 0)
                {
                    obj.Attributes.Visible = true;
                    obj.CommitChanges();
                }
                else
                {
                    obj.Attributes.Visible = false;
                    obj.CommitChanges();
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }

        /*
        public static int[] getLayerCounts()
        {
            Rhino.DocObjects.ObjectEnumeratorSettings objEnum = new Rhino.DocObjects.ObjectEnumeratorSettings
            {
                VisibleFilter = false,
                IncludeLights = true,
                IncludeGrips = true,
                NormalObjects = true,
                LockedObjects = true,
                HiddenObjects = true
            };
            var allObjectsInDoc = Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(objEnum);
            int[] layerCounts = Enumerable.Repeat(0, Rhino.RhinoDoc.ActiveDoc.Layers.Count).ToArray();
            foreach (var obj in allObjectsInDoc)
            {
                layerCounts[obj.Attributes.LayerIndex]++;
            }
            return layerCounts;
        }*/


        private void makeLayerVisible(Rhino.DocObjects.Layer lay)
        {
            Rhino.DocObjects.Layer layer1 = lay;
            List<Guid> guidList = new List<Guid>();
            while (true)
            {
                guidList.Add(layer1.Id);
                if (!(layer1.ParentLayerId == Guid.Empty))
                    layer1 = Rhino.RhinoDoc.ActiveDoc.Layers[(Rhino.RhinoDoc.ActiveDoc.Layers.Find(layer1.ParentLayerId, true))];
                else
                    break;
            }
            for (int index = guidList.Count - 1; index >= 0; --index)
            {
                Rhino.DocObjects.Layer layer2 = Rhino.RhinoDoc.ActiveDoc.Layers[Rhino.RhinoDoc.ActiveDoc.Layers.Find(guidList[index], true)];
                layer2.IsVisible = true;
                layer2.CommitChanges();
            }
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
        public static void OnIdle(object sender, EventArgs e)
        {
            //DebugWriteMethod();
        }

        /// <summary>
        /// Called when the escape key has been pressed
        /// </summary>
        public static void OnEscapeKeyPressed(object sender, EventArgs e)
        {
            DebugWriteMethod();
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
            DebugWriteMethod();
        }

        void OnReplaceRhinoObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when an object is deleted
        /// </summary>
        void OnDeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            DebugWriteMethod();
        }

        void OnUndeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when Rhino permanently deletes an object
        /// </summary>
        void OnPurgeRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when an object's attributes have been modified
        /// </summary>
        void OnModifyObjectAttributes(object sender, Rhino.DocObjects.RhinoModifyObjectAttributesEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called before objects are being transformed
        /// </summary>
        void OnBeforeTransformObjects(object sender, Rhino.DocObjects.RhinoTransformObjectsEventArgs e)
        {
            DebugWriteMethod();
        }

        #endregion

        #region Object Select Events

        /// <summary>
        /// Called when objects are selected
        /// </summary>
        public static void OnSelectObjects(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when objects are deselected
        /// </summary>
        public static void OnDeselectObjects(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when all objects are deselected
        /// </summary>
        public static void OnDeselectAllObjects(object sender, Rhino.DocObjects.RhinoDeselectAllObjectsEventArgs e)
        {
            DebugWriteMethod();
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
            RhinoApp.WriteLine("** EVENT: {0}.{1} **", class_name, method_name);
#endif
        }
    }


}

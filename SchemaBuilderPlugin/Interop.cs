﻿using CefSharp.WinForms;
using CefSharp;
using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SchemaBuilder
{



    public class Interop
    {

        private static ChromiumWebBrowser Browser { get; set; }

        public SchemaBuilderDisplay myDisplay { get; set; }
        public bool? myDisplayBool = null;
        public SchemaBuilderDisplayOnHover myDisplayOnHover { get; set; }
        public bool? myDisplayBoolOnHover = null;
        public SchemaBuilderDisplayGraphOnHover myDisplayGraphOnHover { get; set; }
        public bool? myDisplayGraphBoolOnHover = null;
        public List<Guid> allGuids { get; set; }
        public List<Rhino.Geometry.Point3d> originalCentroids = new List<Rhino.Geometry.Point3d>();
        public Rhino.Geometry.Point3d explosionCenter { get; set; }

        private RhinoEventListeners myListeners { get; set; }

        public SchemaBuilderDisplayEdges myDisplayEdges { get; set; }
        public bool? myDisplayEdgesBool = null;

        //public Rhino.Geometry.BoundingBox myGlobalBbox { get; set; }

        public Interop(ChromiumWebBrowser browser)
        {
            Browser = browser;
            RhinoEventListeners.Browser = browser;
            myListeners = new RhinoEventListeners();
            myListeners.Enable(true);
            myListeners.SetInteropInstance(this);
        }

        public void ShowDev()
        {
            Browser.ShowDevTools();
        }


        public void ShowHoveredGraph(string myGUIDstring, string myDirectedPath)
        {
            myDisplayGraphBoolOnHover = true;
            List<List<Guid>> myDirectedGUIDs = JsonConvert.DeserializeObject<List<List<Guid>>>(myDirectedPath);

            myDisplayGraphOnHover = new SchemaBuilderDisplayGraphOnHover();
            myDisplayGraphOnHover.Edges = myDirectedGUIDs;
            Guid myGuid = new Guid(myGUIDstring);
            myDisplayGraphOnHover.Id = myGuid;
            myDisplayGraphOnHover.Enabled = true;
            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void HideHoveredGraph()
        {
            if (myDisplayGraphBoolOnHover == null)
            {
            }
            else
            {
                myDisplayGraphOnHover.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
        }

        public void ShowObjects(string myGUIDs)
        {
            myDisplayBool = true;
            myDisplay = new SchemaBuilderDisplay();
            
            List<Guid> myListGUIDs = JsonConvert.DeserializeObject<List<Guid>>(myGUIDs);
            allGuids = myListGUIDs;

            //RhinoApp.WriteLine(myListGUIDs[0].ToString());
            myDisplay.Ids = myListGUIDs;
            myDisplay.Enabled = true;
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void SpreadObjects(double step, Boolean refresh)
        {
            
            if (myDisplayBool == null)
            {
            }
            else
            {
                Rhino.Display.RhinoView myViewport = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
                
                Rhino.Display.RhinoViewport viewport = myViewport.ActiveViewport;
                viewport.DisplayMode = Rhino.Display.DisplayModeDescription.FindByName("Wireframe");
                if (step == 0.0)
                    viewport.DisplayMode = Rhino.Display.DisplayModeDescription.FindByName("Copy of Artistic 01");
                //else
                    //viewport.DisplayMode = Rhino.Display.DisplayModeDescription.FindByName("Wireframe");
                Rhino.Geometry.BoundingBox myGlobalBbox = new Rhino.Geometry.BoundingBox();
                myGlobalBbox = Rhino.Geometry.BoundingBox.Empty;
                List<Rhino.Geometry.Point3d> myCentroids = new List<Rhino.Geometry.Point3d>();
                //myCentroids.Clear();
                //Rhino.Geometry.Point3d explosionCenter;

                // First iteration: find initial object and initial bounding box center
                if (originalCentroids.Count == 0 || refresh == true)
                {
                    myCentroids.Clear();
                    foreach (Guid guid in allGuids)
                    {
                        RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(guid);
                        Rhino.Geometry.BoundingBox foundBbox = foundObject.Geometry.GetBoundingBox(true);
                        myGlobalBbox.Union(foundBbox);
                        myCentroids.Add(foundBbox.Center);
                        explosionCenter = myGlobalBbox.Center;
                    }
                    originalCentroids = myCentroids;
                    
                }
                else
                {
                    for (int i = 0; i < allGuids.Count; i++)
                    {
                        RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(allGuids[i]);
                        Rhino.Geometry.Vector3d trans = explosionCenter - originalCentroids[i];
                        
                        // Brings back to original position.
                        Rhino.Geometry.Vector3d backTrans = originalCentroids[i] - foundObject.Geometry.GetBoundingBox(true).Center;
                        trans.Unitize();
                        Rhino.RhinoDoc.ActiveDoc.Objects.Transform(foundObject, Rhino.Geometry.Transform.Translation(backTrans-Rhino.Geometry.Vector3d.Multiply(step, trans)), true);
                        Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                    }
                }
            }
        }

        public void HideObjects()
        {
            if (myDisplayBool == null)
            {
            }
            else
            {
                myDisplay.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
        }

        public void ShowEdges(string myEdges)
        {
            myDisplayEdgesBool = true;
            myDisplayEdges = new SchemaBuilderDisplayEdges();
            List<List<Guid>> tupGUIDs = JsonConvert.DeserializeObject<List<List<Guid>>>(myEdges);

            //RhinoApp.WriteLine(myListGUIDs[0].ToString());
            myDisplayEdges.Edges = tupGUIDs;
            myDisplayEdges.Enabled = true;
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void HideEdges()
        {
            if (myDisplayEdgesBool == null)
            {
            }
            else
            {
                myDisplayEdges.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
        }

        public void CheckIfSomethingIsSelected()
        {
            Rhino.RhinoApp.WriteLine("izMe");
            var oes = new ObjectEnumeratorSettings
            {
                SelectedObjectsFilter = true,
                IncludeLights = false,
                IncludeGrips = false,
                IncludePhantoms = false
            };

            var objs = RhinoDoc.ActiveDoc.Objects.GetObjectList(oes).ToArray();
            int numObjSel = objs.Length;
            if (numObjSel == 0)
            {
                //Rhino.RhinoApp.WriteLine("no object is selected");
                var script = string.Format("window.bus.$emit('{0}')", "no-object-selected");
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
            else if (numObjSel == 1) {
                //Rhino.RhinoApp.WriteLine("one object is selected");
                var script = string.Format("window.bus.$emit('{0}')", "one-object-selected");
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
            else
            {
                //Rhino.RhinoApp.WriteLine("more than one object are selected");
                var script = string.Format("window.bus.$emit('{0}')", "one-object-selected");
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
            //Rhino.RhinoApp.WriteLine(objs.Length.ToString());
        }





        public void OnClickProperties()
        {
            RhinoApp.WriteLine("Hi there.");
            //Check if object is selected
            //if it is, add that object
            //if not, run a getter
          
            PropertyObject();
        }

        //public void GetGroupList(){}

        public void PropertyObject()
        {
            var oes = new ObjectEnumeratorSettings
            {
                SelectedObjectsFilter = true,
                IncludeLights = false,
                IncludeGrips = false,
                IncludePhantoms = false
            };

            var objs = RhinoDoc.ActiveDoc.Objects.GetObjectList(oes).ToArray();

            if (objs != null && objs.Count() > 0)
            {
                //have one or more objects
                if (objs.Count() == 1)
                {
                    var obj = objs[0];
                    var propertyInfos = obj.GetType().GetProperties();
                    var propertyDict = new Dictionary<string, Object>();
                    //propertyDict.Add("Guid", obj.Id.ToString());
                    foreach (var pi in propertyInfos)
                    {
                        object objVal;
                        if (pi.PropertyType == typeof(RhinoDoc))
                        {
                            objVal = RhinoDoc.ActiveDoc.Name;
                            if (string.IsNullOrEmpty(objVal.ToString()))
                                objVal = "Untitled";
                            propertyDict.Add(pi.Name, objVal);
                            //RhinoApp.WriteLine("SchemaBuilder: " + pi.Name + " : " + objVal);
                        }
                        else if (pi.PropertyType == typeof(Rhino.Geometry.GeometryBase) || pi.PropertyType.BaseType == typeof(Rhino.Geometry.GeometryBase) || pi.PropertyType.BaseType.BaseType == typeof(Rhino.Geometry.GeometryBase))
                        {
                        }
                        else if( pi.Name == "Id") {
                        }
                        else if (pi.PropertyType == typeof(ObjectAttributes))
                        {
                            var objAttr = obj.Attributes.GetType().GetProperties();
                            foreach (var att in objAttr)
                            {
                                object objAtt = att.GetValue(obj.Attributes, null);
                                //RhinoApp.WriteLine("SchemaBuilder: " + "    " + att.Name + " : " + objAtt);
                                if (propertyDict.ContainsKey(att.Name) || objAtt == null || string.IsNullOrEmpty(objAtt.ToString())) { }
                                else
                                {
                                    propertyDict.Add(att.Name, objAtt);
                                    //RhinoApp.WriteLine("SchemaBuilder: " + att.Name + " : " + objAtt);
                                }
                            }
                        }
                        else
                        {
                            objVal = pi.GetValue(obj, null);
                            if (propertyDict.ContainsKey(pi.Name) || objVal == null || string.IsNullOrEmpty(objVal.ToString()) ){ }
                            else
                            {
                                propertyDict.Add(pi.Name, objVal);
                                //RhinoApp.WriteLine("SchemaBuilder: " + pi.Name + " : " + objVal);
                            }
                        }

                    }

                    //propertyDict.Add("attributes", obj.Attributes);

                    var script = string.Format("window.bus.$emit('{0}', '{1}')", "get-properties", JsonConvert.SerializeObject(propertyDict));
                    Browser.GetMainFrame().EvaluateScriptAsync(script);

                }
                else
                {
                    RhinoApp.WriteLine("SchemaBuilder: Too many objects selected. Select only one Object and try again.");
                    return;
                }
            }
            else
            {
                RhinoApp.WriteLine("SchemaBuilder: No object is selected. Select one Object and try again.");
            }
        }


    }

    public static class MyGlobals
    {
        public static string myGlobalGUID { get; set; }
    }


}

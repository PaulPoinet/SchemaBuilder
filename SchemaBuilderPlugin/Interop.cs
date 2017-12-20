using CefSharp.WinForms;
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

        public Interop(ChromiumWebBrowser browser)
        {
            Browser = browser;

        }

        public void ShowDev()
        {
            Browser.ShowDevTools();
        }

        public string myGlobalGUID = null;

        public Rhino.Geometry.BoundingBox ShowObject(string myGUID)
        {
            //RhinoApp.WriteLine(myGUID);
            Guid myGuid = new Guid(myGUID);
            RhinoObject foundObject =  Rhino.RhinoDoc.ActiveDoc.Objects.Find(myGuid);
            Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
            myGlobalGUID = myGUID;
            return bbox;
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

    public class MyConduit : Rhino.Display.DisplayConduit
    {
        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void PreDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            e.Display.DrawBoxCorners(Interop.ShowObject(Interop.myGlobalGUID), System.Drawing.Color.AliceBlue, 3);
        }
    }
}

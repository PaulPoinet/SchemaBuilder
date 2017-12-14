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

        private static ChromiumWebBrowser Browser {get; set;}

        public Interop(ChromiumWebBrowser browser)
        {
            Browser = browser;
            
        }

        public void OnClickProperties()
        {
            RhinoApp.WriteLine("Hi there.");
            //Check if object is selected
            //if it is, add that object
            //if not, run a getter
            Browser.ShowDevTools();
            PropertyObject();
        }

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
                    //good, lets do something
                    //get the object properties
                    var obj = objs[0];
                    var propertyInfos = obj.GetType().GetProperties();
                    var propertyDict = new Dictionary<string, string>();
                    propertyDict.Add("Giud", obj.Id.ToString());
                    foreach (var pi in propertyInfos)
                    {
                        RhinoApp.WriteLine("SchemaBuilder: " + pi.Name + " " + pi.PropertyType.Name);
                        propertyDict.Add(pi.Name, pi.PropertyType.Name);
                    }

                    var script = string.Format("window.bus.$emit('{0}', '{1}')", "get-properties", JsonConvert.SerializeObject(propertyDict));
                    Browser.GetMainFrame().EvaluateScriptAsync(script);

                }
                else
                {
                    RhinoApp.WriteLine("SchemaBuilder: Too many objects selected. Select one Object and try again.");
                    return;
                }
            } else
            {
                //use getter for object
            }
        }
    }
}

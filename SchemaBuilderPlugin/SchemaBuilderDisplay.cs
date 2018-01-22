using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Display;

namespace SchemaBuilder
{
    public class SchemaBuilderDisplay : Rhino.Display.DisplayConduit
    {
        public List<Guid> Ids = new List<Guid>();

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void PreDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            for (int i = 0; i < Ids.Count; i++) {
                RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Ids[i]);
                Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
                e.Display.DrawBoxCorners(bbox, System.Drawing.Color.DarkGray, 3, 4);
                //e.Display.DrawDot(bbox.Center, Ids[i].ToString());
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
    }
    public class displayTest : Rhino.Display.DisplayConduit
    {
        public List<Guid> Ids = new List<Guid>();

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void PreDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            for (int i = 0; i < Ids.Count; i++)
            {
                RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Ids[i]);
                Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
                e.Display.DrawBoxCorners(bbox, System.Drawing.Color.DarkGray, 3, 4);
                //e.Display.DrawDot(bbox.Center, Ids[i].ToString());
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
    }

    public class SchemaBuilderDisplayEdges : Rhino.Display.DisplayConduit
    {
        public List<List<Guid>> Edges = new List<List<Guid>>();

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void DrawForeground(DrawEventArgs e)
        {
            base.DrawForeground(e);
            if (Edges.Count > 0)
            {
                System.Drawing.Color colorKid = System.Drawing.Color.LightCoral;
                foreach (List<Guid> li in Edges)

                {
                    RhinoObject foundObject0 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[0]);
                    RhinoObject foundObject1 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[1]);
                    Rhino.Geometry.Point3d ce0 = foundObject0.Geometry.GetBoundingBox(true).Center;
                    Rhino.Geometry.Point3d ce1 = foundObject1.Geometry.GetBoundingBox(true).Center;
                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;
                    e.Display.DrawLineArrow(graphEdge, System.Drawing.Color.Black, 5, 5);
                }

            }

            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
        
    }

    public class SchemaBuilderDisplayGraphOnHover : Rhino.Display.DisplayConduit
    {
        public List<List<Guid>> Edges = new List<List<Guid>>();
        public Guid Id { get; set; }


        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }

        protected override void PreDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);

            RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Id);
            Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
            List<Rhino.Geometry.Point3d> bboxCorners = bbox.GetCorners().ToList();
            List<Rhino.Geometry.Line> bboxEdges = bbox.GetEdges().ToList();
            e.Display.DrawBoxCorners(bbox, System.Drawing.Color.Red, 3, 4);


            foreach (Rhino.Geometry.Line ln in bboxEdges)
            {
                e.Display.DrawDottedLine(ln, System.Drawing.Color.Red);
            }

            List<Rhino.Geometry.Line> myEdges = new List<Rhino.Geometry.Line>();

            if (Edges.Count > 0)
            {
                System.Drawing.Color colorKid = System.Drawing.Color.LightCoral;
                foreach (List<Guid> li in Edges)
                {
                    RhinoObject foundObject0 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[0]);
                    RhinoObject foundObject1 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[1]);
                    Rhino.Geometry.Point3d ce0 = foundObject0.Geometry.GetBoundingBox(true).Center;
                    Rhino.Geometry.BoundingBox bboxKid = foundObject1.Geometry.GetBoundingBox(true);
                    List<Rhino.Geometry.Point3d> bboxCornersKid = bboxKid.GetCorners().ToList();
                    List<Rhino.Geometry.Line> bboxEdgesKid = bboxKid.GetEdges().ToList();
                    e.Display.DrawBoxCorners(bboxKid, colorKid, 3, 4);

                    foreach (Rhino.Geometry.Line ln in bboxEdgesKid)
                    {
                        e.Display.DrawDottedLine(ln, colorKid);
                    }

                    Rhino.Geometry.Point3d ce1 = bboxKid.Center;

                    
                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;

                    //e.Display.DrawLineArrow(graphEdge, System.Drawing.Color.Red, 7, 10);
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
        protected override void DrawForeground(DrawEventArgs e)
        {
            base.DrawForeground(e);
            //e.Display.DrawDot(bbox.Center, Id.ToString());
            if (Edges.Count > 0)
            {
                System.Drawing.Color colorKid = System.Drawing.Color.LightCoral;
                foreach (List<Guid> li in Edges)
                {
                    RhinoObject foundObject0 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[0]);
                    RhinoObject foundObject1 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[1]);
                    Rhino.Geometry.Point3d ce0 = foundObject0.Geometry.GetBoundingBox(true).Center;
                    Rhino.Geometry.Point3d ce1 = foundObject1.Geometry.GetBoundingBox(true).Center;
                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;

                    e.Display.DrawLineArrow(graphEdge, System.Drawing.Color.Red, 5, 8);
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

    }
    public class SchemaBuilderDisplayOnHover : Rhino.Display.DisplayConduit
    {
        //public Guid Id = new Guid();
        public Guid Id { get; set; }

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void PreDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            

            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
    }

    public class ObjectsInTheTree : Rhino.Display.DisplayConduit
    {
        public List<Guid> Ids = new List<Guid>();

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void DrawForeground(Rhino.Display.DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            for (int i = 0; i < Ids.Count; i++)
            {
                RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Ids[i]);
                Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
                //e.Display.DrawBoxCorners(bbox, System.Drawing.Color.DarkGray, 3, 4);
                e.Display.DrawDot(bbox.Center, i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                //e.Display.DrawDot()
                //e.Display.DrawDot()
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
    }
}

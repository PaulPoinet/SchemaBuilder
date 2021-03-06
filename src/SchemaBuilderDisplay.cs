﻿using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Rhino.Geometry;
using Rhino.Display;
using Rhino;

namespace SchemaBuilder
{
    
    public class SchemaBuilderDisplay : Rhino.Display.DisplayConduit
    {
        public List<Guid> Ids = new List<Guid>();
        public System.Drawing.Color myCol = System.Drawing.Color.Red;
        //public List<Point3d> locations = new List<Point3d>();




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
                bbox.Inflate(2);

                e.Display.EnableDepthWriting(false);
                //e.Display.DrawBoxCorners(bbox, System.Drawing.Color.DarkGray, 3, 2);
                e.Display.EnableDepthWriting(true);
                switch (foundObject.ObjectType)
                {
                    case Rhino.DocObjects.ObjectType.Point:
                        e.Display.DrawPoint(((Rhino.Geometry.Point)foundObject.Geometry).Location, PointStyle.X, 2, myCol);
                        break;

                    case Rhino.DocObjects.ObjectType.Curve:
                        e.Display.DrawCurve((Rhino.Geometry.Curve)foundObject.Geometry, myCol, 3);
                        break;

                    case Rhino.DocObjects.ObjectType.Extrusion:
                        DisplayMaterial eMaterial = new DisplayMaterial(myCol, 0.5);
                        e.Display.DrawBrepShaded(((Rhino.Geometry.Extrusion)foundObject.Geometry).ToBrep(), eMaterial);
                        break;

                    case Rhino.DocObjects.ObjectType.Brep:
                        DisplayMaterial bMaterial = new DisplayMaterial(myCol, 0.5);
                        e.Display.DrawBrepShaded((Brep)foundObject.Geometry, bMaterial);
                        break;

                    case Rhino.DocObjects.ObjectType.Mesh:
                        var mesh = foundObject.Geometry as Rhino.Geometry.Mesh;
                        if (mesh.VertexColors.Count > 0)
                        {
                            for (int j = 0; j < mesh.VertexColors.Count; j++)
                                mesh.VertexColors[j] = Color.FromArgb(100, mesh.VertexColors[j]);
                            e.Display.DrawMeshFalseColors(mesh);
                        }
                        else
                        {
                            DisplayMaterial mMaterial = new DisplayMaterial(myCol, 0.5);
                            e.Display.DrawMeshShaded(mesh, mMaterial);
                        }
                        //e.Display.DrawMeshWires((Mesh)obj, Color.DarkGray);
                        break;

                    case Rhino.DocObjects.ObjectType.TextDot:
                        //e.Display.Draw3dText( ((TextDot)obj).Text, Colors[count], new Plane(((TextDot)obj).Point));
                        var textDot = (TextDot)foundObject.Geometry;
                        e.Display.DrawDot(textDot.Point, textDot.Text, myCol, Color.White);

                        break;

                    case Rhino.DocObjects.ObjectType.Annotation:

                        var textObj = (Rhino.Geometry.TextEntity)foundObject.Geometry;
                        e.Display.Draw3dText(textObj.Text, Color.Black, textObj.Plane, textObj.TextHeight, Rhino.RhinoDoc.ActiveDoc.Fonts[textObj.FontIndex].FaceName);
                        break;
                }


            }
            
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
        protected override void DrawOverlay(Rhino.Display.DrawEventArgs e)
        {
            base.DrawOverlay(e);

            List<Guid> noDupGUIDs = new List<Guid>();
            

            for (int i = 0; i < Ids.Count; i++)
            {
                if (noDupGUIDs.Contains(Ids[i])){}
                else
                {
                    noDupGUIDs.Add(Ids[i]);
                    RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Ids[i]);
                    int isSelected = foundObject.IsSelected(false);

                    switch (foundObject.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            if (isSelected > 0)
                                e.Display.DrawDot(((Rhino.Geometry.Point)foundObject.Geometry).Location, i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                            else
                                e.Display.DrawDot(((Rhino.Geometry.Point)foundObject.Geometry).Location, i.ToString(), System.Drawing.Color.Black, System.Drawing.Color.White);
                            break;
                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            if (isSelected > 0)
                                e.Display.DrawDot(myCurve.PointAtNormalizedLength(0.5), i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                            else
                                e.Display.DrawDot(myCurve.PointAtNormalizedLength(0.5), i.ToString(), System.Drawing.Color.Black, System.Drawing.Color.White);
                            break;
                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject.Geometry;
                            Rhino.Geometry.Point3d myExtruLocation = myExtru.GetBoundingBox(true).Center;
                            //Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            if (isSelected > 0)
                                e.Display.DrawDot(myExtruLocation, i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                            else
                                e.Display.DrawDot(myExtruLocation, i.ToString(), System.Drawing.Color.Black, System.Drawing.Color.White);
                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Brep myBrep = (Rhino.Geometry.Brep)foundObject.Geometry;
                            Rhino.Geometry.Point3d myBrepLocation = myBrep.GetBoundingBox(true).Center;
                            if (isSelected > 0)
                                e.Display.DrawDot(myBrepLocation, i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                            else
                                e.Display.DrawDot(myBrepLocation, i.ToString(), System.Drawing.Color.Black, System.Drawing.Color.White);
                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshLocation = mesh.GetBoundingBox(true).Center;
                            if (isSelected > 0)
                                e.Display.DrawDot(myMeshLocation, i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);
                            else
                                e.Display.DrawDot(myMeshLocation, i.ToString(), System.Drawing.Color.Black, System.Drawing.Color.White);
                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            var textDot = (TextDot)foundObject.Geometry;
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            var textObj = (Rhino.Geometry.TextEntity)foundObject.Geometry;
                            break;
                    }
                }
                
                
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
                    Rhino.Geometry.Point3d ce0 = new Rhino.Geometry.Point3d();
                    Rhino.Geometry.Point3d ce1 = new Rhino.Geometry.Point3d();

                    switch (foundObject0.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            ce0 = ((Rhino.Geometry.Point)foundObject0.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject0.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce0 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject0.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce0 = myExtruCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject0.Geometry).GetBoundingBox(true).Center;
                            ce0 = myBrepCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject0.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce0 = myMeshCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }

                    switch (foundObject1.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            ce1 = ((Rhino.Geometry.Point)foundObject1.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject1.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce1 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject1.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce1 = myExtruCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject1.Geometry).GetBoundingBox(true).Center;
                            ce1 = myBrepCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject1.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce1 = myMeshCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }


                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;
                    e.Display.DrawLineArrow(graphEdge, System.Drawing.Color.Black, 5, 4);
                }

            }

            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
        
    }

    public class SchemaBuilderDisplaySiblingEdges : Rhino.Display.DisplayConduit
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
                    Rhino.Geometry.Point3d ce0 = new Rhino.Geometry.Point3d();
                    Rhino.Geometry.Point3d ce1 = new Rhino.Geometry.Point3d();

                    switch (foundObject0.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            ce0 = ((Rhino.Geometry.Point)foundObject0.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject0.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce0 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject0.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce0 = myExtruCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject0.Geometry).GetBoundingBox(true).Center;
                            ce0 = myBrepCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject0.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce0 = myMeshCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }

                    switch (foundObject1.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            ce1 = ((Rhino.Geometry.Point)foundObject1.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject1.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce1 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject1.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce1 = myExtruCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject1.Geometry).GetBoundingBox(true).Center;
                            ce1 = myBrepCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject1.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce1 = myMeshCentroid;
                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }


                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;
                    e.Display.DrawLine(graphEdge, Color.Orange, 4);
                }

            }

            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }

    }
    public class SchemaBuilderDisplayGraphOnHover : Rhino.Display.DisplayConduit
    {
        public List<List<Guid>> Edges = new List<List<Guid>>();
        public Guid Id { get; set; }
        System.Drawing.Color myCol = System.Drawing.Color.DarkRed;


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
            bbox.Inflate(2);
            e.Display.EnableDepthWriting(false);
            List<Rhino.Geometry.Point3d> bboxCorners = bbox.GetCorners().ToList();
            List<Rhino.Geometry.Line> bboxEdges = bbox.GetEdges().ToList();
            //e.Display.DrawBoxCorners(bbox, System.Drawing.Color.Red, 3, 4);

            
            Rhino.Display.RhinoView myViewport = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            Rhino.Display.RhinoViewport viewport = myViewport.ActiveViewport;
            
            e.Display.EnableDepthWriting(true);
            e.Display.EnableDepthWriting(false);
            switch (foundObject.ObjectType)
            {
                case Rhino.DocObjects.ObjectType.Point:
                    e.Display.DrawPoint(((Rhino.Geometry.Point)foundObject.Geometry).Location, PointStyle.X, 2, myCol);
                    break;

                case Rhino.DocObjects.ObjectType.Curve:
                    e.Display.DrawCurve((Rhino.Geometry.Curve)foundObject.Geometry, myCol, 4);
                    break;

                case Rhino.DocObjects.ObjectType.Extrusion:
                    DisplayMaterial eMaterial = new DisplayMaterial(myCol, 0.5);
                    e.Display.DrawBrepShaded(((Rhino.Geometry.Extrusion)foundObject.Geometry).ToBrep(), eMaterial);
                    break;
                    
                case Rhino.DocObjects.ObjectType.Brep:
                    DisplayMaterial bMaterial = new DisplayMaterial(myCol, 0.5);
                    e.Display.DrawBrepShaded((Brep)foundObject.Geometry, bMaterial);

                    Mesh[] meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)foundObject.Geometry);
                    Mesh globalMesh = new Rhino.Geometry.Mesh();
                    foreach(Mesh m in meshes)
                        globalMesh.Append(m);
                    Polyline[] myPolys2 = globalMesh.GetOutlines(viewport);
                    foreach (Polyline poly in myPolys2)
                        e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Black, 6);
                    break;

                case Rhino.DocObjects.ObjectType.Mesh:
                    var mesh = foundObject.Geometry as Rhino.Geometry.Mesh;
                    if (mesh.VertexColors.Count > 0)
                    {
                        for (int i = 0; i < mesh.VertexColors.Count; i++)
                            mesh.VertexColors[i] = Color.FromArgb(100, mesh.VertexColors[i]);

                        e.Display.DrawMeshFalseColors(mesh);
                    }
                    else
                    {
                        DisplayMaterial mMaterial = new DisplayMaterial(myCol, 0.5);
                        e.Display.DrawMeshShaded(mesh, mMaterial);
                    }
                    //e.Display.DrawMeshWires((Mesh)obj, Color.DarkGray);
                    break;

                case Rhino.DocObjects.ObjectType.TextDot:
                    //e.Display.Draw3dText( ((TextDot)obj).Text, Colors[count], new Plane(((TextDot)obj).Point));
                    var textDot = (TextDot)foundObject.Geometry;
                    e.Display.DrawDot(textDot.Point, textDot.Text, myCol, Color.White);

                    break;

                case Rhino.DocObjects.ObjectType.Annotation:

                    var textObj = (Rhino.Geometry.TextEntity)foundObject.Geometry;
                    e.Display.Draw3dText(textObj.Text, Color.Black, textObj.Plane, textObj.TextHeight, Rhino.RhinoDoc.ActiveDoc.Fonts[textObj.FontIndex].FaceName);
                    break;
            }
            e.Display.EnableDepthWriting(true);





            List<Rhino.Geometry.Line> myEdges = new List<Rhino.Geometry.Line>();

            if (Edges.Count > 0)
            {
                System.Drawing.Color colorKid = System.Drawing.Color.LightCoral;
                foreach (List<Guid> li in Edges)
                {
                    RhinoObject foundObject0 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[0]);
                    RhinoObject foundObject1 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[1]);


                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
        protected override void DrawForeground(DrawEventArgs e)
        {
            base.DrawForeground(e);



            RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Id);
            Rhino.Display.RhinoView myViewport = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            Rhino.Display.RhinoViewport viewport = myViewport.ActiveViewport;


            e.Display.EnableDepthWriting(false);
            switch (foundObject.ObjectType)
            {

                case Rhino.DocObjects.ObjectType.Point:
                    break;

                case Rhino.DocObjects.ObjectType.Curve:
                    break;

                case Rhino.DocObjects.ObjectType.Extrusion:
                    Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject.Geometry;
                    Rhino.Geometry.Brep myBrep = myExtru.ToBrep();
                    Mesh[] extruMeshes = Rhino.Geometry.Mesh.CreateFromBrep(myBrep);
                    Mesh globalExtruMesh = new Rhino.Geometry.Mesh();
                    foreach (Mesh m in extruMeshes)
                        globalExtruMesh.Append(m);
                    Polyline[] myExtruPoly = globalExtruMesh.GetOutlines(viewport);
                    foreach (Polyline poly in myExtruPoly)
                        e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);
                    break;

                case Rhino.DocObjects.ObjectType.Brep:
                    Mesh[] meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)foundObject.Geometry);
                    Mesh globalMesh = new Rhino.Geometry.Mesh();
                    foreach (Mesh m in meshes)
                        globalMesh.Append(m);
                    Polyline[] myPolys2 = globalMesh.GetOutlines(viewport);
                    foreach (Polyline poly in myPolys2)
                        e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);
                    break;

                case Rhino.DocObjects.ObjectType.Mesh:
                    Mesh mesh = foundObject.Geometry as Rhino.Geometry.Mesh;
                    Polyline[] meshOutline = mesh.GetOutlines(viewport);
                    foreach (Polyline poly in meshOutline)
                        e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);
                    break;

                case Rhino.DocObjects.ObjectType.TextDot:
                    break;

                case Rhino.DocObjects.ObjectType.Annotation:
                    break;
            }
            e.Display.EnableDepthWriting(true);
            e.Display.EnableDepthWriting(false);

            if (Edges.Count > 0)
            {
                System.Drawing.Color colorKid = System.Drawing.Color.LightCoral;
                foreach (List<Guid> li in Edges)
                {
                    RhinoObject foundObject0 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[0]);
                    RhinoObject foundObject1 = Rhino.RhinoDoc.ActiveDoc.Objects.Find(li[1]);
                    Rhino.Geometry.Point3d ce0 = new Rhino.Geometry.Point3d();
                    Rhino.Geometry.Point3d ce1 = new Rhino.Geometry.Point3d();

                    switch (foundObject0.ObjectType)
                    {
                        case Rhino.DocObjects.ObjectType.Point:
                            ce0 = ((Rhino.Geometry.Point)foundObject0.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject0.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce0 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject0.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce0 = myExtruCentroid;

                            Rhino.Geometry.Brep myBrep = myExtru.ToBrep();
                            Mesh[] extruMeshes = Rhino.Geometry.Mesh.CreateFromBrep(myBrep);
                            Mesh globalExtruMesh = new Rhino.Geometry.Mesh();
                            foreach (Mesh m in extruMeshes)
                                globalExtruMesh.Append(m);
                            Polyline[] myExtruPoly = globalExtruMesh.GetOutlines(viewport);
                            foreach (Polyline poly in myExtruPoly)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);

                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject0.Geometry).GetBoundingBox(true).Center;
                            ce0 = myBrepCentroid;

                            Mesh[] meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)foundObject0.Geometry);
                            Mesh globalMesh = new Rhino.Geometry.Mesh();
                            foreach (Mesh m in meshes)
                                globalMesh.Append(m);
                            Polyline[] myPolys2 = globalMesh.GetOutlines(viewport);
                            foreach (Polyline poly in myPolys2)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);

                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject0.Geometry as Rhino.Geometry.Mesh;

                            Polyline[] meshOutline = mesh.GetOutlines(viewport);
                            foreach (Polyline poly in meshOutline)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);

                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce0 = myMeshCentroid;




                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }

                    switch (foundObject1.ObjectType)
                    {



                        case Rhino.DocObjects.ObjectType.Point:
                            ce1 = ((Rhino.Geometry.Point)foundObject1.Geometry).Location;
                            break;

                        case Rhino.DocObjects.ObjectType.Curve:
                            Rhino.Geometry.Curve myCurve = (Rhino.Geometry.Curve)foundObject1.Geometry;
                            myCurve.Domain = new Rhino.Geometry.Interval(0, 1);
                            ce1 = myCurve.PointAtNormalizedLength(0.5);
                            break;

                        case Rhino.DocObjects.ObjectType.Extrusion:
                            Rhino.Geometry.Extrusion myExtru = (Rhino.Geometry.Extrusion)foundObject1.Geometry;
                            Rhino.Geometry.Point3d myExtruCentroid = Rhino.Geometry.AreaMassProperties.Compute(myExtru.ToBrep()).Centroid;
                            ce1 = myExtruCentroid;


                            Rhino.Geometry.Brep myBrep = myExtru.ToBrep();
                            Mesh[] extruMeshes = Rhino.Geometry.Mesh.CreateFromBrep(myBrep);
                            Mesh globalExtruMesh = new Rhino.Geometry.Mesh();
                            foreach (Mesh m in extruMeshes)
                                globalExtruMesh.Append(m);
                            Polyline[] myExtruPoly = globalExtruMesh.GetOutlines(viewport);
                            foreach (Polyline poly in myExtruPoly)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);

                            break;

                        case Rhino.DocObjects.ObjectType.Brep:
                            Rhino.Geometry.Point3d myBrepCentroid = ((Brep)foundObject1.Geometry).GetBoundingBox(true).Center;
                            ce1 = myBrepCentroid;

                            Mesh[] meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)foundObject1.Geometry);
                            Mesh globalMesh = new Rhino.Geometry.Mesh();
                            foreach (Mesh m in meshes)
                                globalMesh.Append(m);
                            Polyline[] myPolys2 = globalMesh.GetOutlines(viewport);
                            foreach (Polyline poly in myPolys2)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);

                            break;

                        case Rhino.DocObjects.ObjectType.Mesh:
                            var mesh = foundObject1.Geometry as Rhino.Geometry.Mesh;
                            Rhino.Geometry.Point3d myMeshCentroid = mesh.GetBoundingBox(true).Center;
                            ce1 = myMeshCentroid;


                            Polyline[] meshOutline = mesh.GetOutlines(viewport);
                            foreach (Polyline poly in meshOutline)
                                e.Display.DrawCurve(poly.ToNurbsCurve(), Color.Red, 6);


                            break;

                        case Rhino.DocObjects.ObjectType.TextDot:
                            // todo
                            break;

                        case Rhino.DocObjects.ObjectType.Annotation:
                            // todo
                            break;
                    }


                    Rhino.Geometry.Line graphEdge = new Rhino.Geometry.Line();
                    graphEdge.From = ce0;
                    graphEdge.To = ce1;

                    e.Display.DrawLineArrow(graphEdge, System.Drawing.Color.DarkRed, 7, 4);
                }
            }
            e.Display.EnableDepthWriting(true);

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
        public List<Rhino.Geometry.Curve> FrustumCurves { get; set; }

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            //e.IncludeBoundingBox(new Rhino.Geometry.Point3d(0, 0, 0));
        }
        protected override void DrawOverlay(Rhino.Display.DrawEventArgs e)
        {
            base.DrawOverlay(e);
            Rhino.Display.RhinoView myViewport = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            Rhino.Display.RhinoViewport viewport = myViewport.ActiveViewport;

            for (int i = 0; i < Ids.Count; i++)
            {
                RhinoObject foundObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(Ids[i]);
                Rhino.Geometry.BoundingBox bbox = foundObject.Geometry.GetBoundingBox(true);
                Rhino.Geometry.Plane myFrustumPlane = new Rhino.Geometry.Plane();
                //viewport.GetFrustumFarPlane(out myFrustumPlane);
                //myFrustumPlane.Origin = bbox.Center;
                //Rhino.Geometry.Circle myFrustumCircle = new Rhino.Geometry.Circle();
                //myFrustumCircle.Plane = myFrustumPlane;
                //myFrustumCircle.Radius = bbox.Diagonal.Length / 2;
                //Rhino.Geometry.Curve myFrustumCurve = myFrustumCircle.ToNurbsCurve();

                //myFrustumCurve.Domain = new Rhino.Geometry.Interval(0.0,1.0);



                //e.Display.DrawDot(myFrustumCurve.PointAtNormalizedLength(0.4), i.ToString(), System.Drawing.Color.Red, System.Drawing.Color.White);

            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }
    }
}

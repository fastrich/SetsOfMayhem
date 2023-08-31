using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UILine : Graphic
{
    #region InspectorVariables
    [Header("General")]
    public List<Vector2> points = new();
    public float width = 5f;

    [Header("Rounding")]
    public bool roundCorners = true;
    public bool roundStart = false;
    public bool roundEnd = false;

    [Header("Dashed")]
    public bool dashed = false;
    public int dashLength = 10;
    public int dashSpacing = 5;
    #endregion InspectorVariables

    #region UnityMethods
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // clear old vertecies and triangles
        vh.Clear();

        // a line with less than 2 points does not make any sense => return
        if (points.Count < 2)
            return;

        if (dashed)
            GenerateLinesDashed(vh);
        else
            GenerateLinesStandard(vh);

        if (roundStart)
            CreateCircle(vh, points[0], width);

        if (roundEnd)
            CreateCircle(vh, points[^1], width);
    }
    #endregion UnityMethods

    #region Implementation

    #region GenerateLines
    private void GenerateLinesStandard(VertexHelper vh)
    {
        points.Zip(points.Skip(1), (a, b) => Tuple.Create(a, b))
            .ToList()
            .ForEach(tup => CreateSegment(vh, tup.Item1, tup.Item2));

        if (roundCorners)
        {
            // take all points except for start and end and apply rounding
            points.Take(points.Count - 1).Skip(1).ToList()
                .ForEach(p => CreateCircle(vh, p, width));
        }
    }

    private void GenerateLinesDashed(VertexHelper vh)
    {
        float restLen = dashLength;
        bool isDash = true;
        var lines = points.Zip(points.Skip(1), (a, b) => Tuple.Create(a, b)).ToList();
        for (int i = 0; i < lines.Count; i++)
        {
            var tup = lines[i];
            Vector2 start = tup.Item1;
            Vector2 end = tup.Item2;
            Vector2 dir = (end - start).normalized;

            // main spacing algorithm
            Vector2 current = start;
            while (Vector2.Distance(current, end) >= restLen)
            {
                Vector2 segmentEnd = current + restLen * dir;
                if (isDash)
                    CreateSegment(vh, current, segmentEnd);
                current = segmentEnd;
                isDash = !isDash;
                restLen = isDash ? dashLength : dashSpacing;
            }

            // is there a dash wrapping around a corner?
            bool dashOverCorner = false;

            
            float distLeft = Vector2.Distance(current, end);
            if (!isDash)
                restLen -= distLeft;
            // dont fill remaining distance with dash, if it would be short (shorter than width/2)
            else if (isDash && distLeft > width/2)
            {
                CreateSegment(vh, current, end);
                restLen -= distLeft;
                dashOverCorner = true;
            }

            // discard rest of dash if it is too short (shorter than width/2)
            if (isDash && restLen < width/2)
            {
                isDash = false;
                restLen = dashSpacing;
                dashOverCorner = false; // dash does not wrap around corner as dashSpacing will follow
            }

            // only create round corners if roundCorners are enabled and there is a dash wrapping around the corner and not last corner (aka end)
            if (roundCorners && dashOverCorner && i != lines.Count-1)
                CreateCircle(vh, end, width);
        }
    }
    #endregion GenerateLines

    #region CreateLine
    private void CreateSegment(VertexHelper vh, Vector2 start, Vector2 end)
    {
        //start += (Vector2)transform.position;
        //end += (Vector2)transform.position;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector2 dir = end - start;
        Vector2 perp = Vector2.Perpendicular(dir).normalized;
        Vector2 off = perp * width / 2;

        vertex.position = new Vector3(start.x + off.x, start.y + off.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(end.x + off.x, end.y + off.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(end.x - off.x, end.y - off.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(start.x - off.x, start.y - off.y);
        vh.AddVert(vertex);

        int offset = vh.currentVertCount - 4;
        vh.AddTriangle(0 + offset, 1 + offset, 2 + offset);
        vh.AddTriangle(2 + offset, 3 + offset, 0 + offset);
    }
    #endregion CreateLine

    #region Rounding
    private void CreateCircle(VertexHelper vh, Vector2 center, float diameter, int sideCount = 20)
    {
        //center += (Vector2)transform.position;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3[] vertices = GetCirclePoints(diameter/2, sideCount, center).Union(new Vector3[] { center }).ToArray();
        vertices.ToList().ForEach(vert => {
                vertex.position = vert;
                vh.AddVert(vertex);
            }
        );

        int startVert = vh.currentVertCount - vertices.Length;
        for (int i = 0; i < vertices.Length - 1; i++)
        {
            vh.AddTriangle(
                vh.currentVertCount - 1, // center
                startVert + i,
                startVert + ((i + 1) % (vertices.Length - 1))
            ); 
        }
        return;
    }

    protected static Vector3[] GetCirclePoints(float circleRadius, int pointCount, Vector3 offset)
    {
        Vector3[] circle = new Vector3[pointCount];
        float slice = (2f * Mathf.PI) / pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * slice;
            circle[i] = new Vector3(circleRadius * Mathf.Sin(angle), circleRadius * Mathf.Cos(angle)) + offset;
        }
        return circle;
    }

    #endregion Rounding

    #endregion Implementation
}

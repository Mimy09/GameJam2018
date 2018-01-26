﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour {
    public List<Vector2[]> points = new List<Vector2[]>();
    public List<Vector2> mesh = new List<Vector2>();
    public GameObject[] planets;
    public List<float> angles = new List<float>();
    Vector2 temp;
    MeshFilter filter;

    void Start () {
        planets = GameObject.FindGameObjectsWithTag("Planet");

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,50),
            new Vector2(50,50),
            new Vector2(50,100),
            new Vector2(0,100),
            new Vector2(0,150),
            new Vector2(150,150),
            new Vector2(150,100),
            new Vector2(100,100),
            new Vector2(100,50),
            new Vector2(150,50),
            new Vector2(150,0),
        };

        gameObject.AddComponent(typeof(MeshRenderer));
        filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
    }

    void Update () {
        mesh.Clear();
        points.Clear();
        angles.Clear();

        for (int i = 0; i < planets.Length; i++) {
            points.Add(planets[i].GetComponent<PolygonCollider2D>().points);
        }
        for (int i = 0; i < points.Count; i++) {
            for (int j = 0; j < points[i].Length; j++) {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, (points[i][j] * planets[i].transform.localScale.x) + (Vector2)planets[i].transform.position - (Vector2)this.transform.position, 10.0f);
                Debug.DrawLine((Vector2)this.transform.position, hit.point,Color.cyan);
                mesh.Add(hit.point);
            }
        }

        for (int i = 0; i < mesh.Count; i++) {
            for (int j = 0; j < (mesh.Count - 1); j++) {
                float ang = Vector2.Angle(transform.position, mesh[j]);
                float ang_2 = Vector2.Angle(transform.position, mesh[j + 1]);
                Vector3 cross = Vector3.Cross(transform.position, mesh[j]);
                if (cross.z > 0) { ang = 360 - ang; }
                cross = Vector3.Cross(transform.position, mesh[j + 1]);
                if (cross.z > 0) { ang_2 = 360 - ang_2; }
                
                if (ang > ang_2) {
                    temp = mesh[j];
                    mesh[j] = mesh[j + 1];
                    mesh[j + 1] = temp;
                }
            }
        }

        for (int i = 0; i < mesh.Count; i++) {
            float ang = Vector2.Angle(transform.position, mesh[i]);
            Vector3 cross = Vector3.Cross(transform.position, mesh[i]);
            if (cross.z > 0) { ang = 360 - ang; }

            angles.Add(ang);
        }

        Vector2[] vec_mesh = new Vector2[mesh.Count];
        for (int i = 0; i < mesh.Count; i++) {
            vec_mesh[i] = mesh[i];
        }

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vec_mesh);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vec_mesh.Length];
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = new Vector3(vec_mesh[i].x, vec_mesh[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        filter.mesh = msh;

    }
}

public class Triangulator {
    private List<Vector2> m_points = new List<Vector2>();

    public Triangulator (Vector2[] points) {
        m_points = new List<Vector2>(points);
    }

    public int[] Triangulate () {
        List<int> indices = new List<int>();

        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area() > 0) {
            for (int v = 0; v < n; v++)
                V[v] = v;
        } else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;) {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(u, v, w, nv, V)) {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private float Area () {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private bool Snip (int u, int v, int w, int n, int[] V) {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++) {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}
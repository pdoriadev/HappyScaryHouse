using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;
    // generate shadowcaster gameobject at every tile that's at edge of tilemap collider.
    // shadow should fall onto layers below it. i.e. Boundary will fall onto ground. 

    // Remove previously generated shadowcaster gameobjects.
        // save instances into collection? 
[ExecuteInEditMode]
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class TilemapShadows : MonoBehaviour
{
    [SerializeField]
    private Object ShadowCastersPrefab = default;
    [SerializeField]
    private List<GameObject> ShadowCasters = new List<GameObject>();
    public List<GameObject> shadowCasters => ShadowCasters;
    private Tilemap Tilemap = null;
    public Tilemap tilemap => Tilemap;
    private TilemapRenderer TilemapRenderer = null;
    public TilemapRenderer tilemapRenderer => TilemapRenderer;

    private List<Vector3> filledTiles = new List<Vector3>();
    public void GenerateShadowCasters()
    {
        if (Tilemap == null)
        {
            Tilemap = GetComponent<Tilemap>();
        }
        if (TilemapRenderer == null)
        {
            TilemapRenderer = GetComponent<TilemapRenderer>();
        }

        DestroyPreviouslyGenerated();

        filledTiles.Clear();
        filledTiles.TrimExcess();
        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (Tilemap.HasTile(localPlace))
            {
                Vector3 place = Tilemap.GetCellCenterWorld(localPlace);
                filledTiles.Add(place);
            }
        }
        Debug.Log("number of tiles in tilemap: " + filledTiles.Count);

        int n = 0;
        for (int i = 0; i < filledTiles.Count; i++ )
        {
            if (IsEdgeTile((filledTiles[i])))
            {
                n++;
                GameObject casterGO = (GameObject)PrefabUtility.InstantiatePrefab(ShadowCastersPrefab);
                casterGO.transform.localScale = Vector3.Scale(casterGO.transform.localScale, Tilemap.cellSize);
                casterGO.transform.position = filledTiles[i];
                casterGO.transform.SetParent(gameObject.transform);

                ShadowCaster2D caster = casterGO.GetComponent<ShadowCaster2D>();
                SpriteRenderer casterSR = casterGO.GetComponent<SpriteRenderer>();
                casterSR.sortingLayerID = TilemapRenderer.sortingLayerID;
                
                // Add all sorting layers below the caster's to its target layers.
                List<int> targetLayers = new List<int>();
                bool isAbove = true;
                for (int j = SortingLayer.layers.Length - 1; j >= 0; j--)
                {
                    if (isAbove)
                    {
                        if (casterSR.sortingLayerID == SortingLayer.layers[j].id)
                        {
                            isAbove = false;
                        }
                    }
                    else
                    {
                        targetLayers.Add(j);
                    }
                }

                ShadowCasters.Add(casterGO);
                NameShadowCaster(ref casterGO, casterSR.sortingLayerName);

            }
        }
        Debug.Log(n + " shadow casters generated");
    }
    private void DestroyPreviouslyGenerated()
    {
        ShadowCasters.TrimExcess();
        GameObject casterGO = null;
        int n = 0;
        for (int i = ShadowCasters.Count - 1; i >= 0; i--)
        {
            n++;
            casterGO = ShadowCasters[i];
            ShadowCasters.RemoveAt(i);
            DestroyImmediate(casterGO);
        }
        // ShadowCasters.Clear();
        ShadowCasters.TrimExcess();

        Debug.Log(n + " shadowcaster gameobjects destroyed");
    }
    private void NameShadowCaster(ref GameObject casterGO, string sortingLayerName)
    {
        casterGO.name = "CasterGO_" + sortingLayerName + "_" 
            + ShadowCasters.IndexOf(casterGO) + "_" + (casterGO.transform.position);
    }
    private bool IsEdgeTile(Vector3 tilePos)
    {
        // Debug.Log("Is edge tile check");
        Vector3 V2diag = (Vector3.Scale(new Vector3(1, 1, 0), Tilemap.cellSize));
        Vector3 V2up = (Vector3.Scale(new Vector3(0, 1, 0), Tilemap.cellSize));
        Vector3 V2right = (Vector3.Scale(new Vector3(1, 0, 0), Tilemap.cellSize));

        if (
            !filledTiles.Contains(V2right + tilePos) ||
            !filledTiles.Contains(-V2right + tilePos) ||
            !filledTiles.Contains(V2up + tilePos) ||
            !filledTiles.Contains(-V2up + tilePos) ||
            !filledTiles.Contains(V2diag + tilePos) ||
            !filledTiles.Contains(-V2diag + tilePos) ||
            !filledTiles.Contains(Vector3.Scale(V2diag,-V2up) + tilePos) ||
            !filledTiles.Contains(Vector3.Scale(V2diag, -V2right) + tilePos)
            )
        {
            return true;
        }
         return false;
    }
    private Vector3Int ConvertV3toV3Int(Vector3 vector3)
    {
        return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
    }

}

[CustomEditor(typeof(TilemapShadows))]
public class TilemapShadowsEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) 
        {
            TilemapShadows generator = (TilemapShadows)target;

            generator.GenerateShadowCasters();
            Object [] casters = generator.shadowCasters.ToArray();
            Undo.RecordObjects(casters, "GridShadowCastersGenerator name prefab instances");

            // set each caster's targetLayer
            for (int i=0; i < generator.shadowCasters.Count; i++)
            {
                
            }
        }
    }
}
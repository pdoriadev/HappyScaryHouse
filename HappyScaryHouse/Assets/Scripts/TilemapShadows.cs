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
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class TilemapShadows : MonoBehaviour
{
    [SerializeField]
    private GameObject ShadowCastersPrefab = default;
    private List<GameObject> ShadowCasters = new List<GameObject>();
    public List<GameObject> shadowCasters => ShadowCasters;
    private Tilemap Tilemap = null;
    private TilemapRenderer TilemapRenderer = null;

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

        if (ShadowCasters.Count != 0)
        {
            RemovePreviouslyGenerated();
        }

        // Generate
        List<Vector3> filledTiles = new List<Vector3>();

        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (Tilemap.HasTile(localPlace))
            {
                Vector3 place = Tilemap.CellToWorld(localPlace);
                filledTiles.Add(place);
            }
        }

        for (int i = 0; i < filledTiles.Count; i++ )
        {
            if (IsEdgeTile(ConvertV3toV3Int(filledTiles[i])))
            {
                GameObject casterGO = GameObject.Instantiate(ShadowCastersPrefab, filledTiles[i], Quaternion.identity);
                casterGO.transform.SetParent(gameObject.transform);

                ShadowCaster2D caster = casterGO.GetComponent<ShadowCaster2D>();
                SpriteRenderer casterSR = casterGO.GetComponent<SpriteRenderer>();
                casterSR.sortingLayerID = TilemapRenderer.sortingLayerID;
                
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

                // #CHECK
                // May get overwritten by shadowcaster editor scripts. ShadowCaster2DEditor.cs
                caster.SetTargetLayers(targetLayers.ToArray());

                ShadowCasters.Add(casterGO);
                NameShadowCaster(ref casterGO, casterSR.sortingLayerName);

            }
        }
    }
    private void RemovePreviouslyGenerated()
    {
        GameObject casterGO = null;
        for (int i = ShadowCasters.Count - 1; i >= 0; i--)
        {
            casterGO = ShadowCasters[i];
            ShadowCasters.RemoveAt(i);
            DestroyImmediate(casterGO);
        }
    }
    private void NameShadowCaster(ref GameObject casterGO, string sortingLayerName)
    {
        casterGO.name = "CasterGO_" + sortingLayerName + "_" 
            + ShadowCasters.IndexOf(casterGO) + "_" + ConvertV3toV3Int(casterGO.transform.position);
    }
    private bool IsEdgeTile(Vector3Int tilePos)
    {
        Vector3Int V2diag = new Vector3Int(1, 1, 0);
        Vector3Int V2up = new Vector3Int(0, 1, 0);
        Vector3Int V2right = new Vector3Int(1, 0, 0);

        if (
            !Tilemap.HasTile(V2right + tilePos) ||
            !Tilemap.HasTile(-V2right + tilePos) ||
            !Tilemap.HasTile(V2up + tilePos) ||
            !Tilemap.HasTile(-V2up + tilePos) ||
            !Tilemap.HasTile(V2diag + tilePos) ||
            !Tilemap.HasTile(-V2diag + tilePos) ||
            !Tilemap.HasTile(V2diag * -V2up + tilePos) ||
            !Tilemap.HasTile(V2diag * -V2right + tilePos)
            )
        {
            return true;
        }
        else return false;
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

            // // as a hack to make the editor save the shadowcaster instances, we rename them now instead of when theyre generated.

            // Undo.RecordObjects(casters, "GridShadowCastersGenerator name prefab instances");

            // for (var i = 0; i < casters.Length; i++) {
            //     casters[i].name += "_" + i.ToString();
            // }
        }
    }
}
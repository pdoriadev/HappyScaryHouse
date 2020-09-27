using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilemapShadows : MonoBehaviour
{
    [SerializeField]
    private GameObject ShadowCastersPrefab = default;
    [SerializeField]
    private SortingLayer SortingLayer = default;
    // generate shadowcaster gameobject at every tile that's at edge of tilemap collider.
    // shadow should fall onto layers below it. i.e. Boundary will fall onto ground. 

    // Remove previously generated shadowcaster gameobjects.
        // save instances into collection? 

    private List<GameObject> ShadowCasters = new List<GameObject>();
    public List<GameObject> shadowCasters => ShadowCasters;
    public void GenerateShadowCasters()
    {
        
    }
    private void NameShadowCaster(ref GameObject caster)
    {

    }

}

[CustomEditor(typeof(TilemapShadows))]
public class TilemapShadowsEditor : Editor
{
      public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            TilemapShadows generator = (TilemapShadows)target;

            generator.GenerateShadowCasters();

            // // as a hack to make the editor save the shadowcaster instances, we rename them now instead of when theyre generated.

            // Undo.RecordObjects(casters, "GridShadowCastersGenerator name prefab instances");

            // for (var i = 0; i < casters.Length; i++) {
            //     casters[i].name += "_" + i.ToString();
            // }
        }
}
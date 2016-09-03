using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TileImporter : MonoBehaviour
{
    public UnityEngine.Object jsonData;
    float pixelsPerUnit = 100;
    int padding = 1;
    public List<GameObject> prefabs = new List<GameObject>();

    LayerMask mask = 0;
    string[] allMaskNames;

    /// <summary>
    /// Get ASll Mask names of a 
    /// </summary>
    public void GetAllMaskNames()
    {
        mask.value = 0;
        mask = ~mask;
        allMaskNames = MaskToNames(mask);
    }

    public string[] MaskToNames(LayerMask original)
    {
        var layerNames = new List<string>();

        for (int i = 0; i < 32; ++i)
        {
            int shifted = 1 << i;
            if ((original & shifted) == shifted)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    layerNames.Add(layerName);
                }
            }
        }
        return layerNames.ToArray();
    }

    public bool IsValidMask(string name)
    {
        for (int i = 0; i < allMaskNames.Length; i++)
        {
            if (name == allMaskNames[i])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Obtain All children of a transform
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static Transform[] GetTransformKids(Transform me)
    {
        List<Transform> kids = new List<Transform>();
        foreach (Transform kid in me)
        {
            kids.Add(kid);
        }
        return kids.ToArray();
    }

    /// <summary>
    /// Get a JSON String from a file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetJsonStringFromFile(string path)
    {
        string text = "";
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
            text = reader.ReadToEnd();
        reader.Close();
        return text;
    }

#if UNITY_EDITOR
    [ContextMenu("Import")]
    public void Import()
    {
        if (prefabs.Count < 2)
        {
            Debug.LogError("No Prefabs Found: Please add at least 2 Prefabs to importer");
            return;
        }



        string spriteSheet = AssetDatabase.GetAssetPath(prefabs[0].GetComponent<SpriteRenderer>().sprite.texture);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
                   .OfType<Sprite>().OrderBy(x => x.rect.y).ThenBy(x => x.rect.x).ToArray();

        pixelsPerUnit = sprites[0].pixelsPerUnit;


        //Debug.Log("sprites[1].textureRect.x: " + sprites[1].rect.x);
        //Debug.Log("sprites[0].textureRect.x: " + sprites[0].rect.x) ;
        //Debug.Log("sprites[0].textureRect.width " + sprites[0].rect.width);

        padding = (int)sprites[1].rect.x - ((int)sprites[0].rect.x + (int)sprites[0].rect.width);
        //Debug.Log("Padding Found: " + padding);
        GetAllMaskNames();
        List<Transform> kids = new List<Transform>();
        kids.AddRange(GetTransformKids(transform));
        for (int i = 0; i < kids.Count; i++)
        {
            float percent = (float)(i) / (float)(kids.Count);
            EditorUtility.DisplayProgressBar("Destroying Previous Kids", "Destroying...", percent);
            GameObject.DestroyImmediate(kids[i].gameObject);
        }
        EditorUtility.DisplayProgressBar("Parsing JSON Information", "Parsing... This may take a minute", .5f);
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + AssetDatabase.GetAssetPath(jsonData);
        string jsonString = GetJsonStringFromFile(path);
        EditorUtility.DisplayProgressBar("Converting JSON Information", "Parsing... This may take a minute", .75f);
        TileImportSettings tileImportSettings = new TileImportSettings(jsonString);
        List<GameObject> layerObjs = new List<GameObject>();

        for (int i = 0; i < tileImportSettings.layers.Length; i++)
        {
            Tile[] tiles = tileImportSettings.layers[i].tiles;
            GameObject layerObj = new GameObject(tileImportSettings.layers[i].name);
            layerObjs.Add(layerObj);
            layerObj.transform.parent = transform;
            layerObj.transform.localPosition = Vector3.zero;

            for (int j = 0; j < tiles.Length; j++)
            {
                EditorUtility.DisplayProgressBar("Layer: " + (i + 1) + "/" + tileImportSettings.layers.Length + " Generating Tiles", tileImportSettings.layers[i].name, (float)j / (float)tiles.Length);
                if (tiles[j].tile != -1)
                {
                    GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab((UnityEngine.Object)(GetPrefabWithSpriteId(tiles[j].tile)));
                    if (newTile == null)
                    {
                        Debug.Log("X coord: " + tiles[j].x + " Y coord: " + tiles[j].y);
                        Debug.LogError("Could Not Place Tile at: " + new Vector3(tiles[j].x * tileImportSettings.tilewidth, -tiles[j].y * tileImportSettings.tileheight, 0) * (1f / pixelsPerUnit));
                    }
                    newTile.transform.position = new Vector3(tiles[j].x * tileImportSettings.tilewidth, -tiles[j].y * tileImportSettings.tileheight, 0) * (1f / pixelsPerUnit);
                    if (tiles[j].flipX)
                    {
                        newTile.transform.localScale = new Vector3(newTile.transform.localScale.x * -1, newTile.transform.localScale.y, newTile.transform.localScale.z);
                    }

                    newTile.transform.Rotate(0, 0, tiles[j].rot * 90);
                    newTile.transform.parent = layerObj.transform;
                }
            }
        }
        EditorUtility.DisplayProgressBar("Setting Layer Object Names", "Almost Done", .95f);
        for (int i = 0; i < layerObjs.Count; i++)
        {
            //Debug.Log(layerObjs[i].name);
            if (IsValidMask(layerObjs[i].name))
            {
                layerObjs[i].layer = LayerMask.NameToLayer(layerObjs[i].name);
            }

        }
        EditorUtility.ClearProgressBar();
    }
#endif

    GameObject GetPrefabWithSpriteId(int id)
    {
        List<int> spriteIDs = new List<int>();
        for (int i = 0; i < prefabs.Count; i++)
        {
            Sprite sprite = prefabs[i].GetComponent<SpriteRenderer>().sprite;
            int spriteId = FindSpriteId(sprite);
            spriteIDs.Add(spriteId);
            if (spriteId == id)
                return prefabs[i];
        }
        spriteIDs.Sort();
        for (int i = 0; i < spriteIDs.Count; i++)
        {
            Debug.Log(spriteIDs[i]);
        }
        Debug.LogError("Could Not Get Prefab with Sprite ID: " + id);
        return null;
    }

    public int FindSpriteId(Sprite sprite)
    {
        int horizontalSprites = ((int)sprite.texture.width - padding) / ((int)sprite.rect.width + padding);
        int verticalSprites = ((int)sprite.texture.height - padding) / ((int)sprite.rect.height + padding);

        float horizontalPercent = (sprite.rect.x - padding) / (sprite.texture.width - padding);
        float verticalPercent = (sprite.rect.y - padding) / (sprite.texture.height - padding);

        int horizontalIndex = Mathf.RoundToInt(horizontalPercent * horizontalSprites);
        int verticalIndex = Mathf.RoundToInt(verticalPercent * verticalSprites);
        int verticalValue = verticalSprites - verticalIndex - 1;

        return horizontalIndex + (verticalValue * horizontalSprites);
    }

}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TileImportSettings 
{
    public int tileshigh = 6,
    tilewidth = 16,
    tileswide = 8,
    tileheight = 16;
    public Layer[] layers;

    public TileImportSettings()
    { 
    }

    public TileImportSettings(string jsonString)
    {
        FromJSON(jsonString);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsonObj = new JSONObject();
        jsonObj.AddField("tileshigh", tileshigh);
        jsonObj.AddField("tilewidth", tilewidth);
        jsonObj.AddField("tileswide", tileswide);
        jsonObj.AddField("tileheight", tileheight);

        JSONObject layersArray = new JSONObject(JSONObject.Type.ARRAY);
        if (layers != null)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layersArray.Add(layers[i].ToJSON());
            }
        }
        jsonObj.AddField("layers", layersArray);

        return jsonObj;
    }

    public void FromJSON(string jsonString)
    {
        JSONObject jsonObj = new JSONObject(jsonString);
        tileshigh = (int)jsonObj.GetField("tileshigh").n;
        tilewidth = (int)jsonObj.GetField("tilewidth").n;
        tileswide = (int)jsonObj.GetField("tileswide").n;
        tileheight = (int)jsonObj.GetField("tileheight").n;

        JSONObject layersArray = jsonObj.GetField("layers");
        List<Layer> layersList = new List<Layer>();
        for (int i = 0; i < layersArray.list.Count; i++)
        {
            Layer layer = new Layer(layersArray[i].ToString());
            layersList.Add(layer);
        }
        layers = layersList.ToArray();
    }
}

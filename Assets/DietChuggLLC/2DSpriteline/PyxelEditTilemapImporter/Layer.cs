using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Layer
{
    public string name = "";
    public int number;
    public Tile[] tiles;

    public Layer()
    { 
    }

    public Layer(string jsonString)
    {
        FromJSON(jsonString);
    }

    public JSONObject ToJSON()
    {
        JSONObject jsonObj = new JSONObject();
        jsonObj.AddField("name", name);
        jsonObj.AddField("number", number);

        JSONObject tilesArray = new JSONObject(JSONObject.Type.ARRAY);
        if (tiles != null)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tilesArray.Add(tiles[i].ToJSON());
            }
        }
        jsonObj.AddField("tiles", tilesArray);
        return jsonObj;
    }

    public void FromJSON(string jsonString)
    {
        JSONObject jsonObj = new JSONObject(jsonString);
        name = jsonObj.GetField("name").str;
        number = (int)jsonObj.GetField("number").n;
        JSONObject tileArray = jsonObj.GetField("tiles");
        List<Tile> tileList = new List<Tile>();
        for (int i = 0; i < tileArray.list.Count; i++)
        {
            Tile tile = new Tile(tileArray[i].ToString());
            tileList.Add(tile);
        }
        tiles = tileList.ToArray();
    }
}

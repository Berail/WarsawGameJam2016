using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Tile 
{
    public bool flipX = false;
    public int y = 0;
    public int rot = 0;
    public int tile = -1;//-1 is default which is an empty tile
    public int index = 0;
    public int x = 0;

    public Tile()
    { 
    }

    public Tile(string jsonString)
    {
        FromJSON(jsonString);
    }

    public JSONObject ToJSON()
    { 
        JSONObject jsonObj = new JSONObject();
        jsonObj.AddField("flipX", flipX);
        jsonObj.AddField("y", y);
        jsonObj.AddField("rot", rot);
        jsonObj.AddField("tile", tile);
        jsonObj.AddField("index", index);
        jsonObj.AddField("x", x);
        return jsonObj;
    }

    public void FromJSON(string jsonString)
    {
        JSONObject jsonObj = new JSONObject(jsonString);
        flipX = jsonObj.GetField("flipX").b;
        y = (int)jsonObj.GetField("y").n;
        rot = (int)jsonObj.GetField("rot").n;
        tile = (int)jsonObj.GetField("tile").n;
        index = (int)jsonObj.GetField("index").n;
        x = (int)jsonObj.GetField("x").n;

    }
}

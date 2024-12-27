using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Section {
    
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] public List<Structure.Structure> structures;
    [SerializeField] public Vector2 position;
    [SerializeField] public Vector2 dimension;
    [SerializeField] public Vector2 playerSpawnPos;

    public bool Inside(Vector2 playerPosition) {
        return playerPosition.x >= position.x && playerPosition.x <= position.x + dimension.x &&
               playerPosition.y >= position.y && playerPosition.y <= position.y + dimension.y;
    }

    public string GetName() {
        return name;
    }

    public int GetId() {
        return id;
    }
}
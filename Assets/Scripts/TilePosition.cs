using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePosition : MonoBehaviour
{
    public Tilemap tileMap;
    private CircleCollider2D circle;

    public List<Bounds> availablePlaces;
    public List<Bounds> legalTiles;

    void Awake()
    {
        availablePlaces = new List<Bounds>();
        legalTiles = new List<Bounds>();
        circle = GetComponent<CircleCollider2D>();

        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    place = new Vector3(place.x + 0.5f, place.y + 0.5f, place.z);
                    //Tile at "place"
                    availablePlaces.Add(new Bounds(place, new Vector3(1, 1, 0)));
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(string.Join(", ", legalTiles));
        }
        if (Input.GetKeyDown("p"))
        {
            EstablishSpikeableTiles();
        }
    }

    void EstablishSpikeableTiles()
    {
        legalTiles.Clear();
        for(int i = 0; i < availablePlaces.Count; i++)
        {
            if (circle.bounds.Intersects(availablePlaces[i]))
            {
                legalTiles.Add(availablePlaces[i]);
            }
        }
    }
}

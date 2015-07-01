using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour
{
    private CharacterMovement character;

    // Use this for initialization
    void Start()
    {
		GameObject p = GameObject.FindGameObjectWithTag("Penguin");
		this.character = p.GetComponent<CharacterMovement> ();
    }

    // Update is called once per frame
    void Update()
    {
        var terrainData = Terrain.activeTerrain.terrainData;
        var terrainPos = Terrain.activeTerrain.transform.position;
        var mapX = ((character.transform.position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth;
        var mapZ = ((character.transform.position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight;

        var splatmapData = Terrain.activeTerrain.terrainData.GetAlphamaps((int) character.transform.position.x, (int)character.transform.position.y, 1, 1);

		// texture layer 1
        var texture1Level = splatmapData[0, 0, 0];  
    }
}

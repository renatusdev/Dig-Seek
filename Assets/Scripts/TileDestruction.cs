using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileDestruction : MonoBehaviour
{
    [Range(1, 3)] public float maxPitch;
    [Range(0, 1)] public float timeToResetPitch;
    public ParticleSystem minedTiledGPX; 
    public Tile backUnderground; 
    public Tilemap backgroundTileMap;
    
    private Tilemap m_Tilemap;
    private AudioSource audioSource;
    private GameObject particles;
    private float drillStoppedTimer;

    void Start()
    {
        particles = GameObject.FindGameObjectWithTag("Particles"); 
        
        if(particles.Equals(null))
        {
            particles = new GameObject("Particles");
            particles.tag = "Particles";
        }
        
        m_Tilemap = GetComponent<Tilemap>();
        GetComponent<TilemapRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<TilemapRenderer>().receiveShadows = true;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        drillStoppedTimer += Time.deltaTime;

        if(drillStoppedTimer >= timeToResetPitch)
            audioSource.pitch = 2;
    }

    public void DestroyTileAt(ContactPoint2D hit)
    {   
        Vector3 hitPosition = Vector3.zero;
        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

        Vector3Int cell = m_Tilemap.WorldToCell(hitPosition);

        if(m_Tilemap.GetTile(cell) != null)
        {
            drillStoppedTimer = 0;
            audioSource.Play();
            audioSource.pitch += 0.05f;
            audioSource.pitch = Mathf.Clamp(audioSource.pitch, 0, maxPitch);
            Instantiate(minedTiledGPX, hitPosition, Quaternion.identity, particles.transform);
        }
        
        // backgroundTileMap.SetTile(cell, backUnderground);
        m_Tilemap.SetTile(cell, null);
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestruction : MonoBehaviour
{   
    public static TileDestruction i;

    void Awake()
    {
        if(i == null)
            i = this;
        else
            Destroy(this);
    }   

    [Range(1, 3)] public float maxPitch;
    [Range(0, 1)] public float timeToResetPitch;
    public ParticleSystem minedParticles; 
    public Tile minedHazard;
    public GameObject hazardExplosion;
    public GameObject minedGems;
    public Tilemap tilemap;
    public Tilemap tilemapBackground;
    public AudioSource m_MinedTerrainSFX;

    private GameObject particles;
    private float drillStoppedTimer;

    float timerPerExplosion;
    static float timePerExplosion = 0.5f;  

    void Start()
    {
        particles = GameObject.FindGameObjectWithTag("Particles"); 
        timerPerExplosion = 0;

        if(particles.Equals(null))
        {
            particles = new GameObject("Particles");
            particles.tag = "Particles";
        }
        
        GetComponent<TilemapRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<TilemapRenderer>().receiveShadows = true;
    }

    void Update()
    {
        drillStoppedTimer += Time.deltaTime;

        if(drillStoppedTimer >= timeToResetPitch)
           m_MinedTerrainSFX.pitch = 2;


        timerPerExplosion -= Time.deltaTime;
    }

    public void DestroyTileAt(Vector3Int cell)
    {   
        if(tilemap.GetTile(cell) != null)
        {
            if(tilemap.GetTile(cell).name.Equals("Hazardous"))
            {
                GameObject o = Instantiate(hazardExplosion, tilemap.CellToWorld(cell), Quaternion.identity, particles.transform);
                tilemapBackground.SetTile(cell, minedHazard);

                if(timerPerExplosion > 0)
                {
                    o.GetComponent<AudioSource>().enabled = false;
                }
                else
                {
                    timerPerExplosion = timePerExplosion;
                }
            }
            else if (tilemap.GetTile(cell).name.StartsWith("Gems"))
            {
                Instantiate(minedGems, tilemap.CellToWorld(cell), Quaternion.identity, particles.transform);
                Scoreboard.i.timer.Substract(3);
                Scoreboard.i.DiamondUp();
            }
            else
                Instantiate(minedParticles, tilemap.CellToWorld(cell), Quaternion.identity, particles.transform);
            
            tilemap.SetTile(cell, null);

            drillStoppedTimer = 0;
            m_MinedTerrainSFX.Play();
            m_MinedTerrainSFX.pitch += 0.05f;
            m_MinedTerrainSFX.pitch = Mathf.Clamp(m_MinedTerrainSFX.pitch, 0, maxPitch);
        }
    }

    public void DestroyTileAt(ContactPoint2D hit) 
    {   
        Vector3 hitPosition = Vector3.zero;
        hitPosition.x = hit.point.x - 0.1f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.1f * hit.normal.y;

        Vector3Int cell = tilemap.WorldToCell(hitPosition);

        DestroyTileAt(cell);
    }

    public void DestroyTileAt(Vector3 pos)
    {   
        Vector3Int cell = tilemap.WorldToCell(pos);
        DestroyTileAt(cell);
    }

    public void ExplodeTilesAt(Vector3 pos, int explosionRadius)
    {   
        Vector3Int cell = tilemap.WorldToCell(pos);

        for(int y = -explosionRadius; y <= explosionRadius; y++)
            for(int x = -explosionRadius; x <= explosionRadius; x++)
                DestroyTileAt(new Vector3Int(cell.x + x, cell.y + y, 0));
    }
}

using UnityEngine;

public class RandomSeed : MonoBehaviour
{    
    public static int seed { get; set;}

    public bool randomize;

    private void Awake()
    {
        if(randomize)
            seed = Mathf.Abs(System.DateTime.Now.GetHashCode() % 1000);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fish;
    private List<GameObject> ActiveFish = new List<GameObject>();
    private HookController Hook;

    public int MaxFishPerDepth = 7;

    public int MaxActiveFish;
    private void Start()
    {
        MaxActiveFish = MaxFishPerDepth * 4;
        Hook = GameObject.Find("hook").GetComponent<HookController>();
    }

    void Update()
    {
        if (ActiveFish.Count() >= MaxActiveFish) return;
        int depth = Hook.GetDepthGroup();
        GameObject[] fish = Fish.Where(x => x.GetComponent<FishParent>().DepthSpawnedAt == depth || x.GetComponent<FishParent>().DepthSpawnedAt == depth+1).ToArray();

        List<GameObject> fishRarity = new List<GameObject>();

        foreach(var f in fish)
        {
            var s = f.GetComponent<FishParent>();
            float rarity = (1/(float)s.Price)*1000;
            for(int i = 0; i < rarity; i++)
            {
                fishRarity.Add(f);
            }
        }



        int rdmIndex = Random.Range(0, fishRarity.Count());
        var rdmFish = fishRarity[rdmIndex];

        int potDepth = rdmFish.GetComponent<FishParent>().DepthSpawnedAt;
        if (ActiveFish.Where(x => x.GetComponent<FishParent>().DepthSpawnedAt == potDepth).Count() >= MaxFishPerDepth)
            return;
        var fishObject = Instantiate(rdmFish);
        var fishScript = fishObject.GetComponent<FishParent>();

        float[] spawnXs = { -15, 15 };
        int xIndex = Random.Range(0, 2);

        float yVal = -Hook.DepthToY(33.33f);
        float yStart = yVal * fishScript.DepthSpawnedAt - yVal;
        float yEnd = yVal * fishScript.DepthSpawnedAt;

        fishObject.transform.position = new Vector3(spawnXs[xIndex],Random.Range(yStart,yEnd),fishObject.transform.position.z);

        fishObject.name = fishObject.name.Replace("(Clone)", "");
        ActiveFish.Add(fishObject);
    }

    public void DespawnFish(GameObject fish)
    {
        ActiveFish.Remove(fish);
        Destroy(fish);
    }
}


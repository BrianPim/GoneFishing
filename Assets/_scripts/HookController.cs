using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HookController : MonoBehaviour
{
    private SpriteRenderer waterSprite;
    private SpriteRenderer sr;
    private Camera cam;
    private float maxHeight;
    private LineRenderer lr;

    public float baseSinkSpeed = 1f;
    public float baseReelSpeed = 1f;
    public float strafeSpeed = 1f;

    public GameObject shopWindow;
    public GameObject fishpediaWindow;
    public GameObject pauseWindow;
    public GameObject mainWindow;
    public GameObject fisherman;
    public GameObject moneyUI;
    public GameObject baitUI;
    public GameObject maxDepthUI;
    public GameObject audioHolder;
    public GameObject depthUI;
    public GameObject baitSprite;
    public PostProcessingChanger pp;
    public Color SurfaceBlue = new Color(97, 191, 255);
    public Color DeepBlue = new Color(11, 16, 150);
    public Color FireOrange = new Color(255,114,0);
    public Color DemonRed = new Color(198,0,0);
    public Color CurrentWaterColor;
    public int money = 0;
    public int bait = 0;
    public int rodLevel = 1;
    public int hookCount = 1;
    public bool flashlight = false;
    bool playParticles = true;
    bool isReeled = true;
    bool isCasting = false;
    bool chemicalsInTheWater = false;
    public List<FishParent> hookedFish = new List<FishParent>();

    private Vector3 lineStart;
    private ParticleSystem ps;
    private int moneytoadd = 0;
    private int totalmoneyadding = 0;
    private int moneyBreakTime = 0;
    private int previousDepth = 1;
    public float mousexoffset = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        maxHeight = transform.position.y;
        waterSprite = GameObject.Find("Water").GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.05f;
        lr.SetPosition(0, transform.position - new Vector3(.08f, -.1f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.92f);//9.92 is to make up for camera z offset
        var depth = GetDepth();
        //GameObject.Find("DebugText").GetComponent<Text>().text = /*$"mouse: {cam.ScreenToWorldPoint(mousePos)}\nhook: {transform.position}\n" + */"Depth: " + GetDepthGroup() + "\nMoney: " + money + "\nDeltaTime: " + Time.deltaTime;
        var timeMod = Time.deltaTime * 2;
        var yDist2Mouse = Math.Min(4f,Math.Abs(cam.ScreenToWorldPoint(mousePos).y - transform.position.y)); //either actual dist or 2.5, whichever smaller
        
        float sinkSpeed = baseSinkSpeed * (yDist2Mouse / 3);
        float reelSpeed = baseReelSpeed * (yDist2Mouse / 3);

        if (depth >= 0)
            depthUI.GetComponent<TextMeshProUGUI>().text = "Depth: " + Math.Floor(depth);
        else
            depthUI.GetComponent<TextMeshProUGUI>().text = "Depth: 0";

        if (depth <= 3 && !shopWindow.activeSelf && !fishpediaWindow.activeSelf && !pauseWindow.activeSelf && !mainWindow.activeSelf && !fisherman.activeSelf)
        {
            if (mousePos.y < cam.pixelHeight / 2)
            {
                transform.position -= new Vector3(0, sinkSpeed * timeMod,0);
                if (isReeled && !audioHolder.transform.Find("Rod_Cast").GetComponent<AudioSource>().isPlaying)
                {
                    audioHolder.transform.Find("Rod_Cast").GetComponent<AudioSource>().Play();
                    isReeled = false;
                }
            }
            else
            {
                if (transform.position.y - 0.01f < maxHeight)
                {
                    transform.position += new Vector3(0, reelSpeed * timeMod, 0);
                    if (Input.GetMouseButton(0))
                    {
                        transform.position += new Vector3(0, reelSpeed * timeMod * 3, 0);
                    }
                }
            }

            //return hook to end of rod

            if (1 > transform.position.x)
                transform.position += new Vector3(strafeSpeed * 2 * timeMod, 0, 0);

            if (1 < transform.position.x)
                transform.position -= new Vector3(strafeSpeed * 2 * timeMod, 0, 0);
        }
        else if (!shopWindow.activeSelf && !fishpediaWindow.activeSelf && !pauseWindow.activeSelf && !mainWindow.activeSelf && !fisherman.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                transform.position += new Vector3(0, reelSpeed * timeMod * 3, 0);
            }
            if (cam.ScreenToWorldPoint(mousePos).y > transform.position.y + .25) //.25 for deadzone
            {
                maxDepthUI.SetActive(false);
                transform.position += new Vector3(0, reelSpeed * timeMod, 0);
                
                if (!isCasting)
                {
                    audioHolder.transform.Find("Rod_Reel").GetComponent<AudioSource>().Play();
                    isCasting = true;
                }
            }
            else if (cam.ScreenToWorldPoint(mousePos).y < transform.position.y - .25) //.25 for deadzone
            {
                if (!isCasting)
                {
                    audioHolder.transform.Find("Rod_Reel").GetComponent<AudioSource>().Play();
                    isCasting = true;
                }

                maxDepthUI.SetActive(false);
                if (depth + sinkSpeed * timeMod < rodLevel * 33.33 || rodLevel == 4)
                    transform.position -= new Vector3(0, sinkSpeed * timeMod, 0);
                    
                else //reached rod limit
                    maxDepthUI.SetActive(true);
            } 
            else
            {
                if (isCasting)
                {
                    audioHolder.transform.Find("Rod_Reel").GetComponent<AudioSource>().Stop();
                    isCasting = false;
                }
            }

            mousePos.x += mousexoffset;

            if (cam.ScreenToWorldPoint(mousePos).x > transform.position.x)
                transform.position += new Vector3(strafeSpeed * timeMod, 0, 0);

            

            if (cam.ScreenToWorldPoint(mousePos).x < transform.position.x)
                transform.position -= new Vector3(strafeSpeed * timeMod, 0, 0);
        }

        lr.SetPosition(1, transform.position - new Vector3(.08f,-.1f,0));

        if (depth < 0)
        {
            if (isCasting)
                isCasting = false;
        }

        if (depth <= 0.01f && !audioHolder.transform.Find("Rod_Cast").GetComponent<AudioSource>().isPlaying)
        {
            audioHolder.transform.Find("Rod_Reel").GetComponent<AudioSource>().Stop();
            isReeled = true;
        }

        if (depth < 1) //sell fish
        {
            bool soldDesiredFish = false;
            foreach(var fish in hookedFish)
            {
                float multiplier = 1;
                if (GameObject.Find("Job Menu").GetComponent<FishermanMenu>().desiredFish == fish.name)
                {
                    multiplier = 2f;
                    soldDesiredFish = true;
                }
                moneytoadd += (int)(fish.Price * multiplier);
                totalmoneyadding += (int)(fish.Price * multiplier);

                if (fishpediaWindow.transform.parent.gameObject.GetComponent<Fishpedia>().caught[fish.gameObject.name] == false)
                {
                    if (fish.gameObject.GetComponent<BaitLovingFish>() != null)
                        fishpediaWindow.transform.parent.gameObject.GetComponent<Fishpedia>().FoundFish(fish.gameObject.name, fish.gameObject.GetComponent<BaitLovingFish>().DepthSpawnedAt, fish.Price, true);
                    else if (fish.gameObject.GetComponent<ScaredyFish>() != null)
                        fishpediaWindow.transform.parent.gameObject.GetComponent<Fishpedia>().FoundFish(fish.gameObject.name, fish.gameObject.GetComponent<ScaredyFish>().DepthSpawnedAt, fish.Price, false);
                    else if (fish.gameObject.GetComponent<DefaultFish>() != null)
                        fishpediaWindow.transform.parent.gameObject.GetComponent<Fishpedia>().FoundFish(fish.gameObject.name, fish.gameObject.GetComponent<DefaultFish>().DepthSpawnedAt, fish.Price, false);
                }
                    
                if (fish.gameObject.name == "frog")
                    chemicalsInTheWater = true;

                GameObject.Find("FishSpawner").GetComponent<FishSpawner>().DespawnFish(fish.gameObject);
            }
            if (soldDesiredFish) GameObject.Find("Job Menu").GetComponent<FishermanMenu>().NewFish();
            hookedFish.Clear();
        }

        if (moneytoadd > 0 && playParticles)
        {
            ps = GetComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = moneytoadd*0.05f;
            GetComponent<ParticleSystem>().Play();
            playParticles = false;

            if (chemicalsInTheWater)
            {
                audioHolder.transform.Find("Globalist").GetComponent<AudioSource>().Play();
                chemicalsInTheWater = false;
            }
            else
                audioHolder.transform.Find("Rod_Sell").GetComponent<AudioSource>().Play();
        }

        if (moneytoadd > 0 && moneyBreakTime >= -Math.Pow(.1*moneytoadd,2) + -.5*moneytoadd + 15)
        {
            money++;
            moneyUI.GetComponent<TextMeshProUGUI>().text = money.ToString();
            moneytoadd--;
            moneyBreakTime = 0;
        }
        else
        {
            moneyBreakTime++;
        }

        if (moneytoadd == 0)
        {
            playParticles = true;
            totalmoneyadding = 0;
        }

        if (GetDepthGroup() == 1 && previousDepth != 1)
        {
            audioHolder.GetComponent<MusicHandler>().PlayMusic(1);
            previousDepth = GetDepthGroup();
            Debug.Log(GetDepthGroup());
        }
        else if (GetDepthGroup() == 2 && previousDepth != 2)
        {
            audioHolder.GetComponent<MusicHandler>().PlayMusic(2);
            previousDepth = GetDepthGroup();
            Debug.Log(GetDepthGroup());
        }
        else if (GetDepthGroup() == 3 && previousDepth != 3)
        {
            audioHolder.GetComponent<MusicHandler>().PlayMusic(3);
            previousDepth = GetDepthGroup();
            Debug.Log(GetDepthGroup());
        }
        else if (GetDepthGroup() == 4 && previousDepth != 4)
        {
            audioHolder.GetComponent<MusicHandler>().PlayMusic(4);
            previousDepth = GetDepthGroup();
            Debug.Log(GetDepthGroup());
        }
        
        SetWaterColour(depth);
    }


    private void SetWaterColour(float depth)
    {
        if (depth < 1)
        {
            pp.SetVignetteIntensity(0);
        }
        else if (!flashlight)
        {
            pp.SetVignetteColour(pp.defaultVignette);
        }
        else
        {
            pp.SetVignetteColour(pp.flashlightVignette);
        }

        if (depth <= 100)
        {
            CurrentWaterColor = Color.Lerp(SurfaceBlue, DeepBlue, depth / 100); //lagoon to depth
        } 
        else if (depth <= 110)
        {
            CurrentWaterColor = Color.Lerp(DeepBlue, FireOrange, (depth - 100) / 10); //depth to hell
        } 
        else
        {
            CurrentWaterColor = Color.Lerp(FireOrange, DemonRed, (depth - 110)/50); //hell to depths of hell
        }

        CurrentWaterColor.a = 1; //for some reason they come out with less alpha


        waterSprite.color = CurrentWaterColor;

        pp.SetVignetteIntensity(Mathf.Lerp(0, 1, depth / 110));
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (hookedFish.Count < hookCount && !hookedFish.Contains(collision.gameObject.GetComponent<FishParent>()))
            {
                var fish = collision.gameObject;
                var script = fish.GetComponent<FishParent>();
                hookedFish.Add(script);
                audioHolder.transform.Find("Rod_Caught").GetComponent<AudioSource>().Play();
                script.hooked = true;
                //fish.transform.parent = transform;


                if (bait > 0)
                {
                    bait--;
                }

                //Vector3 colliderPos =  script.bc2d.bounds.center;
                //colliderPos = transform.InverseTransformPoint(colliderPos);
                //fish.transform.position -= colliderPos;
            }
        }
    }

    public void UpdateResourcesUI()
    {
        moneyUI.GetComponent<TextMeshProUGUI>().text = money.ToString();
        baitUI.GetComponent<TextMeshProUGUI>().text = bait.ToString();

        if (bait > 0)
            baitSprite.SetActive(true);
        else
            baitSprite.SetActive(false);
    }

    public float GetDepth()
    {
        return maxHeight - transform.position.y;
    }

    public float DepthToY(float depth)
    {
        return depth - maxHeight;
    }

    public int GetDepthGroup()
    {
        return (int)System.Math.Floor(GetDepth() / 33.33) + 1;
    }
}

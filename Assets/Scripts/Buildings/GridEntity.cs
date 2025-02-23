using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GridEntity
{
    public readonly Color ConstructionBarColor = new Color(1, 0.5f, 0, 1);
    public readonly Color HealthBarColor = new Color(0,1,0.3f,1);
    /// <summary>
    /// Used for market name.
    /// </summary>
    public string Name;
    /// <summary>
    /// Strongness of the building.
    /// </summary>
    public ushort Health;
    public Sprite Sprite;
    public Sprite ConstructionSprite;
    public ushort MarketPrice;
    public float ConstructionSpeed;
    public ushort ConstructionSpeedBoost;

    [HideInInspector] public SpriteRenderer GridSpriteRenderer;
    [HideInInspector] public Transform HealthBar;
    [HideInInspector] public Transform HealthBarValue;
    [HideInInspector] public SpriteRenderer HealthBarValueSpriteRenderer;
    [HideInInspector] public ushort CurrentHealth;
    [HideInInspector] public bool IsConstructionDone;
    /// <summary>
    /// 255 is max Construction Progress Means Grid Entity is built.
    /// </summary>
    [HideInInspector] public float ConstructionProgress;
    public void print(object x)
    {
        Debug.Log(x);
    }
    public void GetDamaged(ushort Damage)
    {
        if (!IsConstructionDone)
            return;
        CurrentHealth = (ushort)Mathf.Clamp(CurrentHealth-Damage, 0, 65535);

        UpdateHealth();
    }
    public void UpdateHealth()
    {
        if (!IsConstructionDone)
            return;
        float ratio = (float)CurrentHealth / Health;
        print("health ratio: " + ratio + " " + CurrentHealth + " " + Health + " " + Name);
        if (ratio == 1)//If full hp or no hp don't show health bar
            HealthBar.gameObject.SetActive(false);
        else if (ratio == 0)
            DestroyEntity();
        else
        {
            if (!HealthBar.gameObject.activeSelf)
                HealthBar.gameObject.SetActive(true);
            HealthBarValue.transform.localScale = new Vector3(ratio * 2.04f, HealthBarValue.transform.localScale.y, HealthBarValue.transform.localScale.z);
        }


    }
    public void DestroyEntity()
    {
        foreach(Transform child in GridSpriteRenderer.transform)
        {
            if (child.name != "Is Selected")
                GameManager.Destroy(child.gameObject);
        }

        GridSpriteRenderer.sprite = null;
        GridSpriteRenderer.GetComponent<BoxCollider2D>().isTrigger = true;

        for (ushort j = 0; j < GameManager.main.SpawnedTowerEntities.Count; j++) 
            if(GameManager.main.SpawnedTowerEntities[j] == this)
                GameManager.main.SpawnedTowerEntities.RemoveAt(j);



        for (ushort j = 0; j < GameManager.main.SpawnedPlaneEntities.Count; j++)
            if (GameManager.main.SpawnedPlaneEntities[j] == this)
                GameManager.main.SpawnedPlaneEntities.RemoveAt(j);
         
        //Smoke effects here to Instantiate smoke for breaking building


    }
    public void UpdateBase()
    {
        Update();
    }
    public virtual void Update()
    {

    }
    public void FixedUpdateBase()
    {
       if(ConstructionProgress <100)
       {
           ConstructionProgress = (float)Mathf.Clamp(ConstructionSpeed+ConstructionProgress,0,100); 
           HealthBarValue.transform.localScale = new Vector3((ConstructionProgress/100f) * 2.04f, HealthBarValue.transform.localScale.y, HealthBarValue.transform.localScale.z);
           print("Construction progressing: " + ConstructionProgress);
       }
       else if(ConstructionProgress == 100 && !IsConstructionDone)
       {
           IsConstructionDone = true;
           HealthBarValueSpriteRenderer.color = HealthBarColor;
           GridSpriteRenderer.sprite = Sprite;
           UpdateHealth();
           ConstructionComplete();
       }
       FixedUpdate();
    }
    public virtual void FixedUpdate() { }
    public virtual void ConstructionComplete() { }
    public void StartBase()
    { 
        HealthBar.transform.localPosition = new Vector3(0, -0.405f, -0.1f);
        HealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
        HealthBar.transform.localScale = new Vector3(0.14f, 0.01574188f, 1);

        HealthBarValue = HealthBar.transform.Find("Bar Value");
        HealthBarValueSpriteRenderer = HealthBar.transform.Find("Bar Value").GetComponent<SpriteRenderer>();

        GridSpriteRenderer.sprite = ConstructionSprite;
        GridSpriteRenderer.GetComponent<BoxCollider2D>().isTrigger = false;

        if (GridSpriteRenderer.sprite == null)
            throw new System.Exception("No Sprite Found!");

        HealthBar.gameObject.SetActive(true);
        HealthBarValueSpriteRenderer.color = ConstructionBarColor;
        CurrentHealth = Health;
        Start();
        print("Start Is Called");
    }
    public virtual void Start() { }
    public GridEntity CloneThis() => (GridEntity)this.MemberwiseClone();
}

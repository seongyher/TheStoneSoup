using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{

    public List<GameObject> group1;
    public List<GameObject> group2;
    public List<GameObject> group3;
    public List<GameObject> group4;
    public Transform spawnL;
    public Transform spawnR;
    public float spawnMin = 0.5f;
    public float spawnMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnIngredient());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn ingredient every 0.5-1s
    IEnumerator SpawnIngredient()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnMin, spawnMax));
            int group = Random.Range(1, 4);
            SpawnFromGroup(group);
        }
    }

    void SpawnFromGroup(int group) {
        GameObject g;
        if (group == 1) {
            int i = Random.Range(0, group1.Count);
            g = group1[i];
        } else if (group == 2) {
            int i = Random.Range(0, group2.Count);
            g = group2[i];
        } else if (group == 3) {
            int i = Random.Range(0, group3.Count);
            g = group3[i];
        } else if (group == 4) {
            int i = Random.Range(0, group4.Count);
            g = group4[i];
        } else {
            return;
        }


        //pick pos between spawnL and spawnR
        float t = Random.Range(0.0f, 1.0f);
        float x = Mathf.Lerp(spawnL.position.x, spawnR.position.x, t);
        float y = Mathf.Lerp(spawnL.position.y, spawnR.position.y, t);
        Vector3 spawnPos = new Vector3(x,y,0);


        Instantiate(g, spawnPos, Quaternion.identity);
    }
}

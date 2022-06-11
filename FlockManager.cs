using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlockManager : MonoBehaviour
{
    //variáveis
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;
    [Header("Configurações do Cardume")]//variaveis com as caracteristicas do cardume
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;
    void Start()
    {
        //"seta" o tamanho do array, igual o numero do peixe
        allFish = new GameObject[numFish];
        //Um loop com o numero de peixes
        for (int i = 0; i < numFish; i++)
        {
            //Ele instancia os peixes em areas aleatorias, dentro dos limites definidos
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //a posição que ele ira rodar em volta
        goalPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
        goalPos = this.transform.position;
        //Adiciona variações no movimento
        if (Random.Range(0, 100) < 10)
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
    }
}
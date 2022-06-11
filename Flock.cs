using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flock : MonoBehaviour
{
    //variáveis
    public FlockManager myManager;
    public float speed;
    bool turning = false;
    // Start is called before the first frame update
    void Start()
    {
        //Pefa uma velocidade aleatoria, entre os dois valores do myManager
        speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
    }
    void Update()
    {
        //Cria uma area na posição do myMnager com o tamanho da area da area do swinLimits
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        RaycastHit hit = new RaycastHit();
        //Pega a direção entre o peixe e o pilar
        Vector3 direction = myManager.transform.position - transform.position;
        //Se o peixe não estiver dentro da area, "seta" turning para verdadeiro
        if (!b.Contains(transform.position))
        {
            turning = true;
            //Pega a direção entre o peixe e o pilar
            direction = myManager.transform.position - transform.position;
        }
        //caso o contrário, se o Raycast detectar uma colisao, "seta" o turning para verdadeiro e "reflete" o peixe.
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
            turning = false;
        //Se for verdadeiro, rotaciona o peixe com o Quaternion.Slerp para a direção declarada anteriomente.
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
        }
        //caso contrário 
        else
        {
            //Uma porcentagem de 10%, ele "seta" uma velocidade
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed,
                myManager.maxSpeed);
            //Se essa porcentagem for de 20%, chama o metodo "applyRules".
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        //Move o peixe para frente
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    
    void ApplyRules()
    {
        //variaveis do metodo applyRules
        GameObject[] gos;
        gos = myManager.allFish;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        foreach (GameObject go in gos)//loop para cada peixe
        {
            //se o peixe for diferente ao atual, 
            if (go != this.gameObject)
            {
                //calcula a distancia entre um peixe e esse
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //se a distancia for menor que a vizinho
                if (nDistance <= myManager.neighbourDistance)
                {
                    //adiciona a posição do peixe ao vcentre
                    vcentre += go.transform.position;
                    groupSize++;//aumenta o tamanho do grupo
                    
                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //Ele instancia um novo script Flock
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //aumenta a velocidade do grupo com a velocidade do script instanciado
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
     
        if (groupSize > 0)
        {
            
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
    }
}
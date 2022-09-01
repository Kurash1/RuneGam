using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceturretcontroller : MonoBehaviour
{
    private float time = 30;
    private float cooldown = 5;
    [SerializeField] private LayerMask EnemyMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {
            Vector3 pos = transform.position;
            Collider[] coll = Physics.OverlapSphere(pos, 1f, EnemyMask);
            if (coll[0] != null)
            {
                Vector3 start = pos;
                Vector3 end = coll[0].transform.position;
                EnemyScript enemy = coll[0].GetComponent<EnemyScript>();
                enemy.health -= 5;
                cooldown = 5;
            }
        }
        else { cooldown -= Time.deltaTime; }
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }
}

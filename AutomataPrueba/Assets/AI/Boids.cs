﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boids : MonoBehaviour
{[SerializeField]
    GameObject agent;
    List<Agent> agents;

    public float agentRadius;
    [SerializeField]
    float separationRatio = 1f, cohesionRatio = 0.5f, alignmentRatio = 0.5f;

    public Transform targetPos;
    public float targetPosSpeed;

    private int currentPos;
    private Vector2 input;
   
    // Start is called before the first frame update
    void Start()
    {
        agents = new List<Agent>();
        for(int i = 0; i<20; i++)
        {
            Vector3 position = Vector3.up * Random.Range(0,10) 
             +Vector3.right * Random.Range(0,10) + Vector3.forward * Random.Range(0,10);
            //Vector3 position = Vector3.zero;
            agents.Add(Instantiate(agent, position, Quaternion.identity, transform).GetComponent<Agent>());
            agents[i].startVector = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f) * Vector3.forward ;
           

        }
    }

    // Update is called once per frame
    void Update()
    {

        input.x = Input.GetButton("Horizontal") ? Input.GetAxis("Horizontal") : 0;
        input.y = Input.GetButton("Vertical") ? Input.GetAxis("Vertical") : 0;

        targetPos.position += ((input.x * targetPos.right) + (input.y * targetPos.forward)).normalized * targetPosSpeed * Time.deltaTime;

        foreach (Agent a in agents)
        {
            //a.velocity = Quaternion.Euler(0.0f, Random.Range(-30, 30), 0.0f) * a.startVector;
            a.velocity = (targetPos.position - a.transform.position).normalized;
            checkForNeightBours(a);
            calculateSeparation(a);
            calculateAlignment(a);
            calculateCohesion(a);
            calculateWander(a);
            a.updateAgent();
            a.neightbours.Clear();
            a.velocity = Vector3.zero;
        }
    }

    void checkForNeightBours(Agent a)
    {
        Collider[] checks = Physics.OverlapSphere(a.transform.position, agentRadius);
        foreach (Collider c in checks)
        {
            a.neightbours.Add(c.GetComponent<Agent>());
            //a.neightbours.Remove(a.neightbours[0]);
        }
    }

    void calculateSeparation(Agent a)
    {
      
            Vector3 separationVector = Vector3.zero;
            foreach(Agent neightbour in a.neightbours)
            {
                float distance = Vector3.Distance(a.transform.position, neightbour.transform.position);
            //distance /= agentRadius;
            //distance = (1 - distance);
            if (distance > 0.0f)
                separationVector += ((a.transform.position - neightbour.transform.position).normalized / distance) * separationRatio;
                
                
            }

        a.addForce(separationVector/a.neightbours.Count);
    }

    void calculateCohesion(Agent a)
    {
        Vector3 cohesionVector = Vector3.zero;

        Vector3 centralPoint = Vector3.zero;
        foreach (Agent neightbour in a.neightbours)
        {
            centralPoint += neightbour.transform.position;
        }
        centralPoint /= a.neightbours.Count;
        a.addForce((centralPoint-a.transform.position) * cohesionRatio);

    }

    void calculateAlignment(Agent a)
    {
        Vector3 alignmentVector = Vector3.zero;
        foreach (Agent neightbour in a.neightbours)
        {
            alignmentVector += neightbour.velocity;
        }
        alignmentVector /= a.neightbours.Count;
        a.addForce((alignmentVector) * alignmentRatio);
    }


    void collisionAvoidance(Agent a)
    {
    /*
     *  Collision Detection
     *  Ahead Vector
     * avoidance_force = ahead - obstacle_center
        avoidance_force = normalize(avoidance_force) * MAX_AVOID_FORCE
     */
  //  https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-seek--gamedev-849
    }
    void calculateWander(Agent a)
    {
        Vector3 wanderForce = Vector3.zero;
        Vector3 circleCenter = a.velocity.normalized;
        circleCenter *= a.wanderCircleDist;
        
        Vector3 displacement = Quaternion.Euler(0,a.wanderAngle,0) * Vector3.forward;
        
        displacement *= a.circleRadius;

        a.wanderAngle += (Random.value * 30.0f) - (30.0f * .5f);

        wanderForce += circleCenter + displacement;

        a.addForce(wanderForce);
    }
}

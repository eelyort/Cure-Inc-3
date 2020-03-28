using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	//Variables
	LinkedList<Node> nodeList = new LinkedList<Node>();
	LinkedListNode<Node> firstNode;
	LinkedListNode<Node> currentNode;
	
	int difficulty = 1;
	int enemySpawnRate = 1;
	int playerSpawnRate = 1;
	
	
	
	
	
	// Start is called before the first frame update
    void Start()
    {
        //Sets up first node and currentNode variable for iteration
		firstNode = nodeList.First;
		currentNode = firstNode;
		
		//TODO: Set up variable values depending on difficulty
		
    }

    // Update is called once per frame
    void Update()
    {
		while(currentNode != null){
			currentNode.Value.tick();
			currentNode = currentNode.Next;
		}
		
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    //Node variables
    // LinkedList<Node> nodeList = new LinkedList<Node>();
    Node[] nodeList = new Node[11];
	// LinkedListNode<Node> firstNode;
	// LinkedListNode<Node> currentNode;

    // set in editor so can find everything u need
    public GameObject canvas;
    // set in Start() assuming above is filled
	GameObject scoreText;
    GameObject freeWhiteBloodCellText;
    GameObject sectionText;
    GameObject infectedSectionsText;
	
	bool paused = false;

    //Settings settings;
	
	ulong freeVirusStart; 
	uint whiteBloodStart; 
	uint bodyCells;
	uint infectBodyStart;
	double deadVirusperWhiteBlood;
	double deadWhiteBloodperDeadVirus;
	double deadInfectedCellsperVirus;
	double infectedCellsperVirus;
	double virusesPerInfectedCell;
	double chanceICbursts;
	double spreadPerVirus;
	double whiteBloodResistance;
	int breakEvenPoint;
	
	long totalFreeViruses;
	int totalWhiteBloodCount;
	int totalInfectedWhiteBloodCells;
	int totalUninfectedBodyCells;
	int totalInfectedBodyCells;
	int totalOrignalBodyCellCount;

	int difficulty = 1;
	// int enemySpawnRate = 1;
	int playerSpawnRate;
	
	int freeWhiteBloodCells = 0;
	long tickCount = 0;

    // x ticks per second
    double ticksPerSecond = 0.5;
    float timeLast;

    // which zone is currently selected, -1 is none
    int selected = -1;

    // infected sections
    HashSet<int> infectedSects = new HashSet<int>();


    // Start is called before the first frame update
    void Start() {
        // speed control stuff, Time.time is in seconds
        timeLast = Time.time;

        freeWhiteBloodCells = 10;

        // get all the texts and whatnot needed
        // search through all children of canvas
        for (int i = 0; i < canvas.transform.childCount; i++) {
            GameObject curr = canvas.transform.GetChild(i).gameObject;
            if (curr.name == "TRText") {
                scoreText = curr;
            }
            else if(curr.name == "Free WBC Count Text") {
                freeWhiteBloodCellText = curr;
            }
            else if (curr.name == "Section Text") {
                sectionText = curr;
                Debug.Log("sectionText set to: " + sectionText);
            }
            else if (curr.name == "InfectedSections") {
                infectedSectionsText = curr;
            }
            Debug.Log("searching: " + curr.name);
            // else if... for other needed values
        }

        GameSettings newbieSettings = new GameSettings {
            // TODO
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000,

            startInfectedNodes = 2,

            playerSpawnRate = 10
        };
        GameSettings casualSettings = new GameSettings {
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000,

            startInfectedNodes = 2,

            playerSpawnRate = 15
        };
        GameSettings insaneSettings = new GameSettings {
            // TODO
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000,

            startInfectedNodes = 2,

            playerSpawnRate = 10
        };


        GameSettings[] settings = new GameSettings[3] {newbieSettings, casualSettings, insaneSettings};

        difficulty = GlobalStaticVariables.getDiff();
        playerSpawnRate = settings[difficulty].playerSpawnRate;

        // randomly generate which nodes start affected
        HashSet<int> startInfected = new HashSet<int>();
        while(startInfected.Count < settings[difficulty].startInfectedNodes) {
            int rand = (int)Mathf.Min(10.1f, Random.Range(0.0f, (float)nodeList.Length));
            // Debug.Log(rand);
            if (!startInfected.Contains(rand)) {
                startInfected.Add(rand);
            }
        }

        // DEBUG --------------------------------
        /*
        Debug.Log("startInfected: " + startInfected);
        foreach(int a in startInfected) {
            Debug.Log("a: " + a);
        }
        */
        // --------------------------------------

        //Create nodes
        for(int i = 0; i < 11; i++) {
            Node temp = new Node(settings[difficulty], startInfected.Contains(i));
            nodeList[i] = temp;
        }
        /*
        Node node1 = new Node(settings[diff]);
		Node node2 = new Node(settings[diff]);
        Node node3 = new Node(settings[diff]);
        Node node4 = new Node(settings[diff]);
        Node node5 = new Node(settings[diff]);
        Node node6 = new Node(settings[diff]);
        Node node7 = new Node(settings[diff]);
        Node node8 = new Node(settings[diff]);
        Node node9 = new Node(settings[diff]);
        Node node10 = new Node(settings[diff]);
        Node node11 = new Node(settings[diff]);

        //Adds nodes to nodeList
        nodeList.AddLast(node1);
		nodeList.AddLast(node2);
		nodeList.AddLast(node3);
		nodeList.AddLast(node4);
		nodeList.AddLast(node5);
		nodeList.AddLast(node6);
		nodeList.AddLast(node7);
		nodeList.AddLast(node8);
		nodeList.AddLast(node9);
		nodeList.AddLast(node10);
		nodeList.AddLast(node11);
        
		
		// firstNode = nodeList.First;
		LinkedListNode<Node> currentNode = nodeList.First;
        */
		
		//Add adjacent nodes for node1
		nodeList[0].adjacents.AddLast(nodeList[1]);
		nodeList[0].adjacents.AddLast(nodeList[2]);
		
		//Add adjacent nodes for node2
		nodeList[1].adjacents.AddLast(nodeList[0]);
		nodeList[1].adjacents.AddLast(nodeList[2]);
		
		//Add adjacent nodes for node3
		nodeList[2].adjacents.AddLast(nodeList[0]);
		nodeList[2].adjacents.AddLast(nodeList[1]);
		nodeList[2].adjacents.AddLast(nodeList[3]);
		
		//Add adjacent nodes for node4
		nodeList[3].adjacents.AddLast(nodeList[2]);
		nodeList[3].adjacents.AddLast(nodeList[4]);
		nodeList[3].adjacents.AddLast(nodeList[5]);
		
		//Add adjacent nodes for node5
		nodeList[4].adjacents.AddLast(nodeList[3]);
		nodeList[4].adjacents.AddLast(nodeList[5]);
		
		//Add adjacent nodes for node6
		nodeList[5].adjacents.AddLast(nodeList[3]);
		nodeList[5].adjacents.AddLast(nodeList[4]);
		
		//Add adjacent nodes for node7
		nodeList[6].adjacents.AddLast(nodeList[7]);
		nodeList[6].adjacents.AddLast(nodeList[8]);
		
		//Add adjacent nodes for node8
		nodeList[7].adjacents.AddLast(nodeList[6]);
		nodeList[7].adjacents.AddLast(nodeList[8]);
		
		//Add adjacent nodes for node9
		nodeList[8].adjacents.AddLast(nodeList[6]);
		nodeList[8].adjacents.AddLast(nodeList[7]);
		nodeList[8].adjacents.AddLast(nodeList[9]);
		nodeList[8].adjacents.AddLast(nodeList[10]);
		
		//Add adjacent nodes for node10
		nodeList[9].adjacents.AddLast(nodeList[8]);
		nodeList[9].adjacents.AddLast(nodeList[10]);
		
		//Add adjacent nodes for node11
		nodeList[10].adjacents.AddLast(nodeList[8]);
		nodeList[10].adjacents.AddLast(nodeList[9]);
    }
    public void alertNotHidden(int index) {
        Debug.Log("node unhidden: " + index);
    }
    public  bool changeSelected(int val) {
        if(val <= 0 || val > nodeList.Length) {
            selected = -1;
            return false;
        }
        selected = val - 1;
        return true;
    }
    void allocate(int sect, float percent) {
        int allocated = Mathf.Min(freeWhiteBloodCells, Mathf.CeilToInt(freeWhiteBloodCells * percent));
        nodeList[sect].changeWhiteBloodCount(allocated);
        freeWhiteBloodCells -= allocated;
    }
    // function which runs the game ticks, here for more control over tick rate
    void tick() {
        // Debugging ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /*
        Debug.Log("tick: " + tickCount);
        string format = "  {0,-2} | {1,-16} | {2,-16} | {3,-16} | {4,-16} | {5,-16} | {6,-16}";
        */
        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        freeWhiteBloodCells += playerSpawnRate;
        tickCount++;

        //Temp variables for summing up node information
        long tempFreeViruses = 0;
        int tempWhiteBloodCount = 0;
        int tempInfectedWhiteBloodCells = 0;
        int tempUninfectedBodyCells = 0;
        int tempInfectedBodyCells = 0;
        int tempOrignalBodyCellCount = 0;

        //Iterates through linkedlist of nodes
        for(int i = 0; i < nodeList.Length; i++) {
            bool unhidden = false;
            nodeList[i].tick(out unhidden);

            if (unhidden) {
                alertNotHidden(i);
            }

            //Sums up information about each nodes
            tempFreeViruses += nodeList[i].getFreeViruses();
            tempWhiteBloodCount += nodeList[i].getWhiteBloodCount();
            tempInfectedWhiteBloodCells += nodeList[i].getInfectedWhiteBloodCells();
            tempUninfectedBodyCells += nodeList[i].getUninfectedBodyCells();
            tempInfectedBodyCells += nodeList[i].getInfectedBodyCells();
            tempOrignalBodyCellCount += nodeList[i].getOriginalCellCount();

            int tempNumVir = (int)nodeList[i].getFreeViruses() + nodeList[i].getInfectedWhiteBloodCells() + nodeList[i].getInfectedBodyCells();
            if(tempNumVir == 0) {
                if (infectedSects.Contains(i)) {
                    infectedSects.Remove(i);
                }
            }
            else {
                infectedSects.Add(i);
            }
        }

        totalFreeViruses = tempFreeViruses;
        totalWhiteBloodCount = tempWhiteBloodCount;
        totalInfectedWhiteBloodCells = tempInfectedWhiteBloodCells;
        totalUninfectedBodyCells = tempUninfectedBodyCells;
        totalInfectedBodyCells = tempInfectedBodyCells;
        totalOrignalBodyCellCount = tempOrignalBodyCellCount;

        /*
        scoreText.GetComponent<Text>().text = "SCORE: " + getScore() + "\nFREE WHITE BLOOD CELLS REMAINING: " + freeWhiteBloodCells;

        
        freeWhiteBloodCellText.GetComponent<Text>().text = ("Free White Blood Cells: " + freeWhiteBloodCells);
        freeWhiteBloodCellText.GetComponent<Text>().text = ("Reeee");
        */
    }
    // Update is called once per frame
    void Update(){
        // Debug.Log("sectionText: " + sectionText);
        // update text fields
        sectionText.GetComponent<Text>().text = "Section Selected: " + ((selected != -1) ? ("" + selected) : "None") + 
            "\nFree Viruses: " + ((selected != -1) ? ("" + nodeList[selected].getFreeViruses()) : "N/A") +
            "\nWhite Blood Cells: " + ((selected != -1) ? ("" + nodeList[selected].getWhiteBloodCount()) : "N/A") +
            "\nInfected White Blood Cells: " + ((selected != -1) ? ("" + nodeList[selected].getInfectedWhiteBloodCells()) : "N/A") +
            "\nInfected Body Cells: " + ((selected != -1) ? ("" + nodeList[selected].getInfectedBodyCells()) : "N/A");

        string temp = "Infected Sections\n";
        foreach(int a in infectedSects) {
            temp += a + ", ";
        }
        infectedSectionsText.GetComponent<Text>().text = temp;

        // key input
        if (selected != -1) {
            /*
            if (Input.GetButtonDown("q")) {
                allocate(selected, .25f);
            }
            if (Input.GetButtonDown("w")) {
                allocate(selected, .50f);
            }
            if (Input.GetButtonDown("e")) {
                allocate(selected, .75f);
            }
            if (Input.GetButtonDown("r")) {
                allocate(selected, 1f);
            }
            */
            if (Input.GetMouseButton(2)) {
                allocate(selected, 0.25f);
            }
            if (Input.GetMouseButton(1)) {
                allocate(selected, 0.25f);
            }
        }

		if(!paused){	//Checks if unpaused
            // moved code to tick() for control over ticks per sec
            // control tick rate
            float deltaTime = Time.time - timeLast;
            float secondsPerTick = 1 / (float)ticksPerSecond;
            if (deltaTime > secondsPerTick) {
                timeLast = timeLast + secondsPerTick;
                tick();
            }
		}
    }
	
	public long getFreeViruses(){
        // Debug.Log("getFV returning: " + (long)totalFreeViruses);
		return (long)totalFreeViruses;
	}
	
	public int getWhiteBloodCount(){
		return (int)totalWhiteBloodCount;
	}
	
	public int getInfectedWhiteBloodCells(){
		return (int)totalInfectedWhiteBloodCells;
	}
	
	public int getUninfectedBodyCells(){
		return (int)totalUninfectedBodyCells;
	}
	
	public int getInfectedBodyCells(){
		return (int)totalInfectedBodyCells;
	}
	
	public int getOrignalBodyCellCount(){
		return (int)totalOrignalBodyCellCount;
	}

    public int getTotalViruses() {
        return (int)getFreeViruses() + getInfectedWhiteBloodCells() + getInfectedBodyCells();
    }
	
	public long getTickCount(){
		return tickCount;
	}

	public long getScore()
	{
        if(getTotalViruses() == 0) {
            Debug.Log("getScore() called: getTotalViruses() is 0, returning 0");
            return 0;
        }
		long a = getTickCount();
		return (long)(((double)a / getTotalViruses()) * getOrignalBodyCellCount());
	}
	
    public double getHealth() {
        // returns a percentage of health
        return (double)getUninfectedBodyCells() / (double)getOrignalBodyCellCount();
    }

	public int getFreeWBC()
	{
		return freeWhiteBloodCells;
	}

	public void pauseGame(){
		paused = true;
	}
	
	public void unpauseGame(){
		paused = false;
	}
	
}

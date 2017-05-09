using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<int[]> route(int[,] map, int[] start, int[] end)
    {
        List<int[]> ans = new List<int[]>();
        int[,] direction = new int[map.GetLength(0), map.GetLength(1)];
        float[,] value = new float[map.GetLength(0), map.GetLength(1)];
        value[start[0], start[1]] = 1; //make sure that one can start at the start location

        // initialize queue for Dijsktra points
        List<float[]> queue = new List<float[]>();
        queue.Add(new float[3] { start[0], start[1], 1 });
        /*
         * catch some misuses
         * either start is equal to end, or end is out of bounds
         * in those cases, don't move the unit
         */
        if (start[0] == end[0] && start[1] == end[1])
        {
            Debug.Log("start equals end");
            ans.Add(start);
            return ans;
        }

        if (end[0] < 0 || end[0] >= map.GetLength(0) || end[1] < 0 || end[1] >= map.GetLength(1))
        {
            Debug.Log("endpoint out of bounds");
            ans.Add(start);
            return ans;
        }


        // main recursive while loop
        int j = 0;
        while (queue.Count > 0 && j < 400)
        {
            dijkstra_iteration(direction, value, queue, 1);
            queue.RemoveAt(0);
            if (queue[0][0] == end[0] && queue[0][1] == end[1])
            {

                return findRoute(direction, start, end);
            }
            j++;
        }
        ans.Add(start);
        return ans;
    }

    /*
     * One iteration of the Dijkstra Hexagon Algorithm. 
     * direction: stores the way back to the start
     * value: stores the duration of tarveling from the start to the current tile
     * queue: stores the list of tiles that need to be expanded, sorted
     * penalty: contains the penalty of traveling over the current tile
     */
    static void dijkstra_iteration(int[,] direction, float[,] value, List<float[]> queue, float penalty)
    {
        //Way to find the neighbors in a hexagon formation. This is different for odd or even hexagons
        int[,] neighbors = new int[6, 2] {      { 0, 1 },
                                                    { 1, 0 },
                                                    { 1, -1 },
                                                    { 0, -1 },
                                                    { -1, -1 },
                                                    { -1, 0 }};

        if (queue[0][0] % 2 != 0) // if uneven, the route to neighbors changes
        {
            neighbors = new int[6, 2]{           { 0, 1 },
                                                    { 1, 1 },
                                                    { 1, 0 },
                                                    { 0, -1 },
                                                    { -1, 0 },
                                                    { -1, 1 }};
        }

        int x, y;
        for (int i = 0; i < 6; i++)
        {
            x = (int)queue[0][0] + neighbors[i, 0];
            y = (int)queue[0][1] + neighbors[i, 1];

            if (x < value.GetLength(0) && 0 <= x && y < value.GetLength(1) && 0 <= y)
            {
                // check whether a new best route to the current position is found
                if (value[x, y] == 0 || value[x, y] > queue[0][2])
                {
                    value[x, y] = queue[0][2]; // update the duration of the shortest route to this poitn
                    direction[x, y] = i; // update the fastest way back to the start
                    insertQueue(new float[3] { x, y, queue[0][2] + penalty }, queue); // add this entry in the queue to expand in a later stage
                }
            }
        }
        return;
    }

    static void insertQueue(float[] entry, List<float[]> queue)
    {
        int i = 0;
        //Console.WriteLine(queue[0][2]);
        while (i < queue.Count)
        {
            if (entry[2] < queue[i][2])
                break;

            //Console.WriteLine("new iter: (" + queue[0][0] + "," + queue[0][1] + ")");
            i++;
        }
        queue.Insert(i, entry);
        //Console.WriteLine("new iter: (" + queue[0][0] + "," + queue[0][1] + ")");
        return;
    }

    static List<int[]> findRoute(int[,] direction, int[] start, int[] end)
    {
        List<int[]> ans = new List<int[]>();
        ans.Add(new int[2] { end[0], end[1] });
        int[,] neighbors_even = new int[6, 2] { { 0, 1 },
                                                    { 1, 0 },
                                                    { 1, -1 },
                                                    { 0, -1 },
                                                    { -1, -1 },
                                                    { -1, 0 }};
        int[,] neighbors_odd = new int[6, 2] {  { 0, 1 },
                                                    { 1, 1 },
                                                    { 1, 0 },
                                                    { 0, -1 },
                                                    { -1, 0 },
                                                    { -1, 1 } };
        int[] pos = new int[2] { end[0], end[1] };
        int[] next_pos = new int[2];
        while (true)
        {
            //trace back steps back to start 1 by 1. even and odd have different neighbor routes
            if (pos[0] % 2 == 1)
            {
                next_pos[0] = pos[0] - neighbors_even[direction[pos[0], pos[1]], 0];
                next_pos[1] = pos[1] - neighbors_even[direction[pos[0], pos[1]], 1];
            }
            else
            {
                next_pos[0] = pos[0] - neighbors_odd[direction[pos[0], pos[1]], 0];
                next_pos[1] = pos[1] - neighbors_odd[direction[pos[0], pos[1]], 1];
            }
            // stop if the next step back brings us to the start
            if (next_pos[0] == start[0] && next_pos[1] == start[1])
                break;


            ans.Insert(0, new int[2] { next_pos[0], next_pos[1] });
            pos[0] = next_pos[0];
            pos[1] = next_pos[1];

        }
        // we now know that the current position is the step we are looking for 
        return ans;
    }
}

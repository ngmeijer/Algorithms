using GXPEngine;
using System;
using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    private List<Path> allPaths = new List<Path>();
    private Path currentPath = new Path();
    private List<Node> visitedNodes = new List<Node>();
    private List<Node> toDo = new List<Node>();
    private Node startNode;
    private Node endNode;
    private Node lastNode;
    private bool foundFinalNode;
    private bool foundNextNode = false;
    private int iterationCount = 0;

    public RecursivePathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        startNode = pFrom;
        endNode = pTo;

        Console.WriteLine($"1.A Starting with node {startNode.id}");
        lastNode = null;
        recursivelyLoopThroughConnections(startNode);

        Console.WriteLine("\n\n---------\nNodes in path:\n:");
        foreach (Node node in currentPath.nodes)
        {
            Console.WriteLine($"{node.id}");
        }

        return currentPath.nodes;
    }


    private void recursivelyLoopThroughConnections(Node pNode)
    {
        if (foundFinalNode) return;

        foundNextNode = false;

        iterationCount++;
        if (pNode == endNode)
        {
            Console.WriteLine($"\n--------- FOUND FINAL NODE {pNode.id} -----------\n");
            foundFinalNode = true;
            currentPath.nodes.Add(pNode);
            visitedNodes.Add(pNode);
            return;
        }

        Console.WriteLine($"Iteration count: {iterationCount}");

        Console.WriteLine($"\n\n1.A Adding {pNode.id} to CurrentPath and to VisitedNodes.");
        currentPath.nodes.Add(pNode);
        visitedNodes.Add(pNode);

        if (lastNode != null) Console.WriteLine($"1.B Last node: {lastNode.id}");
        Console.WriteLine($"1.C   Looping over node {pNode.id}'s connections:");
        bool visited = false;
        foreach (Node connection in pNode.connections)
        {
            if (foundFinalNode) break;
      
            visited = visitedNodes.Contains(connection);
            Console.WriteLine($"            Now checking {connection.id}. \n            Already visited?: {visited}");
            if (visited)
            {
                Console.WriteLine($"        2.B Moving on to next connection of {pNode.id}.");
                continue; 
            }

            lastNode = pNode;
            foundNextNode = true;
            if(!foundFinalNode) recursivelyLoopThroughConnections(connection);
        }

        if (!foundNextNode && !foundFinalNode && lastNode != null)
        {
            Console.WriteLine($"------- Recursive call from node {pNode.id} ended. No path found. Moving back to node {lastNode.id}. Removed {pNode.id} from CurrentPath. Removed {lastNode.id} from VisitedNodes.\n\n");
            currentPath.nodes.Remove(pNode);
            visitedNodes.Remove(lastNode);
            recursivelyLoopThroughConnections(lastNode);
        }
    }

    private List<Node> getShortestPath()
    {
        Path currentShortestPath = null;
        int lastNodeCount = int.MaxValue;

        foreach (Path path in allPaths)
        {
            if(path.nodes.Count < lastNodeCount)
            {
                currentShortestPath = path;
            }
        }

        return currentShortestPath.nodes;
    }
}

public class Path
{
    public List<Node> nodes = new List<Node>();
}
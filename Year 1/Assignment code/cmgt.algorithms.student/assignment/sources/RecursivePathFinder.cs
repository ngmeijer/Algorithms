using GXPEngine;
using System;
using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    private List<Node> visitedNodes = new List<Node>();
    private List<Node> currentNodesInPath = new List<Node>();
    private Node endNode;
    private Node lastNode;
    private bool foundTargetNode;
    private bool foundNextNode = false;
    private int iterationCount = 0;
    private bool finishLoop;

    public RecursivePathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        Node startNode = pFrom;
        endNode = pTo;

        iterationCount = 0;

        Console.WriteLine($"1.A Starting with node {startNode.id}");
        lastNode = null;

        currentNodesInPath.Add(startNode);
        visitedNodes.Add(startNode);
        findPath(startNode);

        List<Node> shortestPath = getShortestPath();
        Console.WriteLine("\n\n---------\nNodes in path:\n");
        foreach (Node node in shortestPath)
        {
            Console.WriteLine($"{node.id}");
        }

        return shortestPath;
    }

    private void findPath(Node pNode)
    {
        //Has the iteration count reached its max? STOP RUNNING.
        if (finishLoop) return;
        printConnections(pNode);

        foundNextNode = false;

        //Adding node to path & already visited nodes.
        Console.WriteLine($"Adding {pNode.id} to path & visitedNodes.");
        lastNode = currentNodesInPath[currentNodesInPath.Count - 1];
        if (!currentNodesInPath.Contains(pNode)) currentNodesInPath.Add(pNode);
        if (!visitedNodes.Contains(pNode)) visitedNodes.Add(pNode);

        foundTargetNode = checkIfIsTargetNode(pNode);

        if (foundTargetNode)
        {
            moveToLastNode(pNode);
            return;
        }

        //Check connections
        loopOverConnections(pNode);

        //If all children have been visited but a next node has not been found, remove node from path. Retrace path to last node and loopOverConnections().
        if (!foundNextNode)
        {
            Console.WriteLine($"Haven't found an unvisited connection for {pNode.id}. Returning to last node {lastNode.id}.");
            moveToLastNode(pNode);
        }
    }

    private void printConnections(Node pNode)
    {
        Console.WriteLine($"\nInvestigating node {pNode.id}");
        foreach (Node connection in pNode.connections)
        {
            Console.WriteLine($"- connection {connection.id}. Visited? {visitedNodes.Contains(connection)}");
        }
    }

    private bool checkIfIsTargetNode(Node pNode)
    {
        //If final node is found, save path. Copy path to a new instance.
        if (pNode == endNode)
        {
            Console.WriteLine($"\n--------- FOUND FINAL NODE {pNode.id} -----------\n" +
                                $"--------- Saving path.        -----------");
            currentNodesInPath.Add(pNode);
            visitedNodes.Add(pNode);

            savePath(currentNodesInPath);
            currentNodesInPath = new List<Node>(currentNodesInPath);

            return true;
        }

        return false;
    }

    private void loopOverConnections(Node pNode)
    {
        foreach (Node connection in pNode.connections)
        {
            if (finishLoop) break;

            if (connection != endNode)
            {
                if (visitedNodes.Contains(connection) || pNode.alreadyVisited.Contains(connection))
                {
                    Console.WriteLine($"Skipping connection {connection.id}. Already visited.");
                    continue;
                }
            }
            else Console.WriteLine($"");

            pNode.alreadyVisited.Add(connection);
            foundNextNode = true;
            Console.WriteLine($"Selected node {connection.id} to check out.");
            findPath(connection);
        }
    }

    private void moveToLastNode(Node pNode)
    {
        if(pNode == lastNode)
        {
            int indexOfpNode = currentNodesInPath.IndexOf(pNode);
            Console.WriteLine($"Last node {lastNode.id} is the same as pNode. Is pNode {pNode.id} in path? {currentNodesInPath.Contains(pNode)}. Index of pNode in path: {indexOfpNode}");
        }
        //If the algorithm has run less than X times, find previous node and call recursive.
        if (iterationCount < AlgorithmsAssignment.MAX_PATH_ITERATION_COUNT)
        {
            iterationCount++;

            Console.WriteLine($"Current node: {pNode.id} (removing from path). Retracing further to {lastNode.id}");
            currentNodesInPath.RemoveAt(currentNodesInPath.Count - 1);
            loopOverConnections(lastNode);
        }
        else finishLoop = true;
    }
}
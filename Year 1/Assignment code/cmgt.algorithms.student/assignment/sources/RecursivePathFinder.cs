using GXPEngine;
using System;
using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    private List<Node> visitedNodes = new List<Node>();
    private List<Node> toDo = new List<Node>();
    private List<Node> currentNodesInPath = new List<Node>();
    private Node startNode;
    private Node endNode;
    private Node lastNode;
    private bool foundFinalNode;
    private bool foundNextNode = false;
    private int iterationCount = 0;
    private int indexOfCurrentNode;
    private bool finishLoop;

    public RecursivePathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        startNode = pFrom;
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

        checkIfIsTargetNode(pNode);

        //Adding node to path & already visited nodes.
        if (!currentNodesInPath.Contains(pNode)) currentNodesInPath.Add(pNode);
        if (!visitedNodes.Contains(pNode)) visitedNodes.Add(pNode);

        //If the path contains more than 1 node, set lastNode to the node added BEFORE currentNode.
        if (currentNodesInPath.Count > 1) lastNode = currentNodesInPath[currentNodesInPath.Count - 2];

        //Check connection so
        loopOverConnections(pNode);

        //If all children have been visited but a next node has not been found, remove node from path. Retrace path to last node and loopOverConnections().

        if (!foundNextNode && !finishLoop && lastNode != null)
        {
            currentNodesInPath.Remove(pNode);
            loopOverConnections(lastNode);
        }
    }

    private static void printConnections(Node pNode)
    {
        Console.WriteLine($"\nCurrently investigating {pNode.id}");
        Console.WriteLine($"Node has connections:");
        foreach (Node connection in pNode.connections)
        {
            Console.WriteLine($"{connection.id}");
        }
    }

    private void checkIfIsTargetNode(Node pNode)
    {
        //If final node is found, save path. Copy path to a new instance.
        if (pNode == endNode)
        {
            Console.WriteLine($"\n--------- FOUND FINAL NODE {pNode.id} -----------\n");
            currentNodesInPath.Add(pNode);
            visitedNodes.Add(pNode);
            foundFinalNode = true;

            savePath(currentNodesInPath);
            currentNodesInPath = new List<Node>(currentNodesInPath);
        }
    }

    private void loopOverConnections(Node pNode)
    {
        foreach (Node connection in pNode.connections)
        {
            if (finishLoop) break;

            if (visitedNodes.Contains(connection) || pNode.alreadyVisited.Contains(connection))
            {
                continue;
            }

            pNode.alreadyVisited.Add(connection);
            foundNextNode = true;
            findPath(connection);
        }
    }



    private void moveToLastNode(Node pNode)
    {
        //If the algorithm has run less than X times, find previous node and call recursive.
        if (iterationCount < AlgorithmsAssignment.MAX_PATH_ITERATION_COUNT)
        {
            iterationCount++;

            int currentNodeIndex = currentNodesInPath.IndexOf(pNode);
            Node previousNode = null;
            if (currentNodeIndex < 0)
            {
                Console.WriteLine($"Couldn't find previous node of {pNode.id} in currentNodesInPath.");
                return;
            }
            previousNode = currentNodesInPath[currentNodeIndex - 1];
            Console.WriteLine($"Current node: {pNode.id}. Previous node: {previousNode.id}");
            currentNodesInPath.Remove(pNode);
            findPath(previousNode);
        }
        else finishLoop = true;
    }
}
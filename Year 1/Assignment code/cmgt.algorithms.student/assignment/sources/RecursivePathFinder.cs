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
    private Node currentNode;
    private bool foundFinalNode;
    private bool foundNextNode = false;
    private int iterationCount = 0;
    private int indexOfCurrentNode;

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
        foundNextNode = false;
        lastNode = currentNodesInPath[currentNodesInPath.Count - 1];
        currentNode = pNode;

        //If final node is found, save path.
        if (currentNode == endNode)
        {
            Console.WriteLine($"\n--------- FOUND FINAL NODE {currentNode.id} -----------\n");
            currentNodesInPath.Add(pNode);
            visitedNodes.Add(pNode);
            foundFinalNode = true;

            savePath(currentNodesInPath);
            currentNodesInPath = new List<Node>(currentNodesInPath);
        }

        //If final node is found.
        if (foundFinalNode)
        {
            moveToLastNode();
        }

        //Adding node to path & already visited nodes.
        if (!currentNodesInPath.Contains(pNode)) currentNodesInPath.Add(pNode);
        if (!visitedNodes.Contains(pNode)) visitedNodes.Add(pNode);

        loopOverConnections(currentNode);

        if (!foundNextNode && !foundFinalNode && lastNode != null)
        {
            currentNodesInPath.Remove(pNode);
            visitedNodes.Remove(lastNode);
        }
    }

    private void loopOverConnections(Node pNode)
    {
        foreach (Node connection in pNode.connections)
        {
            if (visitedNodes.Contains(connection) || pNode.alreadyVisited.Contains(connection))
            {
                continue;
            }

            pNode.alreadyVisited.Add(connection);
            foundNextNode = true;
            findPath(connection);
        }
    }

    private void moveToLastNode()
    {
        //If the algorithm has run less than X times, find previous node and call recursive.
        if (iterationCount < AlgorithmsAssignment.MAX_PATH_ITERATION_COUNT)
        {
            iterationCount++;

            
            int currentNodeIndex = currentNodesInPath.IndexOf(currentNode);
            Console.WriteLine(currentNodeIndex);
            if (currentNodeIndex == -1)
            {
                Console.WriteLine($"Couldn't find node {currentNode.id} in currentNodesInPath.");
                return;
            }
            else Console.WriteLine($"Found {currentNode.id} at index {currentNodeIndex}!");
            Node previousNode = currentNodesInPath[currentNodeIndex - 1];
            Console.WriteLine($"Current node: {currentNode.id}. Previous node: {previousNode.id}");
            currentNodesInPath.Remove(currentNode);
            findPath(previousNode);
        }
    }
}
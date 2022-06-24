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
        currentNode = pNode;

        //If final node is found, save path.
        if (pNode == endNode)
        {
            Console.WriteLine($"\n--------- FOUND FINAL NODE {pNode.id} -----------\n");
            currentNodesInPath.Add(pNode);
            visitedNodes.Add(pNode);
            foundFinalNode = true;

            savePath(currentNodesInPath);
        }

        //If final node is found AND the algorithm has run less than X times, rerun the algorithm passing lastNode.
        moveToLastNode();

        //Adding node to path & already visited nodes.
        if (!currentNodesInPath.Contains(pNode)) currentNodesInPath.Add(pNode);
        if (!visitedNodes.Contains(pNode)) visitedNodes.Add(pNode);

        //Assigning LastNode to the LAST node in CurrentNodesInPath , so when we have to retrace the next time the algorithm is run, it refers to this node.
        lastNode = pNode;
        loopOverConnections(pNode);

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
            if (foundFinalNode) break;

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
        //If final node is found AND the algorithm has run less than X times, rerun the algorithm passing lastNode.
        if (foundFinalNode && iterationCount < AlgorithmsAssignment.MAX_PATH_ITERATION_COUNT)
        {
            iterationCount++;

            indexOfCurrentNode = currentNodesInPath.IndexOf(currentNode);
            if (indexOfCurrentNode - 1 < 0) indexOfCurrentNode = 1;
            Node tempLastNode = currentNodesInPath[indexOfCurrentNode - 1];
            Console.WriteLine($"Last node after finding final node: {tempLastNode.id}");

            findPath(tempLastNode);
            return;
        }
    }
}
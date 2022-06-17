using GXPEngine;
using System;
using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    private List<Node> path = new List<Node>();
    private List<Node> visitedNodes = new List<Node>();
    private List<Node> toDo = new List<Node>();
    private Node startNode;
    private Node endNode;
    private Node currentNode;
    private Node lastNode;

    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        startNode = pFrom;
        endNode = pTo;
        currentNode = startNode;

        path.Add(currentNode);
        visitedNodes.Add(currentNode);

        recursivelyLoopThroughChildren();

        return path;
    }

    private void recursivelyLoopThroughChildren()
    {
        if (currentNode == endNode) return;

        foreach(Node child in currentNode.connections)
        {
            if (currentNode.alreadyVisited.Contains(child)) continue;

            Console.WriteLine($"Visiting {child.id}");

            currentNode.alreadyVisited.Add(child);
            currentNode.parentNode = lastNode;
            lastNode = currentNode;

            path.Add(currentNode);
            currentNode = child;

            recursivelyLoopThroughChildren();
        }
    }
}
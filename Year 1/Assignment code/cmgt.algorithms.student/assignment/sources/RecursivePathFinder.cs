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

    public RecursivePathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        startNode = pFrom;
        endNode = pTo;

        Console.WriteLine($"1.A Starting with node {startNode.id}");
        recursivelyLoopThroughChildren(startNode);

        //List<Node> shortestPath = getShortestPath();

        Console.WriteLine("Nodes in path IN ORDER:");
        foreach (Node node in currentPath.nodes)
        {
            Console.WriteLine($"{node.id}");
        }

        return currentPath.nodes;
    }

    private void findNewPath(Node pNode)
    {
        currentPath = new Path();

        recursivelyLoopThroughChildren(pNode);
    }

    private void recursivelyLoopThroughChildren(Node pNode)
    {
        if(pNode == endNode)
        {
            Console.WriteLine($"|||||FOUND FINAL NODE {pNode.id}");
            foundFinalNode = true;
            currentPath.nodes.Add(pNode);
            visitedNodes.Add(pNode);
            return;
        }

        if (visitedNodes.Contains(pNode))
        {
            Console.WriteLine($"        2.A Node {pNode.id} has already been visited. Returning to previous node.");
            return;
        }

        currentPath.nodes.Add(pNode);
        visitedNodes.Add(pNode);

        Console.WriteLine($"            3.A Moving down recursively to node {pNode}.");

        Console.WriteLine($"            3.B Node {pNode.id} has connections to node:");
        foreach (Node child in pNode.connections)
        {
            Console.WriteLine($"                {child.id}.");
        }

        foreach(Node child in pNode.connections)
        {
            if (foundFinalNode) return;

            Console.WriteLine($"            3.C Looping over connections of {pNode.id}. Current child being checked: {child.id}");

            recursivelyLoopThroughChildren(child);
        }


        Console.WriteLine($"        2.B Call from node {pNode.id} ended. No path found. Moving back up the tree.\n\n");
        lastNode = pNode;
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
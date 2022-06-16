using GXPEngine;
using System;
using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    private List<Node> currentPath = new List<Node>();
    private Dictionary<List<Node>, int> generatedPaths = new Dictionary<List<Node>, int>();

    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        int maxIterations = 4;
        int currentIteration = 0;
        Node currentNode = pFrom;
        currentPath.Add(pFrom);

        if (pFrom == pTo) return currentPath;

        while (currentIteration < maxIterations)
        {
            currentPath.Clear();
            recursivelyLoopThroughChildren(currentNode, pTo);

            generatedPaths.Add(currentPath, currentPath.Count);
            currentIteration++;
        }

        List<Node> shortestPath = selectShortestPath();
        return shortestPath;
    }

    private void recursivelyLoopThroughChildren(Node pCurrentNode, Node pTo)
    {
        foreach (Node childNode in pCurrentNode.connections)
        {
            if (currentPath.Contains(childNode)) continue;

            currentPath.Add(childNode);

            if(childNode == pTo) return;

            recursivelyLoopThroughChildren(childNode, pTo);
        }
    }

    private List<Node> selectShortestPath()
    {
        List<Node> path = null;

        return path;
    }
}
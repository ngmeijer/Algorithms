﻿using System.Collections.Generic;

internal class BreadthFirstPathFinder : PathFinder
{
    public BreadthFirstPathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> path = new List<Node>();
        Queue<Node> nodeQ = new Queue<Node>();
        List<Node> visitedNodes = new List<Node>();

        nodeQ.Enqueue(pFrom);

        findPath(pTo, nodeQ, visitedNodes);

        Node nodeBeingTraced = pTo;
        while (nodeBeingTraced != pFrom)
        {
            path.Add(nodeBeingTraced);
            nodeBeingTraced = nodeBeingTraced.cameFromNode;
        }
        path.Add(pFrom);
        path.Reverse();


        return path;
    }

    private void findPath(Node pTo, Queue<Node> nodeQ, List<Node> visitedNodes)
    {
        while (nodeQ.Count > 0)
        {
            Node currentNode = nodeQ.Peek();
            nodeQ.Dequeue();
            if (currentNode == pTo) break;

            foreach (Node connection in currentNode.connections)
            {
                if (visitedNodes.Contains(connection)) continue;

                nodeQ.Enqueue(connection);
                visitedNodes.Add(connection);
                connection.cameFromNode = currentNode;
            }
        }
    }
}
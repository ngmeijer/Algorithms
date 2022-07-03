using System;
using System.Collections.Generic;

internal class RecursivePathFinder3 : PathFinder
{
    public RecursivePathFinder3(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> historyNodes = new List<Node>();
        List<Node> path = new List<Node>();
        int depth = -1;

        //Base case.
        if (pFrom == pTo) return null;

        //Begin with pFrom.
        dfs(pFrom, pTo, depth, 0, int.MaxValue);
        retracePath(pTo, path);

        Console.WriteLine($"Nodes in final path");
        foreach (Node node in path)
        {
            Console.WriteLine($"Node {node.id}");
        }

        return path;
    }

    private void dfs(Node pNode, Node pEndNode, int pDepth, int pCurrentLength, int pShortestLength)
    {
        pDepth += 1;
        pCurrentLength += 1;

        //1st base case
        if (pCurrentLength > pShortestLength) return;

        //2nd base case
        if (pNode == pEndNode)
        {
            pShortestLength = pCurrentLength;
            return;
        }

        pNode.visited = true;

        foreach(Node connection in pNode.connections)
        {
            if (!connection.visited)
            {
                connection.cameFromNode = pNode;
                dfs(connection, pEndNode, pDepth, pCurrentLength, pShortestLength);
            }
        }
    }

    private void retracePath(Node pEndNode, List<Node> pPath)
    {
        Node currentNode = pEndNode;
        while (currentNode != null)
        {
            pPath.Add(currentNode);
            currentNode = currentNode.cameFromNode;
        }
        pPath.Reverse();
    }
}
using System;
using System.Collections.Generic;

internal class RecursivePathFinder3 : PathFinder
{
    private int shortestLength = int.MaxValue;

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
        dfs(pFrom, pTo, depth, 0);
        path = retracePath(pTo);

        Console.WriteLine($"Nodes in final path");
        foreach (Node node in path)
        {
            Console.WriteLine($"Node {node.id}");
        }

        return path;
    }

    private void dfs(Node pNode, Node pEndNode, int pDepth, int pCurrentLength)
    {
        pDepth += 1;
        pCurrentLength += 1;

        //1st base case
        if (pCurrentLength > shortestLength)
        {
            Console.WriteLine($"{indent(pDepth)}Current path is already longer than shortest path. Returning.");
            return;
        }

        Console.WriteLine($"{indent(pDepth)}Current node - {pNode.id} -");

        //2nd base case
        if (pNode == pEndNode)
        {
            Console.WriteLine($"{indent(pDepth)}Found path to node - {pNode.id} - Current length: {pCurrentLength}, shortest length: {shortestLength}");
            shortestLength = pCurrentLength;
            return;
        }

        pNode.visited = true;

        Console.WriteLine($"{indent(pDepth)}Looping through connections of node - {pNode.id} -");
        foreach (Node connection in pNode.connections)
        {
            Console.WriteLine($"{indent(pDepth)}Current connection of node {pNode.id} - {connection.id} -, already visited? {connection.visited}");
            if (connection.visited) continue;
            if (connection == pNode.cameFromNode) continue;

            //Console.WriteLine($"{indent(pDepth)}Marking current node - {connection.id} - as visited");

            Console.WriteLine($"{indent(pDepth)}Setting previous node of connection - {connection.id} - to node {pNode.id}");
            connection.cameFromNode = pNode;

            dfs(connection, pEndNode, pDepth, pCurrentLength);
        }

        Console.WriteLine($"{indent(pDepth)}Tracing back from node {pNode.id}");
        pCurrentLength -= 1;
    }

    private List<Node> retracePath(Node pEndNode)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = pEndNode;
        while (currentNode != null)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.cameFromNode;
        }
        newPath.Reverse();

        return newPath;
    }
}
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
        askChildForPath(pFrom, pTo, historyNodes, path, depth += 1);

        if(!path.Contains(pTo)) path.Clear();

        Console.WriteLine($"Nodes in final path");
        foreach (Node node in path)
        {
            Console.WriteLine($"Node {node.id}");
        }

        return path;
    }

    private bool askChildForPath(Node pNode, Node pEndNode, List<Node> pHistoryNodes, List<Node> pPath, int pDepth)
    {
        pPath.Add(pNode);
        pHistoryNodes.Add(pNode);

        Console.WriteLine($"{indent(pDepth)}Continuing path from {pNode.id}... depth: {pDepth}");

        Console.WriteLine($"{indent(pDepth)}Nodes in path... depth {pDepth}");
        foreach (Node node in pPath)
        {
            Console.WriteLine($"{indent(pDepth)}Node {node.id}");
        }

        if (pNode == pEndNode) 
        {
            Console.WriteLine($"{indent(pDepth)}Path found! Depth: {pDepth}");
            return true; 
        }

        foreach (Node connection in pNode.connections)
        {
            if (pHistoryNodes.Contains(connection)) continue;
            if (askChildForPath(connection, pEndNode, pHistoryNodes, pPath, pDepth += 1)) return true;
            else pPath.Remove(connection);
        }

        Console.WriteLine($"{indent(pDepth)}No path returned after asking {pNode.id}... depth: {pDepth}");
        return false;
    }
}
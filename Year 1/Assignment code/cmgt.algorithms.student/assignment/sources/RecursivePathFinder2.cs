using System;
using System.Collections.Generic;

internal class RecursivePathFinder2 : PathFinder
{
    public RecursivePathFinder2(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> historyNodes = new List<Node>();
        List<Node> path = new List<Node>();
        int depth = -1;

        path.Add(pFrom);

        //Base case.
        if (pFrom == pTo) return null;

        //Begin with pFrom.
        findPath(pFrom, pTo, historyNodes, path, depth += 1);

        Console.WriteLine($"Nodes in final path");
        foreach (Node node in path)
        {
            Console.WriteLine($"Node {node.id}");
        }

        return path;
    }

    private void findPath(Node pGivenNode, Node pEndNode, List<Node> pPath, List<Node> pHistory, int pDepth)
    {
        Console.WriteLine($"{indent(pDepth)}Investigating node {pGivenNode.id} at depth {pDepth}");
        if (!pPath.Contains(pGivenNode)) pPath.Add(pGivenNode);

        Console.WriteLine($"{indent(pDepth)}Current nodes in pPath at depth {pDepth}");
        string nodes = "";
        foreach (Node node in pPath)
        {
            nodes += $"{node.id},";
        }
        Console.WriteLine($"{indent(pDepth)}{nodes}");

        if (pGivenNode == pEndNode)
        {
            //Found path!
            Console.WriteLine($"{indent(pDepth)}--------FOUND pEndNode {pGivenNode.id} at depth {pDepth}--------");
            return;
        }

        //If this point is reached, we know pGivenNode is not the same as pEndNode.
        //Loop over connections of pGivenNode
        loopOverConnections(pGivenNode, pEndNode, pPath, pHistory, pDepth);
    }

    private void loopOverConnections(Node pGivenNode, Node pEndNode, List<Node> pPath, List<Node> pHistory, int pDepth)
    {
        Node selectedNode = null;
        Console.WriteLine($"{indent(pDepth)}Looping through connections of {pGivenNode.id}");
        //Foreach loop
        foreach (Node connection in pGivenNode.connections)
        {
            Console.WriteLine($"{indent(pDepth)}Checking out connection node {connection.id}");
            if (pHistory.Contains(connection)) Console.WriteLine($"{indent(pDepth)}Already visited node {connection.id}. Skipping.");
            else Console.WriteLine($"{indent(pDepth)}Selected {connection.id}. Adding to history");
            ////Check if connection exists in pHistory.
            ///If true, skip
            if (pHistory.Contains(connection)) continue;

            //////If false, add to history and run findPath(connection).
            pHistory.Add(connection);
            selectedNode = connection;
            break;
            ///Do not try to visit the other connections. 
        }

        //If a new possible node was found, increase recursion level.
        if (selectedNode != null)
        {
            selectedNode.cameFromNode = pGivenNode;
            findPath(selectedNode, pEndNode, pPath, pHistory, pDepth += 1);
        }
        //If all nodes have been visited, the path is a dead end. So, retrace to the last node in the pHistory list.
        else
        {
            Console.WriteLine($"{indent(pDepth)}No unvisited connections for {pGivenNode.id}. \n{indent(pDepth)}Removing {pGivenNode.id} from path. \n{indent(pDepth)}Tracing back to {pGivenNode.cameFromNode.id}");
            pPath.Remove(pGivenNode);

            //Trace back to the previous node visited.
            //findPath(pGivenNode.parentNode, pEndNode, pPath, pHistory, pDepth-=1);
        }
    }
}

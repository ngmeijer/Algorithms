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

        return path;
    }

    private void findPath(Node pGivenNode, Node pEndNode, List<Node> pPath, List<Node> pHistory, int pDepth)
    {
        Console.WriteLine($"{indent(pDepth)}Investigating node {pGivenNode.id} at depth {pDepth}");
        pPath.Add(pGivenNode);
        if (pGivenNode == pEndNode)
        {
            Console.WriteLine($"{indent(pDepth)}--------FOUND pEndNode at depth {pDepth}--------");
            return;
        }

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
            ////Check if connection exists in pHistory AND if exists in pNode.alreadyVisited.
            ///If true, skip
            if (pHistory.Contains(connection)) continue;

            //////If false, add to both history collections and run findPath(connection).
            pHistory.Add(connection);
            pGivenNode.alreadyVisited.Contains(connection);
            selectedNode = connection;
            break;
            ///Do not try to visit the other connections. 
        }

        //If a new possible node was found, increase recursion level.
        if (selectedNode != null)
        {
            selectedNode.parentNode = pGivenNode;
            findPath(selectedNode, pEndNode, pPath, pHistory, pDepth += 1);
        }
        //If all nodes have been visited, the path is a dead end. So, retrace to the last node in the pHistory list.
        else
        {
            Console.WriteLine($"{indent(pDepth)}No unvisited connections for {pGivenNode.id}. \nRemoving {pGivenNode.id} from path. \nTracing back to {pGivenNode.parentNode.id}");
            pPath.Remove(pGivenNode);
            findPath(pGivenNode.parentNode, pEndNode, pPath, pHistory, pDepth--);
            //Trace back to the previous node visited. Somehow figure out which one that is.
        }
    }

    private static string indent(int pDepth = 0) => "".PadLeft(pDepth * 5);
}

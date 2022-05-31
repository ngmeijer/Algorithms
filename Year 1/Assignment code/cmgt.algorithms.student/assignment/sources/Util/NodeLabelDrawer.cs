using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using RoomCreation;

/**
 * Helper class that draws nodelabels for a nodegraph.
 */
class NodeLabelDrawer : Canvas
{
	private Font _labelFont;
	private bool _showLabels = false;
	private NodeGraph _graph = null;

	public NodeLabelDrawer(NodeGraph pNodeGraph) : base(pNodeGraph.width, pNodeGraph.height)
	{
		Console.WriteLine("\n-----------------------------------------------------------------------------");
		Console.WriteLine("NodeLabelDrawer created.");
		Console.WriteLine("* L key to toggle node label display.");
		Console.WriteLine("-----------------------------------------------------------------------------");

		_labelFont = new Font(SystemFonts.DefaultFont.FontFamily, pNodeGraph.nodeSize, FontStyle.Bold);
		_graph = pNodeGraph;
	}

	/////////////////////////////////////////////////////////////////////////////////////////
	///							Update loop
	///							

	//this has to be virtual otherwise the subclass won't pick it up
	protected virtual void Update()
	{
		//toggle label display when L is pressed
		if (Input.GetKeyDown(Key.L))
		{
			_showLabels = !_showLabels;
			graphics.Clear(Color.Transparent);
			if (_showLabels) drawLabels();
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////
	/// NodeGraph visualization helper methods

	protected virtual void drawLabels()
	{
		foreach (KeyValuePair<DungeonComponent, Node> node in _graph.nodes) drawNode(node.Value);
	}

	protected virtual void drawNode(Node pNode)
	{
		SizeF size = graphics.MeasureString(pNode.id, _labelFont);
		graphics.DrawString(pNode.id, _labelFont, Brushes.Black, pNode.location.X - size.Width / 2, pNode.location.Y - size.Height / 2);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using GraphProcessor;

[NodeCustomEditor(typeof(IfNode))]
public class IfNodeView : BaseNodeView
{
	public override void Enable()
	{
<<<<<<< HEAD
=======
		hasSettings = true;	// or base.Enable();
>>>>>>> 1834eda40dd381be28ee7c530b27a31b525ca99a
		var node = nodeTarget as IfNode;

        // Create your fields using node's variables and add them to the controlsContainer

		controlsContainer.Add(new Label($"Last Evaluation: {node.condition}"));
	}
}
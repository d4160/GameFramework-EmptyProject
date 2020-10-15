using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using NodeGraphProcessor.Examples;
using UnityEngine.Rendering;

[System.Serializable, NodeMenuItem("Conditional/If"), NodeMenuItem("Conditional/Branch")]
public class IfNode : ConditionalNode
{
	[Input(name = "Condition")]
    public bool				condition;

	[Output(name = "True")]
	public ConditionalLink	@true;
	[Output(name = "False")]
	public ConditionalLink	@false;

<<<<<<< HEAD
=======
	[Setting("Compare Function")]
>>>>>>> 1834eda40dd381be28ee7c530b27a31b525ca99a
	public CompareFunction		compareOperator;

	public override string		name => "If";

	public override IEnumerable< ConditionalNode >	GetExecutedNodes()
	{
		string fieldName = condition ? nameof(@true) : nameof(@false);

		// Return all the nodes connected to either the true or false node
		return outputPorts.FirstOrDefault(n => n.fieldName == fieldName)
			.GetEdges().Select(e => e.inputNode as ConditionalNode);
	}
}

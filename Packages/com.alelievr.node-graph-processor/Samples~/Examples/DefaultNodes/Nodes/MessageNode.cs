﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/MessageNode")]
public class MessageNode : BaseNode
{
	const string k_InputIsNot42Error = "Input is not 42 !";

	[Input(name = "In")]
    public float                input;

	public override string		name => "MessageNode";

<<<<<<< HEAD
	protected override void Process()
	{
		if (input != 42)
			AddMessage(k_InputIsNot42Error, NodeMessageType.Error);
=======
	[Setting("Message Type")]
	public NodeMessageType messageType = NodeMessageType.Error;

	protected override void Process()
	{
		if (input != 42)
			AddMessage(k_InputIsNot42Error, messageType);
>>>>>>> 1834eda40dd381be28ee7c530b27a31b525ca99a
		else
			RemoveMessage(k_InputIsNot42Error);
	}
}

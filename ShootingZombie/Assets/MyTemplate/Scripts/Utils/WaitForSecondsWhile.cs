using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsWhile : CustomYieldInstruction
{
	private float? lastTrue;

	private float delay;

	private Func<bool> predicate;

	public WaitForSecondsWhile(Func<bool> predicate, float delay)
	{
		lastTrue = new float?(Time.time);
		this.delay = delay;
		this.predicate = predicate;
	}

	public override bool keepWaiting
	{
		get
		{
			if (predicate())
			{
				if (lastTrue.Value + delay < Time.time)
				{
					return false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
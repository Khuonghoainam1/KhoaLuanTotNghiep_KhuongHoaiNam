using Lean.Pool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Yurowm.GameCore
{
	public class ContentPoolable : Content
	{
        public override GameObject Spawn(GameObject refObj)
        {
            ILiveContentPoolable poolable = refObj.GetComponent<ILiveContentPoolable>();
            if (poolable == null)
            {
                return Instantiate(refObj);
            }
            return LeanPool.Spawn(refObj);
        }

        public static AA_Game.Item Emit(ItemID id)
        {
            AA_Game.Item prefab = GetPrefab<AA_Game.Item>(x =>
            {
                return x.id == id;
            });

            return Emit(prefab);
        }
    }
}

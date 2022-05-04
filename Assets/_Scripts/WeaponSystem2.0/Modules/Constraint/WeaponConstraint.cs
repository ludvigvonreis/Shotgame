using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace WeaponSystem
{
	public class WeaponConstraint : Weapon.Module
	{
		public List<IInterface> List { get; protected set; }
		public interface IInterface
		{
			bool Constraint { get; }
		}

		public bool Active
		{
			get
			{
				for (int i = 0; i < List.Count; i++)
				{
					if (List[i].Constraint)
						return true;
				}

				return false;
			}
		}

		public override void Configure()
		{
			base.Configure();

			List = new List<IInterface>();

			var selection = weaponReference.behaviours.Where(x => x is IInterface).Cast<IInterface>();

			List.AddRange(selection);
		}
	}
}
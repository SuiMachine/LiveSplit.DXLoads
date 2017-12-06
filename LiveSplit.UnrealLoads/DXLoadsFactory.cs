using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

namespace LiveSplit.DXLoads
{
	public class DXLoadsFactory : IComponentFactory
	{
		public string ComponentName => "DXLoads";

		public string Description => "Autosplitting and load removal component for some Deus Ex games";

		public ComponentCategory Category => ComponentCategory.Control;

		public IComponent Create(LiveSplitState state) => new DXLoadsComponent(state);

		public string UpdateName => ComponentName;

		public string UpdateURL => "";

		public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		public string XMLURL => UpdateURL + "Components/update.LiveSplit.DXLoads.xml";
	}
}

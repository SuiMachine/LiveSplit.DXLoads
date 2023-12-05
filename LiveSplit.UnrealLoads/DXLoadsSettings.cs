using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.DXLoads.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LiveSplit.DXLoads
{
	public partial class DXLoadsSettings : UserControl
	{
		public bool AutoStart { get; set; }
		public bool AutoReset { get; set; }
		public bool AutoSplitOnMapChange { get; set; }
		public bool AutoSplitExceptOnRepeatMaps { get; set; }
		public bool NoAutoSplit { get; set; }
		public bool Whitelist { get; set; }
		public bool DbgShowMap { get; set; }
		public Dictionary<string, bool> Maps { get; private set; }
		const bool DEFAULT_AUTOSTART = true;
		const bool DEFAULT_AUTORESET = true;
		const bool DEFAULT_AUTOSPLITONMAPCHANGE = false;
		const bool DEFAULT_AUTOSPLITEXCEPTONREPEATMAPS = true;
		const bool DEFAULT_NOAUTOSPLIT = false;
		const bool DEFAULT_WHITELIST = true;

		LiveSplitState _state;
		bool _isRefreshingListbox;

		public DXLoadsSettings(LiveSplitState state)
		{
			InitializeComponent();

			_state = state;

			Maps = new Dictionary<string, bool>();
			cbGame.DataSource = GameMemory.SupportedGames.Select(s => s.GetType()).ToList();
			cbGame.DisplayMember = "Name";

			chkAutoStart.DataBindings.Add("Checked", this, nameof(AutoStart), false, DataSourceUpdateMode.OnPropertyChanged);
			chkAutoReset.DataBindings.Add("Checked", this, nameof(AutoReset), false, DataSourceUpdateMode.OnPropertyChanged);
			rbAutoSplitOnMapChange.DataBindings.Add("Checked", this, nameof(AutoSplitOnMapChange), false, DataSourceUpdateMode.OnPropertyChanged);
			rbAutoSplitExceptOnRepeatMaps.DataBindings.Add("Checked", this, nameof(AutoSplitExceptOnRepeatMaps), false, DataSourceUpdateMode.OnPropertyChanged);
			rbNoAutoSplit.DataBindings.Add("Checked", this, nameof(NoAutoSplit), false, DataSourceUpdateMode.OnPropertyChanged);
			gbMapWhitelist.DataBindings.Add("Enabled", this, nameof(Whitelist), false, DataSourceUpdateMode.OnPropertyChanged);
			chkDbgShowMap.DataBindings.Add("Checked", this, nameof(DbgShowMap), false, DataSourceUpdateMode.OnPropertyChanged);

			// defaults
			AutoStart = DEFAULT_AUTOSTART;
			AutoReset = DEFAULT_AUTORESET;
			AutoSplitOnMapChange = DEFAULT_AUTOSPLITONMAPCHANGE;
			AutoSplitExceptOnRepeatMaps = DEFAULT_AUTOSPLITEXCEPTONREPEATMAPS;
			NoAutoSplit = DEFAULT_NOAUTOSPLIT;
			Whitelist = DEFAULT_WHITELIST;
			cbGame.SelectedItem = SearchGameSupport(_state.Run.GameName)?.GetType() ?? GameMemory.SupportedGames[0].GetType();

#if DEBUG
			chkDbgShowMap.Visible = true;
#endif
		}

		static GameSupport SearchGameSupport(string name)
		{
			var game = GameMemory.SupportedGames.FirstOrDefault(g => g.GetType().Name == name);
			if (game != null)
				return game;

			return GameMemory.SupportedGames.FirstOrDefault(
				g => g.GameNames.Any(n => name.ToLower().Contains(n.ToLower()))
				);
		}

		void RefreshCheckList()
		{
			_isRefreshingListbox = true;

			chklbMapSet.Items.Clear();
			foreach (var pair in Maps)
			{
				chklbMapSet.Items.Add(pair.Key, pair.Value);
			}

			_isRefreshingListbox = false;
		}

		public XmlNode GetSettings(XmlDocument doc)
		{
			XmlElement settingsNode = doc.CreateElement("Settings");

			settingsNode.AppendChild(SettingsHelper.ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(AutoStart), AutoStart));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(AutoReset), AutoReset));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(AutoSplitOnMapChange), AutoSplitOnMapChange));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(AutoSplitExceptOnRepeatMaps), AutoSplitExceptOnRepeatMaps));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(NoAutoSplit), NoAutoSplit));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, nameof(Whitelist), Whitelist));
			settingsNode.AppendChild(SettingsHelper.ToElement(doc, "Game", ((Type)cbGame.SelectedItem).Name));

			var mapsNode = settingsNode.AppendChild(doc.CreateElement("MapWhitelist"));
			foreach (var pair in Maps)
			{
				var elem = (XmlElement)mapsNode.AppendChild(doc.CreateElement("Map"));
				elem.InnerText = pair.Key;
				elem.SetAttribute("enabled", pair.Value.ToString());
			}

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			var element = (XmlElement)settings;

			AutoStart = SettingsHelper.ParseBool(settings[nameof(AutoStart)], DEFAULT_AUTOSTART);
			AutoReset = SettingsHelper.ParseBool(settings[nameof(AutoReset)], DEFAULT_AUTOSTART);
			AutoSplitOnMapChange = SettingsHelper.ParseBool(settings[nameof(AutoSplitOnMapChange)], DEFAULT_AUTOSPLITONMAPCHANGE);
			AutoSplitExceptOnRepeatMaps = SettingsHelper.ParseBool(settings[nameof(AutoSplitExceptOnRepeatMaps)], DEFAULT_AUTOSPLITEXCEPTONREPEATMAPS);
			NoAutoSplit = SettingsHelper.ParseBool(settings[nameof(NoAutoSplit)], DEFAULT_NOAUTOSPLIT);
			Whitelist = SettingsHelper.ParseBool(settings[nameof(Whitelist)], DEFAULT_WHITELIST);

			Type game = null;
			if (!string.IsNullOrWhiteSpace(settings["Game"]?.InnerText))
				game = SearchGameSupport(settings["Game"].InnerText)?.GetType();

			if (game == null)
				game = SearchGameSupport(_state.Run.GameName)?.GetType() ?? GameMemory.SupportedGames[0].GetType();

			cbGame.SelectedItem = game;

			if (settings["MapWhitelist"] != null)
			{
				foreach (XmlElement elem in settings["MapWhitelist"].ChildNodes)
				{
					if (Maps.ContainsKey(elem.InnerText))
						Maps[elem.InnerText] = bool.Parse(elem.GetAttribute("enabled"));
				}
			}
			RefreshCheckList();
		}

		void rb_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAutoSplitOnMapChange.Checked)
			{
				AutoSplitOnMapChange = true;
				Whitelist = true;
			}
			else if (rbAutoSplitExceptOnRepeatMaps.Checked)
			{
				AutoSplitExceptOnRepeatMaps = true;
				Whitelist = true;
			}
			else if (rbNoAutoSplit.Checked)
			{
				NoAutoSplit = true;
				Whitelist = false;
			}
		}
		void btnAddMap_Click(object sender, EventArgs e)
		{
			txtMap.Text = txtMap.Text.Trim();

			if (!string.IsNullOrWhiteSpace(txtMap.Text))
			{
				Maps.Add(txtMap.Text, true);
				chklbMapSet.Items.Add(txtMap.Text, true);
				txtMap.Clear();
			}
		}

		void btnRemoveMap_Click(object sender, EventArgs e)
		{
			if (chklbMapSet.SelectedIndex < 0)
				return;

			var selectedIndex = chklbMapSet.SelectedIndex;
			Maps.Remove((string)chklbMapSet.SelectedItem);
			chklbMapSet.Items.RemoveAt(selectedIndex);

			var count = chklbMapSet.Items.Count;
			if (count > 0)
				chklbMapSet.SelectedIndex = selectedIndex <= count - 1 ? selectedIndex : selectedIndex - 1;
		}

		void cbGame_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selected = (GameSupport)Activator.CreateInstance((Type)cbGame.SelectedItem);

			var copy = new Dictionary<string, bool>(Maps);
			Maps.Clear();
			if (selected?.Maps != null)
			{
				foreach (var map in selected.Maps)
				{
					Maps[map] = copy.ContainsKey(map) ? copy[map] : true;
				}
			}

			RefreshCheckList();
		}

		void chklbMapSet_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (_isRefreshingListbox)
				return;

			var map = (string)chklbMapSet.Items[e.Index];
			Maps[map] = CheckState.Checked == e.NewValue;
		}
	}
}

﻿namespace LiveSplit.DXLoads
{
    partial class DXLoadsSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.gbStartSplits = new System.Windows.Forms.GroupBox();
			this.tlpStartSplits = new System.Windows.Forms.TableLayoutPanel();
			this.chkAutoStart = new System.Windows.Forms.CheckBox();
			this.chkAutoReset = new System.Windows.Forms.CheckBox();
			this.chkSplitOnNewMap = new System.Windows.Forms.CheckBox();
			this.chkDbgShowMap = new System.Windows.Forms.CheckBox();
			this.gbMapWhitelist = new System.Windows.Forms.GroupBox();
			this.tlpMapWhitelist = new System.Windows.Forms.TableLayoutPanel();
			this.chklbMapSet = new System.Windows.Forms.CheckedListBox();
			this.tlpGameSelection = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.cbGame = new System.Windows.Forms.ComboBox();
			this.tlpMapSetEdit = new System.Windows.Forms.TableLayoutPanel();
			this.btnAddMap = new System.Windows.Forms.Button();
			this.btnRemoveMap = new System.Windows.Forms.Button();
			this.txtMap = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tlpMain.SuspendLayout();
			this.gbStartSplits.SuspendLayout();
			this.tlpStartSplits.SuspendLayout();
			this.gbMapWhitelist.SuspendLayout();
			this.tlpMapWhitelist.SuspendLayout();
			this.tlpGameSelection.SuspendLayout();
			this.tlpMapSetEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpMain
			// 
			this.tlpMain.AutoSize = true;
			this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.gbStartSplits, 0, 0);
			this.tlpMain.Controls.Add(this.gbMapWhitelist, 0, 1);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpMain.Location = new System.Drawing.Point(7, 7);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 2;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(462, 340);
			this.tlpMain.TabIndex = 0;
			// 
			// gbStartSplits
			// 
			this.gbStartSplits.AutoSize = true;
			this.gbStartSplits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbStartSplits.Controls.Add(this.tlpStartSplits);
			this.gbStartSplits.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbStartSplits.Location = new System.Drawing.Point(3, 3);
			this.gbStartSplits.Name = "gbStartSplits";
			this.gbStartSplits.Size = new System.Drawing.Size(456, 111);
			this.gbStartSplits.TabIndex = 5;
			this.gbStartSplits.TabStop = false;
			this.gbStartSplits.Text = "General";
			// 
			// tlpStartSplits
			// 
			this.tlpStartSplits.AutoSize = true;
			this.tlpStartSplits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpStartSplits.BackColor = System.Drawing.Color.Transparent;
			this.tlpStartSplits.ColumnCount = 1;
			this.tlpStartSplits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpStartSplits.Controls.Add(this.chkAutoStart, 0, 0);
			this.tlpStartSplits.Controls.Add(this.chkAutoReset, 0, 1);
			this.tlpStartSplits.Controls.Add(this.chkSplitOnNewMap, 0, 2);
			this.tlpStartSplits.Controls.Add(this.chkDbgShowMap, 0, 3);
			this.tlpStartSplits.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpStartSplits.Location = new System.Drawing.Point(3, 16);
			this.tlpStartSplits.Name = "tlpStartSplits";
			this.tlpStartSplits.RowCount = 4;
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStartSplits.Size = new System.Drawing.Size(450, 92);
			this.tlpStartSplits.TabIndex = 4;
			// 
			// chkAutoStart
			// 
			this.chkAutoStart.AutoSize = true;
			this.chkAutoStart.Checked = true;
			this.chkAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoStart.Location = new System.Drawing.Point(3, 3);
			this.chkAutoStart.Name = "chkAutoStart";
			this.chkAutoStart.Size = new System.Drawing.Size(98, 17);
			this.chkAutoStart.TabIndex = 4;
			this.chkAutoStart.Text = "Automatic Start";
			this.chkAutoStart.UseVisualStyleBackColor = true;
			// 
			// chkAutoReset
			// 
			this.chkAutoReset.AutoSize = true;
			this.chkAutoReset.Checked = true;
			this.chkAutoReset.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoReset.Location = new System.Drawing.Point(3, 26);
			this.chkAutoReset.Name = "chkAutoReset";
			this.chkAutoReset.Size = new System.Drawing.Size(104, 17);
			this.chkAutoReset.TabIndex = 5;
			this.chkAutoReset.Text = "Automatic Reset";
			this.chkAutoReset.UseVisualStyleBackColor = true;
			// 
			// chkSplitOnNewMap
			// 
			this.chkSplitOnNewMap.AutoSize = true;
			this.chkSplitOnNewMap.Location = new System.Drawing.Point(3, 49);
			this.chkSplitOnNewMap.Name = "chkSplitOnNewMap";
			this.chkSplitOnNewMap.Size = new System.Drawing.Size(175, 17);
			this.chkSplitOnNewMap.TabIndex = 6;
			this.chkSplitOnNewMap.Text = "Automatic Split on Map Change";
			this.chkSplitOnNewMap.UseVisualStyleBackColor = true;
			// 
			// chkDbgShowMap
			// 
			this.chkDbgShowMap.AutoSize = true;
			this.chkDbgShowMap.Location = new System.Drawing.Point(3, 72);
			this.chkDbgShowMap.Name = "chkDbgShowMap";
			this.chkDbgShowMap.Size = new System.Drawing.Size(188, 17);
			this.chkDbgShowMap.TabIndex = 7;
			this.chkDbgShowMap.Text = "Debug: Show Map Name on Load";
			this.chkDbgShowMap.UseVisualStyleBackColor = true;
			this.chkDbgShowMap.Visible = false;
			// 
			// gbMapWhitelist
			// 
			this.gbMapWhitelist.AutoSize = true;
			this.gbMapWhitelist.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbMapWhitelist.Controls.Add(this.tlpMapWhitelist);
			this.gbMapWhitelist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbMapWhitelist.Enabled = false;
			this.gbMapWhitelist.Location = new System.Drawing.Point(3, 120);
			this.gbMapWhitelist.Name = "gbMapWhitelist";
			this.gbMapWhitelist.Size = new System.Drawing.Size(456, 217);
			this.gbMapWhitelist.TabIndex = 6;
			this.gbMapWhitelist.TabStop = false;
			this.gbMapWhitelist.Text = "Map Whitelist";
			// 
			// tlpMapWhitelist
			// 
			this.tlpMapWhitelist.AutoSize = true;
			this.tlpMapWhitelist.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpMapWhitelist.ColumnCount = 1;
			this.tlpMapWhitelist.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMapWhitelist.Controls.Add(this.chklbMapSet, 0, 1);
			this.tlpMapWhitelist.Controls.Add(this.tlpGameSelection, 0, 0);
			this.tlpMapWhitelist.Controls.Add(this.tlpMapSetEdit, 0, 2);
			this.tlpMapWhitelist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMapWhitelist.Location = new System.Drawing.Point(3, 16);
			this.tlpMapWhitelist.Name = "tlpMapWhitelist";
			this.tlpMapWhitelist.RowCount = 3;
			this.tlpMapWhitelist.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMapWhitelist.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMapWhitelist.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMapWhitelist.Size = new System.Drawing.Size(450, 198);
			this.tlpMapWhitelist.TabIndex = 0;
			// 
			// chklbMapSet
			// 
			this.chklbMapSet.CheckOnClick = true;
			this.chklbMapSet.Dock = System.Windows.Forms.DockStyle.Top;
			this.chklbMapSet.FormattingEnabled = true;
			this.chklbMapSet.Location = new System.Drawing.Point(3, 36);
			this.chklbMapSet.MultiColumn = true;
			this.chklbMapSet.Name = "chklbMapSet";
			this.chklbMapSet.Size = new System.Drawing.Size(444, 124);
			this.chklbMapSet.TabIndex = 2;
			this.chklbMapSet.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklbMapSet_ItemCheck);
			// 
			// tlpGameSelection
			// 
			this.tlpGameSelection.AutoSize = true;
			this.tlpGameSelection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpGameSelection.ColumnCount = 2;
			this.tlpGameSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpGameSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpGameSelection.Controls.Add(this.label2, 0, 0);
			this.tlpGameSelection.Controls.Add(this.cbGame, 1, 0);
			this.tlpGameSelection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpGameSelection.Location = new System.Drawing.Point(3, 3);
			this.tlpGameSelection.Name = "tlpGameSelection";
			this.tlpGameSelection.RowCount = 1;
			this.tlpGameSelection.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpGameSelection.Size = new System.Drawing.Size(444, 27);
			this.tlpGameSelection.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Game:";
			// 
			// cbGame
			// 
			this.cbGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGame.FormattingEnabled = true;
			this.cbGame.Location = new System.Drawing.Point(47, 3);
			this.cbGame.MinimumSize = new System.Drawing.Size(100, 0);
			this.cbGame.Name = "cbGame";
			this.cbGame.Size = new System.Drawing.Size(100, 21);
			this.cbGame.TabIndex = 1;
			this.cbGame.SelectedIndexChanged += new System.EventHandler(this.cbGame_SelectedIndexChanged);
			// 
			// tlpMapSetEdit
			// 
			this.tlpMapSetEdit.AutoSize = true;
			this.tlpMapSetEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpMapSetEdit.ColumnCount = 4;
			this.tlpMapSetEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMapSetEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMapSetEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMapSetEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMapSetEdit.Controls.Add(this.btnAddMap, 2, 0);
			this.tlpMapSetEdit.Controls.Add(this.btnRemoveMap, 3, 0);
			this.tlpMapSetEdit.Controls.Add(this.txtMap, 1, 0);
			this.tlpMapSetEdit.Controls.Add(this.label3, 0, 0);
			this.tlpMapSetEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMapSetEdit.Location = new System.Drawing.Point(3, 166);
			this.tlpMapSetEdit.Name = "tlpMapSetEdit";
			this.tlpMapSetEdit.RowCount = 1;
			this.tlpMapSetEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMapSetEdit.Size = new System.Drawing.Size(444, 29);
			this.tlpMapSetEdit.TabIndex = 4;
			this.tlpMapSetEdit.Visible = false;
			// 
			// btnAddMap
			// 
			this.btnAddMap.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnAddMap.Location = new System.Drawing.Point(153, 3);
			this.btnAddMap.Name = "btnAddMap";
			this.btnAddMap.Size = new System.Drawing.Size(75, 23);
			this.btnAddMap.TabIndex = 0;
			this.btnAddMap.Text = "Add";
			this.btnAddMap.UseVisualStyleBackColor = true;
			this.btnAddMap.Click += new System.EventHandler(this.btnAddMap_Click);
			// 
			// btnRemoveMap
			// 
			this.btnRemoveMap.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnRemoveMap.Location = new System.Drawing.Point(234, 3);
			this.btnRemoveMap.Name = "btnRemoveMap";
			this.btnRemoveMap.Size = new System.Drawing.Size(75, 23);
			this.btnRemoveMap.TabIndex = 1;
			this.btnRemoveMap.Text = "Remove";
			this.btnRemoveMap.UseVisualStyleBackColor = true;
			this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
			// 
			// txtMap
			// 
			this.txtMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMap.Location = new System.Drawing.Point(40, 4);
			this.txtMap.Name = "txtMap";
			this.txtMap.Size = new System.Drawing.Size(107, 20);
			this.txtMap.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Map:";
			// 
			// DXLoadsSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpMain);
			this.Name = "DXLoadsSettings";
			this.Padding = new System.Windows.Forms.Padding(7);
			this.Size = new System.Drawing.Size(476, 487);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.gbStartSplits.ResumeLayout(false);
			this.gbStartSplits.PerformLayout();
			this.tlpStartSplits.ResumeLayout(false);
			this.tlpStartSplits.PerformLayout();
			this.gbMapWhitelist.ResumeLayout(false);
			this.gbMapWhitelist.PerformLayout();
			this.tlpMapWhitelist.ResumeLayout(false);
			this.tlpMapWhitelist.PerformLayout();
			this.tlpGameSelection.ResumeLayout(false);
			this.tlpGameSelection.PerformLayout();
			this.tlpMapSetEdit.ResumeLayout(false);
			this.tlpMapSetEdit.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbStartSplits;
        private System.Windows.Forms.TableLayoutPanel tlpStartSplits;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.CheckBox chkAutoReset;
		private System.Windows.Forms.CheckBox chkSplitOnNewMap;
		private System.Windows.Forms.GroupBox gbMapWhitelist;
		private System.Windows.Forms.TableLayoutPanel tlpMapWhitelist;
		private System.Windows.Forms.CheckBox chkDbgShowMap;
		private System.Windows.Forms.TableLayoutPanel tlpGameSelection;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tlpMapSetEdit;
		private System.Windows.Forms.Button btnAddMap;
		private System.Windows.Forms.Button btnRemoveMap;
		private System.Windows.Forms.TextBox txtMap;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbGame;
		private System.Windows.Forms.CheckedListBox chklbMapSet;
	}
}

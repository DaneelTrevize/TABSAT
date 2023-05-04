namespace TABSAT
{
    partial class MapViewerControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.zoomTrackBar = new System.Windows.Forms.TrackBar();
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.layersGroupBox = new System.Windows.Forms.GroupBox();
            this.swarmsCheckBox = new System.Windows.Forms.CheckBox();
            this.zombieComboBox = new System.Windows.Forms.ComboBox();
            this.zombieLabel = new System.Windows.Forms.Label();
            this.zombieCheckBox = new System.Windows.Forms.CheckBox();
            this.zombieTrackBar = new System.Windows.Forms.TrackBar();
            this.navQuadLabel = new System.Windows.Forms.Label();
            this.navQuadsTrackBar = new System.Windows.Forms.TrackBar();
            this.navQuadsCheckBox = new System.Windows.Forms.CheckBox();
            this.directionCheckBox = new System.Windows.Forms.CheckBox();
            this.navigableCheckBox = new System.Windows.Forms.CheckBox();
            this.activityCheckBox = new System.Windows.Forms.CheckBox();
            this.fogCheckBox = new System.Windows.Forms.CheckBox();
            this.terrainCheckBox = new System.Windows.Forms.CheckBox();
            this.distanceCheckBox = new System.Windows.Forms.CheckBox();
            this.gridCheckBox = new System.Windows.Forms.CheckBox();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.rotateCheckBox = new System.Windows.Forms.CheckBox();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.vodsCheckBox = new System.Windows.Forms.CheckBox();
            this.hugeCheckBox = new System.Windows.Forms.CheckBox();
            this.pickablesCheckBox = new System.Windows.Forms.CheckBox();
            this.removablesGroupBox = new System.Windows.Forms.GroupBox();
            this.joinableCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.mapPanel.SuspendLayout();
            this.layersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zombieTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navQuadsTrackBar)).BeginInit();
            this.optionsGroupBox.SuspendLayout();
            this.removablesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // zoomTrackBar
            // 
            this.zoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomTrackBar.LargeChange = 2;
            this.zoomTrackBar.Location = new System.Drawing.Point(1059, 741);
            this.zoomTrackBar.Maximum = 8;
            this.zoomTrackBar.Minimum = 1;
            this.zoomTrackBar.Name = "zoomTrackBar";
            this.zoomTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBar.Size = new System.Drawing.Size(45, 298);
            this.zoomTrackBar.TabIndex = 1;
            this.zoomTrackBar.Value = 2;
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(1024, 1024);
            this.mapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mapPictureBox.TabIndex = 0;
            this.mapPictureBox.TabStop = false;
            // 
            // mapPanel
            // 
            this.mapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapPanel.AutoScroll = true;
            this.mapPanel.Controls.Add(this.mapPictureBox);
            this.mapPanel.Location = new System.Drawing.Point(12, 12);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(1024, 1027);
            this.mapPanel.TabIndex = 0;
            // 
            // layersGroupBox
            // 
            this.layersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layersGroupBox.Controls.Add(this.swarmsCheckBox);
            this.layersGroupBox.Controls.Add(this.zombieComboBox);
            this.layersGroupBox.Controls.Add(this.zombieLabel);
            this.layersGroupBox.Controls.Add(this.zombieCheckBox);
            this.layersGroupBox.Controls.Add(this.zombieTrackBar);
            this.layersGroupBox.Controls.Add(this.navQuadLabel);
            this.layersGroupBox.Controls.Add(this.navQuadsTrackBar);
            this.layersGroupBox.Controls.Add(this.navQuadsCheckBox);
            this.layersGroupBox.Controls.Add(this.directionCheckBox);
            this.layersGroupBox.Controls.Add(this.navigableCheckBox);
            this.layersGroupBox.Controls.Add(this.activityCheckBox);
            this.layersGroupBox.Controls.Add(this.fogCheckBox);
            this.layersGroupBox.Controls.Add(this.terrainCheckBox);
            this.layersGroupBox.Controls.Add(this.distanceCheckBox);
            this.layersGroupBox.Location = new System.Drawing.Point(1053, 12);
            this.layersGroupBox.Name = "layersGroupBox";
            this.layersGroupBox.Size = new System.Drawing.Size(110, 535);
            this.layersGroupBox.TabIndex = 3;
            this.layersGroupBox.TabStop = false;
            this.layersGroupBox.Text = "Show Layers";
            // 
            // swarmsCheckBox
            // 
            this.swarmsCheckBox.AutoSize = true;
            this.swarmsCheckBox.Checked = true;
            this.swarmsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.swarmsCheckBox.Location = new System.Drawing.Point(6, 512);
            this.swarmsCheckBox.Name = "swarmsCheckBox";
            this.swarmsCheckBox.Size = new System.Drawing.Size(63, 17);
            this.swarmsCheckBox.TabIndex = 14;
            this.swarmsCheckBox.Text = "Swarms";
            this.swarmsCheckBox.UseVisualStyleBackColor = true;
            // 
            // zombieComboBox
            // 
            this.zombieComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zombieComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zombieComboBox.FormattingEnabled = true;
            this.zombieComboBox.Location = new System.Drawing.Point(6, 344);
            this.zombieComboBox.Name = "zombieComboBox";
            this.zombieComboBox.Size = new System.Drawing.Size(82, 21);
            this.zombieComboBox.TabIndex = 13;
            // 
            // zombieLabel
            // 
            this.zombieLabel.AutoSize = true;
            this.zombieLabel.Location = new System.Drawing.Point(42, 419);
            this.zombieLabel.Name = "zombieLabel";
            this.zombieLabel.Size = new System.Drawing.Size(42, 39);
            this.zombieLabel.TabIndex = 12;
            this.zombieLabel.Text = "Zombie\r\nQuad\r\nZoom";
            // 
            // zombieCheckBox
            // 
            this.zombieCheckBox.AutoSize = true;
            this.zombieCheckBox.Location = new System.Drawing.Point(6, 321);
            this.zombieCheckBox.Name = "zombieCheckBox";
            this.zombieCheckBox.Size = new System.Drawing.Size(66, 17);
            this.zombieCheckBox.TabIndex = 11;
            this.zombieCheckBox.Text = "Zombies";
            this.zombieCheckBox.UseVisualStyleBackColor = true;
            // 
            // zombieTrackBar
            // 
            this.zombieTrackBar.LargeChange = 2;
            this.zombieTrackBar.Location = new System.Drawing.Point(6, 371);
            this.zombieTrackBar.Maximum = 7;
            this.zombieTrackBar.Minimum = 1;
            this.zombieTrackBar.Name = "zombieTrackBar";
            this.zombieTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zombieTrackBar.Size = new System.Drawing.Size(45, 135);
            this.zombieTrackBar.SmallChange = 6;
            this.zombieTrackBar.TabIndex = 10;
            this.zombieTrackBar.Value = 3;
            // 
            // navQuadLabel
            // 
            this.navQuadLabel.AutoSize = true;
            this.navQuadLabel.Location = new System.Drawing.Point(46, 228);
            this.navQuadLabel.Name = "navQuadLabel";
            this.navQuadLabel.Size = new System.Drawing.Size(34, 39);
            this.navQuadLabel.TabIndex = 9;
            this.navQuadLabel.Text = "Nav\r\nQuad\r\nZoom";
            // 
            // navQuadsTrackBar
            // 
            this.navQuadsTrackBar.LargeChange = 2;
            this.navQuadsTrackBar.Location = new System.Drawing.Point(6, 180);
            this.navQuadsTrackBar.Maximum = 7;
            this.navQuadsTrackBar.Minimum = 1;
            this.navQuadsTrackBar.Name = "navQuadsTrackBar";
            this.navQuadsTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.navQuadsTrackBar.Size = new System.Drawing.Size(45, 135);
            this.navQuadsTrackBar.TabIndex = 6;
            this.navQuadsTrackBar.Value = 3;
            // 
            // navQuadsCheckBox
            // 
            this.navQuadsCheckBox.AutoSize = true;
            this.navQuadsCheckBox.Checked = true;
            this.navQuadsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.navQuadsCheckBox.Location = new System.Drawing.Point(6, 157);
            this.navQuadsCheckBox.Name = "navQuadsCheckBox";
            this.navQuadsCheckBox.Size = new System.Drawing.Size(77, 17);
            this.navQuadsCheckBox.TabIndex = 8;
            this.navQuadsCheckBox.Text = "NavQuads";
            this.navQuadsCheckBox.UseVisualStyleBackColor = true;
            // 
            // directionCheckBox
            // 
            this.directionCheckBox.AutoSize = true;
            this.directionCheckBox.Location = new System.Drawing.Point(6, 134);
            this.directionCheckBox.Name = "directionCheckBox";
            this.directionCheckBox.Size = new System.Drawing.Size(68, 17);
            this.directionCheckBox.TabIndex = 7;
            this.directionCheckBox.Text = "Direction";
            this.directionCheckBox.UseVisualStyleBackColor = true;
            // 
            // navigableCheckBox
            // 
            this.navigableCheckBox.AutoSize = true;
            this.navigableCheckBox.Location = new System.Drawing.Point(6, 88);
            this.navigableCheckBox.Name = "navigableCheckBox";
            this.navigableCheckBox.Size = new System.Drawing.Size(74, 17);
            this.navigableCheckBox.TabIndex = 5;
            this.navigableCheckBox.Text = "Navigable";
            this.navigableCheckBox.UseVisualStyleBackColor = true;
            // 
            // activityCheckBox
            // 
            this.activityCheckBox.AutoSize = true;
            this.activityCheckBox.Location = new System.Drawing.Point(6, 65);
            this.activityCheckBox.Name = "activityCheckBox";
            this.activityCheckBox.Size = new System.Drawing.Size(53, 17);
            this.activityCheckBox.TabIndex = 4;
            this.activityCheckBox.Text = "Noise";
            this.activityCheckBox.UseVisualStyleBackColor = true;
            // 
            // fogCheckBox
            // 
            this.fogCheckBox.AutoSize = true;
            this.fogCheckBox.Checked = true;
            this.fogCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fogCheckBox.Location = new System.Drawing.Point(6, 42);
            this.fogCheckBox.Name = "fogCheckBox";
            this.fogCheckBox.Size = new System.Drawing.Size(44, 17);
            this.fogCheckBox.TabIndex = 3;
            this.fogCheckBox.Text = "Fog";
            this.fogCheckBox.UseVisualStyleBackColor = true;
            // 
            // terrainCheckBox
            // 
            this.terrainCheckBox.AutoSize = true;
            this.terrainCheckBox.Checked = true;
            this.terrainCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.terrainCheckBox.Location = new System.Drawing.Point(6, 19);
            this.terrainCheckBox.Name = "terrainCheckBox";
            this.terrainCheckBox.Size = new System.Drawing.Size(59, 17);
            this.terrainCheckBox.TabIndex = 2;
            this.terrainCheckBox.Text = "Terrain";
            this.terrainCheckBox.UseVisualStyleBackColor = true;
            // 
            // distanceCheckBox
            // 
            this.distanceCheckBox.AutoSize = true;
            this.distanceCheckBox.Checked = true;
            this.distanceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.distanceCheckBox.Location = new System.Drawing.Point(6, 111);
            this.distanceCheckBox.Name = "distanceCheckBox";
            this.distanceCheckBox.Size = new System.Drawing.Size(68, 17);
            this.distanceCheckBox.TabIndex = 6;
            this.distanceCheckBox.Text = "Distance";
            this.distanceCheckBox.UseVisualStyleBackColor = true;
            // 
            // gridCheckBox
            // 
            this.gridCheckBox.AutoSize = true;
            this.gridCheckBox.Location = new System.Drawing.Point(6, 19);
            this.gridCheckBox.Name = "gridCheckBox";
            this.gridCheckBox.Size = new System.Drawing.Size(75, 17);
            this.gridCheckBox.TabIndex = 8;
            this.gridCheckBox.Text = "Show Grid";
            this.gridCheckBox.UseVisualStyleBackColor = true;
            // 
            // zoomLabel
            // 
            this.zoomLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.zoomLabel.AutoSize = true;
            this.zoomLabel.Location = new System.Drawing.Point(1103, 877);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(34, 26);
            this.zoomLabel.TabIndex = 4;
            this.zoomLabel.Text = "Map\r\nZoom";
            // 
            // rotateCheckBox
            // 
            this.rotateCheckBox.AutoSize = true;
            this.rotateCheckBox.Checked = true;
            this.rotateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotateCheckBox.Location = new System.Drawing.Point(6, 42);
            this.rotateCheckBox.Name = "rotateCheckBox";
            this.rotateCheckBox.Size = new System.Drawing.Size(58, 17);
            this.rotateCheckBox.TabIndex = 9;
            this.rotateCheckBox.Text = "Rotate";
            this.rotateCheckBox.UseVisualStyleBackColor = true;
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsGroupBox.Controls.Add(this.rotateCheckBox);
            this.optionsGroupBox.Controls.Add(this.gridCheckBox);
            this.optionsGroupBox.Location = new System.Drawing.Point(1053, 670);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(110, 65);
            this.optionsGroupBox.TabIndex = 5;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // vodsCheckBox
            // 
            this.vodsCheckBox.AutoSize = true;
            this.vodsCheckBox.Location = new System.Drawing.Point(6, 19);
            this.vodsCheckBox.Name = "vodsCheckBox";
            this.vodsCheckBox.Size = new System.Drawing.Size(54, 17);
            this.vodsCheckBox.TabIndex = 6;
            this.vodsCheckBox.Text = "VODs";
            this.vodsCheckBox.UseVisualStyleBackColor = true;
            // 
            // hugeCheckBox
            // 
            this.hugeCheckBox.AutoSize = true;
            this.hugeCheckBox.Checked = true;
            this.hugeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hugeCheckBox.Location = new System.Drawing.Point(6, 42);
            this.hugeCheckBox.Name = "hugeCheckBox";
            this.hugeCheckBox.Size = new System.Drawing.Size(95, 17);
            this.hugeCheckBox.TabIndex = 7;
            this.hugeCheckBox.Text = "Huge Zombies";
            this.hugeCheckBox.UseVisualStyleBackColor = true;
            // 
            // pickablesCheckBox
            // 
            this.pickablesCheckBox.AutoSize = true;
            this.pickablesCheckBox.Checked = true;
            this.pickablesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pickablesCheckBox.Location = new System.Drawing.Point(6, 65);
            this.pickablesCheckBox.Name = "pickablesCheckBox";
            this.pickablesCheckBox.Size = new System.Drawing.Size(72, 17);
            this.pickablesCheckBox.TabIndex = 8;
            this.pickablesCheckBox.Text = "Pickables";
            this.pickablesCheckBox.UseVisualStyleBackColor = true;
            // 
            // removablesGroupBox
            // 
            this.removablesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removablesGroupBox.Controls.Add(this.joinableCheckBox);
            this.removablesGroupBox.Controls.Add(this.vodsCheckBox);
            this.removablesGroupBox.Controls.Add(this.pickablesCheckBox);
            this.removablesGroupBox.Controls.Add(this.hugeCheckBox);
            this.removablesGroupBox.Location = new System.Drawing.Point(1053, 553);
            this.removablesGroupBox.Name = "removablesGroupBox";
            this.removablesGroupBox.Size = new System.Drawing.Size(110, 111);
            this.removablesGroupBox.TabIndex = 9;
            this.removablesGroupBox.TabStop = false;
            this.removablesGroupBox.Text = "Show Removables";
            // 
            // joinableCheckBox
            // 
            this.joinableCheckBox.AutoSize = true;
            this.joinableCheckBox.Checked = true;
            this.joinableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.joinableCheckBox.Location = new System.Drawing.Point(6, 88);
            this.joinableCheckBox.Name = "joinableCheckBox";
            this.joinableCheckBox.Size = new System.Drawing.Size(65, 17);
            this.joinableCheckBox.TabIndex = 9;
            this.joinableCheckBox.Text = "Neutrals";
            this.joinableCheckBox.UseVisualStyleBackColor = true;
            // 
            // MapViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.removablesGroupBox);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.layersGroupBox);
            this.Controls.Add(this.zoomLabel);
            this.Controls.Add(this.zoomTrackBar);
            this.Controls.Add(this.mapPanel);
            this.Name = "MapViewerControl";
            this.Size = new System.Drawing.Size(1166, 1048);
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.mapPanel.ResumeLayout(false);
            this.mapPanel.PerformLayout();
            this.layersGroupBox.ResumeLayout(false);
            this.layersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zombieTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navQuadsTrackBar)).EndInit();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.removablesGroupBox.ResumeLayout(false);
            this.removablesGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar zoomTrackBar;
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.GroupBox layersGroupBox;
        private System.Windows.Forms.CheckBox activityCheckBox;
        private System.Windows.Forms.CheckBox fogCheckBox;
        private System.Windows.Forms.CheckBox terrainCheckBox;
        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.CheckBox navigableCheckBox;
        private System.Windows.Forms.CheckBox distanceCheckBox;
        private System.Windows.Forms.CheckBox directionCheckBox;
        private System.Windows.Forms.CheckBox gridCheckBox;
        private System.Windows.Forms.CheckBox rotateCheckBox;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.CheckBox navQuadsCheckBox;
        private System.Windows.Forms.TrackBar navQuadsTrackBar;
        private System.Windows.Forms.Label navQuadLabel;
        private System.Windows.Forms.Label zombieLabel;
        private System.Windows.Forms.CheckBox zombieCheckBox;
        private System.Windows.Forms.TrackBar zombieTrackBar;
        private System.Windows.Forms.ComboBox zombieComboBox;
        private System.Windows.Forms.CheckBox vodsCheckBox;
        private System.Windows.Forms.CheckBox hugeCheckBox;
        private System.Windows.Forms.CheckBox pickablesCheckBox;
        private System.Windows.Forms.GroupBox removablesGroupBox;
        private System.Windows.Forms.CheckBox joinableCheckBox;
        private System.Windows.Forms.CheckBox swarmsCheckBox;
    }
}
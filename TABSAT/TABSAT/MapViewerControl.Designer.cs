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
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.mapPanel.SuspendLayout();
            this.layersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navQuadsTrackBar)).BeginInit();
            this.optionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // zoomTrackBar
            // 
            this.zoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomTrackBar.LargeChange = 2;
            this.zoomTrackBar.Location = new System.Drawing.Point(1045, 410);
            this.zoomTrackBar.Maximum = 8;
            this.zoomTrackBar.Minimum = 1;
            this.zoomTrackBar.Name = "zoomTrackBar";
            this.zoomTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBar.Size = new System.Drawing.Size(45, 629);
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
            this.mapPanel.Size = new System.Drawing.Size(1027, 1027);
            this.mapPanel.TabIndex = 0;
            // 
            // layersGroupBox
            // 
            this.layersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layersGroupBox.Controls.Add(this.navQuadLabel);
            this.layersGroupBox.Controls.Add(this.navQuadsTrackBar);
            this.layersGroupBox.Controls.Add(this.navQuadsCheckBox);
            this.layersGroupBox.Controls.Add(this.directionCheckBox);
            this.layersGroupBox.Controls.Add(this.navigableCheckBox);
            this.layersGroupBox.Controls.Add(this.activityCheckBox);
            this.layersGroupBox.Controls.Add(this.fogCheckBox);
            this.layersGroupBox.Controls.Add(this.terrainCheckBox);
            this.layersGroupBox.Controls.Add(this.distanceCheckBox);
            this.layersGroupBox.Location = new System.Drawing.Point(1045, 12);
            this.layersGroupBox.Name = "layersGroupBox";
            this.layersGroupBox.Size = new System.Drawing.Size(89, 321);
            this.layersGroupBox.TabIndex = 3;
            this.layersGroupBox.TabStop = false;
            this.layersGroupBox.Text = "Show Layers";
            // 
            // navQuadLabel
            // 
            this.navQuadLabel.AutoSize = true;
            this.navQuadLabel.Location = new System.Drawing.Point(54, 228);
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
            this.navQuadsTrackBar.Value = 4;
            // 
            // navQuadsCheckBox
            // 
            this.navQuadsCheckBox.AutoSize = true;
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
            this.zoomLabel.Location = new System.Drawing.Point(1096, 711);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(34, 26);
            this.zoomLabel.TabIndex = 4;
            this.zoomLabel.Text = "Map\r\nZoom";
            // 
            // rotateCheckBox
            // 
            this.rotateCheckBox.AutoSize = true;
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
            this.optionsGroupBox.Location = new System.Drawing.Point(1045, 339);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(89, 65);
            this.optionsGroupBox.TabIndex = 5;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // MapViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.layersGroupBox);
            this.Controls.Add(this.zoomLabel);
            this.Controls.Add(this.zoomTrackBar);
            this.Controls.Add(this.mapPanel);
            this.Name = "MapViewerControl";
            this.Size = new System.Drawing.Size(1146, 1048);
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.mapPanel.ResumeLayout(false);
            this.mapPanel.PerformLayout();
            this.layersGroupBox.ResumeLayout(false);
            this.layersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navQuadsTrackBar)).EndInit();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
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
    }
}
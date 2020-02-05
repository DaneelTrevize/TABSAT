using System;

namespace TABSAT
{
    partial class MainWindow
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
            this.cloudGroupBox = new System.Windows.Forms.GroupBox();
            this.cloudDisableLabel = new System.Windows.Forms.Label();
            this.updatesLabel = new System.Windows.Forms.Label();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.propertiesPictureBox = new System.Windows.Forms.PictureBox();
            this.updatesPictureBox = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.beforeUsingTabPage = new System.Windows.Forms.TabPage();
            this.saveEditorTabPage = new System.Windows.Forms.TabPage();
            this.autoBackupTabPage = new System.Windows.Forms.TabPage();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.tabsLogSplitContainer = new System.Windows.Forms.SplitContainer();
            this.cloudGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.beforeUsingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabsLogSplitContainer)).BeginInit();
            this.tabsLogSplitContainer.Panel1.SuspendLayout();
            this.tabsLogSplitContainer.Panel2.SuspendLayout();
            this.tabsLogSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cloudGroupBox
            // 
            this.cloudGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.cloudGroupBox.Controls.Add(this.cloudDisableLabel);
            this.cloudGroupBox.Controls.Add(this.updatesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesPictureBox);
            this.cloudGroupBox.Controls.Add(this.updatesPictureBox);
            this.cloudGroupBox.Location = new System.Drawing.Point(3, 6);
            this.cloudGroupBox.Name = "cloudGroupBox";
            this.cloudGroupBox.Size = new System.Drawing.Size(796, 572);
            this.cloudGroupBox.TabIndex = 0;
            this.cloudGroupBox.TabStop = false;
            this.cloudGroupBox.Text = "Please ensure Steam Cloud sync for TAB is NOT enabled";
            // 
            // cloudDisableLabel
            // 
            this.cloudDisableLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cloudDisableLabel.Location = new System.Drawing.Point(59, 421);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(216, 42);
            this.cloudDisableLabel.TabIndex = 2;
            this.cloudDisableLabel.Text = "Steam Cloud being disabled for TAB cannot be automatically verified at this time." +
    "";
            this.cloudDisableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updatesLabel
            // 
            this.updatesLabel.AutoSize = true;
            this.updatesLabel.Location = new System.Drawing.Point(288, 28);
            this.updatesLabel.Name = "updatesLabel";
            this.updatesLabel.Size = new System.Drawing.Size(124, 13);
            this.updatesLabel.TabIndex = 1;
            this.updatesLabel.Text = "2) View the Updates tab:";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(30, 28);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(197, 13);
            this.propertiesLabel.TabIndex = 0;
            this.propertiesLabel.Text = "1) Right-click TAB in your Steam Library:";
            // 
            // propertiesPictureBox
            // 
            this.propertiesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_properties_menu;
            this.propertiesPictureBox.Location = new System.Drawing.Point(30, 44);
            this.propertiesPictureBox.Name = "propertiesPictureBox";
            this.propertiesPictureBox.Size = new System.Drawing.Size(255, 257);
            this.propertiesPictureBox.TabIndex = 0;
            this.propertiesPictureBox.TabStop = false;
            // 
            // updatesPictureBox
            // 
            this.updatesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_updates_menu;
            this.updatesPictureBox.Location = new System.Drawing.Point(291, 44);
            this.updatesPictureBox.Name = "updatesPictureBox";
            this.updatesPictureBox.Size = new System.Drawing.Size(457, 520);
            this.updatesPictureBox.TabIndex = 1;
            this.updatesPictureBox.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.beforeUsingTabPage);
            this.tabControl1.Controls.Add(this.saveEditorTabPage);
            this.tabControl1.Controls.Add(this.autoBackupTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.MinimumSize = new System.Drawing.Size(820, 610);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(820, 610);
            this.tabControl1.TabIndex = 0;
            // 
            // beforeUsingTabPage
            // 
            this.beforeUsingTabPage.Controls.Add(this.cloudGroupBox);
            this.beforeUsingTabPage.Location = new System.Drawing.Point(4, 22);
            this.beforeUsingTabPage.Name = "beforeUsingTabPage";
            this.beforeUsingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.beforeUsingTabPage.Size = new System.Drawing.Size(812, 584);
            this.beforeUsingTabPage.TabIndex = 0;
            this.beforeUsingTabPage.Text = " Before using TABSAT ";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // saveEditorTabPage
            // 
            this.saveEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveEditorTabPage.Name = "saveEditorTabPage";
            this.saveEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveEditorTabPage.Size = new System.Drawing.Size(812, 584);
            this.saveEditorTabPage.TabIndex = 1;
            this.saveEditorTabPage.Text = " Modify Save Files ";
            this.saveEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // autoBackupTabPage
            // 
            this.autoBackupTabPage.Location = new System.Drawing.Point(4, 22);
            this.autoBackupTabPage.Name = "autoBackupTabPage";
            this.autoBackupTabPage.Size = new System.Drawing.Size(812, 584);
            this.autoBackupTabPage.TabIndex = 2;
            this.autoBackupTabPage.Text = "Automatically Backup Save Files";
            this.autoBackupTabPage.UseVisualStyleBackColor = true;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusTextBox.Location = new System.Drawing.Point(0, 3);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusTextBox.Size = new System.Drawing.Size(820, 82);
            this.statusTextBox.TabIndex = 1;
            this.statusTextBox.Text = "Latest version available at:\thttps://github.com/DaneelTrevize/TABSAT/releases\r\nTA" +
    "BSAT Log:\r\n";
            // 
            // tabsLogSplitContainer
            // 
            this.tabsLogSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsLogSplitContainer.Location = new System.Drawing.Point(12, 12);
            this.tabsLogSplitContainer.Name = "tabsLogSplitContainer";
            this.tabsLogSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // tabsLogSplitContainer.Panel1
            // 
            this.tabsLogSplitContainer.Panel1.Controls.Add(this.tabControl1);
            // 
            // tabsLogSplitContainer.Panel2
            // 
            this.tabsLogSplitContainer.Panel2.Controls.Add(this.statusTextBox);
            this.tabsLogSplitContainer.Size = new System.Drawing.Size(820, 691);
            this.tabsLogSplitContainer.SplitterDistance = 602;
            this.tabsLogSplitContainer.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 715);
            this.Controls.Add(this.tabsLogSplitContainer);
            this.MinimumSize = new System.Drawing.Size(860, 754);
            this.Name = "MainWindow";
            this.Text = "TABSAT - They Are Billions Save Automation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.cloudGroupBox.ResumeLayout(false);
            this.cloudGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.beforeUsingTabPage.ResumeLayout(false);
            this.tabsLogSplitContainer.Panel1.ResumeLayout(false);
            this.tabsLogSplitContainer.Panel2.ResumeLayout(false);
            this.tabsLogSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabsLogSplitContainer)).EndInit();
            this.tabsLogSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox propertiesPictureBox;
        private System.Windows.Forms.PictureBox updatesPictureBox;
        private System.Windows.Forms.GroupBox cloudGroupBox;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label updatesLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage beforeUsingTabPage;
        private System.Windows.Forms.TabPage saveEditorTabPage;
        private System.Windows.Forms.Label cloudDisableLabel;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.TabPage autoBackupTabPage;
        private System.Windows.Forms.SplitContainer tabsLogSplitContainer;
    }
}


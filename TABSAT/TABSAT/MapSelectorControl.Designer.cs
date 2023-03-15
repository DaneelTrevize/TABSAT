namespace TABSAT
{
    partial class MapSelectorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mapFileTextBox = new System.Windows.Forms.TextBox();
            this.mapFileChooseButton = new System.Windows.Forms.Button();
            this.mapOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // mapFileTextBox
            // 
            this.mapFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mapFileTextBox.Location = new System.Drawing.Point(20, 38);
            this.mapFileTextBox.Name = "mapFileTextBox";
            this.mapFileTextBox.ReadOnly = true;
            this.mapFileTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mapFileTextBox.Size = new System.Drawing.Size(211, 20);
            this.mapFileTextBox.TabIndex = 1;
            this.mapFileTextBox.TabStop = false;
            this.mapFileTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.mapFileTextBox.WordWrap = false;
            // 
            // mapFileChooseButton
            // 
            this.mapFileChooseButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.mapFileChooseButton.Location = new System.Drawing.Point(237, 32);
            this.mapFileChooseButton.Name = "mapFileChooseButton";
            this.mapFileChooseButton.Size = new System.Drawing.Size(195, 30);
            this.mapFileChooseButton.TabIndex = 2;
            this.mapFileChooseButton.Text = "Choose an extracted Save Data File...";
            this.mapFileChooseButton.UseVisualStyleBackColor = true;
            this.mapFileChooseButton.Click += new System.EventHandler(this.mapFileChooseButton_Click);
            // 
            // MapSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapFileTextBox);
            this.Controls.Add(this.mapFileChooseButton);
            this.MinimumSize = new System.Drawing.Size(580, 290);
            this.Name = "MapSelectorControl";
            this.Size = new System.Drawing.Size(580, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mapFileTextBox;
        private System.Windows.Forms.Button mapFileChooseButton;
        private System.Windows.Forms.OpenFileDialog mapOpenFileDialog;
    }
}

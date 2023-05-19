namespace TABSAT
{
    partial class AreaSelectorControl
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
            this.nothingRadioButton = new System.Windows.Forms.RadioButton();
            this.sectionsRadioButton = new System.Windows.Forms.RadioButton();
            this.radiusRadioButton = new System.Windows.Forms.RadioButton();
            this.boundaryComboBox = new System.Windows.Forms.ComboBox();
            this.radiusLabel = new System.Windows.Forms.Label();
            this.radiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.everywhereRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.radiusNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // nothingRadioButton
            // 
            this.nothingRadioButton.AutoSize = true;
            this.nothingRadioButton.Checked = true;
            this.nothingRadioButton.Location = new System.Drawing.Point(82, 0);
            this.nothingRadioButton.Name = "nothingRadioButton";
            this.nothingRadioButton.Size = new System.Drawing.Size(90, 17);
            this.nothingRadioButton.TabIndex = 0;
            this.nothingRadioButton.TabStop = true;
            this.nothingRadioButton.Text = "Do not modify";
            this.nothingRadioButton.UseVisualStyleBackColor = true;
            // 
            // sectionsRadioButton
            // 
            this.sectionsRadioButton.AutoSize = true;
            this.sectionsRadioButton.Enabled = false;
            this.sectionsRadioButton.Location = new System.Drawing.Point(82, 46);
            this.sectionsRadioButton.Name = "sectionsRadioButton";
            this.sectionsRadioButton.Size = new System.Drawing.Size(128, 17);
            this.sectionsRadioButton.TabIndex = 1;
            this.sectionsRadioButton.TabStop = true;
            this.sectionsRadioButton.Text = "Modify within sections";
            this.sectionsRadioButton.UseVisualStyleBackColor = true;
            // 
            // radiusRadioButton
            // 
            this.radiusRadioButton.AutoSize = true;
            this.radiusRadioButton.Location = new System.Drawing.Point(82, 71);
            this.radiusRadioButton.Name = "radiusRadioButton";
            this.radiusRadioButton.Size = new System.Drawing.Size(14, 13);
            this.radiusRadioButton.TabIndex = 2;
            this.radiusRadioButton.TabStop = true;
            this.radiusRadioButton.UseVisualStyleBackColor = true;
            // 
            // boundaryComboBox
            // 
            this.boundaryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boundaryComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boundaryComboBox.FormattingEnabled = true;
            this.boundaryComboBox.Items.AddRange(new object[] {
            "Within",
            "Beyond"});
            this.boundaryComboBox.Location = new System.Drawing.Point(97, 67);
            this.boundaryComboBox.Name = "boundaryComboBox";
            this.boundaryComboBox.Size = new System.Drawing.Size(59, 21);
            this.boundaryComboBox.TabIndex = 3;
            // 
            // radiusLabel
            // 
            this.radiusLabel.AutoSize = true;
            this.radiusLabel.Location = new System.Drawing.Point(156, 71);
            this.radiusLabel.Name = "radiusLabel";
            this.radiusLabel.Size = new System.Drawing.Size(35, 13);
            this.radiusLabel.TabIndex = 4;
            this.radiusLabel.Text = "radius";
            // 
            // radiusNumericUpDown
            // 
            this.radiusNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.radiusNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.radiusNumericUpDown.Location = new System.Drawing.Point(189, 69);
            this.radiusNumericUpDown.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.radiusNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.radiusNumericUpDown.Name = "radiusNumericUpDown";
            this.radiusNumericUpDown.Size = new System.Drawing.Size(35, 20);
            this.radiusNumericUpDown.TabIndex = 5;
            this.radiusNumericUpDown.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Location = new System.Drawing.Point(1, 1);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(75, 85);
            this.mapPictureBox.TabIndex = 6;
            this.mapPictureBox.TabStop = false;
            // 
            // everywhereRadioButton
            // 
            this.everywhereRadioButton.AutoSize = true;
            this.everywhereRadioButton.Location = new System.Drawing.Point(82, 23);
            this.everywhereRadioButton.Name = "everywhereRadioButton";
            this.everywhereRadioButton.Size = new System.Drawing.Size(114, 17);
            this.everywhereRadioButton.TabIndex = 7;
            this.everywhereRadioButton.TabStop = true;
            this.everywhereRadioButton.Text = "Modify everywhere";
            this.everywhereRadioButton.UseVisualStyleBackColor = true;
            // 
            // AreaSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.everywhereRadioButton);
            this.Controls.Add(this.mapPictureBox);
            this.Controls.Add(this.radiusNumericUpDown);
            this.Controls.Add(this.radiusLabel);
            this.Controls.Add(this.boundaryComboBox);
            this.Controls.Add(this.radiusRadioButton);
            this.Controls.Add(this.sectionsRadioButton);
            this.Controls.Add(this.nothingRadioButton);
            this.Name = "AreaSelectorControl";
            this.Size = new System.Drawing.Size(225, 90);
            ((System.ComponentModel.ISupportInitialize)(this.radiusNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label radiusLabel;
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.RadioButton sectionsRadioButton;
        private System.Windows.Forms.ComboBox boundaryComboBox;
        private System.Windows.Forms.NumericUpDown radiusNumericUpDown;
        private System.Windows.Forms.RadioButton radiusRadioButton;
        private System.Windows.Forms.RadioButton nothingRadioButton;
        private System.Windows.Forms.RadioButton everywhereRadioButton;
    }
}

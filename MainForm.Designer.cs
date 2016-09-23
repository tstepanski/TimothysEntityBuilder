namespace EntityBuilder
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.DataSourceTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IntegratedSecurityCheckbox = new System.Windows.Forms.CheckBox();
            this.DatabaseComboBox = new System.Windows.Forms.ComboBox();
            this.TableComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.OutputTextbox = new System.Windows.Forms.TextBox();
            this.SchemaCombobox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Server (Format: server\\instance):";
            // 
            // DataSourceTextBox
            // 
            this.DataSourceTextBox.Location = new System.Drawing.Point(15, 25);
            this.DataSourceTextBox.Name = "DataSourceTextBox";
            this.DataSourceTextBox.Size = new System.Drawing.Size(211, 20);
            this.DataSourceTextBox.TabIndex = 0;
            this.DataSourceTextBox.TextChanged += new System.EventHandler(this.DataSourceTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Database:";
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Location = new System.Drawing.Point(15, 64);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.Size = new System.Drawing.Size(211, 20);
            this.UsernameTextbox.TabIndex = 1;
            this.UsernameTextbox.TextChanged += new System.EventHandler(this.UsernameTextbox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username:";
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Location = new System.Drawing.Point(12, 103);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new System.Drawing.Size(211, 20);
            this.PasswordTextbox.TabIndex = 2;
            this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // IntegratedSecurityCheckbox
            // 
            this.IntegratedSecurityCheckbox.AutoSize = true;
            this.IntegratedSecurityCheckbox.Location = new System.Drawing.Point(12, 129);
            this.IntegratedSecurityCheckbox.Name = "IntegratedSecurityCheckbox";
            this.IntegratedSecurityCheckbox.Size = new System.Drawing.Size(121, 17);
            this.IntegratedSecurityCheckbox.TabIndex = 3;
            this.IntegratedSecurityCheckbox.Text = "Integrated Security?";
            this.IntegratedSecurityCheckbox.UseVisualStyleBackColor = true;
            this.IntegratedSecurityCheckbox.CheckedChanged += new System.EventHandler(this.IntegratedSecurityCheckbox_CheckedChanged);
            // 
            // DatabaseComboBox
            // 
            this.DatabaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DatabaseComboBox.Enabled = false;
            this.DatabaseComboBox.FormattingEnabled = true;
            this.DatabaseComboBox.Location = new System.Drawing.Point(12, 165);
            this.DatabaseComboBox.Name = "DatabaseComboBox";
            this.DatabaseComboBox.Size = new System.Drawing.Size(208, 21);
            this.DatabaseComboBox.TabIndex = 4;
            this.DatabaseComboBox.SelectedIndexChanged += new System.EventHandler(this.DatabaseComboBox_SelectedIndexChanged);
            // 
            // TableComboBox
            // 
            this.TableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TableComboBox.Enabled = false;
            this.TableComboBox.FormattingEnabled = true;
            this.TableComboBox.Location = new System.Drawing.Point(12, 245);
            this.TableComboBox.Name = "TableComboBox";
            this.TableComboBox.Size = new System.Drawing.Size(208, 21);
            this.TableComboBox.TabIndex = 6;
            this.TableComboBox.SelectedIndexChanged += new System.EventHandler(this.TableComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Table:";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Enabled = false;
            this.GenerateButton.Location = new System.Drawing.Point(145, 272);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(75, 23);
            this.GenerateButton.TabIndex = 8;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(64, 272);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 9;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // OutputTextbox
            // 
            this.OutputTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputTextbox.Location = new System.Drawing.Point(232, 12);
            this.OutputTextbox.Multiline = true;
            this.OutputTextbox.Name = "OutputTextbox";
            this.OutputTextbox.ReadOnly = true;
            this.OutputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.OutputTextbox.Size = new System.Drawing.Size(333, 389);
            this.OutputTextbox.TabIndex = 13;
            this.OutputTextbox.WordWrap = false;
            // 
            // SchemaCombobox
            // 
            this.SchemaCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SchemaCombobox.Enabled = false;
            this.SchemaCombobox.FormattingEnabled = true;
            this.SchemaCombobox.Location = new System.Drawing.Point(12, 205);
            this.SchemaCombobox.Name = "SchemaCombobox";
            this.SchemaCombobox.Size = new System.Drawing.Size(208, 21);
            this.SchemaCombobox.TabIndex = 5;
            this.SchemaCombobox.SelectedIndexChanged += new System.EventHandler(this.SchemaCombobox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Schema:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 413);
            this.Controls.Add(this.SchemaCombobox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.OutputTextbox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.TableComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DatabaseComboBox);
            this.Controls.Add(this.IntegratedSecurityCheckbox);
            this.Controls.Add(this.PasswordTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UsernameTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DataSourceTextBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Timothy\'s Entity Builder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DataSourceTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox IntegratedSecurityCheckbox;
        private System.Windows.Forms.ComboBox DatabaseComboBox;
        private System.Windows.Forms.ComboBox TableComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.TextBox OutputTextbox;
        private System.Windows.Forms.ComboBox SchemaCombobox;
        private System.Windows.Forms.Label label6;
    }
}


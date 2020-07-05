namespace GraphtreonComment
{
    partial class AddTextForm
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
            this.labelDesc = new System.Windows.Forms.Label();
            this.BtnOk = new System.Windows.Forms.Button();
            this.txtString = new System.Windows.Forms.TextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelDesc.Location = new System.Drawing.Point(13, 13);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(142, 17);
            this.labelDesc.TabIndex = 0;
            this.labelDesc.Text = "Please enter the text.";
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(47, 59);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // txtString
            // 
            this.txtString.Location = new System.Drawing.Point(12, 33);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(260, 20);
            this.txtString.TabIndex = 0;
            this.txtString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtString_KeyDown);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(158, 59);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // AddTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(284, 88);
            this.Controls.Add(this.txtString);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.labelDesc);
            this.Name = "AddTextForm";
            this.Text = "AddTextForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.TextBox txtString;
        private System.Windows.Forms.Button BtnCancel;
    }
}
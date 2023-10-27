
namespace CalculaNDC
{
    partial class Form1
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
            this.btnNDC = new System.Windows.Forms.Button();
            this.txtOBUID = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNDC
            // 
            this.btnNDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNDC.Location = new System.Drawing.Point(101, 66);
            this.btnNDC.Name = "btnNDC";
            this.btnNDC.Size = new System.Drawing.Size(312, 70);
            this.btnNDC.TabIndex = 0;
            this.btnNDC.Text = "Calcula NDC";
            this.btnNDC.UseVisualStyleBackColor = true;
            this.btnNDC.Click += new System.EventHandler(this.btnNDC_Click);
            // 
            // txtOBUID
            // 
            this.txtOBUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOBUID.Location = new System.Drawing.Point(101, 159);
            this.txtOBUID.Name = "txtOBUID";
            this.txtOBUID.Size = new System.Drawing.Size(312, 30);
            this.txtOBUID.TabIndex = 1;
            this.txtOBUID.Tag = "";
            this.txtOBUID.Text = "Enter your OBUID";
            this.txtOBUID.Enter += new System.EventHandler(this.txtOBUID_Enter);
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(101, 215);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(312, 30);
            this.txtResult.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 344);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtOBUID);
            this.Controls.Add(this.btnNDC);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNDC;
        private System.Windows.Forms.TextBox txtOBUID;
        private System.Windows.Forms.TextBox txtResult;
    }
}


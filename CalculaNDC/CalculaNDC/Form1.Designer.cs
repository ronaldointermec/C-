
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
            this.txtHEX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textNCC = new System.Windows.Forms.TextBox();
            this.txtOBUID2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnNCC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNDC
            // 
            this.btnNDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNDC.Location = new System.Drawing.Point(416, 35);
            this.btnNDC.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNDC.Name = "btnNDC";
            this.btnNDC.Size = new System.Drawing.Size(162, 106);
            this.btnNDC.TabIndex = 0;
            this.btnNDC.Text = "To HEX";
            this.btnNDC.UseVisualStyleBackColor = true;
            this.btnNDC.Click += new System.EventHandler(this.btnNDC_Click);
            // 
            // txtOBUID
            // 
            this.txtOBUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOBUID.Location = new System.Drawing.Point(177, 35);
            this.txtOBUID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtOBUID.Name = "txtOBUID";
            this.txtOBUID.Size = new System.Drawing.Size(235, 26);
            this.txtOBUID.TabIndex = 1;
            this.txtOBUID.Tag = "";
            this.txtOBUID.Text = "Enter your OBUID";
            this.txtOBUID.Enter += new System.EventHandler(this.txtOBUID_Enter);
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(177, 76);
            this.txtResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(235, 26);
            this.txtResult.TabIndex = 1;
            // 
            // txtHEX
            // 
            this.txtHEX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHEX.Location = new System.Drawing.Point(177, 115);
            this.txtHEX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHEX.Name = "txtHEX";
            this.txtHEX.Size = new System.Drawing.Size(235, 26);
            this.txtHEX.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "OBUID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "NCC:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(46, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "OBUID HEX:";
            // 
            // textNCC
            // 
            this.textNCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNCC.Location = new System.Drawing.Point(177, 169);
            this.textNCC.Margin = new System.Windows.Forms.Padding(2);
            this.textNCC.Name = "textNCC";
            this.textNCC.Size = new System.Drawing.Size(235, 26);
            this.textNCC.TabIndex = 1;
            this.textNCC.Tag = "";
            this.textNCC.Text = "Enter your NCC";
            this.textNCC.Enter += new System.EventHandler(this.textNCC_Enter);
            // 
            // txtOBUID2
            // 
            this.txtOBUID2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOBUID2.Location = new System.Drawing.Point(177, 210);
            this.txtOBUID2.Margin = new System.Windows.Forms.Padding(2);
            this.txtOBUID2.Name = "txtOBUID2";
            this.txtOBUID2.Size = new System.Drawing.Size(235, 26);
            this.txtOBUID2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "NCC:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(46, 214);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "OBUID:";
            // 
            // btnNCC
            // 
            this.btnNCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNCC.Location = new System.Drawing.Point(416, 169);
            this.btnNCC.Margin = new System.Windows.Forms.Padding(2);
            this.btnNCC.Name = "btnNCC";
            this.btnNCC.Size = new System.Drawing.Size(162, 67);
            this.btnNCC.TabIndex = 0;
            this.btnNCC.Text = "From HEX";
            this.btnNCC.UseVisualStyleBackColor = true;
            this.btnNCC.Click += new System.EventHandler(this.btnNCC_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 281);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHEX);
            this.Controls.Add(this.txtOBUID2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.textNCC);
            this.Controls.Add(this.txtOBUID);
            this.Controls.Add(this.btnNCC);
            this.Controls.Add(this.btnNDC);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conversor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNDC;
        private System.Windows.Forms.TextBox txtOBUID;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtHEX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textNCC;
        private System.Windows.Forms.TextBox txtOBUID2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnNCC;
    }
}


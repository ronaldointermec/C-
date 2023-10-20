
namespace Polymorphism
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn1Parameter = new System.Windows.Forms.Button();
            this.btn2Parameter = new System.Windows.Forms.Button();
            this.btn3Parameter = new System.Windows.Forms.Button();
            this.btn4Parameter = new System.Windows.Forms.Button();
            this.btnVirtual = new System.Windows.Forms.Button();
            this.btnOveride = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(245, 389);
            this.textBox1.TabIndex = 0;
            // 
            // btn1Parameter
            // 
            this.btn1Parameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1Parameter.Location = new System.Drawing.Point(287, 22);
            this.btn1Parameter.Name = "btn1Parameter";
            this.btn1Parameter.Size = new System.Drawing.Size(148, 55);
            this.btn1Parameter.TabIndex = 1;
            this.btn1Parameter.Text = "1 Parameter";
            this.btn1Parameter.UseVisualStyleBackColor = true;
            this.btn1Parameter.Click += new System.EventHandler(this.btn1Parameter_Click);
            // 
            // btn2Parameter
            // 
            this.btn2Parameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2Parameter.Location = new System.Drawing.Point(441, 22);
            this.btn2Parameter.Name = "btn2Parameter";
            this.btn2Parameter.Size = new System.Drawing.Size(148, 55);
            this.btn2Parameter.TabIndex = 1;
            this.btn2Parameter.Text = "2 Parameter";
            this.btn2Parameter.UseVisualStyleBackColor = true;
            this.btn2Parameter.Click += new System.EventHandler(this.btn2Parameter_Click);
            // 
            // btn3Parameter
            // 
            this.btn3Parameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3Parameter.Location = new System.Drawing.Point(287, 83);
            this.btn3Parameter.Name = "btn3Parameter";
            this.btn3Parameter.Size = new System.Drawing.Size(148, 55);
            this.btn3Parameter.TabIndex = 1;
            this.btn3Parameter.Text = "3 Parameter";
            this.btn3Parameter.UseVisualStyleBackColor = true;
            this.btn3Parameter.Click += new System.EventHandler(this.btn3Parameter_Click);
            // 
            // btn4Parameter
            // 
            this.btn4Parameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4Parameter.Location = new System.Drawing.Point(441, 83);
            this.btn4Parameter.Name = "btn4Parameter";
            this.btn4Parameter.Size = new System.Drawing.Size(148, 55);
            this.btn4Parameter.TabIndex = 1;
            this.btn4Parameter.Text = "4 Parameter";
            this.btn4Parameter.UseVisualStyleBackColor = true;
            this.btn4Parameter.Click += new System.EventHandler(this.btn4Parameter_Click);
            // 
            // btnVirtual
            // 
            this.btnVirtual.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVirtual.Location = new System.Drawing.Point(287, 144);
            this.btnVirtual.Name = "btnVirtual";
            this.btnVirtual.Size = new System.Drawing.Size(148, 55);
            this.btnVirtual.TabIndex = 1;
            this.btnVirtual.Text = "Virtual";
            this.btnVirtual.UseVisualStyleBackColor = true;
            this.btnVirtual.Click += new System.EventHandler(this.btnVirtual_Click);
            // 
            // btnOveride
            // 
            this.btnOveride.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOveride.Location = new System.Drawing.Point(441, 144);
            this.btnOveride.Name = "btnOveride";
            this.btnOveride.Size = new System.Drawing.Size(148, 55);
            this.btnOveride.TabIndex = 1;
            this.btnOveride.Text = "Overide";
            this.btnOveride.UseVisualStyleBackColor = true;
            this.btnOveride.Click += new System.EventHandler(this.btnOveride_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOveride);
            this.Controls.Add(this.btnVirtual);
            this.Controls.Add(this.btn4Parameter);
            this.Controls.Add(this.btn3Parameter);
            this.Controls.Add(this.btn2Parameter);
            this.Controls.Add(this.btn1Parameter);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn1Parameter;
        private System.Windows.Forms.Button btn2Parameter;
        private System.Windows.Forms.Button btn3Parameter;
        private System.Windows.Forms.Button btn4Parameter;
        private System.Windows.Forms.Button btnVirtual;
        private System.Windows.Forms.Button btnOveride;
    }
}


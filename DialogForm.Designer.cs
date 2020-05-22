namespace PraugeParkingFrontEnd
{
    partial class DialogForm
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.txtReg = new System.Windows.Forms.TextBox();
            this.rbnCar = new System.Windows.Forms.RadioButton();
            this.rbnMC = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "What do?";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 58);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(92, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(110, 58);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(92, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(208, 58);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(92, 23);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // txtReg
            // 
            this.txtReg.Location = new System.Drawing.Point(79, 33);
            this.txtReg.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.txtReg.MaxLength = 100;
            this.txtReg.Name = "txtReg";
            this.txtReg.Size = new System.Drawing.Size(76, 20);
            this.txtReg.TabIndex = 14;
            // 
            // rbnCar
            // 
            this.rbnCar.AutoSize = true;
            this.rbnCar.Location = new System.Drawing.Point(169, 34);
            this.rbnCar.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.rbnCar.Name = "rbnCar";
            this.rbnCar.Size = new System.Drawing.Size(41, 17);
            this.rbnCar.TabIndex = 15;
            this.rbnCar.Text = "Car";
            this.rbnCar.UseVisualStyleBackColor = true;
            // 
            // rbnMC
            // 
            this.rbnMC.AutoSize = true;
            this.rbnMC.Location = new System.Drawing.Point(212, 34);
            this.rbnMC.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.rbnMC.Name = "rbnMC";
            this.rbnMC.Size = new System.Drawing.Size(41, 17);
            this.rbnMC.TabIndex = 16;
            this.rbnMC.Text = "MC";
            this.rbnMC.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "RegNr";
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 92);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReg);
            this.Controls.Add(this.rbnCar);
            this.Controls.Add(this.rbnMC);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Name = "DialogForm";
            this.Text = "DialogForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox txtReg;
        private System.Windows.Forms.RadioButton rbnCar;
        private System.Windows.Forms.RadioButton rbnMC;
        private System.Windows.Forms.Label label2;
    }
}
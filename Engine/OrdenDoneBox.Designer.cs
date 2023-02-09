
namespace Engine
{
    partial class OrdenDoneBox
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
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nroDeOrden = new System.Windows.Forms.Label();
            this.BtnNuevaOrden = new System.Windows.Forms.Button();
            this.BtnKeepOrder = new System.Windows.Forms.Button();
            this.btnDeleteOrder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rectangleShape2.BorderColor = System.Drawing.SystemColors.Desktop;
            this.rectangleShape2.Enabled = false;
            this.rectangleShape2.FillColor = System.Drawing.SystemColors.ButtonFace;
            this.rectangleShape2.FillGradientColor = System.Drawing.Color.Orange;
            this.rectangleShape2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectangleShape2.Location = new System.Drawing.Point(63, 33);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(574, 335);
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rectangleShape1.BorderColor = System.Drawing.SystemColors.Desktop;
            this.rectangleShape1.BorderWidth = 3;
            this.rectangleShape1.Enabled = false;
            this.rectangleShape1.FillColor = System.Drawing.SystemColors.ButtonShadow;
            this.rectangleShape1.FillGradientColor = System.Drawing.Color.LimeGreen;
            this.rectangleShape1.FillGradientStyle = Microsoft.VisualBasic.PowerPacks.FillGradientStyle.Central;
            this.rectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectangleShape1.Location = new System.Drawing.Point(1, 1);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(697, 398);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape3,
            this.rectangleShape1,
            this.rectangleShape2});
            this.shapeContainer1.Size = new System.Drawing.Size(700, 400);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape3
            // 
            this.rectangleShape3.BackColor = System.Drawing.Color.White;
            this.rectangleShape3.BorderColor = System.Drawing.SystemColors.Desktop;
            this.rectangleShape3.Enabled = false;
            this.rectangleShape3.FillColor = System.Drawing.Color.White;
            this.rectangleShape3.FillGradientColor = System.Drawing.Color.Orange;
            this.rectangleShape3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectangleShape3.Location = new System.Drawing.Point(12, 11);
            this.rectangleShape3.Name = "rectangleShape3";
            this.rectangleShape3.Size = new System.Drawing.Size(675, 377);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 22.25F);
            this.label1.Location = new System.Drawing.Point(65, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(569, 92);
            this.label1.TabIndex = 1;
            this.label1.Text = "Listo! Orden generada y cargada en dropbox!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 18.25F);
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(245, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 44);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nro";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nroDeOrden
            // 
            this.nroDeOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.nroDeOrden.Font = new System.Drawing.Font("Nirmala UI", 19F, System.Drawing.FontStyle.Bold);
            this.nroDeOrden.ForeColor = System.Drawing.Color.Crimson;
            this.nroDeOrden.Location = new System.Drawing.Point(328, 145);
            this.nroDeOrden.Name = "nroDeOrden";
            this.nroDeOrden.Size = new System.Drawing.Size(120, 44);
            this.nroDeOrden.TabIndex = 1;
            this.nroDeOrden.Text = "0000";
            this.nroDeOrden.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnNuevaOrden
            // 
            this.BtnNuevaOrden.BackColor = System.Drawing.Color.LimeGreen;
            this.BtnNuevaOrden.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnNuevaOrden.Font = new System.Drawing.Font("Nirmala UI", 16.25F);
            this.BtnNuevaOrden.ForeColor = System.Drawing.Color.White;
            this.BtnNuevaOrden.Location = new System.Drawing.Point(65, 222);
            this.BtnNuevaOrden.Name = "BtnNuevaOrden";
            this.BtnNuevaOrden.Size = new System.Drawing.Size(569, 40);
            this.BtnNuevaOrden.TabIndex = 2;
            this.BtnNuevaOrden.Text = "Generar una Nueva Orden";
            this.BtnNuevaOrden.UseVisualStyleBackColor = false;
            this.BtnNuevaOrden.Click += new System.EventHandler(this.BtnNuevaOrden_Click);
            // 
            // BtnKeepOrder
            // 
            this.BtnKeepOrder.BackColor = System.Drawing.Color.RoyalBlue;
            this.BtnKeepOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnKeepOrder.Font = new System.Drawing.Font("Nirmala UI", 16.25F);
            this.BtnKeepOrder.ForeColor = System.Drawing.Color.White;
            this.BtnKeepOrder.Location = new System.Drawing.Point(65, 268);
            this.BtnKeepOrder.Name = "BtnKeepOrder";
            this.BtnKeepOrder.Size = new System.Drawing.Size(569, 40);
            this.BtnKeepOrder.TabIndex = 2;
            this.BtnKeepOrder.Text = "Generar otra Orden ( a partir de esta ultima )";
            this.BtnKeepOrder.UseVisualStyleBackColor = false;
            this.BtnKeepOrder.Click += new System.EventHandler(this.BtnKeepOrder_Click);
            // 
            // btnDeleteOrder
            // 
            this.btnDeleteOrder.BackColor = System.Drawing.Color.Crimson;
            this.btnDeleteOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteOrder.Font = new System.Drawing.Font("Nirmala UI", 16.25F);
            this.btnDeleteOrder.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOrder.Location = new System.Drawing.Point(65, 314);
            this.btnDeleteOrder.Name = "btnDeleteOrder";
            this.btnDeleteOrder.Size = new System.Drawing.Size(569, 40);
            this.btnDeleteOrder.TabIndex = 2;
            this.btnDeleteOrder.Text = "Borrarla";
            this.btnDeleteOrder.UseVisualStyleBackColor = false;
            this.btnDeleteOrder.Click += new System.EventHandler(this.btnDeleteOrder_Click);
            // 
            // OrdenDoneBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 400);
            this.Controls.Add(this.btnDeleteOrder);
            this.Controls.Add(this.BtnKeepOrder);
            this.Controls.Add(this.BtnNuevaOrden);
            this.Controls.Add(this.nroDeOrden);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shapeContainer1);
            this.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrdenDoneBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OrdenDoneBox";
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label nroDeOrden;
        private System.Windows.Forms.Button BtnNuevaOrden;
        private System.Windows.Forms.Button BtnKeepOrder;
        private System.Windows.Forms.Button btnDeleteOrder;
    }
}
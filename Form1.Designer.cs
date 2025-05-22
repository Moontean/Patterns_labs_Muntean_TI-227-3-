namespace SimpleGraphicEditor
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.canvasPictureBox = new System.Windows.Forms.PictureBox();
            this.btnPencil = new System.Windows.Forms.Button();
            this.btnRectangle = new System.Windows.Forms.Button();
            this.btnEllipse = new System.Windows.Forms.Button();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnFillColor = new System.Windows.Forms.Button();
            this.chkFill = new System.Windows.Forms.CheckBox();
            this.btnGroup = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnSaveState = new System.Windows.Forms.Button();
            this.btnRestoreState = new System.Windows.Forms.Button();
            this.renderComboBox = new System.Windows.Forms.ComboBox();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // canvasPictureBox
            // 
            this.canvasPictureBox.Location = new System.Drawing.Point(16, 62);
            this.canvasPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.canvasPictureBox.Name = "canvasPictureBox";
            this.canvasPictureBox.Size = new System.Drawing.Size(1013, 468);
            this.canvasPictureBox.TabIndex = 0;
            this.canvasPictureBox.TabStop = false;
            this.canvasPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasPictureBox_Paint);
            this.canvasPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseDown);
            this.canvasPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseMove);
            this.canvasPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseUp);
            // 
            // btnPencil
            // 
            this.btnPencil.Location = new System.Drawing.Point(16, 15);
            this.btnPencil.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(100, 28);
            this.btnPencil.TabIndex = 1;
            this.btnPencil.Text = "Pencil";
            this.btnPencil.UseVisualStyleBackColor = true;
            this.btnPencil.Click += new System.EventHandler(this.btnPencil_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Location = new System.Drawing.Point(124, 15);
            this.btnRectangle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(100, 28);
            this.btnRectangle.TabIndex = 2;
            this.btnRectangle.Text = "Rectangle";
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // btnEllipse
            // 
            this.btnEllipse.Location = new System.Drawing.Point(232, 15);
            this.btnEllipse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Size = new System.Drawing.Size(100, 28);
            this.btnEllipse.TabIndex = 3;
            this.btnEllipse.Text = "Ellipse";
            this.btnEllipse.UseVisualStyleBackColor = true;
            this.btnEllipse.Click += new System.EventHandler(this.btnEllipse_Click);
            // 
            // btnColor
            // 
            this.btnColor.Location = new System.Drawing.Point(340, 15);
            this.btnColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(100, 28);
            this.btnColor.TabIndex = 4;
            this.btnColor.Text = "Color";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnFillColor
            // 
            this.btnFillColor.Location = new System.Drawing.Point(448, 15);
            this.btnFillColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFillColor.Name = "btnFillColor";
            this.btnFillColor.Size = new System.Drawing.Size(100, 28);
            this.btnFillColor.TabIndex = 5;
            this.btnFillColor.Text = "Fill Color";
            this.btnFillColor.UseVisualStyleBackColor = true;
            this.btnFillColor.Click += new System.EventHandler(this.btnFillColor_Click);
            // 
            // chkFill
            // 
            this.chkFill.AutoSize = true;
            this.chkFill.Location = new System.Drawing.Point(556, 18);
            this.chkFill.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkFill.Name = "chkFill";
            this.chkFill.Size = new System.Drawing.Size(46, 20);
            this.chkFill.TabIndex = 6;
            this.chkFill.Text = "Fill";
            this.chkFill.UseVisualStyleBackColor = true;
            this.chkFill.CheckedChanged += new System.EventHandler(this.chkFill_CheckedChanged);
            // 
            // btnGroup
            // 
            this.btnGroup.Location = new System.Drawing.Point(621, 15);
            this.btnGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(100, 28);
            this.btnGroup.TabIndex = 7;
            this.btnGroup.Text = "Group";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(729, 15);
            this.btnClone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(100, 28);
            this.btnClone.TabIndex = 8;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(1071, 215);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(100, 28);
            this.btnUndo.TabIndex = 9;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(1071, 277);
            this.btnRedo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(100, 28);
            this.btnRedo.TabIndex = 10;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnSaveState
            // 
            this.btnSaveState.Location = new System.Drawing.Point(1071, 334);
            this.btnSaveState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveState.Name = "btnSaveState";
            this.btnSaveState.Size = new System.Drawing.Size(100, 28);
            this.btnSaveState.TabIndex = 11;
            this.btnSaveState.Text = "Save State";
            this.btnSaveState.UseVisualStyleBackColor = true;
            this.btnSaveState.Click += new System.EventHandler(this.btnSaveState_Click);
            // 
            // btnRestoreState
            // 
            this.btnRestoreState.Location = new System.Drawing.Point(1071, 396);
            this.btnRestoreState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRestoreState.Name = "btnRestoreState";
            this.btnRestoreState.Size = new System.Drawing.Size(100, 28);
            this.btnRestoreState.TabIndex = 12;
            this.btnRestoreState.Text = "Restore State";
            this.btnRestoreState.UseVisualStyleBackColor = true;
            this.btnRestoreState.Click += new System.EventHandler(this.btnRestoreState_Click);
            // 
            // renderComboBox
            // 
            this.renderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.renderComboBox.FormattingEnabled = true;
            this.renderComboBox.Items.AddRange(new object[] {
            "GDI+",
            "Direct2D"});
            this.renderComboBox.Location = new System.Drawing.Point(858, 16);
            this.renderComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.renderComboBox.Name = "renderComboBox";
            this.renderComboBox.Size = new System.Drawing.Size(160, 24);
            this.renderComboBox.TabIndex = 13;
            this.renderComboBox.SelectedIndexChanged += new System.EventHandler(this.renderComboBox_SelectedIndexChanged);
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(1041, 19);
            this.styleComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(160, 24);
            this.styleComboBox.TabIndex = 14;
            this.styleComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1616, 554);
            this.Controls.Add(this.styleComboBox);
            this.Controls.Add(this.renderComboBox);
            this.Controls.Add(this.btnRestoreState);
            this.Controls.Add(this.btnSaveState);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.btnGroup);
            this.Controls.Add(this.chkFill);
            this.Controls.Add(this.btnFillColor);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.btnEllipse);
            this.Controls.Add(this.btnRectangle);
            this.Controls.Add(this.btnPencil);
            this.Controls.Add(this.canvasPictureBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Simple Graphic Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox canvasPictureBox;
        private System.Windows.Forms.Button btnPencil;
        private System.Windows.Forms.Button btnRectangle;
        private System.Windows.Forms.Button btnEllipse;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Button btnFillColor;
        private System.Windows.Forms.CheckBox chkFill;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Button btnSaveState;
        private System.Windows.Forms.Button btnRestoreState;
        private System.Windows.Forms.ComboBox renderComboBox;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
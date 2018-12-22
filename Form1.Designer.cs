namespace ABBCommTest
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
            this.btn_addCntrl = new System.Windows.Forms.Button();
            this.tbox_ip = new System.Windows.Forms.TextBox();
            this.btn_scanNetwork = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_expose = new System.Windows.Forms.Button();
            this.btnDoWork = new System.Windows.Forms.Button();
            this.tbGridWhereClause = new System.Windows.Forms.TextBox();
            this.btn_loadGrid = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_workfolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_module = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_verfileValue = new System.Windows.Forms.TextBox();
            this.tbVerVarName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_addCntrl
            // 
            this.btn_addCntrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_addCntrl.Location = new System.Drawing.Point(145, 19);
            this.btn_addCntrl.Margin = new System.Windows.Forms.Padding(2);
            this.btn_addCntrl.Name = "btn_addCntrl";
            this.btn_addCntrl.Size = new System.Drawing.Size(118, 19);
            this.btn_addCntrl.TabIndex = 1;
            this.btn_addCntrl.Text = "btn_addCntrl_byIp";
            this.btn_addCntrl.UseVisualStyleBackColor = true;
            this.btn_addCntrl.Click += new System.EventHandler(this.btn_addCtrl_Click);
            // 
            // tbox_ip
            // 
            this.tbox_ip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbox_ip.Location = new System.Drawing.Point(15, 18);
            this.tbox_ip.Margin = new System.Windows.Forms.Padding(2);
            this.tbox_ip.Name = "tbox_ip";
            this.tbox_ip.Size = new System.Drawing.Size(114, 20);
            this.tbox_ip.TabIndex = 2;
            this.tbox_ip.Text = "10.205.94.240";
            // 
            // btn_scanNetwork
            // 
            this.btn_scanNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_scanNetwork.Location = new System.Drawing.Point(427, 34);
            this.btn_scanNetwork.Margin = new System.Windows.Forms.Padding(2);
            this.btn_scanNetwork.Name = "btn_scanNetwork";
            this.btn_scanNetwork.Size = new System.Drawing.Size(113, 19);
            this.btn_scanNetwork.TabIndex = 8;
            this.btn_scanNetwork.Text = "scanNetwork";
            this.btn_scanNetwork.UseVisualStyleBackColor = true;
            this.btn_scanNetwork.Click += new System.EventHandler(this.Btn_scanNetwork_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 10);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1436, 470);
            this.dataGridView1.TabIndex = 10;
            // 
            // btn_expose
            // 
            this.btn_expose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_expose.Location = new System.Drawing.Point(284, 34);
            this.btn_expose.Margin = new System.Windows.Forms.Padding(2);
            this.btn_expose.Name = "btn_expose";
            this.btn_expose.Size = new System.Drawing.Size(118, 19);
            this.btn_expose.TabIndex = 11;
            this.btn_expose.Text = "exposeDatagridToNet";
            this.btn_expose.UseVisualStyleBackColor = true;
            this.btn_expose.Click += new System.EventHandler(this.Btn_expose_Click);
            // 
            // btnDoWork
            // 
            this.btnDoWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDoWork.Location = new System.Drawing.Point(1167, 504);
            this.btnDoWork.Margin = new System.Windows.Forms.Padding(2);
            this.btnDoWork.Name = "btnDoWork";
            this.btnDoWork.Size = new System.Drawing.Size(122, 57);
            this.btnDoWork.TabIndex = 13;
            this.btnDoWork.Text = "DO SELECTED WORK";
            this.btnDoWork.UseVisualStyleBackColor = true;
            this.btnDoWork.Click += new System.EventHandler(this.BtnDoWork_Click);
            // 
            // tbGridWhereClause
            // 
            this.tbGridWhereClause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbGridWhereClause.Location = new System.Drawing.Point(15, 56);
            this.tbGridWhereClause.Margin = new System.Windows.Forms.Padding(2);
            this.tbGridWhereClause.Name = "tbGridWhereClause";
            this.tbGridWhereClause.Size = new System.Drawing.Size(114, 20);
            this.tbGridWhereClause.TabIndex = 14;
            this.tbGridWhereClause.Text = "336020%";
            // 
            // btn_loadGrid
            // 
            this.btn_loadGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_loadGrid.Location = new System.Drawing.Point(145, 56);
            this.btn_loadGrid.Margin = new System.Windows.Forms.Padding(2);
            this.btn_loadGrid.Name = "btn_loadGrid";
            this.btn_loadGrid.Size = new System.Drawing.Size(118, 19);
            this.btn_loadGrid.TabIndex = 15;
            this.btn_loadGrid.Text = "LoadGridFromDB";
            this.btn_loadGrid.UseVisualStyleBackColor = true;
            this.btn_loadGrid.Click += new System.EventHandler(this.Btn_loadGrid_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tbox_ip);
            this.groupBox1.Controls.Add(this.btn_loadGrid);
            this.groupBox1.Controls.Add(this.btn_addCntrl);
            this.groupBox1.Controls.Add(this.tbGridWhereClause);
            this.groupBox1.Controls.Add(this.btn_scanNetwork);
            this.groupBox1.Controls.Add(this.btn_expose);
            this.groupBox1.Location = new System.Drawing.Point(9, 485);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 84);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config robots";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_workfolder);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tb_module);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tb_verfileValue);
            this.groupBox2.Controls.Add(this.tbVerVarName);
            this.groupBox2.Location = new System.Drawing.Point(580, 485);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(534, 86);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LoadModule";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(298, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "workfolder";
            // 
            // tb_workfolder
            // 
            this.tb_workfolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_workfolder.Location = new System.Drawing.Point(360, 15);
            this.tb_workfolder.Margin = new System.Windows.Forms.Padding(2);
            this.tb_workfolder.Name = "tb_workfolder";
            this.tb_workfolder.Size = new System.Drawing.Size(160, 20);
            this.tb_workfolder.TabIndex = 22;
            this.tb_workfolder.Text = "c:\\temp\\debug\\";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Module";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Contains";
            // 
            // tb_module
            // 
            this.tb_module.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_module.Location = new System.Drawing.Point(78, 19);
            this.tb_module.Margin = new System.Windows.Forms.Padding(2);
            this.tb_module.Name = "tb_module";
            this.tb_module.Size = new System.Drawing.Size(160, 20);
            this.tb_module.TabIndex = 20;
            this.tb_module.Text = "DDressing.sys";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Load if: Varname";
            // 
            // tb_verfileValue
            // 
            this.tb_verfileValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_verfileValue.Location = new System.Drawing.Point(343, 56);
            this.tb_verfileValue.Margin = new System.Windows.Forms.Padding(2);
            this.tb_verfileValue.Name = "tb_verfileValue";
            this.tb_verfileValue.Size = new System.Drawing.Size(177, 20);
            this.tb_verfileValue.TabIndex = 17;
            this.tb_verfileValue.Text = "ABB 6V05 - 2016-12-13";
            // 
            // tbVerVarName
            // 
            this.tbVerVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbVerVarName.Location = new System.Drawing.Point(108, 56);
            this.tbVerVarName.Margin = new System.Windows.Forms.Padding(2);
            this.tbVerVarName.Name = "tbVerVarName";
            this.tbVerVarName.Size = new System.Drawing.Size(177, 20);
            this.tbVerVarName.TabIndex = 16;
            this.tbVerVarName.Text = "Version_DDressing";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1344, 511);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 42);
            this.button1.TabIndex = 18;
            this.button1.Text = "dosometing";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 581);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnDoWork);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "DANGER!!!!! Tool connects to robots and can DO A LOT OF NASTY STUF!!!!";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_addCntrl;
        private System.Windows.Forms.TextBox tbox_ip;
        private System.Windows.Forms.Button btn_scanNetwork;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_expose;
        private System.Windows.Forms.Button btnDoWork;
        private System.Windows.Forms.TextBox tbGridWhereClause;
        private System.Windows.Forms.Button btn_loadGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_verfileValue;
        private System.Windows.Forms.TextBox tbVerVarName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_module;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_workfolder;
        private System.Windows.Forms.Button button1;
    }
}


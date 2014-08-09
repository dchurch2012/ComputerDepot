namespace computerdepot
{
    partial class ReportingForm
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
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.rbSales = new System.Windows.Forms.RadioButton();
            this.rbEmployeeSales = new System.Windows.Forms.RadioButton();
            this.rbTopSellers = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReports
            // 
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvReports.Location = new System.Drawing.Point(0, 98);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.Size = new System.Drawing.Size(577, 220);
            this.dgvReports.TabIndex = 0;
            // 
            // rbSales
            // 
            this.rbSales.AutoSize = true;
            this.rbSales.Location = new System.Drawing.Point(12, 12);
            this.rbSales.Name = "rbSales";
            this.rbSales.Size = new System.Drawing.Size(86, 17);
            this.rbSales.TabIndex = 1;
            this.rbSales.TabStop = true;
            this.rbSales.Text = "Sales Report";
            this.rbSales.UseVisualStyleBackColor = true;
            this.rbSales.CheckedChanged += new System.EventHandler(this.rbSales_CheckedChanged);
            // 
            // rbEmployeeSales
            // 
            this.rbEmployeeSales.AutoSize = true;
            this.rbEmployeeSales.Location = new System.Drawing.Point(12, 35);
            this.rbEmployeeSales.Name = "rbEmployeeSales";
            this.rbEmployeeSales.Size = new System.Drawing.Size(100, 17);
            this.rbEmployeeSales.TabIndex = 2;
            this.rbEmployeeSales.TabStop = true;
            this.rbEmployeeSales.Text = "Employee Sales";
            this.rbEmployeeSales.UseVisualStyleBackColor = true;
            this.rbEmployeeSales.CheckedChanged += new System.EventHandler(this.rbEmployeeSales_CheckedChanged);
            // 
            // rbTopSellers
            // 
            this.rbTopSellers.AutoSize = true;
            this.rbTopSellers.Location = new System.Drawing.Point(12, 58);
            this.rbTopSellers.Name = "rbTopSellers";
            this.rbTopSellers.Size = new System.Drawing.Size(188, 17);
            this.rbTopSellers.TabIndex = 3;
            this.rbTopSellers.TabStop = true;
            this.rbTopSellers.Text = "Top Selling Products and Services";
            this.rbTopSellers.UseVisualStyleBackColor = true;
            this.rbTopSellers.CheckedChanged += new System.EventHandler(this.rbTopSellers_CheckedChanged);
            // 
            // ReportingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 318);
            this.Controls.Add(this.rbTopSellers);
            this.Controls.Add(this.rbEmployeeSales);
            this.Controls.Add(this.rbSales);
            this.Controls.Add(this.dgvReports);
            this.Name = "ReportingForm";
            this.Text = "ReportingForm";
            this.Load += new System.EventHandler(this.ReportingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.RadioButton rbSales;
        private System.Windows.Forms.RadioButton rbEmployeeSales;
        private System.Windows.Forms.RadioButton rbTopSellers;
    }
}
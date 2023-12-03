namespace PostgreDB
{
    partial class frmMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dtGrid = new System.Windows.Forms.DataGridView();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnInterruptQuery = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.Label();
            this.btnOpenForm2 = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dtGrid
            // 
            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGrid.Location = new System.Drawing.Point(12, 255);
            this.dtGrid.Name = "dtGrid";
            this.dtGrid.RowTemplate.Height = 24;
            this.dtGrid.Size = new System.Drawing.Size(860, 270);
            this.dtGrid.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(12, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查詢";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(12, 51);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(860, 192);
            this.txtQuery.TabIndex = 2;
            this.txtQuery.Text = "select seqno, si_ver, gross, os_rate, t_time, r_date from sinfo2 where seqno=4527" +
    "\r\n--select r_id as \"廠內批號\", r_time, r_date, aps_delivery as \"AAA\" from pdt where " +
    "r_id=\'A7040005\' or r_id=\'A7050005\'";
            // 
            // btnInterruptQuery
            // 
            this.btnInterruptQuery.Location = new System.Drawing.Point(112, 12);
            this.btnInterruptQuery.Name = "btnInterruptQuery";
            this.btnInterruptQuery.Size = new System.Drawing.Size(75, 28);
            this.btnInterruptQuery.TabIndex = 3;
            this.btnInterruptQuery.Text = "中斷查詢";
            this.btnInterruptQuery.UseVisualStyleBackColor = true;
            this.btnInterruptQuery.Click += new System.EventHandler(this.btnInterruptQuery_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.AutoSize = true;
            this.txtStatus.Location = new System.Drawing.Point(13, 539);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(33, 12);
            this.txtStatus.TabIndex = 4;
            this.txtStatus.Text = "label1";
            // 
            // btnOpenForm2
            // 
            this.btnOpenForm2.Location = new System.Drawing.Point(647, 17);
            this.btnOpenForm2.Name = "btnOpenForm2";
            this.btnOpenForm2.Size = new System.Drawing.Size(130, 28);
            this.btnOpenForm2.TabIndex = 5;
            this.btnOpenForm2.Text = "開啟 Oracle 表單";
            this.btnOpenForm2.UseVisualStyleBackColor = true;
            this.btnOpenForm2.Click += new System.EventHandler(this.btnOpenForm2_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(321, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 28);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "建立連線";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnOpenForm2);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnInterruptQuery);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dtGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PostgreDB 基本操作範例";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dtGrid;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnInterruptQuery;
        private System.Windows.Forms.Label txtStatus;
        private System.Windows.Forms.Button btnOpenForm2;
        private System.Windows.Forms.Button btnConnect;
    }
}


namespace AutoSign
{
    partial class AutoSign
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoSign));
            this.start = new System.Windows.Forms.Button();
            this.tips = new System.Windows.Forms.Label();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.start.Location = new System.Drawing.Point(42, 69);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 25);
            this.start.TabIndex = 1;
            this.start.Text = "开始签到";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // tips
            // 
            this.tips.AutoSize = true;
            this.tips.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tips.ForeColor = System.Drawing.Color.Black;
            this.tips.Location = new System.Drawing.Point(35, 24);
            this.tips.Name = "tips";
            this.tips.Size = new System.Drawing.Size(92, 17);
            this.tips.TabIndex = 102;
            this.tips.Text = "请点击开始签到";
            // 
            // wb
            // 
            this.wb.Location = new System.Drawing.Point(1, 1);
            this.wb.MinimumSize = new System.Drawing.Size(1, 1);
            this.wb.Name = "wb";
            this.wb.ScrollBarsEnabled = false;
            this.wb.Size = new System.Drawing.Size(1, 1);
            this.wb.TabIndex = 100;
            // 
            // AutoSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(164, 111);
            this.Controls.Add(this.tips);
            this.Controls.Add(this.start);
            this.Controls.Add(this.wb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoSign";
            this.Text = "贴吧自动签到";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label tips;
        private System.Windows.Forms.WebBrowser wb;
    }
}


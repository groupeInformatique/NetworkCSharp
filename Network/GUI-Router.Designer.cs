namespace Network
{
    partial class formRouteur
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
            this.formRouter = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // formRouter
            // 
            this.formRouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formRouter.Location = new System.Drawing.Point(0, 0);
            this.formRouter.MinimumSize = new System.Drawing.Size(20, 20);
            this.formRouter.Name = "formRouter";
            this.formRouter.Size = new System.Drawing.Size(671, 450);
            this.formRouter.TabIndex = 0;
            this.formRouter.Url = new System.Uri("http://192.168.0.1", System.UriKind.Absolute);
            // 
            // formRouteur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 450);
            this.Controls.Add(this.formRouter);
            this.Name = "formRouteur";
            this.Text = "Router";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser formRouter;
    }
}
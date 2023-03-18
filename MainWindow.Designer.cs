namespace Stt_to_tts
{
    partial class MainWindow
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
            this.cbOutputList = new System.Windows.Forms.ComboBox();
            this.cbInputList = new System.Windows.Forms.ComboBox();
            this.tbTtsEntry = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btSttStop = new System.Windows.Forms.Button();
            this.btSttStart = new System.Windows.Forms.Button();
            this.btTtsPlay = new System.Windows.Forms.Button();
            this.btTtsStop = new System.Windows.Forms.Button();
            this.tbSttResults = new System.Windows.Forms.TextBox();
            this.plSttThreadIndicator = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cbOutputList
            // 
            this.cbOutputList.FormattingEnabled = true;
            this.cbOutputList.Location = new System.Drawing.Point(13, 38);
            this.cbOutputList.Name = "cbOutputList";
            this.cbOutputList.Size = new System.Drawing.Size(192, 21);
            this.cbOutputList.TabIndex = 0;
            this.cbOutputList.SelectedIndexChanged += new System.EventHandler(this.cbOutputList_SelectedIndexChanged);
            // 
            // cbInputList
            // 
            this.cbInputList.FormattingEnabled = true;
            this.cbInputList.Location = new System.Drawing.Point(250, 37);
            this.cbInputList.Name = "cbInputList";
            this.cbInputList.Size = new System.Drawing.Size(205, 21);
            this.cbInputList.TabIndex = 1;
            this.cbInputList.SelectedIndexChanged += new System.EventHandler(this.cbInputList_SelectedIndexChanged);
            // 
            // tbTtsEntry
            // 
            this.tbTtsEntry.AcceptsReturn = true;
            this.tbTtsEntry.AcceptsTab = true;
            this.tbTtsEntry.Location = new System.Drawing.Point(13, 66);
            this.tbTtsEntry.Multiline = true;
            this.tbTtsEntry.Name = "tbTtsEntry";
            this.tbTtsEntry.Size = new System.Drawing.Size(442, 190);
            this.tbTtsEntry.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Audio Mirror Device";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Audio Input Device";
            // 
            // btSttStop
            // 
            this.btSttStop.Location = new System.Drawing.Point(379, 289);
            this.btSttStop.Name = "btSttStop";
            this.btSttStop.Size = new System.Drawing.Size(75, 23);
            this.btSttStop.TabIndex = 5;
            this.btSttStop.Text = "STT Stop";
            this.btSttStop.UseVisualStyleBackColor = true;
            this.btSttStop.Click += new System.EventHandler(this.btSttStop_Click);
            // 
            // btSttStart
            // 
            this.btSttStart.Location = new System.Drawing.Point(380, 260);
            this.btSttStart.Name = "btSttStart";
            this.btSttStart.Size = new System.Drawing.Size(75, 23);
            this.btSttStart.TabIndex = 6;
            this.btSttStart.Text = "STT Start";
            this.btSttStart.UseVisualStyleBackColor = true;
            this.btSttStart.Click += new System.EventHandler(this.btSttStart_Click);
            // 
            // btTtsPlay
            // 
            this.btTtsPlay.Location = new System.Drawing.Point(299, 260);
            this.btTtsPlay.Name = "btTtsPlay";
            this.btTtsPlay.Size = new System.Drawing.Size(75, 23);
            this.btTtsPlay.TabIndex = 7;
            this.btTtsPlay.Text = "TTS Play";
            this.btTtsPlay.UseVisualStyleBackColor = true;
            this.btTtsPlay.Click += new System.EventHandler(this.btTtsPlay_Click);
            // 
            // btTtsStop
            // 
            this.btTtsStop.Location = new System.Drawing.Point(298, 289);
            this.btTtsStop.Name = "btTtsStop";
            this.btTtsStop.Size = new System.Drawing.Size(75, 23);
            this.btTtsStop.TabIndex = 8;
            this.btTtsStop.Text = "TTS Stop";
            this.btTtsStop.UseVisualStyleBackColor = true;
            this.btTtsStop.Click += new System.EventHandler(this.btTtsStop_Click);
            // 
            // tbSttResults
            // 
            this.tbSttResults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tbSttResults.Location = new System.Drawing.Point(13, 263);
            this.tbSttResults.Multiline = true;
            this.tbSttResults.Name = "tbSttResults";
            this.tbSttResults.ReadOnly = true;
            this.tbSttResults.Size = new System.Drawing.Size(233, 49);
            this.tbSttResults.TabIndex = 9;
            // 
            // plSttThreadIndicator
            // 
            this.plSttThreadIndicator.BackColor = System.Drawing.Color.Maroon;
            this.plSttThreadIndicator.Location = new System.Drawing.Point(252, 263);
            this.plSttThreadIndicator.Name = "plSttThreadIndicator";
            this.plSttThreadIndicator.Size = new System.Drawing.Size(41, 49);
            this.plSttThreadIndicator.TabIndex = 10;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 317);
            this.Controls.Add(this.plSttThreadIndicator);
            this.Controls.Add(this.tbSttResults);
            this.Controls.Add(this.btTtsStop);
            this.Controls.Add(this.btTtsPlay);
            this.Controls.Add(this.btSttStart);
            this.Controls.Add(this.btSttStop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTtsEntry);
            this.Controls.Add(this.cbInputList);
            this.Controls.Add(this.cbOutputList);
            this.Name = "MainWindow";
            this.Text = "Speech To Text To Speech";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbOutputList;
        private System.Windows.Forms.ComboBox cbInputList;
        private System.Windows.Forms.TextBox tbTtsEntry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSttStop;
        private System.Windows.Forms.Button btSttStart;
        private System.Windows.Forms.Button btTtsPlay;
        private System.Windows.Forms.Button btTtsStop;
        private System.Windows.Forms.TextBox tbSttResults;
        private System.Windows.Forms.Panel plSttThreadIndicator;
    }
}


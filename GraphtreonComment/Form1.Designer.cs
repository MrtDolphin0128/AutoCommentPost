namespace GraphtreonComment
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
            this.creatorList = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CreatorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Patrons = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Earnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AvgSupport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnShowAll = new System.Windows.Forms.RadioButton();
            this.BtnShowNSFW = new System.Windows.Forms.RadioButton();
            this.BtnShowSFW = new System.Windows.Forms.RadioButton();
            this.txtPatronNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaxComments = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCommentDelay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCreatorDelay = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.BtnRemoveBlack = new System.Windows.Forms.Button();
            this.BtnAddBlack = new System.Windows.Forms.Button();
            this.blackList = new System.Windows.Forms.ListBox();
            this.genericList = new System.Windows.Forms.ListBox();
            this.commentList = new System.Windows.Forms.ListBox();
            this.keywordList = new System.Windows.Forms.ListBox();
            this.BtnRemoveGeneric = new System.Windows.Forms.Button();
            this.BtnRemoveComment = new System.Windows.Forms.Button();
            this.BtnRemoveKeyword = new System.Windows.Forms.Button();
            this.BtnAddGeneric = new System.Windows.Forms.Button();
            this.BtnAddComment = new System.Windows.Forms.Button();
            this.BtnAddKeyword = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BtnStartStop = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            this.BtnLogInOut = new System.Windows.Forms.Button();
            this.BtnReloadList = new System.Windows.Forms.Button();
            this.commentHistoryList = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnClearHistory = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnPrevPage = new System.Windows.Forms.Button();
            this.txtCurPage = new System.Windows.Forms.TextBox();
            this.BtnNextPage = new System.Windows.Forms.Button();
            this.labelMaxPage = new System.Windows.Forms.Label();
            this.chkPreviousCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // creatorList
            // 
            this.creatorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.CreatorName,
            this.Patrons,
            this.Earnings,
            this.AvgSupport,
            this.Status});
            this.creatorList.GridLines = true;
            this.creatorList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.creatorList.HideSelection = false;
            this.creatorList.Location = new System.Drawing.Point(14, 128);
            this.creatorList.Name = "creatorList";
            this.creatorList.Size = new System.Drawing.Size(759, 314);
            this.creatorList.TabIndex = 8;
            this.creatorList.UseCompatibleStateImageBehavior = false;
            this.creatorList.View = System.Windows.Forms.View.Details;
            // 
            // No
            // 
            this.No.Text = "#";
            this.No.Width = 50;
            // 
            // CreatorName
            // 
            this.CreatorName.Text = "Creator";
            this.CreatorName.Width = 200;
            // 
            // Patrons
            // 
            this.Patrons.Text = "Patrons";
            this.Patrons.Width = 100;
            // 
            // Earnings
            // 
            this.Earnings.Text = "Earnings";
            this.Earnings.Width = 150;
            // 
            // AvgSupport
            // 
            this.AvgSupport.Text = "Avg support per patron";
            this.AvgSupport.Width = 150;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 100;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(599, 13);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(174, 20);
            this.txtEmail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(551, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(599, 42);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(174, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(525, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password";
            // 
            // BtnShowAll
            // 
            this.BtnShowAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.BtnShowAll.Checked = true;
            this.BtnShowAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnShowAll.Location = new System.Drawing.Point(14, 102);
            this.BtnShowAll.Name = "BtnShowAll";
            this.BtnShowAll.Size = new System.Drawing.Size(47, 23);
            this.BtnShowAll.TabIndex = 3;
            this.BtnShowAll.TabStop = true;
            this.BtnShowAll.Text = "All";
            this.BtnShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnShowAll.UseVisualStyleBackColor = true;
            this.BtnShowAll.CheckedChanged += new System.EventHandler(this.BtnShowAll_CheckedChanged);
            // 
            // BtnShowNSFW
            // 
            this.BtnShowNSFW.Appearance = System.Windows.Forms.Appearance.Button;
            this.BtnShowNSFW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnShowNSFW.Location = new System.Drawing.Point(67, 102);
            this.BtnShowNSFW.Name = "BtnShowNSFW";
            this.BtnShowNSFW.Size = new System.Drawing.Size(97, 23);
            this.BtnShowNSFW.TabIndex = 4;
            this.BtnShowNSFW.Text = "NSFW Only";
            this.BtnShowNSFW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnShowNSFW.UseVisualStyleBackColor = true;
            this.BtnShowNSFW.CheckedChanged += new System.EventHandler(this.BtnShowNSFW_CheckedChanged);
            // 
            // BtnShowSFW
            // 
            this.BtnShowSFW.Appearance = System.Windows.Forms.Appearance.Button;
            this.BtnShowSFW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnShowSFW.Location = new System.Drawing.Point(170, 102);
            this.BtnShowSFW.Name = "BtnShowSFW";
            this.BtnShowSFW.Size = new System.Drawing.Size(97, 23);
            this.BtnShowSFW.TabIndex = 5;
            this.BtnShowSFW.Text = "SFW Only";
            this.BtnShowSFW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnShowSFW.UseVisualStyleBackColor = true;
            this.BtnShowSFW.CheckedChanged += new System.EventHandler(this.BtnShowSFW_CheckedChanged);
            // 
            // txtPatronNum
            // 
            this.txtPatronNum.Location = new System.Drawing.Point(435, 102);
            this.txtPatronNum.Name = "txtPatronNum";
            this.txtPatronNum.Size = new System.Drawing.Size(68, 20);
            this.txtPatronNum.TabIndex = 6;
            this.txtPatronNum.TextChanged += new System.EventHandler(this.txtPatronNum_TextChanged);
            this.txtPatronNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronNum_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(298, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Number of Patrons:";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(569, 101);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(205, 20);
            this.txtFilter.TabIndex = 7;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(520, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Filter:";
            // 
            // txtMaxComments
            // 
            this.txtMaxComments.Location = new System.Drawing.Point(155, 23);
            this.txtMaxComments.Name = "txtMaxComments";
            this.txtMaxComments.Size = new System.Drawing.Size(68, 23);
            this.txtMaxComments.TabIndex = 10;
            this.txtMaxComments.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronNum_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(11, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Max Comments Per Day:";
            // 
            // txtCommentDelay
            // 
            this.txtCommentDelay.Location = new System.Drawing.Point(422, 23);
            this.txtCommentDelay.Name = "txtCommentDelay";
            this.txtCommentDelay.Size = new System.Drawing.Size(68, 23);
            this.txtCommentDelay.TabIndex = 11;
            this.txtCommentDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronNum_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(264, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Delay Between Comments:";
            // 
            // txtCreatorDelay
            // 
            this.txtCreatorDelay.Location = new System.Drawing.Point(676, 23);
            this.txtCreatorDelay.Name = "txtCreatorDelay";
            this.txtCreatorDelay.Size = new System.Drawing.Size(68, 23);
            this.txtCreatorDelay.TabIndex = 12;
            this.txtCreatorDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronNum_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(531, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Delay Between Creators:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.BtnRemoveBlack);
            this.groupBox1.Controls.Add(this.BtnAddBlack);
            this.groupBox1.Controls.Add(this.blackList);
            this.groupBox1.Controls.Add(this.genericList);
            this.groupBox1.Controls.Add(this.commentList);
            this.groupBox1.Controls.Add(this.keywordList);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCommentDelay);
            this.groupBox1.Controls.Add(this.BtnRemoveGeneric);
            this.groupBox1.Controls.Add(this.BtnRemoveComment);
            this.groupBox1.Controls.Add(this.BtnRemoveKeyword);
            this.groupBox1.Controls.Add(this.BtnAddGeneric);
            this.groupBox1.Controls.Add(this.BtnAddComment);
            this.groupBox1.Controls.Add(this.BtnAddKeyword);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCreatorDelay);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtMaxComments);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(14, 482);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(762, 336);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comment Settings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(590, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 15);
            this.label11.TabIndex = 16;
            this.label11.Text = "Blacklist:";
            // 
            // BtnRemoveBlack
            // 
            this.BtnRemoveBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRemoveBlack.Location = new System.Drawing.Point(681, 303);
            this.BtnRemoveBlack.Name = "BtnRemoveBlack";
            this.BtnRemoveBlack.Size = new System.Drawing.Size(75, 26);
            this.BtnRemoveBlack.TabIndex = 14;
            this.BtnRemoveBlack.Text = "Remove";
            this.BtnRemoveBlack.UseVisualStyleBackColor = true;
            this.BtnRemoveBlack.Click += new System.EventHandler(this.BtnRemoveBlack_Click);
            // 
            // BtnAddBlack
            // 
            this.BtnAddBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAddBlack.Location = new System.Drawing.Point(588, 303);
            this.BtnAddBlack.Name = "BtnAddBlack";
            this.BtnAddBlack.Size = new System.Drawing.Size(75, 26);
            this.BtnAddBlack.TabIndex = 15;
            this.BtnAddBlack.Text = "Add";
            this.BtnAddBlack.UseVisualStyleBackColor = true;
            this.BtnAddBlack.Click += new System.EventHandler(this.BtnAddBlack_Click);
            // 
            // blackList
            // 
            this.blackList.FormattingEnabled = true;
            this.blackList.ItemHeight = 16;
            this.blackList.Location = new System.Drawing.Point(588, 79);
            this.blackList.Name = "blackList";
            this.blackList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.blackList.Size = new System.Drawing.Size(168, 212);
            this.blackList.TabIndex = 13;
            // 
            // genericList
            // 
            this.genericList.FormattingEnabled = true;
            this.genericList.ItemHeight = 16;
            this.genericList.Location = new System.Drawing.Point(394, 79);
            this.genericList.Name = "genericList";
            this.genericList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.genericList.Size = new System.Drawing.Size(168, 212);
            this.genericList.TabIndex = 12;
            // 
            // commentList
            // 
            this.commentList.FormattingEnabled = true;
            this.commentList.ItemHeight = 16;
            this.commentList.Location = new System.Drawing.Point(200, 79);
            this.commentList.Name = "commentList";
            this.commentList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.commentList.Size = new System.Drawing.Size(168, 212);
            this.commentList.TabIndex = 12;
            // 
            // keywordList
            // 
            this.keywordList.FormattingEnabled = true;
            this.keywordList.ItemHeight = 16;
            this.keywordList.Location = new System.Drawing.Point(6, 79);
            this.keywordList.Name = "keywordList";
            this.keywordList.Size = new System.Drawing.Size(168, 212);
            this.keywordList.TabIndex = 12;
            this.keywordList.SelectedIndexChanged += new System.EventHandler(this.keywordList_SelectedIndexChanged);
            // 
            // BtnRemoveGeneric
            // 
            this.BtnRemoveGeneric.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRemoveGeneric.Location = new System.Drawing.Point(487, 303);
            this.BtnRemoveGeneric.Name = "BtnRemoveGeneric";
            this.BtnRemoveGeneric.Size = new System.Drawing.Size(75, 26);
            this.BtnRemoveGeneric.TabIndex = 9;
            this.BtnRemoveGeneric.Text = "Remove";
            this.BtnRemoveGeneric.UseVisualStyleBackColor = true;
            this.BtnRemoveGeneric.Click += new System.EventHandler(this.BtnRemoveGeneric_Click);
            // 
            // BtnRemoveComment
            // 
            this.BtnRemoveComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRemoveComment.Location = new System.Drawing.Point(293, 303);
            this.BtnRemoveComment.Name = "BtnRemoveComment";
            this.BtnRemoveComment.Size = new System.Drawing.Size(75, 26);
            this.BtnRemoveComment.TabIndex = 9;
            this.BtnRemoveComment.Text = "Remove";
            this.BtnRemoveComment.UseVisualStyleBackColor = true;
            this.BtnRemoveComment.Click += new System.EventHandler(this.BtnRemoveComment_Click);
            // 
            // BtnRemoveKeyword
            // 
            this.BtnRemoveKeyword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRemoveKeyword.Location = new System.Drawing.Point(99, 303);
            this.BtnRemoveKeyword.Name = "BtnRemoveKeyword";
            this.BtnRemoveKeyword.Size = new System.Drawing.Size(75, 26);
            this.BtnRemoveKeyword.TabIndex = 9;
            this.BtnRemoveKeyword.Text = "Remove";
            this.BtnRemoveKeyword.UseVisualStyleBackColor = true;
            this.BtnRemoveKeyword.Click += new System.EventHandler(this.BtnRemoveKeyword_Click);
            // 
            // BtnAddGeneric
            // 
            this.BtnAddGeneric.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAddGeneric.Location = new System.Drawing.Point(394, 303);
            this.BtnAddGeneric.Name = "BtnAddGeneric";
            this.BtnAddGeneric.Size = new System.Drawing.Size(75, 26);
            this.BtnAddGeneric.TabIndex = 9;
            this.BtnAddGeneric.Text = "Add";
            this.BtnAddGeneric.UseVisualStyleBackColor = true;
            this.BtnAddGeneric.Click += new System.EventHandler(this.BtnAddGeneric_Click);
            // 
            // BtnAddComment
            // 
            this.BtnAddComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAddComment.Location = new System.Drawing.Point(200, 303);
            this.BtnAddComment.Name = "BtnAddComment";
            this.BtnAddComment.Size = new System.Drawing.Size(75, 26);
            this.BtnAddComment.TabIndex = 9;
            this.BtnAddComment.Text = "Add";
            this.BtnAddComment.UseVisualStyleBackColor = true;
            this.BtnAddComment.Click += new System.EventHandler(this.BtnAddComment_Click);
            // 
            // BtnAddKeyword
            // 
            this.BtnAddKeyword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAddKeyword.Location = new System.Drawing.Point(6, 303);
            this.BtnAddKeyword.Name = "BtnAddKeyword";
            this.BtnAddKeyword.Size = new System.Drawing.Size(75, 26);
            this.BtnAddKeyword.TabIndex = 9;
            this.BtnAddKeyword.Text = "Add";
            this.BtnAddKeyword.UseVisualStyleBackColor = true;
            this.BtnAddKeyword.Click += new System.EventHandler(this.BtnAddKeyword_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(397, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "Generic Comments:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(207, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Comments To Post:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(11, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Search Keywords:";
            // 
            // BtnStartStop
            // 
            this.BtnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnStartStop.Location = new System.Drawing.Point(14, 452);
            this.BtnStartStop.Name = "BtnStartStop";
            this.BtnStartStop.Size = new System.Drawing.Size(132, 26);
            this.BtnStartStop.TabIndex = 9;
            this.BtnStartStop.Text = "Start";
            this.BtnStartStop.UseVisualStyleBackColor = true;
            this.BtnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // tabMain
            // 
            this.tabMain.ItemSize = new System.Drawing.Size(0, 18);
            this.tabMain.Location = new System.Drawing.Point(794, 13);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(541, 461);
            this.tabMain.TabIndex = 10;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // loadingBar
            // 
            this.loadingBar.BackColor = System.Drawing.SystemColors.Control;
            this.loadingBar.Location = new System.Drawing.Point(105, 259);
            this.loadingBar.MarqueeAnimationSpeed = 25;
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(561, 23);
            this.loadingBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.loadingBar.TabIndex = 11;
            this.loadingBar.UseWaitCursor = true;
            this.loadingBar.Value = 10;
            this.loadingBar.Visible = false;
            // 
            // BtnLogInOut
            // 
            this.BtnLogInOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLogInOut.Location = new System.Drawing.Point(678, 68);
            this.BtnLogInOut.Name = "BtnLogInOut";
            this.BtnLogInOut.Size = new System.Drawing.Size(95, 26);
            this.BtnLogInOut.TabIndex = 9;
            this.BtnLogInOut.Text = "Log In";
            this.BtnLogInOut.UseVisualStyleBackColor = true;
            this.BtnLogInOut.Click += new System.EventHandler(this.BtnLogInOut_Click);
            // 
            // BtnReloadList
            // 
            this.BtnReloadList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnReloadList.Location = new System.Drawing.Point(152, 452);
            this.BtnReloadList.Name = "BtnReloadList";
            this.BtnReloadList.Size = new System.Drawing.Size(132, 26);
            this.BtnReloadList.TabIndex = 9;
            this.BtnReloadList.Text = "Reload List";
            this.BtnReloadList.UseVisualStyleBackColor = true;
            this.BtnReloadList.Click += new System.EventHandler(this.BtnReloadList_Click);
            // 
            // commentHistoryList
            // 
            this.commentHistoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.commentHistoryList.GridLines = true;
            this.commentHistoryList.HideSelection = false;
            this.commentHistoryList.Location = new System.Drawing.Point(800, 512);
            this.commentHistoryList.Name = "commentHistoryList";
            this.commentHistoryList.Size = new System.Drawing.Size(529, 261);
            this.commentHistoryList.TabIndex = 8;
            this.commentHistoryList.UseCompatibleStateImageBehavior = false;
            this.commentHistoryList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Creator";
            this.columnHeader8.Width = 185;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Post Title";
            this.columnHeader9.Width = 190;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Posted Comment";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Creator";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Patrons";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Earnings";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Avg support per patron";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            // 
            // BtnClearHistory
            // 
            this.BtnClearHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClearHistory.Location = new System.Drawing.Point(1234, 785);
            this.BtnClearHistory.Name = "BtnClearHistory";
            this.BtnClearHistory.Size = new System.Drawing.Size(92, 26);
            this.BtnClearHistory.TabIndex = 9;
            this.BtnClearHistory.Text = "Clear";
            this.BtnClearHistory.UseVisualStyleBackColor = true;
            this.BtnClearHistory.Click += new System.EventHandler(this.BtnClearHistory_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(794, 482);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(541, 333);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Comment History";
            // 
            // BtnPrevPage
            // 
            this.BtnPrevPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPrevPage.Location = new System.Drawing.Point(605, 451);
            this.BtnPrevPage.Name = "BtnPrevPage";
            this.BtnPrevPage.Size = new System.Drawing.Size(37, 22);
            this.BtnPrevPage.TabIndex = 13;
            this.BtnPrevPage.Text = "<<";
            this.BtnPrevPage.UseVisualStyleBackColor = true;
            this.BtnPrevPage.Click += new System.EventHandler(this.BtnPrevPage_Click);
            // 
            // txtCurPage
            // 
            this.txtCurPage.Location = new System.Drawing.Point(648, 452);
            this.txtCurPage.Name = "txtCurPage";
            this.txtCurPage.Size = new System.Drawing.Size(40, 20);
            this.txtCurPage.TabIndex = 6;
            this.txtCurPage.TextChanged += new System.EventHandler(this.txtCurPage_TextChanged);
            this.txtCurPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronNum_KeyPress);
            // 
            // BtnNextPage
            // 
            this.BtnNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnNextPage.Location = new System.Drawing.Point(741, 451);
            this.BtnNextPage.Name = "BtnNextPage";
            this.BtnNextPage.Size = new System.Drawing.Size(37, 22);
            this.BtnNextPage.TabIndex = 13;
            this.BtnNextPage.Text = ">>";
            this.BtnNextPage.UseVisualStyleBackColor = true;
            this.BtnNextPage.Click += new System.EventHandler(this.BtnNextPage_Click);
            // 
            // labelMaxPage
            // 
            this.labelMaxPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMaxPage.Location = new System.Drawing.Point(694, 454);
            this.labelMaxPage.Name = "labelMaxPage";
            this.labelMaxPage.Size = new System.Drawing.Size(41, 15);
            this.labelMaxPage.TabIndex = 0;
            this.labelMaxPage.Text = "of 747";
            // 
            // chkPreviousCheck
            // 
            this.chkPreviousCheck.AutoSize = true;
            this.chkPreviousCheck.Checked = true;
            this.chkPreviousCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreviousCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkPreviousCheck.Location = new System.Drawing.Point(301, 457);
            this.chkPreviousCheck.Name = "chkPreviousCheck";
            this.chkPreviousCheck.Size = new System.Drawing.Size(189, 19);
            this.chkPreviousCheck.TabIndex = 13;
            this.chkPreviousCheck.Text = "Check all previous comments.";
            this.chkPreviousCheck.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1338, 827);
            this.Controls.Add(this.chkPreviousCheck);
            this.Controls.Add(this.BtnNextPage);
            this.Controls.Add(this.BtnPrevPage);
            this.Controls.Add(this.loadingBar);
            this.Controls.Add(this.BtnLogInOut);
            this.Controls.Add(this.BtnReloadList);
            this.Controls.Add(this.BtnStartStop);
            this.Controls.Add(this.BtnShowSFW);
            this.Controls.Add(this.BtnShowNSFW);
            this.Controls.Add(this.BtnShowAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnClearHistory);
            this.Controls.Add(this.labelMaxPage);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCurPage);
            this.Controls.Add(this.txtPatronNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.commentHistoryList);
            this.Controls.Add(this.creatorList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Patreon Auto Comment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView creatorList;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton BtnShowAll;
        private System.Windows.Forms.RadioButton BtnShowNSFW;
        private System.Windows.Forms.RadioButton BtnShowSFW;
        private System.Windows.Forms.TextBox txtPatronNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxComments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCommentDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCreatorDelay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnStartStop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ColumnHeader No;
        private System.Windows.Forms.ColumnHeader CreatorName;
        private System.Windows.Forms.ColumnHeader Patrons;
        private System.Windows.Forms.ColumnHeader Earnings;
        private System.Windows.Forms.ColumnHeader AvgSupport;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.ProgressBar loadingBar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnLogInOut;
        private System.Windows.Forms.Button BtnAddKeyword;
        private System.Windows.Forms.Button BtnRemoveKeyword;
        private System.Windows.Forms.Button BtnRemoveGeneric;
        private System.Windows.Forms.Button BtnRemoveComment;
        private System.Windows.Forms.Button BtnAddGeneric;
        private System.Windows.Forms.Button BtnAddComment;
        private System.Windows.Forms.ListBox keywordList;
        private System.Windows.Forms.ListBox genericList;
        private System.Windows.Forms.ListBox commentList;
        private System.Windows.Forms.Button BtnReloadList;
        private System.Windows.Forms.ListView commentHistoryList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button BtnClearHistory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.Button BtnPrevPage;
        private System.Windows.Forms.TextBox txtCurPage;
        private System.Windows.Forms.Button BtnNextPage;
        private System.Windows.Forms.Label labelMaxPage;
        private System.Windows.Forms.CheckBox chkPreviousCheck;
        private System.Windows.Forms.Button BtnRemoveBlack;
        private System.Windows.Forms.Button BtnAddBlack;
        private System.Windows.Forms.ListBox blackList;
        private System.Windows.Forms.Label label11;
    }
}


namespace DotGame;

partial class GameWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ButtonLogin = new Button();
        NameInput = new TextBox();
        UsersLabel = new Label();
        Users = new ListBox();
        SuspendLayout();
        // 
        // ButtonLogin
        // 
        ButtonLogin.Location = new Point(350, 200);
        ButtonLogin.Name = "ButtonLogin";
        ButtonLogin.Size = new Size(100, 30); // задаем размер
        ButtonLogin.TabIndex = 0;
        ButtonLogin.Text = "Войти";
        ButtonLogin.UseVisualStyleBackColor = true;
        ButtonLogin.Click += Login;
        ButtonLogin.FlatStyle = FlatStyle.System; // устанавливаем стиль рамки кнопки
        ButtonLogin.FlatAppearance.BorderSize = 0;
        // 
        // NameInput
        // 
        NameInput.Location = new Point(300, 150);
        NameInput.Name = "NameInput";
        NameInput.Size = new Size(200, 30); // задаем размер
        NameInput.PlaceholderText = "Введите ваше имя";
        NameInput.TabIndex = 5;
        NameInput.BorderStyle = BorderStyle.FixedSingle;
        // 
        // UsersLabel
        // 
        UsersLabel.AutoSize = true;
        UsersLabel.Location = new Point(20, 0);
        UsersLabel.Name = "UsersLabel";
        UsersLabel.Size = new Size(53, 15);
        UsersLabel.TabIndex = 4;
        UsersLabel.Text = "Игроки";
        // Users
        // 
        Users.FormattingEnabled = true;
        Users.ItemHeight = 15;
        Users.Location = new Point(20, 18);
        Users.Name = "Users";
        Users.BorderStyle = BorderStyle.Fixed3D;
        Users.Size = new Size(120, 289);
        Users.TabIndex = 3;
        // 
        // Lab
        // 
        // 
        // GameWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(818, 335);

        Controls.Add(Users);
        Controls.Add(UsersLabel);
        Controls.Add(NameInput);
        Controls.Add(ButtonLogin);
        Margin = new Padding(2, 2, 2, 2);
        Name = "Main";
        Text = "Point Game";
        MouseDown += GameWindow_MouseDown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button ButtonLogin;
    private TextBox NameInput;
    private ListBox Users;
    private Label UsersLabel;
}
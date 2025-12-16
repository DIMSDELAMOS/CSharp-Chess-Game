using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        Database Data;
        string impth = "";
        PictureBox pb1;
        public bool flagClick = false;
        public PictureBox picturebox2;
        public List<int> places = new List<int>();
        public List<string> names = new List<string>();
        public List<string> imagePaths1 = new List<string>();
        public List<string> imagePaths = new List<string>();
        public List<int> blocks = new List<int>();
        int OldValue;
        public Form1()
        {
            InitializeComponent();
            AttachClickEventToPictureBoxes();
        }

        private void AttachClickEventToPictureBoxes()
        {

            for(int i = 1; i<65; i++ )
            {

                string pictureboxName = $"pictureBox{i}";
                Control confound = Controls.Find(pictureboxName, true).FirstOrDefault();
                PictureBox picturebox = (PictureBox)confound;
                picturebox.Click += PictureBox_Click;
                picturebox.Tag = i;
                
            }
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            flagClick = !flagClick;
           
            PictureBox pictureBox = (PictureBox)sender;
            int intValue = Convert.ToInt32(pictureBox.Tag);
            int rowIndex = intValue;
            if (flagClick)
            {
                OldValue = intValue;
                pb1 = pictureBox;
                impth = pictureBox.ImageLocation;
                move(rowIndex);

            }
            else
            {
                int NewValue = intValue;
                PlacePawn(pictureBox , intValue , OldValue, impth , pb1);
                if (names[intValue] == "kb")
                {
                    string Name = "p1";
                    Data.getconnection();
                    string cs = Data.ConnectionString;
                    using (SQLiteConnection con = new SQLiteConnection(cs))
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand();
                        string query = @"INSERT INTO CHESS (Name) 
                        VALUES (@Name)";
                        cmd.CommandText = query;
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@Name", Name));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else if (names[intValue] == "kw")
                {
                    string Name = "p2";
                    Data.getconnection();
                    string cs = Data.ConnectionString;
                    using (SQLiteConnection con = new SQLiteConnection(cs))
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand();
                        string query = @"INSERT INTO CHESS (Name) 
                        VALUES (@Name)";
                        cmd.CommandText = query;
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@Name", Name));
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }

                }
            }

           
        }
        private void PlacePawn(PictureBox sender , int place , int ov, string impth, PictureBox pb)
        {
            int pm;
            bool flag = false;
            foreach (int i in blocks)
            {
                if (i == place)
                {
                    pm = i;
                    flag = true;
                    break;
                }
            }
            if (flag)
            {

                sender.SizeMode = PictureBoxSizeMode.StretchImage;
                sender.ImageLocation = impth;
                pb1.ImageLocation = null;
                string olname = names[ov];
                names[ov] = "";
                names[place]= olname;
            }
        }
        private void move(int index)
        {           
            blocks.Clear();
            string Name = names[index];           
            if (Name == "pb")
            {
                int b1 = index + 8;
                if (names[b1] != "pb" || names[b1] != "tb" || names[b1] != "hb" || names[b1] != "bb" || names[b1] != "qb" || names[b1] != "kb")
                {
                    blocks.Add(b1); 
                
                }


                if (index >= 9 && index <= 16)
                {
                    if (names[b1] != "pb" || names[b1] != "tb" || names[b1] != "hb" || names[b1] != "bb" || names[b1] != "qb" || names[b1] != "kb")
                    {
                        blocks.Add(b1 + 8);

                    }
                    
                }
            }
            else if (Name == "pw")
            {
                int b2 = index - 8;
                if (names[b2] != "pw" && names[b2] != "tw" && names[b2] != "hw" && names[b2] != "bw" && names[b2] != "qw" && names[b2] != "kw")
                {
                    blocks.Add(b2);

                }


                if (index >= 49 && index <= 56)
                {
                    if (names[b2] != "pb" || names[b2] != "tb" || names[b2] != "hb" || names[b2] != "bb" || names[b2] != "qb" || names[b2] != "kb")
                    {
                        blocks.Add(b2 - 8);

                    }

                }
            }
            else if (Name == "tb" || Name == "tw")
            {
                string color;
                if(Name == "tb")
                {
                    color = "black";
                }
                else
                {
                    color = "white";
                }
                bool flagg = true;
                bool flagt1 = true;
                bool flagt2 = true;
                bool flagt3 = true; 
                bool flagt4 = true;
                int bf = index + 8;
                int bb = index - 8;
                int indr = index % 8;
                if(indr == 0)
                {
                    indr = 8;
                }
                int indl = index % 8;
                if(indl == 0)
                {
                    indl = 8;
                }
                bool flag = false;
                int br = index;
                int bl = index;
                
                while (flag == false)
                {
                    if (bb > 0)
                    {
                        if (flagt1 == true)
                        {
                            if (CheckIfMovable(color, bb) == true)
                            {
                                blocks.Add(bb);
                            }
                            else
                            {
                                flagt1 = false;
                            }
                        }
                        bb = bb - 8;
                    }
                    if (bf <= 64)
                    {
                        if (flagt2 == true)
                        {
                            if (CheckIfMovable(color, bf) == true)
                            {
                                blocks.Add(bf);
                            }
                            else
                            {
                                flagt2 = false;
                            }
                        }
                        bf = bf + 8;
                    }
                    if (indr < 8)
                    {

                        br += 1;
                        if (flagt3 == true)
                        {
                            if (CheckIfMovable(color, br) == true)
                            {
                                blocks.Add(br);
                            }
                            else
                            {
                                flagt3 = false;
                            }
                        }
                        indr += 1;
                    }
                    if (indl > 0)
                    {
                        bl -= 1;
                        if (flagt4 == true)
                        {
                            if (CheckIfMovable(color, bl) == true)
                            {
                                blocks.Add(bl);
                            }
                            else
                            {
                                flagt4 = false;
                            }
                        }
                        indl -= 1;
                    }
                    if (bb < 0 && bf > 64 && indr >= 8 && indl <= 0)
                    {
                        flag = true;
                    }
                }
                
            }
            else if (Name == "hb" || Name == "hw")
            {
                int bh1 = index + 17;
                int bh2 = index - 17;
                int bh3 = index - 15;
                int bh4 = index + 15;
                blocks.Add(bh1);
                blocks.Add(bh2);
                blocks.Add(bh3);
                blocks.Add(bh4);

            }
            else if (Name == "bb" || Name == "bw")
            {
                string color;
                if (Name == "bb")
                {
                    color = "black";
                }
                else
                {
                    color = "white";
                }
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;
                bool flagb = false;
                int bb1 = index;
                int bb2 = index;
                int bb3 = index;
                int bb4 = index;
                int mr = 0;
                int mr2 = 1;
                int mr3 = 0;
                int mr4 = 1;
                while (flagb == false)
                {
                    if (bb1 % 8 != 0 && bb1 <= 64 )
                    {

                        bb1 += 9;
                        if (bb1 <= 64 && flag1 == true)
                        {
                            if (CheckIfMovable(color, bb1) == true)
                            {
                                blocks.Add(bb1);
                            }
                            else
                            {
                                flag1 = false;
                            }

                        }
                        
                        
                        mr = bb1 % 8;

                    }
                    if (bb2 % 8 != 1 && bb2 >= 0 )
                    {
                        bb2 -= 9;
                        if (bb2 >= 0 && flag2 == true)
                        {
                            if (CheckIfMovable(color, bb2) == true)
                            {
                                blocks.Add(bb2);
                            }
                            else
                            {
                                flag2 = false;
                            }

                        }
                        
                       
                        mr2 = bb2 % 8;

                    }
                    if (bb3 % 8 != 0 && bb3 >= 0 )
                    {
                        bb3 -= 7;
                        if (bb3 >= 0 && flag3 == true)
                        {
                            if (CheckIfMovable(color, bb3) == true)
                            {
                                blocks.Add(bb3);
                            }
                            else
                            {
                                flag3 = false;
                            }
                        }
                       
                        
                        mr3 = bb3 % 8;

                    }
                    if (bb4 % 8 != 1 && bb4 <= 64)
                    {
                        bb4 += 7;
                        if (bb4 <= 64 && flag4 == true)
                        {
                            if (CheckIfMovable(color, bb4) == true)
                            {
                                blocks.Add(bb4);
                            }
                            else
                            {
                                flag4 = false;
                            }
                        }
                       
                        
                        mr4 = bb4 % 8;

                    }
                    if ((mr == 0 || bb1 > 64) && (mr2 == 1 || bb2 < 0) && (mr3 == 0 || bb3 < 0) && (mr4 == 1 || bb4 > 64))
                    {
                        flagb = true;
                    }

                }
                


            }
            else if( Name == "qb" || Name == "qw")
            {
                bool flagq1 = true;
                bool flagq2 = true;
                bool flagq3 = true;
                bool flagq4 = true;
                bool flagq5 = true;
                bool flagq6 = true;
                bool flagq7 = true;
                bool flagq8 = true;
                string color;
                if (Name == "qb")
                {
                    color = "black";
                }
                else
                {
                    color = "white";
                }
                int qf = index + 8;
                int qb = index - 8;
                int qr = index;
                int ql = index;
                int qpr1;
                int qpr2;
                if (index % 8 == 0)
                {
                    qpr1 = 8;
                    qpr2 = 8;
                }
                else
                {
                    qpr1 = index % 8;
                    qpr2 = index % 8;
                }
                int qd1 = index;
                int qd2 = index;
                int qd3 = index;
                int qd4 = index;
                bool flagq = true;
                int mr = 0;
                int mr2 = 1;
                int mr3 = 0;
                int mr4 = 1;
                while (flagq)
                {
                    if (qb > 0)
                    {
                        if (flagq1 == true)
                        {
                            if (CheckIfMovable(color, qb) == true)
                            {
                                blocks.Add(qb);
                            }
                            else
                            {
                                flagq1 = false;
                            }
                        }
                        qb = qb - 8;
                    }
                    if (qf <= 64)
                    {
                        if (flagq2 == true)
                        {
                            if (CheckIfMovable(color, qf) == true)
                            {
                                blocks.Add(qf);
                            }
                            else
                            {
                                flagq2 = false;
                            }
                        }
                        qf = qf + 8;
                    }
                    if (qpr1 < 8)
                    {

                        qr += 1;
                        if (flagq3 == true)
                        {
                            if (CheckIfMovable(color, qr) == true)
                            {
                                blocks.Add(qr);
                            }
                            else
                            {
                                flagq3 = false;
                            }
                        }
                        qpr1 += 1;
                    }
                    if (qpr2 > 0)
                    {
                        ql -= 1;
                        if (flagq4 == true)
                        {
                            if (CheckIfMovable(color, ql) == true)
                            {
                                blocks.Add(ql);
                            }
                            else
                            {
                                flagq4 = false;
                            }
                        }
                        qpr2 -= 1;
                    }
                    if (qd1 % 8 != 0 && qd1 <= 64)
                    {

                        qd1 += 9;
                        if (qd1 <= 64 && flagq5 == true)
                        {
                            if (CheckIfMovable(color, qd1) == true)
                            {
                                blocks.Add(qd1);
                            }
                            else
                            {
                                flagq5 = false;
                            }

                        }


                        mr = qd1 % 8;

                    }
                    if (qd2 % 8 != 1 && qd2 >= 0)
                    {
                        qd2 -= 9;
                        if (qd2 >= 0 && flagq6 == true)
                        {
                            if (CheckIfMovable(color, qd2) == true)
                            {
                                blocks.Add(qd2);
                            }
                            else
                            {
                                flagq6 = false;
                            }
                        }
                        mr2 = qd2 % 8;
                    }
                    if (qd3 % 8 != 0 && qd3 >= 0)
                    {
                        qd3 -= 7;
                        if (qd3 >= 0 && flagq7 == true)
                        {
                            if (CheckIfMovable(color, qd3) == true)
                            {
                                blocks.Add(qd3);
                            }
                            else
                            {
                                flagq7 = false;
                            }
                        }


                        mr3 = qd3 % 8;

                    }
                    if (qd4 % 8 != 1 && qd4 <= 64)
                    {
                        qd4 += 7;
                        if (qd4 <= 64 && flagq8 == true)
                        {
                            if (CheckIfMovable(color, qd4) == true)
                            {
                                blocks.Add(qd4);
                            }
                            else
                            {
                                flagq8 = false;
                            }
                        }
                        mr4 = qd4 % 8;

                    }
                    if (((mr == 0 || qd1 > 64) && (mr2 == 1 || qd2 < 0) && (mr3 == 0 || qd3 < 0) && (mr4 == 1 || qd4 > 64)) && (qb <= 0 && qf > 64 && qpr1 >= 8 && qpr2 <= 0))
                    {
                        flagq = false;
                    }
                }
            }
          
                    
            
        }
        public bool CheckIfMovable(string color, int b)
        {
            bool flag = true;
            if (color == "black")
            {
                if (names[b] == "pb" || names[b] == "tb" || names[b] == "hb" || names[b] == "bb" || names[b] == "qb" || names[b] == "kb")
                {
                    flag = false;
                }
                else if (names[b] == "pw" || names[b] == "tw" || names[b] == "hw" || names[b] == "bw" || names[b] == "qw" || names[b] == "kw")
                {
                    blocks.Add(b);
                    flag = false;
                }
                else
                {
                    flag = true;
                    return flag;
                }
                return flag;
            }
            else
            {
                if (names[b] == "pw" || names[b] == "tw" || names[b] == "hw" || names[b] == "bw" || names[b] == "qw" || names[b] == "kw")
                {
                    flag = false;
                }
                else if (names[b] == "pb" || names[b] == "tb" || names[b] == "hb" || names[b] == "bb" || names[b] == "qb" || names[b] == "kb")
                {
                    blocks.Add(b);
                    flag = false;
                }
                else
                {
                    flag = true;
                    return flag;
                }
                return flag;
            }

        }
        
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            Data = new Database();
            AccessPictureBoxes();
            string folderPath = @"C:\Users\30694\Pictures\Screenshots\chess1";
            imagePaths = GetImagePaths(folderPath);
            names.Add("k");
            chessReseter();
            insertInLists();
            InitializeClickOnPicBox();
            
        }

        private void InitializeClickOnPicBox()
        {
            int i = 1;
            while (i < 64)
            {

                i++;
            }
        }

        private void insertInLists()
        {
            int c = 1;
            string name;
            
            
            while (c <= 64)
            {
                string i = c.ToString();
                if (c == 1)
                {

                    Insert("tb", @"C:\Users\30694\Pictures\Screenshots\chess1\1.png" ,i);
                }
                if (c == 2)
                {

                    Insert("hb", @"C:\Users\30694\Pictures\Screenshots\chess1\2.png", i);
                }
                if (c == 3)
                {

                    Insert("bb", @"C:\Users\30694\Pictures\Screenshots\chess1\3.png",i);
                }
                if (c == 4)
                {

                    Insert("qb", @"C:\Users\30694\Pictures\Screenshots\chess1\4.png",i);
                }
                if (c == 5)
                {

                    Insert("kb", @"C:\Users\30694\Pictures\Screenshots\chess1\5.png",i);
                }
                if (c == 6)
                {

                    Insert("bb", @"C:\Users\30694\Pictures\Screenshots\chess1\3.png",i);
                }

                if (c == 8)
                {

                    Insert("towb", @"C:\Users\30694\Pictures\Screenshots\chess1\1.png", i );
                }
                if (c == 7)
                {

                    Insert("hb", @"C:\Users\30694\Pictures\Screenshots\chess1\2.png",   i);
                }
                if (c > 8 && c < 17)
                {
                    Insert("pb", @"C:\Users\30694\Pictures\Screenshots\Screenshot 2023-12-25 003040.png", i);
                }
                if (c > 16 && c < 49)
                {
                    Insert("n", "n", i);
                }
                if (c > 48 && c < 57)
                {
                    Insert("pw", @"C:\Users\30694\Pictures\Screenshots\Screenshot 2023-12-25 003712.png", i);
                }
            
                
                
                if (c == 57)
                {

                    Insert("tw", @"C:\Users\30694\Pictures\Screenshots\chess1\6.png", i);
                }
                if (c == 58)
                {

                    Insert("hw", @"C:\Users\30694\Pictures\Screenshots\chess1\7.png", i );
                }
                if (c == 59)
                {

                    Insert("bw", @"C:\Users\30694\Pictures\Screenshots\chess1\8.png", i);
                }
                if (c == 60)
                {

                    Insert("kw", @"C:\Users\30694\Pictures\Screenshots\chess1\9.png", i);
                }
                if (c == 61)
                {

                    Insert("qw", @"C:\Users\30694\Pictures\Screenshots\chess1\99.png", i);
                }
                if (c == 62)
                {

                    Insert("bw", @"C:\Users\30694\Pictures\Screenshots\chess1\8.png", i);
                }

                if (c == 63)
                {

                    Insert("hw", @"C:\Users\30694\Pictures\Screenshots\chess1\7.png", i);
                }
                if (c == 64)
                {

                    Insert("tw", @"C:\Users\30694\Pictures\Screenshots\chess1\6.png", i );
                }

                c++;
            }
        }
        public void Insert(string Name , string ImagePath , string Place)
        {
            imagePaths.Add(ImagePath);
            int k = int.Parse(Place);
            places.Add(k);
            names.Add(Name);
        }
        private void chessReseter()
        {

            int s = 1;
            int m = 5;
            bool flag = true;
            int i = 1;
            int r = 0;
            for (int j = 0; j <= 64; j++)
            {
                int k = j / 4;
                if (k <= 1 && flag == true)
                {
                    string pictureboxName = $"pictureBox{i}";
                    Control confound = Controls.Find(pictureboxName, true).FirstOrDefault();
                    PictureBox picturebox = (PictureBox)confound;
                    picturebox.SizeMode = PictureBoxSizeMode.StretchImage;
                    picturebox.ImageLocation = imagePaths[r];
                    if (i <= 3)
                    {
                        pictureboxName = $"pictureBox{9 - i}";
                        confound = Controls.Find(pictureboxName, true).FirstOrDefault();
                        picturebox = (PictureBox)confound;
                        picturebox.SizeMode = PictureBoxSizeMode.StretchImage;
                        picturebox.ImageLocation = imagePaths[r];
                    }

                    r++;
                    if (k == 1)
                    {
                        flag = false;
                    }
                }
                if (j >= 57 && j <= 61)
                {
                    string pictureboxName2 = $"pictureBox{j}";
                    Control confound2 = Controls.Find(pictureboxName2, true).FirstOrDefault();
                    picturebox2 = (PictureBox)confound2;
                    picturebox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    picturebox2.ImageLocation = imagePaths[m];
                    m++;
                }
                if (j >= 57 && j <= 59)

                {
                    string pictureboxName3 = $"pictureBox{65 - s}";
                    Control confound3 = Controls.Find(pictureboxName3, true).FirstOrDefault();
                    PictureBox picturebox3 = (PictureBox)confound3;
                    picturebox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    picturebox3.ImageLocation = picturebox2.ImageLocation;
                    s++;

                }



                i++;



            }
        }

        private List<string> GetImagePaths(string folderPath)
        {
            List<string> imagePaths = new List<string>();
            if (Directory.Exists(folderPath))
            {
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower())).ToArray();

                imagePaths.AddRange(files);
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
            }
            return imagePaths;
        }

        private void AccessPictureBoxes()
        {

            int pictureBoxIndex = 1;
            while (pictureBoxIndex <= 64)
            {

                string pictureBoxName = $"pictureBox{pictureBoxIndex}";
                Control foundControl = Controls.Find(pictureBoxName, true).FirstOrDefault();
                if (foundControl != null && foundControl is PictureBox pictureBox)
                {
                    if (pictureBoxIndex > 8 && pictureBoxIndex <= 16)
                    {
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.ImageLocation = "C:\\Users\\30694\\Pictures\\Screenshots\\Screenshot 2023-12-25 003040.png";
                    }
                    if (pictureBoxIndex > 48 && pictureBoxIndex <= 56)
                    {
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.ImageLocation = "C:\\Users\\30694\\Pictures\\Screenshots\\Screenshot 2023-12-25 003712.png";
                    }

                }
                else
                {
                    MessageBox.Show($"Control with name '{pictureBoxName}' not found or is not a PictureBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pictureBoxIndex++;

            }
        }
    }
}



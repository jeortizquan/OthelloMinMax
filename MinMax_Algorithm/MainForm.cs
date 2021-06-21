using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace MinMax_Algorithm
{
    public partial class MainForm : Form
    {
        #region " Attributes "
        private Board GameBoard;
        private Image ImgWhite, ImgBlack, ImgBoard;
        private Bitmap WhitePiece, BlackPiece, BmpBoard;
        private Graphics Canvas;
        private byte Turn = 1;

        private Connection PC1;
        public Thread ReadThread;
        public string Recibido;
        public ArrayList Datos;
        private APTree ABTree;
        private byte fcolor;
        #endregion

        #region " Constructor "
        public MainForm()
        {
            InitializeComponent();
            GameBoard = new Board();
            GameBoard.SetPiece(3, 4, 1);
            GameBoard.SetPiece(4, 4, 2);
            GameBoard.SetPiece(4, 3, 1);
            GameBoard.SetPiece(3, 3, 2);

        }
        #endregion

        #region " Draw Othello Board "
        private void DrawBoard(ref Graphics Canvas, ref Bitmap WhitePiece, ref Bitmap BlackPiece)
        {
            for(int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    SolidBrush br = new SolidBrush(Color.White);
                    Canvas.DrawString(i.ToString() + ":" + j.ToString(), new Font("Arial", 8, GraphicsUnit.Pixel), br, (int)i * 50 + 13, (int)j * 50 + 13);
                    switch (GameBoard.PieceBoard[j][i])
                    {
                        case 1:
                            Canvas.DrawImage(BlackPiece, new Rectangle((int)i*50+13, (int)j*50+13, 50, 50), new Rectangle(0, 0, BlackPiece.Width, BlackPiece.Height), GraphicsUnit.Pixel);
                            break;
                        case 2:
                            Canvas.DrawImage(WhitePiece, new Rectangle((int)i*50+13, (int)j*50+13, 50, 50), new Rectangle(0, 0, WhitePiece.Width, WhitePiece.Height), GraphicsUnit.Pixel);
                            break;
                    }
                }

        }
        #endregion

        #region " Draw Piece "
        private bool DrawPiece(byte i, byte j, byte _Color)
        {            
            if (GameBoard.isValidPosition(i, j))
            {
                GameBoard.SetPiece(i, j, Turn);
                GameBoard.Move(i, j, Turn);
                switch (_Color)
                {
                    case 1:                        
                        Canvas.DrawImage(BlackPiece, new Rectangle((int)i * 50 + 13, (int)j * 50 + 13, 50, 50), new Rectangle(0, 0, BlackPiece.Width, BlackPiece.Height), GraphicsUnit.Pixel);
                        break;
                    case 2:
                        Canvas.DrawImage(WhitePiece, new Rectangle((int)i * 50 + 13, (int)j * 50 + 13, 50, 50), new Rectangle(0, 0, WhitePiece.Width, WhitePiece.Height), GraphicsUnit.Pixel);
                        break;
                }
                return true;
            }
            else
                return false;
        }
        #endregion

        #region " Main Form Load "
        private void MainForm_Load(object sender, EventArgs e)
        {
            ABTree = new APTree();
            /* Conexión */
            List<Position> ValidMoves = new List<Position>();
            PC1 = new Connection();
            txtDatos.Text = "";
            Recibido = "";
            Datos = new ArrayList();

            ImgWhite = Image.FromFile("C:\\Temp\\White.bmp");
            WhitePiece = new Bitmap(ImgWhite);
            WhitePiece.MakeTransparent(Color.Magenta);
            ImgWhite = (Image)WhitePiece;
            
            ImgBlack = Image.FromFile("C:\\Temp\\Black.bmp");
            BlackPiece = new Bitmap(ImgBlack);
            BlackPiece.MakeTransparent(Color.Magenta);
            ImgBlack = (Image)BlackPiece;

            ImgBoard = Image.FromFile("c:\\Temp\\Board.bmp");
            BmpBoard = new Bitmap(ImgBoard);            
            Canvas = Graphics.FromImage(BmpBoard);            

            DrawBoard(ref Canvas, ref WhitePiece, ref BlackPiece);            
            pnlGame.BackgroundImage = BmpBoard;
            pnlGame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            
            ValidMoves = GameBoard.getValidMoves(Turn);
            foreach (Position p in ValidMoves)
                lstMoves.Items.Add("x: " + p.x.ToString() + " y:" + p.y.ToString() + "      B:" + p.Black.ToString() + " W:" + p.White.ToString());
            if (Turn == 1)
                lblTurn.Text = "Turn: Black";
            else
                lblTurn.Text = "Turn: White";
        }
        #endregion

        #region " Exit Program "
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            try
            {
                ReadThread.Abort();
            }
            catch { }
        }
        #endregion

        #region " Single Player Mouse Click"
        private void pnlGame_MouseClick(object sender, MouseEventArgs e)
        {
            byte x, y;
            List<Position> ValidMoves = new List<Position>();
            
            x = (byte)(e.Location.X/50);
            y = (byte)(e.Location.Y/50);            
            DrawPiece(x, y, Turn);
            txtMensaje.Text = x.ToString() + "," + y.ToString();
            DrawBoard(ref this.Canvas,ref this.WhitePiece,ref this.BlackPiece);
            Turn = (byte)((Turn % 2) + 1);
            lstMoves.Items.Clear();
            ValidMoves = GameBoard.getValidMoves(Turn);
            foreach (Position p in ValidMoves)
                lstMoves.Items.Add("x: " + p.x.ToString() + " y:" + p.y.ToString() + "      B:" + p.Black.ToString() + " W:" + p.White.ToString());
            pnlGame.Refresh();
            if (Turn == 1)
                lblTurn.Text = "Turn: Black";
            else
                lblTurn.Text = "Turn: White";
        }
        #endregion

        #region " Listen Check Button "
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                lblPlayer.Text = "Player: White";
                fcolor = 2;
                PC1.SetAddress(txtIP.Text, Convert.ToInt32(txtPuerto.Text));
                ReadThread = new Thread(new ThreadStart(ReadLoop));
                PC1.Set_Listen(100);
                btnConection.Enabled = false;
                try
                {
                    ReadThread.Start();
                    timer1.Enabled = true;
                }
                catch { }
            }
            else
            {
                btnConection.Enabled = true;
                ReadThread.Abort();
                timer1.Enabled = false;
            }

        }
        #endregion

        #region " Connect Button "
        private void btnConection_Click(object sender, EventArgs e)
        {
            if (btnConection.Text == "Connect")
            {
                fcolor = 1;
                lblPlayer.Text = "Player: Black";
                PC1.SetAddress(txtIP.Text, Convert.ToInt32(txtPuerto.Text));
                ReadThread = new Thread(new ThreadStart(ReadLoop));
                btnConection.Text = "Disconnect";

                checkBox1.Enabled = false;
                try
                {
                    ReadThread.Start();
                    timer1.Enabled = true;
                }
                catch { }
            }
            else
            {
                btnConection.Text = "Connect";
                checkBox1.Enabled = true;
                ReadThread.Abort();
                timer1.Enabled = false;
            }

        }
        #endregion

        #region " Send Data "
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string aux="";
            aux = txtMensaje.Text;
            PC1.SendData(aux);
            Put_Piece(aux);
        }
        #endregion

        #region " Fill Tree "
        private void Fill_Tree(Board _tree,int depth)
        {
            List<positionT> Ps = new List<positionT>();
            List<positionT> Pieces = new List<positionT>();
            Position p = new Position();

            if (depth != 0)
            {
                Board GameBoard = new Board();
                GameBoard = _tree;

                List<Position> ValidMoves = new List<Position>();

                if (depth>2)
                  DrawBoard(ref this.Canvas, ref this.WhitePiece, ref this.BlackPiece);
                Turn = (byte)((Turn % 2) + 1);
                lstMoves.Items.Clear();
                ValidMoves = GameBoard.getValidMoves(Turn);
                
                for (byte k = 0; k < ValidMoves.Count; k++)
                {
                    positionT _aux = new positionT();
                    positionT _aux2 = new positionT();
                    p = ValidMoves[k];

                    _aux.x = p.White;
                    _aux.y = p.Black;

                    Pieces.Add(_aux);

                    _aux2.x = p.x;
                    _aux2.y = p.y;
                    Ps.Add(_aux2);
                }
                //if (depth != 0)
                    ABTree.add(Ps, Pieces);

                for (int i = 0; i < Ps.Count; i++)
                {
                    Fill_Tree(GameBoard, depth - 1);
                }


            }
            else
            {
                Ps.Clear();
                Pieces.Clear();
                ABTree.add(Ps, Pieces);
            }

        }
        #endregion

        #region " Init Fill Tree "
        private int Fill_Tree()
        {
            List<Position> ValidMoves = new List<Position>();            
            Turn = (byte)((Turn % 2) + 1);
            lstMoves.Items.Clear();
            ValidMoves = GameBoard.getValidMoves(Turn);

            if (ValidMoves.Count==0)
                return 0;

            List<positionT> Ps = new List<positionT>();
            List<positionT> Pieces = new List<positionT>();

            foreach (Position p in ValidMoves)
            {

                positionT _aux = new positionT();
                positionT _aux2 = new positionT();

                _aux.x = p.White;
                _aux.y = p.Black;

                Pieces.Add(_aux);

                _aux2.x = p.x;
                _aux2.y = p.y;

                Ps.Add(_aux2);
            }
            ABTree.add(Ps, Pieces);

            for (int i = 0; i < Ps.Count; i++)
            {
                Fill_Tree(GameBoard, 3);
            }

            return 1;

        }
        #endregion

        #region " Put Piece "
        private bool Put_Piece(string _String)
        {
            bool done=true;
            byte b, w;
            b = w = 0;
            if ((GameBoard.isNotFull()))
            {
                    timer1.Enabled = false;
                    if (GameBoard.fichas()==32)
                    {
                      lblResult.Text = "Draw";
                    }
                    else
                    {
                        if (GameBoard.fichas() > 32)
                        {
                            if (fcolor == 1)
                                lblResult.Text = "You Win!!!";
                            else
                                lblResult.Text = "You Lost :(";
                        }
                        else
                        {
                            if (fcolor == 1)
                                lblResult.Text = "You Lost :(";
                            else
                                lblResult.Text = "You Win!!!";
                        }
                    }
             }
            if (_String != "")
            {
                    int _x = 0, _y = 0;
                    string aux = "";
                    txtDatos.Text = _String;
                    aux = parse(_String);
                    _x = Convert.ToInt32(aux);
                    //if (Recibido)
                    aux = parse(Remove(_String));
                    _y = Convert.ToInt32(aux);
                    txtDatos.Text = "X= " + _x.ToString() + "Y= " + _y.ToString();
                    if ((_x != -1)&&(_y!=-1))
                    {
                        System.GC.SuppressFinalize(ABTree);
                        ABTree = new APTree();
                        //ABTree.fina
                        pnlGame.Refresh();
                        if (Turn == 1)
                            lblTurn.Text = "Turn: Black";
                        else
                            lblTurn.Text = "Turn: White";

                        if (DrawPiece(Convert.ToByte(_x), Convert.ToByte(_y), Turn))
                        {
                            DrawBoard(ref this.Canvas, ref this.WhitePiece, ref this.BlackPiece);
                            pnlGame.Refresh();
                        }
                        else
                        {
                            Turn = (byte)((Turn % 2) + 1);
                            done = false;
                        }

                        byte a = Turn;
                        int _next = 0;
                        if (fcolor != a)
                            _next = Fill_Tree();

                        Turn = a;
                        Turn = (byte)((Turn % 2) + 1);
                        positionT xy = new positionT();
                        if (fcolor != a)
                        {
                            if (_next == 1)
                            {
                                xy = ABTree.ABP();
                                txtMensaje.Text = xy.x.ToString() + "," + xy.y.ToString();
                                Put_Piece(txtMensaje.Text);
                            }
                            else
                            {
                                txtMensaje.Text = "-1,-1";
                                Turn = (byte)((Turn % 2) + 1);
                            }

                            PC1.SendData(txtMensaje.Text);

                        }

                    }
                    else
                    {
                        // aqui ver que va a hacer cuando regrese un error

                        System.GC.SuppressFinalize(ABTree);
                        ABTree = new APTree();
                        //ABTree.fina
                        pnlGame.Refresh();
                        if (Turn == 1)
                            lblTurn.Text = "Turn: Black";
                        else
                            lblTurn.Text = "Turn: White";


                        byte a = Turn;
                        int _next = 0;
                        if (fcolor != a)
                            _next = Fill_Tree();

                        Turn = a;
                        Turn = (byte)((Turn % 2) + 1);
                        positionT xy = new positionT();
                        if (fcolor != a)
                        {
                            if (_next == 1)
                            {
                                xy = ABTree.ABP();
                                txtMensaje.Text = xy.x.ToString() + "," + xy.y.ToString();
                                Put_Piece(txtMensaje.Text);
                            }
                            else
                            {
                                txtMensaje.Text = "-1,-1";
                                Turn = (byte)((Turn % 2) + 1);
                            }
                            PC1.SendData(txtMensaje.Text);

                        }

                    }
                    Recibido = "";
            }
            GameBoard.Pieces(ref b, ref w);
            lblScore.Text = "Score = Black : " + b.ToString() + " White: " + w.ToString();
            return done;
        }
        #endregion

        #region " Timer Tick "
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!(Put_Piece(Recibido)))
                PC1.SendData("-1,-1");

        }
        #endregion

        #region " Parse Message "
        private string parse(string _string)
        {
            string aux="";
            for (int a = 0; a < _string.Length; a++)
            {
                if ((_string[a] != ',') && (_string[a] != '\n'))
                    aux = aux + _string[a];
                else
                    return aux;
            }
            return aux;
        }
        #endregion

        #region " Remove Message "
        private string Remove(string _string)
        {
            string aux = "";
            for (int a = 0; a < _string.Length; a++)
            {
                aux = aux + _string[a];
                if ((_string[a] == ','))
                    aux = "";
            }
            return aux;
        }
        #endregion

        #region " Read Loop "
        private void ReadLoop()
        {
            byte[] TempBuffer;
            string TempString;

            // Intentar conectarse con la silla operativa y obtener el error si no se puede.
            string error = PC1.Connect();

            // Si no hubo ningún error al conectarse.
            if (error == "")
            {
                while (true)
                {
                    try
                    {
                        // Llamada a la recepción de datos, regresa cuando se recibieron datos.
                        TempBuffer = PC1.GetReceivedData();
                        System.Text.Encoding enc = System.Text.Encoding.ASCII;
                        // Obtener el string equivalente a los bytes recibidos.
                        TempString = enc.GetString(TempBuffer);
                        // Colocar los dos bytes en el buffer para su procesamiento posterior.
                        if (TempString[0] != 0)
                            Recibido = Recibido + TempString[0];
                        if (TempBuffer[1] != 0)
                            Recibido = Recibido + TempString[1];
                    }
                    catch
                    {
                        ReadThread.Abort(); // Agregado
                    }
                }
            }
            // Si ocurrió un error al intentar conectarse.
            else
            {
                ReadThread.Abort();
                btnConection.Text = "Connect";
            }
        }
        #endregion

        #region " Main Form Close "
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ReadThread.Abort();
            }
            catch { }
        }
        #endregion

        #region " About "
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("UVG AI 2008 \nby\nHenry Martinez & Jorge Ortiz", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region " New Game "
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameBoard.Clear();
            GameBoard.SetPiece(3, 4, 1);
            GameBoard.SetPiece(4, 4, 2);
            GameBoard.SetPiece(4, 3, 1);
            GameBoard.SetPiece(3, 3, 2);
            MainForm_Load(sender, e);
        }
        #endregion

    }
}

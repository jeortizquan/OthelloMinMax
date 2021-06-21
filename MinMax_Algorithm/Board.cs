using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinMax_Algorithm
{
    class Board
    {
        #region " Attributes "
        public byte [][] PieceBoard;
        public byte [][] VirtualPieceBoard;
        #endregion

        #region " Constructor "
        public Board()
        {
            PieceBoard = new byte[8][];
            for (int i = 0; i < 8; i++)
                PieceBoard[i] = new byte[8];

            VirtualPieceBoard = new byte[8][];
            for (int i = 0; i < 8; i++)
                VirtualPieceBoard[i] = new byte[8];

        }
        #endregion

        #region " Valid Position "
        public bool isValidPosition(byte i, byte j)
        {
            if (PieceBoard[j][i] == 0)         
                return true;            
            else
                return false;
        }
        #endregion

        #region " Obtain Positions of Valid Moves "
        public List<Position> getValidMoves(byte Color)
        {
            List<Position> lstPositions = new List<Position>();
            List<Position> lstResult = new List<Position>();
            Position p = new Position();
            byte Black, White;
            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (isValidPosition(i, j))
                        lstPositions.Add(new Position(i, j));
                }
            }
            for (byte k = 0; k < lstPositions.Count; k++)
            {
                Black = White = 0;
                p = lstPositions[k];
                if (PosibleMove(p.x, p.y, Color, ref Black, ref White))
                {
                    lstResult.Add(new Position(p.x, p.y, Black, White));
                }
            }           
            return lstResult;

        }
        #endregion

        #region " Set Piece in position i, j, Color "
        public void SetPiece(byte i, byte j, byte Color)
        {
            PieceBoard[j][i] = Color;
        }
        #endregion

        #region " Set Virtual Piece in position  i, j, Color "
        public void SetVirtualPiece(byte i, byte j, byte Color)
        {
            VirtualPieceBoard[j][i] = Color;
        }
        #endregion

        #region " Eat Piece "
        public void Move(byte i, byte j, byte Color)
        {
            bool found;
            byte v = 0;
            byte u = 0;

            #region " *** Up *** "
            // Up
            // --
            found = false;
            u = j;
            if (u < 7)
            {
                while (u < 7 && PieceBoard[u + 1][i] != Color && PieceBoard[u + 1][i] != 0)
                {
                    u = (byte)(u + 1);

                    if (u < 7 && PieceBoard[u + 1][i] != (Color % 2) + 1 && PieceBoard[u + 1][i] != 0)
                    {
                        found = true;                        
                    }
                }
                u = j;
                while (found && PieceBoard[u + 1][i] != Color && PieceBoard[u + 1][i] != 0)
                {
                    u = (byte)(u + 1);
                    SetPiece(i, u, Color);
                }
            }
            #endregion

            #region " *** Down *** "
            // Down
            // ----
            found = false;
            u = j;
            if (u > 0)
            {
                while (u > 0 && PieceBoard[u - 1][i] != Color && PieceBoard[u - 1][i] != 0)
                {
                    u = (byte)(u - 1);
                    if (u > 0 && PieceBoard[u - 1][i] != (Color % 2) + 1 && PieceBoard[u - 1][i] != 0)
                    {
                        found = true;                        
                    }
                }
                u = j;
                while (found && PieceBoard[u - 1][i] != Color && PieceBoard[u - 1][i] != 0)
                {
                    u = (byte)(u - 1);
                    SetPiece(i, u, Color);
                }
            }
            #endregion

            #region " *** Right *** "
            // Right
            // -----
            found = false;
            u = i;
            if (u < 7)
            {
                while (u < 7 && PieceBoard[j][u + 1] != Color && PieceBoard[j][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    if (u < 7 && PieceBoard[j][u + 1] != (Color % 2) + 1 && PieceBoard[j][u + 1] != 0)
                    {
                        found = true;                       
                    }
                }
                u = i;
                while (found && PieceBoard[j][u + 1] != Color && PieceBoard[j][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    SetPiece(u, j, Color);
                }
            }
            #endregion

            #region " *** Left *** "
            // Left
            // -----
            found = false;
            u = i;
            if (u > 0)
            {
                while (u > 0 && PieceBoard[j][u - 1] != Color && PieceBoard[j][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    if (u > 0 && PieceBoard[j][u - 1] != (Color % 2) + 1 && PieceBoard[j][u - 1] != 0)
                    {
                        found = true;
                    }
                }
                u = i;
                while (found && PieceBoard[j][u - 1] != Color && PieceBoard[j][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    SetPiece(u, j, Color);
                }
            }
            #endregion

            #region " *** Down Right *** "
            // Down Right
            // ----------
            found = false;
            u = i; v = j;
            if (u < 7 && v < 7)
            {
                while (u < 7 && v < 7 && PieceBoard[v + 1][u + 1] != Color && PieceBoard[v + 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v + 1);
                    if (u < 7 && v < 7 && PieceBoard[v + 1][u + 1] != (Color % 2) + 1 && PieceBoard[v + 1][u + 1] != 0)
                    {
                        found = true;                        
                    }
                }
                u = i; v = j;
                while (found && PieceBoard[v + 1][u + 1] != Color && PieceBoard[v + 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v + 1);
                    SetPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Up Right *** "
            // Up Right
            // --------
            found = false;
            u = i; v = j;
            if (u < 7 && v > 0)
            {
                while (u < 7 && v > 0 && PieceBoard[v - 1][u + 1] != Color && PieceBoard[v - 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v - 1);
                    if (u < 7 && v > 0 && PieceBoard[v - 1][u + 1] != (Color % 2) + 1 && PieceBoard[v - 1][u + 1] != 0)
                    {
                        found = true;                        
                    }
                }
                u = i; v = j;
                while (found && PieceBoard[v - 1][u + 1] != Color && PieceBoard[v - 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v - 1);
                    SetPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Down Left *** "
            // Down Left
            // ---------
            found = false;
            u = i; v = j;
            if (u > 0 && v > 0)
            {
                while (u > 0 && v > 0 && PieceBoard[v - 1][u - 1] != Color && PieceBoard[v - 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v - 1);
                    if (u > 0 && v > 0 && PieceBoard[v - 1][u - 1] != (Color % 2) + 1 && PieceBoard[v - 1][u - 1] != 0)
                    {
                        found = true;                        
                    }
                }
                u = i; v = j;
                while (found && PieceBoard[v - 1][u - 1] != Color && PieceBoard[v - 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v - 1);
                    SetPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Up Left *** "
            // Up Left
            // -------
            found = false;
            u = i; v = j;
            if (u > 0 && v < 7)
            {
                while (u > 0 && v < 7 && PieceBoard[v + 1][u - 1] != Color && PieceBoard[v + 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v + 1);
                    if (u > 0 && v < 7 && PieceBoard[v + 1][u - 1] != (Color % 2) + 1 && PieceBoard[v + 1][u - 1] != 0)
                    {
                        found = true;                        
                    }
                }
                u = i; v = j;
                while (found && PieceBoard[v + 1][u - 1] != Color && PieceBoard[v + 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v + 1);
                    SetPiece(u, v, Color);
                }
            }
            #endregion
        }
        #endregion

        #region " Check if the Board is Full "
        public bool isNotFull()
        { 
         for (int i=0; i<8;i++)
         {
             for (int j=0; j<8;j++)
             {
                 if (PieceBoard[i][j] == 0)
                     return false;
             }
         }
         return true;
        }
        #endregion

        #region " Pieces "
        public int fichas()
        {
            int aux = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (PieceBoard[i][j] == 1)
                        aux += 1;
                }
            }
            return aux;
        }
        #endregion


        #region " Posible Moves in position i, j with Color, return Black & White Pieces  "
        public bool PosibleMove(byte i, byte j, byte Color, ref byte Black, ref byte White)
        {
            bool [] found = new bool[8];
            byte v = 0;
            byte u = 0;

            for (u = 0; u < 8; u++)
                for (v = 0; v < 8; v++)
                    VirtualPieceBoard[u][v] = PieceBoard[u][v];

            #region " *** Up *** "
            // Up
            // --
            found[0] = false;
            u = j;
            if (u < 7)
            {
                while (u < 7 && VirtualPieceBoard[u + 1][i] != Color && VirtualPieceBoard[u + 1][i] != 0)
                {
                    u = (byte)(u + 1);

                    if ( u < 7 && VirtualPieceBoard[u + 1][i] != (Color % 2) + 1 && VirtualPieceBoard[u + 1][i] != 0)
                    {
                        found[0] = true;                        
                    }
                }
                u = j;
                while (found[0] && VirtualPieceBoard[u + 1][i] != Color && VirtualPieceBoard[u + 1][i] != 0)
                {
                    u = (byte)(u + 1);
                    SetVirtualPiece(i, u, Color);
                }
            }
            #endregion

            #region " *** Down *** "
            // Down
            // ----
            found[1] = false;
            u = j;
            if (u > 0)
            {
                while (u > 0 && VirtualPieceBoard[u - 1][i] != Color && VirtualPieceBoard[u - 1][i] != 0)
                {
                    u = (byte)(u - 1);
                    if (u > 0 && VirtualPieceBoard[u - 1][i] != (Color % 2) + 1 && VirtualPieceBoard[u - 1][i] != 0)
                    {
                        found[1] = true;
                    }
                }
                u = j;
                while (found[1] && VirtualPieceBoard[u - 1][i] != Color && VirtualPieceBoard[u - 1][i] != 0)
                {
                    u = (byte)(u - 1);
                    SetVirtualPiece(i, u, Color);
                }
            }
            #endregion 

            #region " *** Right *** "
            // Right
            // -----
            found[2] = false;
            u = i;
            if (u < 7)
            {
                while (u < 7 && VirtualPieceBoard[j][u + 1] != Color && VirtualPieceBoard[j][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    if (u < 7 && VirtualPieceBoard[j][u + 1] != (Color % 2) + 1 && VirtualPieceBoard[j][u + 1] != 0)
                    {
                        found[2] = true;
                    }
                }
                u = i;
                while (found[2] && VirtualPieceBoard[j][u + 1] != Color && VirtualPieceBoard[j][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    SetVirtualPiece(u, j, Color);
                }
            }
            #endregion

            #region " *** Left *** "
            // Left
            // -----
            found[3] = false;
            u = i;
            if (u > 0)
            {
                while (u > 0 && VirtualPieceBoard[j][u - 1] != Color && VirtualPieceBoard[j][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    if (u > 0 && VirtualPieceBoard[j][u - 1] != (Color % 2) + 1 && VirtualPieceBoard[j][u - 1] != 0)
                    {
                        found[3] = true;
                    }
                }
                u = i;
                while (found[3] && VirtualPieceBoard[j][u - 1] != Color && VirtualPieceBoard[j][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    SetVirtualPiece(u, j, Color);
                }
            }
            #endregion

            #region " *** Down Right *** "
            // Down Right
            // ----------
            found[4] = false;
            u = i; v = j;
            if (u < 7 && v < 7)
            {
                while (u < 7 && v < 7 && VirtualPieceBoard[v + 1][u + 1] != Color && VirtualPieceBoard[v + 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v + 1);
                    if (u < 7 && v < 7 && VirtualPieceBoard[v + 1][u + 1] != (Color % 2) + 1 && VirtualPieceBoard[v + 1][u + 1] != 0)
                    {
                        found[4] = true;
                    }
                }
                u = i; v = j;
                while (found[4] && VirtualPieceBoard[v + 1][u + 1] != Color && VirtualPieceBoard[v + 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v + 1);
                    SetVirtualPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Up Right *** "
            // Up Right
            // --------
            found[5] = false;
            u = i; v = j;
            if (u < 7 && v > 0)
            {
                while (u < 7 && v > 0 && VirtualPieceBoard[v - 1][u + 1] != Color && VirtualPieceBoard[v - 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v - 1);
                    if (u < 7 && v > 0 && VirtualPieceBoard[v - 1][u + 1] != (Color % 2) + 1 && VirtualPieceBoard[v - 1][u + 1] != 0)
                    {
                        found[5] = true;
                    }
                }
                u = i; v = j;
                while (found[5] && VirtualPieceBoard[v - 1][u + 1] != Color && VirtualPieceBoard[v - 1][u + 1] != 0)
                {
                    u = (byte)(u + 1);
                    v = (byte)(v - 1);
                    SetVirtualPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Down Left *** "
            // Down Left
            // ---------
            found[6] = false;
            u = i; v = j;
            if (u > 0 && v > 0)
            {
                while (u > 0 && v > 0 && VirtualPieceBoard[v - 1][u - 1] != Color && VirtualPieceBoard[v - 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v - 1);
                    if (u > 0 && v > 0 && VirtualPieceBoard[v - 1][u - 1] != (Color % 2) + 1 && VirtualPieceBoard[v - 1][u - 1] != 0)
                    {
                        found[6] = true;
                    }
                }
                u = i; v = j;
                while (found[6] && VirtualPieceBoard[v - 1][u - 1] != Color && VirtualPieceBoard[v - 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v - 1);
                    SetVirtualPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Up Left *** "
            // Up Left
            // -------
            found[7] = false;
            u = i; v = j;
            
            if (u > 0 && v < 7)
            {
                while (u > 0 && v < 7 && VirtualPieceBoard[v + 1][u - 1] != Color && VirtualPieceBoard[v + 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v + 1);
                    if (u > 0 && v < 7 && VirtualPieceBoard[v + 1][u - 1] != (Color % 2) + 1 && VirtualPieceBoard[v + 1][u - 1] != 0)
                    {
                        found[7] = true;
                    }
                }
                u = i; v = j;
                while (found[7] && VirtualPieceBoard[v + 1][u - 1] != Color && VirtualPieceBoard[v + 1][u - 1] != 0)
                {
                    u = (byte)(u - 1);
                    v = (byte)(v + 1);
                    SetVirtualPiece(u, v, Color);
                }
            }
            #endregion

            #region " *** Obtain if was a valid move at given position *** "
            // Obtain if was a valid move at given position
            // --------------------------------------------
            for (byte k = 1; k < 8; k++)
            {
                found[0] |= found[k];
            }
            #endregion

            #region " *** Obtain Number of Pieces of each Color *** "
            // Obtain Number of Pieces of each Color
            // -------------------------------------
            for (byte k = 0; k < 8; k++)
            {
                for (byte l = 0; l < 8; l++)
                {
                    if (VirtualPieceBoard[k][l] == 1)
                        Black++;
                    if (VirtualPieceBoard[k][l] == 2)
                        White++;
                }
            }
            #endregion
            return found[0];
        }
        #endregion

        #region " Clear Board "
        public void Clear()
        {
            for (byte u = 0; u < 8; u++)
                for (byte v = 0; v < 8; v++)
                    this.PieceBoard[u][v] = 0;
        }
        #endregion
        
        #region "Obtain Number of Pieces of each Color"
        public void Pieces(ref byte Black,ref byte White)
        {            
            for (byte k = 0; k < 8; k++)
            {
                for (byte l = 0; l < 8; l++)
                {
                    if (PieceBoard[k][l] == 1)
                        Black++;
                    if (PieceBoard[k][l] == 2)
                        White++;
                }
            }
        }
        #endregion

        #region " Get the Number of White Pieces "
        public byte getWhite()
        {
            byte White = 0;
            for (byte k = 0; k < 8; k++)
            {
                for (byte l = 0; l < 8; l++)
                {             
                    if (PieceBoard[k][l] == 2)
                        White++;
                }
            }
            return White;
        }
        #endregion

        #region " Get the Number of Black Pieces "
        public byte getBlack()
        {
            byte Black = 0;
            for (byte k = 0; k < 8; k++)
            {
                for (byte l = 0; l < 8; l++)
                {
                    if (PieceBoard[k][l] == 1)
                        Black++;                   
                }
            }
            return Black;
        }
        #endregion
    }
}

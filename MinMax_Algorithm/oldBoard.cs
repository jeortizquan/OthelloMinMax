using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinMax_Algorithm
{
    class Board
    {
        public byte [][] PieceBoard;
        public Board()
        {
            PieceBoard = new byte[8][];
            for (int i = 0; i < 8; i++)
                PieceBoard[i] = new byte[8];
                
        }
        public bool SetPiece(byte i, byte j, byte Color)
        {
            if (PieceBoard[j][i]==0)
            {
                PieceBoard[j][i] = Color;
                return true;
            }
            else
                return false;
        }
        public void Move(byte i, byte j, byte Color)
        {
            byte v = 0;
            byte u = 0;
            u = j;            
            while (PieceBoard[u + 1][i] != Color && PieceBoard[u + 1][i] != 0)
            {
                u =(byte)(u + 1);
                SetPiece(i, u, Color);
            }
            u = j;
            while (PieceBoard[u - 1][i] != Color && PieceBoard[u - 1][i] != 0)
            {
                u = (byte)(u - 1);
                SetPiece(i, u, Color);
            }
            u = i;
            while (PieceBoard[j][u + 1] != Color && PieceBoard[j][u + 1] != 0)
            {
                u = (byte)(u + 1);
                SetPiece(u,j, Color);
            }
            u = i;
            while (PieceBoard[j][u - 1] != Color && PieceBoard[j][u - 1] != 0)
            {
                u = (byte)(u - 1);
                SetPiece(u, j, Color);
            }
            u = i; v = j;
            while (PieceBoard[v + 1][u + 1] != Color && PieceBoard[v + 1][u + 1] != 0)
            {
                u = (byte)(u + 1);
                v = (byte)(v + 1);
                SetPiece(u, v, Color);
            }
            u = i; v = j;
            while (PieceBoard[v - 1][u + 1] != Color && PieceBoard[v - 1][u + 1] != 0)
            {
                u = (byte)(u + 1);
                v = (byte)(v - 1);
                SetPiece(u, v, Color);
            }
            u = i; v = j;
            while (PieceBoard[v - 1][u - 1] != Color && PieceBoard[v - 1][u - 1] != 0)
            {
                u = (byte)(u - 1);
                v = (byte)(v - 1);
                SetPiece(u, v, Color);
            }
            u = i; v = j;
            while (PieceBoard[v + 1][u - 1] != Color && PieceBoard[v + 1][u - 1] != 0)
            {
                u = (byte)(u - 1);
                v = (byte)(v + 1);
                SetPiece(u, v, Color);
            }
        }
    }
}

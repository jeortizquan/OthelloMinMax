using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinMax_Algorithm
{

    class positionT
    {
        public int x, y;
        public positionT()
        {
            x = 0;
            y = 0;
        }
    }

    class APNode
    {
        private positionT[]_position;
        private positionT[] _fichas;
        //private int f1, f2;
        public APNode[] children;
        public int num;
        public int actual;

    public APNode() 
    {
        _position = null;
        _fichas = null;
        children = null;
        num = 0;
        actual = 0;
    }

    public void crete_Posiciones(List<positionT> pos, List<positionT> Fichas)
    {
        int size = pos.Count;
        _position = new positionT[size];
        _fichas = new positionT[size];
        create_Children(size);
        for (int i = 0; i < size; i++)
        {
            _position[i] = pos[i];
            _fichas[i] = Fichas[i];
        }
        num = size;
    }
    public void create_Children(int num)
    {
        children = new APNode[num];
    }
    public void set_Position(int _num,positionT pos)
    {
        _position[_num] = pos;
    }
    public void swap(int num)
    { 
        int aux;
        aux = _fichas[num].x;
        _fichas[num].x = _fichas[num].y;
        _fichas[num].y = aux;
    }

    public positionT get_Position(int _num)
    {
        if (_position.Length > 0)
            return _position[_num];
        else
            return new positionT();
    }
	
    public int return_Valuex(int num)
    {
        return _fichas[num].x;
    }

    public int return_Valuey(int num)
    {
        return _fichas[num].y;
    }


    }

    class APTree
    {
        public APNode root;
        public positionT _position;
        public APNode _actual;
        public APTree() 
        {
            root = null;
            _position = new positionT();
            _actual = root;
        }
        public APNode create_Node()
        {
            APNode aux;
            aux = new APNode();
            return aux;
        }

        public APNode get_Root()
        {
            return root;
        }
        public void clear()
        {
            root = null;
        }

        public int add(List<positionT> posibles_lugares_Numero, List<positionT> Fichas)
        {
            _actual = create_Node();
            int aux=0;
            if (root == null)
            {
                root = _actual;
            }
            else
            {
                aux=next(root);
            }
            _actual.crete_Posiciones(posibles_lugares_Numero,Fichas);

            return aux;
            //_actual.set_Position
        
        }

        private int next(APNode _aux)
        {
            int tmp=1;
            if (_aux.actual != _aux.num)
            {
                if (_aux.children[_aux.actual] == null)
                {
                    _aux.children[_aux.actual] = _actual;
                    tmp = 1;
                }
                else
                {
                    int b = next(_aux.children[_aux.actual]);
                    if ((b != 1))
                    {
                        _aux.actual++;
                        if (_aux.actual < _aux.num)
                        {
                            _aux.children[_aux.actual] = _actual;
                            tmp = 1;
                        }
                        else
                            tmp = 2;

                    }
                }
            }
            else
                tmp = 2;
            return tmp;
        }

        public positionT ABP()
        {
            if (root != null)
            {
                _position = root.get_Position(0);
                ABP(root, 3, -80, 80,0);
            }
            else
                ABP(root, 3, -80, 80,0);

            return _position;
        }

        private int max(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }
        private int ABP(APNode node, int depth, int Alpha, int Beta,int _child)
        {
            if (depth == 0)
                return node.return_Valuey(_child);
            if  (node.num == 0)
            {
                if (depth%2==0)
                    return -80;
                else
                    return 80;
            }
            //_position = node.get_Position(_aux);
            //foreach (APNode n in node.children)
            for (int ll = 0; ll < node.num;ll++ )
            {
                Alpha = max(Alpha, -ABP(node.children[ll], depth - 1, -Beta, -Alpha,ll));
                if (Alpha > Beta)
                {
                    _position = node.children[ll].get_Position(ll);
                    return Alpha;
                }
                return Alpha;
            }
            return 0;
        }




    }
}

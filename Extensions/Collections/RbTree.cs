using System;
using System.Collections.Generic;

namespace Extensions.Collections
{
    public class RbNode<T>
    {
        public RbNode<T> Right;
        public RbNode<T> Left;
        public RbNode<T> Parent;
        public bool Red;
        public T Key;

        private static bool Compare(T x, T y, int res)
        {
            var comparer = Comparer<T>.Default;
            return comparer.Compare(x, y) == res;
        }

        public static bool IsGreaterThan(T x, T y)
        {
            return Compare(x, y, 1);
        }

        public static bool IsEqualTo(T x, T y)
        {
            return Compare(x, y, 0);
        }
    }

    public class RbTree<T> where T : IComparable<T>
    {
        public RbNode<T> Root { get; private set; }
        public int Count { get; private set; }
        public int Height => Root != TNull ? GetHeight(Root) : 0;
        
        private readonly RbNode<T> TNull;

        public RbTree()
        {
            TNull = new RbNode<T>();
            Root = TNull;
        }
        
        public bool Contains(T key)
        {
            var node = Find(key);
            return node != null;
        }

        public void Insert(T key)
        {
            var x = new RbNode<T>()
            {
                Key = key,
                Red = true,
                Left = TNull,
                Right = TNull,
            };
            
            if(Root != TNull)
            {
                var y = Root;
                var p = Root;

                while (y != TNull)
                {
                    p = y;
                    y = RbNode<T>.IsGreaterThan(x.Key, y.Key) ? y.Right : y.Left;
                }

                x.Parent = p;
                if (RbNode<T>.IsGreaterThan(x.Key, p.Key))
                    x.Parent.Right = x;
                else
                    x.Parent.Left = x;

                InsertFix(x);
            }
            else
            {
                Root = x;
                Root.Red = false;
            }

            Count++;
        }

        public void Remove(T key)
        {
            var z = Find(key);
            if (z == null) return;

            RbNode<T> y;

            if (z.Left == TNull || z.Right == TNull)
            {
                y = z;
            }
            else
            {
                y = z.Right;
                while (y.Left != TNull) y = y.Left;
            }

            var x = y.Left != TNull ? y.Left : y.Right;

            x.Parent = y.Parent;
            if (y.Parent != null)
            {
                if (y == y.Parent.Left)
                    y.Parent.Left = x;
                else
                    y.Parent.Right = x;
            }
            else
            {
                Root = x;
            }

            if (y != z) z.Key = y.Key;

            if (!y.Red)
                RemoveFix(x);

            Count--;
        }

        private void InsertFix(RbNode<T> x)
        {
            while (x != Root && x.Parent.Red)
            {
                RbNode<T> y;
                if (x.Parent == x.Parent.Parent.Left)
                {
                    y = x.Parent.Parent.Right;
                    if (y.Red)
                    {
                        x.Parent.Red = false;
                        y.Red = false;
                        x.Parent.Parent.Red = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Right)
                        {
                            x = x.Parent;
                            RotateLeft(x);
                        }

                        x.Parent.Red = false;
                        x.Parent.Parent.Red = true;
                        RotateRight(x.Parent.Parent);
                    }
                }
                else
                {
                    y = x.Parent.Parent.Left;
                    if (y.Red)
                    {
                        x.Parent.Red = false;
                        y.Red = false;
                        x.Parent.Parent.Red = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Left)
                        {
                            x = x.Parent;
                            RotateRight(x);
                        }

                        x.Parent.Red = false;
                        x.Parent.Parent.Red = true;
                        RotateLeft(x.Parent.Parent);
                    }
                }
            }

            Root.Red = false;
        }

        private void RemoveFix(RbNode<T> x)
        {
            while (x != Root && !x.Red)
            {
                if (x == x.Parent.Left)
                {
                    var w = x.Parent.Right;
                    if (w.Red)
                    {
                        w.Red = false;
                        x.Parent.Red = true;
                        RotateLeft(x.Parent);
                        w = x.Parent.Right;
                    }

                    if (!w.Left.Red && !w.Right.Red)
                    {
                        w.Red = true;
                        x = x.Parent;
                    }
                    else
                    {
                        if (!w.Right.Red)
                        {
                            w.Left.Red = false;
                            w.Red = true;
                            RotateRight(w);
                            w = x.Parent.Right;
                        }

                        w.Red = x.Parent.Red;
                        x.Parent.Red = false;
                        w.Right.Red = false;
                        RotateLeft(x.Parent);
                        x = Root;
                    }
                }
                else
                {
                    var w = x.Parent.Left;
                    if (w.Red)
                    {
                        w.Red = false;
                        x.Parent.Red = true;
                        RotateRight(x.Parent);
                        w = x.Parent.Left;
                    }

                    if (!w.Right.Red && !w.Left.Red)
                    {
                        w.Red = true;
                        x = x.Parent;
                    }
                    else
                    {
                        if (!w.Left.Red)
                        {
                            w.Right.Red = false;
                            w.Red = true;
                            RotateLeft(w);
                            w = x.Parent.Left;
                        }

                        w.Red = x.Parent.Red;
                        x.Parent.Red = false;
                        w.Left.Red = false;
                        RotateRight(x.Parent);
                        x = Root;
                    }
                }
            }

            x.Red = false;
        }

        private void RotateLeft(RbNode<T> x)
        {
            var y = x.Right;

            x.Right = y.Left;
            if (y.Left != TNull) y.Left.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent != null)
            {
                if (x == x.Parent.Left) x.Parent.Left = y;
                else x.Parent.Right = y;
            }
            else
                Root = y;

            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(RbNode<T> x)
        {
            var y = x.Left;

            x.Left = y.Right;
            if (y.Right != TNull) y.Right.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent != null)
            {
                if (x == x.Parent.Right) x.Parent.Right = y;
                else x.Parent.Left = y;
            }
            else
                Root = y;

            y.Right = x;
            x.Parent = y;
        }

        public RbNode<T> Find(T key)
        {
            var current = Root;
            while (current != TNull)
            {
                if (RbNode<T>.IsEqualTo(key, current.Key))
                    return current;

                current = RbNode<T>.IsGreaterThan(key, current.Key) ? current.Right : current.Left;
            }

            return null;
        }

        private int GetHeight(RbNode<T> x)
        {
            if (x == null || x == TNull) return -1;
            var a = GetHeight(x.Left);
            var b = GetHeight(x.Right);
            return a > b ? a + 1 : b + 1; 
        }
    }
}
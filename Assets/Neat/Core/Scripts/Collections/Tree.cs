using System;

namespace Neat.Collections
{
    public class Tree<T> where T : IComparable
    {
        public class Node
        {
            private T data;
            private IList<Node> children;

            public Node(T data)
            {
                this.data = data;
                children = new List<Node>();
            }
            public T GetData()
            {
                return data;
            }
            public IList<Node> Children()
            {
                return children;
            }
            public void AddChild(T data)
            {
                children.AddLast(new Node(data));
            }
            public void AddChildren(T[] data)
            {
                Node[] nodes = new Node[data.Length];
                for (int i = 0; i < data.Length; i++)
                    nodes[i] = new Node(data[i]);
                children.AddRange(nodes);
            }
        }

        protected Node root;

        public Tree(T root)
        {
            this.root = new Node(root);
        }
        public void Add(T item)
        {
            Node current = root;
            int comp = item.CompareTo(current.GetData());
            while (comp < 0)
            {
                // find the node in the children of the current node that the parameter belongs under
                IList<Node> children = current.Children();

                foreach (Node child in children)
                {
                    comp = item.CompareTo(child);
                    if (comp < 0)
                    {
                        current = child;
                        break;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;


namespace KNNSearch
{
    class KdTree
    {
        public Point root;
        public KdTree leftNode;
        public KdTree rightNode;
        public int spilt;
        public KdTree parent;
        public List<Point> range;

        KdTree()
        {
            leftNode = null;
            rightNode = null;
            parent = null;
            root = null;
        }

        public bool isEmpty()
        {
            return root == null;
        }

        public bool isLeaf()
        {
            return (!isEmpty()) && rightNode == null && leftNode == null;
        }

        public bool isRoot()
        {
            return (!isEmpty()) && parent == null;
        }

        public bool isLeft()
        {
            return parent.leftNode.root == root;
        }

        public bool isRight()
        {
            return parent.rightNode.root == root;
        }
    }


    class Point
    {
        double x, y, z;
    }

}
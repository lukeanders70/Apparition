using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectionTypes
{
    public class PriorityQueue<T>
    {
        private IComparer<T> comparer;
        private ArrayList list;
        public PriorityQueue(IComparer<T> c)
        {
            comparer = c;
            list = new ArrayList();
        }

        public int Count() { return list.Count; }

        public void Enqueue(T item)
        {
            list.Add(item);
            var itemIndex = list.Count - 1;
            var parent = getParent(itemIndex);
            while(parent.valid && comparer.Compare(item, parent.value) < 0)
            {
                swap(parent.index, itemIndex);

                itemIndex = parent.index;
                parent = getParent(itemIndex);
            }
        }

        public T Dequeue()
        {
            if(list.Count == 0)
            {
                return default(T);
            }

            var moveElement = (T) list[list.Count - 1];
            var firstElement = (T) list[0];
            list[0] = moveElement;
            list.RemoveAt(list.Count - 1);

            var bubbleIndex = 0;
            while(!satisfiesHeap(bubbleIndex))
            {
                var newBubbleIndex = getSmallerChildIndex(bubbleIndex);
                swap(bubbleIndex, newBubbleIndex);
                bubbleIndex = newBubbleIndex;
            }
            return firstElement;
        }

        public ArrayList getList()
        {
            return list;
        }

        public override string ToString()
        {
            if(list.Count == 0)
            {
                return "Empty PQ";
            }
            var printList = new List<List<string>>();
            var addQueue = new List<int>();
            var nextLevelAddQueue = new List<int>();
            addQueue.Add(0);
            int currentLevel = 0;
            while(addQueue.Count != 0)
            {
                var currentIndex = addQueue[0];
                addQueue.RemoveAt(0);

                if(printList.Count <= currentLevel)
                {
                    printList.Add(new List<string>());
                }
                printList[currentLevel].Add(list[currentIndex].ToString());

                var left = getLeftChild(currentIndex);
                var right = getRightChild(currentIndex);
                if(left.valid) { nextLevelAddQueue.Add(left.index); }
                if(right.valid) { nextLevelAddQueue.Add(right.index); }

                if(addQueue.Count == 0)
                {
                    currentLevel += 1;
                    addQueue = nextLevelAddQueue;
                    nextLevelAddQueue = new List<int>();
                }
            }
            var retString = "";
            foreach(T item in list)
            {
                retString += " " + item.ToString() + " ";
            }
            retString += "\n";
            foreach (List<string> row in printList)
            {
                foreach (string item in row)
                {
                    retString += " | " + item;
                }
                retString += "\n";
            }
            return retString;
        }

        private (bool valid, int index, T value) getParent(int index)
        {
            if(index == 0)
            {
                return (false, 0, (T) list[0]);
            } else
            {
                var subFactor = index % 2 == 0 ? 2 : 1;
                var i = (index - subFactor) / 2;
                return (true, i, (T) list[i]);
            }
        }

        private (bool valid, int index, T value) getLeftChild(int index)
        {
            var i = (index * 2) + 1;
            if(i < list.Count)
            {
                return (true, i, (T)list[i]);
            } else
            {
                return (false, index, (T)list[index]);
            }

        }

        private (bool valid, int index, T value) getRightChild(int index)
        {
            var i = (index * 2) + 2;
            if (i < list.Count)
            {
                return (true, i, (T)list[i]);
            }
            else
            {
                return (false, index, (T)list[index]);
            }
        }

        private void swap(int index1, int index2)
        {
            var val1 = list[index1];
            var val2 = list[index2];
            list[index1] = val2;
            list[index2] = val1;
        }

        private bool satisfiesHeap(int index)
        {
            if(index >= list.Count)
            {
                return true;
            }
            var element = (T) list[index];
            var leftChild = getLeftChild(index);
            var rightChild = getRightChild(index);

            return (!leftChild.valid || comparer.Compare(element, leftChild.value) <= 0) &&
            (!rightChild.valid || comparer.Compare(element, rightChild.value) <= 0);
        }
        private int getSmallerChildIndex(int index)
        {
            var leftChild = getLeftChild(index);
            var rightChild = getRightChild(index);

            if (!leftChild.valid) { return rightChild.index; }
            else if (!rightChild.valid) { return leftChild.index; }
            else if (comparer.Compare(leftChild.value, rightChild.value) < 0)
            {
                return leftChild.index;
            } else
            {
                return rightChild.index;
            }
        }
    }
}


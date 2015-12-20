using System;
using System.Collections.Generic;

public class QuickSort
{
	public static void Sort<T>(T[] array, Func<T, T, int> compare) {
		Sort<T>(array, 0, array.Length - 1, compare);
	}

	public static void Sort<T>(T[] array, int left, int right, Func<T, T, int> compare)
    {
        int i = left;
        int j = right;
        T pivot = array[(left + right) / 2];
        while (i <= j)
		{
			while (compare (array[i], pivot) < 0)
			{
				i ++;
            }
			while (compare (array[j], pivot) > 0)
			{
				j --;
            }
            
            if (i <= j)
            {
				var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
                i++;
                j--;
            }
            if (left < j)
				Sort(array, left, j, compare);

            if (i < right)
				Sort(array, i, right, compare);
        }
    }

	public static void SimpleSort<T>(T[] array, Func<T, T, int> compare) {
		var indexNull = -1;
		var lengthNotNull = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null && indexNull == -1)
			{
				indexNull = i;
				continue;
			}
			else if (indexNull != -1) 
			{
				var tmp = array[indexNull];
				array[indexNull] = array[i];
				array[i] = tmp;
				i = indexNull;
				indexNull = -1;
			}
			lengthNotNull = array[i] != null ? lengthNotNull + 1 : lengthNotNull;
		}
		if (compare != null)
		{
			Sort(array, 0, lengthNotNull - 1, compare);
		}
	}

//		public static void WhileSort(int[] array, Func<int, int, int> compare)
//        {
//            Queue<int> queue = new Queue<int>();
//
//            int left = 0;
//            int right = array.Length - 1;
//
//            queue.Enqueue(left);
//            queue.Enqueue(right);
//
//            while (queue.Count != 0)
//            {
//                left = queue.Dequeue();
//                right = queue.Dequeue();
//
//                int i = left;
//                int j = right;
//
//				Console.WriteLine(i + " / " + j);
//
//                int pivot = array[(i + j) / 2];
//
//				while (compare (array[i], pivot) < 0)
//                {
//                    i++;
//                }
//				while (compare (array[j], pivot) > 0)
//                {
//                    j--;
//                }
//
//                if (i <= j)
//                {
//                    int tmp = array[i];
//                    array[i] = array[j];
//                    array[j] = tmp;
//                    i++;
//                    j--;
//                }
//				if (compare (left, j) < 0) 
//                {
//                    queue.Enqueue(left);
//                    queue.Enqueue(j);
//                }
//
//				if (compare (i, right) < 0)
//                {
//                    queue.Enqueue(i);
//                    queue.Enqueue(right);
//                }
//            }
//        }

//		public static void WhileSort<T>(T[] array, Func<T, T, int> compare)
//		{
//			Queue<T> queue = new Queue<T>();
//
//			T left = array[0];
//			T right = array[array.Length - 1];
//
//			queue.Enqueue(left);
//			queue.Enqueue(right);
//
//			while (queue.Count != 0)
//			{
//				left = queue.Dequeue();
//				right = queue.Dequeue();
//
//				int i = Array.IndexOf (array, left);
//				int j = Array.IndexOf (array, right);
//
//				T pivot = array[(i + j) / 2];
//
//				while (compare (array[i], pivot) < 0)
//				{
//					i++;
//				}
//				while (compare (array[j], pivot) > 0)
//				{
//					j--;
//				}
//
//				if (i <= j)
//				{
//					var tmp = array[i];
//					array[i] = array[j];
//					array[j] = tmp;
//					i++;
//					j--;
//				}
//				if (compare (left, array[j]) < 0) 
//				{
//					queue.Enqueue(left);
//					queue.Enqueue(array[j]);
//				}
//
//				if (compare (array[i], right) < 0)
//				{
//					queue.Enqueue(array[i]);
//					queue.Enqueue(right);
//				}
//			}
//		}
}

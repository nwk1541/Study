using System;

namespace Sort
{
	class Program
	{
		static void Main(string[] args)
		{
			int[] arr1 = new int[] { 8, 5, 6, 2, 4, 1, 9, 10, 7, 3 };
			HeapSort(arr1);
			Print(arr1);
			
			int[] arr2 = new int[] { 0, 0, 2, 2, 1, 1, 4, 4, 9 };
			HeapSort(arr2);
			Print(arr2);
		}

		private static void InsertionSort(int[] data)
        {
			for(int idx = 1; idx < data.Length; idx++)
            {
				int key = data[idx];
				int secIdx = idx - 1;

				while (secIdx >= 0 && data[secIdx] > key)
                {
					data[secIdx + 1] = data[secIdx];
					secIdx--;
                }

				data[secIdx + 1] = key;
            }
        }

		private static void HeapSort(int[] data)
        {
			int length = data.Length;
			for (int idx = length / 2 - 1; idx >= 0; idx--)
				Heapify(data, idx, length);

			for (int idx = length - 1; idx >= 0; idx--)
            {
				Swap(data, idx, 0);
				Heapify(data, 0, idx);
            }
        }

		private static void Heapify(int[] data, int idx, int len)
        {
			int pivot = idx;
			int left = idx * 2 + 1;
			int right = idx * 2 + 2;

			if (left < len && data[pivot] < data[left])
				pivot = left;

			if (right < len && data[pivot] < data[right])
				pivot = right;

			if(idx != pivot)
            {
				Swap(data, idx, pivot);
				Heapify(data, pivot, len);
            }
        }

		private static void Swap(int[] data, int left, int right)
        {
			int temp = data[left];
			data[left] = data[right];
			data[right] = temp;
        }

		private static void QuickSort(int[] data)
        {

        }

		private static void Print(int[] data)
        {
			for (int idx = 0; idx < data.Length; idx++)
				Console.Write("{0} ", data[idx]);

			Console.WriteLine();
        }

		private static void IntroSort(int[] data)
        {
			int length = data.Length;
			// int maxDepth = (int)Math.Floor(Math.Log2(length)) * 2;

			// case 1
			// insertion sort
			// heap sort

			// case 2
			// insertion sort
			// heap sort
			// quick sort
        }
	}
}

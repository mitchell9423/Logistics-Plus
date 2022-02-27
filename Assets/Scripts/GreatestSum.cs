using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

public class Memoizer
{
    public List<int> numbers;
    public List<int> combination;

    public Memoizer(List<int> _numbers, List<int> _combination)
	{
        numbers = _numbers;
        combination = _combination;
	}
}

public class Graph
{
    private List<int> values;
    private int totalVertices;
    private LinkedList<int>[] adjacencyList;
    public LinkedList<int>[] AdjacencyList { get; }
    public int Size { get { return totalVertices; } }
    public Graph(List<int> _values)
	{
        values = _values;
        int n = _values.Count;
        totalVertices = n;
        adjacencyList = new LinkedList<int>[n];

		for (int i = 0; i < n; i++)
		{
            adjacencyList[i] = new LinkedList<int>();
		}

        AddEdges();
    }

    public void AddEdges()
    {
        for (int i = 0; i < totalVertices; i++)
        {
            for (int j = 0; j < totalVertices; j++)
            {
                if (j != i) adjacencyList[i].AddLast(j);
            }
        }
	}

    public string PrintAdjacencyList()
	{
        string nodeString = "";
		for (int i = 0; i < adjacencyList.Length; i++)
		{
            if (adjacencyList[i].Count > 0)
			{
            nodeString += $"[Node Value: {values[i]} with Neighbors";
			foreach (var item in adjacencyList[i])
			{
                nodeString += $" -> {values[item]}";
			}
            nodeString += $" ]\n";
			}
		}

        return nodeString;
	}
}

public class GreatestSum
{
    // Memoizer
    public static List<Memoizer> memoizer = new List<Memoizer>();
    private static int target;
    private static List<int> list;
    public static int bestSum = 0;
    public static List<int> bestPath = new List<int>();
    /// <summary>
    /// Find the combination from an array of positive integers with the sum closest to the target sum without exceeding it.
    /// If two combination are equally close, return the longest combination.
    /// </summary>
    /// <param name="targetSum">A positive integer.</param>
    /// <param name="numbers">An list of positive integers.</param>
    /// <returns>An integer list containing the best combination.</returns>
    public static List<int> BestSum(List<int> numbers, int targetSum)
    {
        target = targetSum;
        list = numbers;
        bestSum = 0;
        bestPath = new List<int>();
        ComputeTree(bestPath, 0);
        return bestPath;
    }

    public static void ComputeTree(List<int> path, int index = 0)
    {
        path = new List<int>(path);
        int sum = path.Sum();

        if (index >= list.Count || sum > target) return;

        if (sum > bestSum || (sum == bestSum && path.Count > bestPath.Count))
        {
            bestSum = sum;
            bestPath = new List<int>(path);
        }

        ComputeTree(path, index + 1);
        
        path.Add(list[index]);
        sum = path.Sum();

        if (sum > target) return;

        if (sum > bestSum || (sum == bestSum && path.Count > bestPath.Count))
        {
            bestSum = sum;
            bestPath = new List<int>(path);
        }

        ComputeTree(path, index + 1);
    }

    public static List<int> Compute(int targetSum, List<int> numbers)
    {
        // base case
        // because the fuction recursively removes numbers, if the sum of the numbers are less than or
        // eqaul to the target sum there is no point going any further.
        if (numbers.Sum() <= targetSum) return numbers;

		// Check memoizer to see if the number combination has already been calculated.
		if (memoizer.Exists((memo) => memo.numbers.SequenceEqual(numbers))) return memoizer.Find((memo) => memo.numbers.SequenceEqual(numbers)).combination;

		//bool sequenceExists = false;

		//for (int i = 0; i < memoizer.Count; i++)
		//{
		//    if (memoizer[i].numbers.Count == numbers.Count)
		//    {
		//        sequenceExists = true;

		//        for (int j = 0; j < memoizer[i].numbers.Count; j++)
		//        {
		//            if (memoizer[i].numbers[j] != numbers[j])
		//            {
		//                sequenceExists = false;
		//                break;
		//            }
		//        }

		//        if (sequenceExists) return memoizer[i].combination;
		//    }
		//}


		int bestSum = 0;
        List<int> bestCombination = new List<int>();

        for (int i = 0; i < numbers.Count; i++)
        {
            // Remove only the current number and recursively compute the remaining numbers from the number list.
            List<int> remainingNumbers = new List<int>(numbers);
            remainingNumbers.Remove(numbers[i]);

            List<int> combination = Compute(targetSum, remainingNumbers);

            int remainingSum = combination.Sum();

            // Test the result and assign as best combination if true.
            if (remainingSum > bestSum
                || (remainingSum == bestSum && combination.Count > bestCombination.Count))
            {
                bestSum = remainingSum;
                bestCombination = combination;
            }
        }

        // Add computed number list to memoizer for optimization.
        memoizer.Add(new Memoizer(numbers, bestCombination));
        return bestCombination;
    }
}

using System.Collections.Generic;
using System.Linq;

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

public class GreatestSum
{
    // Memoizer
    public static List<Memoizer> memoizer = new List<Memoizer>();
    
    /// <summary>
    /// Find the combination from an array of positive integers with the sum closest to the target sum without exceeding it.
    /// If two combination are equally close, return the longest combination.
    /// </summary>
    /// <param name="targetSum">A positive integer.</param>
    /// <param name="numbers">An list of positive integers.</param>
    /// <returns>An integer list containing the best combination.</returns>
    public static List<int> BestSum(int targetSum, List<int> numbers)
    {
        memoizer.Clear();
        return Compute(targetSum, numbers);
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

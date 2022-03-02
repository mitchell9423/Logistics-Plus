using System.Collections.Generic;
using System.Linq;

public class GreatestSum
{
    // Memoizer
    public static List<Dictionary<int, int>> memoizer = new List<Dictionary<int, int>>();

    private static int targetSum;
    private static List<int> numberList;

    public static int bestSum = 0;
    public static List<int> bestCombination = new List<int>();

    /// <summary>
    /// Find the combination from an array of positive integers with the sum closest to the target sum without exceeding it.
    /// If two combination are equally close, return the longest combination.
    /// </summary>
    /// <param name="_targetSum">An integer.</param>
    /// <param name="_numbersList">A list of integers.</param>
    /// <returns>An integer list containing the best subset combination.</returns>
    public static List<int> BestSum(List<int> _numbersList, int _targetSum)
    {
        targetSum = _targetSum;
        numberList = _numbersList;

        memoizer.Clear();
        numberList.ForEach((item) => memoizer.Add(new Dictionary<int, int>()));

        bestSum = 0;
        bestCombination.Clear();

        CumputeSumComination(bestCombination);
        return bestCombination;
    }

    private static void CumputeSumComination(List<int> combination, int index = 0)
    {
        int sum = combination.Sum();

        // exclusive base case check
        if (IsBaseCase(index, sum)) return;

        // check memoizer for previously computed exclusive sums for the current index
        if (!isComputed(index, sum, combination.Count))
        {
            // check if current sum and/combination is better and save if true
            if (IsBestCombination(sum, combination))
            {
                CumputeSumComination(bestCombination, index + 1);
            }
            else
            {
                CumputeSumComination(combination, index + 1);
            }
        }
        
        sum += numberList[index];

        // inclusive base case check
        if (IsBaseCase(index, sum)) return;

        // check memoizer for previously computed inclusive sums for the current index
        if (!isComputed(index, sum, combination.Count + 1))
        {
            // check if current sum and/combination is better and save if true
            if (IsBestCombination(sum, combination, numberList[index]))
            {
                CumputeSumComination(bestCombination, index + 1);
            }
            else
            {
                combination = new List<int>(combination);
                combination.Add(numberList[index]);
                CumputeSumComination(combination, index + 1);
            }
        }
    }

    #region Helper Functions
    // Helper Functions

    private static bool IsBaseCase(int index, int sum) => index >= numberList.Count || sum > targetSum;

    private static bool isComputed(int index, int sum, int combinationLength)
    {
        bool hasKey = memoizer[index].ContainsKey(sum);
        bool isLonger = hasKey && memoizer[index][sum] >= combinationLength;

        if (!hasKey)
        {
            memoizer[index].Add(sum, combinationLength);
        }
        else if (!isLonger)
        {
            memoizer[index][sum] = combinationLength;
        }

        return hasKey && isLonger;
    }

    private static bool IsBestCombination(int sum, List<int> combination, int includeNumber = 0)
    {
        bool isBiggerSum = sum > bestSum;
        bool isLongerCombination = sum == bestSum && combination.Count > bestCombination.Count;
        bool isBest = isBiggerSum || isLongerCombination;

        if (isBest)
        {
            bestSum = sum;
            bestCombination = new List<int>(combination);
            if (includeNumber > 0) bestCombination.Add(includeNumber);
        }

        return isBest;
    }
    #endregion
}

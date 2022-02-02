using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatestSum
{
    public static int[] BestSum(int targetSum, int[] numbers)
    {
        if (targetSum <= 0) return new int[0];

        int smallestRemainder = targetSum;
        int[] bestCombination = new int[0];

        for (int i = 0; i < numbers.Length; i++)
        {
            var remainder = targetSum - numbers[i];

            int[] remainingNumbers = new int[numbers.Length - 1];
            for (int j = 0, k = 0; j < numbers.Length && k < remainingNumbers.Length; j++)
            {
                if (j != i)
                {
                    remainingNumbers[k] = numbers[j];
                    k++;
                }
            }

            int[] remainderCombination = BestSum(remainder, remainingNumbers);
            int[] combination = new int[remainderCombination.Length + 1];
            remainderCombination.CopyTo(combination, 0);
            combination[combination.Length - 1] = numbers[i];

            if (remainder >= 0)
			{
                if (remainder < smallestRemainder || (remainder == smallestRemainder && combination.Length > bestCombination.Length))
                {
                    bestCombination = combination;
                    smallestRemainder = remainder;
                }
            }
        }

        return bestCombination;
    }
}

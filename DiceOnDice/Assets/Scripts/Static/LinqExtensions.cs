using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class LinqExtensions
{
    /*
     *  Determines if a Vector2Int coordinate is a valid location within a given grid
     *  EX: LinqExtensions.IsCoordinateValid( grid, new Vector2Int(8,8) )
     */
    public static bool IsCoordinateValid<T>(this T[,] grid, Vector2Int coordinate)
    {
        if (coordinate.x < 0 || coordinate.x >= grid.GetLength(0) ||
            coordinate.y < 0 || coordinate.y >= grid.GetLength(1))
        {
            return false;
        }

        return true;
    }

    /*
     *  Returns a list of neighbors for a given coordinate in a grid.
     *  If it's boolean is toggled, it will also return diagonal neighbors
     *  EX: LinqExtensions.GetNeighbors( grid, new Vector2Int(1,1), false )
     */
    public static IList<T> GetNeighbors<T>(this T[,] grid, Vector2Int coordinate, bool getDiagonalNeighbors)
    {
        List<T> neighborList = new List<T>();

        if (!IsCoordinateValid(grid, coordinate)) Debug.LogError("Cannot get neighbors of an invalid coordinate.");

        if (IsCoordinateValid(grid, new Vector2Int(coordinate.x - 1, coordinate.y)))
            neighborList.Add(grid[coordinate.x - 1, coordinate.y]);

        if (IsCoordinateValid(grid, new Vector2Int(coordinate.x + 1, coordinate.y)))
            neighborList.Add(grid[coordinate.x + 1, coordinate.y]);

        if (IsCoordinateValid(grid, new Vector2Int(coordinate.x, coordinate.y - 1)))
            neighborList.Add(grid[coordinate.x, coordinate.y - 1]);

        if (IsCoordinateValid(grid, new Vector2Int(coordinate.x, coordinate.y + 1)))
            neighborList.Add(grid[coordinate.x, coordinate.y + 1]);

        if (getDiagonalNeighbors)
        {
            if (IsCoordinateValid(grid, new Vector2Int(coordinate.x - 1, coordinate.y - 1)))
                neighborList.Add(grid[coordinate.x - 1, coordinate.y - 1]);

            if (IsCoordinateValid(grid, new Vector2Int(coordinate.x + 1, coordinate.y - 1)))
                neighborList.Add(grid[coordinate.x + 1, coordinate.y - 1]);

            if (IsCoordinateValid(grid, new Vector2Int(coordinate.x - 1, coordinate.y + 1)))
                neighborList.Add(grid[coordinate.x - 1, coordinate.y + 1]);

            if (IsCoordinateValid(grid, new Vector2Int(coordinate.x + 1, coordinate.y + 1)))
                neighborList.Add(grid[coordinate.x + 1, coordinate.y + 1]);
        }

        return neighborList;
    }

    /*
     *  Checks if a given grid contains a specified element
     *  EX: LinqExtensions.Includes( grid, 2 )
     */
    public static bool Includes<T>(this T[,] grid, T element)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y].Equals(element))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /*
     *  Returns a slice of an array from a start index up until an end index
     *  EX: LinqExtensions.Slice( list.ToArray(), 1, 3 );
     */
    public static T[] Slice<T>(this T[] source, int start, int end = 0)
    {
        if (end <= 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        T[] results = new T[len];
        for (int i = 0; i < len; i++)
        {
            results[i] = source[i + start];
        }

        return results;
    }

    /*
     *  Maps a value within a new range
     *  EX: LinqExtensions.Map(1, 0, 2, 10, 20); ==> should equal 15
     */
    public static float Map(float value, float currentMin, float currentMax, float newMin, float newMax)
    {
        return (value - currentMin) / (currentMax - currentMin) * (newMax - newMin) + newMin;
    }

    private static System.Random RNG = new System.Random();

    /*
     *  Shuffles a given list of items to randomize its order.
     *  Note that it doesn't return the list, it simply shuffles it
     *  EX: LinqExtensions.Shuffle( list )
     */
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RNG.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /*
     *  Shuffles a given list of items and returns it
     *  EX: LinqExtensions.ShuffleAndReturn( list )
     */
    public static IList<T> ShuffleAndReturn<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RNG.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    /*
     *  Returns a random element from a list
     *  EX: LinqExtensions.GetRandomElement( list );
     */
    public static T GetRandomElement<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogError("Cannot get a random element from an empty list.");
        }
        return list[RNG.Next(list.Count)];
    }

    /*
     *  Returns a random element from a list and also removes it from the list
     *  EX: LinqExtensions.GetAndRemoveRandomElement( list );
     */
    public static T GetAndRemoveRandomElement<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogError("Cannot get and remove random element from an empty list.");
        }
        int randomIndex = RNG.Next(list.Count);
        T itemAtIndex = list[randomIndex];
        list.RemoveAt(randomIndex);
        return itemAtIndex;
    }

    /*
     *  Returns a random element within a range of indices in a list
     *  EX: LinqExtensions.GetRandomElementInRange( list, 0, 2 );
     */
    public static T GetRandomElementInRange<T>(this IList<T> list, int min, int max)
    {
        if (min < 0)
        {
            min = 0;
        }
        if (max > list.Count)
        {
            max = list.Count;
        }

        return list[RNG.Next(min, max)];
    }

    /*
     *  Returns a random element within a range of indices in a list
     *  EX: LinqExtensions.GetRandomElementInRange( list, 0, 2 );
     */
    public static IList<T> GetRandomElements<T>(this IList<T> list, int count)
    {
        if (list.Count < count)
        {
            return list.ToList();
        }

        HashSet<T> returnHash = new HashSet<T>();

        while (returnHash.Count < count)
        {
            T newItem = list.GetRandomElement();
            while (returnHash.Contains(newItem))
            {
                newItem = list.GetRandomElement();
            }

            returnHash.Add(newItem);
        }

        return returnHash.ToList();
    }

    /*
     *  Returns a specificed number of random elements within a range of indices in a list
     *  EX: LinqExtensions.GetRandomElementsInRange( list, 0, 5, 2 );
     */
    public static IList<T> GetRandomElementsInRange<T>(this IList<T> list, int min, int max, int count)
    {
        if (count > max + 1 - min)
        {
            Debug.LogError("Not enough indices to get random elements");
            return null;
        }

        T[] slicedArray = Slice(list.ToArray<T>(), min, max+1);

        return GetAndRemoveRandomElements(slicedArray.ToList(), count); ;
    }

    /*
     *  Returns a specified number of random elements from a list and also removes them from the list
     *  EX: LinqExtensions.GetAndRemoveRandomElements( list, 2 );
     */
    public static IList<T> GetAndRemoveRandomElements<T>(this IList<T> list, int count)
    {
        List<T> returnList = new List<T>();

        while (returnList.Count < count && list.Count > 0)
        {
            returnList.Add(list.GetAndRemoveRandomElement());
        }

        return returnList;
    }
}

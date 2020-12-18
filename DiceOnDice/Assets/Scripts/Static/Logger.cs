using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Logger
{
    // This bool toggles all of the log messages, so if it's false nothing will log
    public static bool DEBUG_ON;

    /*
     *  Equivalent to Debug.Log() - Prints out a given paramenter
     *  EX: Logger.Log( thing );
     */
    public static void Log<T>(T item)
    {
        if (DEBUG_ON)
        {
            Debug.Log(item.ToString());
        }
    }

    /*
     *  Equivalent to Debug.Log() - Prints out a given paramenter
     *  EX: Logger.Log( thing );
     */
    public static void LogError<T>(T item)
    {
        if (DEBUG_ON)
        {
            Debug.LogError(item);
        }
    }

    /*
     *  Prints out each item in a generic list on a new line.
     *  EX: Logger.List( listOfThings );
     */
    public static void List<T>(this IList<T> list)
    {
        if (DEBUG_ON)
        {
            Type itemType = typeof(T);

            foreach (T item in list)
            {
                if (itemType == typeof(GameObject))
                {
                    Debug.Log(item.ToString());
                }
                else
                {
                    Debug.Log(item);
                }
            }
        }
    }

    /*
     *  Prints out values so that they're comma-separated in the console
     *  EX: Logger.MultipleItems( new int[] { 1, 2, 3 } );
     */
    public static void MultipleItems<T>(T[] list)
    {
        if (DEBUG_ON)
        {
            string returnString = "";

            for (int i = 0; i < list.Length; i++)
            {
                returnString += list[i];
                if (i != list.Length - 1) returnString += ", ";
            }

            Debug.Log(returnString);
        }
    }

    /*
     *  Prints out the values of two items to compare as well as their equivalence
     *  EX: Logger.Assert( list1.Count, list2.Count );
     */
    public static void Assert<T>(T item1, T item2)
    {
        if (DEBUG_ON)
        {
            Debug.Log("Item 1: " + item1 + " and " + "Item 2: " + item2 + ": " + item1.Equals(item2));
        }
    }
}

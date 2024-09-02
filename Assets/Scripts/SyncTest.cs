using UnityEngine;
using System.Collections.Generic;

public class SyncTest : MonoBehaviour 
{

    public void SyncData()
    {
        string[] array1 = { "apple", "banana", "cherry", "date" };
        string[] array2 = { "apple", "banana",  "date", "cherry", "Strawberry" };

        MerkleTree tree1 = new MerkleTree(array1);
        MerkleTree tree2 = new MerkleTree(array2);

        Debug.Log("Root hash of tree1: " + tree1.GetRoot());
        Debug.Log("Root hash of tree2: " + tree2.GetRoot());

        List<string> differences = tree1.GetDifferences(tree2);
        Debug.Log("Differences: " + string.Join(", ", differences));

    }
    
}

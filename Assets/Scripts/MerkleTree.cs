using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class MerkleTree
{
    private List<string> leaves;
    private List<string> nodes;
    private string rootHash;

    public MerkleTree(string[] data)
    {
        leaves = new List<string>(data);
        BuildTree();
    }

    private void BuildTree()
    {
        nodes = new List<string>(leaves.Select(ComputeHash));
        while (nodes.Count > 1)
        {
            List<string> parents = new List<string>();
            for (int i = 0; i < nodes.Count; i += 2)
            {
                if (i + 1 < nodes.Count)
                {
                    parents.Add(HashPair(nodes[i], nodes[i + 1]));
                }
                else
                {
                    parents.Add(nodes[i]);
                }
            }
            nodes = parents;
        }
        rootHash = nodes[0];
    }

    public static string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }

    private string HashPair(string left, string right)
    {
        return ComputeHash(left + right);
    }

    public string GetRoot()
    {
        return nodes[0];
    }

    public List<string> GetDifferences(MerkleTree other)
    {
         if (this.rootHash == other.rootHash)
        {
            return new List<string>(); // No differences if root hashes match
        }
        HashSet<string> thisSet = new HashSet<string>(this.leaves);
        HashSet<string> otherSet = new HashSet<string>(other.leaves);

        List<string> differences = new List<string>();
        differences.AddRange(thisSet.Except(otherSet));
        differences.AddRange(otherSet.Except(thisSet));

        return differences;
    }
}
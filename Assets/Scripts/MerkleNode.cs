using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MerkleNode
{
    public string Hash { get; private set; }
    public MerkleNode Left { get; private set; }
    public MerkleNode Right { get; private set; }
    public string Data { get; private set; }

    public MerkleNode(string data)
    {
        Data = data;
        Hash = MerkleTree.ComputeHash(data);
    }

    public MerkleNode(MerkleNode left, MerkleNode right)
    {
        Left = left;
        Right = right;
        Hash = MerkleTree.ComputeHash(left.Hash + right.Hash);
    }

    public override string ToString()
    {
        return $"MerkleNode {{ Data: {Data}, Hash: {Hash} }}";
    }
    
}

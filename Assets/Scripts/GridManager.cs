using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
   [Header("Grid Properties")] 
   [SerializeField] private int _width;
   [SerializeField] private int _height;
   [SerializeField] private float _verticalOffset;
   [SerializeField] private float _horizontalOffset;

   [Header("Start Point")]
   [SerializeField] private int _xPoint;
   [SerializeField] private int _yPoint;
   
   [Header("Components")] 
   [SerializeField] private Tile _tilePref;

   private void Start()
   {
       GenerateGrid();
   }

   private void GenerateGrid()
   {
       for (int x = _xPoint ; x < _width + _xPoint; x++)
       {
           for (int y = _yPoint; y < _height + _yPoint; y++)
           {
               var spawnedTile = Instantiate(_tilePref, new Vector3(x - _horizontalOffset, y - _verticalOffset), Quaternion.identity);
               spawnedTile.name = $"Tile({x}:{y})";
           }
       }
        
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
   [SerializeField]private List<int> testList;

   private void Awake()
   {
      testList = new List<int>();
   }

   private void Start()
   {
      var tl = G();
      tl.Add(1);
   }

   public List<int> G()
   {
      return testList;
   }
}

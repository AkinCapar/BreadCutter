using System.Collections;
using System.Collections.Generic;
using System.Text;
using BreadCutter.Views;
using UnityEngine;

namespace BreadCutter.Models
{
    public class BreadModel
    {
        private bool _isBreadSlotsAreFull;
        public bool IsBreadSlotsAreFull => _isBreadSlotsAreFull;
        public List<List<BreadView>> breadLines = new List<List<BreadView>>();
        

        public void BreadLineSpawned(List<BreadView> list, int slotAmount, int lineIndex)
        {
            if (breadLines.Count == 0)
            {
                for (int i = 0; i < slotAmount; i++)
                {
                    breadLines.Add(new List<BreadView>());
                }
            }
            breadLines[lineIndex].AddRange(list);
        }

        public int GetNextBreadLineIndexToSpawn(int slotAmount)
        {
            if (breadLines.Count >= slotAmount)
            {
                for (int i = 0; i < slotAmount; i++)
                {
                    if (breadLines[i].Count == 0)
                    {
                        return i;
                    }
                }
            }
            
            return breadLines.Count;
        }

        public void EmptyBreadSlotCheck(int slotAmount)
        {
            if (breadLines.Count >= slotAmount)
            {
                for (int i = 0; i < slotAmount; i++)
                {
                    if (breadLines[i].Count == 0)
                    {
                        _isBreadSlotsAreFull = false;
                        return;
                    }
                }
                
                _isBreadSlotsAreFull = true;
            }
            
            else
            {
                _isBreadSlotsAreFull = false;
            }
        }

        public void SingleBreadSpawned(int lineIndex, BreadView bread)
        {
            breadLines[lineIndex].Remove(breadLines[lineIndex][0]);
            breadLines[lineIndex].Add(bread);
        }

        public BreadView GetLastBreadInLine(int lineIndex)
        {
            return breadLines[lineIndex][breadLines[lineIndex].Count - 1];
        }
    }
}

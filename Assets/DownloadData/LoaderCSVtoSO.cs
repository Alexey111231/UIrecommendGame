using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorUtilites
{
    public class LoaderCSVtoSO
    {
        public string _casePath;

        private Dictionary<string, string> _cuts;

        public LoaderCSVtoSO(string casePath)
        {
            _casePath = casePath;
        }

        public ResultScore ParseAsPrizeValue(string CSVFile)
        {
            var result = new ResultScore();
            var grid = GetCSVGrid(CSVFile);

            result.PrizeMinValue = Convert.ToInt32(grid[0, 1]);
            result.MinScore = Convert.ToInt32(grid[0, 3]);
            result.MaxScore = Convert.ToInt32(grid[1, 3]);

            return result;
        }

        public List<Selector> ParseAsSelectors(string CSVFile)
        {
            var result = new List<Selector>();
            var grid = GetCSVGrid(CSVFile);

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (j is 0 or 1) continue;
                var selector = new Selector(
                    grid[0, j],
                    grid[1, j],
                    grid[2, j],
                    grid[3, j],
                    grid[4, j],
                    grid[5, j],
                    grid[6, j],
                    grid[7, j],
                    grid[8, j]);

                result.Add(selector);
            }
            return result;
        }

        public List<Feature> ParseAsFeatures(string CSVFile)
        {
            var result = new List<Feature>();
            var grid = GetCSVGrid(CSVFile);

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (j is 0 or 1) continue;
                var selector = new Feature(
                    grid[0, j],
                    grid[1, j],
                    grid[2, j],
                    grid[3, j],
                    grid[4, j],
                    grid[5, j],
                    grid[6, j],
                    grid[7, j],
                    grid[8, j]);

                result.Add(selector);
            }
            return result;
        }

        public List<CombinationSelectors> ParseAsCombinationSelectors(string CSVFile)
        {
            var result = new List<CombinationSelectors>();
            var grid = GetCSVGrid(CSVFile);

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (j is 0 or 1) continue;

                var idList = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    if (grid[i, j] != "") idList.Add(grid[i, j]);
                }

                var combination = new CombinationSelectors(
                    idList,
                    grid[3, j],
                    grid[4, j],
                    grid[5, j],
                    grid[6, j],
                    grid[7, j]);

                result.Add(combination);
            }
            return result;
        }

        public List<CombinationSelectorAndFeature> ParseAsCombinationSelectorAndFeature(string CSVFile)
        {
            var result = new List<CombinationSelectorAndFeature>();
            var grid = GetCSVGrid(CSVFile);

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (j is 0 or 1) continue;

                var condition = new SelectorFeatureCombination();
                condition.SelectorId = grid[0, j];
                condition.FeatureId = grid[1, j];

                var combination = new CombinationSelectorAndFeature(
                    condition,
                    grid[3, j],
                    grid[4, j],
                    grid[5, j],
                    grid[6, j],
                    grid[7, j]);

                result.Add(combination);
            }
            return result;
        }

        public string[,] GetCSVGrid(string csvText)
        {
            //first is column
            string[] dataItems = csvText.Split("\r\n");

            int totalColumns = 0;
            for (int i = 0; i < dataItems.Length; i++)
            {
                string[] row = dataItems[i].Split('\t');
                totalColumns = Mathf.Max(totalColumns, row.Length);
            }

            string[,] outputGrid = new string[totalColumns, dataItems.Length];
            for (int y = 0; y < dataItems.Length; y++)
            {
                string[] row = dataItems[y].Split('\t');
                for (int x = 0; x < row.Length; x++)
                {
                    outputGrid[x, y] = row[x];
                }
            }
            return outputGrid;
        }

        public void SaveSO(ScriptableObject so, string fileName)
        {
#if UNITY_EDITOR
            var address = $"{_casePath}/{fileName}.asset";
            UnityEditor.AssetDatabase.DeleteAsset(address);
            UnityEditor.AssetDatabase.CreateAsset(so, address);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
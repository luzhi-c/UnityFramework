using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Test
{
    public class TestGame : MonoBehaviour
    {
        public GameObject prefab;
        public TestItem[,] items;

        private int col = 11;
        private int row = 10;

        private float gridSize = 50;

        // Start is called before the first frame update

        public bool[,] solution;
        void Start()
        {
            items = new TestItem[col, row];
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    var item = Instantiate(prefab);
                    var testItem = item.GetComponent<TestItem>();
                    item.transform.SetParent(gameObject.transform, false);
                    item.transform.localPosition = new Vector3(-(float)col / 2 * gridSize + i * gridSize, (float)row / 2 * gridSize - j * gridSize);
                    items[i, j] = testItem;
                    testItem.Init(i, j, UnityEngine.Random.Range(0, 1f) > 0.5f);
                    testItem.OnTouch += OnClick;
                }
            }
            StartCoroutine(nameof(Task));
        }


        IEnumerator Task()
        {
            yield return new WaitForSeconds(1f);
            bool[,] puzzle = new bool[col, row];
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    puzzle[i, j] = !items[i, j].isTouch;
                }
            }
            if (Solve(puzzle, out solution))
            {
                Debug.Log("有解");
                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (solution[i, j])
                        {
                            OnClick(i, j);
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("无解");
            }

        }

        public void OnClick(int col, int row)
        {
            var self = new int[2] { col, row };
            var top = new int[2] { col, row + 1 };
            var bottom = new int[2] { col, row - 1 };
            var left = new int[2] { col - 1, row };
            var right = new int[2] { col + 1, row };

            var arr = new int[5][] { self, top, bottom, left, right };
            for (int i = 0; i < arr.Length; i++)
            {
                var item = arr[i];
                if (item[0] < 0 || item[0] > this.col - 1 || item[1] < 0 || item[1] > this.row - 1)
                {
                    continue;
                }
                items[item[0], item[1]].SetTouch();
            }
        }

        void ClickAll()
        {
            var col = items.GetLength(0);
            var row = items.GetLength(1);
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    OnClick(i, j);
                }
            }
        }

        // Update is called once per frame

        void OnGUI()
        {
            if (GUILayout.Button("测试"))
            {
                ClickAll();
            }
        }

        /// <summary>
        /// 这个算法是有问题的 只能算M*N的方形矩阵
        /// </summary>
        /// <param name="puzzle"></param>
        /// <param name="solution"></param>
        /// <returns></returns>
        bool Solve(bool[,] puzzle, out bool[,] solution)
        {
            int n = puzzle.GetLength(0);
            int m = puzzle.GetLength(1);
            int total = n * m;

            // 构建增广矩阵
            bool[,] matrix = new bool[total, total + 1];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    int index = i * m + j;
                    matrix[index, total] = puzzle[i, j];

                    // 自身和上下左右
                    matrix[index, index] = true;
                    if (i > 0) matrix[index, (i - 1) * m + j] = true;
                    if (i < n - 1) matrix[index, (i + 1) * m + j] = true;
                    if (j > 0) matrix[index, i * m + (j - 1)] = true;
                    if (j < m - 1) matrix[index, i * m + (j + 1)] = true;
                }

            // 高斯消元（模2）
            bool[] result = new bool[total];
            for (int i = 0; i < total; i++)
            {
                int pivot = -1;
                for (int j = i; j < total; j++)
                    if (matrix[j, i]) { pivot = j; break; }

                if (pivot == -1) continue;

                // 交换行
                for (int k = i; k <= total; k++)
                    (matrix[i, k], matrix[pivot, k]) = (matrix[pivot, k], matrix[i, k]);

                // 消元
                for (int j = i + 1; j < total; j++)
                    if (matrix[j, i])
                        for (int k = i; k <= total; k++)
                            matrix[j, k] ^= matrix[i, k];
            }

            // 回代
            for (int i = total - 1; i >= 0; i--)
            {
                if (!matrix[i, i])
                {
                    bool hasSolution = !matrix[i, total];
                    if (!hasSolution) { solution = null; return false; }
                    result[i] = false;
                    continue;
                }

                result[i] = matrix[i, total];
                for (int j = i + 1; j < total; j++)
                    if (matrix[i, j])
                        result[i] ^= result[j];
            }

            // 转换为二维解
            solution = new bool[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    solution[i, j] = result[i * m + j];

            return true;
        }

    }


}

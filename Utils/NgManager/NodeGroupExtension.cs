using DirN.Utils.NgManager.Curves;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.NgManager
{
    public static class NodeGroupExtension
    {
        [Obsolete("This method is deprecated, use HaveLoopEdgeEx instead")]
        public static bool HaveLoopEdge(this NodeGroup Nodes, out IList<ICurve> errorCurves)
        {
            // ToDo: Implement loop detection   
            errorCurves = [];
            if (Nodes.Count == 0) return false;
            HashSet<INode> visitedNodes = [];
            HashSet<INode> visitedNodesOnOneScan = [];
            Queue<INode> queue = new();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (visitedNodes.Contains(Nodes[i])) continue;
                visitedNodesOnOneScan.Clear();
                queue.Enqueue(Nodes[i]);
                while (queue.Count > 0)
                {
                    INode node = queue.Dequeue();
                    visitedNodes.Add(Nodes[i]);
                    visitedNodesOnOneScan.Add(node);
                    foreach (var next in node.Output)
                    {
                        if (next.EndNode == null) continue;
                        if (visitedNodesOnOneScan.Contains(next.EndNode))
                        {
                            errorCurves.Add(next);
                            continue;
                        }
                        if (visitedNodes.Contains(next.EndNode)) continue;
                        queue.Enqueue(next.EndNode);
                    }
                }
            }
            return errorCurves.Count > 0;
        }

        [Obsolete("This method is deprecated, use GetExecutionOrderEx instead")]
        public static IList<INode> GetExecutionOrder(this NodeGroup Nodes)
        {
            // ToDo: Implement execution order
            IList<INode> executionOrder = [];
            Stack<INode> predecessors = [];
            HashSet<INode> visitedNodes = [];
            Stack<INode> nodesToVisit = new();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (visitedNodes.Contains(Nodes[i])) continue;
                nodesToVisit.Push(Nodes[i]);
                while (nodesToVisit.Count > 0)
                {
                    INode top = nodesToVisit.Pop();
                    predecessors.Push(top);
                    foreach (var next in top.OutputNodes)
                    {
                        if (visitedNodes.Contains(next)) continue;
                        nodesToVisit.Push(next);
                    }
                    while (predecessors.Count > 0)
                    {
                        INode node = predecessors.Peek();
                        if (visitedNodes.Contains(node))
                        {
                            predecessors.Pop();
                            continue;
                        }
                        IList<INode> predecessorsOfNode = node.InputNodes;
                        if (predecessorsOfNode.Count == 0)
                        {
                            executionOrder.Add(node);
                            visitedNodes.Add(node);
                            predecessors.Pop();
                            continue;
                        }
                        int cnt = 0;
                        foreach (var predecessor in predecessorsOfNode)
                        {
                            if (visitedNodes.Contains(predecessor))
                            {
                                cnt++;
                                continue;
                            }
                            predecessors.Push(predecessor);
                        }
                        if (cnt == predecessorsOfNode.Count)
                        {
                            executionOrder.Add(node);
                            visitedNodes.Add(node);
                            predecessors.Pop();
                            continue;
                        }
                    }
                }
            }
            return executionOrder;
        }
        
        public static bool HaveLoopEdgeEx(this NodeGroup nodes, out IList<ICurve> errorCurves)
        {
            errorCurves = [];
            if (nodes == null || nodes.Count == 0) return false;

            // 节点状态字典：0=未访问, 1=访问中, 2=已访问
            var state = new Dictionary<INode, int>();
            var stack = new Stack<INode>();
            var pathEdges = new Dictionary<INode, ICurve>(); // 记录路径中的边

            foreach (var node in nodes)
            {
                if (state.TryGetValue(node, out int value) && value != 0)
                    continue;

                stack.Push(node);
                state[node] = 1; // 标记为访问中

                while (stack.Count > 0)
                {
                    var current = stack.Peek();
                    bool hasUnvisitedChild = false;

                    foreach (var edge in current.Output)
                    {
                        var nextNode = edge.EndNode;
                        if (nextNode == null) continue;

                        // 发现回边（环）
                        if (state.TryGetValue(nextNode, out int nextState) && nextState == 1)
                        {
                            errorCurves.Add(edge);
                            // 回溯完整环路径（可选）
                            // errorCurves.AddRange(BacktrackCycleEdges(current, nextNode, pathEdges));
                            return true; // 发现环立即返回
                        }

                        if (!state.ContainsKey(nextNode) || state[nextNode] == 0)
                        {
                            pathEdges[nextNode] = edge; // 记录边
                            state[nextNode] = 1; // 标记为访问中
                            stack.Push(nextNode);
                            hasUnvisitedChild = true;
                            break; // 深度优先
                        }
                    }

                    if (!hasUnvisitedChild)
                    {
                        state[current] = 2; // 标记为已访问
                        stack.Pop();
                        pathEdges.Remove(current); // 移出路径记录
                    }
                }
            }
            return false;
        }

        public static IList<INode> GetExecutionOrderEx(this NodeGroup nodes)
        {
            var executionOrder = new List<INode>();

            if (nodes == null || nodes.Count == 0)
                return executionOrder;

            // 1. 构建邻接表和入度字典
            var inDegree = new Dictionary<INode, int>();
            var adjacencyList = new Dictionary<INode, List<INode>>();

            foreach (var node in nodes)
            {
                inDegree[node] = 0;
                adjacencyList[node] = [];
            }

            foreach (var node in nodes)
            {
                foreach (var outputNode in node.OutputNodes)
                {
                    if (nodes.Contains(outputNode)) // 确保属于当前图
                    {
                        adjacencyList[node].Add(outputNode);
                        inDegree[outputNode]++;
                    }
                }
            }

            // 2. 初始化队列（入度为0的节点）
            var queue = new Queue<INode>();
            foreach (var node in nodes)
            {
                if (inDegree[node] == 0)
                    queue.Enqueue(node);
            }

            // 3. 处理队列
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                executionOrder.Add(current);

                foreach (var neighbor in adjacencyList[current])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }

            // 4. 环检测（若存在未处理的节点说明有环）
            if (executionOrder.Count != nodes.Count)
                throw new InvalidOperationException("图中存在循环依赖");

            return executionOrder;
        }
    }
}

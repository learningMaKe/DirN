using DirN.Utils.Extension;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes.Converters;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.NgManager
{
    [JsonConverter(typeof(NodeDetailConverter))]
    public class NodeDetail:BindableBase
    {
        private const string DefaultText = "请输入文字";

        public NodeGroup Nodes { get; set; } = [];

        public ObservableCollection<SWord> SWords { get; private set; } = [];

        public ObservableCollection<ICurve> BezierCurves { get; private set; } = [];

        public bool LoopDetect()
        {
            if (Nodes.HaveLoopEdgeEx(out IList<ICurve> errorCurves))
            {
                foreach (var curve in errorCurves)
                {
                    TooltipManager.Instance.Tooltip(curve, "存在循环边", TooltipType.Error);
                }
                return true;
            }
            return false;
        }

        public void Execute()
        {
            if (LoopDetect()) return;
            IList<INode> executionOrder = Nodes.GetExecutionOrderEx();
            bool executeError = false;
            for (int i = 0; i < executionOrder.Count; i++)
            {
                TooltipManager.Instance.Tooltip(executionOrder[i], $"正在执行第{i + 1}步", TooltipType.Info);
                try
                {
                    //executionOrder[i].DataFlow();
                }
                catch (Exception ex)
                {
                    executeError = true;
                    TooltipManager.Instance.Tooltip(executionOrder[i], $"执行失败：{ex.Message}", TooltipType.Error);
                }
                if (executeError) break;
            }

        }

        public void AlignNode(INode node, NodeAlignment alignment)
        {
            IList<INode> SelectedNodes = Nodes.SelectedNodes;
            if (SelectedNodes.Count == 0) return;
            if (alignment == NodeAlignment.Left)
            {
                foreach (var selectedNode in SelectedNodes)
                {
                    selectedNode.Position = new(node.Position.X, selectedNode.Position.Y);
                }
            }
            else if (alignment == NodeAlignment.Top)
            {
                foreach (var selectedNode in SelectedNodes)
                {
                    selectedNode.Position = new(selectedNode.Position.X, node.Position.Y);
                }
            }
        }

        public void AlignNodes(NodeAlignment alignment)
        {
            IList<INode> SelectedNodes = Nodes.SelectedNodes;
            if (SelectedNodes.Count == 0) return;
            if (alignment == NodeAlignment.Left)
            {
                double minX = SelectedNodes.Min(x => x.Position.X);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(minX, node.Position.Y);
                }
            }
            else if (alignment == NodeAlignment.Right)
            {
                double maxX = SelectedNodes.Max(x => x.Position.X);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(maxX, node.Position.Y);
                }
            }
            else if (alignment == NodeAlignment.Top)
            {
                double minY = SelectedNodes.Min(x => x.Position.Y);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(node.Position.X, minY);
                }
            }
            else if (alignment == NodeAlignment.Bottom)
            {
                double maxY = SelectedNodes.Max(x => x.Position.Y);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(node.Position.X, maxY);
                }
            }
        }

        public void MoveNode(Vector delta, bool onlySelected = false)
        {
            Nodes.MoveNode(delta, onlySelected);
        }

        public void SelectNode(params INode[] nodes)
        {
            ModifierKeys modifiers = Keyboard.Modifiers;
            Nodes.SelectNode(
                modifiers == ModifierKeys.None,
                modifiers != ModifierKeys.Alt,
                [.. nodes]);
        }

        public void AddSWord()
        {
            SWords.Add(new SWord()
            {
                Word = DefaultText,
                Index = SWords.Count
            });
        }

        public void RemoveSWord(SWord word)
        {
            int index = SWords.IndexOf(word);
            if (index == -1) return;
            SWords.Remove(word);
            for (int i = index; i < SWords.Count; i++)
            {
                SWords[i].Index = i;
            }

        }
    }
}

﻿using DirN.Utils.NgManager.Curves;
using DirN.Views.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.ViewModels.Node
{

    public interface IConnector:INodePasser,INodeData
    {
        public Color ConnectorColor { get; }

        public Brush ConnectorBrush { get; }

        public Connector Connector { get; }

        public bool IsInput { get; }

        public void RemoveLink(ICurve curve);

        public void AddLink(ICurve curve);

        public Action<bool>? ConnectorStateUpdated { get; set; }

    }

    public interface IInputConnector : IConnector
    {

    }

    public interface IOutputConnector : IConnector
    {

    }
}

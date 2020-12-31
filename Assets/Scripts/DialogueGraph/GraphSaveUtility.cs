﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEditor;
using System;
using UnityEngine.UIElements;

public class GraphSaveUtility 
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        if (!SaveNodes(dialogueContainer))
        {
            return;
        }
        SaveExposedProperties(dialogueContainer);

        if (!AssetDatabase.IsValidFolder("Assets/Scripts/DialogueGraph/Resources"))
        {
            AssetDatabase.CreateFolder("Assets/Scripts/DialogueGraph", "Resources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Scripts/DialogueGraph/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    private void SaveExposedProperties(DialogueContainer dialogueContainer)
    {
        dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
    }

    private bool SaveNodes(DialogueContainer dialogueContainer)
    {
        if (!Edges.Any()) //Si no hay connections
        {
            return false;
        }

        //var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        //SAVE CONNECTIONS
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        //SAVE NODES
        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });

        }
        return true;
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<DialogueContainer>(fileName);

        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");
            return;
        }

        ClearGraph();
        GenerateNodes();
        ConnectNodes();
        CreateExposedProperties();
    }

    private void CreateExposedProperties()
    {
        //clear existing properties
        _targetGraphView.ClearBlackBoardAndExposedProperties();
        //Add properties from data
        foreach (var exposedProperty in _containerCache.ExposedProperties)
        {
            _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }
    }

    private void ClearGraph()
    {
        Nodes.Find(x=>x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) 
            {
                continue;
            }
            //Remove edges connected to this node
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            //Remove the node
            _targetGraphView.RemoveElement(node);
        }

    }

    private void GenerateNodes()
    {
        foreach (var nodeData in _containerCache.DialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            for (var j =0; j< connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x=>x.GUID == targetNodeGuid);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(_containerCache.DialogueNodeData.First(x=>x.Guid == targetNodeGuid).Position, _targetGraphView.defaultNodeSize));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }
}

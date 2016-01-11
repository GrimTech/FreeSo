﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSO.Content;
using FSO.Files.Formats.IFF;
using FSO.Files.Formats.IFF.Chunks;
using System.Threading;
using FSO.IDE.EditorComponent;

namespace FSO.IDE.ResourceBrowser.ResourceEditors
{
    public partial class BHAVResourceControl : UserControl, IResourceControl
    {
        public BHAV ActiveChunk;
        public TPRP ActiveMeta;
        public GameIffResource ActiveResource;

        public GameObject ActiveObject;

        public BHAVResourceControl()
        {
            InitializeComponent();
        }

        public void SetActiveObject(GameObject obj)
        {
            ActiveObject = obj;
        }

        public void SetActiveResource(IffChunk chunk, GameIffResource res)
        {
            ActiveChunk = (BHAV)chunk;
            ActiveMeta = res.Get<TPRP>(chunk.ChunkID);
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            bool meta = (ActiveMeta != null);

            TPRPButton.Enabled = !meta;
            DescriptionBox.Enabled = meta;
            LocalRenameBtn.Enabled = meta;
            ParamRenameBtn.Enabled = meta;

            //populate param and local lists
            ParamList.Items.Clear();
            LocalList.Items.Clear();
            if (meta)
            {
                foreach (var param in ActiveMeta.ParamNames)
                    ParamList.Items.Add(param);
                foreach (var local in ActiveMeta.LocalNames)
                    LocalList.Items.Add(local);
            }
            else
            {
                for (int i = 0; i < ActiveChunk.Args; i++)
                    ParamList.Items.Add("Parameter " + i);
                for (int i = 0; i < ActiveChunk.Locals; i++)
                    LocalList.Items.Add("Local " + i);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                var bhav = ActiveChunk;
                var editor = new BHAVEditor(bhav, new EditorScope(ActiveObject, bhav), null);
                Application.Run(editor);
            }).Start();
        }
    }
}

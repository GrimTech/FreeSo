﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TSOClient.Code.UI.Model;
using System.IO;
using TSOClient.Code.UI.Framework.Parser;

namespace TSOClient.Code.UI.Framework
{
    public class UIContainer : UIElement
    {
        /// <summary>
        /// List of UIElements inside this UIContainer
        /// </summary>
        protected List<UIElement> Children { get; set; }

        public UIContainer()
        {
            Children = new List<UIElement>();
        }

        /// <summary>
        /// Adds a UIElement at the top most position in the container
        /// </summary>
        /// <param name="child"></param>
        public void Add(UIElement child)
        {
            lock (Children)
            {
                if (Children.Contains(child))
                {
                    Children.Remove(child);
                }
                Children.Add(child);
                child.Parent = this;
            }
        }

        /// <summary>
        /// Adds a UIElement at a specific depth in the container
        /// </summary>
        /// <param name="index"></param>
        /// <param name="child"></param>
        public void AddAt(int index, UIElement child)
        {
            lock (Children)
            {
                if (Children.Contains(child))
                {
                    Children.Remove(child);
                }

                Children.Insert(index, child);
                child.Parent = this;
            }
        }

        public void Remove(UIElement child)
        {
            lock (Children)
            {
                Children.Remove(child);
            }
            //child.Parent = null;
        }

        /// <summary>
        /// Get a list of the children, this is for debug only,
        /// you should not modify this array
        /// </summary>
        /// <returns></returns>
        public List<UIElement> GetChildren()
        {
            return Children;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CalculateMatrix()
        {
            base.CalculateMatrix();

            /**
             * If our matrix has changed, then our children's matrixes will have to as well
             */
            lock (Children)
            {
                foreach (var child in Children)
                {
                    child.InvalidateMatrix();
                }
            }
        }

        protected override void CalculateOpacity()
        {
            base.CalculateOpacity();

            /**
             * If our matrix has changed, then our children's matrixes will have to as well
             */
            lock (Children)
            {
                foreach (var child in Children)
                {
                    child.InvalidateOpacity();
                }
            }
        }

        /// <summary>
        /// Generates & plumbs in UI from UI script
        /// </summary>
        /// <param name="uiScript"></param>
        public UIScript RenderScript(string uiScript)
        {
            var path = Path.Combine(GlobalSettings.Default.StartupPath, @"gamedata\uiscripts\" + uiScript);
            var script = new UIScript(GameFacade.GraphicsDevice, this);
            script.Parse(path);
            return script;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="mtx"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!Visible)
            {
                return;
            }
            lock (Children)
            {
                foreach (var child in Children)
                {
                    child.Draw(batch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public override void Update(UpdateState state)
        {
            if (!Visible)
            {
                return;
            }

            base.Update(state);
            lock (Children)
            {
                foreach (var child in Children)
                {
                    child.Update(state);
                }
            }
        }



        public void SendToBack(params UIElement[] elements)
        {
            lock (Children)
            {
                for (int i = elements.Length - 1; i >= 0; i--)
                {
                    Children.Remove(elements[i]);
                    Children.Insert(0, elements[i]);
                }
            }
        }

    }
}

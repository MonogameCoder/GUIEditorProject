using _GUIProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;



namespace _GUIProject.Events
{
    public class MouseClickArgs : EventArgs
    {
        public UIObject sender;
        public Action action;
        public string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public UIObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        public Action Action
        {
            get { return action; }
            set { action = value; }
        }


        public MouseClickArgs()
        {
        }
        public MouseClickArgs(UIObject sender, Action action)
        {
            Sender = sender;
            Action = action;

        }
        public MouseClickArgs(UIObject sender, Action action, object target)
        {
            Sender = sender;
            Action = action;
            Target = target.GetType().Name;

        }  
    }

    public class MouseOverArgs : EventArgs
    {
        public UIObject sender;
        public Action action;
        public string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public UIObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        public Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public MouseOverArgs()
        {
        }
        public MouseOverArgs(UIObject sender, Action action)
        {
            Sender = sender;
            Action = action;

        }
        public MouseOverArgs(UIObject sender, Action action, object target)
        {
            Sender = sender;
            Action = action;
            Target = target.GetType().Name;

        }
    }
    public class MouseOutArgs : EventArgs
    {
        public UIObject sender;
        public Action action;
        public string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public UIObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        public Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public MouseOutArgs()
        {
        }
        public MouseOutArgs(UIObject sender, Action action)
        {
            Sender = sender;
            Action = action;

        }
        public MouseOutArgs(UIObject sender, Action action, object target)
        {
            Sender = sender;
            Action = action;
            Target = target.GetType().Name;

        }   
    }
    public class MouseMoveArgs : EventArgs
    {
        public UIObject sender;
        public Action action;
        public string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public UIObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        public Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public MouseMoveArgs()
        {
        }
        public MouseMoveArgs(UIObject sender, Action action)
        {
            Sender = sender;
            Action = action;

        }
        public MouseMoveArgs(UIObject sender, Action action, object target)
        {
            Sender = sender;
            Action = action;
            Target = target.GetType().Name;

        }       
    }

    public class MouseEvents
    {
        public UIObject Owner { get; private set; }
        public event EventHandler<MouseClickArgs> onMouseClick;
        public event EventHandler<MouseOverArgs> onMouseOver;
        public event EventHandler<MouseOutArgs> onMouseOut;
        public event EventHandler<MouseMoveArgs> onMouseMove;

        public bool IsOnClickNull 
        {
            get { return onMouseClick == null; }
        }
        public bool IsOnOverNull
        {
            get { return onMouseOver == null; }
        }
        public bool IsOnOutNull
        {
            get { return onMouseOut == null; }
        }
        public bool IsOnMoveNull
        {
            get { return onMouseMove == null; }
        }

        public MouseEvents(UIObject owner)
        {
            Owner = owner;
        }
        public void Click()
        {
            if(onMouseClick != null)
            {
                onMouseClick.Invoke(Owner, onMouseClick.Target as MouseClickArgs);
            }            
        }
        public void Over()
        {
            if(onMouseOver != null)
            {
                onMouseOver.Invoke(Owner, onMouseOver.Target as MouseOverArgs);
            }
           
        }
        public void Out()
        {
            if(onMouseOut != null)
            {
                onMouseOut.Invoke(Owner, onMouseOut.Target as MouseOutArgs);
            }            
        }
        public void Move()
        {
            if(onMouseMove != null)
            {
                onMouseMove.Invoke(Owner, onMouseMove.Target as MouseMoveArgs);
            }
            
        }
    }
}

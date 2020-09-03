using Microsoft.Xna.Framework;

using System.Collections.Generic;
using static _GUIProject.UI.UIObject;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace _GUIProject.UI
{
    public static class UIFactory<T,U> where T : UIObject where U: IContainer
    {
       public static IContainer CreateContainer(IContainer oldContainer, IContainer newContainer, SpriteBatch batch)
        {
            Slot<UIObject>[] backup = new Slot<UIObject>[oldContainer.Slots.Count];
            oldContainer.Slots.CopyTo(backup);
            oldContainer.Slots.Clear();          

            newContainer.Position = new Point(568, 100);
            newContainer.Initialize();
            newContainer.Setup();
            newContainer.AddSpriteRenderer(batch);
            newContainer.AddStringRenderer(batch);

            newContainer.Position = new Point(568, 100);         

            for (int i = 0; i < backup.Length; i++)
            {
                backup[i].Item.ResetSize();
                newContainer.AddItem(backup[i].Position, backup[i].Item, backup[i].Item.Priority);
            }
            newContainer.UpdateLayout();
            return newContainer;

        }
        public static T CreateObject(Point location, T obj, U container) 
        {          
            var newObj = Activator.CreateInstance(obj.GetType()) as T;          
            newObj.Initialize();
            newObj.Setup();
            newObj.MoveState = MoveOption.DYNAMIC;
            newObj.TextColor.Color = Color.Black;
            newObj.Name = obj.GetType().Name + container.Slots.Where((s) => s.Item.GetType() == obj.GetType()).Count();
           
            newObj.Editable = true;         
            newObj.Active = true;                       
       
            container.AddItem(location - container.Position, newObj, DrawPriority.LOWEST);         
            return (T)newObj;
        }       
      
    }
}

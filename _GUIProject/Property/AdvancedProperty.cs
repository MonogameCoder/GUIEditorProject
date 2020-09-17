using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static _GUIProject.UI.Sprite;
using static _GUIProject.UI.PropertyPanel;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    class AdvancedProperty
    {
        TextBoxConfirmAction _comboAddConfirm;
        Button _addButton;
        Button _remButton;

        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }

        public AdvancedProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            Owner = parent.Owner;
            Parent = parent;
        }
        public void AddProperties(PropertyOwner owner)
        {
            _comboAddConfirm = new TextBoxConfirmAction();

            _addButton = new Button("PropertyPanelComboAddBTNTX", OverlayOption.NORMAL, DrawPriority.NORMAL);
            _remButton = new Button("PropertyPanelComboDelBTNTX", OverlayOption.NORMAL, DrawPriority.NORMAL);


            Properties.Add(_comboAddConfirm, new Point(55, 555));

            _comboAddConfirm.Initialize();
            _addButton.Initialize();
            _addButton.Initialize();

            _comboAddConfirm.AddButton(_addButton, 4);
            _comboAddConfirm.AddButton(_remButton, 4);
            _comboAddConfirm.Show();

            AddEvents();


        }
        public void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (Owner is ComboBox)
            {

                _addButton.MouseEvent.onMouseClick += (sender, args) =>
                {

                    if (!string.IsNullOrEmpty(_comboAddConfirm.Text))
                    {
                        Owner.AddSpriteRenderer(MainWindow._mainBatch);
                        Owner.AddStringRenderer(MainWindow._mainBatch);
                        //MessageDialog msg = new MessageDialog("This item already exists.");
                        if (!(Owner as ComboBox).Contains(_comboAddConfirm.Text))
                        {
                            (Owner as ComboBox).AddNewItem(_comboAddConfirm.Text, () => { });
                            _comboAddConfirm.Clear();

                        }
                        else
                        {
                            MessageBox.Show("Item " + _comboAddConfirm.Text + " already exists: ", "Message",  MessageBoxButtons.OK);
                        }

                    }
                };


                _remButton.MouseEvent.onMouseClick += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(_comboAddConfirm.Text))
                    {
                        if ((Owner as ComboBox).Contains(_comboAddConfirm.Text))
                        {
                            (Owner as ComboBox).RemoveItem((Owner as ComboBox)[_comboAddConfirm.Text]);

                            _comboAddConfirm.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Item does not exist. ", "Message", MessageBoxButtons.OK);
                        }

                    }
                };               
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void Update(GameTime gameTime)
        {
            // Nothing
        }
    }
}

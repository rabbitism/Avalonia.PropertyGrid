﻿using Avalonia.Controls;
using Avalonia.PropertyGrid.Model.ComponentModel;
using Avalonia.PropertyGrid.Model.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.PropertyGrid.Controls.Factories.Builtins
{
    internal class BooleanCellEditFactory : AbstractCellEditFactory
    {
        public override int ImportPriority => base.ImportPriority - 100000;

        public override Control HandleNewProperty(PropertyCellContext context)
        {
            var propertyDescriptor = context.Property;

            if (propertyDescriptor.PropertyType != typeof(bool) && propertyDescriptor.PropertyType != typeof(bool?))
            {
                return null;
            }

            var control = new CheckBox();

            control.IsThreeState = propertyDescriptor.PropertyType == typeof(bool?) || propertyDescriptor is MultiObjectPropertyDescriptor;

            control.IsCheckedChanged += (s, e) =>
            {
                SetAndRaise(context, control, control.IsChecked);
            };

            return control;
        }

        public override bool HandlePropertyChanged(PropertyCellContext context)
        {
            var propertyDescriptor = context.Property;
            var target = context.Target;
            var control = context.CellEdit;

            if (propertyDescriptor.PropertyType != typeof(bool) && propertyDescriptor.PropertyType != typeof(bool?))
            {
                return false;
            }

            ValidateProperty(control, propertyDescriptor, target);

            if (control is CheckBox ts)
            {
                if (ts.IsThreeState)
                {
                    var obj = propertyDescriptor.GetValue(target);

                    if( obj != null)
                    {
                        ts.IsChecked = (bool)obj;
                    }
                    else
                    {
                        ts.IsChecked = null;
                    }
                }
                else
                {
                    ts.IsChecked = (bool)propertyDescriptor.GetValue(target);
                }

                return true;
            }

            return false;
        }
    }
}
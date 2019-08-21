﻿using Emblazon;
using System.Diagnostics;
using Xamarin.Forms;

namespace Blaxamarin.Framework
{
    /// <summary>
    /// Represents a "shadow" item that Blazor uses to map changes into the live Xamarin.Forms Element tree.
    /// </summary>
    public class BlelementAdapter : EmblazonAdapter<Element>
    {
        public BlelementAdapter()
        {
        }

        protected override void RemoveChildControl(EmblazonAdapter<Element> child)
        {
            var targetAsLayout = TargetControl as Layout<View>;
            var childTargetAsView = child.TargetControl as View;

            targetAsLayout.Children.Remove(childTargetAsView);
        }

        protected override EmblazonAdapter<Element> CreateAdapter()
        {
            return new BlelementAdapter();
        }

        protected override bool IsChildControlParented(Element nativeChild)
        {
            return nativeChild.Parent != null;
        }

        protected override void AddChildControl(Element parentControl, int siblingIndex, Element childControl)
        {
            // TODO: What is the set of types that support child elements? Do they all need to be special-cased here? (Maybe...)

            if (parentControl is Layout<View> parentAsLayout)
            {
                var childAsView = childControl as View;

                if (siblingIndex <= parentAsLayout.Children.Count)
                {
                    // WinForms ControlCollection doesn't support Insert(), so add the new child at the end,
                    // and then re-order the collection to move the control to the correct index.
                    parentAsLayout.Children.Insert(siblingIndex, childAsView);
                }
                else
                {
                    Debug.WriteLine($"WARNING: {nameof(AddChildControl)} called with {nameof(siblingIndex)}={siblingIndex}, but parentAsLayout.Children.Count={parentAsLayout.Children.Count}");
                    parentAsLayout.Children.Add(childAsView);
                }
            }
            else if (parentControl is Application parentAsApp)
            {
                if (parentAsApp.MainPage != null)
                {
                    Debug.Fail($"Application already has MainPage set; cannot set {parentAsApp.GetType().FullName}'s MainPage to {childControl.GetType().FullName}");
                }
                else
                {
                    if (childControl is Page childControlAsPage)
                    {
                        parentAsApp.MainPage = childControlAsPage;
                    }
                    else
                    {
                        // TODO: Do we need a BlelementAdapter representing the dummy ContentPage? Or should the Razor page be a ContentPage somehow?
                        var dummyView = new ContentPage
                        {
                            Content = childControl as View
                        };
                        parentAsApp.MainPage = dummyView;
                        //Debug.Fail($"Application MainPage must be a Page; cannot set {parentAsApp.GetType().FullName}'s MainPage to {childControl.GetType().FullName}");
                    }
                }
            }
            else
            {
                Debug.Fail($"Don't know how to handle parent control type {parentControl.GetType().FullName} in order to add child {childControl.GetType().FullName}");
            }
        }
    }
}
